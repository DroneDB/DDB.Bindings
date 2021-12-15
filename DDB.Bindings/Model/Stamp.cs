using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace DDB.Bindings.Model
{

    public class Stamp
    {
        [JsonProperty("checksum")]
        public string Checksum { get; set; }

        [JsonProperty("entries")]
        public List<Dictionary<string,string>> Entries { get; set; }

        
    }

}
