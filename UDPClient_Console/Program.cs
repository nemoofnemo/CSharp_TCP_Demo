using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPClient_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.2"), 6002);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            string str = "hello server";
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(str);
            sock.SendTo(data, ip);
        }
    }
}
