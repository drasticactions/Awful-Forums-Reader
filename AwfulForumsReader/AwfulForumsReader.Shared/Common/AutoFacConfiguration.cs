﻿using System;
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
            builder.RegisterType<LastPostPageViewModel>().SingleInstance();
            builder.RegisterType<SmiliesPageViewModel>().SingleInstance();
            builder.RegisterType<MainForumsPageViewModel>().SingleInstance();
            builder.RegisterType<ThreadListPageViewModel>().SingleInstance();
            builder.RegisterType<BookmarksPageViewModel>().SingleInstance();
            builder.RegisterType<PrivateMessageListViewModel>().SingleInstance();
            builder.RegisterType<ThreadPageViewModel>().SingleInstance();
            builder.RegisterType<NewThreadReplyPageViewModel>().SingleInstance();
            builder.RegisterType<NewThreadPageViewModel>().SingleInstance();
            builder.RegisterType<PostIconListPageViewModel>().SingleInstance();
            builder.RegisterType<BbCodeListPageViewModel>().SingleInstance();
            builder.RegisterType<PreviewThreadPageViewModel>().SingleInstance();

            builder.RegisterType<MainForumsPage>();
            return builder.Build();
        }
    }
}
