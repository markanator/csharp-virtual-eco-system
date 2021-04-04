namespace VirtualEcoSystem.Organisms
{
    public class Lizard : Organism
    {
        public Lizard(string _name, string _desc) : base(_name, _desc)
        {
            this.Name = _name;
            this.Description = _desc;
        }
    }
}