using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class NavigateToSaclopediaTopicsList : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs;
            if (args == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Navigation failed", new Exception("Arguments are null"));
                return;
            }
            Locator.ViewModels.SaclopediaPageVm.IsLoading = true;
            try
            {
                var navEntity = (SaclopediaNavigationEntity)args.ClickedItem;
                Locator.ViewModels.SaclopediaPageVm.SaclopediaNavigationTopicEntities =
                    await Locator.ViewModels.SaclopediaPageVm.SaclopediaManager.GetSaclopediaTopics(Constants.BaseUrl + navEntity.Link);

            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Navigation failed", ex);
            }
            Locator.ViewModels.SaclopediaPageVm.IsLoading = false;
        }
    }

    public class NavigateToSaclopediaTopic : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs;
            if (args == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Navigation failed", new Exception("Arguments are null"));
                return;
            }
            Locator.ViewModels.SaclopediaPageVm.IsLoading = true;
            try
            {
                var navEntity = (SaclopediaNavigationTopicEntity)args.ClickedItem;
                var result =
                    await Locator.ViewModels.SaclopediaPageVm.SaclopediaManager.GetSaclopediaEntity(Constants.BaseUrl + navEntity.Link);
                Locator.ViewModels.SaclopediaPageVm.Title = result.Title;
                PlatformIdentifier platformIdentifier = PlatformIdentifier.Windows8;
#if WINDOWS_PHONE_APP
                 platformIdentifier = PlatformIdentifier.WindowsPhone;
                 App.RootFrame.Navigate(typeof (SaclopediaEntryPage));
#endif
                Locator.ViewModels.SaclopediaPageVm.Body = await HtmlFormater.FormatSaclopediaEntry(result.Body, platformIdentifier);
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Navigation failed", ex);
            }
            Locator.ViewModels.SaclopediaPageVm.IsLoading = false;
        }
    }
}
