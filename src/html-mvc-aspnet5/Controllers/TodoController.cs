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
    [Route("Todo")]
    public class TodoController : Controller
    {
        private IItemRepository itemRepository;

        public TodoController(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            itemRepository.Save();
        }

        [Route("")]
        public TodoIndexViewModel Index(TodoIndexFormModel form)
        {
            switch (form.Command)
            {
                case TodoIndexFormModel.CreateCommand:
                    itemRepository.AddItem(new Item
                    {
                        Description = form.Description
                    });
                    form.Description = string.Empty;
                    break;
                case TodoIndexFormModel.DeleteCommand:
                    itemRepository.RemoveItem(form.ItemId);
                    break;
            }


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

            return new TodoIndexViewModel
            {
                Items = items.ToList(),
                Form = form
            };
        }
    }
}
