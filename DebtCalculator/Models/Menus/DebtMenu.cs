using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Models.Menus
{
    internal static partial class Menu
    {
        internal static void DisplayDebtOptions()
        {
            bool validKey = false,
                goBack = false;

            ConsoleKeyInfo key = dummyKey;

            while (!validKey && !goBack)
            {
                Console.Clear();
                Console.WriteLine("Debt Options");
                Console.WriteLine("-------------------------");
                Console.WriteLine("Total Debt: {0:C}", DebtCollection.TotalDebt);
                Console.WriteLine("Total Required Income: {0:C}", DebtCollection.TotalRequiredIncome);
                Console.WriteLine("----- Current Debts -----");
                DebtCollection.ListDebts();
                Console.WriteLine("-------------------------");
                Console.WriteLine("1: Add Debt");
                Console.WriteLine("2: Remove Debt");
                Console.WriteLine("B: Go Back");

                key = Console.ReadKey();

                validKey = key.Key == ConsoleKey.D1
                    || key.Key == ConsoleKey.D2
                    || key.Key == ConsoleKey.B;

                if (key.Key == ConsoleKey.B)
                    goBack = true;

                if (key.Key == ConsoleKey.D1)
                {
                    Menu.DisplayAddDebt();
                }

                if (key.Key == ConsoleKey.D2)
                {
                    Menu.DisplayRemoveDebt();
                }
            }
        }

        internal static void DisplayRemoveDebt()
        {
            throw new NotImplementedException();
        }

        internal static void DisplayAddDebt()
        {
            decimal balance = 0.00m;
            decimal apr = 0.00m;
            Console.Clear();
            Console.WriteLine("Adding Debt");
            Console.WriteLine("-----------");
            Console.Write("Debt Name: ");
            string name = Console.ReadLine();

            do
            {
                Console.Write("Balance (e.g. 5000): ");
            } while (!decimal.TryParse(Console.ReadLine(), out balance));

            do
            {
                Console.Write("APR (e.g. 22.99): ");
            } while (!decimal.TryParse(Console.ReadLine(), out apr));

            apr = apr / 100.0m;

            var newDebt = new Debt(name, apr, balance);
            DebtCollection.AddDebt(newDebt);
        }
    }
}
