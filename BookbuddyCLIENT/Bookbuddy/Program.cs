
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class BookBuddyClient
{
    public static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.Error.WriteLine("Usage: KnockKnockClient <host name> <port number>");
            Environment.Exit(1);
        }

        string hostName = args[0];
        if (!int.TryParse(args[1], out int portNumber))
        {
            Console.Error.WriteLine("Port number moet een getal zijn.");
            Environment.Exit(1);
        }

        try
        {
            using TcpClient kkClient = new TcpClient(hostName, portNumber);
            using NetworkStream stream = kkClient.GetStream();
            using StreamWriter outWriter = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            using StreamReader inReader = new StreamReader(stream, Encoding.UTF8);
            using StreamReader stdIn = new StreamReader(Console.OpenStandardInput());

            string fromServer;
            string fromUser;

            while ((fromServer = inReader.ReadLine()) != null)
            {
                Console.WriteLine("Server: " + fromServer);

                if (fromServer == "Bye.")
                    break;

                fromUser = stdIn.ReadLine();
                if (fromUser != null)
                {
                    Console.WriteLine("Client: " + fromUser);
                    outWriter.WriteLine(fromUser);
                }
            }
        }
        catch (SocketException)
        {
            Console.Error.WriteLine($"Don't know about host {hostName}");
            Environment.Exit(1);
        }
        catch (IOException)
        {
            Console.Error.WriteLine($"Couldn't get I/O for the connection to {hostName}");
            Environment.Exit(1);
        }
    }
}