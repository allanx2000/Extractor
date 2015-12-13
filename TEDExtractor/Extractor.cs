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
    public class Extractor : AbstractExtractor
    {
        public override bool CanExtract(InputData input)
        {
            return input.DataType == DataType.PlainText;
        }

        public override List<Item> Extract(InputData input)
        {
            List<Item> results = new List<Item>();

            List<ID> ids = GetIDs((string) input.Data);
            
            foreach (var id in ids)
            {
                string url = SearchUrl + ToQuery(id.Name);

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

                    if (monthyear[1] == id.Year)
                    {
                        finalTitle = title;
                        finalUrl = link;
                        break;
                    }
                }
                
                if (finalUrl != null)
                {
                    string fullLink = String.Format(OutputFormat, finalUrl, finalTitle);
                    results.Add(new Item(fullLink));
                }
            }


            return results;
        }

        private string ToQuery(string name)
        {
            return name.Replace(" ", "+");
        }

        struct ID
        {
            public string Name { get; set; }
            public string Year { get; set; }

        }


        private const string OutputFormat = "<a href='{0}'>{1}</a>";
        private const string BaseUrl = "http://www.ted.com";
        private const string SearchUrl = BaseUrl + "/search?cat=talks&q=";

        private List<ID> GetIDs(string data)
        {
            List<ID> ids = new List<ID>();

            StringReader sr = new StringReader(data);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
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

                var id = new ID() {
                    Name = name.ToString(),
                    Year = parts[1]
                };

                ids.Add(id);
            }

            return ids;
            
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
    }
}
