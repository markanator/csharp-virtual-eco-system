using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualEcoSystem.Interfaces;
using VirtualEcoSystem.Items;
using VirtualEcoSystem.Organisms;
using Pastel;

namespace VirtualEcoSystem
{
    public class Player
    {
        public Dictionary<string, int> Inventory;
        public int MaxTurns;
        public int CurrentTurns;
        public int OverageTurns;
        public string PlayerName;

        public Player()
        {
            this.PlayerName = ConsoleUIBuilder.AskForPlayerName();
            Inventory = new Dictionary<string, int>();
            MaxTurns = 10;
            CurrentTurns = 10;
            OverageTurns = 3;
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

        public void UseItem() { }

        public void AddItemFromPlant(Plant _plant)
        {
            if (_plant.Name == "Yucca Plant")
            {
                if (Inventory.ContainsKey("Yucca Plant"))
                {
                    // already exists => add to it
                    Inventory["Yucca Plant"] += 1;
                }
                else
                {
                    // fresh insert => assign default value
                    Inventory.Add("Yucca Plant", 1);
                }
            }
        }

        public void CheckInventory()
        {
            Console.WriteLine("~~~ Checking Inventory ~~~".Pastel("#b642f5"));
            if (this.Inventory.Count > 0)
            {
                foreach (var item in this.Inventory)
                {
                    Console.WriteLine($"x{item.Value} - {item.Key}");
                }
            }
            else
            {
                Console.WriteLine("Nothing found.");
            }
            
            Console.WriteLine("~~~ END REPORT ~~~".Pastel("#792ca3"));
        }
    }
}