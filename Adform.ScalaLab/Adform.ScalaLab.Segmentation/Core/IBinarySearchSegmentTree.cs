using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Adform.ScalaLab.Segmentation.Core
{
    public interface IBinarySearchSegmentTree
    {
        List<string> FindSegmentsNamesById(long ip);
    }
}
