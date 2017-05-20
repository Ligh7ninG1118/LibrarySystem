using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace LibrarySys
{
    /// <summary>
    /// Interaction logic for wLogin.xaml
    /// </summary>
    public partial class wLogin : Window
    {
        private OleDbConnection _dbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "userdb.mdb");
        private OleDbDataAdapter _dbAda;
        public wLogin()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            rtxtWrnUser.Document.Blocks.Clear();
            txtUser.BorderBrush = Brushes.Black;
            rtxtWrnPass.Document.Blocks.Clear();
            txtPass.BorderBrush = Brushes.Black;
            rtxtWrnOpt.Document.Blocks.Clear();
            radAdmin.BorderBrush = Brushes.Black;
            radUser.BorderBrush = Brushes.Black;

            if (txtUser.Text == "" || txtPass.Password == "" || (radAdmin.IsChecked == false && radUser.IsChecked == false))
            {
                if (txtUser.Text == "")
                {
                    ErrorMsgOutput(rtxtWrnUser, "Username Cannot Be Empty");
                    txtUser.BorderBrush = Brushes.Red;
                }
                if (txtPass.Password == "")
                {
                    ErrorMsgOutput(rtxtWrnPass, "Password Cannot Be Empty");
                    txtPass.BorderBrush = Brushes.Red;
                }
                if (radAdmin.IsChecked == false && radUser.IsChecked == false)
                {
                    ErrorMsgOutput(rtxtWrnOpt, "User Option Cannot Be Empty");
                    radAdmin.BorderBrush = Brushes.Red;
                    radUser.BorderBrush = Brushes.Red;
                }
                return;
            }

            string user = txtUser.Text;
            string pass = txtPass.Password;
            string role = (bool)radAdmin.IsChecked ? "Admin" : "User";
            int ans;
            if (role == "Admin")
            {
                ans = VerifyAdmin(user, pass);
            }
            else
            {
                ans = VerifyUser(user, pass);
            }

            if (ans == 0)
            {
                if (role == "Admin")
                {
                    wAdmin wa = new wAdmin();
                    wa.Show(); 
                }
                else
                {
                    wUser wu = new wUser();
                    wu.Show();
                }
                this.Close();
            }
            if (ans == 1)
            {
                ErrorMsgOutput(rtxtWrnPass, "Password Wrong");
                txtPass.BorderBrush = Brushes.Red;
            }
            if (ans == 2)
            {
                ErrorMsgOutput(rtxtWrnUser, "Username Does Not Exists");
                txtUser.BorderBrush = Brushes.Red;
            }
            if(ans==3)//Pending
            {
                ErrorMsgOutput(rtxtWrnUser, "Regeister Request is Still Pending");
                txtUser.BorderBrush = Brushes.Red;
            }
            if(ans==4)
            {
                ErrorMsgOutput(rtxtWrnUser, "Regeister Request failed");
                txtUser.BorderBrush = Brushes.Red;
            }
        }

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtxtWrnUser.Document.Blocks.Clear();
            txtUser.BorderBrush = Brushes.Black;
        }

        private void PasswordChangedHandler(object sender, RoutedEventArgs e)
        {
            rtxtWrnPass.Document.Blocks.Clear();
            txtPass.BorderBrush = Brushes.Black;
        }

        private void rad_Checked(object sender, RoutedEventArgs e)
        {
            rtxtWrnOpt.Document.Blocks.Clear();
            radAdmin.BorderBrush = Brushes.Black; radUser.BorderBrush = Brushes.Black;
        }

        private int VerifyAdmin(string u, string p)
        {
            string sqlcomm = "select * from Admin where username = '" + u + "'";
            _dbAda = new OleDbDataAdapter(sqlcomm, _dbConn);
            DataSet dt = new DataSet();
            _dbAda.Fill(dt);
            string dbp = null;
            if (dt.Tables[0].Rows.Count != 0)
                dbp = dt.Tables[0].Rows[0]["password"].ToString();
            if (dbp == null)
                return 2;
            else if (p == dbp)
                return 0;
            else
                return 1;
        }

        private int VerifyUser(string u, string p)
        {
            string sqlcomm = "SELECT * FROM Nuser WHERE username = '" + u + "'";
            _dbAda = new OleDbDataAdapter(sqlcomm, _dbConn);
            DataSet dt = new DataSet();
            _dbAda.Fill(dt);
            string dbp = null;
            string dbs = null;
            if (dt.Tables[0].Rows.Count != 0)
            {
                dbp = dt.Tables[0].Rows[0]["password"].ToString();
                dbs = dt.Tables[0].Rows[0]["status"].ToString();
            }
            if (dbp == null)
                return 2;
            if(dbs == "Pending") //Pending
                return 3;
            if (dbs == "Rejected")//Failed
                return 4;
            else if (p == dbp)
                return 0;
            else
                return 1;
        }

        private void ErrorMsgOutput(RichTextBox rtxt, string msg)
        {
            TextRange tr = new TextRange(rtxt.Document.ContentEnd, rtxt.Document.ContentEnd);
            tr.Text = msg;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
        }
    }
}