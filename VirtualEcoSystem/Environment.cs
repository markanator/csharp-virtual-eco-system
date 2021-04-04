using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualEcoSystem
{
    public class Environment
    {
        public string Name;
        //public int GenerateRandomEvent;
        public string Description;
        public int CurrentTemp;
        private Random RandGen = new Random();
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
        

        public Environment(string _name, string _desc)
        {
            this.Name = _name;
            this.Description = _desc;
            PerformDailyWeatherChange();
        }

        public string FetchCurrentTempFromEnvironment()
        {
            return $"The current temperature in the {this.Name} is: {CurrentTemp}°F";
        }

        // TODO, WEATHER SYSTEM using delegates?
        private string GenerateRandomEvent()
        {
            if (RandGen.Next(0,3) >= 1)
            {
                if (CurrentTemp >= 104)
                {
                    return "FIRE";
                }
                else if (CurrentTemp < 104 && CurrentTemp >= 99)
                {
                    return "DROUGHT";
                }
                else if (CurrentTemp < 99 && CurrentTemp >= 75)
                {
                    // lets make is mostly sunny
                    return "SUNNY";
                }
                else if (CurrentTemp < 75 && CurrentTemp >= 65)
                {
                    int alternativeNum = RandGen.Next(0, 1);
                    return alternativeNum <= 0 ? "OVERCAST" : "FLOOD";
                }
                else
                {
                    return "SUNNY";
                }
            } 
            else
            {
                return "Normal Day";
            }
        }

        public void PerformDailyWeatherChange()
        {
            // get a temp from arr
            int preTemp = AverageTempList[RandGen.Next(0, AverageTempList.Count - 1)];
            // hold a temp value
            int currTempIndex = preTemp > 85 ? 0 + RandGen.Next(11) : 0 - RandGen.Next(11);
            // assign temperature
            this.CurrentTemp = currTempIndex + preTemp;
            // generate an event, ran only once
            this.CurrentEvent = GenerateRandomEvent();
        }
    }
}