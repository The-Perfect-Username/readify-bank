using System;
using System.Collections.Generic;
using System.Linq;
using ReadifyBank.Interfaces;


namespace Readify
{
    public class ReadifyBankImpl: IReadifyBank
    {
        // Used for account numbers
        uint numberOfSavingsAccounts = 0;
        uint numberOfHomeLoanAccounts = 0;

        /// <summary>
        /// Bank accounts list
        /// </summary>
        public IList<IAccount> AccountList
        {
            get; set;
        }
        /// <summary>
        /// Transactions log of the bank
        /// </summary>
        public IList<IStatementRow> TransactionLog
        {
            get; set;
        }
        /// <summary>
        /// Readify Bank Class Constructor which initiates 
        /// the AccountList and TransactionLog lists
        /// </summary>
        public ReadifyBankImpl()
        {
            AccountList = new List<IAccount>();
            TransactionLog = new List<IStatementRow>();
        }

        /// <summary>
        /// Open a home loan account
        /// </summary>
        /// <param name="customerName">Customer name</param>
        /// <param name="openDate">The date of the transaction</param>
        /// <returns>Opened Account</returns>
        public IAccount OpenHomeLoanAccount(string customerName, DateTimeOffset openDate)
        {
            // Iterate number of homeloan accounts by 1 to generate account number for new account
            numberOfHomeLoanAccounts += 1;
            // Create new account number
            string accountNumber = string.Format("{0:LN-000000}", numberOfHomeLoanAccounts);
            
            Account homeLoanAccount = new Account(accountNumber, openDate)
            {
                CustomerName = customerName,
            };

            // Add account to account list
            AccountList.Add(homeLoanAccount);

            return homeLoanAccount;
        }

        /// <summary>
        /// Open a saving account
        /// </summary>
        /// <param name="customerName">Customer name</param>
        /// <param name="openDate">The date of the transaction</param>
        /// <returns>Opened account</returns>
        public IAccount OpenSavingsAccount(string customerName, DateTimeOffset openDate)
        {
            // Iterate number of savings accounts by 1 to generate account number for new account
            numberOfSavingsAccounts += 1;
            // Generate new account number
            string accountNumber = string.Format("{0:SV-000000}", numberOfSavingsAccounts);
            
            Account savingsAccount = new Account(accountNumber, openDate)
            {
                CustomerName = customerName
            };
            // Add account to account list
            AccountList.Add(savingsAccount);

            return savingsAccount;
        }
        
        /// <summary>
        /// Deposit amount in an account
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="amount">Deposit amount</param>
        /// <param name="description">Description of the transaction</param>
        /// <param name="depositDate">The date of the transaction</param>
        public void PerformDeposit(IAccount account, decimal amount, string description, DateTimeOffset depositDate)
        {
            try
            {
                lock (account)
                {

                    // Throw exception if the amount is a negative number
                    if (amount < 0)
                    {
                        throw new ArgumentException("Cannot despoit a negative amount");
                    }

                    account.Balance += amount;

                    StatementRow depositStatement = new StatementRow(
                        account,
                        amount,
                        account.Balance,
                        description,
                        depositDate);
                    // Add transaction statement to TransactionLog
                    TransactionLog.Add(depositStatement);
                }
            }
            catch (ArgumentException E)
            {
                Console.WriteLine(E.Message);
            }

        }
        /// <summary>
        /// Withdraw amount in an account
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="amount">Withdrawal amount</param>
        /// <param name="description">Description of the transaction</param>
        /// <param name="withdrawalDate">The date of the transaction</param>
        public void PerformWithdrawal(IAccount account, decimal amount, string description, DateTimeOffset withdrawalDate)
        {
            try
            {
                lock (account)
                {
                    // Throw exception if the amount is a negative number 
                    if (amount < 0)
                    {
                        throw new Exception("Cannot Withdraw a negative amount");
                    }
                    // Throw esception if the amount is greater than the current balance
                    if (account.Balance < amount)
                    {
                        throw new Exception("Transaction failed: Insufficient funds");
                    }

                    account.Balance -= amount;

                    StatementRow withdrawalStatement = new StatementRow(
                            account,
                            amount,
                            account.Balance,
                            description,
                            withdrawalDate);
                    // Add transaction statement to TransactionLog
                    TransactionLog.Add(withdrawalStatement);
                }

            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
            }
        }
        /// <summary>
        /// Transfer amount from an account to an account
        /// </summary>
        /// <param name="from">From account</param>
        /// <param name="to">To account</param>
        /// <param name="amount">Transfer amount</param>
        /// <param name="description">Description of the transaction</param>
        /// <param name="transferDate">The date of the transaction</param>
        public void PerformTransfer(IAccount from, IAccount to, decimal amount, string description, DateTimeOffset transferDate)
        {
            try
            {
                // Throw exception if the amount is a negative number
                if (amount < 0)
                {
                    throw new ArgumentException("Cannot Withdraw a negative amount");
                }
                // Throw exception if the amount is greater than the current balance of the account
                // performing the transfer
                if (amount > from.Balance)
                {
                    throw new ArgumentException("Transaction failed: Insufficient funds");
                }

                // Update account balances
                from.Balance -= amount;
                to.Balance += amount;

                StatementRow fromStatement = new StatementRow(
                    from,
                    amount,
                    from.Balance,
                    description,
                    transferDate);

                StatementRow toStatement = new StatementRow(
                    to,
                    amount,
                    to.Balance,
                    description,
                    transferDate);

                // Add statements for both sending and recieving accounts
                TransactionLog.Add(fromStatement);
                TransactionLog.Add(toStatement);

            }
            catch (ArgumentException E)
            {
                Console.WriteLine(E.Message);
            }
        }

