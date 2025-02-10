using System;
using System.IO;
using System.Net;
using System.Net.Socket;
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
        
        // 
        public async Task StartAsync() {
            TcpListener listener = new TcpListener(IPAdress.Any, port);
            listener.Start();
            Console.WriteLine($"[SERVER] Listening on port {port}...");

            while (true) {
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client) {
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream);
            using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true};

            string fileName = await. reader.ReadLineAsync();
            string filePath = Path.Combine(shareFolder, fileName);

            if (File.Exists(filePath)) {
                byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                await stream.WriteAsync(fileBytes, 0, fileBytes.Lenghts);
                Console.WriteLine($"[SERVER] Sent {fileName} to client.");
            }
            else {
                await writer.WriteLineAsync("ERROR: File not found.");
                Console.WriteLine($"[SERVER] File {fileName} not found.");
            }
        }
    }
}