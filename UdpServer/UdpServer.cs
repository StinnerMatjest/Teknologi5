using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;

class UdpServer
{
    public static void Main()
    {
        const int port = 11000;
        UdpClient udpc = new UdpClient(port);
        Console.WriteLine($"Server started, servicing on port {port}");
        IPEndPoint ep = null;
        while (true)
        {
            byte[] rdata = udpc.Receive(ref ep);
            string name = Encoding.ASCII.GetString(rdata);

            byte[] sdata = Encoding.ASCII.GetBytes(name);
            udpc.Send(sdata, sdata.Length, ep);

        }
    }
}