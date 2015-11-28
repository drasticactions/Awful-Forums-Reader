using AwfulForumsReader.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
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
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Commands.Navigation;
using AwfulForumsReader.Models;
using AwfulForumsReader.Tools;
using Newtonsoft.Json;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace AwfulForumsReader.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage
    {

        private NavigationHelper navigationHelper;
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public MainPage()
        {
            this.InitializeComponent();
            App.RootFrame = MainFrame;
            App.RootFrame.Navigated += RootFrameOnNavigated;
            Locator.ViewModels.MainPageVm.MainPageSplitView = Splitter;
            if (_localSettings.Values.ContainsKey(Constants.BookmarkStartup))
            {
                if ((bool) _localSettings.Values[Constants.BookmarkStartup])
                {
                    var test = new NavigateToBookmarksCommand();
                    test.Execute(null);
                }
                else
                {
                    var test2 = new NavigateToMainForumsPage();
                    test2.Execute(null);
                }
            }
            else
            {
                var test3 = new NavigateToMainForumsPage();
                test3.Execute(null);
            }

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
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
            string launchString = (string)e.NavigationParameter;
            if (!string.IsNullOrEmpty(launchString))
            {
                var arguments = JsonConvert.DeserializeObject<ToastNotificationArgs>(launchString);

                if (arguments != null && arguments.openBookmarks)
                {
                    var bookmarkCommand = new NavigateToBookmarksCommand();
                    bookmarkCommand.Execute(null);
                    return;
                }

                if (arguments != null && arguments.openPrivateMessages)
                {
                    var bookmarkCommand = new NavigateToPrivateMessageListPageCommand();
                    bookmarkCommand.Execute(null);
                    return;
                }

                if (arguments != null && arguments.openForum)
                {
                    var jumpCommand = new NavigateToThreadListPageCommandViaJumplist();
                    jumpCommand.Execute(arguments.forumId);
                    return;
                }


                if (arguments != null && arguments.threadId > 0)
                {
                    var bookmarkCommand = new NavigateToBookmarksCommand();
                    bookmarkCommand.Execute(arguments.threadId);
                    return;
                }

                var forumEntity = JsonConvert.DeserializeObject<ForumEntity>(launchString);
                if (forumEntity != null)
                {
                    var navigateCommand = new NavigateToThreadListPageCommandViaTile();
                    navigateCommand.Execute(forumEntity);
                }
            }
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

        private void RootFrameOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = App.RootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private async void MenuClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as MenuItem;
            menuItem?.Command.Execute(null);
            if (Splitter.IsSwipeablePaneOpen)
            {
                Splitter.IsSwipeablePaneOpen = false;
            }
        }

        private void MenuSelection_Click(object sender, RoutedEventArgs e)
        {
            var menuListView = (ListView)sender;
            if (menuListView.SelectedItem == null)
            {
                return;
            }

            var menuItem = menuListView.SelectedItem as MenuItem;
            menuItem?.Command.Execute(null);
            if (Splitter.IsSwipeablePaneOpen)
            {
                Splitter.IsSwipeablePaneOpen = false;
            }
            menuListView.SelectedItem = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Splitter.DisplayMode = (Splitter.DisplayMode == SplitViewDisplayMode.Inline) ? SplitViewDisplayMode.CompactInline : SplitViewDisplayMode.Inline;
            Splitter.IsSwipeablePaneOpen = (Splitter.IsSwipeablePaneOpen != true);
        }

        private void Sidebar_Click(object sender, RoutedEventArgs e)
        {
            SidebarFontIcon.Glyph = Locator.ViewModels.MainPageVm.IsSidebarHidden ? "\uE127" : "\uE126";
            Locator.ViewModels.MainPageVm.IsSidebarHidden = !Locator.ViewModels.MainPageVm.IsSidebarHidden;
        }
    }
}
