namespace VirtualEcoSystem.Organisms
{
    public class Bird : Organism
    {

        public Bird(string _name, string _desc) : base(_name, _desc)
        {
            this.Name = _name;
            this.Description = _desc;
        }
    }
}