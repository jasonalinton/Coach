using System;
using System.Collections.Generic;
using System.Text;

namespace CoachModel._App._Universal
{
    public class Note
    {
        public Note(int idNote, string title, string text)
        {
            this.idNote = idNote;
            this.Title = title;
            this.Text = text;
        }

        public int idNote { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
