using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;

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
            bool checkboxspam  = false;
            bool checkboxinbox = false;
            foreach (Control item in group_spam_yahoo.Controls)
            {
                CheckBox c = item as CheckBox;
                if(c != null && c.Checked)
                {
                    checkboxspam = true;
                    break;
                }
            }
            foreach (Control item in group_inbox_yahoo.Controls)
            {
                CheckBox c = item as CheckBox;
                if (c != null && c.Checked)
                {
                    checkboxinbox = true;
                    break;
                }
            }

            if (!checkboxspam && !checkboxinbox)
            {
                MessageBox.Show("Please select an action to do", "Actions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                data.Yahoo yahoo = new data.Yahoo("sanaezayed@yahoo.com", "aqwxsz1A");

                yahoo.Init();
                yahoo.Connect();
               
                if (yahoo.CheckIfConnected())
                {


                    yahoo.Navigate("https://mail.yahoo.com/neo/b/launch");
                    //to spam action
                    if (checkboxspam)
                    {
                        spam(yahoo);
                        if(checkboxinbox)
                        {
                            inbox(yahoo);
                        }
                    }


                    //to inbox action()
                    if(checkboxinbox)
                    {
                        inbox(yahoo);
                    }
                    //yahoo.Destroy();

                }
                else
                {
                    txt_log.AppendText("Could not connect to this account => ");
                }
            }
        }
        private void spam(data.Yahoo yahoo)
        {
            

            if (yahoo.GoToSpamFolder())
            {
                //case 1 
                if (checkbox_readNotSpam.Checked)
                {

                    while (yahoo.ReadNotSpam()) { }
                }
                //case 2
                else if (check_checkNotSpam.Checked)
                {

                    while (yahoo.CheckNotSpam()) { }
                }
                //case 3
                else if (check_ckeckallNotSpam.Checked)
                {

                    while (yahoo.CheckAllNotSpam()) { }
                }
            }
            else
            {
                MessageBox.Show("Cant Find Spam Folder", "SPAM", MessageBoxButtons.OK);
            }
        }

        private void inbox(data.Yahoo yahoo)
        {
            bool repeat (int s)
            {
                var status = yahoo.OpenArchive(s);
                if ((bool)status[0])
                {
                    s = (int)status[1];
                    return repeat(s);
                }
                return false;
            }

            if(yahoo.GotoInboxFolder())
            {
                
                if(open_archive.Checked)
                {
                    int size = 0;
                    while (repeat(size))
                    {
                        Application.DoEvents();
                    }
                     
                }
            }
            else
            {
                MessageBox.Show("Cant Find Inbox Folder", "INBOX", MessageBoxButtons.OK);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
