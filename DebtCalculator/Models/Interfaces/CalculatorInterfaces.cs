using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtCalculator.Models.Interfaces
{
    /// <summary>
    /// Save and Load function defintions
    /// </summary>
    interface ISaveLoad
    {
        string Delimiter { get; }
        string SaveString();
        void LoadString(string s);
    }

    interface IAccount
    {
        DateTime OpenDate { get; }
        IList<IMember> Members { get; }
        IList<ISubAccount> SubAccounts { get; }

        void OpenSubAccount(ISubAccount shln);
        void AddMember(IMember member);
    }

    interface IMember
    {
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string SSN { get; }
        DateTime DateOfBirth { get; }
        DateTime ExpirationDate { get; }

        void ExpireMember();
    }

    interface ISubAccount
    {
        string Id { get; }
        string Name { get; }
        DateTime OpenDate { get; }
        DateTime CloseDate { get; }
        decimal Balance { get; }

        IList<IMember> AllowedMembers { get; }
        IList<ITransaction> Transactions { get; }

        void AddTransaction(ITransaction transaction);
        void AddMemberToAllowed(IMember member);
        void CloseAccount();
    }

    /// <summary>
    /// Tracks amount and date of transaction
    /// Cannot be changed after creation
    /// Can be voided, voided transactions should not be counted towards any balances
    /// </summary>
    interface ITransaction
    {
        ISourceInformation Source { get; }
        ISubAccount Destination { get; }
        
        decimal Amount { get; }
        DateTime Date { get; }
        bool Void { get; }
        DateTime VoidDate { get; }

        void VoidTransaction();
    }

    ///// <summary>
    ///// Creates and income on each due day associated
    ///// </summary>
    //interface IIncome
    //{
    //    List<ITransaction> Income { get; set; }
    //    IEnumerable<IDue> PayDays { get; set; }
    //}

    interface ISourceInformation
    {
        string AccountNumber { get; }
        string RoutingNumber { get; }
        string InstitutionName { get; }
    }

    interface IDue
    {
        DateTime CurrentDay { get; }
        int DueDay { get; set; }

        void AddDay();

        event EventHandler<AmountEventArgs> OnTransactionDue;
    }

    interface IBill
    {
        string Name { get; set; }
        decimal Balance { get; set; }
        decimal PastDueAmount { get; set; }
        bool PastDue { get; set; }

        IDue BillDue { get; set; }
        ITransaction MinPayment { get; set; }
        IList<ITransaction> Payments { get; set; }
        void MakeSinglePayment(decimal amount);
    }

    interface IInterest
    {
        decimal Apr { get; set; }
    }

    interface IPayoffCalculator<T>
        where T : IInterest
    {
        IList<ITransaction> Payments { get; set; }
        IList<ITransaction> CalculatePayoff(IList<T> debts);
        void DisplayAllPayments();
        void ExportDelimitedPayments(string delim);
    }

    public class AmountEventArgs : EventArgs
    {
        public AmountEventArgs(decimal amount)
        {
            Amount = amount;
        }

        decimal Amount { get; set; }
    }
}
