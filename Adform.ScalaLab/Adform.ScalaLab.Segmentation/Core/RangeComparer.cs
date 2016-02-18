using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Adform.ScalaLab.Segmentation.Core
{
    public static class RangeComparer
    {
        public static IntersectionResult Intersect(Range range1, Range range2)
        {
            var compareResult = range1.CompareTo(range2);

            if (compareResult != 0)
            {
                return null;
            }
            if (range1.From > range2.From)
            {
                return Intersect(range2, range1);
            }

            // r1.From-r1.To-r2.From-r2-To
            if (range1.To < range2.From)
            {
                return null;
            }
            // r1.From-r2.From-r1.To-r2.To
            if (range1.From <= range2.From && range2.From <= range1.To && range1.To <= range2.To)
            {
                Range leftSetDifference = null;
                var intersection = new Range() { From = Min(range1.To, range2.From), To = Max(range1.To, range2.From) };
                Range rightSetDifference = null;

                if (range1.From < intersection.From)
                {
                    leftSetDifference = new Range() {From = range1.From, To = intersection.From - 1};
                }
                if (range2.To > intersection.To)
                {
                    rightSetDifference = new Range() { From = intersection.To + 1, To = range2.To };
                }

                return new IntersectionResult()
                {
                    Intersection = intersection,
                    LeftSetDifference = leftSetDifference,
                    RightSetDifference = rightSetDifference
                };
            }
            // r1.From-r2.From-r2.To-r2.1rom
            if (range1.From <= range2.From && range2.To <= range1.To)
            {
                Range leftSetDifference = null;
                var intersection = new Range() { From = Max(range1.From, range2.From), To = Min(range1.To, range2.To) };
                Range rightSetDifference = null;

                if (range1.From < intersection.From)
                {
                    leftSetDifference = new Range() { From = range1.From, To = intersection.From - 1 };
                }
                if (range1.To > intersection.To)
                {
                    rightSetDifference = new Range() { From = intersection.To + 1, To = range1.To };
                }

                return new IntersectionResult()
                {
                    Intersection = intersection,
                    LeftSetDifference = leftSetDifference,
                    RightSetDifference = rightSetDifference
                };
            }

            throw new NotSupportedException(
                string.Format(
                    "The ranges (From: {0}, To: {1}) and (From: {2}, To: {3}) are not supported to intersect",
                    range1.From, range1.To, range2.From, range2.To));
        }

        public static long Min(long i1, long i2)
        {
            if (i1 <= i2)
            {
                return i1;
            }
            return i2;
        }

        public static long Max(long i1, long i2)
        {
            if (i1 >= i2)
            {
                return i1;
            }
            return i2;
        }
    }
}
