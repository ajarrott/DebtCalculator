using DebtCalculator.Models;
using DebtCalculator.Models.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Interfaces
{
    internal static class Extensions
    {
        public static string HorizontalPaymentInfo(this List<Payment> payments)
        {
            if (payments == null || payments.Count == 0) return null;

            StringBuilder pmts = new StringBuilder();
            StringBuilder dates = new StringBuilder();
            int numMonthsToPay = 0;

            var uniqueLoanNames = payments.GroupBy(x => x.LoanName)
                .OrderByDescending(g => g.Count())
                .SelectMany(g => g).Select(x => x.LoanName).Distinct();

            foreach (var name in uniqueLoanNames)
            {
                var currentPayments = payments.Where(x => x.LoanName == name).OrderBy(x => x.CurrentMonth);
                if (numMonthsToPay < currentPayments.Count()) numMonthsToPay = currentPayments.Count();
                if (currentPayments == null || currentPayments.Count() == 0) continue;
                pmts.Append(currentPayments.First().LoanName + "|");
                foreach (var pmt in currentPayments)
                {
                    dates.Append(string.Format("{0}|", pmt.CurrentMonth.ToString("MM-yyyy")));
                    pmts.Append(string.Format("{0:C}|", pmt.Amount));
                }
                pmts.AppendLine();
            }

            pmts.AppendLine(string.Format("Number of months to pay {0:C}: {1}", DebtCollection.TotalIncome, numMonthsToPay));
            pmts.AppendLine(string.Format("Total Paid: {0:C}", payments.Sum(x => x.Amount)));
            pmts.AppendLine(string.Format("Total Interest Paid: {0:C}", payments.Sum(x => (x.Amount - x.AmountPaidToPrincipal))));

            return dates.ToString() + Environment.NewLine + pmts.ToString();
        }
    }
}
