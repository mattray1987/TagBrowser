using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagBrowser.Models
{
    public class Story
    {
        internal string Name { get; set; }
        internal string FilePath { get; set; }
        internal string Description { get; set; }
        internal ObservableCollection<Tag> Tags = new();

    }
}
