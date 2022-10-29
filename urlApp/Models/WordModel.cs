using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urlApp.Models
{
    internal class WordModel: INotifyPropertyChanged
    {
        private string _WChina;
        private string _WTranscription;
        private string _WRussia;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "0";
        public string WChina
        {
            get { return _WChina; }
            set 
            {
                if (_WChina == value)
                    return;
                _WChina = value;
                OnPropertyChanged("WChina");
            }
        }
        

        public string WTranscription
        {
            get { return _WTranscription; }
            set
            {
                if (_WTranscription == value)
                    return;
                _WTranscription = value;
                OnPropertyChanged("WTranscription");
            }


        }
        public string WRussia
        {
            get { return _WRussia; }
            set
            {
                if (_WRussia == value)
                    return;
                _WRussia = value;
                OnPropertyChanged("WRussia");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // аналог if(propertyName!= null)
        }
    }
}
