using System;
using System.Collections.Generic;
using System.IO;
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
        //ExtractSound es = new ExtractSound();
        private MediaPlayer player = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                //es.Enco = tbIn.Text;

                //string path = es.GetPath();
                //string str = es.CutCsound(es.GetCSound());
                //using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                //{
                //    writer.Write(System.Convert.FromBase64String(str));
                //}
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            //player.Open(new Uri(es.GetPath(), UriKind.RelativeOrAbsolute));
            //player.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //es.Code = tbIn.Text;
            //tbOut.Text = es.Code;
        }
    }
}
