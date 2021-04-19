using System.Collections.Generic;
using VirtualEcoSystem.Organisms;
using static System.Console;


namespace VirtualEcoSystem.Events
{
    public class Overcast : Event
    {
        public static new void CustomEvent(List<Organism> _orgList)
        {
            WriteLine("Overcast EVENT CALLED");
            List<Organism> OrgsToDelete = new List<Organism>();
            List<Organism> FreshOrgs = new List<Organism>();

            int count = 0;
            foreach (var org in _orgList)
            {
                switch (org)
                {
                    case Plant p:
                        p.Hydration++;
                        break;
                    case Insect bug:
                        if (count % 4 == 0)
                        {
                            FreshOrgs.Add(new Insect("Yucca Moth", "Relies on Yucca Plant for species growth. Collect to gain YY item."));
                        }
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
            OrgsToDelete.Clear();
            FreshOrgs.Clear();

            WriteLine($"{deathToll} removed from the world...");
            WriteLine($"{freshMeat} added to the world...");
        }
    }
}