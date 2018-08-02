using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("Current Excess Income for Debts: {0}", _totalIncome);
                Console.WriteLine("-----------------------------------");
                Console.Write("Amount to allocate (e.g. 1000, blank to cancel)): ");
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return;
            } while (!decimal.TryParse(input, out tempIncome));

            _totalIncome = tempIncome;
        }
    }
}
