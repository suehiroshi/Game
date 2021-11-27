using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Media;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private NetworkStream stream;
        private TcpClient tcpClient;

        SoundPlayer player = new SoundPlayer("D:/Fuchikami/nene/Lopu$ - So Cute~.mp3");
       
        Socket socket_send;
        void send_stream(string str)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            System.Text.Encoding GBK = System.Text.Encoding.GetEncoding("GBK");
            byte[] buffer = GBK.GetBytes(str + "\n");
            stream.Write(buffer, 0, buffer.Length);
        }
       
        void receive_stream()
        {
            byte[] receive_data = new byte[1024];
            //定义编码格式
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);//为使用GB2312做准备
            System.Text.Encoding GBK = System.Text.Encoding.GetEncoding("GBK");
            if (stream.CanRead)
            {

                int len = stream.Read(receive_data, 0, receive_data.Length);
                string msg = GBK.GetString(receive_data, 0, receive_data.Length);

                string str = "\r\n";
                char[] str1 = str.ToCharArray();
                string[] messy_code = { "??[2J ", "[5m", "[44m", "[37;0m", "[1;33m", "[1;32m", "[1;31m" };
                string[] msg1 = msg.Split(str1);//以换行符为分隔符
                for (int j = 0; j < msg1.Length; j++)//逐行显示
                {

                    msg1[j] = msg1[j].Replace(messy_code[0], " ");
                    msg1[j] = msg1[j].Replace(messy_code[1], " ");
                    msg1[j] = msg1[j].Replace(messy_code[2], " ");
                    msg1[j] = msg1[j].Replace(messy_code[3], " ");
                    msg1[j] = msg1[j].Replace(messy_code[4], " ");
                    msg1[j] = msg1[j].Replace(messy_code[5], " ");
                    msg1[j] = msg1[j].Replace(messy_code[6], " ");
                    listBox1.Items.Add(msg1[j]);
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            tcpClient = new TcpClient();
            //套接字建立连接


            socket_send = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint point = new IPEndPoint(IPAddress.Parse("10.1.230.74"), 3900);
            socket_send.Connect(point);
            try
            {
                //向指定的IP地址的服务器发送连接请求
                tcpClient.Connect("10.1.230.74", 3900);
                listBox1.Items.Add("连接成功");
                stream = tcpClient.GetStream();
                receive_stream();//接收字节流并显示在屏幕上

            }
            catch
            {
                listBox1.Items.Add("服务器未启动");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcpClient.Connected)
            {
                string action = textBox1.Text.ToString();
                listBox1.Items.Add("输入的信息为：" + action);
                send_stream(action);
                receive_stream();

            }
            else
            {
                listBox1.Items.Add("连接已断开");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (stream != null)
            {
                stream.Close();
                tcpClient.Close();
                socket_send.Close();
            }
            listBox1.Items.Add("退出游戏");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            player.Load();
            player.Play();
        }
     

    }
}
