using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        private Dictionary<string,List<Items>> inventory;
        VendingMachineFileWriter FW = new VendingMachineFileWriter();

        public VendingMachine(Dictionary<string, List<Items>> Inventory)
        {
            this.inventory = Inventory;
            this.amountDue = 0;
        }

        private decimal amountDue;
        private decimal amountPaid;

        public decimal GetAmountDue(List<string> ProductCodes)//Calculates amount due based off list of product codes
        {
            decimal cost = 0;

            foreach (string productCode in ProductCodes)
            {
                foreach (KeyValuePair<string, List<Items>> KVP in inventory)
                {
                    if (KVP.Key == productCode)
                    {
                        List<Items> item = KVP.Value;
                        cost = item[0].GetCost();
                    }
                }
                amountDue += cost;              
            }
            return amountDue;
        }

        public decimal GetAmountPaid(string userPayment)
        {
            if (userPayment == "1")
            {
                amountPaid++;
                return amountPaid;
            }
            else if (userPayment == "2")
            {
                amountPaid += 2;
                return amountPaid;
            }
            else if (userPayment == "3")
            {
                amountPaid += 5;
                return amountPaid;
            }
            else if (userPayment == "4")
            {
                amountPaid += 10;
                return amountPaid;
            }
            else if (userPayment == "Q")
            {
                amountPaid = 0;
                amountDue = 0;
            }
            return amountPaid;
        }

        public bool DidUserPayEnough()
        {
            if (amountPaid > amountDue)
            {
                return true;
            }
            return false;
        }

        public string GetChange()
        {
            Change change = new Change();
            return "Your change is: " + (amountDue-amountPaid).ToString("C") + "\n"
                + "Returning: " + change.GetChange(amountPaid, amountDue);
        }

        public List<string> GetTypes(List<string> ProductCodes)
        {
            List<string> types = new List<string>();
            foreach (string productCode in ProductCodes)
            {
                foreach (KeyValuePair<string, List<Items>> KVP in inventory)
                {
                    if (KVP.Key == productCode)
                    {
                        List<Items> item = KVP.Value;
                        types.Add(item[0].ReturnType());
                    }
                }
            }
            return types;
        }

        public void CompleteTransaction(List<string> productCodes)
        {
            string itemType = "";
            string productName = "";
            foreach (string productCode in productCodes)
            {
                foreach (KeyValuePair<string, List<Items>> KVP in inventory)
                {
                    if (productCode == KVP.Key)
                    {
                        List<Items> items = KVP.Value;
                        itemType = items[0].ReturnType();
                        productName = items[0].GetProductName();
                        KVP.Value.Remove(KVP.Value[0]);
                        FW.WriteToLog(productName, amountPaid.ToString("C"), amountDue.ToString("C"));
                    }
                }
                Console.WriteLine(itemType);
            }
        }
    }
}
