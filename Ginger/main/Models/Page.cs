using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Models
{
    public class Page
    {
        public Page(int pageNumber, string text, bool enabled)
        {
            this.PageNumber = pageNumber;
            this.Text = text;
            this.Enabled = enabled;
        }

        public string Text { get; }

        public int PageNumber { get; }

        public bool Enabled { get; }
    }
}
