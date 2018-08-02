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

            Menu.DisplayMainMenu();
        }
    }
}
