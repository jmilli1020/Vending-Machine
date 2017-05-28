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
        private List<Items> SelectedItems;
        private VendingMachineFileWriter FW = new VendingMachineFileWriter();
        
        public VendingMachine(Dictionary<string, List<Items>> Inventory)
        {
            this.inventory = Inventory;
            this.amountDue = 0;
            List<Items> SelectedItems = new List<Items>();
            this.SelectedItems = SelectedItems;
        }

        private decimal amountDue;
        private decimal amountPaid;

        public decimal GetAmountDue()
        {
            decimal cost = 0;

            foreach (Items item in SelectedItems)
            {
                cost = item.GetCost();
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

        public bool DidUserEnterValidProductCode(string productCode)
        {
            foreach (KeyValuePair<string, List<Items>> KVP in inventory)
            {
                    if (KVP.Key == productCode)
                    {
                        return true;
                    }
            }
            return false;
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
            string changeInCoins = "Your change is: " + (amountDue-amountPaid).ToString("C") + "\n"
                + "Returning: " + change.GetChange(amountPaid, amountDue);
            FW.WriteToLog("GIVE CHANGE", amountDue.ToString("C"), "$0.00");
            return changeInCoins;
        }

        public bool RemoveItem(string userInput)
        {
                foreach (KeyValuePair<string, List<Items>> KVP in inventory)
                {
                    if (userInput == KVP.Key && KVP.Value.Count > 0)
                    {
                        SelectedItems.Add(KVP.Value[0]);
                        KVP.Value.Remove(KVP.Value[0]);
                        return true;
                    }
                }
                return false;
        }

        //public AddItemsBack(List<string> productCodes)
        //{
        //    foreach (string productCode in productCodes)
        //    {
        //        foreach (KeyValuePair<string, List<Items>> KVP in inventory)
        //        {
        //            if (productCode == KVP.Key)
        //            {
        //                List<Items> items = KVP.Value;
        //                KVP.Value.Add(KVP.Value[0]);
        //            }
        //        }
        //    }
        //}

        public List<string> GetTypes()
        {
            List<string> types = new List<string>();
            foreach (Items item in SelectedItems)
            {
                types.Add(item.ReturnType());
            }
            return types;
        }

        public void CompleteTransaction()
        {
            string productName = "";

            foreach (Items item in SelectedItems)
            {
                productName = item.GetProductName();
                FW.WriteToLog(productName, amountPaid.ToString("C"), amountDue.ToString("C"));
            }
            while (SelectedItems.Count > 0)
            {
                SelectedItems.Remove(SelectedItems[0]);
            }
        }
    }
}
