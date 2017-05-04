using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    class Program
    {
        //Declare the Port & IP that the tcpListener will use to listen for incoming connections on.
        //- Note: The TCPListener listens for incoming connections on the specified port & IP.
        const int port = 1273;
        const string serverIP = "127.0.0.1";

        static void Main(string[] args)
        {
            Console.Title = "Bunnies Chat Server";
            Console.ForegroundColor = ConsoleColor.Green;

            //Convert our string IP to an acutual IP adress instance.
            IPAddress localAdress = IPAddress.Parse(serverIP);

            //Inizialize a new isntance of the TcpListener so that the application can listen for incoming calls / connections.
            //- Note: The TcpListener is currently offline, we are mearly just giving it a few settings aka parameters / overloads.
            TcpListener listener = new TcpListener(localAdress, port);
            Console.WriteLine("Listening..");
            
            //Start the TcpListener with the overloads we gave it earlier.
            listener.Start();

            //Inizialize a new instance of the TcpClient so we can provide client connections to connect with our client.
            //- Note: We are establishing a connection by using the listener and accepting the pending connection requests.
            //- Since the client is trying to connect to the server -> the client is therefor sending a request to the server.
            TcpClient client = listener.AcceptTcpClient();

            //Inizialize an instance of the NetworkStream so that we can get the stream that is coming from the client connection.
            //- Note: We can now accept the buffer (MessageBoxes, Messages, Send Pictures) using the nwStream.
            NetworkStream nwStream = client.GetStream();

            //Create a byte array (a buffer). This will hold the byte size that we recieve from the client in memory.
            byte[] buffer = new byte[client.ReceiveBufferSize];

            //Now we need to read the bytes and store the bytes we read in a int, we do this by using our nwStream.Read function and pass it the correct parameters.
            //1. [Buffer] - an array of type byte, and we declared that above [buffer] <- This is what we are reading from.
            //2. [Offset] - Now we need to set the offset, where we want to start reading in the buffer. so since its an array we start at 0.
            //3. [Size] - The number of bytes we want to read from the NetworkStream.
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            //Now we need to decode the message we recieved by using the Encoding.ASCII.GetString to get the string and passing the correct parameters.
            //1. [Bytes] - What we want to decode, this is where we give it a byte array 
            //2. [Index] - We need to give it the first index of the array that we want to decode so it knows where to start, we do this bya dding 0 since its an array.
            //3. [Count] - The number of bytes we want to decode and we created an int to hold that number above so let's pass it as a parameter.
            string dataRecieved = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Recieved: " + dataRecieved);

            Console.WriteLine("Sending back : " + dataRecieved);
            nwStream.Write(buffer, 0, bytesRead);
            client.Close();
            listener.Stop();
            Console.ReadLine();
        }
    }
}
