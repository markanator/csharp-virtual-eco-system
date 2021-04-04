using System.Collections.Generic;
using VirtualEcoSystem.Interfaces;

namespace VirtualEcoSystem.Items
{
    public class Item
    {
        public string Name
        {
            get;
            set;
        }

        public Dictionary<string, int> MaterialsRequiredToCraft;
    }
}