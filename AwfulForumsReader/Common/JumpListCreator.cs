using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using AwfulForumsReader.Tools;
using Newtonsoft.Json;

namespace AwfulForumsReader.Common
{
    public class JumpListCreator
    {
        public static async Task CreateDefaultList()
        {
            
            //if (JumpList.IsSupported())
            //{
            //    var jumplist = await JumpList.LoadCurrentAsync();
            //    var args = new ToastNotificationArgs()
            //    {
            //        type = "jumplist",
            //        openBookmarks = true
            //    };
            //    var item = JumpListItem.CreateWithArguments(JsonConvert.SerializeObject(args), "Open Bookmarks");
            //    item.Logo = new Uri("ms-appx:///Assets/BadgeLogo.scale-100.png");
            //    args.openBookmarks = false;
            //    args.openPrivateMessages = true;

            //    var item2 = JumpListItem.CreateWithArguments(JsonConvert.SerializeObject(args), "Open Private Messages");
            //    jumplist.Items.Add(item);
            //    jumplist.Items.Add(item2);
            //    await jumplist.SaveAsync();
            //}
            //var jumpListItemType = typeof(Windows.UI.StartScreen.SecondaryTile).GetTypeInfo().Assembly.GetType("Windows.UI.StartScreen.JumpListItem");
            //if (jumpListItemType != null)
            //{
            //    var jumpListItem = jumpListItemType.GetMethod("CreateWithArguments").Invoke(null, new[] { "Test", "test" });
            //    jumpListItemType.GetProperty("DisplayName").SetValue(jumpListItem, "Test");
            //    jumpListItemType.GetProperty("Description").SetValue(jumpListItem, "Test");

            //    var jumpListType = typeof(Windows.UI.StartScreen.SecondaryTile).GetTypeInfo().Assembly.GetType("Windows.UI.StartScreen.JumpList");
            //    var jumpList = await (dynamic)jumpListType.GetMethod("LoadCurrentAsync").Invoke(null, null);
            //    var jumpListItems = jumpListType.GetProperty("Items").GetValue(jumpList);
            //    jumpListType.GetProperty("Items").PropertyType.GetType().GetMethod("Add").Invoke(jumpListItems, new[] { jumpListItem });
            //    await (dynamic)jumpListType.GetMethod("SaveAsync").Invoke(jumpList, null);

            //    Debug.WriteLine("Done");
            //}
            //else
            //{
            //    Debug.WriteLine("L'api non c'è");
            //}
        }
    }
}
