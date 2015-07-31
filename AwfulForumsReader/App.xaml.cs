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
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Autofac;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;
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
                        RequestedTheme = ApplicationTheme.Dark;
                        break;
                    case ThemeTypes.Dark:
                        RequestedTheme = ApplicationTheme.Dark;
                        break;
                    case ThemeTypes.YOSPOS:
                        RequestedTheme = ApplicationTheme.Dark;
                        break;
                }
            }

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

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                RootFrame = new Frame();
                // Set the default language
                RootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                RootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = RootFrame;
            }

            if (_localSettings.Values.ContainsKey(Constants.BookmarkBackground) && (bool)_localSettings.Values[Constants.BookmarkBackground])
            {
                BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
                var task = await
                    BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.BackgroundTaskEntryPoint,
                        BackgroundTaskUtils.BackgroundTaskName,
                        new TimeTrigger(15, false),
                        null);
            }

            var localStorageManager = new LocalStorageManager();
            CookieContainer cookieTest = await localStorageManager.LoadCookie("SACookies2.txt");
            if (cookieTest.Count <= 0)
            {
                if (!RootFrame.Navigate(typeof(LoginPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            else
            {
                if (!RootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
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
