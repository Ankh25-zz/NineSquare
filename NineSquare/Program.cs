using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineSquare
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            //NineSquareElement[] elements = new NineSquareElement[9];
            //elements[0] = new NineSquareElement(-1, 3, -2, 1);
            //elements[1] = new NineSquareElement(-3, -2, 1, -3);
            //elements[2] = new NineSquareElement(4, -1, -3, 2);

            //elements[3] = new NineSquareElement(-2, 1, 4, 2);
            //elements[4] = new NineSquareElement(2, -1, 4, -4);
            //elements[5] = new NineSquareElement(-4, 3, -4, 1);

            //elements[6] = new NineSquareElement(1, 2, -1, 3);
            //elements[7] = new NineSquareElement(4, -2, 3, -3);
            //elements[8] = new NineSquareElement(2, -1, -2, -4);


            //NineSquarePuzzle puzzleSolver = new NineSquarePuzzle(elements);
            //puzzleSolver.Solve();
            //Console.WriteLine("Press Any Key To Continue.");
            //Console.ReadKey();
        }
    }
}