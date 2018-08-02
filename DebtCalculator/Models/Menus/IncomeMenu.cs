using System;

namespace DebtCalculator.Models.Menus
{
    internal static partial class Menu
    {
        internal static void DisplayModifyIncome()
        {
            decimal tempIncome = 0.00m;
            string input = "";
            do
            {
                Console.Clear();
                Console.WriteLine("Modifying Income");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Current Amount Allocated for Debts: {0:C}", DebtCollection.TotalIncome);
                Console.WriteLine("-----------------------------------");
                Console.Write("Amount to allocate (e.g. 1000, blank to cancel)): ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return;
            } while (!decimal.TryParse(input, out tempIncome));

            DebtCollection.TotalIncome = tempIncome;
        }
    }
}
