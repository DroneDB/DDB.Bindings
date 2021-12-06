﻿using Newtonsoft.Json;

namespace DDB.Bindings.Model
{
    public class AddAction
    {
        [JsonProperty("path")]
        public string Path { get;  }

        [JsonProperty("hash")]
        public string Hash { get;  }

        public AddAction(string path, string hash)
        {
            Path = path;
            Hash = hash;
        }

        public override string ToString()
        {
            return $"ADD -> [{(string.IsNullOrEmpty(Hash) ? 'D' : 'F')}] {Path}";
        }
    }
}