        /// <summary>
        /// Return the balance for an account
        /// </summary>
        /// <param name="account">Customer account</param>
        /// <returns></returns>
        public decimal GetBalance(IAccount account)
        {
            return account.Balance;
        }

        /// <summary>
        /// Calculate interest rate for an account to a specific time
        /// The interest rate for Saving account is 6% monthly
        /// The interest rate for Home loan account is 3.99% annually
        /// </summary>
        /// <param name="account">Customer account</param>
        /// <param name="toDate">Calculate interest to this date</param>
        /// <returns>The added value</returns>
        public decimal CalculateInterestToDate(IAccount account, DateTimeOffset toDate)
        {
            DateTimeOffset now = DateTimeOffset.Now;

            decimal monthlyRate = (decimal)0.06;
            decimal yearlyRate = (decimal)0.0399;
            decimal numberOfMonths = ((toDate.Year - now.Year) * 12) + toDate.Month - now.Month;
            decimal interest = 0;

            try
            {
                // Throw new exception if the toDate value is before the current date
                if (numberOfMonths < 0)
                {
                    throw new ArgumentException("Specified date must be after the current date");
                }

                // Return 0 if the current balance is 0
                if (account.Balance == 0)
                {
                    return interest;
                }

                if (account.AccountNumber.Contains("LN"))
                {
                    decimal numberOfYears = Math.Floor(numberOfMonths / 12);
                    // Simple Interest Formula
                    interest = account.Balance * (1 + yearlyRate * numberOfYears);
                }
                else
                {
                    interest = account.Balance * (1 + monthlyRate * numberOfMonths);
                }
                return interest;
            } catch(ArgumentException E)
            {
                Console.WriteLine(E.Message);
                return 0;
            }
        }

        /// <summary>
        /// Get mini statement (the last 5 transactions occurred on an account)
        /// </summary>
        /// <param name="account">Customer account</param>
        /// <returns>Last five transactions</returns>
        public IEnumerable<IStatementRow> GetMiniStatement(IAccount account)
        {

            // Retrieve all of the accounts statements from the transactions log 
            IEnumerable<IStatementRow> statements = from transaction in TransactionLog
                                                    where transaction.Account.Equals(account)
                                                    orderby transaction.Date descending
                                                    select transaction;

            // Return the last 5 statements
            return statements.Take(5);

        }

        /// <summary>
        /// Close an account
        /// </summary>
        /// <param name="account">Customer account</param>
        /// <param name="closeDate">Close Date</param>
        /// <returns>All transactions happened on the closed account</returns>
        public IEnumerable<IStatementRow> CloseAccount(IAccount account, DateTimeOffset closeDate)
        {
            lock (account)
            {
                StatementRow closingStatement = new StatementRow(
                    account,
                    0,
                    0,
                    "Closing Account",
                    closeDate);

                // Add final transaction statement
                TransactionLog.Add(closingStatement);
                // Remove account from the account list
                AccountList.Remove(account);

                // Retrieve all statements for the closing account
                IEnumerable<IStatementRow> fullStats = from transaction in TransactionLog
                                                       where transaction.Account.Equals(account)
                                                       select transaction;
                return fullStats;
            }

        }

    }
}
