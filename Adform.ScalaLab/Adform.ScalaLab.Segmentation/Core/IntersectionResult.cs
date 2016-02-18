using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adform.ScalaLab.Segmentation.Core
{
    public class IntersectionResult
    {
        public Range LeftSetDifference { get; set; }

        public Range Intersection { get; set; }

        public Range RightSetDifference { get; set; }
    }
}
