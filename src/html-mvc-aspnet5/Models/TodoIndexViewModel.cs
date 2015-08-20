using html_mvc_aspnet5.Objects;
using System.Collections.Generic;

namespace html_mvc_aspnet5.Models
{
    public class TodoIndexViewModel
    {
        public TodoIndexFormModel Form { get; set; }

        public IEnumerable<Item> Items { get; set; } = new List<Item>();

        public string TotalItems { get; set; }
    }
}
