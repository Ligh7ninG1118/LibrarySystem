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
    /// Interaction logic for wBookEdit.xaml
    /// </summary>
    public partial class wBookEdit : Window
    {
        private OleDbConnection _dbConnBook = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source =" + "bookdb.mdb");
        private OleDbDataAdapter _dbAda;
        private DataSet bd = new DataSet();

        private int bid = -1;
        public wBookEdit(int bid)
        {
            InitializeComponent();
            this.bid = bid;
            
            string cmd = "select * from Books where [ID] = " + bid;
            _dbAda = new OleDbDataAdapter(cmd, _dbConnBook);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(_dbAda);
            _dbAda.Fill(bd);
            txtBookName.Text = bd.Tables[0].Rows[0]["BookName"].ToString();
            txtAuthor.Text = bd.Tables[0].Rows[0]["Author"].ToString();
            txtPress.Text = bd.Tables[0].Rows[0]["Press"].ToString();
            txtSN.Text = bd.Tables[0].Rows[0]["StoNum"].ToString();
            txtYear.Text = bd.Tables[0].Rows[0]["PubYear"].ToString();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            bd.Tables[0].Rows[0]["BookName"] = txtBookName.Text;
            bd.Tables[0].Rows[0]["Author"] = txtAuthor.Text;
            bd.Tables[0].Rows[0]["Press"] = txtPress.Text;
            bd.Tables[0].Rows[0]["StoNum"] = txtSN.Text;
            bd.Tables[0].Rows[0]["PubYear"] = txtYear.Text;
            _dbAda.Update(bd);
            MessageBox.Show("Edit done");
            this.Close();
        }
    }
}
