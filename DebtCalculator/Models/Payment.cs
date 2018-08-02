using DebtCalculator.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Models
{
    internal class Payment
    {
        public Payment() { }
        public Payment(string loanName, DateTime currentMonth, decimal apr, decimal startingBalance, decimal amount = 0.00m)
        {
            LoanName = loanName;
            CurrentMonth = currentMonth;
            Apr = apr;
            StartingBalance = startingBalance;
            Amount = amount == 0.00m ? MinPayment : amount;

            if (Amount < MinPayment)
            {
                throw new PaymentException(string.Format("Amount given too small, must pay at least {0:C} tried to pay {1:C}", MinPayment, Amount));
            }

            if(Amount > BalanceWithInterest)
            {
                Amount = BalanceWithInterest;
            }
        }

        public string LoanName { get; set; }
        public decimal Apr { get; set; }
        public decimal StartingBalance { get; set; }
        public DateTime CurrentMonth { get; set; }

        public int DaysInMonth
        {
            get
            {
                return DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);
            }
        }

        public decimal DayDecimal
        {
            get
            {
                return DaysInMonth / 365.0m;
            }
        }

        public decimal InterestRatio
        {
            get
            {
                return DayDecimal * Apr;
            }
        }

        public decimal Interest
        {
            get
            {
                return (StartingBalance * InterestRatio);
            }
        }

        private decimal _amount;

        public decimal Amount
        {
            get
            {
                return _amount > StartingBalance + Interest ?
                    StartingBalance + Interest
                    : _amount;
            }
            set
            {
                _amount = value;
            }
        }

        public decimal AmountPaidToPrincipal
        {
            get
            {
                return Amount - Interest;
            }
        }

        private decimal MinPayment
        {
            get
            {
                return (StartingBalance * 0.01m) + Interest;
            }
        }

        public decimal BalanceWithInterest
        {
            get
            {
                return (StartingBalance + Interest);
            }
        }

        public decimal NewBalance
        {
            get
            {
                return BalanceWithInterest - Amount;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}: {2:C} (New Balance {3:C})", CurrentMonth.ToString("MMMM yyyy"), LoanName, Amount, NewBalance);
        }
    }
}
