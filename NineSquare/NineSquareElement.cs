using System;
using System.Windows.Forms;

namespace NineSquare
{
    public class NineSquareElement : IComparable<NineSquareElement>, ICloneable
    {
        public int EdgeLeftWeight { get; set; }
        public int EdgeRightWeight { get; set; }
        public int EdgeTopWeight { get; set; }
        public int EdgeBottomWeight { get; set; }

        public NineSquareElement ForwardElement { get; set; }

        public int[] Edges
        {
            get
            {
                int[] edges = new int[] {EdgeLeftWeight, EdgeTopWeight, EdgeRightWeight, EdgeBottomWeight};
                return edges;
            }
        }

        public NineSquareElement() : this(0, 0, 0, 0)
        {
        }

        public NineSquareElement(int left, int top, int right, int bottom)
        {
            EdgeLeftWeight = left;
            EdgeTopWeight = top;
            EdgeRightWeight = right;
            EdgeBottomWeight = bottom;
        }

        /// <summary>
        /// Rotates the Element ClockWise.
        /// </summary>
        public void RotateCW()
        {
            var initialLeft = EdgeLeftWeight;
            EdgeLeftWeight = EdgeBottomWeight;
            EdgeBottomWeight = EdgeRightWeight;
            EdgeRightWeight = EdgeTopWeight;
            EdgeTopWeight = initialLeft;
        }

        public void Print()
        {
            Console.Write("\t{0}\n{1}\t\t{2}\n\t{3}\n", EdgeTopWeight, EdgeLeftWeight, EdgeRightWeight, EdgeBottomWeight);
        }

        public int CompareTo(NineSquareElement other)
        {
            return (EdgeLeftWeight == other.EdgeLeftWeight &&
                    EdgeTopWeight == other.EdgeTopWeight && EdgeBottomWeight == other.EdgeBottomWeight &&
                    EdgeRightWeight ==
                    other.EdgeRightWeight)
                ? 0
                : 1;
        }

        public override string ToString()
        {
            return "[" + EdgeLeftWeight + "," + EdgeTopWeight + "," + EdgeRightWeight + "," + EdgeBottomWeight + "]";
        }

        public object Clone()
        {
            return new NineSquareElement(EdgeLeftWeight, EdgeTopWeight, EdgeRightWeight, EdgeBottomWeight);
        }
    }
}