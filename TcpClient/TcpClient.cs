using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

class EmployeeTCPClient
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Using TCP");
        TcpClient client = new TcpClient(Dns.GetHostEntry("localhost").AddressList[0].ToString(), 11000);
        try
        {
            Stream s = client.GetStream();
            StreamReader sr = new StreamReader(s);
            StreamWriter sw = new StreamWriter(s);
            sw.AutoFlush = true;
            Console.WriteLine(sr.ReadLine());
            sw.WriteLine(Console.ReadLine());
            while (true)
            {
                string name = Console.ReadLine();
                sw.WriteLine(name);
                if (name == "") break;
                Console.WriteLine(sr.ReadLine());
            }
            s.Close();
        }
        finally
        {
            // code in finally block is guranteed 
            // to execute irrespective of 
            // whether any exception occurs or does 
            // not occur in the try block
            client.Close();
        }
    }
}