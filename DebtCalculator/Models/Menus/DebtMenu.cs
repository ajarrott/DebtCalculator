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
            bool goBack = false;

            ConsoleKeyInfo key;

            do
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
                Console.WriteLine("3: Modify Debt");
                Console.WriteLine("B: Go Back");

                key = Console.ReadKey();

                if (key.Key == ConsoleKey.B)
                    goBack = true;

                if (key.Key == ConsoleKey.D1)
                {
                    DisplayAddDebt();
                }

                if (key.Key == ConsoleKey.D2)
                {
                    DisplayRemoveDebt();
                }

                if(key.Key == ConsoleKey.D3)
                {
                    DisplayModifyDebt();
                }
            } while (!goBack);
        }

        private static void DisplayModifyDebt()
        {
            bool goBack = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Remove Debt");
                Console.WriteLine("-------------------------");
                Console.WriteLine("Total Debt: {0:C}", DebtCollection.TotalDebt);
                Console.WriteLine("Total Required Income: {0:C}", DebtCollection.TotalRequiredIncome);
                Console.WriteLine("----- Current Debts -----");
                DebtCollection.ListDebts();
                Console.WriteLine("-------------------------");
                Console.Write("Debt to remove (B to go back)");
                var key = Console.ReadKey();

                if (key.KeyChar == 'b' || key.KeyChar == 'B')
                    goBack = true;

                if(int.TryParse(key.KeyChar.ToString(), out int keyNum))
                {
                    keyNum = keyNum - 1;
                    if(DebtCollection.GetDebts.Count() > keyNum && keyNum >= 0)
                    {
                        ModifyDebt(DebtCollection.GetDebts[keyNum]);
                    }
                }

            } while (!goBack);
        }

        private static void ModifyDebt(Debt debt)
        {
            bool goBack = false;

            do
            {
                Console.Clear();
                Console.WriteLine("Modify Debt: " + debt.ToString());
                Console.WriteLine("-------------------------");
                Console.WriteLine("1: Modify Name ({0})", debt.LoanName ?? "");
                Console.WriteLine("2: Modify Balance ({0:C})", debt.CurrentBalance);
                Console.WriteLine("3: Modify Apr ({0:P2})", debt.Apr);
                Console.Write("Selection (B to go back): ");
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.B)
                    goBack = true;

                if(key.Key == ConsoleKey.D1)
                {
                    Console.Write("New Name ({0}, blank for no change): ", debt.LoanName);
                    var input = Console.ReadLine();
                    Console.WriteLine();

                    if (string.IsNullOrWhiteSpace(input)) continue;

                    debt.LoanName = input;
                }

                if(key.Key == ConsoleKey.D2)
                {
                    bool validInput = false;
                    decimal newBalance = 0.00m;
                    string input = "";
                    do
                    {
                        Console.Write("New Balance ({0}, blank for no change): ", debt.CurrentBalance);
                        input = Console.ReadLine();

                        // leave loop if blank
                        if (string.IsNullOrWhiteSpace(input)) break;
                        validInput = decimal.TryParse(input, out newBalance);

                        if(validInput)
                            debt.CurrentBalance = newBalance;
                    } while (!validInput);
                }

                if(key.Key == ConsoleKey.D3)
                {
                    bool validInput = false;
                    decimal newApr = 0.00m;
                    string input = "";
                    do
                    {
                        Console.Write("New Apr ({0:P2}, [e.g. 22.99] blank for no change): ", debt.Apr);
                        input = Console.ReadLine();

                        // leave loop if blank
                        if (string.IsNullOrWhiteSpace(input)) break;
                        validInput = decimal.TryParse(input, out newApr);

                        if (validInput)
                            debt.Apr = newApr / 100.00m;
                    } while (!validInput);
                }
            } while (!goBack);

        }

        private static void DisplayRemoveDebt()
        {
            bool goBack = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Remove Debt");
                Console.WriteLine("-------------------------");
                Console.WriteLine("Total Debt: {0:C}", DebtCollection.TotalDebt);
                Console.WriteLine("Total Required Income: {0:C}", DebtCollection.TotalRequiredIncome);
                Console.WriteLine("----- Current Debts -----");
                DebtCollection.ListDebts();
                Console.WriteLine("-------------------------");
                Console.Write("Debt to remove (B to go back)");
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.KeyChar == 'b' || key.KeyChar == 'B')
                    goBack = true;

                // remove key from list (convert to 0 based indexing)
                if(int.TryParse(key.KeyChar.ToString(), out int keyNum))
                {
                    keyNum = keyNum - 1;
                    if (DebtCollection.GetDebts.Count() > keyNum && keyNum >= 0)
                    {
                        DebtCollection.RemoveDebt(DebtCollection.GetDebts[keyNum]);
                    }
                }

            } while (!goBack);
        }

        private static void DisplayAddDebt()
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
