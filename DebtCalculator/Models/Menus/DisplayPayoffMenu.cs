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

            decimal allowance = _totalIncome;
            decimal extraIncome = _totalIncome - DebtCollection.TotalRequiredIncome;

            var mostInterest = DebtCollection.GetDebts.OrderByDescending(x => x.GetCurrentMinimumPayment().Interest).Select(x => new Debt(x.LoanName, x.Apr, x.CurrentBalance)).ToList();
            List<Payment> allPayments = new List<Payment>();

            // highest will be first in list
            while (mostInterest.Any(x => x.CurrentBalance > 0.00m))
            {
                extraIncome = _totalIncome - (mostInterest.Sum(x => x.GetCurrentMinimumPayment().Amount));

                for (int i = 0; i < mostInterest.Count(); i++)
                {
                    var payment = mostInterest[i].MakeSinglePayment(mostInterest[i].GetCurrentMinimumPayment().Amount + extraIncome);

                    // don't add 0 payment
                    if (payment.Amount == 0.00m) continue;

                    allPayments.Add(payment);
                }

                mostInterest = mostInterest.OrderByDescending(x => x.GetCurrentMinimumPayment().Interest).ToList();
            }


            foreach(var pmt in allPayments)
            {
                Console.WriteLine(pmt.ToString());
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
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
                } while (decimal.TryParse(Console.ReadLine(), out amount) && amount < debts[i].GetCurrentMinimumPayment().Amount);

                amountsToPay.Add(amount);
            }

            for (int i = 0; i < debts.Count; i++)
            {
                Console.WriteLine(debts[i].CalculateEstimatedPayoff(amountsToPay[i]).ToString());
            }
        }
    }
}
