using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TagBrowser.Models;
using TagBrowser.Utilities;
using TagBrowser.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TagBrowser
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        internal ObservableCollection<Story> AllStories = new();

        public MainWindow()
        {
            this.InitializeComponent();
        }
        MainViewModel ViewModel => (Application.Current as App).ViewModel;

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenStoryClick(sender,e);
        }
    }
}
