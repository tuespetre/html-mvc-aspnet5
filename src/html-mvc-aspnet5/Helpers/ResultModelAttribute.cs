using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Internal;

namespace html_mvc_aspnet5.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ResultModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Creates a new instance of the MultiObjectResult attribute
        /// using the specified type.
        /// </summary>
        /// <param name="type">The type of object to resolve from the
        /// application service collection.</param>
        public ResultModelAttribute(Type type)
        {
            Type = type;
        }

        public string Name { get; set; }

        public Type Type { get; private set; }

        public bool Persistent { get; set; }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult)
            {
                var name = Name;
                var services = context.HttpContext.RequestServices;
                var multiContext = (MultiObjectResultContext)services.GetService(typeof(MultiObjectResultContext));

                if (string.IsNullOrEmpty(Name))
                {
                    name = Type.Name;
                }

                multiContext.AdditionalObjects[name] = Tuple.Create(Type, Persistent);
            }
        }
    }
}
