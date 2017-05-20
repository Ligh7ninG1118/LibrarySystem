using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Data.OleDb;

namespace LibrarySys
{
    /// <summary>
    /// Interaction logic for wAddBook.xaml
    /// </summary>
    public partial class wAddBook : Window
    {
        private OleDbConnection _dbConnBook = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source =" + "bookdb.mdb");

        public wAddBook()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(txtAuthor.Text==""|| txtPress.Text == ""|| txtBookName.Text == ""|| txtSN.Text == ""|| txtYear.Text == "")
            {
                MessageBox.Show("Please fill all fields");
            }

            //TODO 判断非法输入

            string a = txtAuthor.Text;
            string b = txtBookName.Text;
            string press = txtPress.Text;
            int year = Convert.ToInt32(txtYear.Text);
            int sn = Convert.ToInt32(txtSN.Text);

            _dbConnBook.Open();
            string comm = "insert into Books([BookName], [Author], [Press], [PubYear], [StoNum]) values('" + b + "', '" + a + "', '" + press + "', '" + year + "', '" + sn + "')";
            OleDbDataAdapter _ins = new OleDbDataAdapter(comm, _dbConnBook);
            DataSet ds = new DataSet();
            _ins.Fill(ds);
            _dbConnBook.Close();
            MessageBox.Show("Book added");
            txtAuthor.Text="";
            txtBookName.Text = "";
            txtPress.Text = "";
            txtYear.Text = "";
            txtSN.Text = "";
            return;
        }
    }
}
