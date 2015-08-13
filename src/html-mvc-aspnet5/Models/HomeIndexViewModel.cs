using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Models
{
    public class HomeIndexViewModel
    {
        public string Contents { get; } = "My awesome page contents";

        public NestedModel Nested { get; } = new NestedModel();

        public string Random { get; } = Guid.NewGuid().ToString();

        public class NestedModel
        {
            public string Contents { get; } = "My awesome nested contents";
        }
    }
}
