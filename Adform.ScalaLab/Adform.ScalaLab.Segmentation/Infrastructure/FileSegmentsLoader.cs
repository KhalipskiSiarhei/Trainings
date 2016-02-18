using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Adform.ScalaLab.Segmentation.Core;

namespace Adform.ScalaLab.Segmentation.Infrastructure
{
    public class FileSegmentsLoader : FileLoaderBase, ISegmentsLoader
    {
        public FileSegmentsLoader(string pathToFile)
            : base(pathToFile)
        {
        }

        public List<Segment> GetNextSegments(int count = 1)
        {
            var segments = new List<Segment>();

            while (segments.Count < count)
            {
                var segmentStr = _reader.ReadLine();
                if (!String.IsNullOrEmpty(segmentStr))
                {
                    segments.Add(Convert(segmentStr));
                }
                else
                {
                    if (_reader.EndOfStream)
                    {
                        break;
                    }
                }
            }
            return segments;
        }

        private Segment Convert(string segmentStr)
        {
            var segmentParts = segmentStr.Split(new[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            var ipSegmentsParts = segmentParts[0].Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            var ipFrom = GetLongIpByStr(ipSegmentsParts[0]);
            var ipTo = GetLongIpByStr(ipSegmentsParts[1]);
            var name = segmentParts[1];

            return new Segment() { Range = new Range() { From = ipFrom, To = ipTo }, Name = name };
        }
    }
}