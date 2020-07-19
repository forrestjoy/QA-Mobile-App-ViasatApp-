using System;
using System.Collections.Generic;
using UserType;
using ItemType;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;

namespace Viasat_App
{
    public partial class LoginPage : ContentPage
    {
        UserModel userRequest = new UserModel();

        ObservableCollection<UserModel> userReceived = new ObservableCollection<UserModel>();
        List<String> demoUsers = new List<string>();

        string requestString;
        string responseString;

        public LoginPage()
        {
            InitializeComponent();
            demoUsers.Add("000000000000000000000001");
            demoUsers.Add("000000000000000000000002");
            demoUsers.Add("000000000000000000000003");
            demoUsers.Add("000000000000000000000004");
            demoUsers.Add("000000000000000000000005");
            demoUsers.Add("000000000000000000000006");
            demoUsers.Add("000000000000000000000007");

            globals.Globals.TheUser.recently_viewed = new ObservableCollection<string>();
            globals.Globals.favoritesList = new ObservableCollection<string>();
            globals.Globals.favoritesItemsList = new ObservableCollection<ItemModel>();
        }

        private async void loginButton_Clicked(object sender, EventArgs e)
        {
            if (demoUsers.Contains(usernameEntry.Text) && passwordEntry.Text == "1234") //demo auth
            {
                createRequest(usernameEntry.Text);

                using (var httpClient = new HttpClient())
                {
                    var httpContent = new StringContent(requestString, Encoding.UTF8, "application/json");

                    var httpResponse = await httpClient.PostAsync("http://52.13.18.254:3000/returnuser", httpContent);

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

                userReceived = JsonConvert.DeserializeObject<ObservableCollection<UserModel>>(responseString);

                globals.Globals.TheUser = userReceived[0];

                await Navigation.PushAsync(new MainPage(globals.Globals.TheUser));
            }
            else
            {
                DisplayAlert("Error", "Please enter a valid combination.", "OK");
            }
        }

        public void createRequest(string userID)
        {
            UserModel tempUser = new UserModel();
            tempUser._id = userID;

            var jsonString = JsonConvert.SerializeObject(tempUser,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            requestString = jsonString;
        }
    }
}