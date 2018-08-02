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

            do
            {
                Console.Clear();
                Console.WriteLine("---------------------");
                Console.WriteLine("Payoff Calculator");
                Console.WriteLine("---------------------");
                Console.WriteLine("1: Manual Entry (Estimate Payoff)");
                Console.WriteLine("2: Generate Payment Plan (Highest Interest per Month First)");
                Console.WriteLine("3: Generate Payment Plan (Snowball, Lowest Balance First)");
                Console.WriteLine("4: Update Current Excess Income ({0:C}, {1:C} Required)", _totalIncome, DebtCollection.TotalRequiredIncome);
                Console.WriteLine("B: Go Back");
                Console.WriteLine("---------------------");
                Console.Write("Selection: ");

                key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.B)
                {
                    return;
                }

                if (key.Key == ConsoleKey.D1)
                {
                    CalculateManualEntry();
                }

                if (key.Key == ConsoleKey.D2)
                {
                    CalculateOptimalPayoff();
                }

                if (key.Key == ConsoleKey.D3)
                {
                    CalculateSnowballPayoff();
                }

                if (key.Key == ConsoleKey.D4)
                {
                    do
                    {
                        Console.Write("Enter new excess income amount (e.g. 5000): ");
                    } while (!decimal.TryParse(Console.ReadLine(), out tempIncome));

                    _totalIncome = tempIncome;
                }

            } while (true);
        }

        private static bool IncomeHighEnough()
        {
            if (_totalIncome >= DebtCollection.TotalRequiredIncome) return true;
            
            Console.WriteLine("Must have at least {0:C} (current income: {1:C})", DebtCollection.TotalRequiredIncome, _totalIncome);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            return false;
        }

        private static void CalculateSnowballPayoff()
        {
            if (!IncomeHighEnough()) return;

            decimal allowance = _totalIncome;
            decimal extraIncome = _totalIncome - DebtCollection.TotalRequiredIncome;

            // get copy of list
            var lowestFirst = DebtCollection.GetDebts.OrderBy(x => x.GetCurrentMinimumPayment().StartingBalance).Select(x => new Debt(x.LoanName, x.Apr, x.CurrentBalance)).ToList();
            List<Payment> allPayments = new List<Payment>();

            while(lowestFirst.Any(x => x.CurrentBalance > 0.00m))
            {
                var pmt = PaymentLogic(lowestFirst);
                allPayments.AddRange(pmt);

                lowestFirst = lowestFirst.OrderBy(x => x.GetCurrentMinimumPayment().StartingBalance).ToList();
            }

            DisplayPayments(allPayments);
        }

        private static void CalculateOptimalPayoff()
        {
            if (!IncomeHighEnough()) return;

            var mostInterest = DebtCollection.GetDebts.OrderByDescending(x => x.GetCurrentMinimumPayment().Interest).Select(x => new Debt(x.LoanName, x.Apr, x.CurrentBalance)).ToList();
            List<Payment> allPayments = new List<Payment>();

            // highest will be first in list
            while (mostInterest.Any(x => x.CurrentBalance > 0.00m))
            {
                var pmt = PaymentLogic(mostInterest);
                allPayments.AddRange(pmt);

                mostInterest = mostInterest.OrderByDescending(x => x.GetCurrentMinimumPayment().Interest).ToList();
            }

            DisplayPayments(allPayments);
        }

        private static void DisplayPayments(List<Payment> payments)
        {
            foreach (var pmt in payments)
            {
                Console.WriteLine(pmt.ToString());
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private static IEnumerable<Payment> PaymentLogic(IList<Debt> debts)
        {
            List<Payment> currentMonthPayments = new List<Payment>();
            decimal extraIncome = _totalIncome - (debts.Sum(x => x.GetCurrentMinimumPayment().Amount));
            for (int i = 0; i < debts.Count(); i++)
            {
                decimal amountToPay = debts[i].GetCurrentMinimumPayment().Amount;
                if (amountToPay == 0.00m) continue;

                if (extraIncome > 0.00m && amountToPay > 0.00m)
                {
                    amountToPay = amountToPay + extraIncome;
                }

                var payment = debts[i].MakeSinglePayment(amountToPay);

                if (payment.Amount < (amountToPay))
                {
                    extraIncome = (amountToPay - payment.Amount);
                }
                else
                {
                    extraIncome = 0.00m;
                }

                // don't add 0 payment
                if (payment.Amount == 0.00m) continue;

                currentMonthPayments.Add(payment);
            }

            return currentMonthPayments;
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
