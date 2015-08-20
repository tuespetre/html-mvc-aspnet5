using Microsoft.AspNet.Mvc;
using html_mvc_aspnet5.Models;
using html_mvc_aspnet5.Helpers;

namespace html_mvc_aspnet5.Controllers
{
    public class HomeController : Controller
    {
        [ResultModel(typeof(LayoutViewModel), Persistent = true)]
        [ResultModel(typeof(HomeIndexViewModel))]
        [HttpGet, Route("")]
        public HomeIndexViewModel Index()
        {
            return new HomeIndexViewModel();
        }
    }
}
