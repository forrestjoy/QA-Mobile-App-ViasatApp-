using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NoteType;
using Xamarin.Forms;
    
namespace Viasat_App
{
    public partial class CommentsPage : ContentPage
    {
        public ObservableCollection<NoteModel> NoteList { set; get; }
        string endpoint;
        string theId;

        //PARAMETER 1: the id (user or item) that will be used in case of clicking the write new note button. (So the new note belongs to either the user or item)
        //PARAMETER 2: the array of notes to populate the list
        //PARAMETER 3: the endpoint to be called (depending on if the note will be personal or for an item)
        public CommentsPage(string idReceived, ObservableCollection<NoteModel> notes, string endpointToUse)
        {
            endpoint = endpointToUse;
            InitializeComponent();
            NoteList = notes;
            CommentsListView.ItemsSource = NoteList;
            theId = idReceived;
        }

        private async void noteEntry_Tapped(object sender, EventArgs e)
        {
            NoteModel noteClicked = (NoteModel)((ListView)sender).SelectedItem;
            ((ListView)sender).SelectedItem = null;

            Console.WriteLine("note.author: " + noteClicked.author);
            Console.WriteLine("note.note: " + noteClicked.note);
            Console.WriteLine("note.date: " + noteClicked.date);

            await Navigation.PushAsync(new CommentPage(noteClicked));
        }

        private async void newNoteButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new WriteNotePage(endpoint, theId));
        }
    }
}