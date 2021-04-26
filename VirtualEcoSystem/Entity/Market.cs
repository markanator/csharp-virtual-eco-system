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
using static VirtualEcoSystem.Utils;


namespace VirtualEcoSystem.Entity
{
    [Serializable]
    class Market
    {
        private List<Item> MerchantStock;
        private double Cash;
        public Market()
        {
            MerchantStock = Utils.LoadMerchantXmlItems();
            Cash = 500;
        }

        public void AddItem(Item _item)
        {
            bool itemAlreadyInInventory = false;
            // check if its in the inventory
            foreach (Item inventoryItem in MerchantStock)
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
                MerchantStock.Add(_item);
            }
            // use delegate to update UI
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

        public double ShowVenderCashLimit()
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

        // main menu items
        public void SellItemsToPlayer(Player CurrPlayer)
        {
            Clear();
            WriteLine("~~~ Buying from Marketplace ~~~".Pastel(Utils.Color["Primary"]));
            foreach (string itemLine in this.FetchInventoryList())
            {
                WriteLine(itemLine);
            }
            WriteLine("\n");
            WriteLine($"{CurrPlayer.PlayerName} has: {CurrPlayer.GetCurrentCashAmount():C}".Pastel(Utils.Color["Primary"]));
            WriteLine("What do you want to purchase?\n" +
                "(Q)".Pastel(Utils.Color["Actions"]) +
                " to return to Main Menu" +
                "\nPlease enter name of item, " +
                "Case Sensitive.".Pastel(Utils.Color["Other"]));
            string playerInput = ReadLine().Trim();

            // go to Main Menu
            if (playerInput.ToLower() == "q") return;

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
            Clear();
            WriteLine("~~~ Sell to Marketplace ~~~".Pastel(Utils.Color["Primary"]));
            // display what the user has in their pockets
            foreach (var itemLine in CurrPlayer.FetchInventoryList())
            {
                WriteLine(itemLine);
            }
            WriteLine("\n");
            WriteLine($"Market has: {this.Cash:C}".Pastel(Utils.Color["Other"]));
            WriteLine($"{CurrPlayer.PlayerName} has: {CurrPlayer.GetCurrentCashAmount():C}".Pastel(Utils.Color["Primary"]));
            WriteLine("What do you want to sell?\n" +
                "(Q)".Pastel(Utils.Color["Actions"]) +
                " to return to Main Menu" +
                "\nPlease enter name of item, " +
                "Case Sensitive.".Pastel(Utils.Color["Other"]));
            // read what the player wants to sell
            string playerInput = ReadLine().Trim();
            // go to Main Menu
            if (playerInput.ToLower() == "q") return;

            // MAIN BLOCK
            try
            {
                // attempt to fetch item 
                Item tempItem = CurrPlayer.ReturnPlayersStash().FetchItem(playerInput);
                // throw err if it doesnt match
                if (tempItem == null) throw new Exception("No item found");

                if (CurrPlayer.HasEnoughToUse(tempItem))
                {
                    // can sell
                    WriteLine($"How many items do you wish to sell? Max: {tempItem.Amount}");
                    int amountToSell = 1;
                    try
                    {
                        amountToSell = Convert.ToInt32(ReadLine().Trim());
                        if (amountToSell <= 0 || amountToSell > tempItem.Amount)
                        {
                            throw new Exception("Attempting to sell negative or more than owned.");
                        }
                    }
                    catch 
                    {
                        // amount entered does not conform
                        WriteLine("That's a hard pass. Attempting to sell negative or more than currently owned.".Pastel(Utils.Color["Danger"]));
                        WaitForInput();
                        BuyItemsFromPlayer(CurrPlayer);
                    }
                    

                    // can sell
                    // check to see if merchant has enough money
                    if (this.Cash - tempItem.MerchantPrice >= 0)
                    {
                        // calculate how much the merchant is willing to pay
                        double merchantAmountForXItem = CalcMerchantPurchasePrice(tempItem.MerchantPrice) * amountToSell;
                        // take from merchant wallet && add to player
                        this.Cash -= merchantAmountForXItem;
                        CurrPlayer.CashForSelling(merchantAmountForXItem);
                        // remove one from merchant && add 1 to player inventory
                        this.AddItem(new Item
                        {
                            Name = tempItem.Name,
                            Amount = amountToSell,
                            CurrItemType = tempItem.CurrItemType,
                            MerchantPrice = tempItem.MerchantPrice
                        });
                        CurrPlayer.PInventory.RemoveItem(new Item
                        {
                            Name = tempItem.Name,
                            Amount = amountToSell,
                            CurrItemType = tempItem.CurrItemType,
                            MerchantPrice = tempItem.MerchantPrice
                        });

                        WriteLine("Successfully purchased item!".Pastel(Utils.Color["Success"]));
                        WaitForInput();
                        BuyItemsFromPlayer(CurrPlayer);
                    }
                    else
                    {
                        WriteLine("You do not have enough money for that.".Pastel(Utils.Color["Warning"]));
                        WaitForInput();
                        BuyItemsFromPlayer(CurrPlayer);
                    }
                }
                else
                {
                    // cannot sell
                    WriteLine("You do not any more of that item. Try something else.".Pastel(Utils.Color["Warning"]));
                    WaitForInput();
                    BuyItemsFromPlayer(CurrPlayer);
                }
            }
            catch
            {
                // item not found
                WriteLine("That item was not found, try again...".Pastel(Utils.Color["Danger"]));
                WaitForInput();
                BuyItemsFromPlayer(CurrPlayer);
            }
        }

    }
}
