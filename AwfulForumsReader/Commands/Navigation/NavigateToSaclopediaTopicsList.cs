using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands.Navigation
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

                var db = new SaclopediaDatabase();
                var navTopicList = await db.GetTopicList(navEntity);
                if (!navTopicList.Any())
                {
                    navTopicList =
                        await
                            Locator.ViewModels.SaclopediaPageVm.SaclopediaManager.GetSaclopediaTopics(
                                Constants.BaseUrl + navEntity.Link);
                    foreach (var item in navTopicList)
                    {
                        item.ParentNavId = navEntity.Id;
                        item.ParentNav = navEntity;
                    }
                    await db.SaveTopicList(navTopicList);
                }
                Locator.ViewModels.SaclopediaPageVm.SaclopediaNavigationTopicEntities =
                    navTopicList;

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
                 var platformIdentifier = PlatformIdentifier.Windows8;
                 //App.RootFrame.Navigate(typeof (SaclopediaEntryPage));

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
