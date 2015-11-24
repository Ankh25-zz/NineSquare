using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NineSquare
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NineSquareElement[] elements = new NineSquareElement[9];

            for (int i = 0; i < 9; i++)
            {
                int left = 0, right = 0, top = 0, bottom = 0;

                var textBox = FindControl("textBox" + (i + 1) + "l") as TextBox;
                int.TryParse(textBox.Text.Trim(), out left);
                textBox = FindControl("textBox" + (i + 1) + "t") as TextBox;
                int.TryParse(textBox.Text.Trim(), out top);
                textBox = FindControl("textBox" + (i + 1) + "r") as TextBox;
                int.TryParse(textBox.Text.Trim(), out right);
                textBox = FindControl("textBox" + (i + 1) + "b") as TextBox;
                int.TryParse(textBox.Text.Trim(), out bottom);

                NineSquareElement element = new NineSquareElement(left, top, right, bottom);
                elements[i] = element;
            }

            NineSquarePuzzle puzzle = new NineSquarePuzzle(elements);
            if (!NineSquarePuzzle.CheckSolvable(elements))
                MessageBox.Show("Invalid Puzzle..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (puzzle.Solve())
            {
                for (int i = 0; i < 9; i++)
                {
                    int left = 0, right = 0, top = 0, bottom = 0;

                    var textBox = FindControl("textBox" + (i + 1) + "l") as TextBox;
                    textBox.Text = puzzle.SolvedNineSquareElements[i].EdgeLeftWeight.ToString();
                    textBox = FindControl("textBox" + (i + 1) + "t") as TextBox;
                    textBox.Text = puzzle.SolvedNineSquareElements[i].EdgeTopWeight.ToString();
                    textBox = FindControl("textBox" + (i + 1) + "r") as TextBox;
                    textBox.Text = puzzle.SolvedNineSquareElements[i].EdgeRightWeight.ToString();
                    textBox = FindControl("textBox" + (i + 1) + "b") as TextBox;
                    textBox.Text = puzzle.SolvedNineSquareElements[i].EdgeBottomWeight.ToString();
                }
            }
        }

        private Control FindControl(string key)
        {
            foreach (var grpBox in groupBox1.Controls)
            {
                dynamic ctrlContainer = grpBox;
                var control = ctrlContainer.Controls[key] as Control;
                if (control != null)
                {
                    return control;
                }
            }

            return null;
        }
    }
}