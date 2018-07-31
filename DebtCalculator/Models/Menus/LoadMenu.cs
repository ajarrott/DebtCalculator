using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebtCalculator.Models.Menus
{
    internal static partial class Menu
    {
        public static void DisplayLoad()
        {
            bool validInput = false;

            ConsoleKeyInfo key = dummyKey;

            do
            {
                Console.Clear();
                Console.WriteLine("-------------------------");
                Console.WriteLine("Load settings (this will remove all unsaved information)");
                Console.WriteLine("Total Debt: {0:C}", _totalCurrentDebt);
                Console.WriteLine("Total Income: {0:C}", _totalIncome);
                DebtCollection.ListDebts();
                Console.WriteLine("-------------------------");
                Console.Write("Continue(Y/N): ");
                key = Console.ReadKey();
                validInput = key.Key == ConsoleKey.Y || key.Key == ConsoleKey.N;
            } while (!validInput);
    

            if(key.Key == ConsoleKey.N)
            {
                return;
            }

            OpenFileDialog fd = new OpenFileDialog();

            if(fd.ShowDialog() == DialogResult.OK)
            {
                using (var fs = new FileStream(fd.FileName, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            var items = line.Split(new string[] { _delim }, StringSplitOptions.RemoveEmptyEntries);

                            if (items.Contains("totalDebt"))
                            {
                                _totalCurrentDebt = decimal.Parse(items[1]);
                            }

                            if (items.Contains("totalIncome"))
                            {
                                _totalIncome = decimal.Parse(items[1]);
                            }

                            if (items.Contains("debt"))
                            {
                                Debt d = new Debt();
                                d.LoadString(line);

                                if (string.IsNullOrEmpty(d.LoanName)) continue;

                                DebtCollection.AddDebt(d);
                            }
                        }
                    }
                }
            }
        }
    }
}

