using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Models.Menus
{
    internal static partial class Menu
    {
        public static void CalculatePayoffs()
        {
            decimal tempIncome = 0.00m;

            ConsoleKeyInfo key = dummyKey;
            bool validInput = false;
            do
            {
                Console.Clear();
                Console.WriteLine("---------------------");
                Console.WriteLine("Payoff Calculator");
                Console.WriteLine("---------------------");
                Console.WriteLine("1: Manual Entry (Estimate Payoff)");
                Console.WriteLine("2: Generate Payment Plan");
                Console.WriteLine("3: Update Current Excess Income ({0:C})", _totalIncome);
                Console.WriteLine("B: Go Back");
                Console.WriteLine("---------------------");
                Console.Write("Selection: ");

                key = Console.ReadKey();
                Console.WriteLine();

                // keep looping while updating the excess income
                validInput = (key.KeyChar >= '1' && key.KeyChar <= '2');

                if (key.Key == ConsoleKey.B)
                {
                    return;
                }

                if (key.Key == ConsoleKey.D3)
                {
                    do
                    {
                        Console.Write("Enter new excess income amount (e.g. 5000): ");
                    } while (!decimal.TryParse(Console.ReadLine(), out tempIncome));

                    _totalIncome = tempIncome;
                }

            } while (!validInput);

            if(key.Key == ConsoleKey.D1)
            {
                CalculateManualEntry();
            }

            if(key.Key == ConsoleKey.D2)
            {
                CalculateOptimalPayoff();
            }
            

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }

        private static void CalculateOptimalPayoff()
        {
            if(_totalIncome < DebtCollection.TotalRequiredIncome)
            {
                Console.WriteLine("Must have at least {0:C} (current income: {1:C})", DebtCollection.TotalRequiredIncome, _totalIncome);
                return;
            }

            var highestMinFirst = DebtCollection.GetDebts.OrderBy(x => x.GetMinimumPayment());
            
            for(int i = 0; i < highestMinFirst.Count(); i++)
            {
                
            }
        }

        private static void CalculateManualEntry()
        {
            List<decimal> amountsToPay = new List<decimal>();
            var debts = DebtCollection.GetDebts;

            for (int i = 0; i < debts.Count; i++)
            {
                decimal amount = 0.00m;
                do
                {
                    Console.WriteLine(debts[i].ToString());
                    Console.Write("Amount to Pay (must be above min payment): ");
                } while (decimal.TryParse(Console.ReadLine(), out amount) && amount < debts[i].GetMinimumPayment());

                amountsToPay.Add(amount);
            }

            for (int i = 0; i < debts.Count; i++)
            {
                Console.WriteLine(debts[i].CalculateEstimatedPayoff(amountsToPay[i]).ToString());
            }
        }
    }
}
