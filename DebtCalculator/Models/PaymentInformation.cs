using DebtCalculator.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Models
{
    internal class PaymentInformation
    {
        public PaymentInformation() { }

        public PaymentInformation(string loanName, decimal apr, decimal estimatedPayment, List<Payment> payments)
        {
            LoanName = loanName;
            EstimatedPayment = estimatedPayment;
            Apr = apr;
            TotalAmountPaid = payments.Sum(x => x.Amount);
            InterestPaid = payments.Sum(x => x.Interest);
            LastPayment = payments.Last().Amount;
            NumberOfPayments = payments.Count();
        }

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
            return string.Format("{0} ({1:P2}):\n\r\tEstimated Monthly Payment:\t{2:C} x {3}\n\r" +
                "\tLast Payment:\t\t\t{4:C}\n\r" +
                "\tTotal Amount Paid:\t\t{5:C}\n\r" +
                "\tInterest Paid:\t\t\t{6:C}",
                LoanName,
                Apr,
                EstimatedPayment,
                NumberOfPayments,
                LastPayment,
                TotalAmountPaid,
                InterestPaid);
        }
    }
}
