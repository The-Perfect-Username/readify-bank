using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Readify;
using ReadifyBank.Interfaces;
namespace ReadifyBankUnitTests
{
    [TestClass]
    public class ReadifyBankUnitTests
    {
        [TestMethod]
        public void CreateNewReadifyBankClassTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();

            Assert.IsInstanceOfType(readifyBank, typeof(ReadifyBankImpl));
        }

        [TestMethod]
        public void OpenANewHomeLoanAccountTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount account = readifyBank.OpenHomeLoanAccount("John Doe", now);

            Assert.AreEqual(account.AccountNumber, "LN-000001");
            Assert.AreEqual(account.CustomerName, "John Doe");
            Assert.AreEqual(account.Balance, 0);
            Assert.AreEqual(account.OpenedDate, now);

            Assert.AreEqual(1, readifyBank.AccountList.Count);
        }

        [TestMethod]
        public void OpenMultipleHomeLoanAccountsTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();

            IAccount accountOne = readifyBank.OpenHomeLoanAccount("John Doe", DateTimeOffset.Now);
            IAccount accountTwo = readifyBank.OpenHomeLoanAccount("Jane Doe", DateTimeOffset.Now);
            IAccount accountThree = readifyBank.OpenHomeLoanAccount("Mary Sue", DateTimeOffset.Now);

            Assert.AreEqual("LN-000001", accountOne.AccountNumber);
            Assert.AreEqual("LN-000002", accountTwo.AccountNumber);
            Assert.AreEqual("LN-000003", accountThree.AccountNumber);

            Assert.AreEqual("John Doe", accountOne.CustomerName);
            Assert.AreEqual("Jane Doe", accountTwo.CustomerName);
            Assert.AreEqual("Mary Sue", accountThree.CustomerName);

            Assert.AreEqual(3, readifyBank.AccountList.Count);

        }

        [TestMethod]
        public void OpenANewSavingsAccountTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", now);

            Assert.AreEqual("SV-000001", account.AccountNumber);
            Assert.AreEqual("John Doe", account.CustomerName);
            Assert.AreEqual(0, account.Balance);
            Assert.AreEqual(now, account.OpenedDate);
            Assert.AreEqual(1, readifyBank.AccountList.Count);
        }

        [TestMethod]
        public void OpenMultipleSavingsAccountsTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();

            IAccount accountOne = readifyBank.OpenSavingsAccount("John Doe", DateTimeOffset.Now);
            IAccount accountTwo = readifyBank.OpenSavingsAccount("Jane Doe", DateTimeOffset.Now);
            IAccount accountThree = readifyBank.OpenSavingsAccount("Mary Sue", DateTimeOffset.Now);

            Assert.AreEqual("SV-000001", accountOne.AccountNumber);
            Assert.AreEqual("SV-000002", accountTwo.AccountNumber);
            Assert.AreEqual("SV-000003", accountThree.AccountNumber);

            Assert.AreEqual("John Doe", accountOne.CustomerName);
            Assert.AreEqual("Jane Doe", accountTwo.CustomerName);
            Assert.AreEqual("Mary Sue", accountThree.CustomerName);
            Assert.AreEqual(3, readifyBank.AccountList.Count);

        }

        [TestMethod]
        public void GetBalanceTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", now);

            Assert.AreEqual(readifyBank.GetBalance(account), 0);

            account.Balance = 500;

            Assert.AreEqual(500, readifyBank.GetBalance(account));
        }


        [TestMethod]
        public void PerformDepositTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", now);

            decimal amount = 200;
            readifyBank.PerformDeposit(account, amount, "Deposit", now);
            
            Assert.AreEqual(200, account.Balance);
            Assert.AreEqual(1, readifyBank.TransactionLog.Count);

        }

        [TestMethod]
        public void PerformMultipleDepositsTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", now);

            readifyBank.PerformDeposit(account, 200, "Deposit", now);
       
            Assert.AreEqual(account.Balance, 200);

            readifyBank.PerformDeposit(account, 200, "Deposit", now);

            Assert.AreEqual(400, account.Balance);

            Assert.AreEqual(2, readifyBank.TransactionLog.Count);

        }

        [TestMethod]
        public void PerformDepositFailsBecauseNegativeAmountTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", now);

            readifyBank.PerformDeposit(account, -200, "Deposit", now);
       
            Assert.AreEqual(0, account.Balance);

        }

        [TestMethod]
        public void PerformWithdrawalTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", now);

            decimal amount = 200;
            account.Balance = 300;

            readifyBank.PerformWithdrawal(account, amount, "Withdrawal", now);

            Assert.AreEqual(100, account.Balance);

        }

        [TestMethod]
        public void PerformMultipleWithdrawalsTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", now);

            account.Balance = 300;

            readifyBank.PerformWithdrawal(account, 100, "Withdrawal", now);

            Assert.AreEqual(account.Balance, 200);

            readifyBank.PerformWithdrawal(account, 100, "Withdrawal", now);

            Assert.AreEqual(100, account.Balance);

        }

        [TestMethod]
        public void PerformWithdrawalFailsBecauseOfInsufficientFundsTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", now);

            account.Balance = 100;

            readifyBank.PerformWithdrawal(account, 400, "Withdrawal", now);

            Assert.AreEqual(100, account.Balance);

        }

        [TestMethod]
        public void PerformWithdrawalFailsBecauseNegativeAmountTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", now);

            account.Balance = 100;

            readifyBank.PerformWithdrawal(account, -400, "Withdrawal", now);

            Assert.AreEqual(100, account.Balance);

        }

        [TestMethod]
        public void PerformTransferTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount johnAccount = readifyBank.OpenSavingsAccount("John Doe", now);
            IAccount janeAccount = readifyBank.OpenSavingsAccount("Jane Doe", now);

            johnAccount.Balance = 500;

            readifyBank.PerformTransfer(johnAccount, janeAccount, 50, "Transfer", now);

            Assert.AreEqual(450, johnAccount.Balance);
            Assert.AreEqual(50, janeAccount.Balance);

        }

        [TestMethod]
        public void PerformTransferFailsBecauseOfInsufficientFundsTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount johnAccount = readifyBank.OpenSavingsAccount("John Doe", now);
            IAccount janeAccount = readifyBank.OpenSavingsAccount("Jane Doe", now);

            johnAccount.Balance = 500;

            readifyBank.PerformTransfer(johnAccount, janeAccount, 600, "Transfer", now);

            Assert.AreEqual(500, johnAccount.Balance);
            Assert.AreEqual(0, janeAccount.Balance);

        }

        [TestMethod]
        public void PerformTransferFailsBecauseNegativeAmountTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount johnAccount = readifyBank.OpenSavingsAccount("John Doe", now);
            IAccount janeAccount = readifyBank.OpenSavingsAccount("Jane Doe", now);

            johnAccount.Balance = 500;

            readifyBank.PerformTransfer(johnAccount, janeAccount, -100, "Transfer", now);

            Assert.AreEqual(500, johnAccount.Balance);
            Assert.AreEqual(0, janeAccount.Balance);

        }

        [TestMethod]
        public void GetMiniStatementTestTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount johnAccount = readifyBank.OpenSavingsAccount("John Doe", new DateTimeOffset(2018, 6, 15, 12, 50, 30, new TimeSpan(1, 0, 0)));
            IAccount janeAccount = readifyBank.OpenSavingsAccount("Jane Doe", new DateTimeOffset(2018, 6, 15, 12, 50, 30, new TimeSpan(1, 0, 0)));

            readifyBank.PerformDeposit(johnAccount, 500, "Deposit", new DateTimeOffset(2018, 6, 16, 12, 50, 30, new TimeSpan(1, 0, 0)));
            readifyBank.PerformDeposit(johnAccount, 300, "Deposit", new DateTimeOffset(2018, 6, 17, 12, 50, 30, new TimeSpan(1, 0, 0)));
            readifyBank.PerformDeposit(johnAccount, 200, "Deposit", new DateTimeOffset(2018, 6, 18, 12, 50, 30, new TimeSpan(1, 0, 0)));
            readifyBank.PerformDeposit(johnAccount, 30, "Deposit", new DateTimeOffset(2018, 7, 2, 12, 50, 30, new TimeSpan(1, 0, 0)));
            readifyBank.PerformDeposit(johnAccount, 520, "Deposit", new DateTimeOffset(2018, 7, 12, 12, 50, 30, new TimeSpan(1, 0, 0)));
            readifyBank.PerformDeposit(johnAccount, 684, "Deposit", new DateTimeOffset(2018, 7, 28, 12, 50, 30, new TimeSpan(1, 0, 0)));
            
            readifyBank.PerformTransfer(johnAccount, janeAccount, 700, "Transfer", now);

            IEnumerable<IStatementRow> johnMiniStatement = readifyBank.GetMiniStatement(johnAccount);
            IEnumerable<IStatementRow> janeMiniStatement = readifyBank.GetMiniStatement(janeAccount);

            Assert.IsInstanceOfType(johnMiniStatement, typeof(IEnumerable<IStatementRow>));
            Assert.IsInstanceOfType(janeMiniStatement, typeof(IEnumerable<IStatementRow>));

            IEnumerator<IStatementRow> johnE = johnMiniStatement.GetEnumerator();
            IEnumerator<IStatementRow> janeE = janeMiniStatement.GetEnumerator();

            int counter = 0;

            if (johnE.MoveNext())
            {
                Assert.AreEqual(johnAccount, johnE.Current.Account);
                Assert.AreEqual(700, johnE.Current.Amount);
                Assert.AreEqual(1534, johnE.Current.Balance);
                Assert.AreEqual(now, johnE.Current.Date);
                counter++;
            }

            while (johnE.MoveNext())
            {
                Assert.AreEqual(johnAccount, johnE.Current.Account);
                counter++;
            }

            Assert.AreEqual(5, counter);

            counter = 0;

            if (janeE.MoveNext())
            {
                Assert.AreEqual(janeAccount, janeE.Current.Account);
                Assert.AreEqual(700, janeE.Current.Amount);
                Assert.AreEqual(700, janeE.Current.Balance);
                Assert.AreEqual(now, janeE.Current.Date);
                counter++;
            }

            while (janeE.MoveNext())
            {
                Assert.AreEqual(janeAccount, janeE.Current.Account);
                counter++;
            }

            Assert.AreEqual(1, counter);

        }

        [TestMethod]
        public void CloseAccountTest()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTimeOffset.Now;

            IAccount johnAccount = readifyBank.OpenSavingsAccount("John Doe", new DateTimeOffset(2018, 6, 16, 12, 50, 30, new TimeSpan(1, 0, 0)));

            readifyBank.PerformDeposit(johnAccount,  200, "Deposit", new DateTimeOffset(2018, 6, 17, 12, 50, 30, new TimeSpan(1, 0, 0)));
            readifyBank.PerformDeposit(johnAccount, 200, "Deposit", new DateTimeOffset(2018, 6, 18, 12, 50, 30, new TimeSpan(1, 0, 0)));
            readifyBank.PerformWithdrawal(johnAccount, 200, "Withdrawal", new DateTimeOffset(2018, 6, 19, 12, 50, 30, new TimeSpan(1, 0, 0)));

            IEnumerable<IStatementRow> fullStatements = readifyBank.CloseAccount(johnAccount, now);
            IEnumerator<IStatementRow> e = fullStatements.GetEnumerator();

            int balance = 200;

            while (e.MoveNext())
            {
                Assert.AreEqual(200, e.Current.Account.Balance);
                balance++;
            }

            Assert.AreEqual(4, readifyBank.TransactionLog.Count);
            Assert.AreEqual(0, readifyBank.AccountList.Count);
        }

        [TestMethod]
        public void GetSavingsAccountInterestReturnsZeroBecauseNoTransactions()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset startDate = new DateTimeOffset(2018, 6, 16, 12, 50, 30, new TimeSpan(1, 0, 0));

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", startDate);
            decimal interest = readifyBank.CalculateInterestToDate(account, startDate);

            Assert.AreEqual(0, interest);
        }

        [TestMethod]
        public void GetSavingsAccountInterestAfterOneTransaction()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTime.Now;

            IAccount account = readifyBank.OpenSavingsAccount("John Doe", now);

            readifyBank.PerformDeposit(account, 500, "Deposit", now);

            decimal interest = readifyBank.CalculateInterestToDate(account, now.AddMonths(5));

            Assert.AreEqual(650, interest);
        }

        [TestMethod]
        public void GetSavingsHomeInterestAfterOneTransaction()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTime.Now;

            IAccount account = readifyBank.OpenHomeLoanAccount("John Doe", now);

            readifyBank.PerformDeposit(account, 500, "Deposit", now);

            decimal interest = readifyBank.CalculateInterestToDate(account, now.AddYears(2));

            Assert.AreEqual((decimal)539.9, interest);
        }

        [TestMethod]
        public void GetSavingsHomeInterestAfter18Months()
        {
            ReadifyBankImpl readifyBank = new ReadifyBankImpl();
            DateTimeOffset now = DateTime.Now;

            IAccount account = readifyBank.OpenHomeLoanAccount("John Doe", now);

            readifyBank.PerformDeposit(account, 500, "Deposit", now);

            decimal interest = readifyBank.CalculateInterestToDate(account, now.AddMonths(18));

            Assert.AreEqual((decimal)519.95, interest);
        }
    }
}
