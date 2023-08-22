using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace P_WSListener
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            Console.WriteLine("Starting TCP listener...");

            TcpListener listener = new TcpListener(ipAddress, 500);

            listener.Start();


            while (true)
            {
                

                Socket client = listener.AcceptSocket();

                WebClient wb = new WebClient();

                

                Console.WriteLine("Connection accepted.");

                var childSocketThread = new Thread(() =>
                {
                    byte[] data = new byte[100000];
                    int size = client.Receive(data);
                    Console.WriteLine("Recieved data: ");
                    for (int i = 0; i < size; i++)
                        Console.Write(Convert.ToChar(data[i]));

                    Console.WriteLine();

                    client.Close();
                });
                childSocketThread.Start();
            }

            
			
           
        }
    }
}
