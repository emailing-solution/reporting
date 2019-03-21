using System;
using System.Windows.Forms;

namespace reporting
{
    public partial class Menu : Form
    {
         public Menu()
         {
            InitializeComponent();
            
         }

        private void emailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save_emails s = new save_emails();
            s.Show();
        }

        private void reportingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report r = new Report();
            r.Show();
        }
    }
}
