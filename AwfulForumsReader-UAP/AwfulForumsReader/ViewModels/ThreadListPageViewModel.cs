using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class ThreadListPageViewModel : NotifierBase
    {
        public ForumEntity ForumEntity { get; set; }
        private string _forumTitle;
        private ObservableCollection<ForumEntity> _subForumEntities;
        private PageScrollingCollection _forumPageScrollingCollection;

        public NavigateToLastPageInThreadPageCommand NavigateToLastPageInThreadPageCommand { get; set; } = new NavigateToLastPageInThreadPageCommand();

        public NavigateToThreadPageCommand NavigateToThreadPageCommand { get; set; } = new NavigateToThreadPageCommand();

        public NavigateToNewThreadCommand NavigateToNewThreadCommand { get; set; } = new NavigateToNewThreadCommand();

        public RefreshThreadListCommand RefreshThreadListCommand { get; set; } = new RefreshThreadListCommand();

        public string ForumTitle
        {
            get { return _forumTitle; }
            set
            {
                SetProperty(ref _forumTitle, value);
                OnPropertyChanged();
            }
        }

        public AddOrRemoveBookmarkCommand AddOrRemoveBookmark { get; set; } = new AddOrRemoveBookmarkCommand();

        public UnreadThreadCommand UnreadThread { get; set; } = new UnreadThreadCommand();

        public LastPageCommand LastPage { get; set; } = new LastPageCommand();

        public PageScrollingCollection ForumPageScrollingCollection
        {
            get { return _forumPageScrollingCollection; }
            set
            {
                SetProperty(ref _forumPageScrollingCollection, value);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ForumEntity> SubForumEntities
        {
            get { return _subForumEntities; }
            set
            {
                SetProperty(ref _subForumEntities, value);
                OnPropertyChanged();
            }
        }

        public void Initialize(ForumEntity forumEntity)
        {
            SubForumEntities = new ObservableCollection<ForumEntity>();
            ForumEntity = forumEntity;
            ForumTitle = forumEntity.Name;
            Refresh();
        }

        public void Refresh()
        {
            ForumPageScrollingCollection = new PageScrollingCollection(ForumEntity, 1);
        }

        public void UpdateThreadList()
        {
            OnPropertyChanged("ForumPageScrollingCollection");
        }
    }
}
