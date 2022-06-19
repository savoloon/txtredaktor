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
    public partial class Form3 : Form
    {
        public string f3text;
        public Form3()
        {
            InitializeComponent();
            Name = "FIND";
        }
        public Form3(Form1 f)
        {
            //this.KeyPress += new KeyEventHandler(Form3_KeyPress);
            InitializeComponent();
            Name = "FIND";
            //Form1 main = this.Owner as Form1;
            if (f.richTextBox1.SelectedText.Length != 0)
            {
                textBox1.Text = f.richTextBox1.SelectedText;
            }
            // Form1 main = this.Owner as Form1;
        }
        public event EventHandler ButtonClicked;

        private void button1_click(object sender, EventArgs e)
        {
            OnButtonClicked(EventArgs.Empty);
        }
        protected void OnButtonClicked(EventArgs e)
        {
            var evt = ButtonClicked;
            if (evt != null) evt(this, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                button1.Enabled = true;
                AcceptButton = button1;
            }
            else
            {
                button1.Enabled = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            int findex = FindMyText(textBox1.Text, main.richTextBox1.SelectionStart, radioButton2.Checked);
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F3" && e.Shift)
            {
                if (textBox1.Text != "")
                {
                    Form1 main = this.Owner as Form1;
                    int findex = FindMyText(textBox1.Text, main.richTextBox1.SelectionStart, false);
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
            }
            else
            {
                if (e.KeyCode.ToString() == "F3")
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
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 main = this.Owner as Form1;
            f3text = textBox1.Text;
            main.FindTextStr = f3text;
        }
    }
}
