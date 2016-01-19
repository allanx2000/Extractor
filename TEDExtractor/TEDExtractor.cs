using Extractor.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TEDExtractor
{
    public class TEDExtractor : AbstractExtractor
    {
        public override string Description
        {
            get
            {
                return "Accepts either list of TED video files or path containing them. Then downloads their relevant data from TED.";
            }
        }

        private List<DataType> valid;
        public override ICollection<DataType> ValidTypes
        {
            get
            {
                if (valid == null)
                    valid = new List<DataType>()
                    {
                        DataType.PlainText,
                        DataType.FolderPath
                    };

                return valid;
            }
        }

        public override List<Item> Extract(InputData input)
        {
            List<TEDItem> ids = CreateItemsFromData(input);
            
            foreach (var id in ids)
            {
                string url = SearchUrl + ToQuery((string)id.GetValue(TEDItem.Speaker));

                WebClient client = GetWebClient();

                string page = client.DownloadString(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(page);

                var items = doc.DocumentNode.SelectNodes("//article//h3/a");

                string finalUrl = null, finalTitle = null;
                foreach (var i in items)
                {
                    string link = BaseUrl + i.Attributes["href"].Value;
                    string title = i.InnerText;

                    page = client.DownloadString(link);
                    int start = page.IndexOf('>', page.IndexOf("Filmed<"));
                    int end = page.IndexOf('<', start + 1);
                    string[] monthyear = page.Substring(start, end - start).Trim().Split(' ');

                    if (monthyear[1] == (string) id.GetValue(TEDItem.Year))
                    {
                        finalTitle = title;
                        finalUrl = link;
                        break;
                    }
                }
                
                if (finalUrl != null)
                {
                    string fullLink = String.Format(OutputFormat, finalUrl, finalTitle);
                    id.SetFormattedLink(fullLink);
                    id.SetRawLink(finalUrl);
                    id.SetTitle(finalTitle);
                }
            }


            return ToItemList(ids); //Probably should just move them to a List<Item> instead
        }

        private string ToQuery(string name)
        {
            return name.Replace(" ", "+");
        }

        

        private const string OutputFormat = "<a href='{0}'>{1}</a>";
        private const string BaseUrl = "http://www.ted.com";
        private const string SearchUrl = BaseUrl + "/search?cat=talks&q=";

        private List<TEDItem> CreateItemsFromData(InputData data)
        {
            List<string> files = new List<string>();

            string text = (string) data.Data.ToString();

            if (data.DataType == DataType.FolderPath)
            {
                files.AddRange(Directory.GetFiles(data.Data.ToString()));
            }
            else //Plaintext files
            {
                StringReader sr = new StringReader(text);

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    files.Add(line);
                }

            }

            List<TEDItem> items = new List<TEDItem>();

            foreach (string path in files)
            {
                string line = path;

                line = line.Substring(line.LastIndexOf('\\') + 1);
                line = line.Substring(0, line.IndexOf('-'));
                string[] parts = line.Split('_');

                if (parts.Length < 2)
                    throw new Exception("Incorrect format");

                //Fix Year string
                if (parts[1].Length > 4)
                    parts[1] = parts[1].Substring(0, 4);

                //Space the name
                string upper = parts[0].ToUpper();

                StringBuilder name = new StringBuilder();
                name.Append(upper[0]);

                for (int i = 1; i < upper.Length; i++)
                {
                    char c = parts[0][i];
                    if (c == upper[i])
                    {
                        name.Append(" ");
                    }

                    name.Append(c);
                }

                var id = new TEDItem(name.ToString(), parts[1]);

                items.Add(id);
            }

            return items;
            
        }

        private WebClient client;

        private WebClient GetWebClient()
        {
            if (client == null)
            {
                client = new WebClient();
            }

            return client;
        }

        public override IReadOnlyList<string> GetItemKeys()
        {
            return TEDItem.Keys;
        }
    }
}
