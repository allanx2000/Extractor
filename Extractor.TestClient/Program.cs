using Extractor.Models;
using SlideShareLike;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEDExtractor;

namespace Extractor.TestClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            //TestTED();

            TestSlideShare();
        }

        private static void TestSlideShare()
        {
            string path = @"C:\Users\Allan\Desktop\My favorites on SlideShare.html";
            
            AbstractExtractor extractor = new SlideShareLike.SlideShareLikeExtractor();

            var items = extractor.Extract(new InputData(DataType.FilePath, path));

            StreamWriter sw = new StreamWriter("linked.txt");

            foreach (var i in items)
            {
                string line = MakeLink((string)i.GetValue(SlideShareItem.URL), (string)i.GetValue(SlideShareItem.Title), true);
                sw.WriteLine(line);
            }

            sw.Close();
        }

        private const string LinkFormat = "<a href='{0}'>{1}</a>";

        private static string MakeLink(string url, string title, bool wrapWithLI)
        {
            string link = String.Format(LinkFormat, url, title);

            if (wrapWithLI)
                link = "<li>" + link + "</li>";

            return link;
        }

        private static void TestTED()
        {
            string path = @"C:\Videos\TED Watched";

            string[] files = Directory.GetFiles(path);

            string input = String.Join(Environment.NewLine, files);

            AbstractExtractor extractor = new TEDExtractor.TEDExtractor();

            var items = extractor.Extract(new InputData(DataType.PlainText, input));

            StreamWriter sw = new StreamWriter("out.txt");
            foreach (var item in items)
            {
                sw.WriteLine(item.GetValue(TEDItem.FormattedLink));
            }

            sw.Close();
        }
    }
}
