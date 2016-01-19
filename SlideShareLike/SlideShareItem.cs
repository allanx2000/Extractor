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

        internal static IReadOnlyList<string> Keys = MakeReadOnly(Title, URL);
        
        public SlideShareItem(string title, string url)
        {
            this.SetKeyValue(Title, title);
            this.SetKeyValue(URL, url);
        }

        public override string GetDefaultText()
        {
            return "<a href='" + GetValue(URL) + "'>" + GetValue(Title) + "</a>";
        }

        public override IReadOnlyList<string> GetKeys()
        {
            return Keys;
        }
    }
}
