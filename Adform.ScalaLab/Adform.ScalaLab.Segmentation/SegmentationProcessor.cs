using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adform.ScalaLab.Segmentation.Core;
using Adform.ScalaLab.Segmentation.Infrastructure;

namespace Adform.ScalaLab.Segmentation
{
    public class SegmentationProcessor
    {
        public void Process(string pathToRanges, string pathToTransactions, string processedFilePath="results.tsv")
        {
            // Build tree
            var searchTree = BuildSegmentsSearchTree(pathToRanges);

            // Create empty result file
            if (File.Exists(processedFilePath))
            {
                File.Delete(processedFilePath);
            }

            using (var writer = File.CreateText(processedFilePath))
            {
                // Open transactions file
                var transactionsLoader = new FileTransactionsLoader(pathToTransactions);

                transactionsLoader.Open();
                try
                {
                    // Process transactions step-by-step
                    while (true)
                    {
                        var transactions = transactionsLoader.GetNextTransactions();

                        if (transactions != null && transactions.Count > 0)
                        {
                            transactions.ForEach(t =>
                            {
                                var names = searchTree.FindSegmentsNamesById(t.Ip);

                                if (names != null && names.Count > 0)
                                {
                                    names.ForEach(n =>
                                    {
                                        writer.WriteLine("{0}\t{1}", t.UserId, n);
                                    });
                                }
                                else
                                {
                                    writer.WriteLine("{0}\t<Unknown domain>", t.UserId);
                                }
                            });
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    transactionsLoader.Close();
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        private IBinarySearchSegmentTree BuildSegmentsSearchTree(string pathToRanges)
        {
            var loader = new FileSegmentsLoader(pathToRanges);
            var tree = new BinarySegmentTree();

            loader.Open();
            try
            {
                tree.BuildWithLoader(loader);
                loader.Close();
            }
            finally
            {
                loader.Close();
            }

            return tree;
        }
    }
}