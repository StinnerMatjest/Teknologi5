using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata;
using TcpServer;

class EmployeeTCPServer
{

    private const int PORT = 11000;
    private const int LIMIT = 5;     //5 concurrent clients

    static TcpListener listener;

    private static List<string> messages = new();
    private static List<Group> groups = new();
    private static object groupLock = new object();
    private static object messagesLock = new object();
    public static void Main()
    {
        listener = new TcpListener(Dns.GetHostEntry("localhost").AddressList[0], PORT);
        listener.Start();

        for (int i = 0; i < LIMIT; i++)
        {
            Thread t = new Thread(new ThreadStart(Service));
            t.Start();
        }
    }
    public static void Service()
    {
        List<String> internalMessages = new();
        Socket soc = listener.AcceptSocket();
        //soc.SetSocketOption(SocketOptionLevel.Socket,
        //        SocketOptionName.ReceiveTimeout,10000);
        Stream s = new NetworkStream(soc);
        StreamReader sr = new StreamReader(s);
        StreamWriter sw = new StreamWriter(s);
        sw.AutoFlush = true; // enable automatic flushing
        sw.WriteLine("SERVER: Who are you?");
        User user = new User();
        user.Name = sr.ReadLine();
        lock (messagesLock)
        {
            messages.Add($"SERVER: {user.Name} has joined the chat");
        }
        

        try
        {



            bool chatOver = false;
            do
            {
                for (int i = internalMessages.Count; i < messages.Count; i++)  //Writes all messages that are not in internalMessages
                {
                    if (i != messages.Count)
                    {
                        int remainingMessages = messages.Count - internalMessages.Count;
                        sw.WriteLine(messages[i] + "|" + remainingMessages);
                        internalMessages.Add(messages[i]);
                    }

                }

                string input = sr.ReadLine();
                if (input == "Exit" || input == "exit") //disconnects the user when user writes "Exit"
                {
                    chatOver = true;
                    sw.WriteLine("Bye");
                }
                else if (input.Contains("cgroup/"))
                {
                    Group group = new Group();
                    group.Members.Add(user);
                    group.Name = input.Split('/').Last();
                    lock (groupLock)
                    {
                        groups.Add(group);
                    }
                    lock (messagesLock)
                    {
                        messages.Add($"SERVER: The group {group.Name} has been created.");
                        messages.Add($"SERVER: {user.Name} has joined {group.Name}.");
                    }
                    sw.WriteLine(group.Name + "entered");
                }
                else if (input.Contains("jgroup/"))
                {
                    bool groupFound = false;
                    lock (groupLock)
                    {
                        foreach (var group in groups)
                        {
                            if (group.Name == input.Split('/').Last())
                            {
                                groupFound = true;
                                group.Members.Add(user);
                                messages.Add($"SERVER: {user.Name} has joined {group.Name}.");
                                sw.WriteLine(group.Name + "entered");
                            }
                        }
                        if (!groupFound)
                        {
                            sw.WriteLine("Group name does not exist");
                        }
                    }
                }
                else if (input != "")
                {
                    lock (messagesLock)
                    {

                        messages.Add($"{user.Name}: {input}");
                    }
                }
            } while (!chatOver);
            s.Close();
        }
        catch (Exception e)
        {
            soc.Close();
        }

    }

    public static void BroadcastMessage(string input)
    {

    }
}