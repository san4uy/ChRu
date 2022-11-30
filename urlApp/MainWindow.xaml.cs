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
using System.Windows.Markup;
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
        private List<WordModel> _DataModelList;
        private Border dgBorder;
        private FileIOService _fileIOService;
        private SoundService soundService;
        private int it;
        private bool nPlay = false;
        private bool flg = false;
        private static Random rng = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOService = new FileIOService();
            soundService = new SoundService();
            try
            {
                _dataList = _fileIOService.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }

            var t = dgTranslater.Resources.Values;
            it = 0;
            //if (_dataList.Count == 0) _dataList.Add(new DataModel() { Title = "hell", WordDataList = new BindingList<WordModel>() { new WordModel() { WChina = "111111" } } });
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
                try
                {
                    soundService.CheckMp3Folder(ref toRunFromBindedList);
                    _DataModelList = toRunFromBindedList.WordDataList.ToList();
                    lCh.Content = _DataModelList.ElementAt(0).WChina;
                    lRu.Content = "";
                    lTr.Content = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }

            }
            else if (e.Key == Key.D1)
            {
                dgTranslater.Visibility = Visibility.Visible;
                gWords.Visibility = Visibility.Collapsed;
            }
            if (gWords.Visibility == Visibility.Visible)
            {
                if (e.Key == Key.Right)
                {
                    
                    if (++it > _DataModelList.Count - 1)
                    {
                        it = 0;
                    }
                    lCh.Content = _DataModelList.ElementAt(it).WChina;
                    lRu.Content = "";
                    lTr.Content = "";
                    if (nPlay)  soundService.PlaySound(_DataModelList.ElementAt(it).WChina, toRunFromBindedList.Title);
                }
                if (e.Key == Key.Left)
                {
                    if (--it < 0)
                    {
                        it = _DataModelList.Count - 1;
                    }
                    lCh.Content = _DataModelList.ElementAt(it).WChina;
                    lRu.Content = "";
                    lTr.Content = "";
                    if(nPlay) soundService.PlaySound(_DataModelList.ElementAt(it).WChina, toRunFromBindedList.Title);
                }
                if (e.Key == Key.Down)
                {
                    lRu.Content = _DataModelList.ElementAt(it).WRussia;
                    lTr.Content = _DataModelList.ElementAt(it).WTranscription;
                    soundService.PlaySound(_DataModelList.ElementAt(it).WChina, toRunFromBindedList.Title);
                }
                if (e.Key == Key.Up)
                {
                    soundService.PlaySound(_DataModelList.ElementAt(it).WChina, toRunFromBindedList.Title);
                }
                if(e.Key == Key.F1)
                {
                    nPlay = !nPlay;
                }
                if (e.Key == Key.F2)
                {
                    _DataModelList = _DataModelList.OrderBy(a => rng.Next()).ToList();
                    //var shuffleWoList = _DataModelList.OrderBy(x => x.CreationDate).ToList();

                    it = 0;
                    lCh.Content = _DataModelList.ElementAt(it).WChina;
                    lRu.Content = "";
                    lTr.Content = "";
                    if (nPlay) soundService.PlaySound(_DataModelList.ElementAt(it).WChina, toRunFromBindedList.Title);
                }
                
                if (e.Key == Key.F3)
                {
                    if (soundService.wavFlg)
                    {
                        soundService.wplayer.Stop();
                        soundService.wavFlg = false;
                    }
                    else soundService.PlayWav();
                    
                }
            }

            
            if (e.Key == Key.Space)
            {

            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                if (tbFind.Visibility == Visibility.Visible)
                {
                    tbFind.Visibility = Visibility.Collapsed;
                }
                else
                {
                    tbFind.Visibility = Visibility.Visible;
                    tbFind.Focus();
                }
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
            //var shuffleWoList = toRunFromBindedList.WordDataList.OrderBy(x => x.CreationDate).ToList();
            //toRunFromBindedList.WordDataList.Clear();
            //foreach (var word in shuffleWoList)
            //    toRunFromBindedList.WordDataList.Add(word);
            //shuffleWoList.Clear();
            try
            {
                _fileIOService.SaveData(_dataList);
#if !DEBUG
                string folder = _fileIOService.GetFolder();
                string strCmdText = folder.Insert(folder.Length, "\\git_push.bat");
                System.Diagnostics.Process.Start("CMD.exe", strCmdText.Insert(0, "/C" ));
#endif
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
            tbFind.Visibility = Visibility.Collapsed;
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

            
            dgTranslater.ItemsSource = toRunFromBindedList.WordDataList;
            
            toRunFromBindedList.WordDataList.ListChanged += _wordDataList_ListChanged;
            
        }

        private void tbFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtered = _dataList.Where(DataModel => DataModel.Title.StartsWith(tbFind.Text));

            dgTitle.ItemsSource = filtered;
        }
    }
    

}
