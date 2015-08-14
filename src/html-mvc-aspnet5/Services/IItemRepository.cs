using html_mvc_aspnet5.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace html_mvc_aspnet5.Services
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetAllItems();

        void AddItem(Item item);

        void RemoveItem(Guid id);
    }
}
