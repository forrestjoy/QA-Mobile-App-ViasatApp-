using Newtonsoft.Json;

/*
 *  This model is needed when a user favorites or unfavorites an item.
 *  We create an object of this type in which:
 *      -id is the id of the user that is making the call
 *      -item_id is the id of the item that the user is adding/removing from their favorite list.
 */

namespace favType
{
    public class FavModel
    {
        [JsonProperty("_id")]
        public string id { get; set; }

        [JsonProperty("item_id")]
        public string item_id { get; set; }
    }
}