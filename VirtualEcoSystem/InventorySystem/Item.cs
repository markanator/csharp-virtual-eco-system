﻿using System.Collections.Generic;
using VirtualEcoSystem.Interfaces;

namespace VirtualEcoSystem.Items
{
    public class Item
    {
        public enum ItemType
        {
            None,
            // from organisms
            PlantLeaf,
            MothEggs,

            // craftable
            PlantHealingPotion,
            GrilledMeat,
            Bait,

            // merchant only
            Trap,
            WaterBottle,
            PlantSeed,
            PlantSprout,
            PlantAdult,
            WeakCoffee,
            StrongCoffee,

        }
        public ItemType CurrItemType;
        public int Amount = 1;
        public string Name;

        public Item() { }
        public Item(string _name, int _amount, ItemType _type)
        {
            this.Name = _name;
            this.Amount = _amount;
            this.CurrItemType = _type;
        }

        //public bool IsStackable()
        //{
        //    switch (this.CurrItemType)
        //    {
        //        default:
        //        case ItemType.Bait:
        //        case ItemType.GrilledMeat:
        //        case ItemType.MothEggs:
        //        case ItemType.PlantLeaf:
        //        case ItemType.Trap:
        //            return true;
        //        case ItemType.PlantHealingPotion:
        //            return false;
        //    }
        //}
    }
}