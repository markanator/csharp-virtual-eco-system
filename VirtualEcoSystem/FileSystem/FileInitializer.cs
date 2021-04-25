using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static VirtualEcoSystem.ConsoleUIBuilder;


namespace VirtualEcoSystem.FileSystem
{
    public static class FileInitializer
    {
        public static void CheckForSaveFile()
        {
            Console.WriteLine("Checking for savegame data...");

            if (File.Exists("savegame.wad"))
            {
                Console.WriteLine("Save Game Found");
            }
            else
            {
                Console.WriteLine("Missing Save File");
                CreateNewSaveFile();
            }

            WaitForInput();
        }

        public static void CreateNewSaveFile()
        {
            // create a file
            FileStream fs = File.Create("./savegame.wad");
            fs.Close();
            DateTime createdAt = new DateTime();
            // write to it
            BinaryFormatter.WriteToBinaryFile<DateTime>("./savegame.wad", createdAt);
            // notify user
            Console.WriteLine("Save Game Created");
        }
    }
}
