using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualEcoSystem.Events;
using VirtualEcoSystem.Organisms;

namespace VirtualEcoSystem
{
    public class GameMaster
    {
        public Environment GameEnvironment { get; set; }
        public Player CurrPlayer { get; set; }
        public List<Organism> DailyOrganismsList;
        public int DayCount;

        public static void Start()
        {
            throw new System.NotImplementedException();
        }
    }
}