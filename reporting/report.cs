using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace reporting
{
    public partial class report : Form
    {
        public report()
        {
            InitializeComponent();
            if(File.Exists("data.txt") && new FileInfo("data.txt").Length > 0)
            {
                txt_emails.Text = File.ReadAllText("data.txt");
            }
            else
            {
                Hide();
                MessageBox.Show("data.txt file not found or empty! please check", "error", MessageBoxButtons.OK);
                Close();
                               
            }
        }
    }
}
