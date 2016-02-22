using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adform.ScalaLab.Segmentation.Core
{
    public class BinarySegmentTree : IBinarySearchSegmentTree
    {
        private SegmentTreeNode _root;

        public BinarySegmentTree()
        {
            _root = null;
        }

        public void BuildWithLoader(ISegmentsLoader loader)
        {
            while (true)
            {
                var segments = loader.GetNextSegments();

                if (segments != null && segments.Count > 0)
                {
                    segments.ForEach(Insert);
                }
                else
                {
                    return;
                }
            }
        }

        public void Insert(Segment segment)
        {
            if (_root == null)
            {
                _root = new SegmentTreeNode() { Range = segment.Range, Names = new List<string>() { segment.Name } };
            }
            else
            {
                DoInsert(_root, segment);
            }
        }

        public SegmentTreeNode Find(long ip)
        {
            if (_root == null)
            {
                return null;
            }

            return DoFind(_root, ip);
        }

        public List<string> FindSegmentsNamesById(long ip)
        {
            var node = Find(ip);

            if (node != null)
            {
                return node.Names;
            }
            return null;
        }

        public BinarySearchTreeHeight GetHeight()
        {
            var treeHeight = new BinarySearchTreeHeight();

            if (_root == null)
            {
                return treeHeight;
            }

            treeHeight.MinHeight = GetMinHeight(_root, 0);
            treeHeight.MaxHeight = GetMaxHeight(_root, 0);
            return treeHeight;
        }

        private void DoInsert(SegmentTreeNode top, Segment segment)
        {
            var compareResult = segment.Range.CompareTo(top.Range);

            if (compareResult < 0)
            {
                AddLeftLeaf(top, segment);
            }
            else if (compareResult > 0)
            {
                AddRightLeaf(top, segment);
            }
            else
            {
                var intersection = RangeComparer.Intersect(top.Range, segment.Range);

                top.Range = intersection.Intersection;
                top.Names.Add(segment.Name);

                if (intersection.LeftSetDifference != null)
                {
                    AddLeftLeaf(top, new Segment() { Range = intersection.LeftSetDifference, Name = segment.Name });
                }
                if (intersection.RightSetDifference != null)
                {
                    AddRightLeaf(top, new Segment() { Range = intersection.RightSetDifference, Name = segment.Name });
                }
            }
        }

        private void AddLeftLeaf(SegmentTreeNode top, Segment segment)
        {
            if (top.Left == null)
            {
                top.Left = new SegmentTreeNode()
                {
                    Range = segment.Range,
                    Names = new List<string>() { segment.Name }
                };
            }
            else
            {
                DoInsert(top.Left, segment);
            }
        }

        private void AddRightLeaf(SegmentTreeNode top, Segment segment)
        {
            if (top.Right == null)
            {
                top.Right = new SegmentTreeNode()
                {
                    Range = segment.Range,
                    Names = new List<string>() { segment.Name }
                };
            }
            else
            {
                DoInsert(top.Right, segment);
            }
        }

        private SegmentTreeNode DoFind(SegmentTreeNode top, long ip)
        {
            var compareResult = top.Range.CompareTo(ip);

            if (compareResult == 0)
            {
                return top;
            }
            else if (compareResult > 0 && top.Left != null)
            {
                return DoFind(top.Left, ip);
            }
            else if (compareResult < 0 && top.Right != null)
            {
                return DoFind(top.Right, ip);
            }
            return null;
        }

        private int GetMinHeight(SegmentTreeNode top, int currentHeight)
        {
            int? leftTreeHeight = null;
            int? rightTreeHeight = null;

            if (top.Left == null && top.Right == null)
            {
                return currentHeight + 1;
            }
            if (top.Left != null)
            {
                leftTreeHeight = GetMinHeight(top.Left, currentHeight + 1);
            }
            if (top.Right != null)
            {
                rightTreeHeight = GetMinHeight(top.Right, currentHeight + 1);
            }

            if (leftTreeHeight.HasValue && rightTreeHeight.HasValue)
            {
                return leftTreeHeight.Value > rightTreeHeight.Value ? rightTreeHeight.Value : leftTreeHeight.Value;
            }
            else if (leftTreeHeight.HasValue)
            {
                return leftTreeHeight.Value;
            }
            else if (rightTreeHeight.HasValue)
            {
                return rightTreeHeight.Value;
            }

            throw new InvalidOperationException("The current tree structure is not supported to calculate tree height");
        }

        private int GetMaxHeight(SegmentTreeNode top, int currentHeight)
        {
            int? leftTreeHeight = null;
            int? rightTreeHeight = null;

            if (top.Left == null && top.Right == null)
            {
                return currentHeight + 1;
            }
            if (top.Left != null)
            {
                leftTreeHeight = GetMaxHeight(top.Left, currentHeight + 1);
            }
            if (top.Right != null)
            {
                rightTreeHeight = GetMaxHeight(top.Right, currentHeight + 1);
            }

            if (leftTreeHeight.HasValue && rightTreeHeight.HasValue)
            {
                return leftTreeHeight.Value > rightTreeHeight.Value ? leftTreeHeight.Value : rightTreeHeight.Value;
            }
            else if (leftTreeHeight.HasValue)
            {
                return leftTreeHeight.Value;
            }
            else if (rightTreeHeight.HasValue)
            {
                return rightTreeHeight.Value;
            }

            throw new InvalidOperationException("The current tree structure is not supported to calculate tree height");
        }
    }
}