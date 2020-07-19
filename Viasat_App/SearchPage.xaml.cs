using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Net.Http;
using ItemType;

//external info:
//https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
//https://putsreq.com/ZIailWh2iEVMAOP0RdGr/inspect  <---- for testing the http requests


namespace Viasat_App
{
    public partial class SearchPage : ContentPage
    {
        public ObservableCollection<Parameter> parametersList = new ObservableCollection<Parameter>();
        public string requestString;
        public string responseString;

        public SearchPage()
        {   
            InitializeComponent();
            ParameterListView.ItemsSource = parametersList;
        }

        //START: BUTTONS EVENTS #######################################################

        //PURPOSE: upon user click the app sends a request to the API and waits for a response and handles the incoming data accordingly:
        //PARAMETERS: navigation params
        //ALGORITHM:
        //  -API endpoint is created in a string according to the search parameters 
        //  -HTTP client is created and a POST request is sent (POST because the parameters are in the body instead of in the url)
        //  -App waits for a response in a JSON format
        //  -Response is checked for errors. If success data is parsed into an object of type: ItemModel
        private async void resultsButton_Clicked(object sender, EventArgs e)
        {
            //======================================================================================================
            //======================================================================================================
            //======================================================================================================
            createRequest(parametersList);

            //Creating the http client which will provide us with the network capabilities
            using (var httpClient = new HttpClient())
            {
                //request string to be sent to the API
                var httpContent = new StringContent(requestString, Encoding.UTF8, "application/json");

                //sending the previously created request to the api and waiting for a response that will be saved in the httpResponse var
                //  NOTE: if the api's base url changes this has to be modified.
                var httpResponse = await httpClient.PostAsync("http://52.13.18.254:3000/partialobj", httpContent);

                //to visualize the json sent over the network comment the previous line, uncomment the next one and go to the link.
                // var httpResponse = await httpClient.PostAsync("https://putsreq.com/ZIailWh2iEVMAOP0RdGr/", httpContent);

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
            //======================================================================================================
            //======================================================================================================
            //======================================================================================================

            //parsing from json string to a list of objects of our item model type

            var itemsList = JsonConvert.DeserializeObject<ObservableCollection<ItemModel>>(responseString);

            //reset the parameters list for a future serch
            parametersList.Clear();

            string title = "Search Results";
            //open the results page and pass the list of items to populate it
            await Navigation.PushAsync(new ResultsPage(itemsList, title));
        }

        void addParameterButton_Clicked(object sender, System.EventArgs e)
        {
            Parameter tempParam = new Parameter();

            tempParam.key = ParametersPicker.SelectedItem.ToString();
            tempParam.value = ParameterEntry.Text;

            if (!parametersList.Any(p => p.key == tempParam.key))
            {
                parametersList.Add(tempParam);
                ParameterEntry.Text = "";
            }
            else
            {
                DisplayAlert("Duplicate parameter", "The parameter has already been selected for this search.", "Ok");
            }
        }


        //PURPOSE: to use the parameters entered by the user and create a custom request json string
        //PARAMETERS: parameters list
        public void createRequest(ObservableCollection<Parameter> list)
        {
            //temporary object which will hold all the search parameters
            //this object is to be serizlized into a json string to be sent as a request to the API
            ItemModel tempItem = new ItemModel();
            //tempItem.permission_level = globals.Globals.TheUser.permission_level;

            //Loop to go through all the parameters entered by the user and put them into the object's variables
            foreach(Parameter param in list)
            {
                if(param.key == "ID")
                {
                    tempItem.id = param.value;
                }
                else if(param.key == "Item Number")
                {
                    tempItem.item_number = Convert.ToInt32(param.value);
                }
                else if(param.key == "Revision")
                {
                    tempItem.revision = Convert.ToInt32(param.value);
                }
                else if(param.key == "Description")
                {
                    tempItem.description = param.value;
                }
                else if(param.key == "Part Type")
                {
                    tempItem.part_type = param.value.ToUpper();
                }
            }

            //item serialized to be sent as the request to the API
            //handles parameters not entered by the user, that way they are not included in the json string so the API doesn't have to parse and check for nulls.
            var jsonString = JsonConvert.SerializeObject(tempItem,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            //local string
            requestString = jsonString;
        }

        void Handle_DeleteParameter(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var parameterDelete = (Parameter)menuItem.CommandParameter;
            parametersList.Remove(parameterDelete);
        }
        //END: BUTTONS EVENTS #######################################################
    }
};
