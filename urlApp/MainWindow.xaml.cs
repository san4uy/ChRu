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
       
        private BindingList<DataModel> _dataList;

        private FileIOService _fileIOService;
        private int it;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOService = new FileIOService();

            try
            {
                _dataList = _fileIOService.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
            //_dataList = new BindingList<DataModel>();
            //DataModel t = new DataModel();
            //_dataList.Add(t);
            it = 0;
            //_dataList.Add(new DataModel() { Title = "hell", WordDataList = new BindingList<WordModel>() { new WordModel() { WChina = "111111" } } });
            //if(_dataList[0].WordDataList == null) 
           dgTranslater.ItemsSource = _dataList[0].WordDataList;
           _dataList.ListChanged += _wordDataList_ListChanged;
            //_dataList.Add(new DataModel() { Title = "hell", WordDataList = new BindingList<WordModel>() { new WordModel() { WChina = "2222" } } });
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
            if (e.ListChangedType == ListChangedType.ItemAdded) it++;
        }
        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if ((e.Key == Key.Y) && (dgTranslater.Visibility == Visibility.Visible))
            //{
            //    dgTranslater.Visibility = Visibility.Collapsed;
            //    gWords.Visibility = Visibility.Visible;
            //    lCh.Content = _wordDataList.ElementAtOrDefault(1).WChina;
            //}
            //else if (e.Key == Key.Y)
            //{
            //    dgTranslater.Visibility = Visibility.Visible;
            //    gWords.Visibility = Visibility.Collapsed;
            //}

            //if (e.Key == Key.Right)
            //{
            //    if (++it > _wordDataList.Count - 1)
            //    {
            //        it = 0;
            //    }
            //    lCh.Content = _wordDataList.ElementAt(it).WChina;
            //}
            //if (e.Key == Key.Left)
            //{
            //    if (--it < 0)
            //    {
            //        it = _wordDataList.Count - 1;
            //    }
            //    lCh.Content = _wordDataList.ElementAt(it).WChina;
            //}
            //if (e.Key == Key.S)
            //{
            //    var sortedList = new BindingList<WordModel>(_wordDataList.OrderBy(x => int.Parse(x.Status)).ToList());
            //    _wordDataList.Clear();
            //    foreach (var word in sortedList)
            //        _wordDataList.Add(word);
            //    sortedList.Clear();
            //    it = 0;
            //    //_wordDataList = (BindingList<WordModel>)_wordDataList.OrderByDescending(x => x.Status).ToList();
            //    //List<WordModel> w_list = new List<WordModel>();
            //    //foreach (var word in _wordDataList)
            //    //{
            //    //    w_list.Add(word);
            //    //}


            //}
        }

        private void ControlTemplate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("HEEEEEEELOOOOOOOOOOO1");
        }


    }
}
