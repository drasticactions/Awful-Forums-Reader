using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Tools;
using Newtonsoft.Json;

namespace AwfulForumsReader.Common
{
    public class JumpListCreator
    {
        public static async Task CreateDefaultList(bool clear = false)
        {

            if (JumpList.IsSupported())
            {
                var jumplist = await JumpList.LoadCurrentAsync();
                if (!jumplist.Items.Any() || clear)
                {
                    jumplist.Items.Clear();
                    var args = new ToastNotificationArgs()
                    {
                        type = "jumplist",
                        openBookmarks = true
                    };
                    var item = JumpListItem.CreateWithArguments(JsonConvert.SerializeObject(args), "Open Bookmarks");
                    item.Logo = new Uri("ms-appx:///Assets/BadgeLogo.scale-100.png");
                    args.openBookmarks = false;
                    args.openPrivateMessages = true;

                    var item2 = JumpListItem.CreateWithArguments(JsonConvert.SerializeObject(args), "Open Private Messages");
                    item2.Logo = new Uri("ms-appx:///Assets/BadgeLogo.scale-100.png");
                    jumplist.Items.Add(item);
                    jumplist.Items.Add(item2);
                    var seperate = JumpListItem.CreateSeparator();
                    jumplist.Items.Add(seperate);
                    await jumplist.SaveAsync();
                }
            }
           
        }

        public static async Task AddNewJumplistForum(ForumEntity forum)
        {
            if (!JumpList.IsSupported())
            {
                return;
            }

            var jumplist = await JumpList.LoadCurrentAsync();
            var itemExists = false;
            foreach (var item in jumplist.Items)
            {
                var args = JsonConvert.DeserializeObject<ToastNotificationArgs>(item.Arguments);
                if (args == null)
                {
                    continue;
                }
                if (args.openPrivateMessages || args.openBookmarks)
                {
                    continue;
                }
                if (args.openForum && args.forumId == forum.Id)
                {
                    itemExists = true;
                }
            }

            if (itemExists)
            {
                return;
            }

            var newArgs = new ToastNotificationArgs()
            {
                type = "jumplist",
                openForum = true,
                forumId = forum.Id
            };

            
            var jumpItem = JumpListItem.CreateWithArguments(JsonConvert.SerializeObject(newArgs), $"Open {forum.Name}");
            jumpItem.Logo = new Uri("ms-appx:///Assets/BadgeLogo.scale-100.png");
            jumplist.Items.Add(jumpItem);
            await jumplist.SaveAsync();
        }
    }
}
