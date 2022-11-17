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
        private DataModel toRunFromBindedList;
        private Border dgBorder;
        private FileIOService _fileIOService;
        private int it;
        private bool flg = false;
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
            //foreach (var item in _dataList)
            //{
            //    MenuItem newMenuItem1 = new MenuItem();
            //    newMenuItem1.Header = item.Title;
            //    cm.ContextMenu.Items.Add(newMenuItem1);
            //}
            var t = dgTranslater.Resources.Values;
            it = 0;
            //_dataList.Add(new DataModel() { Title = "hell", WordDataList = new BindingList<WordModel>() { new WordModel() { WChina = "111111" } } });
            //if (_dataList.Count == 0)
            //{
            //    _dataList = new BindingList<DataModel>();
            //    DataModel t = new DataModel();
            //    _dataList.Add(t);
            //    _dataList.Add(new DataModel() { Title = "hell", WordDataList = new BindingList<WordModel>() { new WordModel() { WChina = "111111" } } });
            //}
            if (_dataList.Count == 0) _dataList.Add(new DataModel() { Title = "hell", WordDataList = new BindingList<WordModel>() { new WordModel() { WChina = "111111" } } });
            dgTitle.ItemsSource = _dataList;

            //_dataList.Add(new DataModel() { Title = "hell", WordDataList = new BindingList<WordModel>() { new WordModel() { WChina = "2222" } } });
        }

        private void _wordDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService.SaveData(_dataList);
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
        private void DG_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.D1) && (dgTranslater.Visibility == Visibility.Visible))
            {
                dgTranslater.Visibility = Visibility.Collapsed;
                gWords.Visibility = Visibility.Visible;
                //lCh.Content = _wordDataList.ElementAtOrDefault(1).WChina;
            }
            else if (e.Key == Key.D1)
            {
                dgTranslater.Visibility = Visibility.Visible;
                gWords.Visibility = Visibility.Collapsed;
            }

            if (e.Key == Key.Right)
            {
                if (++it > toRunFromBindedList.WordDataList.Count - 1)
                {
                    it = 0;
                }
                lCh.Content = toRunFromBindedList.WordDataList.ElementAt(it).WChina;
            }
            if (e.Key == Key.Left)
            {
                if (--it < 0)
                {
                    it = toRunFromBindedList.WordDataList.Count - 1;
                }
                lCh.Content = toRunFromBindedList.WordDataList.ElementAt(it).WChina;
            }
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

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                _fileIOService.SaveData(_dataList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void CMEEE_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            dgBorder = sender as Border;
            
            if(!flg)
            {
                foreach (var item in _dataList)
                {
                    //foreach (var cotexI in .)
                    //{
                    //    if(cotexI.Header == item.Title)
                    //}
                    MenuItem newMenuItem1 = new MenuItem();
                    newMenuItem1.Header = item.Title;
                    dgBorder.ContextMenu.Items.Add(newMenuItem1);
                }
                var f = dgBorder.ContextMenu.Items.GetItemAt(0) as MenuItem;
                var k = f.Header;
                flg = true;

            }
            

        }

        private void ContextMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("hello");
        }

        private void Context_Delete(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            var toDeleteFromBindedList = (DataModel)item.SelectedCells[0].Item;

            //Remove the toDeleteFromBindedList object from your ObservableCollection
            _dataList.Remove(toDeleteFromBindedList);
        }

        private void Context_Run(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget; //DataGrid

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            toRunFromBindedList = (DataModel)item.SelectedCells[0].Item;

            item.Visibility = Visibility.Collapsed;
            dgTranslater.Visibility = Visibility.Visible;

            
            dgTranslater.ItemsSource = _dataList[_dataList.IndexOf(toRunFromBindedList)].WordDataList;
            _dataList[_dataList.IndexOf(toRunFromBindedList)].WordDataList.ListChanged += _wordDataList_ListChanged;
        }
    }
}
