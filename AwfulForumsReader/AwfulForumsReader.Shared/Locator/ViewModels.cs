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

        public static MainForumsPageViewModel MainForumsPageVm
        {
            get { return App.Container.Resolve<MainForumsPageViewModel>(); }
        }

        //public static ThreadViewModel ThreadVm
        //{
        //    get { return App.Container.Resolve<ThreadViewModel>(); }
        //}

        public static ThreadListPageViewModel ThreadListPageVm
        {
            get { return App.Container.Resolve<ThreadListPageViewModel>(); }
        }

        public static BookmarksPageViewModel BookmarksPageVm
        {
            get { return App.Container.Resolve<BookmarksPageViewModel>(); }
        }

        public static PrivateMessageListViewModel PrivateMessageVm
        {
            get { return App.Container.Resolve<PrivateMessageListViewModel>(); }
        }

        public static ThreadPageViewModel ThreadPageVm
        {
            get { return App.Container.Resolve<ThreadPageViewModel>(); }
        }
    }
}
