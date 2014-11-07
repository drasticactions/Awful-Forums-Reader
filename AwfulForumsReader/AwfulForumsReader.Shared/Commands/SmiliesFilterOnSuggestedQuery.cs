using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class SmiliesFilterOnSuggestedQuery : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var args = parameter as SearchBoxSuggestionsRequestedEventArgs;
            if (args == null)
            {
                return;
            }

            var vm = Locator.ViewModels.SmiliesPageVm;
            if (vm.SmileCategoryList == null) return;
            string queryText = args.QueryText;
            if (string.IsNullOrEmpty(queryText)) return;
            var suggestionCollection = args.Request.SearchSuggestionCollection;
            foreach (var smile in vm.SmileCategoryList.SelectMany(smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Contains(queryText))))
            {
                suggestionCollection.AppendQuerySuggestion(smile.Title);
            }
        }
    }

    public class SmiliesFilterOnSubmittedQuery : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var args = parameter as SearchBoxQuerySubmittedEventArgs;
            if (args == null)
            {
                return;
            }

            var vm = Locator.ViewModels.SmiliesPageVm;
            if (vm.SmileCategoryList == null) return;
            string queryText = args.QueryText;
            if (string.IsNullOrEmpty(queryText)) return;
            var result = vm.SmileCategoryList.SelectMany(
                smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Equals(queryText))).FirstOrDefault();
            if (result == null)
            {
                return;
            }
            vm.ReplyBox.Text = vm.ReplyBox.Text.Insert(vm.ReplyBox.Text.Length, result.Title);
            switch (Locator.ViewModels.SmiliesPageVm.ReplyBoxLocation)
            {
                case ReplyBoxLocation.NewThread:
                    Locator.ViewModels.NewThreadVm.PostBody = vm.ReplyBox.Text;
                    break;
                case ReplyBoxLocation.NewReply:
                    Locator.ViewModels.NewThreadReplyVm.PostBody = vm.ReplyBox.Text;
                    break;
                default:
                    throw new Exception("Unsupported type");
            }
            App.RootFrame.GoBack();
        }
    }

    public class SmiliesFilterOnItemClick : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs; ;
            if (args == null)
            {
                return;
            }
            var smileEntity = args.ClickedItem as SmileEntity;
            if (smileEntity == null)
            {
                return;
            }
            var vm = Locator.ViewModels.SmiliesPageVm;
            vm.ReplyBox.Text = vm.ReplyBox.Text.Insert(vm.ReplyBox.Text.Length, smileEntity.Title);
            switch (Locator.ViewModels.SmiliesPageVm.ReplyBoxLocation)
            {
                case ReplyBoxLocation.NewThread:
                    Locator.ViewModels.NewThreadVm.PostBody = vm.ReplyBox.Text;
                    break;
                case ReplyBoxLocation.NewReply:
                    Locator.ViewModels.NewThreadReplyVm.PostBody = vm.ReplyBox.Text;
                    break;
                case ReplyBoxLocation.PrivateMessage:
                    Locator.ViewModels.NewPrivateMessagePageVm.PostBody = vm.ReplyBox.Text;
                    break;
                default:
                    throw new Exception("Unsupported type");
            }
            App.RootFrame.GoBack();

        }
    }

    public class SmiliesFilterOnChangedQuery : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var args = parameter as SearchBoxQueryChangedEventArgs;
            if (args == null)
            {
                return;
            }
            var vm = Locator.ViewModels.SmiliesPageVm;
            string queryText = args.QueryText;
            if (string.IsNullOrEmpty(queryText))
            {
                vm.SmileCategoryList = vm.FullSmileCategoryEntities;
                return;
            }
            var result = vm.SmileCategoryList.SelectMany(
                smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Equals(queryText))).FirstOrDefault();
            if (result != null) return;
            var searchList = vm.FullSmileCategoryEntities.SelectMany(
                smileCategory => smileCategory.SmileList.Where(smile => smile.Title.Contains(queryText)));
            List<SmileEntity> smileListEntities = searchList.ToList();
            var searchSmileCategory = new SmileCategoryEntity()
            {
                Name = "Search",
                SmileList = smileListEntities
            };
            var fakeSmileCategoryList = new List<SmileCategoryEntity> { searchSmileCategory };
            vm.SmileCategoryList = fakeSmileCategoryList.ToObservableCollection();
        }
    }
}
