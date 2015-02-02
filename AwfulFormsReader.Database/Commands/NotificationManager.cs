using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Database.Context;

namespace AwfulForumsReader.Database.Commands
{
    public class NotificationManager
    {
        private readonly NotifyThreadListContext _notifyThreadListContext = new NotifyThreadListContext();

        public async Task<bool> AddThreadToNotificationListAsync(ForumThreadEntity threadEntity)
        {
            var threadList = _notifyThreadListContext.NotifyThreads.ToList();
            if (threadList.Any() && threadList.Any(node => node.ThreadId == threadEntity.ThreadId))
            {
                return false;
            }
            await _notifyThreadListContext.NotifyThreads.AddAsync(threadEntity);
            await _notifyThreadListContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveThreadFromNotificationListAsync(ForumThreadEntity threadEntity)
        {
            var thread = _notifyThreadListContext.NotifyThreads.FirstOrDefault(node => node.ThreadId == threadEntity.ThreadId);
            if (thread == null)
            {
                return false;
            }
            _notifyThreadListContext.NotifyThreads.Remove(thread);
            await _notifyThreadListContext.SaveChangesAsync();
            return true;
        }

        public bool IsInList(ForumThreadEntity threadEntity)
        {
            return _notifyThreadListContext.NotifyThreads.Any(node => node.ThreadId == threadEntity.ThreadId);
        }

        public async Task<List<ForumThreadEntity>> GetNotifyThreadListAsync()
        {
            return await _notifyThreadListContext.NotifyThreads.ToListAsync();
        }
    }
}
