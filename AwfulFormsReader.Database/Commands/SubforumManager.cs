using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Database.Context;

namespace AwfulForumsReader.Database.Commands
{
    public class SubforumManager
    {
        private readonly LinkedSubforumContext _linkedSubforumContext = new LinkedSubforumContext();

        public async Task AddForumToHistoryListAsync(ForumEntity threadEntity)
        {
            try
            {
                await _linkedSubforumContext.LinkedForums.AddAsync(threadEntity);
                await _linkedSubforumContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save forum to history list", ex);
            }
        }

        public async Task<ForumEntity> ReturnLastForumEntity()
        {
           return await _linkedSubforumContext.LinkedForums.LastOrDefaultAsync();
        }

        public async Task<ForumEntity> RemoveLastEntry(ForumEntity forum)
        {
            try
            {
                _linkedSubforumContext.LinkedForums.Remove(forum);
                await _linkedSubforumContext.SaveChangesAsync();
                return forum;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to find and remove last forum from history list.", ex);
            }
        }

        public async Task RemoveAllEntries()
        {
            try
            {
                var list = _linkedSubforumContext.LinkedForums;
                _linkedSubforumContext.LinkedForums.RemoveRange(list);
                await _linkedSubforumContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to find and remove forums from history list.", ex);
            }
        }
    }
}
