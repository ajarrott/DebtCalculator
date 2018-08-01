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
        public Payment(int daysInMonth, decimal apr, decimal currentBalance, decimal amount = 0.00m)
        {
            DaysInMonth = daysInMonth;
            Apr = apr;
            CurrentBalance = currentBalance;
            Amount = amount == 0.00m ? MinPayment : amount;

            if (Amount < MinPayment)
            {
                throw new PaymentException(string.Format("Amount given too small, must pay at least {0:C} tried to pay {1:C}", MinPayment, Amount));
            }
        }

        public int DaysInMonth { get; set; }
        public decimal Apr { get; set; }
        public decimal CurrentBalance { get; set; }

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
                return (CurrentBalance * InterestRatio);
            }
        }

        private decimal _amount;

        public decimal Amount
        {
            get
            {
                return _amount > CurrentBalance + Interest ?
                    CurrentBalance + Interest
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
                return (CurrentBalance * 0.01m) + Interest;
            }
        }
    }
}
