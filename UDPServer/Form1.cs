using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDPServer
{
    public partial class Form1 : Form
    {
        private IPEndPoint ep = null;
        private Socket sock = null;

        public Form1()
        {
            InitializeComponent();
        }

        struct ArgItem
        {
            public Socket s;
            public byte[] buf;
            public EndPoint ep;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                ep = new IPEndPoint(IPAddress.Any, Int32.Parse(textBoxPort.Text));
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sock.Bind(ep);
                for(int i = 0;i < 10; ++i)
                {
                    PostRecvFrom();
                }
                
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);

                if(ep != null)
                {
                    ep = null;
                }

                if(sock != null)
                {
                    sock.Close();
                    sock = null;
                }
            }          
        }

        void PostRecvFrom()
        {
            try
            {
                ArgItem arg = new ArgItem();
                IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
                arg.ep = (EndPoint)client;
                arg.buf = new byte[1024];
                arg.s = sock;
                sock.BeginReceiveFrom(arg.buf, 0, 1024, 0, ref arg.ep, RecvCallback, arg);
            }
            catch(Exception ex)
            {
                richTextBox1.AppendText("post recvfrom failled : " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (ep != null)
            {
                ep = null;
            }

            if (sock != null)
            {
                sock.Close();
                sock = null;
            }

            richTextBox1.AppendText("server stopped.");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        public void RecvCallback(IAsyncResult ar)
        {
            try
            {
                ArgItem arg = (ArgItem)ar.AsyncState;
                int recv = arg.s.EndReceiveFrom(ar, ref arg.ep);

                //process data
                richTextBox1.AppendText(Encoding.UTF8.GetString(arg.buf, 0, recv));

                sock.BeginReceiveFrom(arg.buf, 0, 1024, 0, ref arg.ep, RecvCallback, arg);
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
