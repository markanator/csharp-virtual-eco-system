namespace VirtualEcoSystem.Organisms
{
    public class Rabbit : Organism
    {
        public Rabbit(string _name, string _desc) : base(_name, _desc)
        {
            this.Name = _name;
            this.Description = _desc;
        }
    }
}