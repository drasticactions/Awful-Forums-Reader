using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using AwfulForumsReader.Database.Context;
using AwfulForumsLibrary.Entity;
using System.ComponentModel;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Database.Commands;

namespace AwfulForumsReader.Tools
{
    public class PageScrollingCollection : ObservableCollection<ForumThreadEntity>, ISupportIncrementalLoading
    {
        public PageScrollingCollection(ForumEntity forumEntity, int pageCount)
        {
            HasMoreItems = true;
            IsLoading = false;
            PageCount = pageCount;
            ForumEntity = forumEntity;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        private readonly BookmarkManager _bookmarkManager = new BookmarkManager();

        public async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            IsLoading = true;
            var threadManager = new ThreadManager();
            ObservableCollection<ForumThreadEntity> forumThreadEntities;
            try
            {
                forumThreadEntities = await threadManager.GetForumThreadsAsync(ForumEntity, PageCount);
            }
            catch (Exception ex)
            {
                HasMoreItems = false;
                IsLoading = false;
                return new LoadMoreItemsResult { Count = count };
            }

            foreach (ForumThreadEntity forumThreadEntity in forumThreadEntities.Where(forumThreadEntity => !forumThreadEntity.IsAnnouncement))
            {
                if (_bookmarkManager.IsBookmark(forumThreadEntity.ThreadId))
                {
                    forumThreadEntity.IsBookmark = true;
                }
                Add(forumThreadEntity);
            }
            if (forumThreadEntities.Any(node => !node.IsAnnouncement))
            {
                HasMoreItems = true;
                PageCount++;
            }
            else
            {
                HasMoreItems = false;
            }

            IsLoading = false;
            return new LoadMoreItemsResult { Count = count };
        }

        private ForumEntity ForumEntity { get; set; }

        private int PageCount { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }

            private set
            {
                _isLoading = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLoading"));
            }
        }

        public bool HasMoreItems { get; private set; }
    }
}
