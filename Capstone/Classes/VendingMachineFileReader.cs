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
        public VendingMachineFileReader()
        {

        }

        public Dictionary<string, List<Items>> ReadInventory()
        {
            Dictionary<string, List<Items>> Inventory = new Dictionary<string, List<Items>>();
            string directory = Environment.CurrentDirectory;
            string file = "vendingmachine.csv";
            string filePath = Path.Combine(directory, file);

            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] splits = line.Split('|');
                    List<Items> ItemsList = new List<Items>();
                    decimal cost = Decimal.Parse(splits[2]);
                    string productName = (splits[1] + " " + splits[0]);
                    string type = "";

                    for (int i = 0; i < 5; i++)
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
                        Items Item = new Items(cost, productName, type);
                        ItemsList.Add(Item);
                        Inventory[splits[0]] = ItemsList;
                    }
                }
            }
            return Inventory;
        }
    }
}
