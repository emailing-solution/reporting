using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using reporting.data;

namespace reporting
{
    public partial class Report : Form
    {
        //Déclaration du SemaphoreSlim qui prendra en paramètre le nombre de places disponibles.
        public SemaphoreSlim Doorman { get; set ; }
        public Random Random { get; set; }
        public static Report _form { get; set; }

        public Report()
        {
            InitializeComponent();
            _form = this;
            if (File.Exists("data.txt") && new FileInfo("data.txt").Length > 0)
            {
                txt_emails.Text = File.ReadAllText("data.txt");
            }
        }

        private bool Check_yahoo_spam_checked()
        {
            foreach (Control item in group_spam_yahoo.Controls)
            {
                if (item is RadioButton c && c.Checked)
                {
                    return true;
                }
            }
            return false;
        }

        private bool Check_yahoo_inbox_checked()
        {
            foreach (Control item in group_inbox_yahoo.Controls)
            {
                if (item is RadioButton c && c.Checked)
                {
                    return true;
                }
            }
            return false;
        }

        private bool Check_gmail_spam_checked()
        {
            foreach (Control item in group_spam_gmail.Controls)
            {
                if (item is RadioButton c && c.Checked)
                {
                    return true;
                }
            }
            return false;
        }

        private bool Check_gmail_inbox_checked()
        {
            foreach (Control item in group_inbox_gmail.Controls)
            {
                if (item is RadioButton c && c.Checked)
                {
                    return true;
                }
            }
            return false;
        }

