using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Console.WriteLine("Total Debt: {0:C}", DebtCollection.TotalDebt);
                Console.WriteLine("Total Required Income: {0:C}", DebtCollection.TotalRequiredIncome);
                Console.WriteLine("Total Income: {0:C}", _totalIncome);
                DebtCollection.ListDebts();
                Console.WriteLine("-------------------------");
                Console.Write("Continue(Y/N): ");
                key = Console.ReadKey();
                Console.WriteLine();
                validInput = key.Key == ConsoleKey.Y || key.Key == ConsoleKey.N;
            } while (!validInput);
    

            if(key.Key == ConsoleKey.N)
            {
                return;
            }

            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            if(fd.ShowDialog() == DialogResult.OK)
            {
                using (var fs = new FileStream(fd.FileName, FileMode.Open))
                {
                    // clear any current debts
                    DebtCollection.ClearDebts();
                    using (var sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            var items = line.Split(new string[] { _delim }, StringSplitOptions.RemoveEmptyEntries);
                            
                            if (items.Contains("totalIncome"))
                            {
                                _totalIncome = decimal.Parse(items[1]);
                                Console.WriteLine("Total Income Loaded ({0:C})", _totalIncome);
                            }
                            
                            if (items.Contains("debtInfo"))
                            {
                                Debt d = new Debt();
                                d.LoadString(line);

                                if (string.IsNullOrEmpty(d.LoanName)) continue;

                                DebtCollection.AddDebt(d);
                                Console.WriteLine("Loaded Debt ({0}, {1:C}, {2:P2})", d.LoanName, d.CurrentBalance, d.Apr);
                            }
                        }
                    }
                }

                Console.Write("Loaded " + Path.GetFileName(fd.FileName) + " successfully.  Press any key to continue...");
                Console.ReadKey();
                Console.WriteLine();
            }
        }
    }
}

