using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Core {
    // Server Class
    public class Server {
        private readonly int port;
        private readonly string shareFolder;

        // Init Server
        public Server(int port, string shareFolder) {
            this.port = port;
            this.shareFolder = shareFolder;
        }
        
        public async Task StartAsync() {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"[SERVER] Listening on port '{port}'...");

            while (true) {
                // Accept incoming client
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client); // Handle client in parallel
            }
        }

        private async Task HandleClientAsync(TcpClient client) {
            try {
                // Create stream, reader, and writer for communication
                using NetworkStream stream = client.GetStream();
                using StreamReader reader = new StreamReader(stream);
                using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

                // Read file request from client
                string fileName = await reader.ReadLineAsync();
                string filePath = Path.Combine(shareFolder, fileName);

                if (File.Exists(filePath)) {
                    // Notify the client that the file exists
                    await writer.WriteLineAsync("OK");
                    await writer.FlushAsync(); // Ensure the message is sent

                    // Read file and send in chunks
                    byte[] buffer = new byte[1024];
                    using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    int bytesRead;
                    
                    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                        await stream.WriteAsync(buffer, 0, bytesRead);
                    }

                    Console.WriteLine($"[SERVER] Sent '{fileName}' to client.");
                } 
                else {
                    // File not found, send error message
                    await writer.WriteLineAsync("ERROR: File not found.");
                    await writer.FlushAsync();
                    Console.WriteLine($"[SERVER] File '{fileName}' not found.");
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"[SERVER] Error: {ex.Message}");
            }
        }
    }
}
