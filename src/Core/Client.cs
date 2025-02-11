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

                // Send file request to server
                await writer.WriteAsync(fileName);

                char[] buffer = new char[5];
                await reader.ReadAsync(buffer, 0, 5);
                string response = new string(buffer);

                if (response == "ERROR") {
                    Console.WriteLine($"[CLIENT] File '{fileName}' not found on server.");
                    return;
                }

                byte[] fileBytes = new byte[1024];
                using FileStream fileStream = new FileStream(savePath, FileMode.Create);

                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(fileBytes, 0, fileBytes.Length)) > 0) {
                    await fileStream.WriteAsync(fileBytes, 0, bytesRead);
                }

                Console.WriteLine($"[CLIENT] File '{fileName}' downloaded successfully.");
            }
            catch (Exception ex) { 
                Console.WriteLine($"[CLIENT] Error: {ex.Message}");
            }
        }
    }
}