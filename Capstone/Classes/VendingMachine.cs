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
                    CompleteTransaction();
                }
                CompleteTransaction();
        }

        public void FeedMoney()
        {
            while(true)
            {
                Console.WriteLine("Press [1] to add 1 dollar.");
                Console.WriteLine("Press [2] to add 2 dollars.");
                Console.WriteLine("Press [3] to add 5 dollars.");
                Console.WriteLine("Press [4] to add 10 dollars.");
                Console.WriteLine("Enter [ok] to confirm payment");
                string userInput = Console.ReadLine();
                if (userInput == "1")
                {
                    amountPaid++;
                }
                else if(userInput == "2")
                {
                    amountPaid += 2;
                }
                else if(userInput == "3")
                {
                    amountPaid += 5;
                }
                else if(userInput == "4")
                {
                    amountPaid += 10;
                }
                else
                {
                    Console.WriteLine("Invalid entry. Please try again");
                }
                Console.WriteLine($"Amount paid: {amountPaid}");
                Console.WriteLine($"Amount due: {amountDue}");
                if(userInput == "ok")
                {
                    break;
                }
            }

        }
        
        public void CompleteTransaction()
        {

        }
    }
}
