using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Communication
{
    class ServerModule
    {
        private const int BUFFER_SIZE = 8192;
        private enum MessageType { ACCEPT, RECEIVE, SEND, ERROR};
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;

        /// <summary>
        /// constructor
        /// </summary>
        public ServerModule()
        {
            ipEndPoint = new IPEndPoint(IPAddress.Any, 6001);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(100);
        }

        /// <summary>
        /// 
        /// </summary>
        public void run()
        {
            //post Accept
            Console.WriteLine("Server start");
            for (int i = 0; i < 100; ++i)
            {
                serverSocket.BeginAccept(BUFFER_SIZE, AcceptCallback, serverSocket);
            }

            while (true)
            {
                //todo:monitor socket
                //Console.WriteLine("in main thread");
                System.Threading.Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        struct HandlerArgs
        {
            public Socket socket;
            public byte[] buffer;
            public int length;
            public MessageType msgType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket listener = (Socket)ar.AsyncState;
                HandlerArgs args = new HandlerArgs();

                //Socket handler = listener.EndAccept(out Buffer, out bytesTransferred, ar);
                args.socket = listener.EndAccept(out args.buffer, out args.length, ar);
                args.msgType = MessageType.ACCEPT;
                Console.WriteLine("accept a new connection");

                //post accept
                listener.BeginAccept(BUFFER_SIZE, AcceptCallback, listener);

                //process first data. todo:combine data
                string stringTransferred = Encoding.ASCII.GetString(args.buffer, 0, args.length);
                Console.WriteLine(stringTransferred);

                //response
                string str = "hello client";
                byte[] data = Encoding.UTF8.GetBytes(str);
                args.socket.BeginSend(data, 0, str.Length, 0, new AsyncCallback(SendCalback), args);

                //receive remain data
                args.msgType = MessageType.RECEIVE;
                args.socket.BeginReceive(args.buffer, 0, BUFFER_SIZE, 0, new AsyncCallback(ReceiveCallback), args);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                HandlerArgs args = (HandlerArgs)ar.AsyncState;
                Socket s = args.socket;
                int read = s.EndReceive(ar);

                if (read > 0)
                {
                    Console.WriteLine("recv data");
                    string stringTransferred = Encoding.ASCII.GetString(args.buffer, 0, args.length);
                    Console.WriteLine(stringTransferred);

                    //response
                    string str = "hello client";
                    byte[] data = Encoding.UTF8.GetBytes(str);
                    args.socket.BeginSend(data, 0, str.Length, 0, new AsyncCallback(SendCalback), args);

                    args.socket.BeginReceive(args.buffer, 0, BUFFER_SIZE, 0, new AsyncCallback(ReceiveCallback), args);
                }
                else
                {
                    Console.WriteLine("close a connection");
                    s.Close();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCalback(IAsyncResult ar)
        {
            try
            {
                HandlerArgs args = (HandlerArgs)ar.AsyncState;
                Socket s = args.socket;
                int count = s.EndSend(ar);
                Console.WriteLine("send {0} bytes.", count);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private static void MessageHandler(object obj)
        {
            HandlerArgs args = (HandlerArgs)obj;

            //post item to threadpool
            //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(MessageHandler), args);
        }
    }
}
