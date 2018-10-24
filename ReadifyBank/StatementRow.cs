using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadifyBank.Interfaces;

namespace Readify
{
    /// <summary>
    /// Statement Row represents a single row in the bank statement
    /// It represents a single transaction on account
    /// Each statement is immutable and cannot be updated
    /// </summary>
    public class StatementRow : IStatementRow
    {
        /// <summary>
        /// Account on which the transaction is made
        /// </summary>
        public IAccount Account { get; }
        
        /// <summary>
        /// Date and time of the transaction
        /// </summary>
        public DateTimeOffset Date { get; }
        /// <summary>
        /// Amount of the operation
        /// </summary>
        public decimal Amount { get; }
        
        /// <summary>
        /// Balance of the account after the transaction
        /// </summary>
        public decimal Balance { get; }
        
        /// <summary>
        /// Description of the transaction
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StatementRow(IAccount account, decimal amount, decimal balance, string description, DateTimeOffset date)
        {
            Account = account;
            Amount = amount;
            Balance = balance;
            Description = description;
            Date = date;
        }
    }

}
