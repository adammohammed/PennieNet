using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;


namespace PennieNet
{
    public class Commander : ICommander
    {
        private BluetoothAddress bAddr;
        private BluetoothClient bc;
        private Guid serviceClass;
        private BluetoothEndPoint endPoint;
        private Stream bluetoothStream;

        public Commander(string id="984FEE041791")
        {
            bAddr = BluetoothAddress.Parse(id);
            serviceClass = BluetoothService.SerialPort;
            endPoint = new BluetoothEndPoint (bAddr, serviceClass);
            bc = new BluetoothClient();
            try
            {
                bc.Connect(endPoint);
                bluetoothStream = bc.GetStream();
            }catch (Exception)
            {
                bc = null;
            }

        }
        public void IssueCmd(string input)
        {
            char[] output = new char[1];
            //char output = 'p';
            if (input.Equals("left"))
            {
                output[0] = 'a';
            }
            else if (input.Equals("right"))
            {
                output[0] = 'd';
            }
            else if (input.Equals("fwd"))
            {
                output[0] = 'w';
            }
            else if (input.Equals("rev"))
            {
                output[0] = 's';
            }
            else
            {
                output[0] = 'p';
            }

            if (bluetoothStream != null)
            {
                bluetoothStream.Write(Encoding.Unicode.GetBytes(output), 0, 1);
            }
        }


        public void Dispose()
        {
            bc.Dispose();
            bc = null;
        }
    }
}
