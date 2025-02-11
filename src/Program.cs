using System;
using System.Threading.Tasks;
using Core;

class Program {
    static async Task Main() {
        const int port = 5000;

        Console.WriteLine("Start as:");
        Console.WriteLine("Server [1]");
        Console.WriteLine("Client [2]");
        string input = Console.ReadLine();

        if (input == "1") {
            var server = new Server(port, "../test/SharedFiles");
            await server.StartAsync();
        }
        else if (input == "2") {
            Console.WriteLine("Enter Server IP: ");
            string ip = Console.ReadLine();

            Console.WriteLine("Enter file name: ");
            var fileName = Console.ReadLine();

            var client = new Client(ip, port);
            await client.DownloadFileAsync(fileName, $"../test/Downloads/{fileName}");
        }
        else {
            Console.WriteLine("Incorrect input");
        }
    }
}