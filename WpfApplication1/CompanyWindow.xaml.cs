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
using System.Xml;
using System.Data.SqlClient;
using System.Net;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CompanyWindow : Window
    {
        public CompanyWindow()
        {
            InitializeComponent();
            string ImageUrl = "http://ichart.finance.yahoo.com/c/bb/g/" + MainWindow.StockInformation.CompanySymbol;
            StockChart.Source = new BitmapImage(new Uri(ImageUrl));
            CompanyDisplay.IsReadOnly = true;
            DetailQuery CompanyDetail = new DetailQuery();
            CompanyDisplay.Text = CompanyDetail.GetDetail(MainWindow.StockInformation.CompanySymbol);
     
            
        }

        public void Place_Trade_Clicked(object sender, RoutedEventArgs e)
        {
            var nextWIndow = new BuyStock();
            nextWIndow.Show();
        }

        public class Database
        {

            private string connectionAddress = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Study\UIC\CS440 Software Engineering\Code\WpfApplication1\StockBuddy.mdf;Integrated Security=True;Connect Timeout=30";
            private static SqlConnection conn = new SqlConnection();
            private string sql = null;
            private SqlCommand cmd = null;
           
            public void ConnectToDB()
            {
                conn.Close();
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                conn.ConnectionString = connectionAddress;
                conn.Open();

            }
            public void AddToDatabase(string symbol, string company_name, decimal price)
            {

                sql = "INSERT INTO PublicCompanies(symbol, company_name, price, bid, ask)"
                    + "VALUES('" + symbol + "', '" + company_name +"', "+ price +", 32, 23)";
                try
                {
                    cmd = new SqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (SqlException)
                {
                    MessageBox.Show("Failed! Did you add this company symbol before or type wrongly?");
                    //throw;
                }

                conn.Close();
            }

            public void DeleteDatabase(string symbol)
            { 
                sql = "DELETE FROM PublicCompanies WHERE symbol = '" + symbol + "'";
                
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }


        }
        public void More_Details_Clicked(object sender, EventArgs e)
        {
            //pop out the browser appliction and search on the wikipedia. 
            string str = MainWindow.StockInformation.CompanyFullName;
            string[] arr = str.Split(' ');

            System.Diagnostics.Process.Start("http://en.wikipedia.org/wiki/" + arr[0]);
        }    

        public void Add_to_Favorites_Clicked(object sender, EventArgs e)
        {
            /*Add to Database*/

            Database AddStockToFavorites = new Database();
            AddStockToFavorites.ConnectToDB();
            AddStockToFavorites.AddToDatabase(MainWindow.StockInformation.CompanySymbol,
                                              MainWindow.StockInformation.CompanyFullName,
                                              Convert.ToDecimal(MainWindow.StockInformation.StockPrice));
            MessageBox.Show("Succeed!");

        }

        public class DetailQuery
        {
            public string GetDetail(String company)
            {
                XmlDocument docPrice = new XmlDocument();
                XmlDocument docInformation = new XmlDocument();
                string result = null;
                string urlPrice = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22"
                              + company
                              + "%22)&env=store://datatables.org/alltableswithkeys";
                string urlCompanyInformation = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.stocks%20where%20symbol%3D%22"
                              + company
                              + "%22&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
                try
                {
                    docPrice.Load(urlPrice);
                    docInformation.Load(urlCompanyInformation);
                    XmlNode companyInformation = docInformation.SelectSingleNode("/query/results/stock");
                    XmlNodeList companyInformationChild = companyInformation.ChildNodes;
                    foreach (XmlElement xc in companyInformationChild)
                    {
                        if (xc.Name == "CompanyName") continue;
                        else result = result + xc.Name + ": " + xc.InnerText + "\n";
                    }

                    /*Get stock price*/

                    MainWindow.StockInformation.CompanyFullName = docPrice.SelectSingleNode("/query/results/quote/Name").InnerText;
                    MainWindow.StockInformation.StockDaysRange = docPrice.SelectSingleNode("/query/results/quote/DaysRange").InnerText;
                    MainWindow.StockInformation.StockPrice = docPrice.SelectSingleNode("/query/results/quote/LastTradePriceOnly").InnerText;

                    result += "Company name: " + MainWindow.StockInformation.CompanyFullName + "\n"
                                               + "Price : " + MainWindow.StockInformation.StockPrice + "\n"
                                               + "DaysRange : " + MainWindow.StockInformation.StockDaysRange + "\n";

                                      
                }
                catch (WebException)
                {

                    MessageBox.Show("Sorry, no such company symbol");
                }

                return result;  
                      
            }

        }

    }
}
