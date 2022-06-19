using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace txtredaktor
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        public string FindTextStr = "";
        private string fn = string.Empty;
        int pos, line, column;
        List<string> colorList = new List<string>();
        private bool docChanged = false; // true - в тексте есть изменения
        Font defaultfont;
        Size defaultsize;
        public Form1()
        {
            InitializeComponent();
            переносПоСловамToolStripMenuItem.Checked = true;
            richTextBox1.Text = string.Empty;
            this.Text = "PriEdit - Новый документ";
            // отобразить панель инструментов
            toolStrip1.Visible = true;
            // назначаем файл справки
            // helpProvider1.HelpNamespace = "priedit.chm";
            // настройка компонента openDialog1
            openFileDialog1.Title = "Открыть документ";
            openFileDialog1.Multiselect = false;
            // настройка компонента saveDialog1
            saveFileDialog1.DefaultExt = "rtf";
            saveFileDialog1.Filter = "Text document (*.txt)|*.txt|MS Word dosuments (*.doc)|*.doc|Rich text format (*.rtf)|*.rtf";
            saveFileDialog1.Title = "Сохранить документ";
        }

        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            открытьtoolStripButton_Click(открытьtoolStripButton, e);
        }

        private int SaveDocument()
        {
            int result = 0;
            if (fn == string.Empty)
            {
                // отобразить диалог Сохранить
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // отобразить имя файла в заголовке окна
                    fn = saveFileDialog1.FileName;
                    this.Text = fn;
                    docChanged = false;
                }
                else result = -1;
            }
            // сохранить файл
            if (fn != string.Empty)
            {
                try
                {
                    if (saveFileDialog1.FilterIndex == 1)
                    {

                        try
                        { // Создание экземпляра StreamWriter для записи в файл:
                            var Писатель = new System.IO.StreamWriter(saveFileDialog1.FileName, false,
                            System.Text.Encoding.GetEncoding(1251));
                            // - здесь заказ кодовой страницы Winl251 для русских букв
                            Писатель.Write(richTextBox1.Text);
                            Писатель.Close();
                            richTextBox1.Modified = false;
                        }
                        catch (System.Exception Ситуация)
                        { // Отчет обо всех возможных ошибках
                            MessageBox.Show(Ситуация.Message, "Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    //
                    {
                        result = 0;
                        docChanged = false;
                        richTextBox1.SaveFile(fn, RichTextBoxStreamType.RichNoOleObjs);
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString(), "PriEdit",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return result;
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fn = string.Empty;
            saveFileDialog1.FileName = "Новый документ";
            saveFileDialog1.Title = "Сохранить документ как...";
            SaveDocument();
        }

      
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (docChanged)
            {
                DialogResult dr;
                dr = MessageBox.Show("Сохранить изменения?", "PriEdit",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (dr)
                {
                    case DialogResult.Yes:
                        if (SaveDocument() != 0)
                            // пользователь отменил операцию сохранения файла
                            e.Cancel = true; // отменить закрытие окна программы
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        // отменить закрытие окна программы
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (docChanged)
            {
                DialogResult dr;
                dr = MessageBox.Show("Сохранить изменения ?", "PriEdit",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (dr)
                {
                    case DialogResult.Yes:
                        if (SaveDocument() == 0)
                        {
                            richTextBox1.Clear();
                            docChanged = false;
                        }
                        break;
                    case DialogResult.No:
                        richTextBox1.Clear();
                        docChanged = false;
                        break;
                    case DialogResult.Cancel:
                        //
                        break;
                }
            }
            fn = string.Empty;
        }

        
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }


        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                fontDialog1.Font = richTextBox1.Font;

                if (fontDialog1.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.Font = fontDialog1.Font;
                }
            }
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length != 0)
            {
                найтиToolStripMenuItem.Enabled = true;
            }
            else
            {
                найтиToolStripMenuItem.Enabled = false;
            }
            int pos = richTextBox1.SelectionStart;    // get starting point
            int line = richTextBox1.GetLineFromCharIndex(pos);    // get line number
            int column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(line);    // get column number
            toolStripStatusLabel1.Text = "Стр. " + (line + 1) + ", Столб. " + (column + 1);
            if (richTextBox1.CanUndo == true)
            {
                отменитьToolStripMenuItem.Enabled = true;
            }
            else
            {
                отменитьToolStripMenuItem.Enabled = false;
            }
            docChanged = true;
        }

        private void печатьtoolStripButton_Click(object sender, EventArgs e)
        {
            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print(); // Print the document
            }
        }


        private void новоеОкноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 about = new Form1();
            about.Show();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }


        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
            richTextBox1.Focus();
        }


        private void menuStrip1_MouseEnter(object sender, EventArgs e)
        {
            if (richTextBox1.CanPaste(DataFormats.GetFormat(0)) == true)
            {
                вставитьToolStripMenuItem.Enabled = true;
            }
            if (richTextBox1.Text.Length != 0)
            {
                if (richTextBox1.SelectedText.Length != 0)
                {
                    удалитьToolStripMenuItem.Enabled = true;
                    копироватьToolStripMenuItem.Enabled = true;

                }
            }
        }


        private void найтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 f4 = null;
            int i = 0;
            foreach (Form a in Application.OpenForms)
            {

                if (a.Name == "FIND")
                {
                    a.Focus();
                    i++;
                }
                if (a.Name == "REPLACE")
                {
                    i--;
                    f4 = (Form4)a;
                }

            }
            if (i == 0)
            {
                Form3 f3 = new Form3(this);
                f3.Owner = this;
                f3.Show();
            }
            if (i == -1)
            {
                f4.Close();
                Form3 f3 = new Form3(this);
                f3.Owner = this;
                f3.Show();
            }
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            pos = richTextBox1.SelectionStart;
            line = richTextBox1.GetLineFromCharIndex(pos);
            column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(line);
            toolStripStatusLabel1.Text = "Стр. " + (line + 1) + ", Столб. " + (column + 1);
        }

        private void leftAlignStripButton1_Click(object sender, EventArgs e)
        {
            centerAlignStripButton.Checked = false;
            rightAlignStripButton.Checked = false;
            if (leftAlignStripButton.Checked == false)
            {
                leftAlignStripButton.Checked = true;    // LEFT ALIGN is active
            }
            else if (leftAlignStripButton.Checked == true)
            {
                leftAlignStripButton.Checked = false;    // LEFT ALIGN is not active
            }
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;    // selects left alignment
        }


        private void centerAlignStripButton2_Click(object sender, EventArgs e)
        {
            leftAlignStripButton.Checked = false;
            rightAlignStripButton.Checked = false;
            if (centerAlignStripButton.Checked == false)
            {
                centerAlignStripButton.Checked = true;    // CENTER ALIGN is active
            }
            else if (centerAlignStripButton.Checked == true)
            {
                centerAlignStripButton.Checked = false;    // CENTER ALIGN is not active
            }
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void rightAlignStripButton3_Click(object sender, EventArgs e)
        {
            leftAlignStripButton.Checked = false;
            centerAlignStripButton.Checked = false;

            if (rightAlignStripButton.Checked == false)
            {
                rightAlignStripButton.Checked = true;
            }
            else if (rightAlignStripButton.Checked == true)
            {
                rightAlignStripButton.Checked = false;    // RIGHT ALIGN is not active
            }
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void fontStripComboBox_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionFont == null)
            {
                // sets the Font Family style
                richTextBox1.SelectionFont = new Font(fontStripComboBox.Text, richTextBox1.Font.Size);
            }
            // sets the selected font famly style
            richTextBox1.SelectionFont = new Font(fontStripComboBox.Text, richTextBox1.SelectionFont.Size);
            richTextBox1.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            defaultfont = richTextBox1.Font;
            defaultsize = richTextBox1.Size;
            bmp = new Bitmap(5, 5);
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                //gfx.FillRectangle(Brushes.Blue, 0, 0, 1, 1);
                gfx.FillEllipse(Brushes.Blue, 1, 1, 3, 3);
            }
            RefreshFormat();
            foreach (System.Reflection.PropertyInfo prop in typeof(Color).GetProperties()) //fill color list

            {
                if (prop.PropertyType.FullName == "System.Drawing.Color")
                {
                    colorList.Add(prop.Name);
                }
            }

            // fill the drop down items list
            foreach (string color in colorList)
            {
                colorStripDropDownButton.Items.Add(color);
            }
            // fill font sizes in combo box
            for (int i = 8; i < 80; i += 2)
            {
                fontSizeComboBox.Items.Add(i);
            }
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily family in fonts.Families)
            {
                fontStripComboBox.Items.Add(family.Name);
            }
            int pos = richTextBox1.SelectionStart;
            int line = richTextBox1.GetLineFromCharIndex(pos);
            int column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(line);
            toolStripStatusLabel1.Text = "Стр. " + (line + 1) + ", Столб. " + (column + 1);
        }

        private void colorStripDropDownButton_Index(object sender, ToolStripItemClickedEventArgs e)
        {
            KnownColor selectedColor;
            selectedColor = (KnownColor)System.Enum.Parse(typeof(KnownColor), e.ClickedItem.Text);    // converts it to a Color Structure
            richTextBox1.SelectionColor = Color.FromKnownColor(selectedColor);    // sets the selected color
        }

        private void fontSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionFont == null)
            {
                return;
            }
            // sets the font size when changed
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, Convert.ToInt32(fontSizeComboBox.Text), richTextBox1.SelectionFont.Style);
            richTextBox1.Focus();
        }

        private void переносПоСловамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.WordWrap == false)
            {
                richTextBox1.WordWrap = true;    // WordWrap is active

            }
            else if (richTextBox1.WordWrap == true)
            {
                richTextBox1.WordWrap = false;    // WordWrap is not active
            }
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, richTextBox1.Font, Brushes.Black, 100, 20);
            e.Graphics.PageUnit = GraphicsUnit.Inch;
        }

        private void создатьtoolStripButton_Click(object sender, EventArgs e)
        {
            if (docChanged)
            {
                DialogResult dr;
                dr = MessageBox.Show("Сохранить изменения ?", "PriEdit",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (dr)
                {
                    case DialogResult.Yes:
                        if (SaveDocument() == 0)
                        {
                            richTextBox1.Clear();
                            docChanged = false;
                        }
                        break;
                    case DialogResult.No:
                        richTextBox1.Clear();
                        docChanged = false;
                        break;
                    case DialogResult.Cancel:
                        //
                        break;
                }

            }
            fn = string.Empty;
        }

        private void открытьtoolStripButton_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "MS Word dosuments (*.docx)|*.docx|Rich text format (*.rtf)|*.rtf|Text documents (*.txt)|*.txt|MS WORD DOC(*.doc)|*.doc";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fn = openFileDialog1.FileName;
                Text = "PriEdit - " + fn;
                if (openFileDialog1.FilterIndex == 1)//если формат документа Word 2007
                {
                    Word.Application app = new Microsoft.Office.Interop.Word.Application();//процесс ворда
                    Object docxFileName = openFileDialog1.FileName;//имя файла
                    Object missing = Type.Missing;
                    //открыли дркумент
                    app.Documents.Open(ref docxFileName, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing);
                    //путь к папке с временными файлами
                    string temp = System.IO.Path.GetTempPath();
                    //для передачи параметров при пересохранении
                    Object lookComments = false;
                    Object password = String.Empty;
                    Object AddToRecentFiles = true;
                    Object WritePassword = String.Empty;
                    Object ReadOnlyRecommended = false;
                    Object EmbedTrueTypeFonts = false;
                    Object SaveFormsData = false;
                    Object SaveAsAOCELetter = false;
                    //имя файла без расширения
                    Object rtfFileName = openFileDialog1.SafeFileName.Substring(0, openFileDialog1.SafeFileName.Length - ".docx".Length);
                    //создали рандом
                    Random random = new Random();
                    //проверяем есть ли файл с таким именем
                    while (System.IO.File.Exists(rtfFileName + ".rtf"))
                        //генерируем случайное имя файла
                        rtfFileName += random.Next(0, 9).ToString();
                    //формат RTF
                    Object wdFormatRTF = Word.WdSaveFormat.wdFormatRTF;
                    //приписали расширение
                    rtfFileName += ".rtf";
                    //приписали путь к временным файлам
                    rtfFileName = temp + rtfFileName;
                    //пересохранили
                    app.ActiveDocument.SaveAs(ref rtfFileName,
                        ref wdFormatRTF, ref lookComments, ref password, ref AddToRecentFiles, ref WritePassword, ref ReadOnlyRecommended,
                        ref EmbedTrueTypeFonts, ref missing, ref SaveFormsData, ref SaveAsAOCELetter, ref missing,
                        ref missing, ref missing, ref missing, ref missing);
                    //переменная
                    Object @false = false;
                    //закрыли текущий документ
                    app.ActiveDocument.Close(ref @false, ref missing, ref missing);
                    //вышли из ворда
                    app.Quit(ref @false, ref missing, ref missing);
                    //прочли файл
                    richTextBox1.LoadFile((String)rtfFileName);
                }
                if (openFileDialog1.FilterIndex == 2)
                {
                    richTextBox1.LoadFile(openFileDialog1.FileName);
                }

                if (openFileDialog1.FilterIndex == 4)
                {
                    Word.Application app = new Microsoft.Office.Interop.Word.Application();//процесс ворда
                    Object docxFileName = openFileDialog1.FileName;//имя файла
                    Object missing = Type.Missing;
                    //открыли дркумент
                    app.Documents.Open(ref docxFileName, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, true, ref missing,
                        ref missing, ref missing);
                    //путь к папке с временными файлами
                    string temp = System.IO.Path.GetTempPath();
                    //для передачи параметров при пересохранении
                    Object lookComments = false;
                    Object password = String.Empty;
                    Object AddToRecentFiles = true;
                    Object WritePassword = String.Empty;
                    Object ReadOnlyRecommended = false;
                    Object EmbedTrueTypeFonts = false;
                    Object SaveFormsData = false;
                    Object SaveAsAOCELetter = false;
                    //имя файла без расширения
                    Object rtfFileName = openFileDialog1.SafeFileName.Substring(0, openFileDialog1.SafeFileName.Length - ".doc".Length);
                    //создали рандом
                    Random random = new Random();
                    //проверяем есть ли файл с таким именем
                    while (System.IO.File.Exists(rtfFileName + ".rtf"))
                        //генерируем случайное имя файла
                        rtfFileName += random.Next(0, 9).ToString();
                    //формат RTF
                    Object wdFormatRTF = Word.WdSaveFormat.wdFormatRTF;
                    //приписали расширение
                    rtfFileName += ".rtf";
                    //приписали путь к временным файлам
                    rtfFileName = temp + rtfFileName;
                    //пересохранили
                    app.ActiveDocument.SaveAs(ref rtfFileName,
                        ref wdFormatRTF, ref lookComments, ref password, ref AddToRecentFiles, ref WritePassword, ref ReadOnlyRecommended,
                        ref EmbedTrueTypeFonts, ref missing, ref SaveFormsData, ref SaveAsAOCELetter, ref missing,
                        ref missing, ref missing, ref missing, ref missing);
                    //переменная
                    Object @false = false;
                    //закрыли текущий документ
                    app.ActiveDocument.Close(ref @false, ref missing, ref missing);
                    //вышли из ворда
                    app.Quit(ref @false, ref missing, ref missing);
                    //прочли файл
                    richTextBox1.LoadFile((String)rtfFileName);
                }
                if (openFileDialog1.FilterIndex == 3)
                {
                    fn = openFileDialog1.FileName;
                    // отобразить имя файла в заголовке окна
                    this.Text = fn;
                    try
                    {
                        // считываем данные из файла
                        System.IO.StreamReader sr = new System.IO.StreamReader(fn);
                        richTextBox1.Text = sr.ReadToEnd();
                        richTextBox1.SelectionStart = richTextBox1.TextLength;
                        sr.Close();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Ошибка чтения файла.\n" + exc.ToString(), "MEdit",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                RefreshFormat();
            }
        }

        private void сохранитьtoolStripButton_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private void colorStripDropDownButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            KnownColor selectedColor;
            selectedColor = (KnownColor)System.Enum.Parse(typeof(KnownColor), colorStripDropDownButton.Text);    // converts it to a Color Structure
            richTextBox1.SelectionColor = Color.FromKnownColor(selectedColor);    // sets the selected color
            richTextBox1.Focus();
        }

        private void toolStripButton2_Click_2(object sender, EventArgs e)
        {
            colorDialog1 = new ColorDialog();
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = colorDialog1.Color;
                //textBox1.ForeColor = colorDlg.Color;
                //listBox1.ForeColor = colorDlg.Color;
                //button3.ForeColor = colorDlg.Color;
            }
        }

        private void перейтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int line = richTextBox1.GetLineFromCharIndex(richTextBox1.MaxLength);    // get lines
            Form2 forme = new Form2(line + 1);
            forme.Owner = this;
            forme.ShowDialog();
        }

        public static Form3 CreateForm()
        {
            // Проверяем существование формы
            foreach (Form frm in Application.OpenForms)
                if (frm is Form3)
                {
                    frm.Activate();
                    return frm as Form3;
                }
            // Создаем новую форму
            Form3 form = new Form3();
            //form.MdiParent = MDIParent;
            form.Show();
            return form;
        }

        private void boldStripButton_Click(object sender, EventArgs e)
        {
            if (boldStripButton.Checked == false)
            {
                boldStripButton.Checked = true; // BOLD is true
            }
            else if (boldStripButton.Checked == true)
            {
                boldStripButton.Checked = false;    // BOLD is false
            }

            if (richTextBox1.SelectionFont == null)
            {
                return;
            }

            // create fontStyle object
            FontStyle style = richTextBox1.SelectionFont.Style;

            // determines the font style
            if (richTextBox1.SelectionFont.Bold)
            {
                style &= ~FontStyle.Bold;
            }
            else
            {
                style |= FontStyle.Bold;

            }
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, style);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (toolStripButton3.Checked == false)
            {
                toolStripButton3.Checked = true;    //active
            }
            else if (toolStripButton3.Checked == true)
            {
                toolStripButton3.Checked = false;    //not active
            }

            if (richTextBox1.SelectionFont == null)
            {
                return;
            }
            // create fontStyle object
            FontStyle style = richTextBox1.SelectionFont.Style;

            // determines font style
            if (richTextBox1.SelectionFont.Italic)
            {
                style &= ~FontStyle.Italic;
            }
            else
            {
                style |= FontStyle.Italic;
            }
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, style);    // sets the font style
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (toolStripButton4.Checked == false)
            {
                toolStripButton4.Checked = true;     //active
            }
            else if (toolStripButton4.Checked == true)
            {
                toolStripButton4.Checked = false;    //not active
            }

            if (richTextBox1.SelectionFont == null)
            {
                return;
            }

            // create fontStyle object
            FontStyle style = richTextBox1.SelectionFont.Style;

            // determines the font style
            if (richTextBox1.SelectionFont.Underline)
            {
                style &= ~FontStyle.Underline;
            }
            else
            {
                style |= FontStyle.Underline;
            }
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, style);    //font style
        }

        public void RefreshFormat()
        {
            if (richTextBox1.SelectionFont != null)
            {
                if (richTextBox1.SelectionFont.Size == 13)
                {
                    fontSizeComboBox.Text = "";
                }
                else
                { fontSizeComboBox.Text = richTextBox1.SelectionFont.Size.ToString(); }
                fontStripComboBox.Text = richTextBox1.SelectionFont.Name;
                boldStripButton.Checked = richTextBox1.SelectionFont.Bold;
                toolStripButton3.Checked = richTextBox1.SelectionFont.Italic;
                toolStripButton4.Checked = richTextBox1.SelectionFont.Underline;
                if (richTextBox1.SelectionAlignment == HorizontalAlignment.Left)
                {
                    leftAlignStripButton.Checked = true;
                    centerAlignStripButton.Checked = false;
                    rightAlignStripButton.Checked = false;
                }
                else
                {
                    if (richTextBox1.SelectionAlignment == HorizontalAlignment.Center)
                    {
                        leftAlignStripButton.Checked = false;
                        rightAlignStripButton.Checked = false;
                        centerAlignStripButton.Checked = true;
                    }
                    else
                    {
                        rightAlignStripButton.Checked = true;
                        centerAlignStripButton.Checked = false;
                        leftAlignStripButton.Checked = false;
                    }
                }
            }
            if (richTextBox1.SelectionColor != null && richTextBox1.SelectionColor.Name != "0")
            {
                colorStripDropDownButton.Text = richTextBox1.SelectionColor.Name;
            }
            else
            {
                colorStripDropDownButton.Text = "";
            }

        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            RefreshFormat();
        }

        private void заменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = 0;
            Form3 f3 = new Form3();
            foreach (Form a in Application.OpenForms)
            {
                if (a.Name == "REPLACE")
                {
                    a.Focus();
                    i++;

                }
                if (a.Name == "FIND")
                {
                    i--;
                    f3 = (Form3)a;
                    //needhelp
                }

            }
            if (i == 0)
            {
                Form4 f4 = new Form4(this);
                f4.Owner = this;
                f4.Show();
            }
            if (i == -1)
            {
                f3.Close();
                Form4 f4 = new Form4(this);
                f4.Owner = this;
                f4.Show();
            }
        }



        public int FindMyText(string text, int start, bool route)
        {
            //

            //

            int returnValue = -1;
            if (text.Length > 0 && richTextBox1.SelectionStart >= 0)
            {
                if (route == true && (start + richTextBox1.SelectedText.Length != richTextBox1.Text.Length))
                {
                    int indexToText = richTextBox1.Find(text, richTextBox1.SelectionStart + richTextBox1.SelectedText.Length, RichTextBoxFinds.None);
                    return indexToText;
                }
                if (route == false && (start != 0))
                {
                    int indexToText = richTextBox1.Find(text, 0, richTextBox1.SelectionStart, RichTextBoxFinds.Reverse);
                    return indexToText;
                }
            }
            return returnValue;
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F3" && e.Shift)
            {
                Form3 jk;
                int i = 0;
                foreach (Form a in Application.OpenForms) //search open form3
                {

                    if (a.Name == "FIND")
                    {
                        i = 1;
                        jk = (Form3)a;
                        int findex = FindMyText(jk.textBox1.Text, richTextBox1.SelectionStart, false);
                        if (findex == -1) //error
                        {
                            DialogResult error;
                            error = MessageBox.Show("Не удается найти '" + jk.textBox1.Text + "'", "Найти",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            switch (error)
                            {
                                case DialogResult.OK:
                                    break;
                            }
                        }
                    }

                }

                if (i == 0 && FindTextStr == "")
                {
                    найтиToolStripMenuItem_Click(найтиToolStripMenuItem, e);
                    //Form3 f3 = new Form3(this);
                    //f3.Owner = this;
                    //f3.Show();
                }
                if (i == 0 && FindTextStr != "")
                {
                    int findex = FindMyText(FindTextStr, richTextBox1.SelectionStart, false);
                    if (findex == -1) //error
                    {
                        DialogResult error;
                        error = MessageBox.Show("Не удается найти '" + FindTextStr + "'", "Найти",
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

                    Form3 jk;
                    int i = 0;
                    foreach (Form a in Application.OpenForms)
                    {

                        if (a.Name == "FIND")
                        {
                            i = 1;

                            jk = (Form3)a;
                            if (jk.textBox1.Text != "")
                            {
                                int findex = FindMyText(jk.textBox1.Text, richTextBox1.SelectionStart, true);
                                if (findex == -1) //error
                                {
                                    DialogResult error;
                                    error = MessageBox.Show("Не удается найти '" + jk.textBox1.Text + "'", "Найти",
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

                    if (i == 0 && FindTextStr == "")
                    {
                        найтиToolStripMenuItem_Click(найтиToolStripMenuItem, e);
                        //Form3 f3 = new Form3(this);
                        //f3.Owner = this;
                        //f3.Show();
                    }
                    if (i == 0 && FindTextStr != "")
                    {
                        int findex = FindMyText(FindTextStr, richTextBox1.SelectionStart, true);
                        if (findex == -1) //error
                        {
                            DialogResult error;
                            error = MessageBox.Show("Не удается найти '" + FindTextStr + "'", "Найти",
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
            switch (e.KeyCode)
            {
                case Keys.Down:
                    pos = richTextBox1.SelectionStart;    // get starting point
                    line = richTextBox1.GetLineFromCharIndex(pos);    // get line number
                    column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(line);    // get column number
                    toolStripStatusLabel1.Text = "Стр. " + (line + 1) + ", Столб. " + (column + 1);
                    break;
                case Keys.Right:
                    pos = richTextBox1.SelectionStart; // get starting point
                    line = richTextBox1.GetLineFromCharIndex(pos); // get line number
                    column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(line);    // get column number
                    toolStripStatusLabel1.Text = "Стр. " + (line + 1) + ", Столб. " + (column + 1);
                    break;
                case Keys.Up:
                    pos = richTextBox1.SelectionStart; // get starting point
                    line = richTextBox1.GetLineFromCharIndex(pos); // get line number
                    column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(line);    // get column number
                    toolStripStatusLabel1.Text = "Стр. " + (line + 1) + ", Столб. " + (column + 1);
                    break;
                case Keys.Left:
                    pos = richTextBox1.SelectionStart; // get starting point
                    line = richTextBox1.GetLineFromCharIndex(pos); // get line number
                    column = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(line);    // get column number
                    toolStripStatusLabel1.Text = "Стр. " + (line + 1) + ", Столб. " + (column + 1);
                    break;
            }
        }
    }
};

