using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Models
{
    public class TodoIndexFormModel
    {
        public const string DeleteCommand = "Delete";
        public const string CreateCommand = "Create";

        [FromForm]
        public string Description { get; set; }

        [FromForm]
        public Guid ItemId { get; set; }

        [FromForm]
        public string Command { get; set; }

        [FromQuery]
        public string Filter { get; set; }
    }
}
