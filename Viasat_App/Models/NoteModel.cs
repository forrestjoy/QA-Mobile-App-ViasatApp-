using Newtonsoft.Json;

/*
 * Model of a basic note as found in the database.
 * Each attribute is preceeded by a jsonProperty that defines how each attribute is called in the json string being sent/received
 */

namespace NoteType
{
    public class NoteModel
    {
        [JsonProperty("_id")]
        public string _id { get; set; }

        [JsonProperty("author_id")]
        public string author_id { get; set; }

        [JsonProperty("author")]
        public string author { get; set; }

        [JsonProperty("note")]
        public string note { get; set; }

        [JsonProperty("belongs_to")]
        public string belongs_to { get; set; }

        [JsonProperty("date")]
        public string date { get; set; }

    }
}