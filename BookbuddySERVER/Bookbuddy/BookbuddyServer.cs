namespace BookbuddyVS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;



public class BookbuddyServer
{
    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: KnockKnockServer <port number>");
            Environment.Exit(1);
        }

        if (!int.TryParse(args[0], out int portNumber))
        {
            Console.Error.WriteLine("Port number must be an integer.");
            Environment.Exit(1);
        }

        try
        {
            TcpListener server = new TcpListener(IPAddress.Any, portNumber);
            server.Start();
            Console.WriteLine($"KnockKnockServer listening on port {portNumber}...");

            using TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            using NetworkStream stream = client.GetStream();
            using StreamWriter outWriter = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            using StreamReader inReader = new StreamReader(stream, Encoding.UTF8);

            string inputLine, outputLine;

            BookbuddyProtocol BBprotocol = new BookbuddyProtocol();
            outputLine = BBprotocol.ProcessInput(null);
            outWriter.WriteLine(outputLine);

            while ((inputLine = inReader.ReadLine()) != null)
            {
                outputLine = BBprotocol.ProcessInput(inputLine);
                outWriter.WriteLine(outputLine);

                if (outputLine == "Bye.")
                    break;
            }

            server.Stop();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Error: {e.Message}");
            Environment.Exit(1);
        }
    }
}

