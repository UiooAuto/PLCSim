using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PLCSimulation
{
    class Program
    {
        public static Socket socket;
        public static Server servers;
        static void Main(string[] args)
        {
            Console.WriteLine("Run...");
            Console.WriteLine("请输入：同步 t / 异步 y");

            bool tb = false;

            string v = Console.ReadLine();
            if ("t" == v)
            {
                tb = true;
                Console.WriteLine("已选同步");
            }
            else if ("y" == v)
            {
                tb = false;
                Console.WriteLine("已选异步");
            }
            var localIp = "127.0.0.1";
            var localPort = 12289;
            byte[] buffer = new byte[14];

            IPAddress ipAddress = IPAddress.Parse(localIp);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, localPort);
            socket.Bind(ipEndPoint);
            setSocket(1);
            //socket.Listen(3);

            //Server[] servers = new Server[3];
            /*for (int i = 0; i < 3; i++)
            {
                var clientSocket = socket.Accept();
                servers[i] = new Server(clientSocket);

                ThreadStart threadStart = new ThreadStart(servers[i].plcSim);
                Thread thread = new Thread(threadStart);
                thread.Start();
            }*/
            if (tb)
            {
                Console.WriteLine("同步工作开始");
            }
            else
            {
                Console.WriteLine("异步工作开始");
            }

            while (true)
            {                
                var readLine = Console.ReadLine();
                if (tb)
                {
                    if (readLine == "1")
                    {
                        Server.cmdTB = "11OK0001\r\n";
                        Console.WriteLine(Server.cmdTB);
                    }
                    else if (readLine == "2")
                    {
                        Server.cmdTB = "11OK0002\r\n";
                    }
                    else if (readLine == "3")
                    {
                        Server.cmdTB = "11OK0003\r\n";
                    }
                    else
                    {
                        Server.cmdTB = "11OK0000\r\n";
                    }
                }
                else
                {
                    if (readLine.Contains("1"))
                    {
                        Server.cmd1 = "11OK0001\r\n";
                    }
                    if (readLine.Contains("2"))
                    {
                        Server.cmd2 = "11OK0001\r\n";
                    }
                    if (readLine.Contains("3"))
                    {
                        Server.cmd3 = "11OK0001\r\n";
                    }
                    if (readLine == "0")
                    {
                        Server.cmd1 = "11OK0000\r\n";
                        Server.cmd2 = "11OK0000\r\n";
                        Server.cmd3 = "11OK0000\r\n";
                    }
                }                
            }
        }

        public static void setSocket(int num)
        {
            socket.Listen(num);
            var clientSocket = socket.Accept();
            servers = new Server(clientSocket);

            ThreadStart threadStart = new ThreadStart(servers.plcSim);
            Thread thread = new Thread(threadStart);
            thread.Start();
            Console.WriteLine("监听结束");
        }
    }
}