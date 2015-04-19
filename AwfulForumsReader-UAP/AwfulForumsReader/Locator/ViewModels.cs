using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel;
using Autofac;
using AwfulForumsReader.Common;
using AwfulForumsReader.ViewModels;

namespace AwfulForumsReader.Locator
{
    public class ViewModels
    {
        public ViewModels()
        {
            if (DesignMode.DesignModeEnabled)
            {
                App.Container = AutoFacConfiguration.Configure();
            }
        }
        //public static SaclopediaPageViewModel SaclopediaPageVm => App.Container.Resolve<SaclopediaPageViewModel>();
        public static SearchPageViewModel SearchPageVm => App.Container.Resolve<SearchPageViewModel>();
        //public static ForumUserPageViewModel UserPageVm => App.Container.Resolve<ForumUserPageViewModel>();
        public static NewPrivateMessageViewModel NewPrivateMessagePageVm => App.Container.Resolve<NewPrivateMessageViewModel>();

        public static PrivateMessagePageViewModel PrivateMessagePageVm => App.Container.Resolve<PrivateMessagePageViewModel>();

        public static SmiliesPageViewModel SmiliesPageVm => App.Container.Resolve<SmiliesPageViewModel>();

        //public static LastPostPageViewModel LastPostPageVm => App.Container.Resolve<LastPostPageViewModel>();

        public static MainForumsPageViewModel MainForumsPageVm => App.Container.Resolve<MainForumsPageViewModel>();

        public static MainPageViewModel MainPageVm => App.Container.Resolve<MainPageViewModel>();


        public static BbCodeListPageViewModel BbCodeListVm => App.Container.Resolve<BbCodeListPageViewModel>();

        public static NewThreadReplyPageViewModel NewThreadReplyVm => App.Container.Resolve<NewThreadReplyPageViewModel>();

        public static PostIconListPageViewModel PostIconListPageVm => App.Container.Resolve<PostIconListPageViewModel>();

        public static NewThreadPageViewModel NewThreadVm => App.Container.Resolve<NewThreadPageViewModel>();

        public static ThreadListPageViewModel ThreadListPageVm => App.Container.Resolve<ThreadListPageViewModel>();

        public static BookmarksPageViewModel BookmarksPageVm => App.Container.Resolve<BookmarksPageViewModel>();

        public static PreviewThreadPageViewModel PreviewThreadPageVm => App.Container.Resolve<PreviewThreadPageViewModel>();

        public static PrivateMessageListViewModel PrivateMessageVm => App.Container.Resolve<PrivateMessageListViewModel>();

        public static ThreadPageViewModel ThreadPageVm => App.Container.Resolve<ThreadPageViewModel>();
    }
}
