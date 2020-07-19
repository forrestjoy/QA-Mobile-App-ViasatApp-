using Newtonsoft.Json;
using System.Collections.ObjectModel;

/*
 * Model of a basic user as found in the database.
 * Each attribute is preceeded by a jsonProperty that defines how each attribute is called in the json string being sent/received
 * All the fields are populated after a succesfull login.
 */

namespace UserType
{
    public class UserModel
    {
        [JsonProperty("_id")]
        public string _id { get; set; }

        [JsonProperty("first_name")]
        public string name { get; set; }

        [JsonProperty("last_name")]
        public string lastName { get; set; }

        [JsonProperty("permission_level")]
        public int permission_level { get; set; }

        [JsonProperty("notes")]
        public ObservableCollection<string> personal_notes {get; set;} //array of note id's

        [JsonProperty("recently_viewed")]
        public ObservableCollection<string> recently_viewed { get; set; } //array of item id's

        [JsonProperty("favorites")]
        public ObservableCollection<string> favorites { get; set; } //array of item id's
    }
}