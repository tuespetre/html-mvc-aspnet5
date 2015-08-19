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
    [Route("")]
    public class HomeController : Controller
    {
        private IItemRepository itemRepository;

        public HomeController(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            itemRepository.Save();
        }

        [ResultModel(typeof(LayoutViewModel), Persistent = true)]
        [ResultModel(typeof(HomeIndexViewModel))]
        [HttpGet, Route("")]
        public HomeIndexViewModel Index()
        {
            return new HomeIndexViewModel();
        }

        [HttpGet, Route("Form")]
        public HomeFormViewModel Form()
        {
            return new HomeFormViewModel
            {
                Items = itemRepository.GetAllItems().ToList()
            };
        }

        [HttpPost, Route("Form")]
        public HomeFormViewModel Form(HomeFormFormModel form)
        {
            var model = new HomeFormViewModel
            {
                Form = form
            };

            switch (form.Command)
            {
                case HomeFormFormModel.CreateCommand:
                    if (
                        string.IsNullOrWhiteSpace(form.Description) ||
                        form.Description.Length > 10
                    )
                    {
                        model.DescriptionError = 
                            "Please enter a value between 1 and 10 characters.";
                        break;
                    }

                    itemRepository.AddItem(new Item
                    {
                        Description = form.Description
                    });
                    form.Description = string.Empty;

                    break;
                case HomeFormFormModel.DeleteCommand:
                    itemRepository.RemoveItem(form.ItemId);
                    break;
            }

            model.Items = itemRepository.GetAllItems().ToList();

            return model;
        }

        [HttpGet, Route("Get")]
        public HomeGetViewModel Get(HomeGetFormModel form)
        {
            var items = itemRepository.GetAllItems();

            if (!string.IsNullOrWhiteSpace(form.Filter))
            {
                var filter = form.Filter.ToLowerInvariant().Trim();

                items =
                    from i in items
                    let desc = i.Description.ToLowerInvariant()
                    where desc.Contains(filter)
                    select i;
            }

            return new HomeGetViewModel
            {
                Form = form ?? new HomeGetFormModel(),
                Items = items.ToList()
            };
        }
    }
}
