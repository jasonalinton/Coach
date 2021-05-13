using System;
using System.Collections.Generic;
using System.Text;

namespace CoachModel._App._Universal
{
    public class Tag
    {
        public Tag(int idTag, string title, string text)
        {
            this.idTag = idTag;
            this.Title = title;
            this.Text = text;
        }

        public int idTag { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
