﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
namespace FlightMobileApp
{
    public class Connect
    {
        readonly Object obj = new object();
        TcpClient tcpClient;
        NetworkStream netStream;
        private Boolean connected = false;
        private Boolean firstTime = true;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IClient" /> is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if connected; otherwise, <c>false</c>.
        /// </value>
        public Boolean Connected
        {
            get { return connected; }
            set
            {
                connected = value;
            }
        }
        /// <summary>
        /// Connects the specified ip.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public Boolean ConnectToFG(string ip, int port)
        {
            try
            {
                tcpClient = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpClient.Connect(ip, port);
                // use the ipaddress as in the server program
                Console.WriteLine("Connected");

                //String str = Console.ReadLine();
                netStream = tcpClient.GetStream();
                Connected = true;
                return true;
            }
            catch (Exception e)
            {
                Connected = false;
                Console.WriteLine("Error..... " + e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            if (Connected)
            {
                netStream.Close();
                tcpClient.Close();
                Connected = false;
            }
            else
            {
                Console.WriteLine("Not connected ");
                return;
            }
        }

        /// <summary>
        /// Writes the and read.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public bool WriteAndRead(string command, double value)
        {
            if (firstTime)
            {
                //if flightgear not connected
                if (netStream == null)
                {
                    return false;
                }
                try
                {

                    //set value to server
                    Byte[] sendBytes = Encoding.ASCII.GetBytes("data\r\n");
                    netStream.Write(sendBytes, 0, sendBytes.Length);

                }
                catch (Exception)
                {
                    return false;
                }
                firstTime = false;
            }
            if (!Connected)
            {
                return false;
            }
            if (netStream.CanRead && netStream.CanWrite)
            {
                try
                {
                    lock (obj)
                    {
                        //set value to server
                        Byte[] sendBytes = Encoding.ASCII.GetBytes("set " + command + " " + value + " \r\n");
                        netStream.Write(sendBytes, 0, sendBytes.Length);
                        //get value to server

                        sendBytes = Encoding.ASCII.GetBytes("get " + command + " \r\n");
                        netStream.Write(sendBytes, 0, sendBytes.Length);
                        // Reads NetworkStream into a byte buffer.
                        byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                        // Set a 10000 millisecond = 10 sec timeout for reading.
                        netStream.ReadTimeout = 10000;
                        // Read can return anything from 0 to numBytesToRead. 
                        // This method blocks until at least one byte is read.
                        netStream.Read(bytes, 0, (int)tcpClient.ReceiveBufferSize);

                        // Returns the data received from the host to the console.
                        double returndata = Convert.ToDouble(Encoding.ASCII.GetString(bytes));

                        if (returndata != value)
                        {
                            return false;
                        }


                        return true;

                    }

                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("You cannot read data from this stream.");
                tcpClient.Close();

                // Closing the tcpClient instance does not close the network stream.
                netStream.Close();
                return false;
            }
        }

        public void Send()
        {

            try
            {
                //this.Latitude = SwitchReadWrite(1);
            }

            catch (Exception)
            {
                Disconnect();
            }
        }

    }
}