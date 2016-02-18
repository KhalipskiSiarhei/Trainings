using NUnit.Framework;

namespace Adform.ScalaLab.Segmentation.Tests
{
    [TestFixture]
    public class SegmentationProcessorTests
    {
        private SegmentationProcessor _segmentationProcessor;

        [SetUp]
        public void SetUp()
        {
            _segmentationProcessor = new SegmentationProcessor();
        }

        [TestCase("ranges.tsv", "transactions.tsv")]
        public void Process(string pathToRanges, string pathToTransactions)
        {
            _segmentationProcessor.Process(pathToRanges, pathToTransactions);
        }
    }
}