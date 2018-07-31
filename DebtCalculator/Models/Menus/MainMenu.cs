using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Models.Menus
{
    internal static partial class Menu
    {
        static decimal _totalCurrentDebt = 0.00m;
        static decimal _totalIncome = 0.00m;

        static readonly ConsoleKeyInfo dummyKey = new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false);

        public static ConsoleKey DisplayMainMenu()
        {
            ConsoleKeyInfo keyInfo = dummyKey;
            bool validKey = false;

            while (!validKey)
            {
                Console.Clear();
                Console.WriteLine("Debt Calculator");
                Console.WriteLine("---------------");
                Console.WriteLine("1: Debt Options (Current Total: {0:C})", _totalCurrentDebt);
                Console.WriteLine("2: Modify Income allocated to debt (Currently: {0:C})", _totalIncome);
                Console.WriteLine("3: Save to Xml");
                Console.WriteLine("4: Load from Xml");
                Console.WriteLine("Q: Exit");
                Console.WriteLine("---------------");
                Console.Write("Input: ");

                keyInfo = Console.ReadKey();

                validKey = (keyInfo.KeyChar >= '1'
                   && keyInfo.KeyChar <= '4')
                   || keyInfo.KeyChar == 'q'
                   || keyInfo.KeyChar == 'Q';
            }

            return keyInfo.Key;
        }
    }
}
