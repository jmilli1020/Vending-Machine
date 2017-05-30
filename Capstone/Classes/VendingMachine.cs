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
        private Dictionary<string, List<Item>> inventory;
        private List<Item> selectedItems = new List<Item>();
        private VendingMachineFileWriter fw = new VendingMachineFileWriter();

        public VendingMachine(Dictionary<string, List<Item>> inventory)
        {
            this.inventory = inventory;
            this.amountDue = 0;
        }

        private decimal amountDue;
        private decimal amountPaid;

        public decimal GetAmountDue()
        {
            decimal cost = 0;

            foreach (Item item in selectedItems)
            {
                cost = item.GetCost();
                amountDue += cost;
            }
            return amountDue;
        }

        public decimal AddMoney(string userPayment)
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
            return inventory.ContainsKey(productCode);

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
            string changeInCoins = "Your change is: " + (amountDue - amountPaid).ToString("C") + "\n"
                + "Returning: " + change.GetChange(amountPaid, amountDue);
            fw.WriteToLog("GIVE CHANGE", amountDue.ToString("C"), "$0.00");

            return changeInCoins;
        }

        public bool RemoveItem(string userInput)
        {
            foreach (KeyValuePair<string, List<Item>> KVP in inventory)
            {
                if (userInput == KVP.Key && KVP.Value.Count > 0)
                {
                    selectedItems.Add(KVP.Value[0]);
                    KVP.Value.Remove(KVP.Value[0]);
                    return true;
                }
            }
            return false;
        }

        public void AddItemsBack(List<string> productCodes)
        {
            List<Item> tempItemsToAdd = new List<Item>();
            for (int i = 0; i < productCodes.Count; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    tempItemsToAdd.Add(selectedItems[i]);
                }
                while (tempItemsToAdd.Count > 0)
                {
                    tempItemsToAdd.Remove(tempItemsToAdd[0]);
                }
            }
            while (selectedItems.Count > 0)
            {
                selectedItems.Remove(selectedItems[0]);
            }
        }

        public List<string> GetTypes()
        {
            List<string> types = new List<string>();
            foreach (Item item in selectedItems)
            {
                types.Add(item.ReturnType());
            }
            return types;
        }

        public void CompleteTransaction()
        {
            string productName = "";

            foreach (Item item in selectedItems)
            {
                productName = item.GetProductName();
                fw.WriteToLog(productName, amountPaid.ToString("C"), amountDue.ToString("C"));
            }
            while (selectedItems.Count > 0)
            {
                selectedItems.Remove(selectedItems[0]);
            }
        }
    }
}
