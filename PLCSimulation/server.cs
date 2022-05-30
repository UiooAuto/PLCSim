using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PLCSimulation
{
    public class Server
    {
        public Socket socket;
        public static string cmdTB = "11OK0000\r\n";
        public byte[] buffer = new byte[1024];

        public static string cmd1 = "11OK0000\r\n";
        public static string cmd2 = "11OK0000\r\n";
        public static string cmd3 = "11OK0000\r\n";

        public Server(Socket socket)
        {
            this.socket = socket;
        }

        public void plcSim()
        {
            //serverThread(socket, cmdTB);
            ServerThreadTB(socket);
        }

        private void ServerThreadTB(Socket socket)
        {
            string lastStr = "";
            while (true)
            {
                lock (this)
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = 0;
                    }

                    var receive = socket.Receive(buffer);
                    if (receive != 0)
                    {
                        var str = Encoding.ASCII.GetString(buffer);
                        var indexOf = str.IndexOf('\r');
                        if (indexOf != -1)
                        {
                            str = str.Substring(0, indexOf);
                        }
                        else
                        {
                            str = str.Substring(0, receive);
                        }
                        //Console.WriteLine(str);
                        if (str == "01WRDD6030 01")
                        {
                            socket.Send(Encoding.ASCII.GetBytes(cmd1));
                        }
                        if (str == "01WRDD8030 01")
                        {
                            socket.Send(Encoding.ASCII.GetBytes(cmd2));
                        }
                        if (str == "01WRDD10030 01")
                        {
                            socket.Send(Encoding.ASCII.GetBytes(cmd3));
                        }
                        if (str == "01WRDD5950 01")
                        {
                            socket.Send(Encoding.ASCII.GetBytes(cmdTB));
                        }
                        if (str == "01WWRD6030 01 0000")
                        {
                            cmd1 = "11OK0000\r\n";
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("1-Reset");
                        }
                        if (str == "01WWRD8030 01 0000")
                        {
                            cmd2 = "11OK0000\r\n";
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("2-Reset");
                        }
                        if (str == "01WWRD10030 01 0000")
                        {
                            cmd3 = "11OK0000\r\n";
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("3-Reset");
                        }
                        if (str == "01WWRD5950 01 0000")
                        {
                            cmdTB = "11OK0000\r\n";
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("TB-Reset");
                        }
                        if (str == "01WWRD6032 01 0001")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("1-OK");
                        }
                        if (str == "01WWRD8032 01 0001")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("2-OK");
                        }
                        if (str == "01WWRD10032 01 0001")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("3-OK");
                        }
                        if (str == "01WWRD6032 01 0002")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("1-NG");
                        }
                        if (str == "01WWRD8032 01 0002")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("2-NG");
                        }
                        if (str == "01WWRD10032 01 0002")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("3-NG");
                        }
                        if (str == "01WWRD5952 01 0001")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("TB-1");
                        }
                        if (str == "01WWRD5952 01 0002")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("TB-2");
                        }
                        if (str == "01WWRD5952 01 0003")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("TB-3");
                        }
                        if (str == "01WWRD5952 01 0004")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("TB-4");
                        }
                        lastStr = str;
                    }

                    Thread.Sleep(10);
                }
            }
        }

        private void serverThread(Socket socket, string cmdStr)
        {
            string lastStr = "";
            while (true)
            {
                lock (this)
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = 0;
                    }
                    
                    var receive = socket.Receive(buffer);
                    if (receive != 0)
                    {
                        var str = Encoding.ASCII.GetString(buffer);
                        var indexOf = str.IndexOf('\r');
                        if (indexOf != -1)
                        {
                            str = str.Substring(0, indexOf);
                        }
                        else
                        {
                            str = str.Substring(0, receive);
                        }
                        //Console.WriteLine(str);
                        if (str == "01WRDD6030 01" || str == "01WRDD8030 01" || str == "01WRDD10030 01" || str == "01WRDD5950 01")
                        {
                            //Console.WriteLine("Return "+ cmd);
                            socket.Send(Encoding.ASCII.GetBytes(cmdTB));
                        }
                        if (str == "01WWRD6030 01 0000")
                        {
                            cmdTB = "11OK0000\r\n";
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("1-Reset");
                        }
                        if (str == "01WWRD8030 01 0000")
                        {
                            cmdTB = "11OK0000\r\n";
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("2-Reset");
                        }
                        if (str == "01WWRD10030 01 0000")
                        {
                            cmdTB = "11OK0000\r\n";
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("3-Reset");
                        }
                        if (str == "01WWRD5950 01 0000")
                        {
                            cmdTB = "11OK0000\r\n";
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("TB-Reset");
                        }
                        if (str == "01WWRD6032 01 0001")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("1-OK");
                        }
                        if (str == "01WWRD8032 01 0001")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("2-OK");
                        }
                        if (str == "01WWRD10032 01 0001")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("3-OK");
                        }
                        if (str == "01WWRD6032 01 0002")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("1-NG");
                        }
                        if (str == "01WWRD8032 01 0002")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("2-NG");
                        }
                        if (str == "01WWRD10032 01 0002")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("3-NG");
                        }
                        if (str == "01WWRD5952 01 0001")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("TB-1");
                        }
                        if (str == "01WWRD5952 01 0002")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("TB-2");
                        }
                        if (str == "01WWRD5952 01 0003")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("TB-3");
                        }
                        if (str == "01WWRD5952 01 0004")
                        {
                            socket.Send(Encoding.ASCII.GetBytes("11OK\r\n"));
                            Console.WriteLine("TB-4");
                        }
                        lastStr = str;
                    }

                    Thread.Sleep(10);
                }
            }
        }
    }
}