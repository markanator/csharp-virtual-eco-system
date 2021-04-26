/*
 * Virtual Ecosystem: Mojave Desery
 * By: Mark Ambrocio
 * 
 * Credits:
 * Using Pastel from nuget packages.
 * XML loading from Vending Machine Exercise
 * Icon from: https://icon-icons.com/icon/desert
 * SaveData/BinaryFormatter help from: @Epitome https://youtu.be/Q2nEsa209ew
 * Inventory with help from: 
 * @CodeMonkey on youtube.com
 * & Kryzel on youtube: https://www.youtube.com/watch?v=gZsJ_rG5hdo
 * Obligate Mutual guidance from: Prof. J. Baxter.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Pastel;
using VirtualEcoSystem.Items;
using VirtualEcoSystem.Organisms;
using VirtualEcoSystem.Entity;
using VirtualEcoSystem.FileSystem;
using static VirtualEcoSystem.ConsoleUIBuilder;
using static System.Console;
using System.IO;

namespace VirtualEcoSystem
{
    [Serializable]
    class VirtEcoGame : Game
    {
        private Player CurrPlayer;
        private Enviro CurrEnv;
        private List<Organism> OrgList;
        private int DayCount = 1;
        private int AirMoisture = 5;
        private string PlantMothRatioMsg = "No new messages.";

        private Market DesertMarket;
        [NonSerialized]
        private bool WantsToSkip = false;
        private SaveState SaveData;


        public VirtEcoGame()
        {
            if (Utils.SaveFileExsists())
            {
                // Save Game found, lets load from it
                LoadContent();

                CurrPlayer = SaveData.CurrPlayer;
                CurrEnv = SaveData.CurrEnv;
                OrgList = SaveData.OrgList;
                DayCount = SaveData.DayCount;
                AirMoisture = SaveData.AirMoisture;
                PlantMothRatioMsg = SaveData.PlantMothRatioMsg;

                DesertMarket = SaveData.DesertMarket;
            }
            else
            {
                // otherwise, lets create new set of data
                CurrPlayer = new Player();
                CurrEnv = new Enviro("Desert", "Its hot here.");
                OrgList = new List<Organism>();
                DesertMarket = new Market();

                GenerateWildLife(true);
            }

            // regaardless of above, lets save the game...
            SaveGame();
            // and start playing!
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
                    "Check Temperature",            // 1
                    "Check Environment",            // 2
                    "Go to the Market",             // 3
                    "Check Inventory",              // 4
                     "Use Items from Inventory",    // 5
                    // "Craft Items from Inventory",
                    "Skip Day",                     // 6
                    "Exit Game",                    // 7
                    }))
                {
                    case 1:
                        Clear();
                        DisplayTopUI();
                        CurrEnv.ConductWeatherCheck();
                        break;
                    case 2:
                        Clear();
                        DisplayTopUI();
                        ConductEnvironmentCheck();
                        break;
                    case 3:
                        Clear();
                        DisplayTopUI();
                        ConductMarketCheck();
                        break;
                    case 4:
                        Clear();
                        DisplayTopUI();
                        CurrPlayer.PInventory.PrintInventory();
                        WaitForInput();
                        break;
                    case 5:
                        Clear();
                        DisplayTopUI();
                        CurrPlayer.ConductItemUsage(OrgList);
                        break;
                    case 6:
                        this.WantsToSkip = true;
                        break;
                    case 7:
                        this.IsPlaying = false;
                            break;
                    default:
                        break;
                }



                // no turns left
                if (CurrPlayer.CurrentTurns <= 0 || WantsToSkip)
                {
                    Clear();
                    List<string> itemsToLog = new List<string>();
                    if (!Utils.__PROD__) Console.WriteLine("=== Fresh Orgs Entered ===");
                    GenerateWildLife();
                    if (!Utils.__PROD__) Console.WriteLine("=== START DAILIES ===");

                    // add to day count
                    DayCount++;
                    itemsToLog.Add($"DAY Count:: {DayCount}");

                    // reset player health
                    CurrPlayer.CurrentTurns = CurrPlayer.MaxTurns;
                    itemsToLog.Add($"Player Stats:: {CurrPlayer.PlayerName} | {CurrPlayer.GetCurrentCashAmount():C}");
                    itemsToLog.Add("Player Inventory:: ");
                    foreach (var item in CurrPlayer.ReturnPlayersStash().GetItemList())
                    {
                        itemsToLog.Add($"{item.Name} : (x{item.Amount}) : {item.MerchantPrice:C}");
                    }
                    itemsToLog.Add("~~~ END ~~~");

                    // perform daily environment actions
                    string env = CurrEnv.PerformDailyWeatherChange();
                    itemsToLog.Add(env);

                    if (!Utils.__PROD__) Console.WriteLine($"Weather: " +
                        $"{CurrEnv.CurrentTemp} | " +
                        $"{CurrEnv.CurrentEvent}");

                    // adjust plant stuff
                    CheckWeatherMoisture();
                    itemsToLog.Add($"Weather: Air Moisture:: {AirMoisture}");
                    // perform daily organism actions
                    List<string> orgDailyFeedback = PerformOrganismDailies();

                    foreach (string item in orgDailyFeedback)
                    {
                        itemsToLog.Add(item);
                    }


                    // finally, check and run temperature event
                    string eventCalled = CurrEnv.PerformTemperatureEvent(OrgList);
                    itemsToLog.Add("EVENT CALLED::: "+eventCalled);

                    string calcFeedback = ConductPlantMothRatioCheck();
                    itemsToLog.Add(calcFeedback);
                    
                    itemsToLog.Add("~~~~~~ END DEV STATS ~~~~~~");
                    if (!Utils.__PROD__) WaitForInput("~~~~~~ END DEV STATS ~~~~~~\nPress any key to return...");
                    
                    // create log file
                    Utils.CreateLogFile(itemsToLog,DayCount);
                    // save game
                    SaveGame();

                }
                if (!IsPlaying) break;
            }

            Clear();
            WaitForInput("Thanks for playing!\nVirtEco: Mojave Desert\nBy: Mark Ambrocio");
        }

        private void SaveGame()
        {
            // no file, create a new one
            if (SaveData == null)
            {
                SaveData = new SaveState();
            }
            
            // set data we want to save
            SaveData.LasteSaved = DateTime.Now;
            SaveData.CurrPlayer = CurrPlayer;
            SaveData.CurrEnv = CurrEnv;
            SaveData.AirMoisture = AirMoisture;
            SaveData.DayCount = DayCount;
            SaveData.DesertMarket = DesertMarket;
            SaveData.OrgList = OrgList;
            SaveData.PlantMothRatioMsg = PlantMothRatioMsg;

            // TODO change BinaryFormatter to xmlSerializer
            FileStream saveFile = File.Open(Utils.SaveGameFileTxt, FileMode.OpenOrCreate, FileAccess.Write);
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            
            // save content
            bf.Serialize(saveFile, SaveData);

            // close stream
            saveFile.Close();
        }

        private void LoadContent()
        {
            // TODO change BinaryFormatter to xmlSerializer
            try
            {
                // attempt to read from file
                FileStream loadFile = File.Open(Utils.SaveGameFileTxt, FileMode.Open, FileAccess.Read);
                var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                // load savestate from file
                SaveData = (SaveState)bf.Deserialize(loadFile);
                loadFile.Close();

                if (!Utils.__PROD__) 
                {
                    WriteLine("Successfully Loaded File.".Pastel(Utils.Color["Success"]));
                    WaitForInput("\nPress any key to continue...");
                }
            } 
            catch
            {
                // if any errors, just run savegame
                SaveGame();
            }

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
            int plantCount = new Random().Next(5, _firstTime ? 25 : 15);
            int mothCount = new Random().Next(8, _firstTime ? 21 : 18);

            for (int i = 0; i < plantCount; i++)
            {
                OrgList.Add(new Plant("Yucca Plant", "Relies on Yucca Moth for species growth. Harvest to gain an item."));
            }

            for (int i = 0; i < mothCount; i++)
            {
                OrgList.Add(new Insect("Yucca Moth", "Relies on Yucca Plant for species growth. Collect to gain an item.", 6));
            }

            // sort list by Name 
            OrgList.OrderBy(org => org.Name);
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
            switch (PlayerOptions(new string[] { "Harvest x1 Yucca Plant. (-2 Turn)", "Collect x2 Moths.(-1 Turn)", "Return to office" }))
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
                            CurrPlayer.PInventory.AddItem(new Item { 
                                Name = "Plant Leaf",
                                CurrItemType = Item.ItemType.PlantLeaf,
                                Amount = 1,
                                MerchantPrice = 1
                            });
                            CurrPlayer.RemovePlayerTurn(2);
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
                            CurrPlayer.PInventory.AddItem(new Item
                            {
                                Name = "Moth Eggs",
                                CurrItemType = Item.ItemType.MothEggs,
                                Amount = 2,
                                MerchantPrice = 1
                            });
                            CurrPlayer.RemovePlayerTurn(1);
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

        private List<string> PerformOrganismDailies()
        {
            List<string> logList = new List<string>();
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
                        if (p.Age >= 8 && !p.CanHarvest)
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
                        string msg= $"age {p.Age}" +
                            $"\nhydration: {p.Hydration}" +
                            $"\ncanHarvest:{p.CanHarvest}" +
                            $"\ndaysInCycle:{p.LifeCycleDayCount}" +
                            $"\nlifeStage:{p.CurrentLifeStage}";

                        if (!Utils.__PROD__) Console.WriteLine(msg);
                        logList.Add(msg);
                        break;

                    case Insect bugg:
                        bugg.IncreaseAge();
                        bugg.ConductBirthday();


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
                        string bmsg = $"-------" +
                            $"\nmoth idx:{OrgList.IndexOf(bugg)}" +
                            $"\nage::{bugg.Age}" +
                            $"\ndaysInCycle:{bugg.DaysInCycle}" +
                            $"\nlifestage: {bugg.CurrentCycleStage}";

                        if (!Utils.__PROD__) Console.WriteLine(bmsg);
                        logList.Add(bmsg);
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


                logList.Add($"{count} moths have died.");
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

            return logList;
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

        private string ConductPlantMothRatioCheck()
        {
            //string feedback = "No new messages.";
            Dictionary<string, int> ObligateRatios = PerformObligateRatioCalc();
            int acceptRange = 8;
            int dangerAmount = 85;
            int warnAmount = 65;
            int attenAmount = 55;

            // moth to plant
            bool mothsAcceptableRange = ObligateRatios["mRatio"] + acceptRange < ObligateRatios["pRatio"] || ObligateRatios["mRatio"] - acceptRange < ObligateRatios["pRatio"];
            bool mothsDangerRange = ObligateRatios["mRatio"] >= dangerAmount && ObligateRatios["mRatio"] > ObligateRatios["pRatio"];
            bool mothsWarningRange = ObligateRatios["mRatio"] >= warnAmount && ObligateRatios["mRatio"] > ObligateRatios["pRatio"];
            bool mothsAttentionRange = ObligateRatios["mRatio"] >= attenAmount && ObligateRatios["mRatio"] > ObligateRatios["pRatio"];
            // plant to moth
            bool plantsAcceptableRange = ObligateRatios["pRatio"] + acceptRange < ObligateRatios["mRatio"] || ObligateRatios["pRatio"] - acceptRange < ObligateRatios["mRatio"];
            bool plantsDangerRange = ObligateRatios["pRatio"] >= dangerAmount && ObligateRatios["pRatio"] > ObligateRatios["mRatio"];
            bool plantsWarningRange = ObligateRatios["pRatio"] >= warnAmount && ObligateRatios["pRatio"] > ObligateRatios["mRatio"];
            bool plantsAttentionRange = ObligateRatios["pRatio"] >= attenAmount && ObligateRatios["pRatio"] > ObligateRatios["mRatio"];

            string msg ="";
            // if there are too many moths, compared to plants
            if (mothsAcceptableRange && plantsAcceptableRange)
            {
                msg = "\nMoth to Plant ratio ok".Pastel("#12c754");
                WriteLine(msg);
                PlantMothRatioMsg = "No new messages.";

            }
            else if (mothsDangerRange || plantsDangerRange)
            {
                msg = "Warning! Environment out of balance, organism extinction imminent.".Pastel("#D50000");
                WriteLine("\n"+msg);
                PlantMothRatioMsg = msg;
            }
            else if (mothsWarningRange || plantsWarningRange)
            {
                msg = "Fix ratio before its too late".Pastel("#FF3333");
                WriteLine("\n" + msg);
                PlantMothRatioMsg = msg;
            }
            else if (mothsAttentionRange || plantsAttentionRange)
            {
                // warning
                msg = "Too many of one species!".Pastel("#FFA733");
                WriteLine("\n" + msg);
                PlantMothRatioMsg = msg;
            }

            return msg;
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

        private void ConductMarketCheck()
        {
            Clear();
            DisplayTopUI();
            WriteLine("~~~ Viewing Marketplace ~~~");
            switch (PlayerOptions(new string[] { "Buy items from market", "Sell items to Market", "Return to office" }))
            {
                case 1:
                    // player wants to buy items
                    Clear();
                    DisplayTopUI();
                    DesertMarket.SellItemsToPlayer(CurrPlayer);
                    break;
                case 2:
                    Clear();
                    DisplayTopUI();
                    DesertMarket.BuyItemsFromPlayer(CurrPlayer);
                    break;
                case 3:
                    return;
                default:
                    ConductMarketCheck();
                    break;
            }            
        }

    }
}
