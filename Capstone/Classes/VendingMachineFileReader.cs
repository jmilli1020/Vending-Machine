using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capstone.Classes
{
    public class VendingMachineFileReader
    {
        private const int SlotId = 0;
        private const int ProductName = 1;
        private const int Price = 2;
        private const int InitialQuantity = 5;

        public VendingMachineFileReader()
        {

        }

        public Dictionary<string, List<Item>> ReadInventory()
        {
            Dictionary<string, List<Item>> inventory = new Dictionary<string, List<Item>>();
            string directory = Environment.CurrentDirectory;
            string file = "vendingmachine.csv";
            string filePath = Path.Combine(directory, file);

            try
            {


                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] splits = line.Split('|');
                        List<Item> itemList = new List<Item>();

                        decimal cost = Decimal.Parse(splits[Price]);
                        string productName = (splits[ProductName] + " " + splits[SlotId]);
                        string type = "";

                        for (int i = 0; i < InitialQuantity; i++)
                        {
                            if (splits[0].Substring(0, 1) == "A")
                            {
                                type = "Crunch Crunch, Yum!";
                            }
                            else if (splits[0].Substring(0, 1) == "B")
                            {
                                type = "Munch Munch, Yum!";
                            }
                            else if (splits[0].Substring(0, 1) == "C")
                            {
                                type = "Glug Glug, Yum!";
                            }
                            else if (splits[0].Substring(0, 1) == "D")
                            {
                                type = "Chew Chew, Yum!";
                            }
                            Item Item = new Item(cost, productName, type);
                            itemList.Add(Item);
                            inventory[splits[0]] = itemList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return inventory;
        }
    }
}
