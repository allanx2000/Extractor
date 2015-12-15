using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Models
{
    public abstract class AbstractExtractor
    {
        public abstract bool CanExtract(InputData input);

        public abstract List<Item> Extract(InputData input);
        
        public abstract string Description { get; }

        protected static List<Item> ToItemList<T>(List<T> items) where T : Item
        {
            return items.Cast<Item>().ToList();
        } 
    }
    
}
