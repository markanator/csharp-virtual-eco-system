using System.Collections.Generic;
using VirtualEcoSystem.Organisms;
using static System.Console;


namespace VirtualEcoSystem.Events
{
    public class Drought : Event
    {
        public static new void CustomEvent(List<Organism> _orgList)
        {
            WriteLine("Drought EVENT CALLED");
            List<Organism> OrgsToDelete = new List<Organism>();

            int count = 0;
            foreach(var org in _orgList)
            {
                switch (org)
                {
                    case Plant p:
                        // remove every 7th plant
                        if (count  % 7 == 0)
                        {
                            OrgsToDelete.Add(p);
                        } 
                        else
                        {
                            // or make them thirsty
                            p.Hydration = 0;
                            p.CurrentLifeStage = Plant.ReproductiveCycle.IMMINENT_DEATH;
                            p.LifeCycleDayCount = 0;
                        }
                        break;
                    case Insect bug:
                        string CCS = bug.CurrentCycleStage;
                        if (CCS == "EGG" || CCS == "LARVA" || CCS == "PUPA")
                        {
                            OrgsToDelete.Add(bug);
                        } // delete every 10th bug
                        else if (count % 10 == 0)
                        {
                            OrgsToDelete.Add(bug);
                        }
                        break;
                    default:
                        break;
                }
                count++;
            }

            int deathtoll = OrgsToDelete.Count;
            // delete orgs
            Utils.RemoveOrgsFromMain(_orgList, OrgsToDelete);

            WriteLine($"{deathtoll} organisms died...");
        }
    }
}