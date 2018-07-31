using DebtCalculator.Models;
using DebtCalculator.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            List<Debt> loans = new List<Debt>();

            while (true)
            {
                ConsoleKey key = Menu.DisplayMainMenu();

                switch (key)
                {
                    case ConsoleKey.D1:
                        // Debt Options
                        Menu.DisplayDebtOptions();
                        break;
                    case ConsoleKey.D2:
                        //Modify Income
                        Menu.DisplayModifyIncome();
                        break;
                    case ConsoleKey.D3:
                        Menu.CalculatePayoffs();
                        break;
                    case ConsoleKey.D4:
                        //Save
                        break;
                    case ConsoleKey.D5:
                        break;
                    case ConsoleKey.Q:
                        return;
                }
            }


            // Test Code
            // TODO: Move to unit tests
            /*loans.Add(new Debt("CapitalOne", .2599m, 10100.00m));

            foreach(var loan in loans)
            {
                try
                {
                    Console.WriteLine(loans[0].CalculatePayoff(410).ToString());
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            
            Console.ReadLine();*/
        }


    }
}
