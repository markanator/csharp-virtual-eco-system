using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualEcoSystem.Interfaces;
using VirtualEcoSystem.Items;
using System.IO;
using Pastel;
using static System.Console;
using static VirtualEcoSystem.ConsoleUIBuilder;


namespace VirtualEcoSystem.Entity
{
    [Serializable]
    class Market
    {
        private List<Item> MerchantStock;
        private int Cash;
        public Market()
        {
            MerchantStock = Utils.LoadMerchantXmlItems();
            Cash = 500;
        }

        public void AddItem(Item _item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsItem(Item _item)
        {
            // check if its in the inventory
            foreach (var inventoryItem in MerchantStock)
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
            int itemCount = 0;
            foreach (Item inventoryItem in MerchantStock)
            {
                // already in the inventory
                if (inventoryItem.CurrItemType == _item.CurrItemType)
                {
                    itemCount++;
                }
            }
            return itemCount;
        }

        public Item FetchItem(string itemName)
        {
            return MerchantStock
                    .Where<Item>(item=> item.Name == itemName)
                    .First();
        }

        public List<string> FetchInventoryList()
        {
            List<string> tempItemsList = new List<string>();
            foreach(var item in MerchantStock)
            {
                tempItemsList.Add($"[{item.Name}] : (x{item.Amount}) : ${item.MerchantPrice}");
            }

            return tempItemsList;
        }

        public int FetchInventoryCount()
        {
            return this.MerchantStock.Count();
        }

        public void GetCashFromBuyer(int soldItemPrice)
        {
            Cash += soldItemPrice;
        }

        public int ShowVenderCashLimit()
        {
            return Cash;
        }

        public void RemoveItem(Item _item)
        {
            Item itemInInventory = null;
            // check if its in the inventory
            foreach (Item inventoryItem in MerchantStock)
            {
                // already in the inventory
                if (inventoryItem.CurrItemType == _item.CurrItemType)
                {
                    inventoryItem.Amount -= _item.Amount;
                    itemInInventory = inventoryItem;
                }
            }
            // if we found something and it's amount is less than oo equal to zero
            // then remove it from the list
            if (itemInInventory != null && itemInInventory.Amount <= 0)
            {
                MerchantStock.Remove(itemInInventory);
            }
            // use delegate to update UI
        }

        public void UseItem(Item _item)
        {
            // not being used by the market
        }

        public void SellItemsToPlayer(Player CurrPlayer)
        {
            foreach (string itemLine in this.FetchInventoryList())
            {
                WriteLine(itemLine);
            }
            WriteLine("\n");
            WriteLine($"{CurrPlayer.PlayerName} has: {CurrPlayer.GetCurrentCashAmount():C}".Pastel(Utils.Color["Primary"]));
            WriteLine("What do you want to purchase? " +
                "(Q)".Pastel(Utils.Color["Actions"]) +
                " to return to Main Menu\nPlease enter name of item within square brackets, " +
                "Case Sensitive.".Pastel(Utils.Color["Other"]));
            string playerInput = ReadLine().Trim();

            if (playerInput.ToLower() == "q")
            {
                // main menu
                return;
            }

            try
            {
                // attempt to fetch item from merchant
                Item tempItem = this.FetchItem(playerInput);
                // throw err if it doesnt match
                if (tempItem == null) throw new Exception("No item found");
                // check to see if merchant has enough to sell
                if (tempItem.Amount - 1 >= 0)
                {
                    // check to see if player has enough money
                    if (CurrPlayer.GetCurrentCashAmount() - tempItem.MerchantPrice >= 0)
                    {
                        // take from player wallet && add to merchant
                        CurrPlayer.PayWithCash(tempItem.MerchantPrice);
                        this.GetCashFromBuyer(tempItem.MerchantPrice);
                        // remove one from merchant && add 1 to player inventory
                        this.RemoveItem(new Item
                        {
                            Name = tempItem.Name,
                            Amount = 1,
                            CurrItemType = tempItem.CurrItemType,
                            MerchantPrice = tempItem.MerchantPrice
                        });
                        CurrPlayer.PInventory.AddItem(new Item
                        {
                            Name = tempItem.Name,
                            Amount = 1,
                            CurrItemType = tempItem.CurrItemType,
                            MerchantPrice = tempItem.MerchantPrice
                        });

                        WriteLine("Successfully purchased item!".Pastel(Utils.Color["Success"]));
                        WaitForInput();
                        SellItemsToPlayer(CurrPlayer);
                    }
                    else
                    {
                        WriteLine("You do not have enough money for that.".Pastel(Utils.Color["Warning"]));
                        WaitForInput();
                        SellItemsToPlayer(CurrPlayer);
                    }
                }
                else
                {
                    WriteLine("The market is out of that. Try something else.".Pastel(Utils.Color["Warning"]));
                    WaitForInput();
                    SellItemsToPlayer(CurrPlayer);
                }

            }
            catch
            {
                WriteLine("That item was not found, try again...".Pastel(Utils.Color["Danger"]));
                WaitForInput();
                SellItemsToPlayer(CurrPlayer);
            }
        }

        public void BuyItemsFromPlayer(Player CurrPlayer)
        {

        }

    }
}
