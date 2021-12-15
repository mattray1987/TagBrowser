using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagBrowser.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public Tag(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
        public Tag() { }
    }
}
