using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualEcoSystem
{
    public class HarvestLeaves : IHarvestable
    {
        public bool CanHarvest { get; set; }
        public string HarvestXItem()
        {
            throw new NotImplementedException();
        }
    }
}