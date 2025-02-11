using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Core {
    // Client Class
    public class Client {
        private readonly string serverIP;
        private readonly int serverPort;

        public Client(string serverIP, int serverPort) {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
        }

        public async Task DownloadFileAsync(string fileName, string savePath) {
            try {
                using TcpClient client = new TcpClient();
                await client.ConnectAsync(serverIP, serverPort);
                using NetworkStream stream = client.GetStream();
                using StreamReader reader = new StreamReader(stream);
                using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

                // Send file request
                await writer.WriteLineAsync(fileName);

                // Wait for server response
                string response = await reader.ReadLineAsync();
                if (response != "OK") {
                    Console.WriteLine($"[CLIENT] {response}");
                    return;
                }

                // Read file data and save to disk
                using FileStream fileStream = new FileStream(savePath, FileMode.Create);
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                }

                Console.WriteLine($"[CLIENT] File '{fileName}' downloaded successfully.");
            }
            catch (Exception ex) { 
                Console.WriteLine($"[CLIENT] Error: {ex.Message}");
            }
        }
    }
}