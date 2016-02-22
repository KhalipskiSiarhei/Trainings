using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
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
            var stopwatch = new Stopwatch();

            // Build tree
            stopwatch.Start();
            var searchTree = BuildSegmentsSearchTree(pathToRanges);
            stopwatch.Stop();
            Console.WriteLine("Building of binary search tree time: {0}s", stopwatch.Elapsed.TotalSeconds);

            var treeHeight = searchTree.GetHeight();
            Console.WriteLine("Is tree balanced: {0}; MinHeight: {1}; MaxHeight: {2}", treeHeight.IsBalanced,
                treeHeight.MinHeight, treeHeight.MaxHeight);

            // Create empty result file
            stopwatch.Start();
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
            stopwatch.Stop();
            Console.WriteLine("Processing transactions time: {0}s", stopwatch.Elapsed.TotalSeconds);
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