using Microsoft.AspNet.Mvc;
using System.Dynamic;

namespace html_mvc_aspnet5.Helpers
{
    public class ViewViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string partialName, string scope = null)
        {
            dynamic model = new ExpandoObject();
            model.scope = scope;
            return View(partialName, model);
        }
    }
}
