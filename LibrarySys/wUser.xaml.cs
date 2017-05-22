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
using System.Data.OleDb;
using System.Data;

namespace LibrarySys
{
    /// <summary>
    /// Interaction logic for wUser.xaml
    /// </summary>
    public partial class wUser : Window
    {
        private OleDbConnection _dbConnBook = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source =" + "bookdb.mdb");
        private OleDbConnection _dbConnUser = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "userdb.mdb");
        private OleDbDataAdapter _dbAda;
        private int userid = -1;

        public wUser(int uid)
        {
            InitializeComponent();
            userid = uid;
            string cmd = "select * from Nuser where [ID] = " + uid;
            _dbAda = new OleDbDataAdapter(cmd, _dbConnUser);
            DataSet ds = new DataSet();
            _dbAda.Fill(ds);
            lblWel.Content = "Welcome " + ds.Tables[0].Rows[0]["username"].ToString();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgBooks_Loaded(object sender, RoutedEventArgs e)
        {
            _dbConnBook.Open();
            string ad = "select * from Books";
            _dbAda = new OleDbDataAdapter(ad, _dbConnBook);
            DataTable ds = new DataTable();
            _dbAda.Fill(ds);
            dgBooks.ItemsSource = ds.AsDataView();
            _dbConnBook.Close();
        }

        private void btnBor_Click(object sender, RoutedEventArgs e)
        {
            
            DataRowView selElem = (DataRowView)dgBooks.SelectedItem;
            if((int)selElem.Row[5] <= 0)
            {
                MessageBox.Show("Selected book has borrowed out");
                return;
            }
            DataSet bd = new DataSet();
            string bn = selElem.Row[1].ToString();
            string cmd = "select * from Books where [BookName] = '" + bn + "'";
            _dbAda = new OleDbDataAdapter(cmd, _dbConnBook);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(_dbAda);
            _dbAda.Fill(bd);
            int bid = Convert.ToInt32(bd.Tables[0].Rows[0]["ID"]);
            bd.Tables[0].Rows[0]["StoNum"] = (int)bd.Tables[0].Rows[0]["StoNum"] - 1;
            selElem.Row[5] = (int)selElem.Row[5] - 1;
            _dbAda.Update(bd);
            MessageBox.Show("Borrowed Success");
            
            string cmd2 = "select * from Nuser where [ID] = " + userid;
            _dbAda = new OleDbDataAdapter(cmd2, _dbConnUser);
            DataSet ud = new DataSet();
            cb = new OleDbCommandBuilder(_dbAda);
            _dbAda.Fill(ud);
            ud.Tables[0].Rows[0]["brwed"] += bid + ",";
            _dbAda.Update(ud);
        }

        private void btnRtn_Click(object sender, RoutedEventArgs e)
        {
            wRtn wr = new wRtn(userid);
            wr.Show();
        }

        private void btnRef_Click(object sender, RoutedEventArgs e)
        {
            _dbConnBook.Open();
            string ad = "select * from Books";
            _dbAda = new OleDbDataAdapter(ad, _dbConnBook);
            DataTable ds = new DataTable();
            _dbAda.Fill(ds);
            dgBooks.ItemsSource = ds.AsDataView();
            _dbConnBook.Close();
        }
    }
}
