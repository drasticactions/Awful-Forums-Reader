using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;

namespace AwfulForumsReader.Database
{
    public class SaclopediaDatabase
    {
        public async Task<List<SaclopediaNavigationEntity>> GetNavigationList()
        {
            var ds = new SaclopediaDataSource();
            return await ds.SaclopediaNavigationRepository.GetAllWithChildren();
        }

        public async Task SaveNavigationList(List<SaclopediaNavigationEntity> list)
        {
            var ds = new SaclopediaDataSource();
            await ds.SaclopediaNavigationRepository.RemoveAll();
            foreach (var item in list)
            {
                await ds.SaclopediaNavigationRepository.Create(item);
            }
        }

        public async Task<List<SaclopediaNavigationTopicEntity>> GetTopicList(SaclopediaNavigationEntity entity)
        {
            var ds = new SaclopediaDataSource();
            return await ds.SaclopediaNavigationTopicRepository.Items.Where(node => node.ParentNavId == entity.Id).ToListAsync();
        }

        public async Task SaveTopicList(List<SaclopediaNavigationTopicEntity> list)
        {
            var ds = new SaclopediaDataSource();
            foreach (var item in list)
            {
                await ds.SaclopediaNavigationTopicRepository.CreateWithChildren(item);
            }
        }
    }
}
