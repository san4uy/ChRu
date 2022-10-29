using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using urlApp.Models;
using urlApp.Services;

namespace urlApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly string PATH = $"{Environment.CurrentDirectory}\\wordDataList.json";
        private readonly string PATH;
        private BindingList<WordModel> _wordDataList;
        private FileIOService _fileIOService;

        public MainWindow()
        {
            string st = AppDomain.CurrentDomain.BaseDirectory;
            /////del
            int f = st.LastIndexOf('\\');
            f = st.Remove(f, 1).LastIndexOf('\\');
            f = st.Remove(f, st.Length-f).LastIndexOf('\\');
            f = st.Remove(f, st.Length-f).LastIndexOf('\\');
            st = st.Remove(f, st.Length-f);
            PATH = st.Insert(st.Length, "\\wordDataList.json");
            /////del
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOService = new FileIOService(PATH);
            try
            {
                _wordDataList = _fileIOService.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }

            dgTranslater.ItemsSource = _wordDataList;
            
            _wordDataList.ListChanged += _wordDataList_ListChanged;
        }

        private void _wordDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService.SaveData(sender);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }
        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if((e.Key == Key.Y) && (dgTranslater.Visibility == Visibility.Visible))
            {
                dgTranslater.Visibility = Visibility.Collapsed;
                gWords.Visibility = Visibility.Visible;
                lCh.Content = _wordDataList.ElementAtOrDefault(1).WChina;
            }
            else if(e.Key == Key.Y)
            {
                dgTranslater.Visibility = Visibility.Visible;
                gWords.Visibility = Visibility.Collapsed;
            }
        
            if(e.Key == Key.Up)
            {

            }
            if (e.Key == Key.S)
            {
                
            }
        }
    }
}
