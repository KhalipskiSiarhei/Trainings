using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adform.ScalaLab.Segmentation.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var processor = new SegmentationProcessor();
                var pathToRanges = args.Length > 0 ? args[0] : "ranges.tsv";
                var pathToTransactions = args.Length > 1 ? args[1] : "transactions.tsv";
                var pathToResults = args.Length > 2 ? args[2] : "results.tsv";

                System.Console.WriteLine(
                    "Start: pathToRanges: {0}; pathToTransactions: {1}; pathToResults: {2}",
                    pathToRanges, pathToTransactions, pathToResults);
                processor.Process(pathToRanges, pathToTransactions, pathToResults);
                System.Console.WriteLine("Finished! The results are in {0}. Press any key to exit", pathToResults);
            }
            catch (Exception ex)
            {
                System.Console.Write(ex);
            }
            finally
            {
                System.Console.ReadKey();
            }
        }
    }
}