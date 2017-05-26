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

        //Initiates Purchase Process from start to finish.(Purchase/FeedMoney/GetChange/CompleteTransaction
        public void Purchase(List<string> ProductCodes)
        {
            string actionPerformed = "GIVE CHANGE";
            decimal cost = 0;

            //This Finds Out the Cost of each item in a list of user product code inputs and adds there total cost to the amount due
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
                if (cost == 0)
                {
                    Console.WriteLine("You've entered an incorrect product code. Enter any key to return to main menu.");
                    Console.ReadLine();
                    UserInterface UI = new UserInterface();
                }
                amountDue += cost;            
            }
            //Asks the user to keep adding money until theyve paid as much or more than the amount due.
            Console.WriteLine($"Your total is ${amountDue}.");
                while(amountPaid < amountDue)
                {
                    FeedMoney();
                    if (amountPaid < amountDue)
                        {
                            Console.WriteLine("Not enough money. Please try again.");
                        }
                }
                if(amountPaid > amountDue)
                {
                    Change c = new Change();
                    Console.WriteLine(c.GetChange(amountPaid,amountDue));
                }
                else if (amountPaid == amountDue)
                {
                    CompleteTransaction(ProductCodes);
                }
                CompleteTransaction(ProductCodes);
            FW.WriteToLog(actionPerformed, amountDue.ToString("C"), 0.ToString("C"));
        }

        public void FeedMoney()
        {
            string actionPerformed = "FEED MONEY:";
            while(true)
            {
                Console.WriteLine("Press [1] to add 1 dollar.");
                Console.WriteLine("Press [2] to add 2 dollars.");
                Console.WriteLine("Press [3] to add 5 dollars.");
                Console.WriteLine("Press [4] to add 10 dollars.");
                Console.WriteLine("Enter [ok] to confirm payment");
                string userInput = Console.ReadLine().ToLower();
                int userInt = 0;
                if(userInput == "ok")
                {
                    break;
                }
                else if (userInput == "1")
                {
                    userInt = 1;
                    amountPaid++;
                }
                else if(userInput == "2")
                {
                    userInt = 2;
                    amountPaid += 2;
                }
                else if(userInput == "3")
                {
                    userInt = 5;
                    amountPaid += 5;
                }
                else if(userInput == "4")
                {
                    userInt = 10;
                    amountPaid += 10;
                }
                else
                {
                    Console.WriteLine("Invalid entry. Please try again");
                }

                FW.WriteToLog(actionPerformed, userInt.ToString("C"), amountPaid.ToString("C"));
                Console.WriteLine($"Amount paid: {amountPaid}");
                Console.WriteLine($"Amount due: {amountDue}");
            }

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
