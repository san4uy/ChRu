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
        private readonly string PATH;
        public FileIOService(string path)
        {
            PATH = path;
        }
        public BindingList<WordModel> LoadData()
        {
            var fileExists = File.Exists(PATH);
            if(!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new BindingList<WordModel>();
            }
            using (var reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<WordModel>>(fileText);
            }
        }
        public void SaveData(object wordDataList)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(wordDataList);
                writer.WriteLine(output);
            }
        }
    }
}
