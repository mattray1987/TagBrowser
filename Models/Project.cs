using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TagBrowser.Utilities;
using Windows.Storage;

namespace TagBrowser.Models
{
    public class Project : BindableBase
    {
        public string Name { get; set; }
        public ObservableCollection<AnnotatedFile> AnnotatedFiles { get; set; } = new();
        [JsonInclude]
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
        public Project()
        {

        }
    }
}
