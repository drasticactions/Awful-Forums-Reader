using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources.Core;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Database;

// ReSharper disable once CheckNamespace
namespace AwfulForumsReader.VoiceCommands
{
    public sealed class AwfulVoiceCommandService : IBackgroundTask
    {
        /// <summary>
        /// the service connection is maintained for the lifetime of a cortana session, once a voice command
        /// has been triggered via Cortana.
        /// </summary>
        VoiceCommandServiceConnection voiceServiceConnection;

        /// <summary>
        /// Lifetime of the background service is controlled via the BackgroundTaskDeferral object, including
        /// registering for cancellation events, signalling end of execution, etc. Cortana may terminate the 
        /// background service task if it loses focus, or the background task takes too long to provide.
        /// 
        /// Background tasks can run for a maximum of 30 seconds.
        /// </summary>
        BackgroundTaskDeferral serviceDeferral;

        /// <summary>
        /// ResourceMap containing localized strings for display in Cortana.
        /// </summary>
        ResourceMap cortanaResourceMap;

        /// <summary>
        /// The context for localized strings.
        /// </summary>
        ResourceContext cortanaContext;

        /// <summary>
        /// Get globalization-aware date formats.
        /// </summary>
        DateTimeFormatInfo dateFormatInfo;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();

            // Register to receive an event if Cortana dismisses the background task. This will
            // occur if the task takes too long to respond, or if Cortana's UI is dismissed.
            // Any pending operations should be cancelled or waited on to clean up where possible.
            taskInstance.Canceled += OnTaskCanceled;

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            // Load localized resources for strings sent to Cortana to be displayed to the user.
            cortanaResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");

            // Select the system language, which is what Cortana should be running as.
            cortanaContext = ResourceContext.GetForViewIndependentUse();

            // Get the currently used system date format
            dateFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;

            // This should match the uap:AppService and VoiceCommandService references from the 
            // package manifest and VCD files, respectively. Make sure we've been launched by
            // a Cortana Voice Command.
            if (triggerDetails != null && triggerDetails.Name == "AwfulVoiceCommandService")
            {
                try
                {
                    voiceServiceConnection =
                        VoiceCommandServiceConnection.FromAppServiceTriggerDetails(
                            triggerDetails);

                    voiceServiceConnection.VoiceCommandCompleted += OnVoiceCommandCompleted;

                    VoiceCommand voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();

                    // Depending on the operation (defined in AdventureWorks:AdventureWorksCommands.xml)
                    // perform the appropriate command.
                    switch (voiceCommand.CommandName)
                    {
                        case "didMyThreadsUpdate":
                            await CheckForBookmarksForUpdates();
                            break;
                        default:
                            // As with app activation VCDs, we need to handle the possibility that
                            // an app update may remove a voice command that is still registered.
                            // This can happen if the user hasn't run an app since an update.
                            LaunchAppInForeground();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Handling Voice Command failed " + ex.ToString());
                }
            }
        }

        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        private readonly MainForumsDatabase _bookmarkManager = new MainForumsDatabase();
        private async Task CheckForBookmarksForUpdates()
        {
            // Begin loading data to search for the target store. If this operation is going to take a long time,
            // for instance, requiring a response from a remote web service, consider inserting a progress screen 
            // here, in order to prevent Cortana from timing out. 
            string progressScreenString = "Refreshing your bookmarks...";
            await ShowProgressScreen(progressScreenString);
            var test = await _bookmarkManager.RefreshBookmarkedThreads();
            _localSettings.Values["RefreshBookmarks"] = DateTime.UtcNow.ToString();
            var threadsWithReplies = test.Where(node => node.RepliesSinceLastOpened > 0);

            var userPrompt = new VoiceCommandUserMessage();

            VoiceCommandResponse response;
            if (!threadsWithReplies.Any())
            {
                var userMessage = new VoiceCommandUserMessage();
                userMessage.DisplayMessage = userMessage.SpokenMessage = "Ehhh, I'm not seeing anything new here! Check again later.";
                response = VoiceCommandResponse.CreateResponse(userMessage);
                await voiceServiceConnection.ReportSuccessAsync(response);
            }
            else if (threadsWithReplies.Count() == 1)
            {
                // Prompt the user for confirmation that we've selected the correct trip to cancel.
                string threadBookmarkPrompt = string.Format("You have one thread with unread replies: {0}",
                    threadsWithReplies.First().Name);
                userPrompt.DisplayMessage = userPrompt.SpokenMessage = threadBookmarkPrompt;
                var userReprompt = new VoiceCommandUserMessage();
                string threadBookmarkPromptConfirm = "Would you like to open up this thread?";
                userReprompt.DisplayMessage = userReprompt.SpokenMessage = threadBookmarkPromptConfirm;

                response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, userReprompt);

                var voiceCommandConfirmation = await voiceServiceConnection.RequestConfirmationAsync(response);

                // If RequestConfirmationAsync returns null, Cortana's UI has likely been dismissed.
                if (voiceCommandConfirmation != null)
                {
                    if (voiceCommandConfirmation.Confirmed)
                    {
                        LaunchAppInForegroundWithThread(threadsWithReplies.First());
                    }
                    else
                    {
                        // Confirm no action for the user.
                        var userMessage = new VoiceCommandUserMessage();
                        string dontShowAnythingMessage = "Okay, I'll just keep doing the needful I guess.";
                        userMessage.DisplayMessage = userMessage.SpokenMessage = dontShowAnythingMessage;

                        response = VoiceCommandResponse.CreateResponse(userMessage);
                        await voiceServiceConnection.ReportSuccessAsync(response);
                    }
                }
            }
            else
            {
                string threadBookmarkPromptConfirm = "Would you like to open up this thread?";

                string multipleThreads =
    string.Format(
        "You have {0} threads with unread replies. The most recent is \"{1}\" with {2} unread replies. {3}",
        threadsWithReplies.Count(), threadsWithReplies.First().Name, threadsWithReplies.First().RepliesSinceLastOpened, threadBookmarkPromptConfirm);

                userPrompt.DisplayMessage = userPrompt.SpokenMessage = multipleThreads;
                var userReprompt = new VoiceCommandUserMessage();
                userReprompt.DisplayMessage = userReprompt.SpokenMessage = threadBookmarkPromptConfirm;

                response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, userReprompt);

