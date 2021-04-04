using System;

namespace VirtualEcoSystem.Organisms
{
    public class Insect : Organism
    {

        private string[] LifeCycleStages = new string[]{ "EGG","LARVA","PUPA","ADULT" };
        public string CurrentCycleStage;

        public Insect(string _name, string _desc) : base(_name, _desc)
        {
            this.Name = _name;
            this.Description = _desc;
            this.Age = 1;
            this.CurrentCycleStage = LifeCycleStages[1];
        }
        public Insect(string _name, string _desc, int _age) : base(_name, _desc,_age)
        {
            this.Name = _name;
            this.Description = _desc;
            this.Age = _age;
            this.CurrentCycleStage = LifeCycleStages[ _age >= 3 ? 3 : 2];
        }

        public void IncreaseAge()
        {
            this.Age++;
        }

        public void ConductBirthday()
        {
            // a function that checks the age and updates if necesary
            if (Age >= 6)
            {
                this.CurrentCycleStage = LifeCycleStages[3];
            }
            else if (Age < 6 && Age >= 4)
            {
                this.CurrentCycleStage = LifeCycleStages[2];
            }
            else if (Age < 4 && Age >= 2)
            {
                this.CurrentCycleStage = LifeCycleStages[1];
            }
            else
            {
                this.CurrentCycleStage = LifeCycleStages[0];
            }
        }
    }
}