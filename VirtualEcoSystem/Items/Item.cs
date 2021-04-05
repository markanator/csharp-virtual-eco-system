using System.Collections.Generic;
using VirtualEcoSystem.Interfaces;

namespace VirtualEcoSystem.Items
{
    public class Item
    {
        public string Name;
        public string Description;
        public int UseLimit;

        public Dictionary<string, int> MaterialsRequiredToCraft;
    }
}