using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PennieNet;

namespace BluetoothTests
{
    class Program
    {
        static Commander cmd;
        static void Main(string[] args)
        {
            cmd = new Commander();
            cmd.Connect();
            for (;;)
            {
                if (cmd != null)
                {
                    var data = Console.ReadLine();
                    cmd.IssueCmd(data);
                }
            }
        }
    }
}
