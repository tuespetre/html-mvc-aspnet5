using html_mvc_aspnet5.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Models
{
    public class HomeGetViewModel
    {
        public HomeGetFormModel Form { get; set; }

        public IList<Item> Items { get; set; }

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
    }
}
