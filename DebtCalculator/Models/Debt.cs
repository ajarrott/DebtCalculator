using DebtCalculator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Models
{
    internal class Debt : SaveLoad
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

                if (currentBalance - (estimatedPayment - interest) > 0)
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

        public override void LoadString(string s)
        {
            var items = s.Split(new string[] { _delim }, StringSplitOptions.RemoveEmptyEntries);
            LoanName = items[1];
            CurrentBalance = decimal.Parse(items[2]);
            Apr = decimal.Parse(items[3]);
        }

        public override string SaveString()
        {
            // indicies
            //     0                     1                   2                         3
            return "debtInfo" + _delim + LoanName + _delim + CurrentBalance + _delim + Apr;
        }

        public override string ToString()
        {
            return string.Format("Name({0})\tBal({1:C})\tInterest({2:P2})", LoanName, CurrentBalance, Apr);
        }
    }
}
