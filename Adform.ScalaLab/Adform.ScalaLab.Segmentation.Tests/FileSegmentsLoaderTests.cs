using System;
using Adform.ScalaLab.Segmentation.Core;
using Adform.ScalaLab.Segmentation.Infrastructure;
using NUnit.Framework;

namespace Adform.ScalaLab.Segmentation.Tests
{
    [TestFixture]
    public class FileSegmentsLoaderTests
    {
        private FileSegmentsLoader _fileSegmentsLoader;

        [SetUp]
        public void SetUp()
        {
            _fileSegmentsLoader = new FileSegmentsLoader("ranges.tsv");
            _fileSegmentsLoader.Open();
        }

        [TearDown]
        public void TearDown()
        {
            _fileSegmentsLoader.Close();
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void GetNextSegments_LoadByN_NItemsReturned(int count)
        {
            var segments = _fileSegmentsLoader.GetNextSegments(count);

            Assert.IsTrue(segments != null);
            Assert.IsTrue(segments.Count == count);
            segments.ForEach(AssertSegment);
        }

        [Test]
        public void GetNextSegments_LoadAll_AllItemsLoaded()
        {
            var segments = _fileSegmentsLoader.GetNextSegments(Int32.MaxValue);

            Assert.IsTrue(segments.Count <= Int32.MaxValue);
            segments.ForEach(AssertSegment);
        }

        private void AssertSegment(Segment segment)
        {
            Assert.IsTrue(segment.Range.From > 0);
            Assert.IsTrue(segment.Range.To > 0);
            Assert.IsTrue(segment.Range.From <= segment.Range.To);
            Assert.IsFalse(string.IsNullOrEmpty(segment.Name));
        }
    }
}