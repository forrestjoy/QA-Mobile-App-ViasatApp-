using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http;
using ItemType;
using favType;


namespace Viasat_App
{
    public partial class ResultsPage : ContentPage
    {
        public ObservableCollection<Parameter> parametersList = new ObservableCollection<Parameter>();
        public string requestString;
        public string responseString;

        public ResultsPage(ObservableCollection<ItemModel> itemList, string title)
        {
            InitializeComponent();
            ResultsListView.ItemsSource = itemList;
            Title = title;
        } 

        //START: BUTTONS EVENTS #######################################################

        private async void itemEntry_Tapped(object sender, ItemTappedEventArgs e)
        {


            //Creating an object of type ItemModel 
            ItemModel item = (ItemModel)((ListView)sender).SelectedItem;
            ((ListView)sender).SelectedItem = null;

            string itemId = item.id;
            ItemModel tempItem = new ItemModel();
            tempItem.id = itemId;


            //item serialized to be sent as the request to the API
            //handles parameters not entered by the user, that way they are not included in the json string so the API doesn't have to parse and check for nulls.
            var jsonString = JsonConvert.SerializeObject(tempItem,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            requestString = jsonString;

            //Creating the http client which will provide us with the network capabilities
            using (var httpClient = new HttpClient())
            {
                //request string to be sent to the API
                var httpContent = new StringContent(requestString, Encoding.UTF8, "application/json");

                //sending the previously created request to the api and waiting for a response that will be saved in the httpResponse var
                //  NOTE: if the api's base url changes this has to be modified.
                var httpResponse = await httpClient.PostAsync("http://52.13.18.254:3000/searchbyid", httpContent);


                //to visualize the json sent over the network comment the previous line, uncomment the next one and go to the link.
                //var httpResponse = await httpClient.PostAsync("https://putsreq.com/qmumqAwIq9s5RBEfbNfh", httpContent);

                //verifying that response is not empty
                if (httpResponse.Content != null)
                {
                    //response into a usable var
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    //debugging
                    Console.WriteLine("JSON: " + requestString);
                    Console.WriteLine("POST: " + httpContent.ToString());
                    Console.WriteLine("GET: " + responseContent);

                    responseString = responseContent;
                }
            }

            var itemsList = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(responseString);
            var itemReceived = itemsList[0];

            FavModel itemViewed = new FavModel();

            itemViewed.id = globals.Globals.TheUser._id;
            itemViewed.item_id = itemId;


            jsonString = JsonConvert.SerializeObject(itemViewed,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            requestString = jsonString;

            using (var httpClient = new HttpClient())
            {
                var httpContent = new StringContent(requestString, Encoding.UTF8, "application/json");

                var httpResponse = await httpClient.PostAsync("http://52.13.18.254:3000/itemviewed", httpContent);
            }

            globals.Globals.TheUser.recently_viewed.Add(itemReceived.id);


            //calling the ItemPage into the stack and passing the selected item by the user
            await Navigation.PushAsync(new ItemPage(itemReceived));
        }
    }
}