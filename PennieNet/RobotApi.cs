using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PennieNet
{
    class RobotApi
    {
        string hostip;
        int hostport = 3000;
        string hosturi;
        WebRequest req;
        public RobotApi(string ip, int port = 3000)
        {
            hostip = ip;
            hostport = port;

            hosturi = hostip + ":" + hostport.ToString() + "/api/";
        }

        public async void IssueCmd(string url)
        {
            using(var c = new HttpClient())
            {
                c.BaseAddress = new Uri(hosturi);
                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage resp = await c.GetAsync(url);
            }
        }
        public void Dispose()
        {
            //this does nothing;
        }
    }
}
