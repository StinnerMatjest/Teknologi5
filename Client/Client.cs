// A C# program for Client
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Client
{

    class Client
    {

        // Main Method
        static void Main(string[] args)
        {
            StartClient();
        }

        public static void StartClient()
        {
            byte[] bytes = new byte[1024];
            bool isalive = true;
            while (isalive)
            {
                try
            {
                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    // Connect to Remote EndPoint
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());
                    Console.WriteLine("Type message to send:");

                    SendMessage(sender);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            }
             void SendMessage(Socket sender)
            {
                string text = Console.ReadLine();
                // Encode the data string into a byte array.
                byte[] msg = Encoding.ASCII.GetBytes(text +
                "<EOF>");
                // Send the data through the socket.
                int bytesSent = sender.Send(msg);
            }
        }
    }
}
