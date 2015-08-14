using html_mvc_aspnet5.Objects;
using System;
using System.Collections.Generic;

namespace html_mvc_aspnet5.Services
{
    public class InMemoryItemRepository : IItemRepository
    {
        private List<Item> items = new List<Item>
        {
            new Item { Description = "Put my thing down" },
            new Item { Description = "Flip it" },
            new Item { Description = "And reverse it" }
        };

        public IEnumerable<Item> GetAllItems()
        {
            return items;
        }

        public void AddItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            items.Add(item);
        }

        public void RemoveItem(Guid id)
        {
            items.RemoveAll(i => i.Id == id);
        }
    }
}
