using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualEcoSystem.Interfaces;
using VirtualEcoSystem.Items;

namespace VirtualEcoSystem
{
    public class Player
    {
        public Dictionary<ICollectable, int> Inventory;
        public int MaxTurns;
        public int CurrentTurns;
        public int OverageTurns;
        public string PlayerName;

        public Player()
        {
            this.PlayerName = ConsoleUIBuilder.AskForPlayerName();
            Inventory = new Dictionary<ICollectable, int>();
            MaxTurns = 10;
            CurrentTurns = 10;
            OverageTurns= 3;
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

        public void UseItem(){}
    }
}