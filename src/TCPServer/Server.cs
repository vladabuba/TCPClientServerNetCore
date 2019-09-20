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
        private Logger.ILogger log;

        public Server()
        {
            log = new Logger.LoggerToFile();

            TcpListener serverSocket = new TcpListener(9999);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");
            log.AddToLog("S", "Server started.");

            counter = 0;
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                log.AddToLog("I", ("Client No: " + Convert.ToString(counter) + " started!"));
                HandleClient client = new HandleClient(log);
                client.startClient(clientSocket, Convert.ToString(counter));
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            log.AddToLog("F", "Server has exited.");
            Console.ReadLine();
        }
    }
}
