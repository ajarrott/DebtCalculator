using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Models
{
    internal static class DebtCollection
    {
        static List<Debt> _currentDebts = new List<Debt>();

        public static void AddDebt(Debt debt)
        {
            _currentDebts.Add(debt);
        }

        public static void RemoveDebt(Debt debt)
        {
            if (_currentDebts.IndexOf(debt) >= 0)
            {
                _currentDebts.Remove(debt);
            }
        }

        public static decimal TotalDebt
        {
            get
            {
                return (_currentDebts.Sum(x => x.CurrentBalance));
            }
        }

        public static decimal TotalRequiredIncome
        {
            get
            {
                return (_currentDebts.Sum(x => x.GetMinimumPayment()));
            }
        }

        public static void ListDebts()
        {
            foreach (var debt in _currentDebts)
            {
                Console.WriteLine(_currentDebts.IndexOf(debt) + 1 + ": " + debt.ToString());
            }
        }
        
        public static List<Debt> GetDebts
        {
            get 
            {
                return _currentDebts;
            }
        }

        internal static void ClearDebts()
        {
            _currentDebts.Clear();
        }
    }
}
