using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
class EmployeeUDPClient2
{
    public static void Main(string[] args)
    {
        UdpClient udpc = new UdpClient(Dns.GetHostEntry("localhost").AddressList[0].ToString(), 11000);
        IPEndPoint ep = null;
        while (true)
        {
            string input = Console.ReadLine();

            byte[] sdata = Encoding.ASCII.GetBytes(input);
            udpc.Send(sdata, sdata.Length);
            byte[] rdata = udpc.Receive(ref ep);
            string job = Encoding.ASCII.GetString(rdata);
            Console.WriteLine(job);

            //Console.Write("Name: ");
            //string name = Console.ReadLine();
            //if (name == "") break;
            //byte[] sdata = Encoding.ASCII.GetBytes(name);
            //udpc.Send(sdata, sdata.Length);
            //byte[] rdata = udpc.Receive(ref ep);
            //string job = Encoding.ASCII.GetString(rdata);
            //Console.WriteLine(job);
        }
    }
}