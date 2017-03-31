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
using System.Diagnostics; 
using System.IO;

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

        private void PostRecvFrom()
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
                richTextBox1.AppendText("post recvfrom failled : " + ex.Message + "\n");
            }
        }

        private void PostSendTo(ref ArgItem arg, int length)
        {
            try
            {
                sock.BeginSendTo(arg.buf, 0, length, 0, arg.ep, SendCallback, arg);
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText("post recvfrom failled : " + ex.Message + "\n");
            }
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

            richTextBox1.AppendText("server stopped.\n");
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
                string str = Encoding.UTF8.GetString(arg.buf, 0, recv);
                richTextBox1.AppendText( "recv from " + arg.ep.ToString() + ":" + str + "\n");


                if (checkBox1.Checked)
                {

                }

                sock.BeginReceiveFrom(arg.buf, 0, 1024, 0, ref arg.ep, RecvCallback, arg);
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
            }
        }

        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                ArgItem arg = (ArgItem)ar.AsyncState;
                arg.s.EndSendTo(ar);
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                //this.notifyIcon1.Visible = true;
                this.ShowInTaskbar = false;
            }
        }

        //private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    this.Visible = true;
        //    this.WindowState = FormWindowState.Normal;
        //    richTextBox1.AppendText("test");
        //}
    }
}
