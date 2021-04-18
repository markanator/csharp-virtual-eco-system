/*
 * VirtEco
 * By: Mark Ambrocio
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualEcoSystem.Organisms;
using static VirtualEcoSystem.ConsoleUIBuilder;
using static System.Console;
using Pastel;
using VirtualEcoSystem.Events;

namespace VirtualEcoSystem
{
    class VirtEcoGame : Game
    {
        private Player CurrPlayer;
        private Environment CurrEnv;
        private List<Organism> OrgList;
        private int DayCount;
        private int AirMoisture = 5;
        private string PlantMothRatioMsg;

        private bool WantsToSkip = false;

        public VirtEcoGame()
        {
            CurrPlayer = new Player();
            Setup();
        }

        public override void Setup()
        {
            CurrEnv = new Environment("Desert", "Its hot here.");
            OrgList = new List<Organism>();
            DayCount = 1;

            GenerateWildLife(true);

            StartGame();
        }

        public override void StartGame()
        {
            base.IsPlaying = true;

            Clear();
            IntroScreen(CurrPlayer.PlayerName);
            WaitForInput();
            Clear();
            while (IsPlaying)
            {
                WantsToSkip = false;
                Clear();
                DisplayTopUI();
                switch (PlayerOptions(new string[] { 
                    "Check Temperature",
                    "Check Environment",
                    "Check Inventory",
                    // "Craft Items from Inventory",
                    "Skip Day",
                    "Exit Game"
                    }))
                {
                    case 1:
                        ConductWeatherCheck();
                        break;
                    case 2:
                        ConductEnvironmentCheck();
                        break;
                    case 3:
                        CurrPlayer.CheckInventory();
                        WaitForInput();
                        break;
                    case 4:
                        this.WantsToSkip = true;
                        break;
                    case 5:
                        this.IsPlaying = false;
                            break;
                    default:
                        break;
                }



                // no turns left
                if (CurrPlayer.CurrentTurns <= 0 || WantsToSkip)
                {
                    Clear();
                    if (!Utils.__PROD__) Console.WriteLine("=== Fresh Orgs Entered ===");
                    GenerateWildLife();
                    if (!Utils.__PROD__) Console.WriteLine("=== START DAILIES ===");

                    // add to day count
                    DayCount++;

                    // reset player health
                    CurrPlayer.CurrentTurns = CurrPlayer.MaxTurns;

                    // perform daily environment actions
                    CurrEnv.PerformDailyWeatherChange();

                    if (!Utils.__PROD__) Console.WriteLine($"Weather: " +
                        $"{CurrEnv.CurrentTemp} | " +
                        $"{CurrEnv.CurrentEvent}");

                    // adjust plant stuff
                    CheckWeatherMoisture();
                    // perform daily organism actions
                    PerformOrganismDailies();

                    // finally, check and run temperature event
                    CurrEnv.PerformTemperatureEvent(OrgList);
                    ConductPlantMothRatioCheck();

                    //
                    if (!Utils.__PROD__) WaitForInput();

                }
                if (!IsPlaying) break;
            }

            Clear();
            WaitForInput("Thanks for playing!\nVirtEco: Mojave Desert\nBy: Mark Ambrocio");
        }

        private void DisplayTopUI()
        {
            WriteLine($"Day: {DayCount} | Temp: {CurrEnv.CurrentTemp}°F, {CurrEnv.CurrentEvent}");
            DisplayPlayerTurns(CurrPlayer);
            WriteLine(PlantMothRatioMsg != null ? PlantMothRatioMsg : "No Messages.");
            WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        private void GenerateWildLife(bool _firstTime = false)
        {
            int plantCount = new Random().Next(9, _firstTime ? 15 : 24);
            int mothCount = new Random().Next(6, _firstTime ? 11 : 26);

            for (int i = 0; i < plantCount; i++)
            {
                OrgList.Add(new Plant("Yucca Plant", "Relies on Yucca Moth for species growth. Harvest to gain XX item."));
            }

            for (int i = 0; i < mothCount; i++)
            {
                OrgList.Add(new Insect("Yucca Moth", "Relies on Yucca Plant for species growth. Collect to gain YY item.", 6));
            }

            // sort list by Name 
            OrgList.OrderBy(org => org.Name);
        }

        private void ConductWeatherCheck()
        {
            //if (CurrPlayer.PlayerConstitutionCheck())
            //{
                WriteLine("~~~ CHECKING WEATHER ~~~");
                WriteLine(CurrEnv.FetchCurrentTempFromEnvironment());
                WriteLine("Current Events: " + CurrEnv.CurrentEvent);
                // implement moisture check based on weather
                // TODO: 

                CurrPlayer.RemovePlayerTurn();
            //}
            //else
            //{
            //    WriteLine("Unable to Perform Request.");
            //}

            WaitForInput();
        }

        private void CheckWeatherMoisture()
        {
            int amount = Utils.RandomGen.Next(-2,2);
            AirMoisture += amount;
                // rain fall
                foreach(var org in OrgList)
                {
                    switch (org)
                    {
                        case Plant p:
                            p.EnvironmentHydratation(CurrEnv, AirMoisture);
                            break;
                    }
                }
        }

        private void ConductEnvironmentCheck()
        {
            Clear();
            DisplayTopUI();

            var ObligateRatios = PerformObligateRatioCalc();
            WriteLine("\n~~~ CHECKING Environment ~~~".Pastel("#12c754"));
            WriteLine($"x{ObligateRatios["plantCount"]} - Yucca Plants");
            WriteLine($"x{ObligateRatios["mothCount"]} - Yucca Moths");

            

            WriteLine($"Plant Percentage:: {ObligateRatios["pRatio"]}%");
            WriteLine($"Moth Percentage:: {ObligateRatios["mRatio"]}%");
            // function to display ratio balance
            ConductPlantMothRatioCheck();
            //CurrPlayer.RemovePlayerTurn();
            PerformEnvironmentActions();
        }

        private void PerformEnvironmentActions()
        {
            switch (PlayerOptions(new string[] { "Harvest x1 Yucca Plant. (-1 Turn)", "Collect x2 Moths.(-1 Turn)", "Return to office" }))
            {
                case 1:
                    if (CurrPlayer.PlayerConstitutionCheck())
                    {
                        Plant plantToHarvest = null;

                        // loop through array and find a plant that is harvestable
                        foreach (var candidate in OrgList)
                        {
                            if (candidate.GetType().ToString().Equals("VirtualEcoSystem.Organisms.Plant"))
                            {
                                Plant tempCandidate = (Plant)candidate;
                                if (tempCandidate.CanHarvest)
                                {
                                    //WriteLine("Found a Plant.");
                                    plantToHarvest = tempCandidate;
                                    break;
                                }
                            }
                        }

                        if (plantToHarvest != null && plantToHarvest.CanHarvest)
                        {
                            WriteLine("You harvested a plant");
                            CurrPlayer.AddItemFromOrganism(plantToHarvest);
                            CurrPlayer.RemovePlayerTurn();
                            OrgList.Remove(plantToHarvest);
                            WaitForInput();
                        }
                        else
                        {
                            WriteLine("No plants found, try again tomorrow.");
                            WaitForInput();
                        }
                    }
                    else
                    {
                        WriteLine("Unable to Perform Requested Action. Try again tomorrow.");
                        WaitForInput();
                    }

                    // recursive parent call
                    ConductEnvironmentCheck();
                    break;
                case 2:
                    if (CurrPlayer.PlayerConstitutionCheck())
                    {
                        Insect mothToCatach = null;

                        // loop through array and find a plant that is harvestable
                        foreach (var candidate in OrgList)
                        {
                            if (candidate.GetType().ToString().Equals("VirtualEcoSystem.Organisms.Insect"))
                            {
                                Insect tempCandidate = (Insect)candidate;
                                if (tempCandidate.CurrentCycleStage.Contains("ADULT"))
                                {
                                    //WriteLine("Found a Plant.");
                                    mothToCatach = tempCandidate;
                                    break;
                                }
                            }
                        }

                        if (mothToCatach != null)
                        {
                            WriteLine("You collected a Moth");
                            CurrPlayer.AddItemFromOrganism(mothToCatach);
                            CurrPlayer.RemovePlayerTurn();
                            OrgList.Remove(mothToCatach);
                            WaitForInput();
                        }
                        else
                        {
                            WriteLine("No Adult Moths found, try again tomorrow.");
                            WaitForInput();
                        }
                    }
                    else
                    {
                        WriteLine("Unable to Perform Requested Action. Try again tomorrow.");
                        WaitForInput();
                    }

                    // recursive parent call
                    ConductEnvironmentCheck();
                    break;
                case 3:
                    // return to main menu
                    return;
                //break;
                default:
                    ConductEnvironmentCheck();
                    break;
            }
        }

        // can move to other file
        private void PerformOrganismDailies()
        {
            // List<int> Orgs to remove // a collection of indexes to remove
            List<Insect> HungryMoths = new List<Insect>();
            List<Plant> PlantsToPollinate = new List<Plant>();
            List<int> PlantsToRemove = new List<int>();
            List<int> MothToRemove = new List<int>();
            // cycle through organisms list
            foreach (var org in OrgList)
            {
                switch (org)
                {
                    case Plant p:
                        if (!Utils.__PROD__) Console.WriteLine($"-----" +
                            $"\nplant idx:{OrgList.IndexOf(p)}");
                        // only age when the day is 1 or even
                        if (DayCount % 2 == 0)
                        {
                            if (!Utils.__PROD__) Console.WriteLine("age++");
                            p.IncreasePlantAge();
                        }
                        // a player can harvest this item
                        if (p.PlantAge >= 1 && !p.CanHarvest)
                        {
                            if (!Utils.__PROD__) Console.WriteLine("harvestable++");
                            p.MarkAsHarvestable();
                        }
                        // check to see if plant needs to be pollinated
                        if (p.CurrentLifeStage == Plant.ReproductiveCycle.NEEDS_POLLEN)
                        {
                            if (!Utils.__PROD__) Console.WriteLine("ToPillinate++");
                            PlantsToPollinate.Add(p);
                        }
                        else if (p.CurrentLifeStage == Plant.ReproductiveCycle.NEEDS_POLLEN)
                        {
                            // check to see if plant is dead
                            if (!Utils.__PROD__) Console.WriteLine("PlantToRemove++");
                            PlantsToRemove.Add(OrgList.IndexOf(p));
                        }

                        if (!Utils.__PROD__) Console.WriteLine($"age {p.Age}" +
                            $"\nhydration: {p.Hydration}" +
                            $"\ncanHarvest:{p.CanHarvest}" +
                            $"\ndaysInCycle:{p.LifeCycleDayCount}" +
                            $"\nlifeStage:{p.CurrentLifeStage}");
                        break;
                    case Insect bugg:
                        bugg.IncreaseAge();
                        bugg.ConductBirthday();
                        if (!Utils.__PROD__) Console.WriteLine($"-------" +
                            $"\nmoth idx:{OrgList.IndexOf(bugg)}" +
                            $"\nage::{bugg.Age}" +
                            $"\ndaysInCycle:{bugg.DaysInCycle}" +
                            $"\nlifestage: {bugg.CurrentCycleStage}");


                        if (bugg.Age >= 12)
                        {
                            if (!Utils.__PROD__) Console.WriteLine("moth died...");
                            MothToRemove.Add(OrgList.IndexOf(bugg));
                        }

                        // check if bug can pollinate
                        if (bugg.CurrentCycleStage == "ADULT_HUNGRY" && bugg.Age < 12)
                        {
                            HungryMoths.Add(bugg);
                        }
                            break;
                }
            }

            if (MothToRemove.Count > 0)
            {
                if (!Utils.__PROD__) WriteLine(":: MOTH DEATHS ::");
                int count = 0;
                MothToRemove.ForEach(m => {
                    count++;
                    OrgList.RemoveAt(m);
                });


                if (!Utils.__PROD__) WriteLine($"{count} moths have died.");
            }

            if (PlantsToRemove.Count > 0)
            {
                if (!Utils.__PROD__) Console.WriteLine(":: PLANT DEATHS ::");
                PlantsToRemove.ForEach(d => OrgList.RemoveAt(d));
            }

            if (HungryMoths.Count > PlantsToPollinate.Count)
            {
                // moths pollinate plant
                PerformMothPlantPollination(HungryMoths,PlantsToPollinate, PlantsToPollinate.Count);
            } 
            else if (PlantsToPollinate.Count > HungryMoths.Count)
            {
                PerformMothPlantPollination(HungryMoths, PlantsToPollinate, HungryMoths.Count);
            }
        }

        private void PerformMothPlantPollination(List<Insect> _moths, List<Plant> _plants, int _amount)
        {
            int seedsToPlant = 0;
            for (int i = 0; i <= _amount -1; i++)
            {
                // lets pollinate and if finction returns true,
                // add to seeds to plant
                if (_plants[i].PollinatePlant(_moths[i]))
                {
                    seedsToPlant++;
                }
                
            }
        }

        private void ConductPlantMothRatioCheck()
        {
            Dictionary<string, int> ObligateRatios = PerformObligateRatioCalc();
            string msg;
            // if there are too many moths, compared to plants
            if (ObligateRatios["mRatio"]+10 < ObligateRatios["pRatio"] || ObligateRatios["mRatio"] - 10 < ObligateRatios["pRatio"])
            {
                msg = "\nMoth to plant ratio ok".Pastel("#12c754");
                WriteLine(msg);
            }
            else if (ObligateRatios["mRatio"] >= 85 && ObligateRatios["mRatio"] > ObligateRatios["pRatio"])
            {
                msg = "Warning! Environment out of balance, organism extinction imminent.".Pastel("#D50000");
                WriteLine("\n"+msg);
                PlantMothRatioMsg = msg;
            }
            else if (ObligateRatios["mRatio"] >= 70 && ObligateRatios["mRatio"] > ObligateRatios["pRatio"])
            {
                msg = "Fix ratio before its too late".Pastel("#FF3333");
                WriteLine("\n" + msg);
                PlantMothRatioMsg = msg;
            }
            else if (ObligateRatios["mRatio"] >= 55 && ObligateRatios["mRatio"] > ObligateRatios["pRatio"])
            {
                // warning
                msg = "Too many moths!".Pastel("#FFA733");
                WriteLine("\n" + msg);
                PlantMothRatioMsg = msg;
            }
            
            // moths start dying
            // if there are too many plants, compared to moths
            // plants start to die
        }

        private Dictionary<string, int> PerformObligateRatioCalc()
        {

            int plantCount = 0;
            int mothCount = 0;

            foreach (var org in OrgList)
            {
                if (org.GetType().ToString().Contains("Plant"))
                {
                    plantCount++;
                }
                else if (org.GetType().ToString().Contains("Insect"))
                {
                    mothCount++;
                }
            }

            int baseRatio = OrgList.Count;

            int pRatio = Convert.ToInt32(((float)plantCount / (float)baseRatio) * 100);
            int mRatio = Convert.ToInt32(((float)mothCount / (float)baseRatio) * 100);


            return new Dictionary<string, int>() {
                { "plantCount", plantCount },
                { "mothCount", mothCount },
                { "baseRatio", baseRatio },
                { "pRatio", pRatio },
                { "mRatio", mRatio },
                };

        }
    }
}
