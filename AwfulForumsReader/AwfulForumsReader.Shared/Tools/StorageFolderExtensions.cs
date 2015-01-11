using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AwfulForumsReader.Tools
{
    public static class StorageFolderExtensions
    {
        public static async Task<bool> FileExistsAsync(this StorageFolder folder, string fileName)
        {
            try
            {
                await folder.GetFileAsync(fileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
