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
                        //Calculate Payoff amounts
                        Menu.CalculatePayoffs();
                        break;
                    case ConsoleKey.D4:
                        //Save
                        Menu.DisplaySave();
                        break;
                    case ConsoleKey.D5:
                        Menu.DisplayLoad();
                        //Load
                        break;
                    case ConsoleKey.Q:
                        return;
                }
            }
        }
    }
}
