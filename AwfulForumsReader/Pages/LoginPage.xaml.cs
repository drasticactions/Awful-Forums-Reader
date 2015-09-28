using AwfulForumsReader.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AwfulForumsReader.ViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace AwfulForumsReader.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LoginPage : Page, IDisposable
    {
        public void Dispose()
        {
            var vm = DataContext as LoginPageViewModel;
            if (vm == null) return;
            vm.LoginFailed -= OnLoginFailed;
            vm.LoginSuccessful -= OnLoginSuccessful;
        }

        private void OnLoginSuccessful(object sender, EventArgs e)
        {
            if (Locator.ViewModels.MainPageVm.IsReloggingIn)
            {
                App.RootFrame.Navigate(typeof (MainForumsPage));
            }
            else
            {
                Frame.Navigate(typeof(MainPage));
                Frame.BackStack.Clear();
            }
        }

        private static async void OnLoginFailed(object sender, EventArgs e)
        {
            var msg = new MessageDialog("ERROR: Your Username/Password were incorrect, try again", "Alert");
            await msg.ShowAsync();
        }


        public LoginPage()
        {
            InitializeComponent();
            var vm = (LoginPageViewModel)DataContext;
            vm.LoginFailed += OnLoginFailed;
            vm.LoginSuccessful += OnLoginSuccessful;
        }
    }
}
