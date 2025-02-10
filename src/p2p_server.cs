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
    }
}