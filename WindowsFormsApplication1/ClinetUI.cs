﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class ClientUI : Form
    {
        private const int BUFFER_SIZE = 8192;
        private Socket socket;

        public ClientUI()
        {
            InitializeComponent();
            socket = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                this.richTextBoxResponse.Clear();
                if (socket != null)
                {
                    socket.Close();
                }

                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(this.textBoxIP.Text), Int32.Parse(this.textBoxPort.Text));
                socket = new Socket(ep.Address.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
                this.richTextBoxResponse.AppendText("Start connect." + "\n");
                socket.BeginConnect(ep, new AsyncCallback(this.ConnectCallback), socket);
            }
            catch(Exception ex)
            {
                this.richTextBoxResponse.AppendText("Error In Connect:" + ex.Message + "\n");
                try
                {
                    if (socket != null)
                        socket.Close();
                }
                catch(Exception ex2)
                {
                    this.richTextBoxResponse.AppendText("Error In Connect:" + ex2.Message + "\n");
                }
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket s = (Socket)ar.AsyncState;

                if(s.Equals(socket) == false)
                {
                    s.Close();
                    return;
                }

                if(s != null)
                {
                    s.EndConnect(ar);
                    this.richTextBoxResponse.AppendText("Connect Success.\n");
                    if (this.checkBoxRecvData.Checked)
                    {
                        _RecvArg arg = new _RecvArg();
                        arg.socket = s;
                        arg.buf = new byte[BUFFER_SIZE];
                        arg.length = 0;
                        this.richTextBoxResponse.AppendText("Start receive.\n");
                        s.BeginReceive(arg.buf, 0, BUFFER_SIZE, 0, new AsyncCallback(RecvCallback), arg);
                    }
                }
                else
                {
                    this.richTextBoxResponse.AppendText("Please establish connect before other operation.\n");                    
                }
            }
            catch(Exception ex)
            {
                this.richTextBoxResponse.AppendText("Error In ConnectCallback:" + ex.Message + "\n");
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (socket != null)
                {
                    byte[] arr = System.Text.Encoding.Default.GetBytes(this.richTextBoxRequest.Text);
                    socket.BeginSend(arr, 0, arr.Length, 0, new AsyncCallback(SendCallback), socket);
                    this.richTextBoxResponse.AppendText(String.Format("Sending Data: {0} bytes." + "\n", arr.Length));
                }
                else
                {
                    this.richTextBoxResponse.AppendText("Please establish connect before other operation.\n");
                }
            }
            catch (Exception ex)
            {
                this.richTextBoxResponse.AppendText("Error In Send:" + ex.Message + "\n");
            }
        }

        private struct _RecvArg
        {
            public Socket socket;
            public byte[] buf;
            public int length;
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket s = (Socket)ar.AsyncState;

                if (s.Equals(socket) == false)
                {
                    s.Close();
                    return;
                }

                if (s != null)
                {
                    int count = s.EndSend(ar);
                    this.richTextBoxResponse.AppendText(String.Format("Send Success: {0} bytes.\n", count));
                }
                else
                {
                    this.richTextBoxResponse.AppendText("Please establish connect before other operation.\n");
                }
            }
            catch (Exception ex)
            {
                this.richTextBoxResponse.AppendText("Error In SendCallback:" + ex.Message + "\n");
            }
        }

        private void RecvCallback(IAsyncResult ar)
        {
            try
            {
                _RecvArg arg = (_RecvArg)ar.AsyncState;
                Socket s = arg.socket;

                if (s.Equals(socket) == false)
                {
                    s.Close();
                    return;
                }

                if(s != null)
                {
                    int count = s.EndReceive(ar);
                    if(count == 0)
                    {
                        arg.length = 0;
                    }
                    else
                    {
                        arg.length = count;
                        this.richTextBoxResponse.AppendText(string.Format("Recv {0} bytes, content: \n", count) + System.Text.Encoding.UTF8.GetString(arg.buf, 0, arg.length) + "\n");
                        this.richTextBoxResponse.AppendText("Waiting receive.");
                        s.BeginReceive(arg.buf, 0, BUFFER_SIZE, 0, new AsyncCallback(RecvCallback), arg);
                    }
                }
            }
            catch (Exception ex)
            {
                //this.richTextBoxResponse.AppendText("Error In RecvCallback:" + ex.Message + "\n");
            }
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if(socket != null)
                {
                    socket.Close();
                    this.richTextBoxResponse.AppendText("Connection Closed.\n");
                    socket = null;
                }
                else
                {
                    this.richTextBoxResponse.AppendText("Please establish connect before other operation.\n");
                }
            }
            catch (Exception ex)
            {
                this.richTextBoxResponse.AppendText("Error In Disconnect:" + ex.Message + "\n");
            }
        }

        private void buttonClear1_Click(object sender, EventArgs e)
        {
            this.richTextBoxRequest.Clear();
        }

        private void buttonClear2_Click(object sender, EventArgs e)
        {
            this.richTextBoxResponse.Clear();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.Opacity = (double)this.numericUpDown1.Value;
        }

        private void richTextBoxResponse_TextChanged(object sender, EventArgs e)
        {
            this.richTextBoxResponse.SelectionStart = this.richTextBoxResponse.Text.Length;
            this.richTextBoxResponse.ScrollToCaret();
        }
    }
}
