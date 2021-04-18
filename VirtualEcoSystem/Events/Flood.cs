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
        }
    }
}