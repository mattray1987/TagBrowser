using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TagBrowser.Models;
using TagBrowser.Utilities;
using Windows.Storage;

namespace TagBrowser.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Fields
        private ObservableCollection<Story> AllStories { get; set; } = new();
        private ObservableCollection<Tag> AllTags { get; set; } = new();
        private ObservableCollection<Story> filteredStories = new();
        private ObservableCollection<Tag> filteredTags = new();
        private string searchTerm;
        private Tag selectedTag;
        private Story selectedStory;
        private StorageFolder lastFolder { get; set; }

        #endregion

        #region Properties
        public ObservableCollection<Story> FilteredStories
        {
            get => filteredStories;
            set
            {
                SetProperty(ref filteredStories, value);
            }
        }
        public ObservableCollection<Tag> FilteredTags
        {
            get => filteredTags;
            set 
            { 
                SetProperty(ref filteredTags, value); 
            }
        }
        public Tag SelectedTag
        {
            get => selectedTag;
            set
            {
                SetProperty(ref selectedTag, value);
                FilterStoriesByTag();
            }
        }
        public ICommand LoadFromDirectoryCommand { get; set; }
        public ICommand ReloadFromDirectoryCommand { get; set; }
        public string SearchTerm
        {
            get => searchTerm;
            set 
            { 
                SetProperty(ref searchTerm, value);
            }
        }
        public Story SelectedStory
        {
            get => selectedStory;
            set
            {
                SetProperty(ref selectedStory, value);
            }
        }

        #endregion

        #region Methods
        public void TagQuerySubmitted() 
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                SelectedTag = AllTags.FirstOrDefault(s => s.Name.Equals(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                SelectedTag = null;
            }
        }
        public void TagTextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e) 
        {
            if(e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                FilterTags();
            }
        }
        public void TagSuggestionChosen() 
        {
            
        }
        private void FilterStoriesByTag() 
        {
            FilteredStories.Clear();
            if (AllStories.Any())
            {
                if(SelectedTag != null)
                {
                    foreach(Story story in AllStories)
                    {
                        if (story.Tags.Contains(SelectedTag))
                        {
                            FilteredStories.Add(story);
                        }
                    }
                }
                else
                {
                    foreach(Story story in AllStories)
                    {
                        FilteredStories.Add(story);
                    }
                }
            } 
        }
        private void FilterTags()
        {
            FilteredTags.Clear();
            foreach(Tag tag in AllTags)
            {
                if(tag.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    FilteredTags.Add(tag);
                }
            }
        }
        private async void LoadStoryFromFile(StorageFile file)
        {
            string pattern = @"[tT][aA][gG][sS]\s*\:(.*)";
            Regex regex = new Regex(pattern);

            if (file != null && file.FileType == ".txt")
            {
                Story story = new Story();
                story.Name = file.DisplayName;
                story.FilePath = file.Path;
                string text = await FileIO.ReadTextAsync(file);
                var match = regex.Match(text);
                if (match.Success)
                {
                    string tagListString = match.Groups[1].Value;
                    List<string> tagsList = tagListString.Split(',').Select(s => s.Trim()).ToList();
                    foreach (string tagString in tagsList)
                    {
                        if (!string.IsNullOrWhiteSpace(tagString))
                        {
                            Tag tag = AllTags.FirstOrDefault(s => s.Name.ToLowerInvariant() == tagString.ToLowerInvariant());
                            if (tag == null)
                            {
                                tag = new Tag(tagString);
                                AllTags.Add(tag);
                            }
                            story.Tags.Add(tag);
                        }
                    }
                }
                AllStories.Add(story);
                FilteredStories.Add(story);
            }
        }
        private async void LoadFromDirectoryPickerAsync()
        {
            StorageFolder selectedFolder = await FileFolderPicker.PickFolderDiaglogAsync();
            if (selectedFolder != null)
            {
                // Save this for reloads
                lastFolder = selectedFolder;

                // Clear out the old data if there is any
                AllStories.Clear();
                AllTags.Clear();
                FilteredStories.Clear();
                FilteredTags.Clear();

                // Load in the new data from the selected folder
                IReadOnlyList<StorageFile> storageFiles = await selectedFolder.GetFilesAsync();
                foreach (StorageFile storageFile in storageFiles)
                {
                    if (storageFile != null && storageFile.FileType == ".txt")
                    {
                        LoadStoryFromFile(storageFile);
                    } 
                }
            }
        }

        private async void ReloadFromDirectory()
        {
            if (lastFolder != null)
            {
                // Clear out the old data if there is any
                AllStories.Clear();
                AllTags.Clear();
                FilteredStories.Clear();
                FilteredTags.Clear();

                // Load in the new data from the selected folder
                IReadOnlyList<StorageFile> storageFiles = await lastFolder.GetFilesAsync();
                foreach (StorageFile storageFile in storageFiles)
                {
                    if (storageFile != null && storageFile.FileType == ".txt")
                    {
                        LoadStoryFromFile(storageFile);
                    }
                }
            }
        }
        public void OpenStoryFile()
        {
            if(SelectedStory != null && File.Exists(SelectedStory.FilePath) && SelectedStory.FilePath.Contains(".txt"))
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = SelectedStory.FilePath;
                process.Start();
            }
        }
        public void OpenStoryClick(object sender, RoutedEventArgs e)
        {
            SelectedStory = (Story)(e.OriginalSource as FrameworkElement).DataContext;
            OpenStoryFile();
        }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            LoadFromDirectoryCommand = new RelayCommand(LoadFromDirectoryPickerAsync);
            ReloadFromDirectoryCommand = new RelayCommand(ReloadFromDirectory);
        }

        #endregion
    }
}
