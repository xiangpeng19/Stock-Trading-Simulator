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
    public partial class AddCompanyToFavoriteList : Window
    {
        public AddCompanyToFavoriteList()
        {
            InitializeComponent();
                       
        }

        public void Cancel_Clicked(object sender, EventArgs e)
        {
            this.Close(); 
        }


        


        private void AddFavorite_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.StockInformation.CompanySymbol = AddFavoriteBox.Text;
        }

        public void AddFavorite_Box_OnFocus(object sender, EventArgs e)
        {
            AddFavoriteBox.Text = "";

        }

        private void Yes_Clicked(object sender, EventArgs e)
        {
            CompanyWindow.DetailQuery AddStockToFavorite = new CompanyWindow.DetailQuery();
            CompanyWindow.Database DBAddStockToFavorites = new CompanyWindow.Database();
            AddStockToFavorite.GetDetail(MainWindow.StockInformation.CompanySymbol);
            
            DBAddStockToFavorites.ConnectToDB();
            DBAddStockToFavorites.AddToDatabase(MainWindow.StockInformation.CompanySymbol,
                                              MainWindow.StockInformation.CompanyFullName,
                                              Convert.ToDecimal(MainWindow.StockInformation.StockPrice));



            MessageBox.Show("Succeed!");
        }

        

       


       

    }
}
