using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.OleDb;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibrarySys
{
    /// <summary>
    /// Interaction logic for wRtn.xaml
    /// </summary>
    public partial class wRtn : Window
    {
        private OleDbConnection _dbConnBook = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source =" + "bookdb.mdb");
        private OleDbConnection _dbConnUser = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "userdb.mdb");
        private OleDbDataAdapter _dbAda;
        private int uid = -1;
        public wRtn(int uid)
        {
            InitializeComponent();
            this.uid = uid;
            string cmd = "select * from Nuser where [ID] = " + uid;
            string brw_coll;
            string cmd_book=null;
            _dbAda = new OleDbDataAdapter(cmd, _dbConnUser);
            DataSet ud = new DataSet();
            OleDbCommandBuilder cb = new OleDbCommandBuilder(_dbAda);
            _dbAda.Fill(ud);
            brw_coll = ud.Tables[0].Rows[0]["brwed"].ToString();
            cmd_book = "select * from Books where [ID] = " + Convert.ToInt32(brw_coll);
            _dbAda = new OleDbDataAdapter(cmd_book, _dbConnBook);
            cb = new OleDbCommandBuilder(_dbAda);
            DataTable bd = new DataTable();
            _dbAda.Fill(bd);
            dgvBooks.ItemsSource = bd.AsDataView();
        }

        private void btnExt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRtn_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selElem = (DataRowView)dgvBooks.SelectedItem;
            DataSet bd = new DataSet();
            DataSet ud = new DataSet();
            string bn = selElem.Row[1].ToString();
            string cmd = "select * from Books where [BookName] = '" + bn + "'";
            string cmd2 = "select * from Nuser where [ID] = " + uid;
            _dbAda = new OleDbDataAdapter(cmd, _dbConnBook);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(_dbAda);
            _dbAda.Fill(bd);
            int bid = Convert.ToInt32(bd.Tables[0].Rows[0]["ID"]);
            bd.Tables[0].Rows[0]["StoNum"] = (int)bd.Tables[0].Rows[0]["StoNum"] + 1;
            selElem.Row[5] = (int)selElem.Row[5] + 1;
            _dbAda.Update(bd);

            _dbAda = new OleDbDataAdapter(cmd2, _dbConnUser);
            cb = new OleDbCommandBuilder(_dbAda);
            _dbAda.Fill(ud);
            ud.Tables[0].Rows[0]["brwed"] = -1;
            _dbAda.Update(ud);
            MessageBox.Show("Return Success");
            this.Close();
        }
    }
}
