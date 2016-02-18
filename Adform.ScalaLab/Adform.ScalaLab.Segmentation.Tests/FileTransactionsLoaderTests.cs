using System;
using Adform.ScalaLab.Segmentation.Core;
using Adform.ScalaLab.Segmentation.Infrastructure;
using NUnit.Framework;

namespace Adform.ScalaLab.Segmentation.Tests
{
    [TestFixture]
    public class FileTransactionsLoaderTests
    {
        private FileTransactionsLoader _fileTransactionsLoader;

        [SetUp]
        public void SetUp()
        {
            _fileTransactionsLoader = new FileTransactionsLoader("transactions.tsv");
            _fileTransactionsLoader.Open();
        }

        [TearDown]
        public void TearDown()
        {
            _fileTransactionsLoader.Close();
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void GetNextSegments_LoadByN_NItemsReturned(int count)
        {
            var transactions = _fileTransactionsLoader.GetNextTransactions(count);

            Assert.IsTrue(transactions != null);
            Assert.IsTrue(transactions.Count == count);
            transactions.ForEach(AssertTransaction);
        }

        [Test]
        public void GetNextSegments_LoadAll_AllItemsLoaded()
        {
            var transactions = _fileTransactionsLoader.GetNextTransactions(Int32.MaxValue);

            Assert.IsTrue(transactions.Count <= Int32.MaxValue);
            transactions.ForEach(AssertTransaction);
        }

        private void AssertTransaction(Transaction transaction)
        {
            Assert.IsFalse(string.IsNullOrEmpty(transaction.UserId));
            Assert.IsTrue(transaction.Ip > 0);
        }
    }
}