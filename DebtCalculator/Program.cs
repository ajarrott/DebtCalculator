using DebtCalculator.Models;
using DebtCalculator.Models.Menus;
using System;
using System.Collections.Generic;

namespace DebtCalculator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            List<Debt> loans = new List<Debt>();

            Menu.DisplayMainMenu();
        }
    }
}
