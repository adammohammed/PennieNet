using System;
using System.Collections.Generic;
using System.Linq;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.IO;

namespace PennieNet
{
    public class Commander
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
            char output;
            if (input.Equals("left"))
            {
                output = 'a';
            }
            else if (input.Equals("right"))
            {
                output = 'd';
            }
            else if (input.Equals("fwd"))
            {
                output = 'w'; 
            }
            else if (input.Equals("rev"))
            {
                output = 's'; 
            }
            else
            {
                output = 'p';
            }

            if (bluetoothStream != null)
            {
                using (var c = new StreamWriter(bluetoothStream))
                {
                    c.Write(output);
                }
            }
        }


        public void Dispose()
        {
            bc.Dispose();
            bc = null;
        }
    }
}
