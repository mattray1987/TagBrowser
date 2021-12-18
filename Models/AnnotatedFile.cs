using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagBrowser.Models
{
    public class AnnotatedFile
    {
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Notes { get; set; }
        public bool IsPresent { get; set; }
        public ObservableCollection<Tag> Tags = new();

        internal bool ContainsTag(Tag tag)
        {
            if(Tags.Contains(tag)) return true;
            return false;
        }
    }
}
