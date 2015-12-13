namespace Extractor.Models
{
    public class Item
    {
        public object Data { get; private set; }
        public DataType Type { get; private set; }


        public Item(object data)
        {
            Data = data;
        }
    }
}