using System;
using System.Collections.Generic;
using ReadifyBank.Interfaces;

namespace Readify
{
    public class Program
    {

        static void Main(string[] args)
        {
            ReadifyBankImpl bank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;
            IAccount johnHL = bank.OpenHomeLoanAccount("John Doe", now);
            IAccount johnSV = bank.OpenSavingsAccount("John Doe", now);
            IAccount janeSV = bank.OpenSavingsAccount("Jane Doe", now);

            bank.PerformDeposit(johnSV, 5000, "Despoit", DateTimeOffset.Now);
            bank.PerformDeposit(janeSV, 400, "Despoit", DateTimeOffset.Now);
            bank.PerformDeposit(janeSV, 1300, "Despoit", DateTimeOffset.Now);
            bank.PerformDeposit(johnSV, 300, "Despoit", DateTimeOffset.Now);

            bank.PerformWithdrawal(johnSV, 200, "Withdrawal", DateTimeOffset.Now);
            bank.PerformWithdrawal(janeSV, 50, "Withdrawal", DateTimeOffset.Now);
            // Fails to withdraw
            bank.PerformWithdrawal(janeSV, -200, "Withdrawal", DateTimeOffset.Now);

            bank.PerformTransfer(johnSV, johnHL, 500, "Despoit", DateTimeOffset.Now);
            bank.PerformTransfer(janeSV, johnHL, 500, "Despoit", DateTimeOffset.Now);

            IEnumerable<IStatementRow> johnFullStatement = bank.CloseAccount(johnSV, DateTimeOffset.Now);
            IEnumerator<IStatementRow> johnFullStatementList = johnFullStatement.GetEnumerator();

            Console.WriteLine("Name, Amount, Balance, Description, Date");
            while (johnFullStatementList.MoveNext())
            {
                Console.WriteLine("{0}, {1}, {2}, {3}, {4},", 
                    johnFullStatementList.Current.Account.CustomerName,
                    johnFullStatementList.Current.Amount,
                    johnFullStatementList.Current.Balance,
                    johnFullStatementList.Current.Description,
                    johnFullStatementList.Current.Date
                    );
            }

            Console.WriteLine("John's savings account interest over {0} months: ${1}", 24, bank.CalculateInterestToDate(johnSV, now.AddMonths(24)));

            Console.ReadKey(true);
        }

    }
}