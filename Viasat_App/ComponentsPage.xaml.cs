using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Forms;

using ItemType;

namespace Viasat_App
{
    public partial class ComponentsPage : ContentPage
    {
        public ComponentsPage(List<string> componentsList)
        {
            InitializeComponent();
            ComponentsListView.ItemsSource = componentsList;
        }

        private async void componentEntry_Tapped(object sender, ItemTappedEventArgs e)
        {
            //Creating an object of type ItemModel 
            string selectedComponent = (string)((ListView)sender).SelectedItem;
            ((ListView)sender).SelectedItem = null;

            ItemModel item = new ItemModel();

            //setting up endpoint
            string endpointSt = "http://enriqueae.com/ViasatTest/json4.json";
            Uri apiUri = new Uri(endpointSt);

            //creating a http client to handle the async data retreival
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(apiUri);

            //response stream into a string
            string jsonContent = await response.Content.ReadAsStringAsync();

            //parsing from json string to a list of objects of our item model type
            var itemsList = JsonConvert.DeserializeObject<List<ItemModel>>(jsonContent);

            for(int i=0; i<itemsList.Count; i++)
            {
                if(itemsList[i].item_number.ToString() == selectedComponent)
                {
                    item = itemsList[i];
                }
            }

            //calling the ItemPage into the stack and passing the selected item by the user
            await Navigation.PushAsync(new ItemPage(item));
        }
    }
}
