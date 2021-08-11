﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PLCSimulation
{
    public class Server
    {
        public Socket socket;
        public string cmd = "11OK0000\r\n";
        public byte[] buffer = new byte[1024];

        public Server(Socket socket)
        {
            this.socket = socket;
        }

        public void plcSim()
        {
            serverThread(socket, cmd);
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
                        if (str == "01WRDD6030 01" || str == "01WRDD8030 01" || str == "01WRDD10030 01")
                        {
                            socket.Send(Encoding.ASCII.GetBytes(cmd));
                        }
                        if (str == "01WWRD6030 01 0000")
                        {
                            cmd = "11OK0000\r\n";
                            Console.WriteLine("1-Reset");
                        }
                        if (str == "01WWRD8030 01 0000")
                        {
                            cmd = "11OK0000\r\n";
                            Console.WriteLine("2-Reset");
                        }
                        if (str == "01WWRD10030 01 0000")
                        {
                            cmd = "11OK0000\r\n";
                            Console.WriteLine("3-Reset");
                        }
                        if (str == "01WWRD6032 01 0001")
                        {
                            Console.WriteLine("1-OK");
                        }
                        if (str == "01WWRD8032 01 0001")
                        {
                            Console.WriteLine("2-OK");
                        }
                        if (str == "01WWRD10032 01 0001")
                        {
                            Console.WriteLine("3-OK");
                        }
                        if (str == "01WWRD6032 01 0002")
                        {
                            Console.WriteLine("1-NG");
                        }
                        if (str == "01WWRD8032 01 0002")
                        {
                            Console.WriteLine("2-NG");
                        }
                        if (str == "01WWRD10032 01 0002")
                        {
                            Console.WriteLine("3-NG");
                        }
                        lastStr = str;
                    }

                    Thread.Sleep(10);
                }
            }
        }
    }
}