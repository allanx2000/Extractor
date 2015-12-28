using Extractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEDExtractor
{
    public class TEDItem : Item
    {
        public const string Speaker = "Speaker";
        public const string Year = "Year";
        public const string FormattedLink = "FormattedLink";
        public const string RawLink = "RawLink";
        public const string Title = "Title";


        public override string GetDefaultText()
        {
            return (string) GetValue(FormattedLink);
        }

        public TEDItem(string speaker, string year, string rawLink = null, string formattedLink = null)
        {
            this.SetKeyValue(Speaker, speaker);
            this.SetKeyValue(Year, year);
            this.SetKeyValue(RawLink, rawLink);
            this.SetKeyValue(FormattedLink, formattedLink);
        }

        public void SetRawLink(string link)
        {
            this.SetKeyValue(RawLink, link);
        }

        public void SetFormattedLink(string link)
        {
            this.SetKeyValue(FormattedLink, link);
        }

        public void SetSpeaker(string name)
        {
            SetKeyValue(Speaker, name);
        }

        public void SetTitle(string title)
        {
            SetKeyValue(Title, title);
        }

        private static IReadOnlyList<string> keys = MakeReadOnly(Speaker, Year, RawLink, FormattedLink);

        public override IReadOnlyList<string> GetKeys()
        {
            return keys;
        }
        
    }
}
