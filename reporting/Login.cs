using reporting.Properties;
using Slack.Webhooks;
using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using CryptSharp;
using System.Data;
using MySql.Data.MySqlClient;

namespace reporting
{
    public partial class Login : Form
    {
        private static string ip = GetLocalIPAddress();

        public static string GetLocalIPAddress()
        {
            string address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }
            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }
        public static bool IsNetworkAvailable(long minimumSpeed)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return false;
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // discard because of standard reasons
                if ((ni.OperationalStatus != OperationalStatus.Up) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                    continue;

                // this allow to filter modems, serial, etc.
                // I use 10000000 as a minimum speed for most cases
                if (ni.Speed < minimumSpeed)
                    continue;

                // discard virtual cards (virtual box, virtual pc, etc.)
                if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                    continue;

                // discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
                if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                    continue;

                return true;
            }
            return false;
        }

        public Login()
        {
            if(!IsNetworkAvailable(0))
            {
                DialogResult dialog = MessageBox.Show("No Connexion Available Check please", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if(dialog == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            else if(!ip.Contains(Settings.Default.ip))
            {

                logger();
                MessageBox.Show("Hmmm ? a report has been send :)", "PROSOFT MEDIA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            else
            {
                InitializeComponent();
            }
        }

        private void Btn_login_Click(object sender, EventArgs e)
        {
            
            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                string pass = "";
                data.mysql mysql = new data.mysql();
                var data  = mysql.Select("select password from users where username = '"+username+"'");
                while(data.Read())
                {
                    pass = data.GetString(0);
                }
                data.Close();



                if (Crypter.CheckPassword(password, pass))
                {
                    Hide();
                    Report r = new Report();
                    r.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Unknow Login Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Enter Your Login Data","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void logger()
        {
            string pcname = Environment.MachineName;
            string ipa = ip;
            var slackClient = new SlackClient("https://hooks.slack.com/services/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");
            var slackMessage = new SlackMessage
            {
                Channel = "#reporting",
                Text = "Look at this someone trying to use this app pcname = " + pcname + " and ip = " + ip + " Date : "+DateTime.Now,
                IconEmoji = Emoji.Ghost,
            };
            slackClient.Post(slackMessage);
        }

        private void Btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
