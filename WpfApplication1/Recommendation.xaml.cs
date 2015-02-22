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
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Portfolio.xaml
    /// </summary>
    public partial class Recommendation : Window
    {
        public Recommendation()
        {
            InitializeComponent();

            
        }

        private void Multiple_OnFocus(object sender, EventArgs e)
        {
            MultipleBox.Text = "";
        }

        private void Stock_OnFocus(object sender, EventArgs e)
        {
            StockBox.Text = "";
        }
        private void Stock_TextChanged(object sender, TextChangedEventArgs e)
        {
            StockRecommendation = StockBox.Text;
        }


        private void Multiple_TextChanged(object sender, TextChangedEventArgs e)
        {
            multiple = float.Parse(MultipleBox.Text);
        }

        static string result;
        static float PE_Ratio = 15.52F;
        static float multiple;
        static string StockRecommendation;
        
        private void Search_Clicked(object sender, EventArgs e)
        {
            Recommend_Multiple();
        }
        private void Search1_Clicked(object sender, EventArgs e)
        {
            Recommend_Stock(StockRecommendation);
        }

        private void Recommend_Stock(string companysymbol)
        {
            XmlDocument docPrice = new XmlDocument();
            string PEratio = "";
            string urlPrice = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22"
                          + companysymbol
                          + "%22)&env=store://datatables.org/alltableswithkeys";

            docPrice.Load(urlPrice);
            /*Get stock PERaio*/
            PEratio = docPrice.SelectSingleNode("/query/results/quote/PERatio").InnerText;

            if (float.Parse(PEratio) < 15.52F)
            {
                MessageBox.Show("We DO NOT recommend this stock, because the PE ratio is below the average");
            }

            else if ( 15.52F < float.Parse(PEratio) && float.Parse(PEratio) < 19F )
            {
                MessageBox.Show("The PE ratio is slightly above the PE ratio, we recommend to put it to favorite list.");

            }
            else
            {
                MessageBox.Show("We highly recommend this stock, The PE ratio is much higher than the average.");
            }
        }
        private void Recommend_Multiple() 
        {
            StockQuery query = new StockQuery();
            PE_Ratio *= multiple;
            query.GetStockSymbol();
            query.QueryStockSymbol();
            ResultBox.Text = result;
        }

        private void Set_Boxtext(string s)
        {
            ResultBox.Text = result;
        }
        public class StockQuery
        {
            string id = "112";
            List<string> symbolList = new List<string>();

            public void GetStockSymbol()
            {
                XmlDocument doc = new XmlDocument();
                string urlSymbol = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.industry%20where%20id%3D%22"
                                    + id 
                                    + "%22&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
                doc.Load(urlSymbol);
                XmlNode root = doc.SelectSingleNode("/query/results/industry");
                XmlNodeList child = root.ChildNodes;
                foreach (XmlElement xc in child)
                {
                    symbolList.Add(xc.GetAttribute("symbol"));
                }

            }

            public void QueryStockSymbol()
            {
                
                foreach (string t in symbolList)
                {
                    XmlDocument doc = new XmlDocument();
                    string urlPrice = "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22"
                              + t
                              + "%22)&env=store://datatables.org/alltableswithkeys";
                    try
                    {
                        doc.Load(urlPrice);
                        string PERatio = doc.SelectSingleNode("/query/results/quote/PERatio").InnerText;
                        string Name = doc.SelectSingleNode("/query/results/quote/Name").InnerText;

                        if (PE_Ratio > float.Parse(PERatio))
                            continue;
                        else
                        {
                            result += "Company name: " + Name + "     " + "PE ratio: " + PERatio + "\n";
                        }
                    }
                    catch
                    {
                        continue;
                    }

                }
            }
        }


    }
}
