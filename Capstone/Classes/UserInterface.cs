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
        private VendingMachineFileWriter FW;
        private VendingMachine VM;
        private List<string> productCodes;
        private string userPayment;
        private string amountDue;
        private string amountPaid;

        public UserInterface()
        {
            VendingMachineFileWriter FW = new VendingMachineFileWriter();
            this.FW = FW;
            VendingMachineFileReader FR = new VendingMachineFileReader();
            Dictionary<string, List<Items>> Inventory = FR.ReadInventory();
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
                    Environment.Exit(0);
                }
            }          
        }

        public void ApplicationTitle()
        {
            Console.WriteLine((String.Format("{0," + ((Console.WindowWidth / 2) + ("Virtual Vending Machines Inc.".Length / 2)) + "}", "Virtual Vending Machines Inc.")));
        }

        public void DisplayItems() // I want to display the items persistently. Ideally I would also display remaining stock.
        {
            Console.Clear();
            ApplicationTitle();
            Console.WriteLine("Product Code".PadRight(15) + "Item".PadRight(15) + "Cost");
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

        public void ClearSelectionsPayments()
        {
            while (productCodes.Count > 0)
            {
                productCodes.Remove(productCodes[0]);
            }
            userPayment = "Q";
            VM.GetAmountPaid(userPayment);
            amountPaid = "";
            amountDue = "";
        }

        public void EnterSelections()
        {
            DisplayItems();
            Console.WriteLine("Please enter the product code or [1] return to main menu.");
            string userInput = Console.ReadLine().ToUpper();
            if (userInput == "1")
            {
                MainMenu();
            }
            else if (VM.DidUserEnterValidProductCode(userInput) && VM.RemoveItem(userInput))
            {
                productCodes.Add(userInput);
                Console.Clear();
            }
            else if (!VM.RemoveItem(userInput))
            {
                Console.WriteLine("Item is out of stock. Please press return and make a different selection.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("You've entered an invalid product code. Please press return and try again.");
                Console.ReadLine();
                EnterSelections();
            }
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
                    EnterSelections(); //This still needs some work. Count is never truly 0. Im going to come back to this later.
                }
                else if (userInput == "1")
                {
                    ClearSelectionsPayments();
                    MainMenu();
                }
                else if (VM.DidUserEnterValidProductCode(userInput) && VM.RemoveItem(userInput))
                {
                    productCodes.Add(userInput);
                    continue;
                }
                else if (!VM.RemoveItem(userInput))
                {
                    Console.WriteLine("Item is out of stock. Please press return and make a different selection.");
                    Console.ReadLine();
                    continue;
                }
                Console.WriteLine("You've entered an invalid product code. Please press return and try again.");
                Console.ReadLine();
            }
            Console.Clear();
            amountDue = VM.GetAmountDue().ToString("C");
            DisplayAmountDueAndAmountPaid();
            FeedMoney();
        }

        public void DisplayAmountDueAndAmountPaid()
        {
            ApplicationTitle();
            Console.WriteLine($"Your amount due is: {amountDue}");
            Console.WriteLine($"Amount paid: {amountPaid}");
        }

        public void FeedMoney()
        {
            string previousAmountPaid = "$0.00";
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
                    ClearSelectionsPayments();
                    MainMenu();
                }
                amountPaid = VM.GetAmountPaid(userPayment).ToString("C");
                FW.WriteToLog("FEED MONEY", previousAmountPaid, amountPaid);
                previousAmountPaid = amountPaid;
                Console.Clear();
                DisplayAmountDueAndAmountPaid();

            }
        }

        public void DisplayChangeAndEndTransaction()
        {
            ApplicationTitle();
            VM.CompleteTransaction();
            Console.WriteLine(VM.GetChange());
            foreach (string item in VM.GetTypes())
            {
                Console.WriteLine(item);  
            }
            Console.WriteLine("Thank you for shopping at Virtual Vending Machines.");
            Console.WriteLine("Press [1] to continue using the Virtual Vending Machine.");
            Console.WriteLine("Press [2] to exit the veding machine.");
            string userInput = Console.ReadLine();

            if (userInput == "1")
            {
                ClearSelectionsPayments();
                MainMenu();
            }
            else if (userInput == "2")
            {
                Environment.Exit(0);
            }
        }


    }
}
