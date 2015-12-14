using System.Collections.Generic;

namespace Extractor.Models
{
    public abstract class Item
    {
        private Dictionary<string, object> data = new Dictionary<string, object>();

        protected void SetKeyValue(string key, object value)
        {
            data[key] = value;
        }

        public object GetValue(string key)
        {
            if (!data.ContainsKey(key))
                return null;
            else
                return data[key];
        }

        public abstract IReadOnlyList<string> GetKeys();
    }
}