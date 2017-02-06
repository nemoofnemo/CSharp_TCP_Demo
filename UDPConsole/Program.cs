using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ep = null;
            Socket sock = null;

            ep = new IPEndPoint(IPAddress.Any, 6002);
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.Bind(ep);

            IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
            EndPoint tempRemoteEP = (EndPoint)client;
            byte[] buffer = new byte[1024];

            while (true)
            {
                int recv = sock.ReceiveFrom(buffer, 1024, 0, ref tempRemoteEP);
                Console.WriteLine("recv {0} : {1}", Encoding.ASCII.GetString(buffer, 0, recv), tempRemoteEP.ToString());
            }
        }
    }
}
