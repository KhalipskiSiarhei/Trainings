using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adform.ScalaLab.Segmentation.Core
{
    public class Range : IComparable<Range>, IComparable<long>
    {
        public long From { get; set; }

        public long To { get; set; }

        public int CompareTo(Range other)
        {
            if (To < other.From)
            {
                return -1;
            }
            if (From > other.To)
            {
                return 1;
            }
            return 0;
        }

        public int CompareTo(long other)
        {
            if (To < other)
            {
                return -1;
            }
            if (From > other)
            {
                return 1;
            }
            return 0;
        }
    }
}