        private void Btn_yahoo_action_Click(object sender, EventArgs e)
        {
            //
            if (!Check_yahoo_spam_checked() && !Check_yahoo_inbox_checked())
            {
                MessageBox.Show("Please select an action to do", "Actions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var accounts = txt_emails.Text.Split('\n');
                if (accounts.Length > 0)
                {
                    int sizeThreads = !string.IsNullOrEmpty(txt_thread.Text) ? int.Parse(txt_thread.Text.Trim()) : 1;
                    Doorman = new SemaphoreSlim(sizeThreads);

                    foreach (var account in accounts)
                    {
                        string[] accountDetail = account.Split(':');
                        //Création et lancement des threads.

                        new Thread(() => 
                        {
                            ActionYahoo(accountDetail);

                        }).Start();
                        
                        //On laisse passer 500ms entre les création de thread.
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    MessageBox.Show("Add some accounts", "Actions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }

        private void ActionYahoo(string[] accountDetail)
        {
            Doorman.Wait();
            Yahoo yahoo;
            if (accountDetail.Length == 2)
            {
                yahoo = new Yahoo(accountDetail[0], accountDetail[1]);
            }
            else if (accountDetail.Length == 4)
            {
                yahoo = new Yahoo(accountDetail[0], accountDetail[1], accountDetail[2], accountDetail[3]);
            }
            else
            {
                Doorman.Release();
                return;
            }
            bool error = yahoo.Init();
            if(!error)
            {
                MessageBox.Show("ERROR INIT", "Error");
                Doorman.Release();
                return;
            }

            if (open.Checked)
            {
                yahoo.Connect();
                yahoo.Navigate("https://mail.yahoo.com/");
                
                while(!yahoo.CheckIfClosed())
                {
                    Thread.Sleep(5000);
                }
                Doorman.Release();
                return;
            }

            yahoo.Connect();

            if (yahoo.CheckIfConnected())
            {

                yahoo.Navigate("https://mail.yahoo.com/neo/b/launch");

                //to spam action
                if (Check_yahoo_spam_checked())
                {
                    SpamYahoo(yahoo);
                }

                //to inbox action()
                if (Check_yahoo_inbox_checked())
                {
                    InboxYahoo(yahoo);
                }

                yahoo.Destroy();
            }
            Doorman.Release();

        }

        private void SpamYahoo(Yahoo yahoo)
        {
            if (yahoo.GoToSpamFolder())
            {
                //case 1 
                if (read_not_spam.Checked)
                {

                    while (yahoo.ReadNotSpam()) { Application.DoEvents(); }
                }
                //case 2
                if (check_not_spam.Checked)
                {

                    while (yahoo.CheckNotSpam()) { Application.DoEvents(); }
                }
                //case 3
                if (ckeckall_not_spam.Checked)
                {

                    while (yahoo.CheckAllNotSpam()) { Application.DoEvents(); }
                }
            }
        }

        private void InboxYahoo(Yahoo yahoo)
        {
            int size = 0;
            if (yahoo.GotoInboxFolder())
            {
                //case 1  open and archive
                if (open_archive.Checked)
                {
                    //all magic happen here
                    bool repeat_arhive(int page, bool star, bool click, string search)
                    {
                        var status = yahoo.OpenArchive(page, star, click, search);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_arhive(page, star, click, search);
                        }
                        return false;
                    }

                    while (repeat_arhive(size, star_click.Checked, body_click.Checked, txt_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }

                //case 2 open and reply and archive
                if (open_reply_archive.Checked)
                {
                    //all magic happen here
                    bool repeat_open_reply_arhive(int page, bool star, bool click, string search)
                    {
                        var status = yahoo.OpenReplyArchive(page, star, click, search);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_open_reply_arhive(page, star, click, search);
                        }
                        return false;
                    }

                    while (repeat_open_reply_arhive(size, star_click.Checked, body_click.Checked, txt_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }

                // case 3 open and reply
                if (open_reply.Checked)
                {
                    //all magic happen here
                    bool repeat_open_reply(int page, bool star, bool click, string search)
                    {
                        var status = yahoo.OpenReply(page, star, click, search);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_open_reply(page, star, click, search);
                        }
                        return false;
                    }

                    while (repeat_open_reply(size, star_click.Checked, body_click.Checked, txt_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }

                //case 4 select archive
                if (select_archive.Checked)
                {
                    //all magic happen here
                    bool repeat_select_archive(int page, bool star, string search)
                    {
                        var status = yahoo.SelectArchive(page, star, search);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_select_archive(page, star, search);
                        }
                        return false;
                    }

                    while (repeat_select_archive(size, star_click.Checked, txt_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }

                //case 5 select all archive
                if (select_all_archive.Checked)
                {
                    //all magic happen here
                    bool repeat_select_all_archive(int page, bool star)
                    {
                        var status = yahoo.SelectAllArchive(page, star);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_select_all_archive(page, star);
                        }
                        return false;
                    }

                    while (repeat_select_all_archive(size, star_click.Checked))
                    {
                        Application.DoEvents();
                    }
                }

                //case 6 random
                if(random_actions.Checked)
                {
                    //all magic happen here
                    bool repeat_random(int page, bool star, bool bodyclick, string subject)
                    {
                        var status = yahoo.RandomAction(page, star, bodyclick, subject);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_random(page, star, bodyclick, subject);
                        }
                        return false;
                    }

                    while (repeat_random(size, star_click.Checked, body_click.Checked, txt_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }
            }
            
        }

        private void body_click_CheckedChanged(object sender, EventArgs e)
        {
            if(select_archive.Checked || select_all_archive.Checked)
            {
                body_click.Checked = false;
            }

            if(open.Checked)
            {
 
                body_click.Checked = false;
            }
        }

        private void select_all_archive_CheckedChanged(object sender, EventArgs e)
        {
            body_click.Checked = false;
        }

        private void select_archive_CheckedChanged(object sender, EventArgs e)
        {
            body_click.Checked = false;
        }

        private void edit_emails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            save_emails s = new save_emails();
            s.ShowDialog();
        }

        private void open_CheckedChanged(object sender, EventArgs e)
        {
            star_click.Checked = false;
            body_click.Checked = false;
        }

        private void star_click_CheckedChanged(object sender, EventArgs e)
        {
            if (open.Checked)
            {
                star_click.Checked = false;
 
            }
        }

        private void txt_thread_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void Delete_emails(string[] accountDetail)
        {
            Doorman.Wait();
            Yahoo yahoo;
            if (accountDetail.Length == 2)
            {
                yahoo = new Yahoo(accountDetail[0], accountDetail[1]);
            }
            else if (accountDetail.Length == 4)
            {
                yahoo = new Yahoo(accountDetail[0], accountDetail[1], accountDetail[2], accountDetail[3]);
            }
            else
            {
                Doorman.Release();
                return;
            }

            yahoo.Init();
            yahoo.Connect();

            if (yahoo.CheckIfConnected())
            {
                yahoo.Navigate("https://mail.yahoo.com/neo/b/launch");

                if (checkbox_spam_yahoo.Checked)
                {
                    yahoo.DeleteSpam();
                }

                if (checkbox_inbox_yahoo.Checked)
                {
                    //all magic happen here
                    bool repeat_delete_inbox()
                    {
                        if (yahoo.DeleteInbox())
                        {
                            return yahoo.DeleteInbox();
                        }
                        return false;
                    }

                    while (repeat_delete_inbox())
                    {
                        Application.DoEvents();
                    }
                    new Logger().Info("All inbox email deleted : " + accountDetail[0]);
                }
            }

            yahoo.Destroy();
            Doorman.Release();
        }

        private void Btn_delete_yahoo_Click(object sender, EventArgs e)
        {

            if (checkbox_inbox_yahoo.Checked || checkbox_spam_yahoo.Checked)
            {
                var accounts = txt_emails.Text.Split('\n');
                if (accounts.Length > 0)
                {
                    int sizeThreads = !string.IsNullOrEmpty(txt_thread.Text) ? int.Parse(txt_thread.Text.Trim()) : 1;
                    Doorman = new SemaphoreSlim(sizeThreads);

                    foreach (var account in accounts)
                    {
                        string[] accountDetail = account.Split(':');

                        //Création et lancement des threads.
                        new Thread(() =>
                        {
                            Delete_emails(accountDetail);

                        }).Start();

                        //On laisse passer 500ms entre les création de thread.
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    MessageBox.Show("Add some accounts", "Actions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Report_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        public void UpdateEmails()
        {
            txt_emails.Text = "";
            if (File.Exists("data.txt") && new FileInfo("data.txt").Length > 0)
            {
                txt_emails.Text = File.ReadAllText("data.txt");
            }
        }

        private void Btn_gmail_action_Click(object sender, EventArgs e)
        {
            //
            if (!Check_gmail_spam_checked() && !Check_gmail_inbox_checked())
            {
                MessageBox.Show("Please select an action to do", "Actions", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var accounts = txt_emails.Text.Split('\n');
                if (accounts.Length > 0)
                {
                    int sizeThreads = !string.IsNullOrEmpty(txt_thread.Text) ? int.Parse(txt_thread.Text.Trim()) : 1;
                    Doorman = new SemaphoreSlim(sizeThreads);

                    foreach (var account in accounts)
                    {
                        string[] accountDetail = account.Split(':');
                        //Création et lancement des threads.

                        new Thread(() =>
                        {
                            ActionGmail(accountDetail);

                        }).Start();

                        //On laisse passer 500ms entre les création de thread.
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    MessageBox.Show("Add some accounts", "Actions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
        }

        private void ActionGmail(string[] accountDetail)
        {
            Doorman.Wait();
            Gmail gmail;
            if (accountDetail.Length == 2)
            {
                gmail = new Gmail(accountDetail[0], accountDetail[1]);
            }
            else if (accountDetail.Length == 4)
            {
                gmail = new Gmail(accountDetail[0], accountDetail[1], accountDetail[2], accountDetail[3]);
            }
            else
            {
                Doorman.Release();
                return;
            }
            bool init = gmail.Init();
            if (!init)
            {
                Doorman.Release();
                return;
            }
            gmail.Connect();

            if (open.Checked)
            {
                while (!gmail.CheckIfClosed())
                {
                    Thread.Sleep(5000);
                }
                Doorman.Release();
                return;
            }

            if (gmail.CheckIfConnected())
            {

                gmail.OldGmail();
                //to spam action
                if (Check_gmail_spam_checked())
                {
                    SpamGmail(gmail);
                }

                //to inbox action()
                if (Check_yahoo_inbox_checked())
                {
                    InboxGmail(gmail);
                }

                gmail.Destroy();
            }
            Doorman.Release();
        }
        
        private void SpamGmail(Gmail gmail)
        {
            if(gmail.GotoSpamFolder())
            {
                //case 1 Read ana mark not spam
                if(read_not_spam_gmail.Checked)
                {
                    while (gmail.ReadNotSpam()) { Application.DoEvents(); }
                }
                //case 2 check and mark as not spam
                else if(check_not_spam_gmail.Checked)
                {
                    while (gmail.CheckNotSpam()) { Application.DoEvents(); }
                }
                //case 3 check all and  mark not spam
                else if(checkall_not_spam_gmail.Checked)
                {
                    while (gmail.CheckAllNotSpam()) { Application.DoEvents(); }
                }
            }
        }

        private void InboxGmail(Gmail gmail)
        {
            int size = 0;
            if (gmail.GoToInboxFolder())
            {
                //case 1  open and archive;
                if(open_archive_gmail.Checked)
                {
                    //all magic happen here
                    bool repeat_arhive_gmail(int page, bool star, bool click, string search)
                    {
                        var status = gmail.OpenArchive(page, star, click, search);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_arhive_gmail(page, star, click, search);
                        }
                        return false;
                    }

                    while (repeat_arhive_gmail(size, star_click_gmail.Checked, body_click_gmail.Checked, txt_gmail_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }
                else if (open_reply_archive_gmail.Checked)
                {
                    bool repeat_open_reply_arhive_gmail(int page, bool star, bool click, string search)
                    {
                        var status = gmail.OpenReplyArchive(page, star, click, search);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_open_reply_arhive_gmail(page, star, click, search);
                        }
                        return false;
                    }

                    while (repeat_open_reply_arhive_gmail(size, star_click_gmail.Checked, body_click_gmail.Checked, txt_gmail_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }
                else if (open_reply_gmail.Checked)
                {
                    bool repeat_open_reply_gmail(int page, bool star, bool click, string search)
                    {
                        var status = gmail.OpenReply(page, star, click, search);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_open_reply_gmail(page, star, click, search);
                        }
                        return false;
                    }

                    while (repeat_open_reply_gmail(size, star_click_gmail.Checked, body_click_gmail.Checked, txt_gmail_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }
                else if (select_archive_gmail.Checked)
                {
                    bool repeat_select_archive_gmail(int page, bool star, string search)
                    {
                        var status = gmail.SelectArchive(page, star, search);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_select_archive_gmail(page, star, search);
                        }
                        return false;
                    }

                    while (repeat_select_archive_gmail(size, star_click_gmail.Checked, txt_gmail_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }
                else if (selectall_archive_gmail.Checked)
                {
                    bool repeat_selectall_archive_gmail(int page, bool star, string search)
                    {
                        var status = gmail.SelectAllArchive(page, star, search);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_selectall_archive_gmail(page, star, search);
                        }
                        return false;
                    }

                    while (repeat_selectall_archive_gmail(size, star_click_gmail.Checked, txt_gmail_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }
                else if(random_action_gmail.Checked)
                {
                    bool repeat_random_gmail(int page, bool star, bool clickbody, string search)
                    {
                        var status = gmail.RandomAction(page, star, clickbody, search);
                        if ((bool)status[0])
                        {
                            page = (int)status[1];
                            return repeat_random_gmail(page, star, clickbody, search);
                        }
                        return false;
                    }

                    while (repeat_random_gmail(size, star_click_gmail.Checked, body_click_gmail.Checked, txt_gmail_subject.Text))
                    {
                        Application.DoEvents();
                    }
                }
                else if(connect_gmail.Checked)
                {

                }
            }
        }

        private void Btn_delete_gmail_Click(object sender, EventArgs e)
        {
            if (delete_gmail_inbox.Checked || delete_gmail_spam.Checked)
            {
                var accounts = txt_emails.Text.Split('\n');
                if (accounts.Length > 0)
                {
                    int sizeThreads = !string.IsNullOrEmpty(txt_thread.Text) ? int.Parse(txt_thread.Text.Trim()) : 1;
                    Doorman = new SemaphoreSlim(sizeThreads);

                    foreach (var account in accounts)
                    {
                        string[] accountDetail = account.Split(':');

                        //Création et lancement des threads.
                        new Thread(() =>
                        {
                            Delete_emails_gmail(accountDetail);

                        }).Start();

                        //On laisse passer 500ms entre les création de thread.
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    MessageBox.Show("Add some accounts", "Actions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Delete_emails_gmail(string[] accountDetail)
        {
            Doorman.Wait();
            Gmail gmail;
            if (accountDetail.Length == 2)
            {
                gmail = new Gmail(accountDetail[0], accountDetail[1]);
            }
            else if (accountDetail.Length == 4)
            {
                gmail = new Gmail(accountDetail[0], accountDetail[1], accountDetail[2], accountDetail[3]);
            }
            else
            {
                Doorman.Release();
                return;
            }

            gmail.Init();
            gmail.Connect();
            gmail.OldGmail();

            if(delete_gmail_spam.Checked)
            {
                bool repeat_delete_spam()
                {
                    if (gmail.DeleteSpam())
                    {
                        return gmail.DeleteSpam();
                    }
                    return false;
                }

                while (repeat_delete_spam())
                {
                    Application.DoEvents();
                }
                new Logger().Info("All Spam email deleted : " + accountDetail[0]);
            }

            if(delete_gmail_inbox.Checked)
            {
                bool repeat_delete_inbox()
                {
                    if (gmail.DeleteInbox())
                    {
                        return gmail.DeleteInbox();
                    }
                    return false;
                }

                while (repeat_delete_inbox())
                {
                    Application.DoEvents();
                }
                new Logger().Info("All inbox email deleted : " + accountDetail[0]);
            }

            gmail.Destroy();
            Doorman.Release();
        }
    }
}
