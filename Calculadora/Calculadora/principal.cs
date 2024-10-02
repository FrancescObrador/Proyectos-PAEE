using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculadora
{
    public partial class principal : Form
    {
        enum Operations { Add, Sub, Mult, Div, Pow, Root };
        private decimal operator1 = 0, operator2 = 0;
        private Operations operation;
        private decimal result;

        public principal()
        {
            InitializeComponent();
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Autor: Francesc M. Obrador Artigues \nVersion: 0.0.1",
                "Acerca de: Calculadora",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void AddNumber(string num)
        {
            string txt;

            if (lblDisplay.Text.EndsWith(",0"))
            {
                txt = lblDisplay.Text.Remove(lblDisplay.Text.Length-1) + num;
            }
            else
            {
                txt = lblDisplay.Text + num;
            }

            decimal number = 0;
            
            try
            {
                number = Convert.ToDecimal(txt);
                lblDisplay.Text = number.ToString();
            } catch
            {
                throw new Exception("Something went wrong");
            }
        }

        private void principal_Load(object sender, EventArgs e)
        {

        }

        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Seguro que quieres salir?",
                "Cerrar Calculadora",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AddNumber("0");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddNumber("1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddNumber("2");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddNumber("3");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddNumber("4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddNumber("5");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddNumber("6");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddNumber("7");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddNumber("8");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AddNumber("9");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AddNumber(",0");
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            operator1 = Convert.ToDecimal(lblDisplay.Text);
            operation = Operations.Add;
            lblDisplay.Text = "0";
            statusBar.Text += operator1 + "+";
        }

        private void buttonSub_Click(object sender, EventArgs e)
        {
            operator1 = Convert.ToDecimal(lblDisplay.Text);
            operation = Operations.Sub;
            lblDisplay.Text = "0";
            statusBar.Text += operator1 + "-";
        }

        private void buttonMutl_Click(object sender, EventArgs e)
        {
            operator1 = Convert.ToDecimal(lblDisplay.Text);
            operation = Operations.Mult;
            lblDisplay.Text = "0";
            statusBar.Text += operator1 + "x";
        }
        private void buttonDiv_Click(object sender, EventArgs e)
        {
            operator1 = Convert.ToDecimal(lblDisplay.Text);
            operation = Operations.Div;
            lblDisplay.Text = "0";
            statusBar.Text += operator1 + "/";
        }

        private void buttonPow(object sender, EventArgs e)
        {
            operator1 = Convert.ToDecimal(lblDisplay.Text);
            operation = Operations.Pow;
            lblDisplay.Text = "0";
            statusBar.Text += operator1 + "^";
        }
        private void buttonSquareRoot_Click(object sender, EventArgs e)
        {
            operator1 = Convert.ToDecimal(lblDisplay.Text);
            operation = Operations.Root;
            Calculate();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            operator2 = Convert.ToDecimal(lblDisplay.Text);
            Calculate();
        }

        private void Calculate()
        {
            try
            {
                switch (operation)
                {
                    case Operations.Add:
                        result = operator1 + operator2;
                        break;
                    case Operations.Sub:
                        result = operator1 - operator2;
                        break;
                    case Operations.Mult:
                        result = operator1 * operator2;
                        break;
                    case Operations.Div:
                        if (operator2 != 0)
                        {
                            result = operator1 / operator2;
                        }
                        else
                        {
                            MessageBox.Show("A number can not be divided by zero", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    case Operations.Pow:
                        result = (decimal)Math.Pow((double)operator1, (double)operator2);
                        break;
                    case Operations.Root:
                        result = (decimal)Math.Sqrt((double)operator1);
                        break;
                    default:
                        break;
                }
                lblDisplay.Text = result.ToString();
                statusBar.Text = "";
                operator1 = operator2 = 0;
            }
            catch
            {
                MessageBox.Show("Operation have been overflown",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            lblDisplay.Text = "0";
            statusBar.Text = "";
            operator1 = operator2 = 0;
        }

        private void buttonNegative_Click(object sender, EventArgs e)
        {
            decimal num = Convert.ToDecimal(lblDisplay.Text);
            num = -num;
            lblDisplay.Text = num.ToString();
        }


    }
}
