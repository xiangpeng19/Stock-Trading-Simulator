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
using System.Data.SqlClient;
using System.Data;



namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for FavoriteList.xaml
    /// </summary>
    public partial class FavoriteList : Window
    {
        public FavoriteList()
        {
            InitializeComponent();

            QueryFavoriteList();

        }

        public void AddCompany_Clicked(object sender, EventArgs e)
        {
            var nextWIndow = new AddCompanyToFavoriteList();
            nextWIndow.Show();
        }

        public void DeleteCompany_Clicked(object sender, EventArgs e)
        {
            var nextWIndow = new DeleteCompanyInFavoriteList();
            nextWIndow.Show();
        }

        public void QueryFavoriteList()
        {
            string connectionAddress = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=" + "D:\\Study\\UIC\\CS440 Software Engineering\\Code\\WpfApplication1\\StockBuddy.mdf" + ";Integrated Security=True";
         //   string connectionAddress = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=" + Environment.CurrentDirectory + "\\StockBuddy.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection conn = new SqlConnection();
            string sql = null;
            SqlCommand cmd = null;
            conn.Close();
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();

            conn.ConnectionString = connectionAddress;
            conn.Open();
            sql = "select * from PublicCompanies";
            cmd = new SqlCommand(sql, conn);

            SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
            sda.SelectCommand = cmd;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            FavoriteList1.ItemsSource = ds.Tables[0].DefaultView;
            conn.Close();

        }
        
      

    }
}
