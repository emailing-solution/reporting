using reporting.Properties;
using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace reporting
{
    public partial class Login : Form
    {
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
            else if(GetLocalIPAddress() != Settings.Default.ip)
            {
                MessageBox.Show("Hmmm ?", "PROSOFT MEDIA", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                data.mysql mysql = new data.mysql();
                object result = mysql.scalar("select count(*) from users where username = '"+username+"' and password ='"+password+"'");
                if (int.Parse(result.ToString()) == 1)
                {
                    this.Hide();
                    Menu m = new Menu();
                    m.ShowDialog();
                    this.Close();
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

        private void Btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
