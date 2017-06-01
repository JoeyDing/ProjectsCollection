using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoWorkItemsSync.Core
{
    public class Tag
    {
        public static string FilterTag(string tags)
        {
            HashSet<string> hash_tag = new HashSet<string>();
            string[] tagsArray = tags.Split(';');
            foreach (var tagName in tagsArray)
            {
                hash_tag.Add(tagName.Trim());
            }
            tags = "";
            foreach (var tagName in hash_tag)
            {
                tags += tagName + ";";
            }
            tags = tags.Remove(tags.Length - 1, 1);
            return tags;
        }
    }
}