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
        }
    }
}