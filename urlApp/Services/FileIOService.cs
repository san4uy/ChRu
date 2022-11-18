using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using urlApp.Models;

namespace urlApp.Services
{
    internal class FileIOService
    {
        //private readonly string PATH = $"{Environment.CurrentDirectory}\\wordDataList.json";
        private readonly string PATH;
        public FileIOService()
        {
            PATH = GetPath();
        }
        public BindingList<DataModel> LoadData()
        {
            var fileExists = File.Exists(PATH);
            if(!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new BindingList<DataModel>();
            }
            using (var reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<DataModel>>(fileText);
            }
        }
        public void SaveData(object DataModel)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(DataModel);
                writer.WriteLine(output);
            }
        }
        public string GetPath()
        {
            string st = AppDomain.CurrentDomain.BaseDirectory;
            /////del
            int f = st.LastIndexOf('\\');
            f = st.Remove(f, 1).LastIndexOf('\\');
            f = st.Remove(f, st.Length-f).LastIndexOf('\\');
            f = st.Remove(f, st.Length-f).LastIndexOf('\\');
            st = st.Remove(f, st.Length-f);
            return st.Insert(st.Length, "\\wordDataList.json");
        }
        public string GetFolder()
        {
            string st = AppDomain.CurrentDomain.BaseDirectory;
            /////del
            int f = st.LastIndexOf('\\');
            f = st.Remove(f, 1).LastIndexOf('\\');
            f = st.Remove(f, st.Length-f).LastIndexOf('\\');
            f = st.Remove(f, st.Length-f).LastIndexOf('\\');
            st = st.Remove(f, st.Length-f);
            return st;
        }
    }
}
