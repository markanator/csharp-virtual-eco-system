// code help from @Kryzarel on https://youtu.be/gZsJ_rG5hdo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualEcoSystem.Items;

namespace VirtualEcoSystem.Interfaces
{
    public interface IItemContainer
    {
        // allow us to communicate with anything that can contain items
        // merchant, lootbox, craft system
        bool ContainsItem(Item _item);
        int ItemCount(Item _item);
        void AddItem(Item _item);
        void RemoveItem(Item _item);
        void UseItem(Item _item);
        void PrintInventory();
    }
}
