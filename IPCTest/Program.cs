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

                using (NetworkStream sw = client.GetStream())
                {
                    byte[] buff = Encoding.UTF8.GetBytes("train,1213,123,123,123");
                    byte[] recvbuf = new byte[client.ReceiveBufferSize];
                    sw.Write(buff, 0, buff.Length);
                    if (sw.CanRead)
                    {
                        Console.WriteLine(sw.Read(recvbuf, 0, client.ReceiveBufferSize));
                    }
                }

            }
        }
    }
}
