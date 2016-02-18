using Adform.ScalaLab.Segmentation.Core;
using Adform.ScalaLab.Segmentation.Infrastructure;
using NUnit.Framework;

namespace Adform.ScalaLab.Segmentation.Tests
{
    [TestFixture]
    public class BinarySegmentTreeTests
    {
        private BinarySegmentTree _tree;

        [SetUp]
        public void SetUp()
        {
            _tree = new BinarySegmentTree();
        }

        [Test]
        public void BuildWithLoader_TreeBuilt()
        {
            var loader = new FileSegmentsLoader("ranges.tsv");

            loader.Open();
            try
            {
                _tree.BuildWithLoader(loader);
            }
            finally
            {
                loader.Close();
            }

            _tree.FindSegmentsNamesById(63173067218).Contains("Network57");
            _tree.FindSegmentsNamesById(1189017250).Contains("Network44");
            _tree.FindSegmentsNamesById(27147234109).Contains("Network30");
            _tree.FindSegmentsNamesById(54164250106).Contains("Network62");
            _tree.FindSegmentsNamesById(120172038009).Contains("Network67");
        }
    }
}