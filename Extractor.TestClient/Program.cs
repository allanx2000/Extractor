using Extractor.Models;
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
