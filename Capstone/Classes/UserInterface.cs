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
        public UserInterface()
        {
            VendingMachineFileReader VR = new VendingMachineFileReader();
            Dictionary<string, List<Items>> Inventory = VR.ReadInventory();
            VendingMachine VM = new VendingMachine(Inventory);
            this.VM = VM;
            MainMenu();
        }

        public void MainMenu()
        {
            Console.Clear();
            while(true)
            {
                Console.WriteLine("[1] Display vending machine items.");
                Console.WriteLine("[2] Purchase Item.");
                Console.WriteLine("[3] Exit vending machine");
                string userInput = Console.ReadLine();
                if (userInput == "1")
                {
                    DisplayItems();
                }
                else if (userInput == "2")
                {
                    MakePurchase();
                }
                else if (userInput == "3")
                {
                    break;
                }
            }     
        }

        public void DisplayItems()
        {
            Console.Clear();
            using (StreamReader sr = new StreamReader(Path.Combine(Environment.CurrentDirectory, "vendingmachine.csv")))
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine(sr.ReadLine());
                    }
                }
            Console.WriteLine("Press [1] to make selection.");
            Console.WriteLine("Press [2] to return to main menu.");
            string userInput = Console.ReadLine();
            if(userInput == "1")
            {
                MakePurchase();
            }
            if(userInput == "2")
            {
                MainMenu();
            }
        }

        public void MakePurchase()
        {
            List<string> productCodes = new List<string>();

            Console.WriteLine("Please enter the product code or [1] to return to main menu.");
            string userInput = Console.ReadLine().ToUpper();
            if(userInput == "1")
            {
                MainMenu();
            }
            productCodes.Add(userInput);

            while(true)
            {
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
            VM.Purchase(productCodes);
            
            
        }


    }
}
