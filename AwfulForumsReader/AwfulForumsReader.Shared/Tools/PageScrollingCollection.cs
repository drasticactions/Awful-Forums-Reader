using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using AwfulForumsReader.Context;
using AwfulForumsReader.Core.Entity;
using System.ComponentModel;
using AwfulForumsReader.Core.Manager;

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
                throw new Exception(string.Format("Error getting thread list {0}", ex));
            }

            foreach (ForumThreadEntity forumThreadEntity in forumThreadEntities.Where(forumThreadEntity => !forumThreadEntity.IsAnnouncement))
            {
                using (var db = new MainForumListContext())
                {
                    if (db.BookmarkThreads.Any(node => node.ThreadId == forumThreadEntity.ThreadId))
                    {
                        forumThreadEntity.IsBookmark = true;
                    }
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
