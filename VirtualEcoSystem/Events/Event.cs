using System.Collections.Generic;
using VirtualEcoSystem.Organisms;
using static System.Console;


namespace VirtualEcoSystem.Events
{
    public class Event
    {

        public string Name { get; set; }

        public static void CustomEvent(List<Organism> _org)
        {
            WriteLine("GENERIC TEMPERATURE EVENT CALLED");
        }
    }
}