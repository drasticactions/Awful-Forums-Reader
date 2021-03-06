﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Commands.Navigation;
using AwfulForumsReader.Commands.Posts;
using AwfulForumsReader.Commands.Threads;
using AwfulForumsReader.Common;
using AwfulForumsReader.Models;
using AwfulForumsReader.Tools;

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

        private bool _isSidebarHidden;
        public bool IsSidebarHidden
        {
            get { return _isSidebarHidden; }
            set
            {
                SetProperty(ref _isSidebarHidden, value);
                OnPropertyChanged();
            }
        }

        private bool _isSplitViewPaneOpen;

        public bool IsReloggingIn { get; set; } = false;
        public SplitView MainPageSplitView { get; set; }
        public AsyncDelegateCommand ClickBackButtonCommand { get; private set; }
        private bool _canClickBack;
        public bool CanClickBack
        {
            get { return _canClickBack; }
            set
            {
                if (_canClickBack == value) return;
                _canClickBack = value;
                OnPropertyChanged();
                ClickBackButtonCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsSplitViewOpen
        {
            get { return _isSplitViewPaneOpen; }
            set
            {
                SetProperty(ref _isSplitViewPaneOpen, value);
                OnPropertyChanged();
            }
        }

        public bool CanClickBackButton => App.RootFrame.CanGoBack;
        public List<MenuItem> MenuItems { get; set; }

        public async Task ClickBackButton()
        {
            App.RootFrame.GoBack();
        }

        public MainPageViewModel()
        {
            ClickBackButtonCommand = new AsyncDelegateCommand(async o => { await ClickBackButton(); },
               o => CanClickBackButton);
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
                }
                ,
                new MenuItem()
                {
                    Icon = "\uE15C",
                    Name = "Tabs",
                    Command = new NavigateToTabPageCommand()
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
                ,
                new MenuItem()
                {
                    Icon = "\uEB68",
                    Name = "About",
                    Command = new NavigateToAboutPage()
                }
            };
        }
    }
}
