using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TargetViewAttribute : Attribute
    {
        public string Name { get; set; }

        public bool Export { get; set; }
    }
}
