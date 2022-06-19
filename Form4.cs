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
    public partial class Form4 : Form
    {
        public Form4(Form1 f)
        {
            InitializeComponent();
            Name = "REPLACE";
            if (f.richTextBox1.SelectedText.Length != 0)
            {
                textBox1.Text = f.richTextBox1.SelectedText;
            }
        }
        public int FindMyText(string text, int start, bool route)
        {
            //
            Form1 main = this.Owner as Form1;
            //
            int returnValue = -1;
            if (text.Length > 0 && main.richTextBox1.SelectionStart >= 0)
            {
                if (route == true && (start + main.richTextBox1.SelectedText.Length != main.richTextBox1.Text.Length))
                {
                    int indexToText = main.richTextBox1.Find(text, main.richTextBox1.SelectionStart + main.richTextBox1.SelectedText.Length, RichTextBoxFinds.None);
                    return indexToText;
                }
                if (route == false && (start != 0))
                {
                    int indexToText = main.richTextBox1.Find(text, 0, main.richTextBox1.SelectionStart, RichTextBoxFinds.Reverse);
                    return indexToText;
                }
            }
            return returnValue;
        }
        int rtn = 0;

        public int getFinds(string text, int start, bool route)
        {
            //
            Form1 main = this.Owner as Form1;
            //

            if (text.Length > 0 && main.richTextBox1.SelectionStart >= 0)
            {
                if (route == true && (start + main.richTextBox1.SelectedText.Length != main.richTextBox1.Text.Length))
                {
                    int indexToText = main.richTextBox1.Find(text, main.richTextBox1.SelectionStart + main.richTextBox1.SelectedText.Length, RichTextBoxFinds.None);
                    rtn++;
                    return indexToText;
                }
            }
            return rtn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            int findex = FindMyText(textBox1.Text, main.richTextBox1.SelectionStart, true);
            if (findex == -1) //error
            {
                DialogResult error;
                error = MessageBox.Show("Не удается найти '" + textBox1.Text + "'", "Найти",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                switch (error)
                {
                    case DialogResult.OK:
                        break;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                button2.Enabled = true;
                button3.Enabled = true;
                button1.Enabled = true;
                AcceptButton = button1;
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            if (main.richTextBox1.SelectedText != "")
            {
                main.richTextBox1.SelectedText = textBox2.Text;
                button1_Click(button1, e);
            }
            else
                button1_Click(button1, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            main.richTextBox1.SelectionStart = 0;
            int find = getFinds(textBox1.Text, 0, true);
            while (FindMyText(textBox1.Text, main.richTextBox1.SelectionStart, true) != -1)
            {

                main.richTextBox1.SelectedText = textBox2.Text;
            }
        }


    }
}
