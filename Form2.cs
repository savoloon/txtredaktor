using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace txtredaktor
{
    public partial class Form2 : Form
    {
        public int l;
        public Form2(int lenght)
        {
            InitializeComponent();
            this.AcceptButton = button1;
            l = lenght;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;

            int str = Int32.Parse(textBox1.Text);
            //label1.Text = "l=" + l.ToString() + " str=" + str.ToString();
            if (str <= l)
            {
                main.richTextBox1.SelectionStart = main.richTextBox1.GetFirstCharIndexFromLine(str - 1);
                this.Close();
                main.Focus();

            }
            else
            {
                MessageBox.Show("Номер строки превышает общее число строк.", "Неверное значение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
            main.Focus();
            int pos = main.richTextBox1.SelectionStart; // get starting point
            int line = main.richTextBox1.GetLineFromCharIndex(pos); // get line number
            int column = main.richTextBox1.SelectionStart - main.richTextBox1.GetFirstCharIndexFromLine(line);    // get column number
            main.toolStripStatusLabel1.Text = "Стр. " + (line + 1) + ", Столб. " + (column + 1);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 0)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
    }
}
