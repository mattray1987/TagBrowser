using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TagBrowser.Models;
using TagBrowser.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TagBrowser.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FileListView : Page
    {
        public FileListView()
        {
            this.InitializeComponent();
        }

        public MainViewModel ViewModel => (Application.Current as App).ViewModel;

        private void OpenTagManagementMenu_Click(object sender, RoutedEventArgs e)
        {
            fileList.SelectedItem = ((FrameworkElement)e.OriginalSource).DataContext;
            tagSelectionList.SelectedItems.Clear();
            if(ViewModel.SelectedFile != null)
            {
               foreach(Tag tag in ViewModel.SelectedFile.Tags)
                {
                    tagSelectionList.SelectedItems.Add(tag);
                }
            }
            manageTagsFlyout.ShowAt(fileList);
        }

        private void SaveTagChanges_Click(object sender, RoutedEventArgs e)
        {
            if(ViewModel.SelectedFile != null)
            {
                ViewModel.SelectedFile.Tags.Clear();
                foreach(Tag tag in tagSelectionList.SelectedItems)
                {
                    ViewModel.SelectedFile.Tags.Add(tag);
                }
            }
        }
    }
}
