using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using html_mvc_aspnet5.Models;
using html_mvc_aspnet5.Helpers;
using html_mvc_aspnet5.Services;
using html_mvc_aspnet5.Objects;
using Microsoft.Framework.Internal;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

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
