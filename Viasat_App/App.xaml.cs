using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using ItemType;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Viasat_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //initializing global lists
            globals.Globals.recentlyViewedList = new ObservableCollection<ItemModel>();
            globals.Globals.TheUser = new UserType.UserModel();

            //first page to appear
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
