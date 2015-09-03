using AwfulForumsReader.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Tools;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace AwfulForumsReader.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public SettingsPage()
        {
            this.InitializeComponent();
            if (_localSettings.Values.ContainsKey(Constants.BackgroundEnable))
            {
                BackgroundNotificationsSwitch.IsOn = (bool)_localSettings.Values[Constants.BackgroundEnable];
            }
            if (_localSettings.Values.ContainsKey(Constants.BookmarkNotifications))
            {
                BookmarkNotificationsSwitch.IsOn = (bool)_localSettings.Values[Constants.BookmarkNotifications];
            }
            if (_localSettings.Values.ContainsKey(Constants.BookmarkBackground))
            {
                BookmarkLiveTiles.IsOn = (bool)_localSettings.Values[Constants.BookmarkBackground];
            }
            if (_localSettings.Values.ContainsKey(Constants.BookmarkStartup))
            {
                LoadBookmarksOnLoadSwitch.IsOn = (bool)_localSettings.Values[Constants.BookmarkStartup];
            }

            if (_localSettings.Values.ContainsKey(Constants.NavigateBack))
            {
                KeepCurrentViewSwitch.IsOn = (bool)_localSettings.Values[Constants.NavigateBack];
            }


            if (_localSettings.Values.ContainsKey(Constants.AutoRefresh))
            {
                AutoReloadSwitch.IsOn = (bool)_localSettings.Values[Constants.AutoRefresh];
            }

            if (_localSettings.Values.ContainsKey(Constants.OpenInBrowser))
            {
                InternetEnable.IsOn = (bool)_localSettings.Values[Constants.OpenInBrowser];
            }

            if (_localSettings.Values.ContainsKey(Constants.DarkMode))
            {
                DarkLightThemeSwitch.IsOn = (bool)_localSettings.Values[Constants.DarkMode];
            }
            else
            {
                // If the mode was never set, it's automatically true unless the user actually sets it to false.
                DarkLightThemeSwitch.IsOn = false;
            }

            if (_localSettings.Values.ContainsKey(Constants.ThemeDefault))
            {
                ThemeComboBox.SelectedIndex = (int)_localSettings.Values[Constants.ThemeDefault];
            }

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="Common.SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="Common.NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void SaveSettings_OnClick(object sender, RoutedEventArgs e)
        {

            if (BackgroundNotificationsSwitch.IsOn)
            {
                BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
                var task = await
                    BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.BackgroundTaskEntryPoint,
                        BackgroundTaskUtils.BackgroundTaskName,
                        new TimeTrigger(15, false),
                        null);
                _localSettings.Values[Constants.BackgroundEnable] = true;
            }
            else
            {
                try
                {
                    BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
                }
                catch (Exception)
                {
                   // TODO: Tell user background task failed to unregister :(
                }
                _localSettings.Values[Constants.BackgroundEnable] = false;
            }

            _localSettings.Values[Constants.ThemeDefault] = ThemeComboBox.SelectedIndex;


            if (BookmarkLiveTiles.IsOn)
            {
                // Run bookmark live tile creator every 15 minutes.
                // TODO: Change 15 to user selectable value.
                _localSettings.Values[Constants.BookmarkBackground] = true;
            }
            else
            {
                _localSettings.Values[Constants.BookmarkBackground] = false;
            }

            if (BookmarkNotificationsSwitch.IsOn)
            {
                // Run bookmark live tile creator every 15 minutes.
                // TODO: Change 15 to user selectable value.
                _localSettings.Values[Constants.BookmarkNotifications] = true;
            }
            else
            {
                _localSettings.Values[Constants.BookmarkNotifications] = false;
            }

            if (KeepCurrentViewSwitch.IsOn)
            {
                _localSettings.Values[Constants.NavigateBack] = true;
            }
            else
            {
                _localSettings.Values[Constants.NavigateBack] = false;
            }

            if (InternetEnable.IsOn)
            {
                _localSettings.Values[Constants.OpenInBrowser] = true;
            }
            else
            {
                _localSettings.Values[Constants.OpenInBrowser] = false;
            }

            if (LoadBookmarksOnLoadSwitch.IsOn)
            {
                _localSettings.Values[Constants.BookmarkStartup] = true;
            }
            else
            {
                _localSettings.Values[Constants.BookmarkStartup] = false;
            }

            _localSettings.Values[Constants.BookmarkDefault] = FilterComboBox.SelectedIndex;

            if (DarkLightThemeSwitch.IsOn)
            {
                _localSettings.Values[Constants.DarkMode] = true;
            }
            else
            {
                _localSettings.Values[Constants.DarkMode] = false;
            }

            if (AutoReloadSwitch.IsOn)
            {
                _localSettings.Values[Constants.AutoRefresh] = true;
            }
            else
            {
                _localSettings.Values[Constants.AutoRefresh] = false;
            }
        }
    }
}
