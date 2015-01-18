using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Tools;
using Newtonsoft.Json;
#if WINDOWS_APP
using Windows.UI.ApplicationSettings;
#endif
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Autofac;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database.Context;
using AwfulForumsReader.Core;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Core.Tools;
using AwfulForumsReader.Pages;
using HockeyApp;

namespace AwfulForumsReader
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif
        public static IContainer Container;
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        public static Frame RootFrame;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
#if WINDOWS_APP
            RequestedTheme = ApplicationTheme.Light;
#endif
#if WINDOWS_PHONE_APP
            RequestedTheme = ApplicationTheme.Dark;
                        var type = typeof(Microsoft.Data.Entity.Relational.RelationalDatabase).GetTypeInfo()
                .Assembly.GetType("Microsoft.Data.Entity.Relational.Strings");
            WindowsRuntimeResourceManager.InjectIntoResxGeneratedApplicationResourcesClass(type);
#endif
            //HockeyClient.Current.Configure("4e570f666ab005f8526946c71f5f4824");
            try
            {
                using (var db = new MainForumListContext())
                {
                    // Migrations are not yet enabled in EF7, so use an
                    // API to create the database if it doesn't exist
                    db.Database.EnsureCreated();
                }
            }
            catch (Exception)
            {

            
            }

            try
            {
                using (var db = new NotifyThreadListContext())
                {
                    // Migrations are not yet enabled in EF7, so use an
                    // API to create the database if it doesn't exist
                    db.Database.EnsureCreated();
                }
            }
            catch (Exception)
            {

             
            }

            try
            {
                using (var db = new LinkedThreadListContext())
                {
                    // Migrations are not yet enabled in EF7, so use an
                    // API to create the database if it doesn't exist
                    db.Database.EnsureCreated();
                }
            }
            catch (Exception)
            {

                
            }

            Container = AutoFacConfiguration.Configure();
        }
#if WINDOWS_PHONE_APP
        protected override void OnActivated(IActivatedEventArgs args)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                base.OnActivated(args);
                return;
            }

            // Return to Reply Page with image.

            var settingsPage = rootFrame.Content as SettingsPage;
            if (settingsPage != null && args is FileOpenPickerContinuationEventArgs)
            {
                settingsPage.ContinueFileOpenPicker(args as FileOpenPickerContinuationEventArgs);
            }

            var replyPage = rootFrame.Content as NewThreadReplyPage;
            if (replyPage != null && args is FileOpenPickerContinuationEventArgs)
            {
                replyPage.ContinueFileOpenPicker(args as FileOpenPickerContinuationEventArgs);
            }

            var editPage = rootFrame.Content as EditPostPage;
            if (editPage != null && args is FileOpenPickerContinuationEventArgs)
            {
                editPage.ContinueFileOpenPicker(args as FileOpenPickerContinuationEventArgs);
            }

            var newThreadPage = rootFrame.Content as NewThreadPage;
            if (newThreadPage != null && args is FileOpenPickerContinuationEventArgs)
            {
                newThreadPage.ContinueFileOpenPicker(args as FileOpenPickerContinuationEventArgs);
            }

            var newPmPage = rootFrame.Content as NewPrivateMessagePage;
            if (newPmPage != null && args is FileOpenPickerContinuationEventArgs)
            {
                newPmPage.ContinueFileOpenPicker(args as FileOpenPickerContinuationEventArgs);
            }


            // Voice command!111!!!11!

            if (args.Kind == ActivationKind.VoiceCommand)
            {
                VoiceCommandActivatedEventArgs vcArgs = (VoiceCommandActivatedEventArgs)args;
                //rootFrame.Navigate(typeof(VoiceHandlePage), vcArgs);
            }
            base.OnActivated(args);

        }
#endif

#if WINDOWS_APP 
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "App Settings", "App Settings", (handler) => ShowAppSettingsFlyout()));
        }

        public static void ShowAppSettingsFlyout()
        {
           ForumSettingsFlyout flyout = new ForumSettingsFlyout();
            flyout.Show();
        }
#endif

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
#if WINDOWS_APP
            SettingsPane.GetForCurrentView().CommandsRequested += SettingCharmManager_CommandsRequested;
#endif
            RootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                RootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                RootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = RootFrame;
            }

            string launchString = e.Arguments;
            if (!string.IsNullOrEmpty(launchString))
            {
                var arguments = JsonConvert.DeserializeObject<ToastNotificationArgs>(launchString);
                if (arguments != null && arguments.threadId > 0)
                {
                    var bookmarkCommand = new NavigateToBookmarksCommand();
                    bookmarkCommand.Execute(Convert.ToInt64(arguments.threadId));
                }
            }

            if (RootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (RootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in RootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                RootFrame.ContentTransitions = null;
                RootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif
                if (_localSettings.Values.ContainsKey(Constants.BackgroundWallpaper))
                {
                    var hasWallpaper = (bool) _localSettings.Values[Constants.BackgroundWallpaper];
                    if (hasWallpaper)
                    {
                        var storageFolder = ApplicationData.Current.LocalFolder;
                        var result = await StorageFolderExtensions.FileExistsAsync(storageFolder, Constants.WallpaperFilename);
                        StorageFile file;
                        if (result)
                        {
                            file = await storageFolder.GetFileAsync(Constants.WallpaperFilename);
                            
                        }
                        else
                        {
                            string CountriesFile = @"Assets\Login\Dontrel-Awful2.png";
                            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                            file = await InstallationFolder.GetFileAsync(CountriesFile);
                        }

                        using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            var bitmapImage = new BitmapImage();
                            var brush = new ImageBrush();
                            await bitmapImage.SetSourceAsync(fileStream);
                            brush.ImageSource = bitmapImage;
                            brush.Stretch = Stretch.UniformToFill;
                            RootFrame.Background = brush;
                        }
                    }
                    else
                    {
#if WINDOWS_APP
                        App.RootFrame.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
#endif
#if WINDOWS_PHONE_APP
                        App.RootFrame.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
#endif
                    }
                }
                else
                {
#if WINDOWS_APP
                        App.RootFrame.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
#endif
#if WINDOWS_PHONE_APP
                    App.RootFrame.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
#endif
                }

                var localStorageManager = new LocalStorageManager();
                CookieContainer cookieTest = await localStorageManager.LoadCookie(Constants.CookieFile);
                if (cookieTest.Count <= 0)
                {
                    if (!RootFrame.Navigate(typeof(LoginPage)))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
                else
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    if (_localSettings.Values.ContainsKey(Constants.BookmarkStartup) &&
                        (bool)_localSettings.Values[Constants.BookmarkStartup])
                    {
                        var command = new NavigateToBookmarksCommand();
                        command.Execute(null);
                    }
                    else
                    {
                        if (!RootFrame.Navigate(typeof(MainForumsPage), e.Arguments))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
            //await HockeyClient.Current.SendCrashesAsync();
#if WINDOWS_PHONE_APP
            //await HockeyClient.Current.CheckForAppUpdateAsync();
#endif
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var RootFrame = sender as Frame;
            RootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            RootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

#if WINDOWS_APP
        private void SettingCharmManager_CommandsRequested(SettingsPane sender,
    SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("privacypolicy", "Privacy Policy",
                OpenPrivacyPolicy));
        }

        private async void OpenPrivacyPolicy(IUICommand command)
        {
            var uri = new Uri("https://sites.google.com/site/awfulforumsreader/");
            await Launcher.LaunchUriAsync(uri);
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}