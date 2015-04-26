using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using AwfulForumsLibrary.Entity;
using System.ComponentModel;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Database;

namespace AwfulForumsReader.Tools
{
    public class PageScrollingCollection : ObservableCollection<ForumThreadEntity>, ISupportIncrementalLoading
    {
        public PageScrollingCollection(ForumEntity forumEntity, int pageCount, DateTime date, bool isArchive = false)
        {
            HasMoreItems = true;
            IsLoading = false;
            PageCount = pageCount;
            ForumEntity = forumEntity;
            IsArchive = isArchive;
            ArchiveDateTime = date;
        }

        public DateTime ArchiveDateTime { get; set; }

        public bool IsArchive { get; set; }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        public async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            IsLoading = true;
            if (IsArchive)
            {
                await GetArchiveThreads();
            }
            else
            {
                await GetThreads();
            }
            IsLoading = false;
            return new LoadMoreItemsResult { Count = count };
        }

        public async Task GetArchiveThreads()
        {
            var threadManager = new ThreadManager();
            ObservableCollection<ForumThreadEntity> forumThreadEntities;
            try
            {
                forumThreadEntities = await threadManager.GetArchiveForumThreadsAsync(ForumEntity, PageCount, ArchiveDateTime.Day, ArchiveDateTime.Month, ArchiveDateTime.Year);
            }
            catch (Exception ex)
            {
                HasMoreItems = false;
                IsLoading = false;
                return;
            }

            foreach (ForumThreadEntity forumThreadEntity in forumThreadEntities.Where(forumThreadEntity => !forumThreadEntity.IsAnnouncement))
            {
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
        }

        public async Task GetThreads()
        {
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
                return;
            }

            foreach (ForumThreadEntity forumThreadEntity in forumThreadEntities.Where(forumThreadEntity => !forumThreadEntity.IsAnnouncement))
            {
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
