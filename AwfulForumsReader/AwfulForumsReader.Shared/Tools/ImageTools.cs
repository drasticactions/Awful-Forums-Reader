using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using AwfulForumsReader.Core.Tools;

namespace AwfulForumsReader.Tools
{
    public static class ImageTools
    {
        public async static Task<bool> SaveWallpaper(byte[] img)
        {
            try
            {
                using (var streamWeb = new InMemoryRandomAccessStream())
                {
                    using (var writer = new DataWriter(streamWeb.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes(img);
                        await writer.StoreAsync();
                        var storageFolder = ApplicationData.Current.LocalFolder;
                        var file = await storageFolder.CreateFileAsync(Constants.WallpaperFilename, CreationCollisionOption.OpenIfExists);
                        var raStream = await file.OpenAsync(FileAccessMode.ReadWrite);

                        using (var thumbnailStream = streamWeb.GetInputStreamAt(0))
                        {
                            using (var stream = raStream.GetOutputStreamAt(0))
                            {
                                await RandomAccessStream.CopyAsync(thumbnailStream, stream);
                                await stream.FlushAsync();
                            }
                        }
                        raStream.Dispose();
                        await writer.FlushAsync();
                    }
                    await streamWeb.FlushAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
