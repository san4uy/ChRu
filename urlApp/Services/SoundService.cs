using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using urlApp.Models;

namespace urlApp.Services
{
    internal class SoundService
    {
        private string _enco;
        private string _code;
        private MediaPlayer player = new MediaPlayer();
        public string Enco
        {
            get
            {
                return _enco;
            }
            set
            {
                _enco = value;
            }
        }
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }
        public string GetPath ()
        {
            string st = AppDomain.CurrentDomain.BaseDirectory;
            /////del
            int f = st.LastIndexOf('\\');
            f = st.Remove(f, 1).LastIndexOf('\\');
            f = st.Remove(f, st.Length - f).LastIndexOf('\\');
            f = st.Remove(f, st.Length - f).LastIndexOf('\\');
            st = st.Remove(f, st.Length - f);
            return st.Insert(st.Length, "\\music.mp3");
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
            st += "\\mp3\\";
            return st;
        }
        public void SaveSound(string word, string path)
        {
            string UrlSound = GetUrlSound(word);
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
            {
                writer.Write(System.Convert.FromBase64String(UrlSound));
            }
        }

        public string GetUrlSound(string str)
        {
            var URL = "https://translate.google.com/_/TranslateWebserverUi/data/batchexecute?rpcids=jQ1olc&source-path=/&f.sid=6148368410739897732&bl=boq_translate-webserver_20221102.06_p0&hl=ru&soc-app=1&soc-platform=1&soc-device=1&_reqid=1869635&rt=c";
            var prefix = "[[[\"jQ1olc\",\"[\\\"";
            var suffix = "\\\",\\\"zh-CN\\\",null,\\\"undefined\\\"]\",null,\"generic\"]]]";
            string responseInString;
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["f.req"] = prefix + str + suffix;
                data["at"] = "ADiessZ-V9qcF5UZLzb6X_0QfMn-%3A1667665235141";

                var response = wb.UploadValues(URL, "POST", data);
                responseInString = Encoding.UTF8.GetString(response);
            }
            Regex regex = new Regex("(?<=\\[\\\\\").+(?=\\\\\"\\])"); //"(?<=\[\\").+(?=\\"\])"
            MatchCollection matches = regex.Matches(responseInString);
            return matches[0].ToString();
        }
        public void PlaySound(string word, string title)
        {
            string PATH = GetFolder() + title + "\\" + word + ".mp3";
            player.Open(new Uri(PATH, UriKind.RelativeOrAbsolute));
            player.Play();
        }
        public void CheckMp3Folder(ref DataModel dataModel)
        {
            string PATH = GetFolder() + dataModel.Title;
            if (!Directory.Exists(PATH)) Directory.CreateDirectory(PATH);
            string[] files = System.IO.Directory.GetFiles(PATH).Select(f => Path.GetFileNameWithoutExtension(f)).ToArray();
            bool flg = false;
            foreach (var worddata in dataModel.WordDataList)
            {
                for (int x = 0; x < files.Length; x++)
                {
                    if(worddata.WChina == files[x])
                    {
                        flg = true;
                        break;
                    }
                }
                if (!flg)
                {
                    SaveSound(worddata.WChina, PATH + "\\" + worddata.WChina + ".mp3");
                }
            }

        }
    }
}
