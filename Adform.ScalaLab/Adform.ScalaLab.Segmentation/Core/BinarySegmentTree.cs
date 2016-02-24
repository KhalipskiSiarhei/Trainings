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

        /// <summary>
        /// Balance tree with DSV algorithm. For more info look at the following link: http://www.geekviewpoint.com/java/bst/dsw_algorithm
        /// </summary>
        public void BalanceWithDSV()
        {
            if (_root != null)
            {
                CreateBackbone();
                CreateBalancedTree();
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

        #region Aux methods

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

        private void CreateBackbone()
        {
            SegmentTreeNode grandParent = null;
            var parent = _root;
            SegmentTreeNode leftChild;

            while (null != parent)
            {
                leftChild = parent.Left;
                if (null != leftChild)
                {
                    grandParent = RotateRight(grandParent, parent, leftChild);
                    parent = leftChild;
                }
                else
                {
                    grandParent = parent;
                    parent = parent.Right;
                }
            }
        }

        private SegmentTreeNode RotateRight(SegmentTreeNode grandParent, SegmentTreeNode parent, SegmentTreeNode leftChild)
        {
            if (null != grandParent)
            {
                grandParent.Right = leftChild;
            }
            else
            {
                _root = leftChild;
            }
            parent.Left = leftChild.Right;
            leftChild.Right = parent;
            return grandParent;
        }

        /// <summary>
        /// Time complexity: O(n)
        /// </summary>
        private void CreateBalancedTree()
        {
            int n = 0;
            for (SegmentTreeNode tmp = _root; null != tmp; tmp = tmp.Right)
            {
                n++;
            }
            //m = 2^floor[lg(n+1)]-1, ie the greatest power of 2 less than n: minus 1
            int m = GetGreatestPowerOf2LessThanN(n + 1) - 1;
            MakeRotations(n - m);

            while (m > 1)
            {
                MakeRotations(m /= 2);
            }
        }

        /// <summary>
        /// Time complexity: log(n)
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private int GetGreatestPowerOf2LessThanN(int n)
        {
            int x = MSB(n);//MSB
            return (1 << x);//2^x
        }

        /// <summary>
        /// Time complexity: log(n)
        /// return the index of most significant set bit: index of
        /// least significant bit is 0
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int MSB(int n)
        {
            int ndx = 0;
            while (1 < n)
            {
                n = (n >> 1);
                ndx++;
            }
            return ndx;
        }

        private void MakeRotations(int bound)
        {
            SegmentTreeNode grandParent = null;
            SegmentTreeNode parent = _root;
            SegmentTreeNode child = _root.Right;
            for (; bound > 0; bound--)
            {
                try
                {
                    if (null != child)
                    {
                        RotateLeft(grandParent, parent, child);
                        grandParent = child;
                        parent = grandParent.Right;
                        child = parent.Right;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (NullReferenceException ex)
                {
                    break;
                }
            }
        }

        private void RotateLeft(SegmentTreeNode grandParent, SegmentTreeNode parent, SegmentTreeNode rightChild)
        {
            if (null != grandParent)
            {
                grandParent.Right = rightChild;
            }
            else
            {
                _root = rightChild;
            }
            parent.Right = rightChild.Left;
            rightChild.Left = parent;
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

        #endregion
    }
}