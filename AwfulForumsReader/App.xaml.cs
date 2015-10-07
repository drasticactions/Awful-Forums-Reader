using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Autofac;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Commands.Navigation;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;
using Newtonsoft.Json;
using ThemeManagerRt;

namespace AwfulForumsReader
{
    /// <summary>
    /// 既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    sealed partial class App : Application
    {
        public static IContainer Container;
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        public static Frame RootFrame;
        public static ApplicationTheme SelectedTheme { get; set; }

        async protected override void OnActivated(IActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                CreateRootFrame();

                if (!RootFrame.Navigate(typeof(MainPage)))
                {
                    throw new Exception("Failed to create initial page");
                }

                Window.Current.Activate();
            }

            //Find out if this is activated from a toast;
            if (args.Kind == ActivationKind.ToastNotification)
            {
                //Get the pre-defined arguments and user inputs from the eventargs;
                var toastArgs = args as ToastNotificationActivatedEventArgs;
                if (toastArgs == null)
                    return;
                var arguments = JsonConvert.DeserializeObject<ToastNotificationArgs>(toastArgs.Argument);
                if (arguments != null && arguments.threadId > 0)
                {
                    var bookmarkCommand = new NavigateToBookmarksCommand();
                    bookmarkCommand.Execute(arguments.threadId);
                    return;
                }

                var forumEntity = JsonConvert.DeserializeObject<ForumEntity>(toastArgs.Argument);
                if (forumEntity != null)
                {
                    var navigateCommand = new NavigateToThreadListPageCommandViaTile();
                    navigateCommand.Execute(forumEntity);
                }
            }

            // Cortana
            if (args.Kind == ActivationKind.VoiceCommand)
            {
                var commandArgs = args as VoiceCommandActivatedEventArgs;
                HandleVoiceRequest(commandArgs);
            }

            //...
        }

        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }


        public void HandleVoiceRequest(VoiceCommandActivatedEventArgs commandArgs)
        {
            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = commandArgs.Result;

            // Get the name of the voice command and the text spoken. See AdventureWorksCommands.xml for
            // the <Command> tags this can be filled with.
            string voiceCommandName = speechRecognitionResult.RulePath[0];
            string textSpoken = speechRecognitionResult.Text;

            // The commandMode is either "voice" or "text", and it indictes how the voice command
            // was entered by the user.
            // Apps should respect "text" mode by providing feedback in silent form.
            string commandMode = this.SemanticInterpretation("commandMode", speechRecognitionResult);

            switch (voiceCommandName)
            {
                case "openBookmarks":
                    var bookmarkCommand = new NavigateToBookmarksCommand();
                    bookmarkCommand.Execute(null);
                    break;
                case "openPrivateMessages":
                    var pmCommand = new NavigateToPrivateMessageListPageCommand();
                    pmCommand.Execute(null);
                    break;
                case "lowtaxIsAJerk":
                    var lowtaxCommand = new NavigateToNewPrivateMessagePageLowtaxCommand();
                    lowtaxCommand.Execute(null);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        ///最初の行であるため、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync();
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            if (_localSettings.Values.ContainsKey(Constants.ThemeDefault))
            {
                var themeType = (ThemeTypes)_localSettings.Values[Constants.ThemeDefault];
                switch (themeType)
                {
                    case ThemeTypes.Light:
                        RequestedTheme = ApplicationTheme.Light;
                        break;
                    case ThemeTypes.Dark:
                        RequestedTheme = ApplicationTheme.Dark;
                        break;
                    case ThemeTypes.YOSPOS:
                        RequestedTheme = ApplicationTheme.Dark;
                        break;
                }
            }
            SelectedTheme = RequestedTheme;
            try
            {
                DataSource ds = new DataSource();
                SaclopediaDataSource sds = new SaclopediaDataSource();
                BookmarkDataSource bds = new BookmarkDataSource();
                ds.InitDatabase();
                ds.CreateDatabase();
                bds.InitDatabase();
                bds.CreateDatabase();
                sds.InitDatabase();
                sds.CreateDatabase();
            }
            catch
            {

            }
            Container = AutoFacConfiguration.Configure();
        }

        private void BackPressed(object sender, BackRequestedEventArgs e)
        {
            if (RootFrame == null)
            {
                return;
            }

            if (!RootFrame.CanGoBack) return;
            RootFrame.GoBack();
            e.Handled = true;
        }

        private void CreateRootFrame()
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            RootFrame = new Frame();
            // Set the default language
            RootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

            RootFrame.NavigationFailed += OnNavigationFailed;

            // Place the frame in the current Window
            Window.Current.Content = RootFrame;
        }

        /// <summary>
        /// アプリケーションがエンド ユーザーによって正常に起動されたときに呼び出されます。他のエントリ ポイントは、
        /// アプリケーションが特定のファイルを開くために起動されたときなどに使用されます。
        /// </summary>
        /// <param name="e">起動の要求とプロセスの詳細を表示します。</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            if (_localSettings.Values.ContainsKey(Constants.ThemeDefault))
            {
                var themeType = (ThemeTypes)_localSettings.Values[Constants.ThemeDefault];
                switch (themeType)
                {
                    case ThemeTypes.YOSPOS:
                        ThemeManager.SetThemeManager(ElementTheme.Dark);
                        ThemeManager.ChangeTheme(new Uri("ms-appx:///Themes/Yospos.xaml"));
                        break;
                }
            }
            SystemNavigationManager.GetForCurrentView().BackRequested += BackPressed;
            RootFrame = Window.Current.Content as Frame;
            if (_localSettings.Values.ContainsKey(Constants.NavigateBack) &&
                (bool) _localSettings.Values[Constants.NavigateBack])
            {
                if (RootFrame != null)
                {
                    return;
                }
            }
            var isIoT = ApiInformation.IsTypePresent("Windows.Devices.Gpio.GpioController");

            if (!isIoT)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            }

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (RootFrame == null)
            {
                CreateRootFrame();
            }
            if (!isIoT)
            {
                BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.ToastBackgroundTaskName);
                var task2 = await
                    BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.ToastBackgroundTaskEntryPoint,
                        BackgroundTaskUtils.ToastBackgroundTaskName, new ToastNotificationActionTrigger(),
                        null);

                if (_localSettings.Values.ContainsKey(Constants.BackgroundEnable) && (bool)_localSettings.Values[Constants.BackgroundEnable])
                {
                    BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
                    var task = await
                        BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.BackgroundTaskEntryPoint,
                            BackgroundTaskUtils.BackgroundTaskName,
                            new TimeTrigger(15, false),
                            null);
                }
            }

            var localStorageManager = new LocalStorageManager();
            CookieContainer cookieContainer = await localStorageManager.LoadCookie("SACookies2.txt");
            bool cookieTest = true;
            foreach (Cookie cookie in cookieContainer.GetCookies(new Uri(Constants.CookieDomainUrl)))
            {
                if (cookie.Expired)
                {
                    cookieTest = false;
                }
            }

            if (cookieContainer.Count <= 0)
            {
                if (!RootFrame.Navigate(typeof(LoginPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            else
            {
                if (!cookieTest)
                {
                    if (!RootFrame.Navigate(typeof(LoginPage)))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }

                if (!RootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();

            try
            {
                // Install the main VCD. Since there's no simple way to test that the VCD has been imported, or that it's your most recent
                // version, it's not unreasonable to do this upon app load.
                StorageFile vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"SomethingAwfulCommands.xml");

                await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Installing Voice Commands Failed: " + ex.ToString());
            }
        }

        /// <summary>
        /// 特定のページへの移動が失敗したときに呼び出されます
        /// </summary>
        /// <param name="sender">移動に失敗したフレーム</param>
        /// <param name="e">ナビゲーション エラーの詳細</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// アプリケーションの実行が中断されたときに呼び出されます。
        /// アプリケーションが終了されるか、メモリの内容がそのままで再開されるかに
        /// かかわらず、アプリケーションの状態が保存されます。
        /// </summary>
        /// <param name="sender">中断要求の送信元。</param>
        /// <param name="e">中断要求の詳細。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: アプリケーションの状態を保存してバックグラウンドの動作があれば停止します
            deferral.Complete();
        }
    }
}
