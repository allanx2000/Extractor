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
    }
    
}
