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
using System.Xml;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for BuyStock.xaml
    /// </summary>
    public partial class SellStock : Window
    {
        public SellStock()
        {
            InitializeComponent();
            
        }

        static string SellShares;
        static string SellStockSymbol;
        public void Shares_Box_OnFocus(object sender, EventArgs e)
        {
            tbShares.Text = "";

        }

        public void Shares_TextChanged(object sender, EventArgs e)
        {
            SellShares = tbShares.Text;
        }

        public void Sell_Stock_Clicked(object sender, RoutedEventArgs e)
        {
            Sellstock(SellStockSymbol);

        }

        private void Sellstock(string SellStockSymbol)
        {
            string path = Environment.CurrentDirectory + "\\SaveFiles\\log.txt";
            int StockLimit = 0;

            List<String[]> ls = new List<String[]>();
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            string[] DataLine;
            while (!sr.EndOfStream)
            {
                DataLine = sr.ReadLine().Split(' ');
                ls.Add(DataLine);
            }
            
            foreach (string[] t in ls)
            {
                int i = 0;
                bool flag = false;
                
                foreach (string s in t)
                {                   
                    if (i == 2)
                    {
                        if (s == SellStockSymbol)
                            flag = true;
                    }
                    if (i == 4 && flag == true)
                    {
                        flag = false;
                     
                        StockLimit += Int32.Parse(s);
                    }
                    i++;
                }

            }
            fs.Close();
            sr.Close();

            if (Int32.Parse(SellShares) > StockLimit)
            {
                MessageBox.Show("Failed! You don't have enough shares to sell.");
            }
            else
            {
                string stockPrice = GetCurrentPrice(SellStockSymbol).ToString();
                string log = DateTime.Now.ToString() + " " + SellStockSymbol + " " + stockPrice + " -" + SellShares;
                File.AppendAllText(path, log + "\n");

                MessageBox.Show("Succeed!");
            }
        }

        

        public void Cancel_Clicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Stock_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            SellStockSymbol = Stock_Box.Text;
        }

        private void Stock_Box_OnFocus(object sender, RoutedEventArgs e)
        {
            Stock_Box.Text = "";
        }
        public float GetCurrentPrice(string StockSymbol)
        {
            XmlDocument docPrice = new XmlDocument();
            string result = null;
            string urlPrice = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22"
                          + StockSymbol
                          + "%22)&env=store://datatables.org/alltableswithkeys";

            docPrice.Load(urlPrice);

            /*Get stock price*/
            result = docPrice.SelectSingleNode("/query/results/quote/LastTradePriceOnly").InnerText;
            return float.Parse(result);
        }
    }
}
