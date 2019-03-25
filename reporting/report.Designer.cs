namespace reporting
{
    partial class Report
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report));
            this.txt_emails = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.edit_emails = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.DELETE = new System.Windows.Forms.GroupBox();
            this.checkbox_inbox_yahoo = new System.Windows.Forms.CheckBox();
            this.checkbox_spam_yahoo = new System.Windows.Forms.CheckBox();
            this.Btn_delete_yahoo = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txt_subject = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_yahoo_action = new System.Windows.Forms.Button();
            this.txt_thread = new System.Windows.Forms.TextBox();
            this.group_inbox_yahoo = new System.Windows.Forms.GroupBox();
            this.star_click = new System.Windows.Forms.CheckBox();
            this.body_click = new System.Windows.Forms.CheckBox();
            this.select_all_archive = new System.Windows.Forms.RadioButton();
            this.open = new System.Windows.Forms.RadioButton();
            this.select_archive = new System.Windows.Forms.RadioButton();
            this.random_actions = new System.Windows.Forms.RadioButton();
            this.open_reply = new System.Windows.Forms.RadioButton();
            this.open_reply_archive = new System.Windows.Forms.RadioButton();
            this.open_archive = new System.Windows.Forms.RadioButton();
            this.group_spam_yahoo = new System.Windows.Forms.GroupBox();
            this.ckeckall_not_spam = new System.Windows.Forms.RadioButton();
            this.check_not_spam = new System.Windows.Forms.RadioButton();
            this.read_not_spam = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.DELETE.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.group_inbox_yahoo.SuspendLayout();
            this.group_spam_yahoo.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_emails
            // 
            this.txt_emails.Location = new System.Drawing.Point(6, 17);
            this.txt_emails.Multiline = true;
            this.txt_emails.Name = "txt_emails";
            this.txt_emails.ReadOnly = true;
            this.txt_emails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_emails.Size = new System.Drawing.Size(853, 216);
            this.txt_emails.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.edit_emails);
            this.groupBox5.Controls.Add(this.txt_emails);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(865, 254);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Emails";
            // 
            // edit_emails
            // 
            this.edit_emails.AutoSize = true;
            this.edit_emails.LinkColor = System.Drawing.Color.Black;
            this.edit_emails.Location = new System.Drawing.Point(6, 235);
            this.edit_emails.Name = "edit_emails";
            this.edit_emails.Size = new System.Drawing.Size(58, 13);
            this.edit_emails.TabIndex = 1;
            this.edit_emails.TabStop = true;
            this.edit_emails.Text = "Edit Emails";
            this.edit_emails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.edit_emails_LinkClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(14, 272);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(867, 139);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.DELETE);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Controls.Add(this.group_inbox_yahoo);
            this.tabPage2.Controls.Add(this.group_spam_yahoo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(859, 113);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "YAHOO ACTION";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // DELETE
            // 
            this.DELETE.Controls.Add(this.checkbox_inbox_yahoo);
            this.DELETE.Controls.Add(this.checkbox_spam_yahoo);
            this.DELETE.Controls.Add(this.Btn_delete_yahoo);
            this.DELETE.Location = new System.Drawing.Point(560, 6);
            this.DELETE.Name = "DELETE";
            this.DELETE.Size = new System.Drawing.Size(93, 100);
            this.DELETE.TabIndex = 0;
            this.DELETE.TabStop = false;
            this.DELETE.Text = "DELETE";
            // 
            // checkbox_inbox_yahoo
            // 
            this.checkbox_inbox_yahoo.AutoSize = true;
            this.checkbox_inbox_yahoo.Location = new System.Drawing.Point(17, 25);
            this.checkbox_inbox_yahoo.Name = "checkbox_inbox_yahoo";
            this.checkbox_inbox_yahoo.Size = new System.Drawing.Size(59, 17);
            this.checkbox_inbox_yahoo.TabIndex = 0;
            this.checkbox_inbox_yahoo.Text = "INBOX";
            this.checkbox_inbox_yahoo.UseVisualStyleBackColor = true;
            // 
            // checkbox_spam_yahoo
            // 
            this.checkbox_spam_yahoo.AutoSize = true;
            this.checkbox_spam_yahoo.Location = new System.Drawing.Point(17, 48);
            this.checkbox_spam_yahoo.Name = "checkbox_spam_yahoo";
            this.checkbox_spam_yahoo.Size = new System.Drawing.Size(56, 17);
            this.checkbox_spam_yahoo.TabIndex = 1;
            this.checkbox_spam_yahoo.Text = "SPAM";
            this.checkbox_spam_yahoo.UseVisualStyleBackColor = true;
            // 
            // Btn_delete_yahoo
            // 
            this.Btn_delete_yahoo.ForeColor = System.Drawing.SystemColors.Desktop;
            this.Btn_delete_yahoo.Location = new System.Drawing.Point(6, 70);
            this.Btn_delete_yahoo.Name = "Btn_delete_yahoo";
            this.Btn_delete_yahoo.Size = new System.Drawing.Size(81, 23);
            this.Btn_delete_yahoo.TabIndex = 2;
            this.Btn_delete_yahoo.Text = "DELETE";
            this.Btn_delete_yahoo.UseVisualStyleBackColor = true;
            this.Btn_delete_yahoo.Click += new System.EventHandler(this.Btn_delete_yahoo_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txt_subject);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.Btn_yahoo_action);
            this.groupBox7.Controls.Add(this.txt_thread);
            this.groupBox7.Location = new System.Drawing.Point(659, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(194, 100);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "ACTIONS";
            // 
            // txt_subject
            // 
            this.txt_subject.Location = new System.Drawing.Point(73, 20);
            this.txt_subject.Name = "txt_subject";
            this.txt_subject.Size = new System.Drawing.Size(108, 20);
            this.txt_subject.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "SUBJECT :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Threads :";
            // 
            // Btn_yahoo_action
            // 
            this.Btn_yahoo_action.Location = new System.Drawing.Point(73, 72);
            this.Btn_yahoo_action.Name = "Btn_yahoo_action";
            this.Btn_yahoo_action.Size = new System.Drawing.Size(108, 21);
            this.Btn_yahoo_action.TabIndex = 2;
            this.Btn_yahoo_action.Text = "GO";
            this.Btn_yahoo_action.UseVisualStyleBackColor = true;
            this.Btn_yahoo_action.Click += new System.EventHandler(this.Btn_yahoo_action_Click);
            // 
            // txt_thread
            // 
            this.txt_thread.Location = new System.Drawing.Point(73, 46);
            this.txt_thread.Name = "txt_thread";
            this.txt_thread.Size = new System.Drawing.Size(108, 20);
            this.txt_thread.TabIndex = 1;
            this.txt_thread.Text = "1";
            this.txt_thread.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_thread_KeyPress);
            // 
            // group_inbox_yahoo
            // 
            this.group_inbox_yahoo.Controls.Add(this.star_click);
            this.group_inbox_yahoo.Controls.Add(this.body_click);
            this.group_inbox_yahoo.Controls.Add(this.select_all_archive);
            this.group_inbox_yahoo.Controls.Add(this.open);
            this.group_inbox_yahoo.Controls.Add(this.select_archive);
            this.group_inbox_yahoo.Controls.Add(this.random_actions);
            this.group_inbox_yahoo.Controls.Add(this.open_reply);
            this.group_inbox_yahoo.Controls.Add(this.open_reply_archive);
            this.group_inbox_yahoo.Controls.Add(this.open_archive);
            this.group_inbox_yahoo.Location = new System.Drawing.Point(165, 6);
            this.group_inbox_yahoo.Name = "group_inbox_yahoo";
            this.group_inbox_yahoo.Size = new System.Drawing.Size(389, 100);
            this.group_inbox_yahoo.TabIndex = 0;
            this.group_inbox_yahoo.TabStop = false;
            this.group_inbox_yahoo.Text = "INBOX ACTION";
            // 
            // star_click
            // 
            this.star_click.AutoSize = true;
            this.star_click.Location = new System.Drawing.Point(273, 51);
            this.star_click.Name = "star_click";
            this.star_click.Size = new System.Drawing.Size(55, 17);
            this.star_click.TabIndex = 7;
            this.star_click.Text = "STAR";
            this.star_click.UseVisualStyleBackColor = true;
            this.star_click.CheckedChanged += new System.EventHandler(this.star_click_CheckedChanged);
            // 
            // body_click
            // 
            this.body_click.AutoSize = true;
            this.body_click.Location = new System.Drawing.Point(273, 70);
            this.body_click.Name = "body_click";
            this.body_click.Size = new System.Drawing.Size(116, 17);
            this.body_click.TabIndex = 8;
            this.body_click.Text = "CLICK BODY LINK";
            this.body_click.UseVisualStyleBackColor = true;
            this.body_click.CheckedChanged += new System.EventHandler(this.body_click_CheckedChanged);
            // 
            // select_all_archive
            // 
            this.select_all_archive.AutoSize = true;
            this.select_all_archive.Location = new System.Drawing.Point(144, 48);
            this.select_all_archive.Name = "select_all_archive";
            this.select_all_archive.Size = new System.Drawing.Size(107, 17);
            this.select_all_archive.TabIndex = 4;
            this.select_all_archive.Text = "SelectAll/Archive";
            this.select_all_archive.UseVisualStyleBackColor = true;
            this.select_all_archive.CheckedChanged += new System.EventHandler(this.select_all_archive_CheckedChanged);
            // 
            // open
            // 
            this.open.AutoSize = true;
            this.open.Location = new System.Drawing.Point(273, 28);
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(65, 17);
            this.open.TabIndex = 6;
            this.open.Text = "Connect";
            this.open.UseVisualStyleBackColor = true;
            this.open.CheckedChanged += new System.EventHandler(this.open_CheckedChanged);
            // 
            // select_archive
            // 
            this.select_archive.AutoSize = true;
            this.select_archive.Location = new System.Drawing.Point(144, 26);
            this.select_archive.Name = "select_archive";
            this.select_archive.Size = new System.Drawing.Size(96, 17);
            this.select_archive.TabIndex = 3;
            this.select_archive.Text = "Select/Archive";
            this.select_archive.UseVisualStyleBackColor = true;
            this.select_archive.CheckedChanged += new System.EventHandler(this.select_archive_CheckedChanged);
            // 
            // random_actions
            // 
            this.random_actions.AutoSize = true;
            this.random_actions.Location = new System.Drawing.Point(144, 70);
            this.random_actions.Name = "random_actions";
            this.random_actions.Size = new System.Drawing.Size(103, 17);
            this.random_actions.TabIndex = 5;
            this.random_actions.Text = "Random Actions";
            this.random_actions.UseVisualStyleBackColor = true;
            // 
            // open_reply
            // 
            this.open_reply.AutoSize = true;
            this.open_reply.Location = new System.Drawing.Point(6, 70);
            this.open_reply.Name = "open_reply";
            this.open_reply.Size = new System.Drawing.Size(83, 17);
            this.open_reply.TabIndex = 2;
            this.open_reply.Text = "Open/Reply";
            this.open_reply.UseVisualStyleBackColor = true;
            // 
            // open_reply_archive
            // 
            this.open_reply_archive.AutoSize = true;
            this.open_reply_archive.Location = new System.Drawing.Point(6, 48);
            this.open_reply_archive.Name = "open_reply_archive";
            this.open_reply_archive.Size = new System.Drawing.Size(124, 17);
            this.open_reply_archive.TabIndex = 1;
            this.open_reply_archive.Text = "Open/Reply/Archive";
            this.open_reply_archive.UseVisualStyleBackColor = true;
            // 
            // open_archive
            // 
            this.open_archive.AutoSize = true;
            this.open_archive.Location = new System.Drawing.Point(6, 26);
            this.open_archive.Name = "open_archive";
            this.open_archive.Size = new System.Drawing.Size(92, 17);
            this.open_archive.TabIndex = 0;
            this.open_archive.Text = "Open/Archive";
            this.open_archive.UseVisualStyleBackColor = true;
            // 
            // group_spam_yahoo
            // 
            this.group_spam_yahoo.Controls.Add(this.ckeckall_not_spam);
            this.group_spam_yahoo.Controls.Add(this.check_not_spam);
            this.group_spam_yahoo.Controls.Add(this.read_not_spam);
            this.group_spam_yahoo.Location = new System.Drawing.Point(6, 6);
            this.group_spam_yahoo.Name = "group_spam_yahoo";
            this.group_spam_yahoo.Size = new System.Drawing.Size(153, 100);
            this.group_spam_yahoo.TabIndex = 0;
            this.group_spam_yahoo.TabStop = false;
            this.group_spam_yahoo.Text = "SPAM ACTION";
            // 
            // ckeckall_not_spam
            // 
            this.ckeckall_not_spam.AutoSize = true;
            this.ckeckall_not_spam.Location = new System.Drawing.Point(6, 72);
            this.ckeckall_not_spam.Name = "ckeckall_not_spam";
            this.ckeckall_not_spam.Size = new System.Drawing.Size(116, 17);
            this.ckeckall_not_spam.TabIndex = 2;
            this.ckeckall_not_spam.TabStop = true;
            this.ckeckall_not_spam.Text = "CheckAll/NotSpam";
            this.ckeckall_not_spam.UseVisualStyleBackColor = true;
            // 
            // check_not_spam
            // 
            this.check_not_spam.AutoSize = true;
            this.check_not_spam.Location = new System.Drawing.Point(6, 49);
            this.check_not_spam.Name = "check_not_spam";
            this.check_not_spam.Size = new System.Drawing.Size(105, 17);
            this.check_not_spam.TabIndex = 1;
            this.check_not_spam.TabStop = true;
            this.check_not_spam.Text = "Check/NotSpam";
            this.check_not_spam.UseVisualStyleBackColor = true;
            // 
            // read_not_spam
            // 
            this.read_not_spam.AutoSize = true;
            this.read_not_spam.Location = new System.Drawing.Point(6, 26);
            this.read_not_spam.Name = "read_not_spam";
            this.read_not_spam.Size = new System.Drawing.Size(100, 17);
            this.read_not_spam.TabIndex = 0;
            this.read_not_spam.TabStop = true;
            this.read_not_spam.Text = "Read/NotSpam";
            this.read_not_spam.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 429);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip1.Size = new System.Drawing.Size(893, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(145, 17);
            this.toolStripStatusLabel1.Text = "BY MED AMINE EL ATTABI";
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 451);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Report_FormClosing);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.DELETE.ResumeLayout(false);
            this.DELETE.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.group_inbox_yahoo.ResumeLayout(false);
            this.group_inbox_yahoo.PerformLayout();
            this.group_spam_yahoo.ResumeLayout(false);
            this.group_spam_yahoo.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_emails;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txt_thread;
        private System.Windows.Forms.Button Btn_yahoo_action;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox group_spam_yahoo;
        private System.Windows.Forms.GroupBox group_inbox_yahoo;
        private System.Windows.Forms.Button Btn_delete_yahoo;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txt_subject;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton open_archive;
        private System.Windows.Forms.RadioButton random_actions;
        private System.Windows.Forms.RadioButton open_reply_archive;
        private System.Windows.Forms.CheckBox body_click;
        private System.Windows.Forms.CheckBox star_click;
        private System.Windows.Forms.RadioButton open_reply;
        private System.Windows.Forms.RadioButton select_archive;
        private System.Windows.Forms.RadioButton ckeckall_not_spam;
        private System.Windows.Forms.RadioButton check_not_spam;
        private System.Windows.Forms.RadioButton read_not_spam;
        private System.Windows.Forms.RadioButton select_all_archive;
        private System.Windows.Forms.GroupBox DELETE;
        private System.Windows.Forms.CheckBox checkbox_inbox_yahoo;
        private System.Windows.Forms.CheckBox checkbox_spam_yahoo;
        private System.Windows.Forms.RadioButton open;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.LinkLabel edit_emails;
    }
}