using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualEcoSystem.Interfaces;
using VirtualEcoSystem.InventorySystem;
using VirtualEcoSystem.Items;
using System.IO;


namespace VirtualEcoSystem.Entity
{
    class Market
    {
        //private List<Item> MerchantStock;
        // ex: Water: [x25, $2]
        private Dictionary<Item,int> MerchantStock;
        private int Cash;
        public Market()
        {
            MerchantStock = new Dictionary<Item,int>();
            Cash = 500;
            LoadItems();
        }

        private void LoadItems()
        {
            string PathToFile = "../../Data/Merchant.txt";
            string [] itemsFromFile = File.ReadAllLines(PathToFile);

            foreach(string itemLine in itemsFromFile)
            {
                string[] itemBits = itemLine.Split(',');
                int itemInventoryAmount = Int32.Parse(itemBits[1]);
                int itemCost = Int32.Parse(itemBits[3]);
                MerchantStock.Add(new Item()
                { 
                    Name = itemBits[0],
                    Amount = itemInventoryAmount,
                    CurrItemType = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), itemBits[2])
                },
                itemCost
                );
            }
        }

        public void AddItem(Item _item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsItem(Item _item)
        {
            throw new NotImplementedException();
        }

        public int ItemCount(Item _item)
        {
            throw new NotImplementedException();
        }

        public List<string> FetchInventoryList()
        {
            List<string> tempItemsList = new List<string>();
            foreach(var item in MerchantStock)
            {
                tempItemsList.Add($"(x{item.Key.Amount}) {item.Key.Name} : ${item.Value}");
                //Console.WriteLine($"(x{item.Key.Amount}) {item.Key.Name} : ${item.Value}");
            }

            return tempItemsList;
        }

        public void RemoveItem(Item _item)
        {
            throw new NotImplementedException();
        }

        public void UseItem(Item _item)
        {
            // not being used by the market
        }
    }
}
