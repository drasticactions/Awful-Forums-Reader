using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Models;

namespace AwfulForumsReader.ViewModels
{
    public class MainPageViewModel : NotifierBase
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }

        public List<MenuItem> MenuItems { get; set; }

        public MainPageViewModel()
        {
            MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Icon = "\uE7F8",
                    Name = "Home",
                    Command = new NavigateToMainForumsPage()
                },
                new MenuItem()
                {
                    Icon = "\uE8F1",
                    Name = "Bookmarks",
                    Command = new NavigateToBookmarksCommand()
                },
                new MenuItem()
                {
                    Icon = "\uE91C",
                    Name = "Private Messages",
                    Command = new NavigateToPrivateMessageListPageCommand()
                },
                new MenuItem()
                {
                    Icon = "\uE8A1",
                    Name = "SAclopedia",
                    Command = new NavigateToSaclopedia()
                },
                new MenuItem()
                {
                    Icon = "\uE721",
                    Name = "Search",
                    Command = new NavigateToSearchPageCommand()
                },
                new MenuItem()
                {
                    Icon = "\uE713",
                    Name = "Settings",
                    Command = new NavigateToSettingsCommand()
                }
            };
        }
    }
}
