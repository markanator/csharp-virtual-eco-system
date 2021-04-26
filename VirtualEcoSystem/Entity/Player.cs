using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pastel;
using VirtualEcoSystem.Items;
using static VirtualEcoSystem.ConsoleUIBuilder;
using static System.Console;
using VirtualEcoSystem.Organisms;

namespace VirtualEcoSystem.Entity
{
    [Serializable]
    public class Player
    {
        public Inventory PInventory;
        public int MaxTurns;
        public int CurrentTurns;
        private int OverageTurns;
        public string PlayerName;
        private double Wallet;

        public Player()
        {
            this.PlayerName = ConsoleUIBuilder.AskForPlayerName();
            this.MaxTurns = 10;
            this.CurrentTurns = 10;
            this.OverageTurns = 3;
            this.Wallet = 34;

            this.PInventory = new Inventory(UseItem);
        }

        public bool PlayerConstitutionCheck()
        {
            if (CurrentTurns + OverageTurns > 0)
            {
                return true;
            }

            return false;
        }

        public bool RemovePlayerTurn()
        {
            if (this.OverageTurns > 0)
            {
                OverageTurns -= 1;
                return true;
            }
            else if (CurrentTurns - 1 >= 0)
            {
                CurrentTurns -= 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddTurns(int amountToAdd)
        {
            int currTurns = this.CurrentTurns + this.OverageTurns;
            int overTurns = 0;

            if (currTurns + amountToAdd >= 10)
            {
                overTurns = currTurns + amountToAdd - 10;
            }

            this.CurrentTurns = amountToAdd;
            this.OverageTurns = overTurns;
        }

        public int FetchTotalTurns()
        {
            return this.CurrentTurns + this.OverageTurns;
        }

        public void CraftItem(Item _item)
        {
            if (_item is null)
            {
                throw new ArgumentNullException(nameof(_item));
            }
        }

        public void UseItem(Item item, List<Organism> OrgList)
        {
            switch (item.CurrItemType)
            {

#region CRAFTING SYSTEM STUFF
                //case Item.ItemType.Bait:
                //    Console.WriteLine("Used Bait");
                //    // TODO make special X.type function
                //    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.Bait });
                //    break;
                //case Item.ItemType.GrilledMeat:
                //    Console.WriteLine("Used GrilledMeat");
                //    // TODO make special X.type function
                //    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.GrilledMeat });
                //    break;
                //case Item.ItemType.PlantHealingPotion:
                //    Console.WriteLine("Used PlantHealingPotion");
                //    // TODO make special X.type function
                //    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.PlantHealingPotion });
                //    break;
                //case Item.ItemType.PlantLeaf:
                //    Console.WriteLine("Used PlantLeaf");
                //    // TODO make special X.type function
                //    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.PlantLeaf });
                //    break;
                //case Item.ItemType.Trap:
                //    Console.WriteLine("Used Trap");
                //    // TODO make special X.type function
                //    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.Trap });
                //    break;
#endregion

                case Item.ItemType.WaterBottle:
                    break;
                case Item.ItemType.MothEggs:
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.MothEggs });
                    OrgList.Add(new Insect("Yucca Moth", "Relies on Yucca Plant for species growth. Collect to gain an item."));
                    break;
                case Item.ItemType.PlantSeed:
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.PlantSeed });
                    OrgList.Add(new Plant {
                        Age = 0,
                        CanHarvest = false,
                        CurrentLifeStage = Plant.ReproductiveCycle.GERMINATION,
                        Description = "Relies on Yucca Moth for species growth. Harvest to gain item.",
                        Name = "Yucca Plant",
                        Hydration = 5,
                        LifeCycleDayCount = 1,
                        });
                    break;
                case Item.ItemType.PlantSprout:
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.PlantSprout });
                    OrgList.Add(new Plant
                    {
                        Age = 2,
                        CanHarvest = false,
                        CurrentLifeStage = Plant.ReproductiveCycle.SEEDLING,
                        Description = "Relies on Yucca Moth for species growth. Harvest adults plants to gain an item.",
                        Name = "Yucca Plant",
                        Hydration = 5,
                        LifeCycleDayCount = 1,
                    });
                    break;
                case Item.ItemType.PlantAdult:
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.PlantAdult });
                    OrgList.Add(new Plant
                    {
                        Age = 8,
                        CanHarvest = false,
                        CurrentLifeStage = Plant.ReproductiveCycle.NEEDS_POLLEN,
                        Description = "Relies on Yucca Moth for species growth. Harvest adults plants to gain an item.",
                        Name = "Yucca Plant",
                        Hydration = 5,
                        LifeCycleDayCount = 1,
                    });
                    break;
                case Item.ItemType.WeakCoffee:
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.WeakCoffee });
                    this.AddTurns(4);
                    break;
                case Item.ItemType.StrongCoffee:
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.StrongCoffee });
                    this.AddTurns(7);
                    break;
                default:
                    break;
            }
        }

        // removes amount based on item's price
        public void PayWithCash(double itemPrice)
        {
            this.Wallet -= itemPrice;
        }

        public void CashForSelling(double itemPrice)
        {
            this.Wallet += itemPrice;
        }

        // returns a players current wallet amount
        public double GetCurrentCashAmount()
        {
            return this.Wallet;
        }

        public Inventory ReturnPlayersStash()
        {
            return PInventory;
        }

        public List<string> FetchInventoryList()
        {
            List<string> tempItemsList = new List<string>();
            foreach (Item item in PInventory.FetchInventoryList())
            {
                double calcPurchasePrice = Utils.CalcMerchantPurchasePrice(item.MerchantPrice);
                tempItemsList.Add($"[{item.Name}] : (x{item.Amount}) : ${calcPurchasePrice}");
            }

            return tempItemsList;
        }

        public bool HasEnoughToUse(Item itemToSell)
        {
            try
            {
                Item pitem = PInventory.GetItemList().Find(item => item.CurrItemType == itemToSell.CurrItemType);
                if (pitem != null && pitem.Amount - 1 >= 0)
                {
                        return true;
                }
                return false;
            }
            catch
            {
            // does not contain item or not enough to sell
                return false;
            }
        }

        // MAIN MENU ACTION
        public void ConductItemUsage(List<Organism> orgList)
        {
            Clear();
            foreach (var item in PInventory.GetItemList())
            {
                WriteLine($"[{item.Name}] - (x{item.Amount}) :: (-1 turn)");
            }
            WriteLine("\n");
            WriteLine("What do you want to use? " +
                "(Q)".Pastel(Utils.Color["Actions"]) +
                " to return to Main Menu\nPlease enter name of item within square brackets, " +
                "Case Sensitive.".Pastel(Utils.Color["Other"]));
            // read player input
            string playerInput = ReadLine().Trim();

            // go to Main Menu
            if (playerInput.ToLower() == "q") return;

            try
            {
                if (this.PlayerConstitutionCheck())
                {
                    // attempt to fetch item 
                    Item tempItem = this.PInventory.FetchItem(playerInput);
                    // throw err if it doesnt match
                    if (tempItem == null) throw new Exception("No item found");

                    if (this.HasEnoughToUse(tempItem))
                    {
                        PInventory.UseItem(new Item { 
                            Name = tempItem.Name,
                            Amount = 1,
                            CurrItemType = tempItem.CurrItemType,
                            MerchantPrice = tempItem.MerchantPrice
                            }, orgList);
                        this.RemovePlayerTurn();
                    }
                    else
                    {
                        // does not have enough to use
                        WriteLine("You do not any more of that item. Try something else.".Pastel(Utils.Color["Warning"]));
                        WaitForInput();
                        ConductItemUsage(orgList);
                    }
                }
            }
            catch
            {
                WriteLine("That item was not found, try again...".Pastel(Utils.Color["Danger"]));
                WaitForInput();
                ConductItemUsage(orgList);
            }
        }
    }
}