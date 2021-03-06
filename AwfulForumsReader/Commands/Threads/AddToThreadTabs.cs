﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands.Threads
{
    public class AddToThreadTabs : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var args = parameter as ForumThreadEntity ?? Locator.ViewModels.ThreadPageVm.ForumThreadEntity;
            if (args == null) return;

            var tabManager = new MainForumsDatabase();
            var test = await tabManager.DoesTabExist(args);
            if (!args.IsBookmark && !test)
            {
                await tabManager.AddThreadToTabListAsync(args);
            }
        }
    }
}
