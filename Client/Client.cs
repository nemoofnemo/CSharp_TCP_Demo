using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Client
    {
        private const int BUFFER_SIZE = 8192;
        private TcpClient tcpClient;

        public Client()
        {
            try
            {
                tcpClient = new TcpClient("127.0.0.1", 6001);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void SendRequest(ref byte[] arr)
        {
            NetworkStream ns = tcpClient.GetStream();
            try
            {
                ns.Write(arr, 0, arr.Length);

                //receive data
                byte[] buffer = new byte[BUFFER_SIZE];
                int count = ns.Read(buffer, 0, BUFFER_SIZE);
                Console.WriteLine("receive {0} bytes.", count);
                Console.WriteLine("response:\n{0}", Encoding.Default.GetString(buffer, 0, count));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                ns.Close();
            }
        }

        public void ShutDown()
        {
            tcpClient.Close();
        }


        static void Main(string[] args)
        {
            Client cl = new Client();
            //byte[] data = Encoding.ASCII.GetBytes("insert;1:hello");
            byte[] data = Encoding.ASCII.GetBytes("select");
            cl.SendRequest(ref data);
            cl.ShutDown();
        }
    }
}
