using System;
using System.Collections.Generic;
using NoteType;

using Xamarin.Forms;

namespace Viasat_App
{
    public partial class CommentPage : ContentPage
    {
        public CommentPage(NoteModel note)
        {
            InitializeComponent();

            noteLabel.Text = note.note;
            authorLabel.Text = note.author;
            dateLabel.Text = note.date;
        }
    }
}