                var voiceCommandConfirmation = await voiceServiceConnection.RequestConfirmationAsync(response);
                if (voiceCommandConfirmation != null)
                {
                    if (voiceCommandConfirmation.Confirmed)
                    {
                        LaunchAppInForegroundWithThread(threadsWithReplies.First());
                    }
                    else
                    {
                        // Confirm no action for the user.
                        var userMessage = new VoiceCommandUserMessage();
                        string dontShowAnythingMessage = "I cry alone at night. I mean that's cool, check back later!";
                        userMessage.DisplayMessage = userMessage.SpokenMessage = dontShowAnythingMessage;
                        response = VoiceCommandResponse.CreateResponse(userMessage);
                        await voiceServiceConnection.ReportSuccessAsync(response);
                    }
                }
            }
        }

        /// <summary>
        /// Show a progress screen. These should be posted at least every 5 seconds for a 
        /// long-running operation, such as accessing network resources over a mobile 
        /// carrier network.
        /// </summary>
        /// <param name="message">The message to display, relating to the task being performed.</param>
        /// <returns></returns>
        private async Task ShowProgressScreen(string message)
        {
            var userProgressMessage = new VoiceCommandUserMessage();
            userProgressMessage.DisplayMessage = userProgressMessage.SpokenMessage = message;

            VoiceCommandResponse response = VoiceCommandResponse.CreateResponse(userProgressMessage);
            await voiceServiceConnection.ReportProgressAsync(response);
        }

        private async void LaunchAppInForegroundWithThread(ForumThreadEntity thread)
        {
            var userMessage = new VoiceCommandUserMessage();
            userMessage.SpokenMessage = "Opening the thread...";

            var response = VoiceCommandResponse.CreateResponse(userMessage);
            string test = "{" + string.Format("type:'toast', 'threadId':{0}", thread.ThreadId) + "}";
            response.AppLaunchArgument = test;

            await voiceServiceConnection.RequestAppLaunchAsync(response);
        }

        /// <summary>
        /// Provide a simple response that launches the app. Expected to be used in the
        /// case where the voice command could not be recognized (eg, a VCD/code mismatch.)
        /// </summary>
        private async void LaunchAppInForeground()
        {
            var userMessage = new VoiceCommandUserMessage();
            userMessage.SpokenMessage = cortanaResourceMap.GetValue("LaunchingAdventureWorks", cortanaContext).ValueAsString;

            var response = VoiceCommandResponse.CreateResponse(userMessage);

            response.AppLaunchArgument = "";

            await voiceServiceConnection.RequestAppLaunchAsync(response);
        }

        /// <summary>
        /// Handle the completion of the voice command. Your app may be cancelled
        /// for a variety of reasons, such as user cancellation or not providing 
        /// progress to Cortana in a timely fashion. Clean up any pending long-running
        /// operations (eg, network requests).
        /// </summary>
        /// <param name="sender">The voice connection associated with the command.</param>
        /// <param name="args">Contains an Enumeration indicating why the command was terminated.</param>
        private void OnVoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
        {
            this.serviceDeferral?.Complete();
        }

        /// <summary>
        /// When the background task is cancelled, clean up/cancel any ongoing long-running operations.
        /// This cancellation notice may not be due to Cortana directly. The voice command connection will
        /// typically already be destroyed by this point and should not be expected to be active.
        /// </summary>
        /// <param name="sender">This background task instance</param>
        /// <param name="reason">Contains an enumeration with the reason for task cancellation</param>
        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            System.Diagnostics.Debug.WriteLine("Task cancelled, clean up");
            //Complete the service deferral
            this.serviceDeferral?.Complete();
        }
    }
}
