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
using Forms = System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Windows.Threading;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Set a timer to watch the changes of CurrentBalance
            //DispatcherTimer t = new DispatcherTimer();
            //t.Interval = TimeSpan.FromSeconds(1);
            //t.Tick += new EventHandler(t_Tick);
            //t.Start();

            SearchButton.Visibility = Visibility.Hidden;
            SearchBox.Visibility = Visibility.Hidden;

        }

        void t_Tick(object sender, EventArgs e)
        {
            Currentbalance.Text = Balance.CurrentBalance.ToString();

        }

        public class Balance
        {
            public static float StartingBalance = 100000F;
            public static float CurrentBalance = 100000.00F;


        }
        public class StockInformation
        { 
            public static string CompanySymbol;
            public static string CompanyFullName;
            public static string StockPrice;
            public static string StockDaysRange;
        }

        public void Search_Clicked(object sender, EventArgs e)
        {

            
            var nextWindow = new CompanyWindow();          
            nextWindow.Show();
        }

        public void Portfolio_Clicked(object sender, EventArgs e)
        {


            var nextWindow = new Portfolio();
            nextWindow.Show();
        }

        public void Save_Clicked(object sender, EventArgs e)
        {
            var filePath = getSaveFile();
            if (!File.Exists(Environment.CurrentDirectory + "\\SaveChanges\\log.txt"))
            {
                return;
            }
            File.Copy(Environment.CurrentDirectory + "\\SaveChanges\\log.txt", filePath);
            File.AppendAllText(filePath, Balance.StartingBalance.ToString() + "\r\n");
            File.AppendAllText(filePath, Balance.CurrentBalance.ToString());
            MessageBox.Show("Save successful.");
        }

        public void Search_Box_OnFocus(object sender, EventArgs e)
        {
            SearchBox.Text = "";

        }
        

        private void Favorites_Clicked(object sender, RoutedEventArgs e)
        {
            var nextWIndow = new FavoriteList();
            nextWIndow.Show();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StockInformation.CompanySymbol = SearchBox.Text;
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            this.Close();
        }

        static private string getSaveFile()
        {
            string fileName = "";
            int counter = 1;
            int tempCounter = 1;

            if (!Directory.Exists(Environment.CurrentDirectory + "\\SaveChanges\\"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\SaveChanges\\");
            }

            fileName = DateTime.Now.ToString().Replace(':', '_');
            fileName = fileName.Replace('/', '_');
            fileName = fileName.Replace('.', '_');
            fileName = fileName.Replace(' ', '_') + "_" + counter;

            while (File.Exists(fileName))
            {
                tempCounter = counter;
                counter++;
                fileName.Replace(tempCounter.ToString(), counter.ToString());
            }

            return Environment.CurrentDirectory + "\\SaveChanges\\" + fileName + ".txt";
        }

        private void Window_Closing (object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.Delete(Environment.CurrentDirectory + "\\SaveChanges\\log.txt");
        }

        private void Fill_Clicked(object sender, EventArgs e)
        {
            var portfolioWindow = new Fill();
            portfolioWindow.Show();
        }

        private void Recommendation_Clicked(object sender, EventArgs e)
        {
            var RecommendationWindow = new Recommendation();
            RecommendationWindow.Show();
        }

        private void Load_Clicked(object sender, EventArgs e)
        {
            Forms.OpenFileDialog openFileDialog = new Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory + "\\SaveChanges\\";
            openFileDialog.ShowDialog();
            var fileName = openFileDialog.FileName;
            if (openFileDialog.FileName == "")
            {
                return;
            }
            loadFile(fileName);

            StartingBalanceEntry.Visibility = Visibility.Hidden;
            StartingBalanceButton.Visibility = Visibility.Hidden;
            StartingText.Visibility = Visibility.Hidden;

            SearchButton.Visibility = Visibility.Visible;
            SearchBox.Visibility = Visibility.Visible;
        }

        private void loadFile(string fileName)
        {
            string[] lines;
            if (File.Exists(Environment.CurrentDirectory + "\\SaveChanges\\log.txt"))
            {
                File.Delete(Environment.CurrentDirectory + "\\SaveChanges\\log.txt");
            }

            lines = File.ReadAllLines(fileName);

            Balance.StartingBalance = float.Parse(lines[lines.Length - 2]);
            Balance.CurrentBalance = float.Parse(lines[lines.Length - 1]);

            Startingbalance.Text = Balance.StartingBalance.ToString();
            Currentbalance.Text = Balance.CurrentBalance.ToString();

            File.Create(Environment.CurrentDirectory + "\\SaveChanges\\temp.txt").Close();
            for (int i = 0; i < lines.Length - 2; i++ )
            { 
                File.AppendAllText(Environment.CurrentDirectory + "\\SaveChanges\\temp.txt", lines[i] + "\r\n"); 
            }

            File.Copy(Environment.CurrentDirectory + "\\SaveChanges\\temp.txt", Environment.CurrentDirectory + "\\SaveChanges\\log.txt");
            File.Delete(Environment.CurrentDirectory + "\\SaveChanges\\temp.txt");
        }

        private void Currentbalance_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void StartingBalanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartingBalanceEntry.Text == "")
            {
                MessageBox.Show("Starting balance cannot be blank!");
                return;
            }

            StartingBalanceEntry.Visibility = Visibility.Hidden;
            StartingBalanceButton.Visibility = Visibility.Hidden;
            StartingText.Visibility = Visibility.Hidden;

            SearchButton.Visibility = Visibility.Visible;
            SearchBox.Visibility = Visibility.Visible;

            Balance.StartingBalance = float.Parse(StartingBalanceEntry.Text);
            Balance.CurrentBalance = float.Parse(StartingBalanceEntry.Text);

            Startingbalance.Text = Balance.StartingBalance.ToString();
            Currentbalance.Text = Balance.CurrentBalance.ToString();

            DispatcherTimer t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(1);
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        private void Reset_Clicked (object sender, RoutedEventArgs e)
        {
            if (File.Exists(Environment.CurrentDirectory + "\\SaveChanges\\log.txt"))
            {
                File.Delete(Environment.CurrentDirectory + "\\SaveChanges\\log.txt");
            }

            Balance.CurrentBalance = 0;
            Balance.StartingBalance = 0;

            this.Currentbalance.Text = "";
            this.Startingbalance.Text = "";
            this.StartingBalanceEntry.Text = "";

            SearchButton.Visibility = Visibility.Hidden;
            SearchBox.Visibility = Visibility.Hidden;

            StartingBalanceEntry.Visibility = Visibility.Visible;
            StartingBalanceButton.Visibility = Visibility.Visible;
            StartingText.Visibility = Visibility.Visible;

        }


    }
}
