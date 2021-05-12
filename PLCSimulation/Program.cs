using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PLCSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var localIp1 = "127.0.0.1";
            var localPort1 = 12289;
            
            var localIp2 = "127.0.0.1";
            var localPort2 = 12290;
            
            var localIp3 = "127.0.0.1";
            var localPort3 = 12291;

            IPAddress ipAddress1 = IPAddress.Parse(localIp1);
            Socket socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint1 = new IPEndPoint(ipAddress1, localPort1);
            socket1.Bind(ipEndPoint1);
            socket1.Listen(1);
            
            IPAddress ipAddress2 = IPAddress.Parse(localIp2);
            Socket socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint2 = new IPEndPoint(ipAddress2, localPort2);
            socket2.Bind(ipEndPoint2);
            socket2.Listen(1);
            
            IPAddress ipAddress3 = IPAddress.Parse(localIp3);
            Socket socket3 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint3 = new IPEndPoint(ipAddress3, localPort3);
            socket3.Bind(ipEndPoint3);
            socket3.Listen(1);

            Server server1;
            Server server2;
            Server server3;

            var clientSocket1 = socket1.Accept();
            server1 = new Server(clientSocket1);
            
            var clientSocket2 = socket2.Accept();
            server2 = new Server(clientSocket2);
            
            var clientSocket3 = socket3.Accept();
            server3 = new Server(clientSocket3);

            ThreadStart threadStart1 = new ThreadStart(server1.plcSim);
            Thread thread1 = new Thread(threadStart1);
            thread1.Start();
            
            ThreadStart threadStart2 = new ThreadStart(server2.plcSim);
            Thread thread2 = new Thread(threadStart2);
            thread2.Start();
            
            ThreadStart threadStart3 = new ThreadStart(server3.plcSim);
            Thread thread3 = new Thread(threadStart3);
            thread3.Start();

            /*Server[] servers = new Server[3];
            for (int i = 0; i < 3; i++)
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
                //server.cmd = readLine;
                if (readLine == "10")
                {
                    server1.cmd = "11OK0000";
                }else if (readLine == "11")
                {
                    server1.cmd = "11OK0001";
                }else if (readLine == "20")
                {
                    server2.cmd = "11OK0000";
                }else if (readLine == "21")
                {
                    server2.cmd = "11OK0001";
                }else if (readLine == "30")
                {
                    server3.cmd = "11OK0000";
                }else if (readLine == "31")
                {
                    server3.cmd = "11OK0001";
                }
                else
                {
                    server1.cmd = "11OK0000";
                    server2.cmd = "11OK0000";
                    server3.cmd = "11OK0000";
                }
            }
        }
    }
}