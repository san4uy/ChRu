using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace testA
{
    internal class ExtractSound
    {
        private static readonly HttpClient client = new HttpClient();
        private string _enco;
        private string _code;
        public string Enco
        {
            get
            {
                return HttpUtility.UrlEncode(_enco);
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
                return HttpUtility.UrlDecode(_code);
            }
            set
            {
                _code = value;
            }
        }
        public async Task<string> GetCodeAsync(string str)
        {
            var values = new Dictionary<string, string>
            {
                { "f.req", "%5B%5B%5B%22jQ1olc%22%2C%22%5B%5C%22%E4%BF%84%E7%BD%97%E6%96%AF%5C%22%2C%5C%22zh-CN%5C%22%2Cnull%2C%5C%22undefined%5C%22%5D%22%2Cnull%2C%22generic%22%5D%5D%5D" },
                { "at", "ADiessZ-V9qcF5UZLzb6X_0QfMn-%3A1667665235141" }
            };
            var URL = "https://translate.google.com/_/TranslateWebserverUi/data/batchexecute?rpcids=jQ1olc&source-path=/&f.sid=6148368410739897732&bl=boq_translate-webserver_20221102.06_p0&hl=ru&soc-app=1&soc-platform=1&soc-device=1&_reqid=1869635&rt=c";
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(URL, content);

            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
