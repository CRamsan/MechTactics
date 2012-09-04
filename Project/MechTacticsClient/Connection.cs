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
using System.Text;
using System.Net.Sockets;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;

namespace MechTacticsClient
{
    public class Connection
    {

        public TcpClient tcp = new TcpClient();
        public int connectionNumber;

        public Connection() { }

        public Connection(TcpClient client)
        {
            this.tcp = client;
        }

        public Connection(string address)
        {
            tcp.Connect(address, 4654);
        }

        public void Send(string message)
        {
            tcp.Client.Send(System.Text.Encoding.UTF8.GetBytes(StringCompressor.CompressString(message) + ";"));
        }

        public string Recieve()
        {
            string temp = "";
            try
            {
                byte[] tempBuffer = new byte[1];
                string singleChar = "";
                while(true)
                {
                    tcp.Client.Receive(tempBuffer);
                    singleChar = System.Text.Encoding.UTF8.GetString(tempBuffer);
                    if (singleChar.Equals(";"))
                        break;
                    else
                        temp = temp + singleChar;
                }
                temp = StringCompressor.DecompressString(temp);
                temp.Replace("\n", "");
                return temp;
            }
            catch (SocketException)
            {
                throw;
            }
            catch (XmlException)
            {
                throw;
            }
        }
    }
}
