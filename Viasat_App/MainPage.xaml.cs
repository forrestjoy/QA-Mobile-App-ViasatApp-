using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.ObjectModel;

using UserType;
using ItemType;
using NoteType;

namespace Viasat_App
{
    public partial class MainPage : ContentPage
    {
        UserModel theUser;
        public string responseString;
        public string requestString;
        ObservableCollection<NoteModel> notesList = new ObservableCollection<NoteModel>();

        public MainPage(UserModel user)
        {
            theUser = user;
            InitializeComponent();
        }

        //START: BUTTONS EVENTS ########################################################

        private async void searchButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        private async void profileButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(theUser));
        }

        private async void historyButton_Clicked(object sender, System.EventArgs e)
        {
            globals.Globals.recentlyViewedList.Clear();
            for (int i = 0; i < theUser.recently_viewed.Count; i++)
            {
                string itemId = theUser.recently_viewed[i];
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

                if (tempItem2.Count > 0)
                {
                    itemViewed = tempItem2[0];
                    globals.Globals.recentlyViewedList.Add(itemViewed);
                }
            }
            string title = "Recently viewed";
            await Navigation.PushAsync(new ResultsPage(globals.Globals.recentlyViewedList, title));
        }

        private async void favoritesButton_Clicked(object sender, System.EventArgs e)
        {
            globals.Globals.favoritesItemsList.Clear();
            foreach (string itemId in globals.Globals.TheUser.favorites)
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
                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        responseString = responseContent;
                    }
                }

                List<ItemModel> tempItem2 = JsonConvert.DeserializeObject<List<ItemModel>>(responseString);
                globals.Globals.favoritesItemsList.Add(tempItem2[0]);
            }

            string title = "Favorites";
            await Navigation.PushAsync(new ResultsPage(globals.Globals.favoritesItemsList, title));
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