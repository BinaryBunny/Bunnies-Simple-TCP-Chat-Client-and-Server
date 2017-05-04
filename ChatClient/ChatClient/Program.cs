using System;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    class Program
    {
        //Declare a port that we will be using for the TcpClient to connect to
        const int port = 1273;
        //Declare the server IP that TcpClient will be connecting to.
        const string server_ip = "127.0.0.1";

        static void Main(string[] args)
        {
            Console.Title = "Bunnies Chat Client";
            Console.ForegroundColor = ConsoleColor.Green;

            //Create the message we are going to send.
            string texttoSend = DateTime.Now.ToString();
            
            //Create the TCP client that will be used to connect with.
            TcpClient client = new TcpClient(server_ip, port);

            //Create a network stream to get all the data that comes and goes through the client.
            NetworkStream nwStream = client.GetStream();

            //Convert out string message to a byteArray because we will send it as a buffer later.
            byte[] bytesToSend = Encoding.ASCII.GetBytes(texttoSend);

            //Write out to the console what we are sending.
            Console.WriteLine("Sending: " + texttoSend);

            //Use the networkstream to send the byteArray we just declared above, start at the offset of zero, and the size of the packet we are sending is the size of the messages length.
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            //Recieve the bytes that are coming from the other end (server) through the client and store them in an array.
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            
            //read the bytes, starting from the offset 0, and the size is what ever the client has recieved.
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

            //Decode the bytes we just recieved using the Encoding.ASCII.GetString function and give it the correct parameters
            //1. What it should decode
            //2. Starting to decode from what offset
            //3. How much do we want to decode?
            Console.WriteLine("Recieved: " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            Console.ReadLine();
            
            //Close the client so we're not leaving it open for people to eavesdrop.
            client.Close();
        }
    }
}
