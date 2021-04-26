using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static VirtualEcoSystem.Utils;
using VirtualEcoSystem.Organisms;
using VirtualEcoSystem.Events;
using static System.Console;

namespace VirtualEcoSystem.Entity
{
    [Serializable]
    public class Enviro
    {
        public string Name;
        public string Description;
        public int CurrentTemp;
        public List<int> AverageTempList = new List<int> { 
            82,
            94,
            89,
            71,
            95,
            79,
            86
            };
        public string CurrentEvent;

        public Enviro() { }
        public Enviro(string _name, string _desc)
        {
            this.Name = _name;
            this.Description = _desc;
            this.CurrentEvent = "Normal Day";
            PerformDailyWeatherChange(true);
        }

        public string FetchCurrentTempFromEnvironment()
        {
            return $"The current temperature in the {this.Name} is: {CurrentTemp}°F";
        }

        // TODO, WEATHER SYSTEM using delegates?
        private string GenerateRandomEvent()
        {
            if (RandomGen.Next(0,3) >= 1)
            {
                if (CurrentTemp >= 104)
                {
                    //Fire.CustomEvent(_orgList);
                    return "Fire";
                }
                else if (CurrentTemp < 104 && CurrentTemp >= 99)
                {
                    //Drought.CustomEvent(_orgList);
                    return "Drought";
                }
                else if (CurrentTemp < 99 && CurrentTemp >= 75)
                {
                    // lets make is mostly sunny
                    //Sunny.CustomEvent(_orgList);
                    return "Sunny";
                }
                else if (CurrentTemp < 75 && CurrentTemp >= 65)
                {
                    int alternativeNum = RandomGen.Next(0, 2);
                    if (alternativeNum <= 0)
                    {
                        //Overcast.CustomEvent(_orgList);
                        return "Overcast";
                    }
                    else
                    {
                        //Flood.CustomEvent(_orgList);
                        return "Flood";
                    }
                }
                else
                {
                    //Sunny.CustomEvent(_orgList);
                    return "Sunny";
                }
            } 
            else
            {
                return "Normal Day";
            }
        }

        public void PerformDailyWeatherChange(bool _initial =false)
        {
            // get a temp from arr
            int preTemp = AverageTempList[RandomGen.Next(0, AverageTempList.Count - 1)];
            // hold a temp value
            int currTempIndex = preTemp > 85 ? 0 + RandomGen.Next(11) : 0 - RandomGen.Next(11);
            // assign temperature
            this.CurrentTemp = currTempIndex + preTemp;
            // generate an event, ran only once
            this.CurrentEvent = _initial ? "Normal Day" : GenerateRandomEvent();
        }

        public void PerformTemperatureEvent(List<Organism> _orglist)
        {
            switch (this.CurrentEvent)
            {
                case "Fire":
                    Fire.CustomEvent(_orglist);
                    break;
                case "Drought":
                    Drought.CustomEvent(_orglist);
                    break;
                case "Flood":
                    Flood.CustomEvent(_orglist);
                    break;
                case "Overcast":
                    Overcast.CustomEvent(_orglist);
                    break;
                case "Sunny":
                    Sunny.CustomEvent(_orglist);
                    break;
                default:
                    Console.WriteLine("Normal Day");
                    break;
            }
        }

        // MAIN MENU ACTIONS
        public void ConductWeatherCheck()
        {
            //if (CurrPlayer.PlayerConstitutionCheck())
            //{
            WriteLine("~~~ CHECKING WEATHER ~~~");
            WriteLine(this.FetchCurrentTempFromEnvironment());
            WriteLine("Current Events: " + this.CurrentEvent);
            // implement moisture check based on weather
            // TODO: 
            //CurrPlayer.RemovePlayerTurn();
            //}
            //else
            //{
            //    WriteLine("Unable to Perform Request.");
            //}

            ConsoleUIBuilder.WaitForInput();
        }

    }
}