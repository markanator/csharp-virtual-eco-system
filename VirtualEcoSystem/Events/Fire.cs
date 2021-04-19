using Pastel;
using System.Collections.Generic;
using VirtualEcoSystem.Organisms;
using static System.Console;

namespace VirtualEcoSystem.Events
{
    public class Fire : Event
    {
        public static new void CustomEvent(List<Organism> orgList)
        {
            WriteLine("FIRE EVENT CALLED");
            orgList.RemoveAll(og => og.GetType() == typeof(Plant));
            WriteLine("All plants have died...".Pastel("#c72e2e"));

            
            List<Organism> OrgsToRemove = new List<Organism>();

            // add to death toll
            int count = 0;
            foreach (var org in orgList)
            {
                switch (org)
                {
                    case Insect bug:
                        if (count % 2 == 0)
                        {
                            OrgsToRemove.Add(bug);
                        }
                        break;
                    default:
                        break;
                }
                count++;
            }

            int deathToll = OrgsToRemove.Count;

            // remove all bugs from ogList
            Utils.RemoveOrgsFromMain(orgList,OrgsToRemove);
            // reset removalList
            OrgsToRemove.Clear();

            WriteLine($"Moth death toll:: {deathToll}".Pastel("#c72e2e"));
        }
    }
}