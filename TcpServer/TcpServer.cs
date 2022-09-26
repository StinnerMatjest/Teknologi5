using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

class EmployeeTCPServer
{
    static TcpListener listener;
    const int LIMIT = 5; //5 concurrent clients

    public static void Main()
    {

        const int port = 11000;
        listener = new TcpListener(Dns.GetHostEntry("localhost").AddressList[0], 11000);
        listener.Start();

        for (int i = 0; i < LIMIT; i++)
        {
            Thread t = new Thread(new ThreadStart(Service));
            t.Start();
        }
    }
    public static void Service()
    {
        while (true)
        {
            Socket soc = listener.AcceptSocket();
            //soc.SetSocketOption(SocketOptionLevel.Socket,
            //        SocketOptionName.ReceiveTimeout,10000);

            try
            {
                Stream s = new NetworkStream(soc);
                StreamReader sr = new StreamReader(s);
                StreamWriter sw = new StreamWriter(s);
                sw.AutoFlush = true; // enable automatic flushing
                sw.WriteLine("Who are you?");
                string name = sr.ReadLine();
                bool chatOver = false;
                do {
                    string input = sr.ReadLine();
                    if (input == "Exit" || input == "exit")
                    {
                        chatOver = true;
                        sw.WriteLine("Bye");
                    }
                    else
                    {
                        sw.WriteLine($"{name}: {input}");
                    }
                } while (!chatOver);
                s.Close();
            }
            catch (Exception e)
            {
                soc.Close();
            }
        }
    }

    public static void BroadcastMessage(string input)
    {

    }
}