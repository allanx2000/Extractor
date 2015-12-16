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
                return "Reads the links from Slideshare users' Liked page. Accepts the raw exported HTML or a path it on a local drive.";
            }
        }

        public override bool CanExtract(InputData input)
        {
            return (File.Exists((string)input.Data) && input.DataType == DataType.FilePath)
                || input.DataType == DataType.Html;
        }

        public override List<Item> Extract(InputData input)
        {
            HtmlDocument doc = new HtmlDocument();

            string data = (string)input.Data;

            switch (input.DataType)
            {
                case DataType.FilePath:
                    if (!File.Exists(data))
                    {
                        throw new Exception("The entered path is invalid");
                    }

                    doc.Load(data);
                    break;
                case DataType.Html:
                    doc.LoadHtml(data);
                    break;
                default:
                    throw new NotSupportedException();
            }

            List<SlideShareItem> items = new List<SlideShareItem>();
            
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
