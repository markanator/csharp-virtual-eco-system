namespace VirtualEcoSystem.Events
{
    public class Event
    {

        public string Name { get; set; }

        public delegate void TriggerEvent();  

        public virtual void CustomEvent(){}
    }
}