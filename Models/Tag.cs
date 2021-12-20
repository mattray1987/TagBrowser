using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagBrowser.Utilities;

namespace TagBrowser.Models
{
    public class Tag : BindableBase
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public Tag() { }
        public Tag(string name)
        {
            Name = name;
        }
        public override bool Equals(object obj)
        {
            if(obj.GetType() == typeof(Tag) && (obj as Tag).Name == this.Name)
            {
                return true;
            }
            return false;
        }
    }
}
