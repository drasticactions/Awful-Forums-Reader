using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AwfulForumsReader.Tools
{
    public class ConvertImage
    {
        public static async Task<byte[]> ConvertImagetoByte(StorageFile image)
        {
            IRandomAccessStream fileStream = await image.OpenAsync(FileAccessMode.Read);
            var reader = new DataReader(fileStream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)fileStream.Size);

            var pixels = new byte[fileStream.Size];

            reader.ReadBytes(pixels);

            return pixels;

        }
    }
}
