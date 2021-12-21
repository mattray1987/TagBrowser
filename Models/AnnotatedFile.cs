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
        public string ShortName
        { 
            get 
            { 
                if (DisplayName.Length > 20)
                {
                    return DisplayName.Substring(0,20) + "...";
                }
                return DisplayName;
            } 
        }

        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Notes { get; set; }
        public bool IsPresent { get; set; }
        [JsonInclude]
        public ObservableCollection<Tag> Tags = new();
        public string TagsString 
        {
            get 
            {
                if (Tags.Count > 0)
                {
                    string tempString = "";
                    foreach (Tag tag in Tags)
                    {
                        tempString += tag.Name + ", ";
                    }
                    tempString = tempString.Remove(tempString.Length - 2);
                    return tempString;
                }
                return "";
            }
        }

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
        public AnnotatedFile()
        {
            Tags.CollectionChanged += (o,e) => OnPropertyChanged(nameof(TagsString));
        }

    }
}
