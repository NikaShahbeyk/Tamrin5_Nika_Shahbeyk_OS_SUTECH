//Nika Shahbeyk_Shiraz University Of technology_Student ID:400213013
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = null;
            try
            {
                // Start listening on the specified port
                listener = new TcpListener(IPAddress.Any, 5000);
                listener.Start();
                Console.WriteLine("Server is listening and waiting for a client....");

                while (true)
                {
                    // Accept incoming connection
                    using (TcpClient client = listener.AcceptTcpClient())
                    {
                        Console.WriteLine("Client connected..");

                        // Receive file from client
                        using (NetworkStream stream = client.GetStream())
                        using (var fileStream = File.Create("received_file.txt")) // Create a file
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead;
                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fileStream.Write(buffer, 0, bytesRead); 
                            }
                        }

                        Console.WriteLine("File received from client successfully.");

                        Console.WriteLine("Enter the path of the file to send back to client:");
                        string filePath = Console.ReadLine();

                        // Read file and send to client
                        using (FileStream fileStream = File.OpenRead(filePath))
                        using (NetworkStream stream = client.GetStream())
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead;
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                stream.Write(buffer, 0, bytesRead); // Send file bytes to the client
                            }
                        }

                        Console.WriteLine("File sent back to client successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                listener?.Stop();
            }
            Console.WriteLine("enter something to exit: ");
            Console.ReadKey();
        }
    }
}
