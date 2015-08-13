using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Models
{
    public class LayoutViewModel
    {
        public string Random { get; set; } = Guid.NewGuid().ToString();
    }
}
