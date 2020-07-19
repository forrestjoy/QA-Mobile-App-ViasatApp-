using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http;

using ItemType;
using UserType;
using NoteType;

namespace Viasat_App
{
    public partial class ProfilePage : ContentPage
    {
        public UserModel user = new UserModel();
        public string responseString;
        public string requestString;
        public ObservableCollection<ItemModel> favoritesList = new ObservableCollection<ItemModel>();
        public ObservableCollection<NoteModel> notesList = new ObservableCollection<NoteModel>();


        public ProfilePage(UserModel theUser)
        {
            user = theUser;

            InitializeComponent();

            nameLabel.Text = user.name;
            lastLabel.Text = user.lastName;
            permissionLabel.Text = user.permission_level.ToString();
        }

        public async void recentlyViewedButton_Clicked(object sender, EventArgs e)
        {
            globals.Globals.recentlyViewedList.Clear();
            for (int i=0; i<user.recently_viewed.Count(); i++)
            {
                string itemId = user.recently_viewed[i];
                ItemModel tempItem = new ItemModel();
                tempItem.id = itemId;

                var jsonString = JsonConvert.SerializeObject(tempItem,
                                Newtonsoft.Json.Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });

                requestString = jsonString.ToLower();

                using (var httpClient = new HttpClient())
                {
                    var httpContent = new StringContent(requestString, Encoding.UTF8, "application/json");

                    var httpResponse = await httpClient.PostAsync("http://52.13.18.254:3000/searchbyid", httpContent);

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();

                        Console.WriteLine("JSON: " + requestString.ToString());
                        Console.WriteLine("POST: " + httpContent.ToString());
                        Console.WriteLine("GET: " + responseContent);

                        responseString = responseContent;
                    }
                }


                ObservableCollection<ItemModel> tempItem2 = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(responseString);
                ItemModel itemViewed = new ItemModel();

                if(tempItem2.Count > 0)
                {
                    itemViewed = tempItem2[0];
                    globals.Globals.recentlyViewedList.Add(itemViewed);
                }
            }

            string title = "Recently viewed";
            await Navigation.PushAsync(new ResultsPage(globals.Globals.recentlyViewedList, title));
        }

        public async void favoritesButton_Clicked(object sender, System.EventArgs e)
        {
            globals.Globals.favoritesItemsList.Clear();
            foreach(string itemId in globals.Globals.favoritesList)
            {
                ItemModel tempItem = new ItemModel();
                tempItem.id = itemId;

                var jsonString = JsonConvert.SerializeObject(tempItem,
                                Newtonsoft.Json.Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });

                requestString = jsonString.ToLower();

                using (var httpClient = new HttpClient())
                {
                    var httpContent = new StringContent(requestString, Encoding.UTF8, "application/json");
                    var httpResponse = await httpClient.PostAsync("http://52.13.18.254:3000/searchbyid", httpContent);
                    if(httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        responseString = responseContent;
                    }
                }

                List<ItemModel> tempItem2 = JsonConvert.DeserializeObject<List<ItemModel>>(responseString);
                globals.Globals.favoritesItemsList.Add(tempItem2[0]);
            }

            string title = "Favorites:";
            await Navigation.PushAsync(new ResultsPage(globals.Globals.favoritesItemsList, title));
        }

        private async void clearHistoryButton_Clicked(object sender, System.EventArgs e)
        {
            globals.Globals.TheUser.recently_viewed.Clear();
            globals.Globals.recentlyViewedList.Clear();
            Console.WriteLine("Hist list contents: " + globals.Globals.recentlyViewedList);
            UserModel tempUser = new UserModel();
            tempUser._id = globals.Globals.TheUser._id;

            var jsonString = JsonConvert.SerializeObject(tempUser,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            requestString = jsonString;

            using (var httpClient = new HttpClient())
            {
                var httpContent = new StringContent(requestString, Encoding.UTF8, "application/json");
                await httpClient.PostAsync("http://52.13.18.254:3000/deletehistory", httpContent);

            }

            //globals.Globals.recentlyViewedList.Clear();
        }

        private async void personalNotesButton_Clicked(object sender, System.EventArgs e)
        {
            notesList.Clear();
            NoteModel requestNote = new NoteModel();

            requestNote.belongs_to = globals.Globals.TheUser._id;

            var jsonString = JsonConvert.SerializeObject(requestNote,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            requestString = jsonString;

            using (var httpClient = new HttpClient())
            {
                var httpContent = new StringContent(requestString, Encoding.UTF8, "application/json");

                var httpResponse = await httpClient.PostAsync("http://52.13.18.254:3000/getnotes", httpContent);

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    responseString = responseContent;

                    //debugging
                    Console.WriteLine("JSON: " + requestString);
                    Console.WriteLine("POST: " + httpContent.ToString());
                    Console.WriteLine("GET: " + responseContent);
                }
            }
            var notesInArray = JsonConvert.DeserializeObject<ObservableCollection<NoteModel>>(responseString);

            notesList = notesInArray;

            string endpoint = "http://52.13.18.254:3000/addnoteuser";

            await Navigation.PushAsync(new CommentsPage(globals.Globals.TheUser._id, notesList, endpoint));
        }
    }
}

