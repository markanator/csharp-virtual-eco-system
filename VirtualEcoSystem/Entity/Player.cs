using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualEcoSystem.Items;
using VirtualEcoSystem.Organisms;
using Pastel;

namespace VirtualEcoSystem
{
    public class Player
    {
        public Inventory PInventory;
        public int MaxTurns;
        public int CurrentTurns;
        public int OverageTurns;
        public string PlayerName;

        public Player()
        {
            this.PlayerName = ConsoleUIBuilder.AskForPlayerName();
            //Inventory = new Dictionary<string, int>();
            MaxTurns = 10;
            CurrentTurns = 10;
            OverageTurns = 3;

            PInventory = new Inventory(UseItem);
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

    }
}