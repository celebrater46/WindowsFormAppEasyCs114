using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppEasyCs114
{
    public partial class Form1 : Form
    {
        public static string HOST = "localhost";
        public static int PORT = 10000;
        
        public Form1()
        {
            InitializeComponent();
            IPHostEntry ih = Dns.GetHostEntry(HOST);

            TcpListener tl = new TcpListener(ih.AddressList[0], PORT);
            tl.Start();
            
            Console.WriteLine("Waiting...");
            while (true)
            {
                TcpClient tc = tl.AcceptTcpClient();
                Console.WriteLine("Welcome.");

                Client c = new Client(tc);
                Thread th = new Thread(c.run);
                th.Start();
            }
        }
    }

    class Client
    {
        private TcpClient tc;

        public Client(TcpClient c)
        {
            tc = c;
        }

        public void run()
        {
            StreamWriter sw = new StreamWriter(tc.GetStream());
            StreamReader sr = new StreamReader(tc.GetStream());

            while (true)
            {
                try
                {
                    String str = sr.ReadLine();
                    sw.WriteLine("Server: [" + str + "]!!");
                    sw.Flush();
                }
                catch
                {
                    sr.Close();
                    sw.Close();
                    tc.Close();
                    break;
                }
            }
        }
    }
}