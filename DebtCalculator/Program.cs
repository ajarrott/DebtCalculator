using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator
{
    class Program
    {
        static decimal _totalCurrentDebt;
        static decimal _totalIncome;
        static readonly ConsoleKeyInfo dummyKey = new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false);

        static void DebtOptions()
        {
            bool validKey = false,
                goBack = false;
            ConsoleKeyInfo key = dummyKey;

            while (!validKey && !goBack)
            {
                Console.Clear();
                Console.WriteLine("Debt Options");
                Console.WriteLine("-------------------------");
                Console.WriteLine("Total Debt: {0:C}", _totalCurrentDebt);
                Console.WriteLine("----- Current Debts -----");
                DebtCollection.ListDebts();
                Console.WriteLine("-------------------------");
                Console.WriteLine("1: Add Debt");
                Console.WriteLine("2: Remove Debt");
                Console.WriteLine("B: Go Back");

                key = Console.ReadKey();

                validKey = key.Key == ConsoleKey.D1
                    || key.Key == ConsoleKey.D2
                    || key.Key == ConsoleKey.B;

                if (key.Key == ConsoleKey.B)
                    goBack = true;

                if(key.Key == ConsoleKey.D1)
                {
                    AddDebt();
                }

                if(key.Key == ConsoleKey.D2)
                {
                    RemoveDebt();
                }
            }
        }

        private static void RemoveDebt()
        {
            throw new NotImplementedException();
        }

        private static void AddDebt()
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

            DebtCollection.AddDebt(new Debt(name, apr, balance));
            _totalCurrentDebt = DebtCollection.TotalDebt;
        }

        static void Main(string[] args)
        {
            List<Debt> loans = new List<Debt>();
            DebtCollection.WriteLine = Console.WriteLine;
            while(true)
            {
                ConsoleKey key = Menu();
                switch (key)
                {
                    case ConsoleKey.D1:
                        // Debt Options
                        DebtOptions();
                        break;
                    case ConsoleKey.D2:
                        //Modify Income
                        break;
                    case ConsoleKey.D3:
                        //Save
                        break;
                    case ConsoleKey.D4:
                        break;
                    case ConsoleKey.Q:
                        return;
                }
            }

            loans.Add(new Debt("CapitalOne", .2599m, 10100.00m));

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
            
            Console.ReadLine();
        }

        static ConsoleKey Menu()
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

    public static class DebtCollection
    {
        static List<Debt> _currentDebts = new List<Debt>();
        public static Action<string> WriteLine { get; set; }

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
                return (_currentDebts.Select(x => x.CurrentBalance).Sum());
            }
        }

        public static void ListDebts()
        {
            foreach(var debt in _currentDebts)
            {
                WriteLine(_currentDebts.IndexOf(debt) + 1 + ": " + debt.ToString());
            }
        }
    }

    public class PaymentInformation
    {
        public PaymentInformation() { }
        public PaymentInformation(string loanName, decimal interestPaid, decimal apr, decimal totalAmountPaid, decimal estimatedPayment, decimal lastPayment, int numberOfPayments)
        {
            LoanName = loanName;
            InterestPaid = interestPaid;
            TotalAmountPaid = totalAmountPaid;
            EstimatedPayment = estimatedPayment;
            LastPayment = lastPayment;
            NumberOfPayments = numberOfPayments;
            Apr = apr;
        }

        public string LoanName { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public decimal EstimatedPayment { get; set; }
        public decimal LastPayment { get; set; }
        public decimal Apr { get; set; }
        public int NumberOfPayments { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:\n\r\tEstimated Monthly Payment:\t{1:C}\n\r\tNumber of Payments:\t\t{2}\n\r\tLast Payment:\t\t\t{3:C}\n\r\tTotal Amount Paid:\t\t{4:C}\n\r\tInterest Paid:\t\t\t{5:C}\n\r\tAPR:\t\t\t\t{6:P2}\n\r\n\r",
                LoanName,
                EstimatedPayment,
                NumberOfPayments,
                LastPayment,
                TotalAmountPaid,
                InterestPaid,
                Apr);
        }
    }

    public class Debt
    {
        public Debt() { }
        public Debt(string loanName, decimal interestRate, decimal currentBalance)
        {
            LoanName = loanName;
            Apr = interestRate;
            CurrentBalance = currentBalance;
        }

        public string LoanName { get; set; }
        public decimal Apr { get; set; }
        public decimal CurrentBalance { get; set; } // for now we will assume the current balance is the average daily balance
                                                    // to overestimate we should use the highest balance

        public PaymentInformation CalculatePayoff(decimal estimatedPayment)
        {
            decimal interestPaid = 0.00m;
            decimal totalAmountPaid = 0.00m;
            decimal currentBalance = CurrentBalance;
            decimal lastPayment = 0.00m;
            int numPayments = 0;
            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month + numPayments, 1);

            while (currentBalance > 0.00m)
            {
                var days = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
                var dayDecimal = days / 365.0m;
                var interestRatio = dayDecimal * Apr;
                decimal interest = currentBalance * interestRatio;
                decimal amountPaidToPrincipal = 0.00m;
                decimal minPayment = ((currentBalance * 0.01m) + interest);


                if (minPayment > estimatedPayment) throw new Exception(string.Format("Payment too small, minimum payment must be more than {0:C}, currently {1:C}", minPayment, estimatedPayment));

                if(currentBalance - (estimatedPayment - interest) > 0)
                {
                    amountPaidToPrincipal = (estimatedPayment - interest);
                }
                else
                {
                    // last payment case, don't overpay the bill
                    amountPaidToPrincipal = lastPayment = currentBalance;
                    lastPayment = currentBalance;
                    amountPaidToPrincipal = currentBalance;
                }

                currentBalance -= amountPaidToPrincipal;   // remove payment from current balance
                totalAmountPaid += amountPaidToPrincipal + interest;  
                interestPaid += interest;
                numPayments++;
                currentMonth.AddMonths(1);
            }

            return new PaymentInformation(LoanName, interestPaid, Apr, totalAmountPaid, estimatedPayment, lastPayment, numPayments);
        }

        public override string ToString()
        {
            return string.Format("Name({0})\tBal({1:C})\tInterest({2:P2})", LoanName, CurrentBalance, Apr);
        }
    }
}
