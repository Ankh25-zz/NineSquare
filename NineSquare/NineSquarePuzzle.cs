using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms.VisualStyles;

namespace NineSquare
{
    public class NineSquarePuzzle
    {
        public NineSquareElement[] NineSquareElements { get; private set; }

        public NineSquareElement[] SolvedNineSquareElements { get; private set; }

        public Queue<StepNode> StepsToSolve = new Queue<StepNode>();

        public bool IsLoaded
        {
            get { return ((NineSquareElements?.Count() ?? 0) == 9); }
        }


        public Queue<NineSquarePuzzle> VisualPuzzleSolve = new Queue<NineSquarePuzzle>();

        public static NineSquareElement[] GetElementsNear(ref NineSquareElement elementRelatedTo,
            ref NineSquareElement[] nineSquareElements)
        {
            if ((nineSquareElements?.Count() ?? 0) < 9)
                return null;

            var index = IndexOf(elementRelatedTo, nineSquareElements);
            if (index == -1) return null;

            int row = index/3;
            int col = index%3;

            NineSquareElement[] elements = new NineSquareElement[4];

            //Console.WriteLine("l t r b : {0} {1} {2} {3}", (col - 1 < 0 ? 2 : col - 1) + (row*3),
            //    col + ((row - 1 < 0 ? 2 : row - 1)*3), ((col + 1)%3) + (row*3), col + (((row + 1)%3)*3));

            // left
            elements[0] = nineSquareElements[(col - 1 < 0 ? 2 : col - 1) + (row*3)];
            // top
            elements[1] = nineSquareElements[col + ((row - 1 < 0 ? 2 : row - 1)*3)];
            // right
            elements[2] = nineSquareElements[((col + 1)%3) + (row*3)];
            // bottom
            elements[3] = nineSquareElements[col + (((row + 1)%3)*3)];

            return elements;
        }

        public static bool CheckSolvable(NineSquareElement[] elements)
        {
            int result = 0;

            // check non-null and element count == 9
            if ((elements?.Count() ?? 0) != 9)
                return false;


            // add up all edges
            // should return 0 if valid.
            elements.ToList()
                .ForEach(
                    element =>
                        result +=
                            element.EdgeLeftWeight + element.EdgeTopWeight + element.EdgeRightWeight +
                            element.EdgeBottomWeight);

            return result == 0;
        }


        public NineSquarePuzzle(NineSquareElement[] puzzleElements = null)
        {
            LoadPuzzle(puzzleElements);
        }

        public bool LoadPuzzle(NineSquareElement[] puzzleElements)
        {
            bool validPuzzle = CheckSolvable(puzzleElements);


            if (validPuzzle)
            {
                NineSquareElements = puzzleElements;
                for (int i = 0; i < 9; i++)
                {
                    if (i + 1 < 9)
                        NineSquareElements[i].ForwardElement = NineSquareElements[i + 1];
                    else
                        NineSquareElements[i].ForwardElement = null;
                }
            }


            return validPuzzle;
        }

