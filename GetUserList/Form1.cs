using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.IO;
using System.Runtime.InteropServices;


namespace GetUserList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("shell32.dll", EntryPoint = "#261", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void GetUserTilePath(
                   string username,
               UInt32 whatever, // 0x80000000
           StringBuilder picpath, int maxLength);

        public string GetUserTilePath(string username)

        {

            var sb = new StringBuilder(1000);

            GetUserTilePath(username, 0x80000000, sb, sb.Capacity);

            return sb.ToString();

        }
        public Image GetUserTile(string username)

        {

            return Image.FromFile(GetUserTilePath(username));

        }
        private void listuser()
        {
            var path = string.Format("WinNT://{0},computer", Environment.MachineName);
            using (var userget = new DirectoryEntry(path))
            {
                var userNames = from DirectoryEntry dirchild in userget.Children
                                where dirchild.SchemaClassName == "User"
                                select dirchild.Name;
                foreach (var n in userNames)
                {
                    listBox1.Items.Add(n);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

            label1.Text = Environment.UserName.ToString();
            pictureBox1.Image = GetUserTile(label1.Text.ToString());

        }
        public static bool isadmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal princ = new WindowsPrincipal(id);
            return princ.IsInRole(WindowsBuiltInRole.Administrator);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            listuser();
            string admin = isadmin() ? " User Is Administrator " : "User Access is Normal";
            label2.Text = admin;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
