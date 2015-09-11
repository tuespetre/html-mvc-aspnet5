using System.Linq;
using Microsoft.AspNet.Mvc;
using html_mvc_aspnet5.Models;
using html_mvc_aspnet5.Services;
using html_mvc_aspnet5.Objects;
using Microsoft.AspNet.Mvc.Filters;

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
            var total = items.Count();

            if (!string.IsNullOrWhiteSpace(form.Filter))
            {
                var filter = form.Filter.ToLowerInvariant().Trim();

                items =
                    from i in items
                    let desc = i.Description.ToLowerInvariant()
                    where desc.Contains(filter)
                    select i;
            }

            // Note: you'd probably have this in a resource file somewhere.
            var pluralizedFormat = total == 1 ? "{0} todo" : "{0} todos";

            return new TodoIndexViewModel
            {
                Items = items.ToList(),
                TotalItems = string.Format(pluralizedFormat, total),
                Form = form
            };
        }
    }
}
