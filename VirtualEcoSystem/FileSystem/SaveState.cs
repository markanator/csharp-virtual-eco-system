// Savestate code help from:: @Epitome https://youtu.be/Q2nEsa209ew
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualEcoSystem.Entity;
using VirtualEcoSystem.Organisms;

namespace VirtualEcoSystem.FileSystem
{
    [Serializable]
    class SaveState
    {
        public DateTime LasteSaved { get; set; }
        public Player CurrPlayer { get; set; }
        public Enviro CurrEnv { get; set; }
        public List<Organism> OrgList { get; set; }
        public int DayCount { get; set; }
        public int AirMoisture { get; set; }
        public string PlantMothRatioMsg { get; set; }

        public Market DesertMarket { get; set; }
    }
}
