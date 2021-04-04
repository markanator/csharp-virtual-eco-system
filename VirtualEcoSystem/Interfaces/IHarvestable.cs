using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualEcoSystem
{
    public interface IHarvestable
    {
        bool CanHarvest { get; set; }

        string HarvestXItem();
    }
}