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
        private AppData appData = new AppData();
        private Project selectedProject;
        private ObservableCollection<AnnotatedFile> filteredFiles = new();
        private ObservableCollection<Tag> filteredTags = new();
        private string searchTerm;
        private Tag selectedTag;
        private AnnotatedFile selectedFile;
        private ObservableCollection<Project> projectFolders = new();
        private StorageFolder selectedFolder { get; set; }

        #endregion

        #region Properties
        public ObservableCollection<Project> ProjectFolders { get { return projectFolders; } } 
        public Project SelectedProject { 
            get { return selectedProject; } 
            set 
            { 
                SetProperty(ref selectedProject, value); 
                FilterFilesByTag();
                FilterTags();
            } 
        }
        public ObservableCollection<AnnotatedFile> FilteredFiles
        {
            get => filteredFiles;
            set
            {
                SetProperty(ref filteredFiles, value);
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
                FilterFilesByTag();
            }
        }
        public ICommand LoadWithPickerCommand { get; set; }
        public ICommand ReloadProjectCommand { get; set; }
        public ICommand SaveProjectCommand { get; set; }
        public string SearchTerm
        {
            get => searchTerm;
            set 
            { 
                SetProperty(ref searchTerm, value);
            }
        }
        public AnnotatedFile SelectedFile
        {
            get => selectedFile;
            set
            {
                SetProperty(ref selectedFile, value);
            }
        }

        #endregion

        #region Methods
        public void TagQuerySubmitted() 
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                SelectedTag = SelectedProject.ProjectTags.FirstOrDefault(s => s.Name.Equals(SearchTerm, StringComparison.OrdinalIgnoreCase));
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
        private void FilterFilesByTag() 
        {
            FilteredFiles.Clear();
            if (SelectedProject != null && SelectedProject.AnnotatedFiles.Any())
            {
                if(SelectedTag != null)
                {
                    foreach(AnnotatedFile file in SelectedProject.AnnotatedFiles)
                    {
                        if (file.ContainsTag(SelectedTag))
                        {
                            FilteredFiles.Add(file);
                        }
                    }
                }
                else
                {
                    foreach(AnnotatedFile file in SelectedProject.AnnotatedFiles)
                    {
                        FilteredFiles.Add(file);
                    }
                }
            } 
        }
        private void FilterTags()
        {
            FilteredTags.Clear();
            if(SelectedProject != null && SelectedProject.ProjectTags != null)
            {
                foreach (Tag tag in SelectedProject.ProjectTags)
                {
                    if (tag.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    {
                        FilteredTags.Add(tag);
                    }
                }
            }
        }
       
        private async void LoadFromDirectoryPickerAsync()
        {
            StorageFolder selectedFolder = await FileFolderPicker.PickFolderDiaglogAsync();
            if (selectedFolder != null)
            {
                this.selectedFolder = selectedFolder;

                LoadProjectAsync();
            }
        }

        private async void LoadProjectAsync()
        {
            if (selectedFolder != null)
            {
                // Clear out the old data if there is any
                FilteredFiles.Clear();
                FilteredTags.Clear();
                SelectedProject = null;
                Project projectRecord;

                // Look for an existing project file
                StorageFile projectFile = (StorageFile)await selectedFolder.TryGetItemAsync(appData.DataStorageFileName);
                if (projectFile == null)
                {
                    projectRecord = new Project();
                }
                else
                {
                    string encodedData = await FileIO.ReadTextAsync(projectFile);
                    projectRecord = System.Text.Json.JsonSerializer.Deserialize<Project>(encodedData);
                }

                // Get all the files in the folder
                IReadOnlyList<StorageFile> files = await selectedFolder.GetFilesAsync();

                // If we don't have a record for a file, create a new one
                foreach (StorageFile file in files)
                {
                    if (file.Name != appData.DataStorageFileName && !projectRecord.AnnotatedFiles.Select(s => s.FilePath).Contains(file.Path))
                    {
                        AnnotatedFile annotatedFile = new AnnotatedFile();
                        annotatedFile.FilePath = file.Path;
                        annotatedFile.DisplayName = file.DisplayName;
                        annotatedFile.FileName = file.Name;
                        projectRecord.AnnotatedFiles.Add(annotatedFile);
                    }
                }

                // Now let's check if anything has been deleted
                foreach (AnnotatedFile annotatedFile in projectRecord.AnnotatedFiles)
                {
                    if(!files.Select(x => x.Path).Contains(annotatedFile.FilePath))
                    {
                        annotatedFile.IsPresent = false;
                    }
                }

                // Select the project
                SelectedProject = projectRecord;
            }
        }
        public async void SaveProjectAsync()
        {
            if (SelectedProject != null && selectedFolder != null)
            {
                string encodedData = System.Text.Json.JsonSerializer.Serialize(SelectedProject);

                StorageFile dataFile = await (selectedFolder).CreateFileAsync(appData.DataStorageFileName, CreationCollisionOption.OpenIfExists);
                if (encodedData != null)
                {
                    await FileIO.WriteTextAsync(dataFile, encodedData);
                }
            }
        }
        public void OpenFile()
        {
            if(SelectedFile != null && File.Exists(SelectedFile.FilePath))
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = SelectedFile.FilePath;
                process.Start();
            }
        }
        public void OpenFileClick(object sender, RoutedEventArgs e)
        {
            SelectedFile = (AnnotatedFile)(e.OriginalSource as FrameworkElement).DataContext;
            OpenFile(); 
        }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            LoadWithPickerCommand = new RelayCommand(LoadFromDirectoryPickerAsync);
            ReloadProjectCommand = new RelayCommand(LoadProjectAsync);
            SaveProjectCommand = new RelayCommand(SaveProjectAsync);
        }

        #endregion
    }
}
