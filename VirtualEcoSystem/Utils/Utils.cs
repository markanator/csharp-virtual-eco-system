using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using VirtualEcoSystem.Organisms;
using System.Xml;
using VirtualEcoSystem.Items;

namespace VirtualEcoSystem
{
    public static class Utils
    {
        public const string SaveGameFileTxt = "VirtEcoSave.txt";
        public const string MerchantXmlFile = "../../Data/MerchantStock.xml";
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

            if (File.Exists(Utils.SaveGameFileTxt))
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
    }



}