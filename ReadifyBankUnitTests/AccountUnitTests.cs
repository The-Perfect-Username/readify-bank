using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Readify;

namespace ReadifyBankUnitTests
{
    [TestClass]
    public class AccountUnitTests
    {
        [TestMethod]
        public void CreateNewAccountTest()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            string accountNumber = "SV-000001";

            Account account = new Account(accountNumber, now) {
                CustomerName = "John Doe"
            };

            Assert.AreEqual(account.AccountNumber, accountNumber);
            Assert.AreEqual(account.CustomerName, "John Doe");
            Assert.AreEqual(account.OpenedDate, now);
            Assert.AreEqual(account.Balance, 0);
        }

        [TestMethod]
        public void UpdateAccountPropertiesTest()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            string accountNumber = "SV-000001";

            Account account = new Account(accountNumber, now) {
                CustomerName = "John Doe"
            };

            account.CustomerName = "Jane Doe";
            account.Balance = 300;

            Assert.AreEqual(account.AccountNumber, accountNumber);
            Assert.AreEqual(account.CustomerName, "Jane Doe");
            Assert.AreEqual(account.OpenedDate, now);
            Assert.AreEqual(account.Balance, 300);
        }

    }
}
