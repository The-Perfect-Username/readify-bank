using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadifyBank.Interfaces;

namespace Readify
{
    /// <summary>
    /// Readify Bank IAccount Interface
    /// There are two types of account : Home Loan and Saving 
    /// </summary>
    public class Account : IAccount
    {
        /// <summary>
        /// Customer Name
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Account number 
        /// It is formatted as follows: 2 characters for Account type, dash and 6 digits for account number starting from 1
        /// For home loan account it should start from "LN-000001"
        /// For saving account it should start from "SV-000001"
        /// </summary>
        public string AccountNumber { get; }
        /// <summary>
        /// Current account balance
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// The date when the account was opened
        /// </summary>
        public DateTimeOffset OpenedDate { get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public Account(string accountNumber, DateTimeOffset openedDate)
        {
            AccountNumber = accountNumber;
            OpenedDate = openedDate;
            Balance = 0;
        }
    }
}
