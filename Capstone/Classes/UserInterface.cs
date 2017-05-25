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
            MenuFlowStageOne();
        }

        public void MenuFlowStageOne()
        {
            Console.WriteLine("(1) Display vending machine items.");
            Console.WriteLine("(2) Purchase Item.");
            string userInput = Console.ReadLine();
            if (userInput == "1")
            {
                MenuFlowDisplayItems();
            }
            else if (userInput == "2")
            {
                MenuFlowMakePurchase();
            }
        }

        public void MenuFlowDisplayItems()
        {
            using (StreamReader sr = new StreamReader(Path.Combine(Environment.CurrentDirectory, "vendingmachine.csv")))
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine(sr.ReadLine());
                    }
                }
            Console.WriteLine("Press (1) to make selection");
            Console.ReadLine();
            MenuFlowMakePurchase();
        }

        public void MenuFlowMakePurchase()
        {
            bool toContinue = true;
            List<string> productCodes = new List<string>();
            while(toContinue)
            {
                Console.WriteLine("Please enter the product code.");
                productCodes.Add(Console.ReadLine());
                Console.WriteLine("Enter another product code or (q) to quit.");
                string quit = Console.ReadLine();
                if (quit == "q")
                {
                    toContinue = false;
                }
                continue; 
            }
            VM.Purchase(productCodes);
            
            
        }


    }
}
