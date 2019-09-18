using System;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Client...");
            Client cli = new Client();
            for (int i = 0; i < 9999; i++)
            {
                cli.SendData();
            }
        }
    }
}
