using Adform.ScalaLab.Segmentation.Core;
using NUnit.Framework;

namespace Adform.ScalaLab.Segmentation.Tests.Unit
{
    [TestFixture]
    public class RangeComparerTests
    {
        [TestCase(10, 15, 20, 30)]
        [TestCase(20, 30, 10, 15)]
        [TestCase(10, 20, 21, 25)]
        [TestCase(21, 25, 10, 20)]
        public void Intersect_NoIntersection_NullReturned(long from1, long to1, long from2, long to2)
        {
            var range1 = new Range() { From = from1, To = to1 };
            var range2 = new Range() { From = from2, To = to2 };

            Assert.IsNull(RangeComparer.Intersect(range1, range2));
        }

        [TestCase(10, 20, 20, 30)]
        [TestCase(20, 30, 10, 20)]
        public void Intersect_R1FromR1FromEqualR2FromR2To_IntersectionReturned(long from1, long to1, long from2, long to2)
        {
            var range1 = new Range() { From = from1, To = to1 };
            var range2 = new Range() { From = from2, To = to2 };

            var intersection = RangeComparer.Intersect(range1, range2);
            Assert.IsTrue(intersection.LeftSetDifference.From == RangeComparer.Min(from1, from2));
            Assert.IsTrue(intersection.LeftSetDifference.To == intersection.Intersection.From - 1);
            Assert.IsTrue(intersection.Intersection.From == RangeComparer.Min(to1, to2));
            Assert.IsTrue(intersection.Intersection.To == RangeComparer.Max(from1, from2));
            Assert.IsTrue(intersection.RightSetDifference.From == intersection.Intersection.To + 1);
            Assert.IsTrue(intersection.RightSetDifference.To == RangeComparer.Max(to1, to2));
        }

        [TestCase(10, 20, 15, 30)]
        [TestCase(15, 30, 10, 20)]
        public void Intersect_R1FromR2FromR1ToR2To_IntersectionReturned(long from1, long to1, long from2, long to2)
        {
            var range1 = new Range() { From = from1, To = to1 };
            var range2 = new Range() { From = from2, To = to2 };

            var intersection = RangeComparer.Intersect(range1, range2);
            Assert.IsTrue(intersection.LeftSetDifference.From == RangeComparer.Min(from1, from2));
            Assert.IsTrue(intersection.LeftSetDifference.To == intersection.Intersection.From - 1);
            Assert.IsTrue(intersection.Intersection.From == RangeComparer.Max(from1, from2));
            Assert.IsTrue(intersection.Intersection.To == RangeComparer.Min(to1, to2));
            Assert.IsTrue(intersection.RightSetDifference.From == intersection.Intersection.To + 1);
            Assert.IsTrue(intersection.RightSetDifference.To == RangeComparer.Max(to1, to2));
        }

        [TestCase(10, 40, 20, 30)]
        [TestCase(20, 30, 10, 40)]
        public void Intersect_R1FromR2FromR2ToR1To_IntersectionReturned(long from1, long to1, long from2, long to2)
        {
            var range1 = new Range() { From = from1, To = to1 };
            var range2 = new Range() { From = from2, To = to2 };

            var intersection = RangeComparer.Intersect(range1, range2);
            Assert.IsTrue(intersection.LeftSetDifference.From == RangeComparer.Min(from1, from2));
            Assert.IsTrue(intersection.LeftSetDifference.To == intersection.Intersection.From - 1);
            Assert.IsTrue(intersection.Intersection.From == RangeComparer.Max(from1, from2));
            Assert.IsTrue(intersection.Intersection.To == RangeComparer.Min(to1, to2));
            Assert.IsTrue(intersection.RightSetDifference.From == intersection.Intersection.To + 1);
            Assert.IsTrue(intersection.RightSetDifference.To == RangeComparer.Max(to1, to2));
        }

        [TestCase(10, 40, 10, 20)]
        [TestCase(10, 20, 10, 40)]
        public void Intersect_NoLeftSetDifference_IntersectionReturned(long from1, long to1, long from2, long to2)
        {
            var range1 = new Range() { From = from1, To = to1 };
            var range2 = new Range() { From = from2, To = to2 };

            var intersection = RangeComparer.Intersect(range1, range2);
            Assert.IsTrue(intersection.LeftSetDifference == null);
            Assert.IsTrue(intersection.Intersection.From == from1);
            Assert.IsTrue(intersection.Intersection.To == RangeComparer.Min(to1, to2));
            Assert.IsTrue(intersection.RightSetDifference.From == intersection.Intersection.To + 1);
            Assert.IsTrue(intersection.RightSetDifference.To == RangeComparer.Max(to1, to2));
        }

        [TestCase(10, 40, 20, 40)]
        [TestCase(20, 40, 10, 40)]
        public void Intersect_NoRightSetDifference_IntersectionReturned(long from1, long to1, long from2, long to2)
        {
            var range1 = new Range() { From = from1, To = to1 };
            var range2 = new Range() { From = from2, To = to2 };

            var intersection = RangeComparer.Intersect(range1, range2);
            Assert.IsTrue(intersection.LeftSetDifference.From == RangeComparer.Min(from1, from2));
            Assert.IsTrue(intersection.LeftSetDifference.To == intersection.Intersection.From - 1);
            Assert.IsTrue(intersection.Intersection.From == RangeComparer.Max(from1, from2));
            Assert.IsTrue(intersection.Intersection.To == RangeComparer.Min(to1, to2));
            Assert.IsTrue(intersection.RightSetDifference == null);
        }

        [TestCase(10, 40)]
        public void Intersect_RangesAreEqual_IntersectionReturned(long from, long to)
        {
            var range1 = new Range() { From = from, To = to };
            var range2 = new Range() { From = from, To = to };

            var intersection = RangeComparer.Intersect(range1, range2);
            Assert.IsTrue(intersection.LeftSetDifference == null);
            Assert.IsTrue(intersection.RightSetDifference == null);
            Assert.IsTrue(intersection.Intersection.From == from);
            Assert.IsTrue(intersection.Intersection.To == to);
        }
    }
}