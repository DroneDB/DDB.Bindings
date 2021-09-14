using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace DDB.Bindings.Model
{
    public class MetaListItem
    {
        public int Count { get; set; }
        public string Key { get; set; }

        public string Path { get; set; }
    }
}