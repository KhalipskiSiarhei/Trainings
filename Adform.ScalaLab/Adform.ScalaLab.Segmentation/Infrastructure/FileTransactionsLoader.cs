using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adform.ScalaLab.Segmentation.Core;

namespace Adform.ScalaLab.Segmentation.Infrastructure
{
    public class FileTransactionsLoader : FileLoaderBase, ITransactionsLoader
    {
        public FileTransactionsLoader(string pathToFile)
            : base(pathToFile)
        {
        }

        public List<Transaction> GetNextTransactions(int count = 1)
        {
            var transactions = new List<Transaction>();

            while (transactions.Count < count)
            {
                var segmentStr = _reader.ReadLine();
                if (!String.IsNullOrEmpty(segmentStr))
                {
                    transactions.Add(Convert(segmentStr));
                }
                else
                {
                    if (_reader.EndOfStream)
                    {
                        break;
                    }
                }
            }
            return transactions;
        }

        private Transaction Convert(string transactionStr)
        {
            var transactionParts = transactionStr.Split(new[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            var userId = transactionParts[0];
            var ip = GetLongIpByStr(transactionParts[1]);

            return new Transaction() { UserId = userId, Ip = ip };
        }
    }
}