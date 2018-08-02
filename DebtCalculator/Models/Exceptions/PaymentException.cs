using System;

namespace DebtCalculator.Models.Exceptions
{
    internal class PaymentException : Exception
    {
        public PaymentException() : base() { }
        public PaymentException(string msg) : base(msg) { }
        public PaymentException(string msg, Exception innerExp) : base(msg, innerExp) { }
    }
}
