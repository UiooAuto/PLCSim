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
        public static Server[] servers;
        static void Main(string[] args)
        {
            var localIp = "127.0.0.1";
            var localPort = 12289;
            byte[] buffer = new byte[14];

            IPAddress ipAddress = IPAddress.Parse(localIp);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, localPort);
            socket.Bind(ipEndPoint);
            setSocket(3);
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
            

            while (true)
            {
                var readLine = Console.ReadLine();
                if (readLine == "10")
                {
                    servers[0].cmd = "11OK0000";
                }else if (readLine == "1")
                {
                    servers[0].cmd = "11OK0001";
                }else if (readLine == "20")
                {
                    servers[1].cmd = "11OK0000";
                }else if (readLine == "2")
                {
                    servers[1].cmd = "11OK0001";
                }else if (readLine == "30")
                {
                    servers[2].cmd = "11OK0000";
                }else if (readLine == "3")
                {
                    servers[2].cmd = "11OK0001";
                }
            }
        }

        public static void setSocket(int num)
        {
            socket.Listen(num);
            servers = new Server[num];
            for (int i = 0; i < num; i++)
            {
                var clientSocket = socket.Accept();
                servers[i] = new Server(clientSocket);

                ThreadStart threadStart = new ThreadStart(servers[i].plcSim);
                Thread thread = new Thread(threadStart);
                thread.Start();
            }
        }
    }
}