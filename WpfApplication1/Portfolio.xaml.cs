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
using System.Data;
using System.Xml;
using System.Windows.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Portfolio.xaml
    /// </summary>
    public partial class Portfolio : Window
    {
        public Portfolio()
        {
            InitializeComponent();
            DispatcherTimer t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(1);
            t.Tick += new EventHandler(t_Tick);
            t.Start();
    //        ReadFile();

        }
        static float UnrealizedBalance = 0;
        void t_Tick(object sender, EventArgs e)
        {
            CurrentBlance_Box.Text = MainWindow.Balance.CurrentBalance.ToString();
            UnrealizedBlance_Box.Text = UnrealizedBalance.ToString();
        }

        public void Read_log_file_Clicked(object sender, EventArgs e)
        {
            Portfolio_Box.Text = "";
            ReadFile();
    
       
          

        }

    
        public void Cancel_Clicked(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Sell_Clicked(object sender, EventArgs e)
        {
            var nextWindow = new SellStock();
            nextWindow.Show();
         //   this.Close();
         //   Application.Current.Shutdown();
          
        }

        //public float GetCurrentPrice(string StockSymbol)
        //{
        //    XmlDocument docPrice = new XmlDocument();
        //    string result = null;
        //    string urlPrice = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22"
        //                  + StockSymbol
        //                  + "%22)&env=store://datatables.org/alltableswithkeys";

        //    docPrice.Load(urlPrice);

        //    /*Get stock price*/
        //    result = docPrice.SelectSingleNode("/query/results/quote/LastTradePriceOnly").InnerText;
        //    return float.Parse(result);
        //}

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
            public void ReadFile()
            {
                string path = Environment.CurrentDirectory + "\\SaveFiles\\log.txt";
                string result = "";

                List<String[]> ls = new List<String[]>();



                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
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
                    float Earn = 0f;
                    float CurrentPrice = 0f;
                    float OldPrice = 0f;
                    int StockShares = 0;
                    foreach (string s in t)
                    {

                        i++;
                        if (i == 3)
                        {
                            CurrentPrice = GetCurrentPrice(s);
                        }
                        if (i == 4)
                        {
                            OldPrice = float.Parse(s);
                        }
                        if (i == 5)
                        {
                            StockShares = Int32.Parse(s);
                            Earn = (OldPrice - CurrentPrice) * StockShares;
                            UnrealizedBalance += Earn;
                            result = result + CurrentPrice + "       " + Earn.ToString("0.000") + "       ";
                        }

                        result = result + s + "       ";
                    }
                    result += "\n";
                }

                Portfolio_Box.Text = result;

                sr.Close();
                fs.Close();
                sr.Dispose();
                fs.Dispose();
            }


        

    }
}
