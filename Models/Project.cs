using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagBrowser.Utilities;
using Windows.Storage;

namespace TagBrowser.Models
{
    public class Project : BindableBase
    {
        public string Name { get; set; }
        public ObservableCollection<AnnotatedFile> AnnotatedFiles { get; set; } = new();
        public ObservableCollection<Tag> ProjectTags { get; set; } = new();
        public List<AnnotatedFile> FilesWithTag(Tag tag)
        {
            List<AnnotatedFile> files = new List<AnnotatedFile>();
            foreach(AnnotatedFile file in AnnotatedFiles)
            {
                if (file.ContainsTag(tag))
                {
                    files.Add(file);
                }
            }
            return files;
        }
        public bool ContainsRecord(StorageFile file)
        {
            foreach(AnnotatedFile annotatedFile in AnnotatedFiles)
            {
                if (annotatedFile.FilePath == file.Path)
                {
                    return true;
                }
            }
            return false;
        }

        private void AnnotatedFileAddedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (AnnotatedFile file in e.NewItems)
                {
                    foreach (Tag tag in file.Tags)
                    {
                        if (!ProjectTags.Contains(tag))
                        {
                            ProjectTags.Add(tag);
                        }
                    }
                }
            }
        }
        public Project()
        {
            AnnotatedFiles.CollectionChanged += AnnotatedFileAddedHandler;
        }
    }
}
