using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace testA
{
    internal class ExtractSound
    {
        private string _enco;
        private string _code;
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

        public string GetCSound()
        {
            var URL = "https://translate.google.com/_/TranslateWebserverUi/data/batchexecute?rpcids=jQ1olc&source-path=/&f.sid=6148368410739897732&bl=boq_translate-webserver_20221102.06_p0&hl=ru&soc-app=1&soc-platform=1&soc-device=1&_reqid=1869635&rt=c";

            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["f.req"] = "[[[\"jQ1olc\",\"[\\\"俄罗斯\\\",\\\"zh-CN\\\",null,\\\"undefined\\\"]\",null,\"generic\"]]]";
                data["at"] = "ADiessZ-V9qcF5UZLzb6X_0QfMn-%3A1667665235141";

                var response = wb.UploadValues(URL, "POST", data);
                string responseInString = Encoding.UTF8.GetString(response);
                return responseInString;
            }
        }
        private string CutCsound(string str)
        {
            Regex regex = new Regex("(?<=\\[\\\\\").+(?=\\\\\"\\])"); //"(?<=\[\\").+(?=\\"\])"
            MatchCollection matches = regex.Matches(str);
            return matches[0].ToString();
        }
    }
}
