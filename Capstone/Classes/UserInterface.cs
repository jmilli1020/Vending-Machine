using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Capstone.Classes
{
    public class UserInterface
    {
        private VendingMachine VM;
        private List<string> productCodes;
        private string userPayment;
        private string amountDue;
        private string amountPaid;

        public UserInterface()
        {
            VendingMachineFileReader VR = new VendingMachineFileReader();
            Dictionary<string, List<Items>> Inventory = VR.ReadInventory();
            VendingMachine VM = new VendingMachine(Inventory);
            this.VM = VM;
            List<string> productCodes = new List<string>();
            this.productCodes = productCodes;
            MainMenu();
        }

        public void MainMenu()
        {
            Console.Clear();
            while(true)
            {
                DisplayItems();
                Console.WriteLine("[1] Make a selection.");
                Console.WriteLine("[2] Exit vending machine");
                string userInput = Console.ReadLine();
                if (userInput == "1")
                {
                    EnterSelections();
                }
                else if (userInput == "2")
                {
                    break;
                }
            }
            
        }

        public void DisplayItems() //  want to display the items persistently. Ideally I would also display remaining stock.
        {
            Console.Clear();
            using (StreamReader sr = new StreamReader(Path.Combine(Environment.CurrentDirectory, "vendingmachine.csv")))
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine("".PadRight(5) + sr.ReadLine()); //Im thinking about making this centered.
                    }
                }
        }

        public string DisplayCurrentSelections()
        {
            string currentSelections = "";
            foreach (string item in productCodes)
            {              
                currentSelections += " " + item.ToUpper();
            }
            return currentSelections;
        }

        public void EnterSelections()
        {
            DisplayItems();
            Console.WriteLine("Please enter the product code or [1] return to main menu.");
            string userInput = Console.ReadLine().ToUpper();
            if(userInput == "1")
            {
                MainMenu();
            }
            productCodes.Add(userInput);
            Console.Clear();

            while(true)
            {
                DisplayItems();
                Console.WriteLine("You've currently selected: " + DisplayCurrentSelections());
                Console.WriteLine("Enter another product code, or: \n[C] to confirm selection(s) \n[1] to cancel current selections and return to the main menu.");
                userInput = Console.ReadLine().ToUpper();
                if (userInput == "C" && productCodes.Count > 0)
                {
                    break;
                }
                else if (userInput == "C" && productCodes.Count == 0)
                {
                    Console.WriteLine("You must enter a product code before confirming your selection.");
                    EnterSelections(); //This still needs some work. Im going to come back to it later.
                }
                else if (userInput == "1")
                {
                    while (productCodes.Count > 0)
                    {
                        productCodes.Remove(productCodes[0]);
                    }
                    MainMenu();
                }
                else
                {
                    productCodes.Add(userInput);
                }
            }
            Console.Clear();
            amountDue = VM.GetAmountDue(productCodes).ToString("C");
            DisplayAmountDueAndAmountPaid();
            FeedMoney();
        }

        public void DisplayAmountDueAndAmountPaid()
        {
            Console.WriteLine($"Your amount due is: {amountDue}");
            Console.WriteLine($"Amount paid: {amountPaid}");
        }

        public void FeedMoney()
        {
            while (true)
            {
                Console.WriteLine("Press [1] to add 1 dollar.");
                Console.WriteLine("Press [2] to add 2 dollars.");
                Console.WriteLine("Press [3] to add 5 dollars.");
                Console.WriteLine("Press [4] to add 10 dollars.");
                Console.WriteLine("Press [C] to confirm payment");
                Console.WriteLine("Press [Q] to cancel selections. Return to main menu.");
                userPayment = Console.ReadLine().ToUpper();
                if (userPayment == "C")
                {
                    if(VM.DidUserPayEnough())
                    {
                        Console.Clear();
                        DisplayChangeAndEndTransaction();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Please insert more money before confirming payment.");
                        DisplayAmountDueAndAmountPaid();
                        FeedMoney();
                    }                   
                }
                else if (userPayment == "Q")
                {
                    while (productCodes.Count > 0)
                    {
                        productCodes.Remove(productCodes[0]);
                    }
                    VM.GetAmountPaid(userPayment);
                    amountPaid = "";
                    amountDue = "";
                    MainMenu();
                }
                amountPaid = VM.GetAmountPaid(userPayment).ToString("C");
                Console.Clear();
                DisplayAmountDueAndAmountPaid();
            }
        }

        public void DisplayChangeAndEndTransaction()
        {
            Console.WriteLine(VM.GetChange());
            foreach (string item in VM.GetTypes(productCodes))
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }


    }
}
