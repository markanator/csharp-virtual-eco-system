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
        

        public Environment(string _name, string _desc)
        {
            this.Name = _name;
            this.Description = _desc;
            // get a temp from arr
            int preTemp = AverageTempList[RandGen.Next(0, AverageTempList.Count - 1)];
            // hold a temp value
            int currTempIndex = preTemp > 85 ? 0 + RandGen.Next(11) : 0 - RandGen.Next(11);
            // assign temperature
            this.CurrentTemp = currTempIndex + preTemp;
        }

        public string FetchCurrentTempFromEnvironment()
        {
            return $"The current temperature in the {this.Name} is: {CurrentTemp}°F";
        }

        public string GenerateRandomEvent()
        {
            if (CurrentTemp >= 100)
            {
                return "FIRE";
            } 
            else if (CurrentTemp < 100 && CurrentTemp >= 90)
            {
                return "DROUGHT";
            }
            else if (CurrentTemp < 90 && CurrentTemp >= 75)
            {
                // lets make is mostly sunny
                return   "SUNNY";
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

            throw new NotImplementedException("Oops, no event thing caught");
        }
    }
}