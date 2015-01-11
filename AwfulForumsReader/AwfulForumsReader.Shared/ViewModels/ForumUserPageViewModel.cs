using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class ForumUserPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private ForumUserEntity _forumUserEntity;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }

        public ForumUserEntity ForumUserEntity
        {
            get { return _forumUserEntity; }
            set
            {
                SetProperty(ref _forumUserEntity, value);
                OnPropertyChanged();
            }
        }
        private readonly ForumUserManager _forumUserManager = new ForumUserManager();
        public async Task Initialize(long userId)
        {
            ForumUserEntity = new ForumUserEntity();
            IsLoading = true;
            try
            {
                ForumUserEntity = await _forumUserManager.GetUserFromProfilePage(userId);
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Failed to get user info", ex);
            }
            IsLoading = false;
        }
    }
}
