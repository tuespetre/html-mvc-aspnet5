using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Models
{
    public class HomeFormFormModel
    {
        public const string DeleteCommand = "Delete";
        public const string CreateCommand = "Create";

        public string Description { get; set; }

        public Guid ItemId { get; set; }

        public string Command { get; set; }
    }
}
