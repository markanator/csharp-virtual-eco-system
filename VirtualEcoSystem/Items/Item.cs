﻿using System.Collections.Generic;
using VirtualEcoSystem.Interfaces;

namespace VirtualEcoSystem.Items
{
    public class Item : ICollectable
    {
        public string Name
        {
            get;
            set;
        }

        public Dictionary<string, int> MaterialsRequiredToCraft;

        public void AddToPlayerInventory()
        {
            throw new System.NotImplementedException();
        }
    }
}