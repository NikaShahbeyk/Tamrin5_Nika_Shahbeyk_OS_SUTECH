using System;
using System.IO;
using System.Net.Sockets;

namespace FileTransferClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Connect to the server
                TcpClient client = new TcpClient("127.0.0.1", 5000);
                Console.WriteLine("Connected to server.");

                while (client.Connected)
                {
                    // Prompt user to enter the file path
                    Console.WriteLine("Enter the path of the file to send (or type 'exit' to quit):");
                    string filePath = Console.ReadLine();

                    // If user types 'exit', close the client and break the loop
                    if (filePath.ToLower() == "exit")
                    {
                        client.Close();
                        break;
                    }

                    // Read file and send to server
                    using (FileStream fileStream = File.OpenRead(filePath))
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stream.Write(buffer, 0, bytesRead); // Send file bytes to the server
                        }
                    }

                Console.WriteLine("File sent successfully.");
                TcpClient client1 = new TcpClient("127.0.0.1", 5000);


                    // Receive file from server
                    using (NetworkStream stream = client1.GetStream())
                    using (var fileStream = File.Create("received_file_from_server.txt")) // Create a file
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, bytesRead); 
                        }
                    }

                    Console.WriteLine("File received from server successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        Console.WriteLine("enter something to exit: ");
        Console.ReadKey();
        }
    }
}
