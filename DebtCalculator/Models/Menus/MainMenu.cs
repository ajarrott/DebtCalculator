using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Models.Menus
{
    internal static partial class Menu
    {
        static decimal _totalIncome = 0.00m;
        static bool quit = false;

        public static void DisplayMainMenu()
        {
            ConsoleKeyInfo keyInfo;

            do
            {
                Console.Clear();
                Console.WriteLine("Debt Calculator");
                Console.WriteLine("---------------");
                Console.WriteLine("1: Debt Options (Current Total: {0:C})", DebtCollection.TotalDebt);
                Console.WriteLine("2: Modify Income allocated to debt (Currently: {0:C}, need {1:C})", _totalIncome, DebtCollection.TotalRequiredIncome);
                Console.WriteLine("3: Payoff Calculator");
                Console.WriteLine("4: Save to Xml");
                Console.WriteLine("5: Load from Xml");
                Console.WriteLine("Q: Exit");
                Console.WriteLine("---------------");
                Console.Write("Input: ");

                keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        // Debt Options
                        Menu.DisplayDebtOptions();
                        break;
                    case ConsoleKey.D2:
                        //Modify Income
                        Menu.DisplayModifyIncome();
                        break;
                    case ConsoleKey.D3:
                        //Calculate Payoff amounts
                        Menu.CalculatePayoffs();
                        break;
                    case ConsoleKey.D4:
                        //Save
                        Menu.DisplaySave();
                        break;
                    case ConsoleKey.D5:
                        Menu.DisplayLoad();
                        //Load
                        break;
                    case ConsoleKey.Q:
                        quit = true;
                        break;
                }
            } while (!quit) ;
        }
    }
}
