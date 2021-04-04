using System;

namespace VirtualEcoSystem.Organisms
{
    public class Plant : Organism, IHarvestable
    {
        public int PlantAge { get; private set;}
        public bool CanHarvest { get; set; }
        public enum ReproductiveCycle{ 
            SEED,         // 0
            GERMINATION,  // 1
            SEEDLING,     // 2
            NEEDS_POLLEN, // 3
            POLLINATED,   // 4
            DROPPED_SEED  // 5 - creates new plant
            }; 
        private int LifeCycleDayCount;
        public ReproductiveCycle CurrentLifeStage;
        public int TotalSeedsReleased;

        public Plant (string _name, string _desc): base(_name, _desc)
        {
            PlantAge = 1;
            base.Age = PlantAge;
            CurrentLifeStage = ReproductiveCycle.NEEDS_POLLEN;
            //
            CanHarvest = (int)CurrentLifeStage > 2 ? true: false;
        }

        /// <summary>This function attempts to pollinate plant, will call internal Plant function <c>HandleLifeCycleCount</c> if conditions are not met.</summary>
        public bool PollinatePlant()
        {
            // Needs pollen
            if (this.CurrentLifeStage == ReproductiveCycle.NEEDS_POLLEN)
            {
                this.LifeCycleDayCount = 0;
                this.CurrentLifeStage = ReproductiveCycle.POLLINATED;
                return false;
            } 
            // already pollinated but not old enough to release seeds
            else if (this.CurrentLifeStage == ReproductiveCycle.POLLINATED && LifeCycleDayCount < 2)
            {
                this.LifeCycleDayCount++;
                return false;
            }

            // atleast 2 days with pollinated tag
            if (this.CurrentLifeStage == ReproductiveCycle.POLLINATED && LifeCycleDayCount >= 2)
            {
                // reset and drop seed
                this.LifeCycleDayCount = 0;
                this.CurrentLifeStage = ReproductiveCycle.NEEDS_POLLEN;
                this.TotalSeedsReleased++;
                return true;
            }

            // nothing from above matches
            HandleLifeCycleCount();
            return false;
        }

        public void IncreasePlantAge()
        {
            this.PlantAge++;
        }

        private void HandleLifeCycleCount()
        {
            // anything under NEEDS POLLEN
            if ((int)this.CurrentLifeStage < 3)
            {
                // underage
                if (this.LifeCycleDayCount < 2)
                {
                    this.LifeCycleDayCount++;
                }
                // at or overage
                else if (this.LifeCycleDayCount >= 2)
                {
                    // reset and advance
                    this.LifeCycleDayCount = 0;
                    this.CurrentLifeStage++;
                }
            } else
            {
                throw new Exception("Unhandled Error:: Handle LifeCycle Conditions not met.");
            }
        }

        public void MarkAsHarvestable()
        {
            this.CanHarvest = true;
        }

        public string HarvestXItem()
        {
            return $"{this.Name} was harvested!";
        }
    }
}