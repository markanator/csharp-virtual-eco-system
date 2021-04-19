using System;
using System.Collections.Generic;
using VirtualEcoSystem.Events;
using VirtualEcoSystem.Organisms;
using static System.Console;


namespace VirtualEcoSystem
{
    public class Sunny : Event

    {
        public static new void CustomEvent(List<Organism> _orgList)
        {
            WriteLine("Sunny EVENT CALLED");
            List<Organism> OrgsToDelete = new List<Organism>();
            List<Organism> FreshOrgs = new List<Organism>();

            int count = 0;
            foreach (var org in _orgList)
            {
                switch (org)
                {
                    case Plant p:
                        p.Hydration = 0;
                        p.CurrentLifeStage = Plant.ReproductiveCycle.IMMINENT_DEATH;
                        p.LifeCycleDayCount = 0;

                        if (count % 3 == 0)
                        {
                            FreshOrgs.Add(new Plant("Yucca Moth", "Relies on Yucca Moth for species growth. Harvest to gain XX item."));
                        }
                        break;
                    case Insect bug:
                        bug.CurrentCycleStage = bug.LifeCycleStages[3];
                        bug.DaysInCycle = 0;
                        
                        // TODO
                        // increase chance of being eaten by Lizards
                        break;
                    default:
                        break;
                }
                count++;
            }

            int deathToll = OrgsToDelete.Count;
            int freshMeat = FreshOrgs.Count;
            // delete orgs
            Utils.RemoveOrgsFromMain(_orgList, OrgsToDelete);
            // add to list
            Utils.AddOrgsToMain(_orgList, FreshOrgs);

            // reset removalList
            FreshOrgs.Clear();
            OrgsToDelete.Clear();

            WriteLine($"{deathToll} removed from the world...");
            WriteLine($"{freshMeat} added to the world...");
        }
    }
}