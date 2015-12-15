using Extractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideShareLike
{
    public class SlideShareItem : Item
    {
        public const string Title = "Title";
        public const string URL = "URL";

        private static IReadOnlyList<string> keys = MakeReadOnly(Title, URL);
        
        public SlideShareItem(string title, string url)
        {
            this.SetKeyValue(Title, title);
            this.SetKeyValue(URL, url);
        }
        
        public override IReadOnlyList<string> GetKeys()
        {
            return keys;
        }
    }
}
