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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Data.SqlClient;
using System.Data;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DeleteCompanyInFavoriteList : Window
    {
        public DeleteCompanyInFavoriteList()
        {
            InitializeComponent();
                       
        }

        public void Cancel_Clicked(object sender, EventArgs e)
        {
            this.Close(); 
        }


        private void DeteleFavorite_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.StockInformation.CompanySymbol = DeleteFavoriteBox.Text;
        }

        public void Delete_Box_OnFocus(object sender, EventArgs e)
        {
            DeleteFavoriteBox.Text = "";

        }

        private void Yes_Clicked(object sender, EventArgs e)
        {
            CompanyWindow.Database DBDeleteStockInFavorites = new CompanyWindow.Database();

            DBDeleteStockInFavorites.ConnectToDB();
            DBDeleteStockInFavorites.DeleteDatabase(MainWindow.StockInformation.CompanySymbol);
            MessageBox.Show("Succeed!");
        }

        

       


       

    }
}
