using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Database.Context;
namespace AwfulForumsReader.Database.Commands
{
    public class TabManager
    {
        private readonly LinkedThreadListContext _db = new LinkedThreadListContext();

        public async Task<bool> AddThreadToTabListAsync(ForumThreadEntity threadEntity)
        {
            if (_db.LinkedThreads.Any(node => node.ThreadId == threadEntity.ThreadId))
            {
                return false;
            }

            await _db.LinkedThreads.AddAsync(threadEntity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveThreadFromTabList(ForumThreadEntity threadEntity)
        {
            var realThread = await _db.LinkedThreads.FirstOrDefaultAsync(node => node.ThreadId == threadEntity.ThreadId);
            if (realThread == null)
            {
                return false;
            }

            _db.LinkedThreads.Remove(realThread);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task RemoveAllThreadsFromTabList()
        {
            var all = from c in _db.LinkedThreads select c;
            _db.LinkedThreads.RemoveRange(all);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ForumThreadEntity>> GetAllTabThreads()
        {
            return await _db.LinkedThreads.ToListAsync();
        }
    }
}
