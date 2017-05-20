using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.OleDb;

namespace LibrarySys
{
    /// <summary>
    /// Interaction logic for wAdAdm.xaml
    /// </summary>
    public partial class wAdAdm : Window
    {
        private OleDbConnection _dbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "userdb.mdb");

        public wAdAdm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            rtxtWrnUser.Document.Blocks.Clear();
            txtUser.BorderBrush = Brushes.Black;
            rtxtWrnPass.Document.Blocks.Clear();
            txtPass.BorderBrush = Brushes.Black;

            if (txtUser.Text == "" || txtPass.Password == "")
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
                return;
            }

            string u = txtUser.Text;
            string p = txtPass.Password;

            string sqlcomm = "select * from Admin where username = '" + u + "'";
            OleDbDataAdapter _dbAda = new OleDbDataAdapter(sqlcomm, _dbConn);
            DataSet dt = new DataSet();
            _dbAda.Fill(dt);
            if (dt.Tables[0].Rows.Count != 0)
            {
                ErrorMsgOutput(rtxtWrnUser, "Admin already exists");
                txtUser.BorderBrush = Brushes.Red;
                return;
            }

            _dbConn.Open();
            string comm = "insert into Admin([username], [password]) values('" + u + "', '" + p + "')";
            OleDbDataAdapter _ins = new OleDbDataAdapter(comm, _dbConn);
            DataSet ds = new DataSet();
            _ins.Fill(ds);
            _dbConn.Close();
            MessageBox.Show("Admin added");
            this.Close();
            return;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        private void ErrorMsgOutput(RichTextBox rtxt, string msg)
        {
            TextRange tr = new TextRange(rtxt.Document.ContentEnd, rtxt.Document.ContentEnd);
            tr.Text = msg;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
        }
    }
}
