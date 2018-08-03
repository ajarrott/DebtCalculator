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
        public Debt(string name, decimal interestRate, decimal currentBalance) : this()
        {
            Name = name;
            Apr = interestRate;
            Balance = currentBalance;
            CurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        /* IBill Interface */
        public string Name { get; set; }
        public decimal Balance { get; set; } 
        public int DueDay { get; set; }
        public DateTime CurrentMonth { get; set; }
        public List<Payment> Payments { get; set; }

        public decimal Apr { get; set; }

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
            decimal currentBalance = Balance;
            int numPayments = 0;
            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month + numPayments, 1);

            while (currentBalance > 0.00m)
            {
                Payment pmt = new Payment(Name, currentMonth, Apr, currentBalance, estimatedPayment);

                // last payment
                payments.Add(pmt);
                currentBalance -= pmt.AmountPaidToPrincipal;   // remove payment from current balance
                currentMonth.AddMonths(1);

                if (currentBalance <= 0.00m)
                {
                    break;
                }
            }

            return new PaymentInformation(Name, Apr, estimatedPayment, payments);
        }

        public Payment GetCurrentMinimumPayment()
        {
            return new Payment(Name, CurrentMonth, Apr, Balance);
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
            Payment pmt = new Payment(Name, CurrentMonth, Apr, Balance, amount);
            Balance = pmt.NewBalance;
            CurrentMonth = CurrentMonth.AddMonths(1);
            Payments.Add(pmt);
            return pmt;
        }

        public void LoadString(string s)
        {
            var items = s.Split(new string[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);
            Name = items[1];
            Balance = decimal.Parse(items[2]);
            Apr = decimal.Parse(items[3]);
        }

        public string SaveString()
        {
            // indicies
            //     0                     1                   2                         3
            return "debtInfo" + Delimiter + Name + Delimiter + Balance + Delimiter + Apr;
        }

        public override string ToString()
        {
            return string.Format("Name({0})\tBal({1:C})\tInterest({2:P2})\tMin Payment({3:C})", Name, Balance, Apr, GetCurrentMinimumPayment().Amount);
        }
    }
}
