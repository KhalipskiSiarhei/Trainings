using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adform.ScalaLab.Segmentation.Core
{
    public interface ITransactionsLoader
    {
        void Open();

        List<Transaction> GetNextTransactions(int count = 1);

        void Close();
    }
}