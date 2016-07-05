using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace IPCTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();

            client.Connect("localhost", 3050);
            while (client.Connected)
            {
                Console.Write("Enter a test val: ");
                var str = Console.ReadLine();

                using (StreamWriter sw = new StreamWriter(client.GetStream()))
                {
                    sw.WriteLine(str);
                    using (StreamReader sr = new StreamReader(client.GetStream()))
                    {
                        Console.WriteLine(sr.ReadLine());
                    }
                }

            }
        }
    }
}
