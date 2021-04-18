using Pastel;
using System.Collections.Generic;
using VirtualEcoSystem.Organisms;
using static System.Console;


namespace VirtualEcoSystem.Events
{
    public class Flood : Event
    {
        public static new void CustomEvent(List<Organism> _orgList)
        {
            WriteLine("Flood EVENT CALLED".Pastel("#295eff"));
            List<Organism> OrgsToDelete = new List<Organism>();

            int count = 0;
            foreach (var org in _orgList)
            {
                switch (org)
                {
                    case Plant p:
                        if (count % 5 == 0)
                        {
                            // delete every 5th plant
                            OrgsToDelete.Add(p);
                        }
                        else
                        {
                            // or over hydrate them
                            p.Hydration = 10;
                            p.CurrentLifeStage = Plant.ReproductiveCycle.IMMINENT_DEATH;
                            p.LifeCycleDayCount = 0;
                        }
                        break;
                    case Insect bug:
                        string CCS = bug.CurrentCycleStage;
                        if (CCS == "EGG" || CCS == "LARVA" || CCS == "PUPA")
                        {
                            OrgsToDelete.Add(bug);
                        }
                        // TODO 
                        // increse chance to get eaten by RABBITS
                        break;
                    default:
                        break;
                }
                count++;
            }

        }

    }
}