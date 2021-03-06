// with help from @CodeMonkey on youtube.com
// and Kryzel on youtube: https://www.youtube.com/watch?v=gZsJ_rG5hdo

using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualEcoSystem.Interfaces;
using VirtualEcoSystem.Organisms;

namespace VirtualEcoSystem.Items
{
    [Serializable]
    public class Inventory : IItemContainer
    {
        // void delegate from player to use an item
        private Action<Item, List<Organism>> UseItemAction;
        private List<Item> ItemList;

        public Inventory()
        {
            this.ItemList = new List<Item>();
        }
        public Inventory(Action<Item, List<Organism>> _useItemAction)
        {
            this.UseItemAction = _useItemAction;
            this.ItemList = new List<Item>();
        }

        public List<Item> GetItemList()
        {
            return ItemList;
        }

        public void AddItem(Item _item)
        {
                bool itemAlreadyInInventory = false;
                // check if its in the inventory
                foreach(Item inventoryItem in ItemList)
                {
                    // already in the inventory
                    if (inventoryItem.CurrItemType == _item.CurrItemType)
                    {
                        itemAlreadyInInventory = true;
                        inventoryItem.Amount += _item.Amount;
                    }
                }
                if (!itemAlreadyInInventory)
                {
                    ItemList.Add(_item);
                }
            // use delegate to update UI
        }

        public void RemoveItem(Item _item)
        {
            //wpf consideration, make duplicate before throwing out
            Item itemInInventory = null;
            // check if its in the inventory
            foreach (Item inventoryItem in ItemList)
            {
                // already in the inventory
                if (inventoryItem.CurrItemType == _item.CurrItemType)
                {
                    inventoryItem.Amount -= _item.Amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.Amount <= 0)
            {
                ItemList.Remove(itemInInventory);
            }
            // use delegate to update UI
        }

        public void UseItem(Item _item, List<Organism> orgList)
        {
            UseItemAction(_item, orgList);
        }

        public bool ContainsItem(Item _item)
        {
            // check if its in the inventory
            foreach (Item inventoryItem in ItemList)
            {
                // already in the inventory
                if (inventoryItem.CurrItemType == _item.CurrItemType)
                {
                    return true;
                }
            }
            return false;
        }

        public int ItemCount(Item _item)
        {
            int itemCount=0;
            foreach (Item inventoryItem in ItemList)
            {
                // already in the inventory
                if (inventoryItem.CurrItemType == _item.CurrItemType)
                {
                    itemCount++;
                }
            }
            return itemCount;
        }

        public void PrintInventory()
        {
            Console.WriteLine("~~~ Checking Inventory ~~~".Pastel("#b642f5"));
            if (this.ItemList.Count > 0)
            {
                foreach (Item item in ItemList)
                {
                    Console.WriteLine($"x{item.Name} - {item.Amount}");
                }
            }
            else
            {
                Console.WriteLine("Nothing found.");
            }

            Console.WriteLine("~~~ END REPORT ~~~".Pastel("#792ca3"));
        }

        public List<Item> FetchInventoryList()
        {
            return ItemList;
        }

        public Item FetchItem(string itemName)
        {
            return ItemList
                    .Where<Item>(item => item.Name == itemName)
                    .First();
        }
    }

}
