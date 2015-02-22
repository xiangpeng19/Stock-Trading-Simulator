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
using System.IO;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for BuyStock.xaml
    /// </summary>
    public partial class BuyStock : Window
    {
        public BuyStock()
        {
            InitializeComponent();
    

        }

        public void Cancel_Clicked(object sender, EventArgs e)
        {
            this.Close();
        }

        public class StockNumber
        {
            public static string StockShares;
            public static string StockSymbol;
        }
        class Stock
        {
            private string path = Environment.CurrentDirectory + "\\SaveFiles\\log.txt";
            string log;
            public Boolean Purchase(float currentBalance, float stockPrice, int stockNumber, string symbol) 
            {

                if (!Directory.Exists(Environment.CurrentDirectory + "\\SaveFiles\\"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\SaveFiles\\");
                }

                if (currentBalance - stockNumber * stockPrice < 0)
                {
                    float t = (int) currentBalance / stockPrice;
                    MessageBox.Show("Failed! You have not enough money. The maximum shares you can buy is " + (Convert.ToInt32(t) - 1));
                }
                else
                {
                    currentBalance -= stockNumber * stockPrice;
                    MainWindow.Balance.CurrentBalance = currentBalance;

                    log = DateTime.Now.ToString() + " " + symbol + " " + stockPrice + " " + stockNumber;
                    File.AppendAllText(path, log + "\r\n");
                   
                    MessageBox.Show("Succeed! The current balance is : " + currentBalance);
                }
                return true;
                
            }

           
        }

        public void Shares_Box_OnFocus(object sender, EventArgs e)
        {
            tbShares.Text = "";

        }


        public void Shares_TextChanged(object sender, EventArgs e)
        {
            StockNumber.StockShares = tbShares.Text;
        }



        public void Purchase_Stock_Clicked(object sender, RoutedEventArgs e)
        {
 
            Stock PurchaseStock = new Stock();
            PurchaseStock.Purchase(MainWindow.Balance.CurrentBalance,
                                   float.Parse(MainWindow.StockInformation.StockPrice),
                                   int.Parse(StockNumber.StockShares), MainWindow.StockInformation.CompanySymbol);

            

        }

    }
}
