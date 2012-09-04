/*Copyright (c) 2012 Cesar Ramirez
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
 * documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial 
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using MechTactics.Abstracts;

namespace MechTactics
{
    public class Client
    {
        private BaseServer.commandRecievedCallback sendCommand;
        private int playerId;
        private int ore;

        public Connection connection { get; set; }
        public TcpListener listener { get; set; }
        public bool isConnected { get; set; }
        public int id { get; set; }
        public Player Player { get; set; }

        public Client(TcpListener listener, int id)
        {
            this.listener = listener;
            this.id = id;
            Player = new Player(id);
        }

        public Client()
        {
        }

        public Client(int id)
        {
            this.id = id;
            Player = new Player(id);
            isConnected = true;
        }

        public void connectToServer()
        {
            try
            {
                connection = new Connection(listener.AcceptTcpClient());
                isConnected = true;
            }
            catch (SocketException e)
            {
                isConnected = false;
            }
            catch (InvalidOperationException e)
            {
                isConnected = false;
            }
        }

        public void disconectFromServer()
        {
            isConnected = false;
            if (connection != null)
            {
                if (connection.tcp != null)
                    connection.tcp.Client.Close();
                else
                    listener.Stop();
            }
            else
            {
                listener.Stop(); 
            }
        }

        public void sendString(string text)
        {
            if (isConnected)
            {

                try
                {
                    int bufferSize = text.Length;
                    connection.tcp.Client.Send(System.Text.Encoding.UTF8.GetBytes(StringCompressor.CompressString(text) + ";"));
                }
                catch (SocketException se)
                {
                    isConnected = false;
                    throw;
                }
            }
        }

        public string recieveString()
        {
            try
            {
                return connection.Recieve();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void setCommandRecievedCallback(BaseServer.commandRecievedCallback sendCommand)
        {
            this.sendCommand = sendCommand;
        }
    }
}
