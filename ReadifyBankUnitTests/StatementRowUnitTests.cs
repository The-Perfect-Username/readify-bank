using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Readify;

namespace ReadifyBankUnitTests
{
    [TestClass]
    public class StatementRowUnitTests
    {
        [TestMethod]
        public void CreatedNewStatement()
        {
            Account account = new Account("SV-000001", DateTimeOffset.Now) {
                CustomerName = "John Doe"
            };

            DateTimeOffset transferDate = DateTimeOffset.Now;

            StatementRow statement = new StatementRow(
                account,
                0,
                0,
                "Transfer",
                transferDate);

            Assert.AreEqual(account, statement.Account);
            Assert.AreEqual(0, statement.Amount);
            Assert.AreEqual(0, statement.Balance);
            Assert.AreEqual("Transfer", statement.Description);
            Assert.AreEqual(transferDate, statement.Date);
        }
    }
}
