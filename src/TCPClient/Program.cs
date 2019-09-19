using System;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Client...");

            if(args.Length < 1)
            {
                Console.WriteLine("Provide filename...");
                return;
            }

            string filename = args[0];

            Console.WriteLine("Filename is: " + filename);

            //string filename = @"C:\temp\torrent\files\Lynda Python Essential Training\01. Introduction\01_01-Welcome.mp4";

            //byte[] fileBytes = System.IO.File.ReadAllBytes(filename);

            //Console.WriteLine(fileBytes.Length);

            //return;

            Client cli = new Client();

            //for (int i = 0; i < 9999; i++)
            //{
                cli.SendData(filename);
            //}

            Console.WriteLine(">>> Finished sending file!");
        }
    }
}
