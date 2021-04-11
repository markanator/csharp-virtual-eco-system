using Pastel;
using System;

namespace VirtualEcoSystem.Organisms
{
    public class Plant : Organism, IHarvestable
    {
        public int PlantAge { get; private set;}
        public bool CanHarvest { get; set; }
        public enum ReproductiveCycle{ 
            SEED,          // 0
            GERMINATION,   // 1
            SEEDLING,      // 2
            NEEDS_POLLEN,  // 3
            POLLINATED,    // 4
            DROPPED_SEED,  // 5 - creates new plant
            IMMINENT_DEATH // 6
            }; 
        public int LifeCycleDayCount;
        public ReproductiveCycle CurrentLifeStage;
        public int TotalSeedsReleased;
        public int Hydration;

        public Plant (string _name, string _desc): base(_name, _desc)
        {
            PlantAge = 1;
            base.Age = PlantAge;
            CurrentLifeStage = ReproductiveCycle.NEEDS_POLLEN;
            //
            CanHarvest = (int)CurrentLifeStage > 2 ? true: false;
            this.Hydration = 5;
        }

        /// <summary>This function attempts to pollinate plant, will call internal Plant function <c>HandleLifeCycleCount</c> if conditions are not met.</summary>
        public bool PollinatePlant(Insect _moth)
        {
            // Needs pollen
            if (this.CurrentLifeStage == ReproductiveCycle.NEEDS_POLLEN)
            {
                this.LifeCycleDayCount = 0;
                this.CurrentLifeStage = ReproductiveCycle.POLLINATED;
                _moth.PollinatedPlant();
                return false;
            } 
            // already pollinated but not old enough to release seeds
            else if (this.CurrentLifeStage == ReproductiveCycle.POLLINATED && LifeCycleDayCount < 2)
            {
                this.LifeCycleDayCount++;
                _moth.PollinatedPlant();
                return false;
            }

            // atleast 2 days with pollinated tag
            if (this.CurrentLifeStage == ReproductiveCycle.POLLINATED && LifeCycleDayCount >= 2)
            {
                // reset and drop seed
                this.LifeCycleDayCount = 0;
                this.CurrentLifeStage = ReproductiveCycle.NEEDS_POLLEN;
                this.TotalSeedsReleased++;
                _moth.PollinatedPlant();
                return true;
            }

            // nothing from above matches
            _moth.PollinatedPlant();
            HandleLifeCycleCount();
            return false;
        }

        public void IncreasePlantAge()
        {
            this.PlantAge++;
            this.LifeCycleDayCount++;
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
            } 
            else if (this.CurrentLifeStage == ReproductiveCycle.IMMINENT_DEATH)
            {
                // dead
                this.CanHarvest = false;
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

        public void EnvironmentHydratation(Environment _env, int _airMoisture)
        {
            int rand = Utils.RandomGen.Next(-Math.Abs(_airMoisture), Math.Abs(_airMoisture));

            switch (_env.CurrentEvent)
            {
                case "Normal Day":
                case "FLOOD":
                case "OVERCAST":
                    if (!Utils.__PROD__) Console.WriteLine("water++");
                    this.Hydration += rand;
                    break;
                case "DROUGHT":
                case "SUNNY":
                    if (!Utils.__PROD__) Console.WriteLine("water--");
                    this.Hydration -= rand;
                    break;
                case "FIRE":
                    if (!Utils.__PROD__) Console.WriteLine("burnt");

                    this.CurrentLifeStage = ReproductiveCycle.IMMINENT_DEATH;
                    this.Hydration = 0;
                    this.CanHarvest = false;
                    break;
                default:
                    if (!Utils.__PROD__) Console.WriteLine("water:nada");
                    break;
            }

            // lets keep it within bounds
            if (this.Hydration >= 10)
            {
                this.Hydration = 10;
            } 
            else if (this.Hydration <= 0)
            {
                this.Hydration = 0;
            }

            // check hydration level
            // if over 7, or lower than 2 => starts to die
            if (this.Hydration >= 9 || this.Hydration <= 1)
            {
                if (!Utils.__PROD__) Console.WriteLine("Plant dying..".Pastel("#FFC300"));
                CurrentLifeStage = ReproductiveCycle.IMMINENT_DEATH;
                this.CanHarvest = true;
                this.LifeCycleDayCount = 0;
            }
        }
    }
}