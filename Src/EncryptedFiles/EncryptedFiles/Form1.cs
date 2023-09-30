using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using OpenWithSingleInstance;
using System.Runtime.InteropServices;

namespace EncryptedFiles
{
    public partial class Form1 : Form
    {
        private static string openFilePath = "", fileTextSaved = "";
        private static bool justSaved = true, justSavedbefore = true, onopenwith = false;
        private static DialogResult result;
        private static ContextMenu contextMenu = new ContextMenu();
        private static MenuItem menuItem;
        private static string wordsearch;
        private static string wordreplace;
        private static int pos = -1;
        private static string filename = "", password = "", indicator = "";
        public Form1(string filePath)
        {
            InitializeComponent();
            fastColoredTextBox1.Font = new Font("Arial", 11);
            if (filePath != null)
            {
                onopenwith = true;
                OpenFileWith(filePath);
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == MessageHelper.WM_COPYDATA)
            {
                COPYDATASTRUCT _dataStruct = Marshal.PtrToStructure<COPYDATASTRUCT>(m.LParam);
                string _strMsg = Marshal.PtrToStringUni(_dataStruct.lpData, _dataStruct.cbData / 2);
                OpenFileWith(_strMsg);
            }
            base.WndProc(ref m);
        }
        public void OpenFileWith(string filePath)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Open", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    fastColoredTextBox1.Clear();
                    OpenIndicator(Path.GetDirectoryName(filePath) + "/indicator/" + Path.GetFileName(filePath) + ".indicator");
                    password = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Enter your password", indicator, 0, 0);
                    string txt = File.ReadAllText(filePath, Encoding.UTF8);
                    fastColoredTextBox1.Text = Decrypt(txt);
                    filename = filePath;
                    openFilePath = filePath;
                    this.Text = filePath;
                    fileTextSaved = fastColoredTextBox1.Text;
                    justSaved = true;
                }
            }
            else
            {
                fastColoredTextBox1.Clear();
                OpenIndicator(Path.GetDirectoryName(filePath) + "/indicator/" + Path.GetFileName(filePath) + ".indicator");
                password = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Enter your password", indicator, 0, 0);
                string txt = File.ReadAllText(filePath, Encoding.UTF8);
                fastColoredTextBox1.Text = Decrypt(txt);
                filename = filePath;
                openFilePath = filePath;
                this.Text = filePath;
                fileTextSaved = fastColoredTextBox1.Text;
                justSaved = true;
            }
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "New", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    fastColoredTextBox1.Clear();
                    this.Text = "TextEditor";
                    openFilePath = "";
                    fileTextSaved = fastColoredTextBox1.Text;
                    justSaved = true;
                }
            }
            else
            {
                fastColoredTextBox1.Clear();
                this.Text = "TextEditor";
                openFilePath = "";
                fileTextSaved = fastColoredTextBox1.Text;
                justSaved = true;
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Open", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    OpenFileDialog op = new OpenFileDialog();
                    op.Filter = "All Files(*.*)|*.*";
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        fastColoredTextBox1.Clear();
                        OpenIndicator(Path.GetDirectoryName(op.FileName) + "/indicator/" + Path.GetFileName(op.FileName) + ".indicator");
                        password = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Enter your password", indicator, 0, 0);
                        string txt = File.ReadAllText(op.FileName, Encoding.UTF8);
                        fastColoredTextBox1.Text = Decrypt(txt);
                        filename = op.FileName;
                        openFilePath = op.FileName;
                        this.Text = op.FileName;
                        fileTextSaved = fastColoredTextBox1.Text;
                        justSaved = true;
                    }
                }
            }
            else
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "All Files(*.*)|*.*";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    fastColoredTextBox1.Clear();
                    OpenIndicator(Path.GetDirectoryName(op.FileName) + "/indicator/" + Path.GetFileName(op.FileName) + ".indicator");
                    password = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Enter your password", indicator, 0, 0);
                    string txt = File.ReadAllText(op.FileName, Encoding.UTF8);
                    fastColoredTextBox1.Text = Decrypt(txt);
                    filename = op.FileName;
                    openFilePath = op.FileName;
                    this.Text = op.FileName;
                    fileTextSaved = fastColoredTextBox1.Text;
                    justSaved = true;
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFilePath == null | openFilePath == "")
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                password = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Enter your password", "****", 0, 0);
                indicator = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Enter an indicator", "Name of my cat", 0, 0);
                if (!Directory.Exists(Path.GetDirectoryName(filename) + "/indicator"))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filename) + "/indicator");
                }
                SaveIndicator(Path.GetDirectoryName(filename) + "/indicator/" + Path.GetFileName(filename) + ".indicator");
                File.WriteAllText(openFilePath, Encrypt(fastColoredTextBox1.Text), Encoding.UTF8);
                fileTextSaved = fastColoredTextBox1.Text;
                justSaved = true;
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "All Files(*.*)|*.*";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                password = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Enter your password", "****", 0, 0);
                indicator = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Enter an indicator", "Name of my cat", 0, 0);
                if (!Directory.Exists(Path.GetDirectoryName(sf.FileName) + "/indicator"))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(sf.FileName) + "/indicator");
                }
                SaveIndicator(Path.GetDirectoryName(sf.FileName) + "/indicator/" + Path.GetFileName(sf.FileName) + ".indicator");
                File.WriteAllText(sf.FileName, Encrypt(fastColoredTextBox1.Text), Encoding.UTF8);
                filename = sf.FileName;
                openFilePath = sf.FileName;
                this.Text = sf.FileName;
                fileTextSaved = fastColoredTextBox1.Text;
                justSaved = true;
            }
        }
        private void SaveIndicator(string completepath)
        {
            using (StreamWriter createdfile = new StreamWriter(completepath))
            {
                createdfile.WriteLine(indicator);
                createdfile.Close();
            }
        }
        private void OpenIndicator(string completepath)
        {
            using (StreamReader file = new StreamReader(completepath))
            {
                indicator = file.ReadLine();
                file.Close();
            }
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Cut();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Copy();
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Paste();
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Undo();
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Redo();
        }
        private void fileText_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                fastColoredTextBox1.ContextMenu = contextMenu;
            }
        }
        private void changeCursor(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void cutAction(object sender, EventArgs e)
        {
            fastColoredTextBox1.Cut();
        }
        private void copyAction(object sender, EventArgs e)
        {
            if (fastColoredTextBox1.SelectedText != "")
                Clipboard.SetText(fastColoredTextBox1.SelectedText);
        }
        private void pasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                fastColoredTextBox1.SelectedText = Clipboard.GetText(TextDataFormat.Text).ToString();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!justSaved)
            {
                result = MessageBox.Show("Content will be lost! Are you sure?", "Exit", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
            if (filename != "")
            {
                using (System.IO.StreamWriter createdfile = new System.IO.StreamWriter(Application.StartupPath + @"\tempsave"))
                {
                    createdfile.WriteLine(filename);
                }
            }
        }
        private void fileText_TextChanged(object sender, EventArgs e)
        {
            if (fileTextSaved != fastColoredTextBox1.Text)
                justSaved = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            menuItem = new MenuItem("Cut");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(cutAction);
            menuItem = new MenuItem("Copy");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(copyAction);
            menuItem = new MenuItem("Paste");
            contextMenu.MenuItems.Add(menuItem);
            menuItem.Select += new EventHandler(changeCursor);
            menuItem.Click += new EventHandler(pasteAction);
            fastColoredTextBox1.ContextMenu = contextMenu;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                if (!onopenwith)
                {
                    if (File.Exists(Application.StartupPath + @"\tempsave"))
                    {
                        using (System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + @"\tempsave"))
                        {
                            filename = file.ReadLine();
                        }
                        if (filename != "")
                        {
                            fastColoredTextBox1.Clear();
                            OpenIndicator(Path.GetDirectoryName(filename) + "/indicator/" + Path.GetFileName(filename) + ".indicator");
                            password = Microsoft.VisualBasic.Interaction.InputBox("Prompt", "Enter your password", indicator, 0, 0);
                            string txt = File.ReadAllText(filename, Encoding.UTF8);
                            fastColoredTextBox1.Text = Decrypt(txt);
                            openFilePath = filename;
                            this.Text = filename;
                            fileTextSaved = fastColoredTextBox1.Text;
                            justSaved = true;
                        }
                    }
                }
            }
            catch { }
        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = password;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = password;
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}