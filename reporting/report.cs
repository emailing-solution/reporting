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
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace reporting
{
    public partial class Report : Form
    {
        Random random = new Random();
        public Report()
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

        private void Btn_spam_yahoo_Click(object sender, EventArgs e)
        {
            
        }

        private void checkbox_readNotSpam_CheckedChanged(object sender, EventArgs e)
        {
            if(checkbox_readNotSpam.Checked)
            {
                check_checkNotSpam.Checked = false;
                check_ckeckallNotSpam.Checked = false;
            }
        }

        private void check_checkNotSpam_CheckedChanged(object sender, EventArgs e)
        {
            if (check_checkNotSpam.Checked)
            {
                checkbox_readNotSpam.Checked = false;
                check_ckeckallNotSpam.Checked = false;
            }
        }

        private void check_ckeckallNotSpam_CheckedChanged(object sender, EventArgs e)
        {
            if (check_ckeckallNotSpam.Checked)
            {
                check_checkNotSpam.Checked = false;
                checkbox_readNotSpam.Checked = false;
            }
        }

        private void Btn_yahoo_action_Click(object sender, EventArgs e)
        {

            data.Yahoo yahoo = new data.Yahoo("sanaezayed@yahoo.com", "aqwxsz1A");
            yahoo.Init(false);
            txt_log.AppendText("init\n");
            yahoo.Connect();
            txt_log.AppendText("connecting\n");
            if (yahoo.CheckIfConnected())
            {
                txt_log.AppendText("connected\n");
                yahoo.Navigate("https://mail.yahoo.com/d/folders/1?.intl=uk&.lang=en-GB");
                txt_log.AppendText(yahoo.GoToSpamFolder().ToString());
                yahoo.ReadNotSpam();
            }
            else
            {
                txt_log.AppendText("not connected verify\n");
            }
        }

        
    }
}
