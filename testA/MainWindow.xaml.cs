using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testA
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ExtractSound es = new ExtractSound();
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                es.Enco = tbIn.Text;
                tbOut.Text = es.Enco;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //es.Enco = tbIn.Text;
            //tbOut.Text = es.Enco;
            tbOut.Text = es.GetCodeAsync("ee").Result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            es.Code = tbIn.Text;
            tbOut.Text = es.Code;
        }
    }
}
