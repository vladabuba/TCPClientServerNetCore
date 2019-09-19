using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPServer
{
    public class Server
    {
        public Server()
        {
            TcpListener serverSocket = new TcpListener(9999);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");

            counter = 0;
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                handleClinet client = new handleClinet();
                client.startClient(clientSocket, Convert.ToString(counter));
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }

        //Class to handle each client request separatly
        public class handleClinet
        {
            TcpClient clientSocket;
            string clNo;
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
                        Console.WriteLine(" >> IP address: " +((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address.ToString());

                        var watch = System.Diagnostics.Stopwatch.StartNew();

                        requestCount = requestCount + 1;
                        NetworkStream networkStream = clientSocket.GetStream();

                        byte[] lengthBytes = new byte[4];
                        int read = networkStream.Read(lengthBytes, 0, 4);
                        // read contains the number of read bytes, so we can check it if we want
                        int length = BitConverter.ToInt32(lengthBytes);
                        byte[] buf = new byte[length];
                        networkStream.Read(buf, 0, buf.Length);

                        Console.WriteLine(" >> " + "From client-" + clNo + " filesize " + buf.Length);

                        System.IO.File.WriteAllBytes(@"c:\temp\src\myfile.bin", buf);

                        //networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                        //dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                        //dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                        //Console.WriteLine(" >> " + "From client-" + clNo + dataFromClient);

                        rCount = Convert.ToString(requestCount);
                        serverResponse = "Server to clinet(" + clNo + ") " + rCount;
                        sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
                        Console.WriteLine(" >> " + serverResponse);
                        // the code that you want to measure comes here
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        Console.WriteLine(" Execution time >> " + elapsedMs.ToString());
                        workOK = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" >> " + ex.ToString());
                        workOK = false;
                    }
                }
            }
        }
    }
}
