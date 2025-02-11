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
            Console.WriteLine($"[SERVER] Listening on port {port}...");

            while (true) {
                // Accept incoming client
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client); // Handle client in parallel
            }
        }

        private async Task HandleClientAsync(TcpClient client) {
            // Create stream, reader, and writer for communication
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream);
            using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            // Read file request from client
            string fileName = await reader.ReadLineAsync();
            string filePath = Path.Combine(shareFolder, fileName);

            // Check if the file exists and send it
            if (File.Exists(filePath)) {
                byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
                Console.WriteLine($"[SERVER] Sent {fileName} to client.");
            }
            else {
                // File not found, send error message
                await writer.WriteLineAsync("ERROR: File not found.");
                Console.WriteLine($"[SERVER] File {fileName} not found.");
            }
        }
    }
}
