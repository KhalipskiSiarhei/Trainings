using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adform.ScalaLab.Segmentation.Core
{
    public class SegmentTreeNode
    {
        public Range Range { get; set; }

        public List<string> Names { get; set; }

        public SegmentTreeNode Left { get; set; }

        public SegmentTreeNode Right { get; set; }
    }
}
