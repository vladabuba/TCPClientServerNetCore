using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPServer
{
    public class HandleClient
    {
        TcpClient clientSocket;
        string clNo;
        Logger.ILogger log;

        public HandleClient(Logger.ILogger log)
        {
            this.log = log;
        }

        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;
            bool workOK = true;

            while ((workOK))
            {
                try
                {
                    Console.WriteLine(" >> IP address: " + ((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address.ToString());
                    log.AddToLog("I", "IP address: " + ((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address.ToString());

                    var watch = System.Diagnostics.Stopwatch.StartNew();

                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();

                    byte[] lengthBytes = new byte[4];
                    int read = networkStream.Read(lengthBytes, 0, 4);
                    // read contains the number of read bytes, so we can check it if we want
                    int length = BitConverter.ToInt32(lengthBytes);
                    byte[] buf = new byte[length];
                    networkStream.Read(buf, 0, buf.Length);

                    Console.WriteLine(" >> " + "Got stream from client-" + clNo + " filesize " + buf.Length);
                    log.AddToLog("S", "Got stream from client-" + clNo + " filesize " + buf.Length);

                    System.IO.File.WriteAllBytes(@"c:\temp\src\myfile.bin", buf);

                    Console.WriteLine(" >> File written . ");
                    log.AddToLog("S", "File written.");

                    rCount = Convert.ToString(requestCount);
                    serverResponse = "Server to clinet(" + clNo + ") " + rCount;
                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    Console.WriteLine(" >> " + serverResponse);
                    log.AddToLog("S", " >> " + serverResponse);

                    // the code that you want to measure comes here
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    Console.WriteLine(" Execution time >> " + elapsedMs.ToString() + "ms");
                    log.AddToLog("S", " Execution time >> " + elapsedMs.ToString() + "ms");
                    workOK = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                    log.AddToLog("S", " >> " + ex.ToString());
                    workOK = false;
                }
            }
        }
    }
}
