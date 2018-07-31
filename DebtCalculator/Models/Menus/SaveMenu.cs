using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DebtCalculator.Models.Menus
{
    internal static partial class Menu
    {
        private static readonly string _delim = "|*|";

        public static void DisplaySave()
        {
            string fileName = "";

            do
            {
                Console.Clear();
                Console.WriteLine("-------------------------");
                Console.WriteLine("Save current settings");
                Console.WriteLine("Total Debt: {0:C}", _totalCurrentDebt);
                Console.WriteLine("Total Income: {0:C}", _totalIncome);
                DebtCollection.ListDebts();
                Console.WriteLine("-------------------------");
                Console.Write("FileName (default \'debt.xml\'): ");
                fileName = Console.ReadLine();

                fileName = string.IsNullOrWhiteSpace(fileName) ? "debt.xml" : fileName;
                fileName = Directory.GetCurrentDirectory() + "\\" + fileName;

                try
                {
                    File.Create(fileName);
                }
                catch (IOException e)
                {
                    Console.WriteLine("Error trying to create file " + fileName + ": ");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                
            } while (!File.Exists(fileName));

            using(var fs = new FileStream(fileName, FileMode.Truncate))
            {
                using (var sw = new StreamWriter(fs))
                {
                    sw.WriteLine("totalDebt" + _delim + _totalCurrentDebt);
                    sw.WriteLine("totalIncome" + _delim + _totalIncome);

                    foreach(var debt in DebtCollection.GetDebts)
                    {
                        sw.WriteLine(debt.SaveString());
                    }
                }
            }
        }
    }
}
