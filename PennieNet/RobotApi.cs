using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PennieNet
{
    public class RobotApi
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

        public void IssueCmd(string url)
        {
            req = WebRequest.Create(hosturi + url); 
            req.Proxy = null;
            var resp = req.GetResponse(); 
        }
        public void Dispose()
        {
            //this does nothing;
        }
    }
}
