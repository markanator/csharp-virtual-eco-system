// XML loader from Vending Machine Exercise
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using Pastel;
using VirtualEcoSystem.Organisms;
using VirtualEcoSystem.Items;

namespace VirtualEcoSystem
{
    public static class Utils
    {
        // enable this to remove daily outputs
        public static bool __PROD__ = false;
        // save file locations
        public const string SaveGameFileTxt = "VirtEcoSave.txt";
        public const string MerchantXmlFile = "../../Data/MerchantStock.xml";
        // styling stuff
        public static Dictionary<string, string> Color = new Dictionary<string, string>()
        {
            { "Primary", "#0d6efd" }, // blue
            { "Success", "#20c997" }, // green
            { "Warning", "#ffc107" }, // yellow
            { "Danger", "#dc3545" },  // red
            // Misc..
            { "Noun", "#6f42c1" },    // purple
            { "Actions", "#0dcaf0" }, // cyan
            { "Other", "#fd7e14" },   // orange
        };

        public static Random RandomGen = new Random();

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
            if (!Utils.__PROD__) Console.WriteLine("Checking for savegame data...");

            if (File.Exists(Utils.SaveGameFileTxt))
            {
                if (!Utils.__PROD__) Console.WriteLine("Save Game Found".Pastel(Utils.Color["Success"]));
                return true;
            }
            else
            {
                if (!Utils.__PROD__) Console.WriteLine("Missing Save File".Pastel(Utils.Color["Warning"]));
                //CreateNewSaveFile();
                return false;
            }
        }

        public static List<Item> LoadMerchantXmlItems()
        {
            List<Item> tempStock = new List<Item>();
            XmlDocument doc = new XmlDocument();

            doc.Load(MerchantXmlFile);

            XmlNode root = doc.DocumentElement;

            XmlNodeList xmlItemList = root.SelectNodes("/items/item");

            foreach (XmlElement xItem in xmlItemList)
            {
                Item tempItem = new Item();
                tempItem.Name = xItem.GetAttribute("name");
                tempItem.Amount = Convert.ToInt32(xItem.GetAttribute("stock"));
                tempItem.CurrItemType = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), xItem.GetAttribute("type"));
                tempItem.MerchantPrice = Convert.ToInt32(xItem.GetAttribute("price"));
                tempStock.Add(tempItem);
            }

            return tempStock;

        }

        public static double CalcMerchantPurchasePrice(double ogItemPrice)
        {
            double merchantBuyPrice = ogItemPrice * .5;

            if (merchantBuyPrice <= 0)
            {
                return 1.0;
            } 
            else
            {
                return merchantBuyPrice;
            }
        }

        public static void CreateLogFile(List<string> dailyLog,int dayCount)
        {
            //var fs = File.Open("dailyLog.txt",FileMode.OpenOrCreate,FileAccess.ReadWrite);
            string logPath = $"logs/dailyLog_day_{dayCount}.txt";

            if (!File.Exists(logPath))
            {
                var fs = File.Create(logPath);
                fs.Close();
            }

            File.WriteAllLines(logPath, dailyLog);
        }
    }



}