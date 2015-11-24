using System;

namespace NineSquare
{
    public class StepNode
    {
        public NineSquareElement StartAtElement { get; set; }

        public bool RotateCW { get; set; }

        public int ElementIndex { get; set; }

        public bool BT { get; set; }

        public StepNode(NineSquareElement startElement = null, int index = -1, bool rotated = false, bool bt = false)
        {
            StartAtElement = startElement;
            ElementIndex = index;
            RotateCW = rotated;
            BT = bt;
        }

        public override string ToString()
        {
            return "Step Trace, Element index " + ElementIndex + ": (" + (RotateCW ? "Rotated" : "NOT Rotated") + ")" +
                   StartAtElement.ToString() + (BT ? " [BACKTRACK]" : "");
        }
    }
}