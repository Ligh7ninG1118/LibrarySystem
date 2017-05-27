using System;
using System.Windows;
using System.Data;
using System.Data.OleDb;


namespace LibrarySys
{
    
    public partial class wAdmin : Window
    {
        private OleDbConnection _dbConnUser = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "userdb.mdb");
        private OleDbConnection _dbConnBook = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ="+ "bookdb.mdb");
        private OleDbDataAdapter _dbAda;
        private int uid = -1;
        public wAdmin(int id)
        {
            InitializeComponent();
            uid = id;
            string cmd = "select * from Admin where [ID] = " + uid;
            _dbAda = new OleDbDataAdapter(cmd, _dbConnUser);
            DataSet ds = new DataSet();
            _dbAda.Fill(ds);
            lblWel.Content = "Welcome " + ds.Tables[0].Rows[0]["username"].ToString();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRef_Click(object sender, RoutedEventArgs e)
        {
            _dbConnUser.Open();
            string ad = "select * from Admin";
            _dbAda = new OleDbDataAdapter(ad, _dbConnUser);
            DataTable ds = new DataTable();
            _dbAda.Fill(ds);
            dgAdm.ItemsSource = ds.AsDataView();

            string al = "select * from Nuser";
            _dbAda = new OleDbDataAdapter(al, _dbConnUser);
            DataTable da = new DataTable();
            _dbAda.Fill(da);
            dgAll.ItemsSource = da.AsDataView();
            _dbConnUser.Close();
        }
       

        private void dgAdm_Loaded(object sender, RoutedEventArgs e)
        {
            _dbConnUser.Open();
            string ad = "select * from Admin";
            _dbAda = new OleDbDataAdapter(ad, _dbConnUser);
            DataTable ds = new DataTable();
            _dbAda.Fill(ds);
            dgAdm.ItemsSource = ds.AsDataView();
            _dbConnUser.Close();
        }

        private void dgAll_Loaded(object sender, RoutedEventArgs e)
        {
            _dbConnUser.Open();
            string al = "select * from Nuser";
            _dbAda = new OleDbDataAdapter(al, _dbConnUser);
            DataTable da = new DataTable();
            _dbAda.Fill(da);
            dgAll.ItemsSource = da.AsDataView();
            _dbConnUser.Close();
        }

        private void btnAdm_Click(object sender, RoutedEventArgs e)
        {
            wAdAdm wa = new wAdAdm();
            wa.ShowDialog();
            return;
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            wAddBook ww = new wAddBook();
            ww.ShowDialog();
            return;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selElem = (DataRowView)dgBooks.SelectedItem;
            DataSet bd = new DataSet();
            if(selElem == null)
            {
                MessageBox.Show("Please select one book to edit");
                return;
            }
            string bn = selElem.Row[1].ToString();
            string cmd = "select * from Books where [Bookname] = '" + bn + "'";
            _dbAda = new OleDbDataAdapter(cmd, _dbConnBook);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(_dbAda);
            _dbAda.Fill(bd);

            wBookEdit wb = new wBookEdit(Convert.ToInt32(bd.Tables[0].Rows[0]["ID"]));
            wb.ShowDialog();
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

        private void btnRefB_Click(object sender, RoutedEventArgs e)
        {
            _dbConnBook.Open();
            string ad = "select * from Books";
            _dbAda = new OleDbDataAdapter(ad, _dbConnBook);
            DataTable ds = new DataTable();
            _dbAda.Fill(ds);
            dgBooks.ItemsSource = ds.AsDataView();
            _dbConnBook.Close();
        }

        private void btnApv_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selElem = (DataRowView)dgAll.SelectedItem;
            DataSet ud = new DataSet();
            string un = selElem.Row[1].ToString();
            string cmd = "select * from Nuser where [username] = '" + un + "'";
            _dbAda = new OleDbDataAdapter(cmd, _dbConnUser);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(_dbAda);
            _dbAda.Fill(ud);
            ud.Tables[0].Rows[0]["status"] = "Approved";
            ud.Tables[0].Rows[0]["brwed"] = -1;
            selElem.Row[3] = "Approved";
            _dbAda.Update(ud);
        }
    }
}
