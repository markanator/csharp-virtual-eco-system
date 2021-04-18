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
        }
    }
}