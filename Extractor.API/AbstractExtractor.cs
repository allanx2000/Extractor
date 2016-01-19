using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Models
{
    public abstract class AbstractExtractor
    {
        public virtual bool CanExtract(InputData input)
        {
            return input != null && ValidTypes.Contains(input.DataType);
        }

        public abstract List<Item> Extract(InputData input);
        
        public abstract string Description { get; }
        public abstract ICollection<DataType> ValidTypes { get; }

        protected static List<Item> ToItemList<T>(List<T> items) where T : Item
        {
            return items.Cast<Item>().ToList();
        }

        public abstract IReadOnlyList<string> GetItemKeys();
    }
    
}
