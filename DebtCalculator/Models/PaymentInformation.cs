using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Models
{
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
}
