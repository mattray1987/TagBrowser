using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace TagBrowser.Utilities
{
    public static class FileFolderPicker
    {
        public static async Task<StorageFolder> PickFolderDiaglogAsync()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle((Application.Current as App).MainWindow);
            var folderPicker = new FolderPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            return folder;
        }
    }
}
