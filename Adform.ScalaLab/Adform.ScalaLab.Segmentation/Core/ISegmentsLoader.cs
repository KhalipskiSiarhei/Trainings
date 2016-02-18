using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adform.ScalaLab.Segmentation.Core
{
    public interface ISegmentsLoader
    {
        void Open();

        List<Segment> GetNextSegments(int count=1);

        void Close();
    }
}