using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Models
{
    public class InputData
    {

        public object Data { get; private set; }
        public DataType DataType { get; private set; }

        public InputData(DataType type, object data)
        {
            this.Data = data;
            this.DataType = type;
        }
    }
}