        /// <summary>
        /// Solves this instance.
        /// </summary>
        /// <returns></returns>
        public bool Solve()
        {
            bool validPuzzle = false;
            Console.WriteLine("Puzzle isValid : {0}", validPuzzle = CheckSolvable(NineSquareElements));
            if (!validPuzzle) return false;

            // basic logic
            // elements, start
            // BT(elements, start)
            // -> if start == null return false
            // -> oldelements = elements.Clone() (useful for bt since, ref used)
            // -> -> assume valid path 
            // -> -> Save Instance,
            // -> -> if BT(elements, start.forward) = true, solved
            // -> else
            // -> -> RotateCW()
            // -> -> if BT(elements, start.forward) = true, solved
            // 


            NineSquareElement[] items = NineSquareElements.ToList().ToArray();
            Console.WriteLine("Initial Input : ");
            PrintPuzzle(NineSquareElements);

            bool result = BT(items[0], ref items);
            if (result)
            {
                Console.WriteLine();
                Console.WriteLine("Solved");
                SolvedNineSquareElements = items;
                Console.WriteLine(ToString());
                PrintPuzzle(items);
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Not Solved");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" Display Steps? (Steps Count {0}) [yes]", StepsToSolve.Count);
            if (Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine(" Steps : ");
                while (StepsToSolve.Count > 0 && StepsToSolve.Count == VisualPuzzleSolve.Count)
                {
                    var element = StepsToSolve.Dequeue();
                    var puzzle = VisualPuzzleSolve.Dequeue();

                    PrintPuzzle(puzzle.NineSquareElements);
                    Console.WriteLine(element.ToString());
                }
            }


            return result;
        }

        public int nestedCount = 0;

        public static bool VerifySolved(NineSquareElement[] elements)
        {
            if (elements?.Count() != 9) return false;

            int i = 0;
            bool result = true;

            foreach (var element in elements)
            {
                var directionalElements = GetElementsNear(ref elements[i++], ref elements);
                if (element.EdgeLeftWeight + directionalElements[0].EdgeRightWeight != 0 ||
                    element.EdgeTopWeight + directionalElements[1].EdgeBottomWeight != 0 ||
                    element.EdgeRightWeight + directionalElements[2].EdgeLeftWeight != 0 ||
                    element.EdgeBottomWeight + directionalElements[3].EdgeTopWeight != 0)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public static int IndexOf(NineSquareElement element, NineSquareElement[] elements)
        {
            var index = -1;
            for (var i = 0; i < 9; i++)
            {
                if (elements[i] == element)
                    index = i;
            }
            return index;
        }

        public bool BT(NineSquareElement start, ref NineSquareElement[] nineSquareElements)
        {
            int rotateCount = 0;
            NineSquareElement[] oldElements = null;
            // used here to clone current elements into oldElements
            Revert(ref nineSquareElements, ref oldElements);


            if (VerifySolved(nineSquareElements))
                return true;

            ++nestedCount;
            while (start != null)
            {
                var elements = GetElementsNear(ref start, ref nineSquareElements);

                if (rotateCount <= 3)
                {
                    if (VerifySolved(nineSquareElements))
                    {
                        --nestedCount;
                        return true;
                    }
                    else if (BT(start.ForwardElement, ref nineSquareElements))
                    {
                        --nestedCount;
                        // verify this particular node.

                        return true;
                    }

                    else if (rotateCount <= 3)
                    {
                        // Check & Rotate
                        StepsToSolve.Enqueue(new StepNode((NineSquareElement) start.Clone(),
                            IndexOf(start, nineSquareElements), true));

                        NineSquarePuzzle puzzle = new NineSquarePuzzle(nineSquareElements);
                        VisualPuzzleSolve.Enqueue(puzzle);

                        start.RotateCW();
                        rotateCount++;
                    }
                    else if (rotateCount > 3 && start != nineSquareElements[0])
                    {
                        // backtrack
                        //Console.WriteLine("BT...");
                        StepsToSolve.Enqueue(new StepNode((NineSquareElement) start.Clone(),
                            IndexOf(start, nineSquareElements), false, true));

                        Revert(ref oldElements, ref nineSquareElements);

                        NineSquarePuzzle puzzle = new NineSquarePuzzle(nineSquareElements);
                        VisualPuzzleSolve.Enqueue(puzzle);
                    }
                    else
                    {
                        --nestedCount;
                        return false;
                    }
                }
                else
                {
                    --nestedCount;
                    return false;
                }
            }
            --nestedCount;
            return false;
        }

        private void Revert(ref NineSquareElement[] oldElements, ref NineSquareElement[] newElements)
        {
            if (oldElements == null) return;

            if (newElements == null) newElements = new NineSquareElement[9];
            oldElements.CopyTo(newElements, 0);
            for (int i = 0; i < 9; i++)
            {
                if (i + 1 < 9)
                    newElements[i].ForwardElement = newElements[i + 1];
                else
                    newElements[i].ForwardElement = null;
            }
        }

        public static void PrintPuzzle(NineSquareElement[] elements)
        {
            string line1 = "", line2 = "", line3 = "";

            string s = "";
            int i = 0;
            for (i = 0; i <= 9; i++)
            {
                if (i%3 == 0 && i != 0)
                {
                    s = line1 + "\n" + line2 + "\n" + line3 + "\n" + new string('-', 16*3 + 5);
                    line1 = "";
                    line2 = "";
                    line3 = "";
                    Console.WriteLine(s);
                    if (i == 9)
                        break;
                }

                line1 += "\t" + elements[i].EdgeTopWeight + "\t  | ";
                line2 += elements[i].EdgeLeftWeight + "\t\t" + elements[i].EdgeRightWeight +
                         " | ";
                line3 += "\t" + elements[i].EdgeBottomWeight + "\t  | ";
            }
        }

        public override string ToString()
        {
            string str = "[";
            SolvedNineSquareElements?.ToList()?.ForEach(element => str += element.ToString());
            str += "]";
            return str;
        }
    }
}