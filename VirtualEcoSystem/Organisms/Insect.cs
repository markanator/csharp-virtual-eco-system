using System;

namespace VirtualEcoSystem.Organisms
{
    public class Insect : Organism
    {

        public string[] LifeCycleStages = new string[]{ 
            "EGG",           // 0
            "LARVA",         // 1
            "PUPA",          // 2
            "ADULT_HUNGRY",  // 3
            "ADULT_NORMAL",  // 4
            "ADULT_STARVE",  // 5
            "DEATH_IMMINENT",// 6
            };
        public string CurrentCycleStage;
        public int DaysInCycle = 0;

        public Insect(string _name, string _desc) : base(_name, _desc)
        {
            // from mother moth
            this.Name = _name;
            this.Description = _desc;
            this.Age = 1;
            this.CurrentCycleStage = LifeCycleStages[0];
        }
        public Insect(string _name, string _desc, int _age) : base(_name, _desc,_age)
        {
            // initial render
            this.Name = _name;
            this.Description = _desc;
            this.Age = _age;
            this.CurrentCycleStage = LifeCycleStages[4];
        }

        public void IncreaseAge()
        {
            this.Age++;
            this.DaysInCycle++;
        }

        public void ConductBirthday()
        {
            // a function that checks the age and updates if necesary
            if (Age >= 6 && Age <= 8)
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
            else if (Age < 2)
            {
                this.CurrentCycleStage = LifeCycleStages[0];
            }

            if (this.CurrentCycleStage == LifeCycleStages[3] && this.DaysInCycle > 2)
            {
                this.CurrentCycleStage = LifeCycleStages[4];
            }

        }

        public void PollinatedPlant()
        {
            // no longer hungry
            this.CurrentCycleStage = LifeCycleStages[4];
            this.DaysInCycle = 0;
        }
    }
}