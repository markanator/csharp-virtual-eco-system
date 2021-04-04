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


namespace VirtualEcoSystem
{
    class VirtEcoGame : Game
    {
        private Player CurrPlayer;
        public Environment GameEnvironment;
        public List<Organism> OrganismsList;
        public int DayCount;
       

        public VirtEcoGame()
        {
            CurrPlayer = new Player();
            Setup();
        }

        public override void Setup()
        {
            GameEnvironment = new Environment("Desert", "Its hot here.");
            OrganismsList = new List<Organism>();
            DayCount = 1;

            GenerateInitialWildLife();

            StartGame();
        }

        public override void StartGame()
        {
            base.IsPlaying = true;
            
            Clear();
            IntroScreen();
            WaitForInput();
            Clear();
            while (IsPlaying)
            {
                Clear();
                WriteLine($"Day: {DayCount}");
                DisplayPlayerTurns(CurrPlayer);
                switch(PlayerOptions(new string[] { "Check Temperature. (Free)","Check Environment (Free)", "Exit Game" }))
                {
                    case 1:
                        ConductWeatherCheck();
                        break;
                    case 2:
                        ConductEnvironmentCheck();
                        break;
                    case 3:
                        this.IsPlaying = false;
                        break;
                    default:
                        break;
                }
                
                

                // no turns left
                if (CurrPlayer.CurrentTurns <= 0)
                {
                    // add to day count
                    DayCount++;
                    // reset player health
                    CurrPlayer.CurrentTurns = CurrPlayer.MaxTurns;
                    // perform daily organism actions
                    PerformOrganismDailies();
                }
                if (!IsPlaying) break;
            }

            Clear();
            WaitForInput("Thanks for playing!\nVirtEco: Mojave Desert\nBy: Mark Ambrocio");
        }
        private void DisplayTopUI()
        {
            WriteLine($"Day: {DayCount}");
            DisplayPlayerTurns(CurrPlayer);
        }

        private void GenerateInitialWildLife()
        {
            int plantCount = new Random().Next(5, 21);
            int mothCount = new Random().Next(4, 18);

            for (int i = 0; i < plantCount; i++)
            {
                OrganismsList.Add(new Plant("Yucca Plant", "Relies on Yucca Moth for species growth. Harvest to gain XX item."));
            }

            for (int i = 0; i < mothCount; i++)
            {
                OrganismsList.Add(new Insect("Yucca Moth", "Relies on Yucca Plant for species growth. Collect to gain YY item.", i));
            }
        }

        private void ConductWeatherCheck()
        {
            //if (CurrPlayer.PlayerConstitutionCheck())
            //{
                WriteLine("~~~ CHECKING WEATHER ~~~");
                WriteLine(GameEnvironment.FetchCurrentTempFromEnvironment());
                WriteLine("Current Events: "+GameEnvironment.GenerateRandomEvent());
                //CurrPlayer.RemovePlayerTurn();
            //}
            //else
            //{
            //    WriteLine("Unable to Perform Request.");
            //}

            WaitForInput();
        }

        private void ConductEnvironmentCheck()
        {
            Clear();
            DisplayTopUI();
            int plantCount = 0;
            int mothCount = 0;
            WriteLine("\n~~~ CHECKING Environment ~~~".Pastel("#12c754"));

            foreach (var org in OrganismsList)
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

            int baseRatio = OrganismsList.Count;
            WriteLine($"x{plantCount} - Yucca Plants");
            WriteLine($"x{mothCount} - Yucca Moths");

            int pRatio = Convert.ToInt32(((float)plantCount / (float)baseRatio) * 100);
            int mRatio = Convert.ToInt32(((float)mothCount / (float)baseRatio) * 100);

            WriteLine($"Plant Percentage:: {pRatio}%");
            WriteLine($"Moth Percentage:: {mRatio}%");
            //CurrPlayer.RemovePlayerTurn();
            ConductEnvironmentActions();
        }

        private void ConductEnvironmentActions()
        {
            switch(PlayerOptions(new string[] { "Harvest x1 Yucca Plant. (-1 Turn)", "Collect x2 Moths.(-1 Turn)", "Return to office" }))
            {
                case 1:
                    WriteLine("You harvested a plant");
                    WaitForInput();
                    // recursive parent call
                    ConductEnvironmentCheck();
                    break;
                case 2:
                    WriteLine("You harvested a moth");
                    WaitForInput();
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

        private void PerformOrganismDailies()
        {
            // List<int> Orgs to remove // a collection of indexes to remove
            List<int> ToRemove = new List<int>();
            List<int> ToPollinate = new List<int>();
            // cycle through organisms list
            foreach(var org in OrganismsList)
            {
                switch (org)
                {
                    case Plant p:
                        if (DayCount % 2 == 0)
                        {
                            p.IncreasePlantAge();
                        }
                        if (p.PlantAge >= 1 && !p.CanHarvest)
                        {
                            p.MarkAsHarvestable();
                        }
                        // check to see if plant needs to be pollinated
                        if (p.CurrentLifeStage == Plant.ReproductiveCycle.NEEDS_POLLEN)
                        {
                            ToPollinate.Add(OrganismsList.IndexOf(org));
                        }
                        break;
                }
                if (org.GetType() == typeof(Insect))
                {
                    var tempMoth = (Insect)org;

                    tempMoth.IncreaseAge();
                    tempMoth.ConductBirthday();

                    if(tempMoth.Age > 12)
                    {
                        ToRemove.Add(OrganismsList.IndexOf(org));
                    }
                }
            }

            foreach(int i in ToRemove)
            {
                WriteLine($"{OrganismsList[i].Name} had died.");
                OrganismsList.Remove(OrganismsList[i]);
            }
        }
    }
}
