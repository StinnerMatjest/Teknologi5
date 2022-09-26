using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml.Linq;

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

            Console.WriteLine(sr.ReadLine()); //server spørger efter brugernavn
            string username = Console.ReadLine(); //bruger navn gemmes lokalt
            sw.WriteLine(username); //vi sender serveren vores brugernavn
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            ClearCurrentConsoleLine();


            List<string> messages = new();
            bool chatOver = false;

            while (!chatOver)
            {
                string incoming;
                bool messagesToRead = true;
                do
                {

                    incoming = sr.ReadLine();

                    if (int.Parse(incoming.Split('|').Last()) == 1) //Hvis dette er den sidste besked, er der ikke flere at læse
                    {
                        messagesToRead = false;
                    }
                    if (incoming != "" || incoming != null)
                    {
                        Console.WriteLine(incoming.Split('|').First());

                    }
                    else
                        messagesToRead = false;
                } while (messagesToRead);
                Console.Write(username + ": ");

                string input = Console.ReadLine();
                if (input != "Exit" || input != "exit")
                {
                    sw.WriteLine(input);
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    ClearCurrentConsoleLine();

                }
                else if (input == "") // dette fucker det hele op, skriv altid noget, for nu
                    input = "do nothing";
                //Console.WriteLine(sr.ReadLine());
                else
                    chatOver = true;
            }
            sw.WriteLine(username + " has left the chat");
            s.Close();
        }
        finally
        {
            client.Close();
        }
    }
    public static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
}