using UserType;
using ItemType;
using System.Collections.ObjectModel;

namespace globals
{
    /*
     *  This file contains all the global variables used throught the app
     *  All this lists are filled upon successful login
     */

    public class Globals
    {
        public static UserModel TheUser { get; set; } //contains the information of the user
        public static ObservableCollection<ItemModel> recentlyViewedList { get; set; } //contains the complete object for every item in the user's history array
        public static ObservableCollection<string> favoritesList { get; set; } //ids of the favorite items
        public static ObservableCollection<ItemModel> favoritesItemsList { get; set; } //contains the complete object for every item in the user's favoritesList
    }

}