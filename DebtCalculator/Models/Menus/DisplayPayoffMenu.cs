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
            List<decimal> amountsToPay = new List<decimal>();
            var debts = DebtCollection.GetDebts;
            Console.Clear();
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
                Console.WriteLine(debts[i].CalculatePayoff(amountsToPay[i]).ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
