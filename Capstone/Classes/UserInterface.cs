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
            Console.WriteLine("Please enter the product code or [1] to return to main menu.");
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
                Console.WriteLine("Enter another product code, [C] to confirm selection(s), or [1] to return to main menu.");
                userInput = Console.ReadLine().ToUpper();
                if (userInput == "C")
                {
                    break;
                }
                else if (userInput == "1")
                {
                    MainMenu();
                }
                else
                {
                    productCodes.Add(userInput);
                }
            }
            Console.Clear();
            DisplayAmountDueAndAmountPaid();
        }

        public void DisplayAmountDueAndAmountPaid()
        {
            Console.WriteLine($"Your amount due is: {VM.GetAmountDue(productCodes)}");
            Console.WriteLine($"Amount paid: {VM.GetAmountPaid(userPayment)}");
            FeedMoney();
        }

        public void FeedMoney()
        {
            while (true)
            {
                DisplayAmountDueAndAmountPaid();
                Console.WriteLine("Press [1] to add 1 dollar.");
                Console.WriteLine("Press [2] to add 2 dollars.");
                Console.WriteLine("Press [3] to add 5 dollars.");
                Console.WriteLine("Press [4] to add 10 dollars.");
                userPayment = Console.ReadLine().ToLower();
                VM.GetAmountPaid(userPayment);
            }
        }


    }
}
