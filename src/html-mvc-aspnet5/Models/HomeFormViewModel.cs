using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Models
{
    public class HomeFormViewModel
    {
        public HomeFormFormModel Form { get; set; }

        public string FirstNameError { get; set; }

        public string LastNameError { get; set; }
    }
}
