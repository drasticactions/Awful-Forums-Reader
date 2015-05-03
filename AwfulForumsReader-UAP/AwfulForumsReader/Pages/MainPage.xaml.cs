using AwfulForumsReader.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Models;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace AwfulForumsReader.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        public MainPage()
        {
            this.InitializeComponent();
            App.RootFrame = MainFrame;
            App.RootFrame.Navigated += RootFrameOnNavigated;
            Locator.ViewModels.MainPageVm.MainPageSplitView = Splitter;
            var test = new NavigateToMainForumsPage();
            test.Execute(null);
        }
        private void RootFrameOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            Locator.ViewModels.MainPageVm.CanClickBack = App.RootFrame.CanGoBack;
        }

        private async void MenuClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as MenuItem;
            menuItem?.Command.Execute(null);
            if (Splitter.IsPaneOpen)
            {
                Splitter.IsPaneOpen = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Splitter.DisplayMode = (Splitter.DisplayMode == SplitViewDisplayMode.Inline) ? SplitViewDisplayMode.CompactInline : SplitViewDisplayMode.Inline;
            Splitter.IsPaneOpen = (Splitter.IsPaneOpen != true);
        }
    }
}
