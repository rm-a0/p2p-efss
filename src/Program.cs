using System;
using System.Threading.Tasks;
using Core;

class Program {
    static async Task Main() {
        Console.WriteLine("Start as:");
        Console.WriteLine("Server [1]");
        Console.WriteLine("Client [2]");
        string input = Console.ReadLine();

        if (input == "1") {
            var server = new Server(5000, "../test/SharedFiles");
            await server.StartAsync();
        }
        else if (input == "2") {
            Console.WriteLine("Not implemented yet");
        }
        else {
            Console.WriteLine("Incorrect input");
        }
    }
}