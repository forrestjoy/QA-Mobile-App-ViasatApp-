using Newtonsoft.Json;
using System.Collections.ObjectModel;

/*
 * Model of a basic item as found in the database.
 * Each attribute is preceeded by a jsonProperty that defines how each attribute is called in the json string being sent/received
 */

namespace ItemType
{
    public class ItemModel
    {
        [JsonProperty("_id")]
        public string id { get; set; }

        [JsonProperty("item_number")]
        public int? item_number { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("part_type")]
        public string part_type { get; set; }

        [JsonProperty("permission_level")]
        public int? permission_level { get; set; }

        [JsonProperty("revision")]
        public int? revision { get; set; }

        [JsonProperty("components")]
        public ObservableCollection<string> componentsIDs { get; set; }
    }

}