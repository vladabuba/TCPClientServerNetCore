using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPClient
{
    public class Client
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream;

        public Client()
        {
            msg("Client Started >>>");
            clientSocket.Connect("192.168.0.195", 9999);
            Console.WriteLine("Client Socket Program - Server Connected ...");
        }

        public void SendData()
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Message from Client$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
            serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            msg("Data from Server : " + returndata);
        }

        public void msg(string mesg)
        {
            Console.WriteLine(" >> " + mesg);
        }
    }
}
