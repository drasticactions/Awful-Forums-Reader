﻿using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands.Posts
{
    public class ImgurAddImageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var replyText = parameter as TextBox;
            if (replyText == null)
            {
                return;
            }
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".gif");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null) return;
            await AddImgurImage(file, replyText);
        }

        public async Task AddImgurImage(StorageFile file, TextBox replyText)
        {
            if (file == null)
            {
                return;
            }
            if (!file.ContentType.Contains("image"))
            {
                return;
            }
            var stream = await file.OpenAsync(FileAccessMode.Read);
            var result = await UploadManager.UploadImgur(stream);
            if (result == null)
            {
                var msgDlg = new MessageDialog("Something went wrong with the upload. :-(.");
                msgDlg.ShowAsync();
                return;
            }

            // We have got an image up on Imgur! Time to get it into the reply box!

            string imgLink = $"[TIMG]{result.data.link}[/TIMG]";
            replyText.Text = replyText.Text.Insert(replyText.Text.Length, imgLink);
        }
    }
}
