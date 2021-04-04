using VirtualEcoSystem.Interfaces;

namespace VirtualEcoSystem.Organisms
{
    public class Organism : ICollectable
    {
        public string Name;
        public string Description;
        public int Age;

        public Organism(string _name, string _desc)
        {
            Name = _name;
            Description = _desc;
        }
        public Organism(string _name, string _desc,int _age)
        {
            Name = _name;
            Description = _desc;
            Age = _age;
        }

        public void AddToPlayerInventory()
        {
            throw new System.NotImplementedException();
        }
    }
}