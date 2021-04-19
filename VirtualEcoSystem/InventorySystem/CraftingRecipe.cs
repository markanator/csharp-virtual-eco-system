// code help from @Kryzarel on https://youtu.be/gZsJ_rG5hdo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualEcoSystem.Items;
using VirtualEcoSystem.Interfaces;

namespace VirtualEcoSystem.InventorySystem
{
    public struct ItemAmounts
    {
        public Item Item;
        public int Amount;
    }
    class CraftingRecipe
    {
        // items and quantity needed
        public List<ItemAmounts> Materials;
        // end result of combination
        public List<ItemAmounts> Results;

        public bool CanCraft(IItemContainer itemContainer)
        {
            foreach (ItemAmounts item in Materials)
            {
                // check to see if we have enough
                if (itemContainer.ItemCount(item.Item) < item.Amount)
                {
                    return false;
                }
            }
            return true;
        }

        public void Craft(IItemContainer itemContainer)
        {
            if (CanCraft(itemContainer))
            {
                // loop through and remove items
                foreach (ItemAmounts itemAmount in Materials)
                {
                    for (int i = 0; i < itemAmount.Amount; i++)
                    {
                        // remove as many items that we need
                        itemContainer.RemoveItem(itemAmount.Item);
                    }
                }

                // loop through results and add items
                foreach (ItemAmounts itemAmount in Results)
                {
                    for (int i = 0; i < itemAmount.Amount; i++)
                    {
                        // remove as many items that we need
                        itemContainer.AddItem(itemAmount.Item);
                    }
                }

            }

        }
    }
}
