using DebtCalculator.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace DebtCalculator.Models
{
    internal class Debt : ISaveLoad
    {
        public string Delimiter { get; private set; } = "|*|";
        public Debt()
        {
            Payments = new List<Payment>();
        }
        public Debt(string loanName, decimal interestRate, decimal currentBalance) : this()
        {
            LoanName = loanName;
            Apr = interestRate;
            CurrentBalance = currentBalance;
            CurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public string LoanName { get; set; }
        public decimal Apr { get; set; }
        public decimal CurrentBalance { get; set; } // for now we will assume the current balance is the average daily balance
                                                    // to overestimate we should use the highest balance

        public DateTime CurrentMonth { get; set; }
        public List<Payment> Payments { get; set; }

        /// <summary>
        /// This will not modify the current class
        /// Gives a high level overview if you want to estimate a payoff
        /// at a fixed amount over time.
        /// </summary>
        /// <param name="estimatedPayment"></param>
        /// <returns></returns>
        public PaymentInformation CalculateEstimatedPayoff(decimal estimatedPayment)
        {
            List<Payment> payments = new List<Payment>();
            decimal currentBalance = CurrentBalance;
            int numPayments = 0;
            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month + numPayments, 1);

            while (currentBalance > 0.00m)
            {
                Payment pmt = new Payment(LoanName, currentMonth, Apr, currentBalance, estimatedPayment);

                // last payment
                payments.Add(pmt);
                currentBalance -= pmt.AmountPaidToPrincipal;   // remove payment from current balance
                currentMonth.AddMonths(1);

                if (currentBalance <= 0.00m)
                {
                    break;
                }
            }

            return new PaymentInformation(LoanName, Apr, estimatedPayment, payments);
        }

        private int DaysInMonth(DateTime? currentMonth = null)
        {
            return DateTime.DaysInMonth(currentMonth?.Year ?? CurrentMonth.Year, currentMonth?.Month ?? CurrentMonth.Month);
        }

        public Payment GetCurrentMinimumPayment()
        {
            return new Payment(LoanName, CurrentMonth, Apr, CurrentBalance);
        }

        /// <summary>
        /// This WILL modify the current class's CurrentMonth and CurrentBalance
        /// Make payment to loan returns payment information
        /// Adds payment to Payments within this class
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Payment MakeSinglePayment(decimal amount)
        {
            Payment pmt = new Payment(LoanName, CurrentMonth, Apr, CurrentBalance, amount);
            CurrentBalance = pmt.NewBalance;
            CurrentMonth = CurrentMonth.AddMonths(1);
            Payments.Add(pmt);
            return pmt;
        }

        //public decimal GetMinimumPayment()
        //{
        //    Payment pmt = new Payment()
        //    DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    var days = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
        //    var dayDecimal = days / 365.0m;
        //    var interestRatio = dayDecimal * Apr;
        //    decimal interest = CurrentBalance * interestRatio;
        //    return ((CurrentBalance * 0.01m) + interest);
        //}

        public void LoadString(string s)
        {
            var items = s.Split(new string[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);
            LoanName = items[1];
            CurrentBalance = decimal.Parse(items[2]);
            Apr = decimal.Parse(items[3]);
        }

        public string SaveString()
        {
            // indicies
            //     0                     1                   2                         3
            return "debtInfo" + Delimiter + LoanName + Delimiter + CurrentBalance + Delimiter + Apr;
        }

        public override string ToString()
        {
            return string.Format("Name({0})\tBal({1:C})\tInterest({2:P2})\tMin Payment({3:C})", LoanName, CurrentBalance, Apr, GetCurrentMinimumPayment().Amount);
        }
    }
}
