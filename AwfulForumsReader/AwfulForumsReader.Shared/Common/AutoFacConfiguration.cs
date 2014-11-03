using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using AwfulForumsReader.Pages;
using AwfulForumsReader.ViewModels;

namespace AwfulForumsReader.Common
{
    public class AutoFacConfiguration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            // Register View Models
            //builder.RegisterType<ThreadViewModel>().SingleInstance();
            builder.RegisterType<MainForumsPageViewModel>().SingleInstance();
            builder.RegisterType<ThreadListPageViewModel>().SingleInstance();
            builder.RegisterType<BookmarksPageViewModel>().SingleInstance();
            //builder.RegisterType<PrivateMessageViewModel>().SingleInstance();

            builder.RegisterType<MainForumsPage>();
            return builder.Build();
        }
    }
}
