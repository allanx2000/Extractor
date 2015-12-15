using Extractor.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideShareLike
{
    public class SlideShareLikeExtractor : AbstractExtractor
    {
        public override string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanExtract(InputData input)
        {
            return input.DataType == DataType.FilePath && File.Exists((string)input.Data);
        }

        public override List<Item> Extract(InputData input)
        {
            string htmlPath = (string)input.Data;
            if (!File.Exists(htmlPath))
            {
                throw new Exception("The entered path is invalid");
            }

            List<SlideShareItem> items = new List<SlideShareItem>();

            HtmlDocument doc = new HtmlDocument();
            doc.Load(htmlPath);

            var nodes = doc.DocumentNode.SelectNodes("//div[@id='slideshows']/ul/li");


            foreach (var node in nodes)
            {
                var link = node.SelectSingleNode(".//a");

                string title = link.Attributes["title"].Value;
                string url = link.Attributes["href"].Value;

                items.Add(new SlideShareItem(title, url));
            }

            return ToItemList(items);
        }   
    }
}
