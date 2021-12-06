using Newtonsoft.Json;

namespace DDB.Bindings.Model
{
    public class Delta
    {
        [JsonProperty("adds")]
        public AddAction[] Adds { get; set; }

        [JsonProperty("removes")]
        public RemoveAction[] Removes { get; set; }
    }
}