using html_mvc_aspnet5.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Models
{
    public class HomeFormViewModel
    {
        public HomeFormFormModel Form { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();

        public bool NoItems
        {
            get
            {
                return Items.Count == 0;
            }
        }

        public bool AnyItems
        {
            get
            {
                return Items.Count > 0;
            }
        }

        public string DescriptionError { get; set; }
    }
}
