using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using VirtualEcoSystem.Organisms;
using static System.Console;

namespace VirtualEcoSystem
{
    public static class Utils
    {
        public static string SaveGameFile = "savegame.txt";

        public static Random RandomGen = new Random();

        public static bool __PROD__ = false;

        public static void RemoveOrgsFromMain(List<Organism> mainList, List<Organism> removeList)
        {
            foreach(var org in removeList)
            {
                mainList.Remove(org);
            }

            mainList.OrderBy(org => org.GetType());
        }

        public static void AddOrgsToMain(List<Organism> mainList, List<Organism> newList)
        {
            foreach (var org in newList)
            {
                mainList.Add(org);
            }

            mainList.OrderBy(org => org.GetType());
        }

        public static bool SaveFileExsists()
        {
            Console.WriteLine("Checking for savegame data...");

            if (File.Exists(Utils.SaveGameFile))
            {
                Console.WriteLine("Save Game Found");
                return true;
            }
            else
            {
                Console.WriteLine("Missing Save File");
                //CreateNewSaveFile();
                return false;
            }
        }


    }



}