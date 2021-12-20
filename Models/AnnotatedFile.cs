using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TagBrowser.Utilities;

namespace TagBrowser.Models
{
    public class AnnotatedFile : BindableBase
    {
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Notes { get; set; }
        public bool IsPresent { get; set; }
        [JsonInclude]
        public ObservableCollection<Tag> Tags = new();

        internal bool ContainsTag(Tag tag)
        {
            foreach(Tag existingTag in Tags)
            {
                if (existingTag.Name == tag.Name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
