using DebtCalculator.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DebtCalculator.Models.Menus
{
    internal static partial class Menu
    {
        public static void CalculatePayoffs()
        {
            ConsoleKeyInfo key;

            do
            {
                Console.Clear();
                Console.WriteLine("---------------------");
                Console.WriteLine("Payoff Calculator");
                Console.WriteLine("---------------------");
                Console.WriteLine("1: Manual Entry (Estimate Payoff)");
                Console.WriteLine("2: Generate Payment Plan (Highest Interest per Month First)");
                Console.WriteLine("3: Generate Payment Plan (Snowball, Lowest Balance First)");
                Console.WriteLine("4: Highest Interest first vs Snowball interest difference");
                Console.WriteLine("5: Modify Income allocated to debt (Currently: {0:C}, need {1:C})", DebtCollection.TotalIncome, DebtCollection.TotalRequiredIncome);
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
                    if (!IncomeHighEnough()) continue;

                    var pmts = CalculateOptimalPayoff();
                    DisplayPayments(pmts);
                }

                if (key.Key == ConsoleKey.D3)
                {
                    if (!IncomeHighEnough()) continue;

                    var pmts  = CalculateSnowballPayoff();
                    DisplayPayments(pmts);
                }

                if(key.Key == ConsoleKey.D4)
                {
                    if (!IncomeHighEnough()) continue;

                    var opt = CalculateOptimalPayoff();
                    var snow = CalculateSnowballPayoff();

                    var optAmt = opt.Sum(x => x.Amount);
                    var snowAmt = snow.Sum(x => x.Amount);

                    var optNumMonths = opt.GroupBy(x => x.CurrentMonth).Count();
                    var snowNumMonths = snow.GroupBy(x => x.CurrentMonth).Count();

                    Console.WriteLine("Monthly allocation: {0:C}", DebtCollection.TotalIncome);
                    Console.WriteLine("--------- Highest Interest First ---------");
                    Console.WriteLine("Total paid: {0:C}", optAmt);
                    Console.WriteLine("Last Payment: {0:C}", opt.Last().Amount);
                    Console.WriteLine("Months to payoff: {0}", optNumMonths);
                    Console.WriteLine("---------------- Snowball ----------------");
                    Console.WriteLine("Total paid: {0:C}", snowAmt);
                    Console.WriteLine("Last Payment: {0:C}", snow.Last().Amount);
                    Console.WriteLine("Months to payoff: {0}", snowNumMonths);
                    Console.WriteLine("------------------------------------------");

                    if (snowAmt > optAmt )
                    {
                        Console.WriteLine("Snowball payments will cost {0:C} more than optimal payoff.", snowAmt - optAmt);
                    }
                    else if(snowAmt == optAmt)
                    {
                        Console.WriteLine("Snowball payments and Optimal payments are the same.");
                    }
                    else
                    {
                        Console.WriteLine("Somehow snowball payments will save you {0:C}", optAmt - snowAmt);
                    }

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }

                if (key.Key == ConsoleKey.D5)
                {
                    DisplayModifyIncome();
                }

            } while (true);
        }

        private static bool IncomeHighEnough()
        {
            if (DebtCollection.TotalIncome >= DebtCollection.TotalRequiredIncome) return true;
            
            Console.WriteLine("Must have at least {0:C} (current income: {1:C})", DebtCollection.TotalRequiredIncome, DebtCollection.TotalIncome);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            return false;
        }

        private static List<Payment> CalculateSnowballPayoff()
        {            
            decimal extraIncome = DebtCollection.TotalIncome - DebtCollection.TotalRequiredIncome;

            // get copy of list
            var lowestFirst = DebtCollection.GetDebts.OrderBy(x => x.GetCurrentMinimumPayment().StartingBalance).Select(x => new Debt(x.Name, x.Apr, x.Balance)).ToList();
            List<Payment> allPayments = new List<Payment>();

            while(lowestFirst.Any(x => x.Balance > 0.00m))
            {
                var pmt = PaymentLogic(lowestFirst);
                allPayments.AddRange(pmt);

                lowestFirst = lowestFirst.OrderBy(x => x.GetCurrentMinimumPayment().StartingBalance).ToList();
            }

            return allPayments;
        }

        private static List<Payment> CalculateOptimalPayoff()
        {
            var mostInterest = DebtCollection.GetDebts.OrderByDescending(x => x.GetCurrentMinimumPayment().Interest).Select(x => new Debt(x.Name, x.Apr, x.Balance)).ToList();
            List<Payment> allPayments = new List<Payment>();

            // highest will be first in list
            while (mostInterest.Any(x => x.Balance > 0.00m))
            {
                var pmt = PaymentLogic(mostInterest);
                allPayments.AddRange(pmt);

                mostInterest = mostInterest.OrderByDescending(x => x.GetCurrentMinimumPayment().Interest).ToList();
            }

            return allPayments;
        }

        private static void DisplayPayments(List<Payment> payments)
        {
            foreach (var pmt in payments)
            {
                Console.WriteLine(pmt.ToString());
            }

            Console.WriteLine(payments.HorizontalPaymentInfo());

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private static IEnumerable<Payment> PaymentLogic(IList<Debt> debts)
        {
            List<Payment> currentMonthPayments = new List<Payment>();
            decimal extraIncome = DebtCollection.TotalIncome - (debts.Sum(x => x.GetCurrentMinimumPayment().Amount));
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
            string input = "";

            for (int i = 0; i < debts.Count; i++)
            {
                decimal amount = 0.00m;
                do
                {
                    Console.WriteLine(debts[i].ToString());
                    Console.Write("Amount to Pay Monthly (must be above min payment, B to go back): ");
                    input = Console.ReadLine();

                    if (input == "B" || input == "b") return;
                } while (decimal.TryParse(input, out amount) && amount < debts[i].GetCurrentMinimumPayment().Amount);

                amountsToPay.Add(amount);
            }

            for (int i = 0; i < debts.Count; i++)
            {
                Console.WriteLine(debts[i].CalculateEstimatedPayoff(amountsToPay[i]).ToString());
            }
        }
    }
}
