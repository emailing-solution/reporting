using System;
using System.IO;
using System.Windows.Forms;

namespace reporting
{
    public partial class save_emails : Form
    {
        public save_emails()
        {
            InitializeComponent();
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            if (File.Exists("data.txt"))
            {
                txt_emails.Text = File.ReadAllText("data.txt");
            }
            else
            {
                MessageBox.Show("could not find data.txt maybe a quick save could fix it :)", "Error", MessageBoxButtons.OK);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Save current ? all old data will be erased", "confirmation !", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.WriteAllText("data.txt", txt_emails.Text.Trim());
                MessageBox.Show("Saved", "Emails settings", MessageBoxButtons.OK);
                txt_emails.Text = string.Empty;
                 
            }
        }

        private void save_emails_FormClosing(object sender, FormClosingEventArgs e)
        {

            Report._form.UpdateEmails();
        }
    }
}
