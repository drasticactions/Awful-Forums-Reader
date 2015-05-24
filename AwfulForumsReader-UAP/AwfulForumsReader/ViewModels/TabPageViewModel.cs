using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Commands.Threads;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class TabPageViewModel : NotifierBase
    {
        private ObservableCollection<ForumThreadEntity> _tabThreads;

        public ObservableCollection<ForumThreadEntity> TabThreads
        {
            get { return _tabThreads; }
            set
            {
                SetProperty(ref _tabThreads, value);
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        private bool _isEmpty;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }

        public bool IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                SetProperty(ref _isEmpty, value);
                OnPropertyChanged();
            }
        }

        private readonly MainForumsDatabase _tabManager = new MainForumsDatabase();

        public RemoveTabCommand RemoveTabCommand { get; set; } = new RemoveTabCommand();

        public async Task Initialize()
        {
            IsLoading = true;
            IsEmpty = false;
            try
            {
                TabThreads = new ObservableCollection<ForumThreadEntity>();
                var tabs = await _tabManager.GetAllTabThreads();
                if (tabs != null && tabs.Any())
                {
                    TabThreads = tabs.ToObservableCollection();
                }
                else
                {
                    IsEmpty = true;
                }
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Failed to get Tabs", ex);
            }

            IsLoading = false;
        }
    }
}
