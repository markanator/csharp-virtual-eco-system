using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualEcoSystem.Items;
using VirtualEcoSystem.Organisms;
using Pastel;

namespace VirtualEcoSystem.Entity
{
    [Serializable]
    public class Player
    {
        public Inventory PInventory;
        public int MaxTurns;
        public int CurrentTurns;
        public int OverageTurns;
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

        public void CraftItem(Item _item)
        {
            if (_item is null)
            {
                throw new ArgumentNullException(nameof(_item));
            }
        }

        public void UseItem(Item item)
        {
            switch (item.CurrItemType)
            {
                case Item.ItemType.Bait:
                    Console.WriteLine("Used Bait");
                    // TODO make special X.type function
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.Bait });
                    break;
                case Item.ItemType.GrilledMeat:
                    Console.WriteLine("Used GrilledMeat");
                    // TODO make special X.type function
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.GrilledMeat });
                    break;
                case Item.ItemType.MothEggs:
                    Console.WriteLine("Used MothEggs");
                    // TODO make special X.type function
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.MothEggs });
                    break;
                case Item.ItemType.PlantHealingPotion:
                    Console.WriteLine("Used PlantHealingPotion");
                    // TODO make special X.type function
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.PlantHealingPotion });
                    break;
                case Item.ItemType.PlantLeaf:
                    Console.WriteLine("Used PlantLeaf");
                    // TODO make special X.type function
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.PlantLeaf });
                    break;
                case Item.ItemType.Trap:
                    Console.WriteLine("Used Trap");
                    // TODO make special X.type function
                    PInventory.RemoveItem(new Item { Name = item.Name, Amount = 1, CurrItemType = Item.ItemType.Trap });
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

        public bool HasEnoughToSell(Item itemToSell)
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
    }
}