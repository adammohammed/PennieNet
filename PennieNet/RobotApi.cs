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
    public class RobotApi : ICommander
    {
        string hostip;
        int hostport = 3000;
        string hosturi;
        WebRequest req;
 
        public RobotApi(string ip, int port = 9005)
        {
            hostip = ip;
            hostport = port;

            hosturi = "http://"+ hostip + ":" + hostport.ToString() + "/";
        }

        public async void IssueCmd(string url)
        {
            using(var c = new HttpClient())
            {
                c.BaseAddress = new Uri(hosturi);
                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage resp = await c.GetAsync("api/" + url);
                }
                catch (WebException)
                {
                    return;
                }
            }
        }
        public void Dispose()
        {
            //this does nothing;
        }
    }
}
