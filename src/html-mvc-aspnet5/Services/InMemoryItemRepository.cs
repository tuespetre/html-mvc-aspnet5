using html_mvc_aspnet5.Objects;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNet.Hosting;

namespace html_mvc_aspnet5.Services
{
    public class InMemoryItemRepository : IItemRepository
    {
        private readonly string itemsSessionKey = $"{nameof(InMemoryItemRepository)}|Items";

        private readonly ISession session;

        public InMemoryItemRepository(IHttpContextAccessor accessor)
        {
            session = accessor.HttpContext.Session;

            var serialized = session.GetString(itemsSessionKey) ?? string.Empty;
            var deserialized = JsonConvert.DeserializeObject<List<Item>>(serialized);
            
            items = deserialized ?? new List<Item>
            {
                new Item { Description = "Put my thing down" },
                new Item { Description = "Flip it" },
                new Item { Description = "And reverse it" }
            };
        }

        private readonly List<Item> items;

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

        public void Save()
        {
            session.SetString(itemsSessionKey, JsonConvert.SerializeObject(items));
        }
    }
}
