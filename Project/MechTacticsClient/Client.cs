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
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading;

namespace MechTacticsClient
{
    class Client
    {
        public const string LOCALHOST = "127.0.0.1";

        private static int playerNumber;
        private static int MaxIterations = 100;
        private static Logic logicComponent;
        private static bool Stop;
        private static Connection ServerConnection;

        private static bool Connect()
        {
            System.Console.WriteLine("Client created, initializing...");
            ServerConnection = new Connection();
            System.Console.WindowHeight = 50;
            System.Console.WindowWidth = 50;
            try
            {
                System.Console.WriteLine("Creating connection");
                ServerConnection = new Connection(LOCALHOST);
                System.Console.WriteLine("Connected, waiting for other players");
                RecieveInitialInformation();
            }
            catch (System.Net.Sockets.SocketException)
            {
                System.Console.WriteLine("Error while initializing game connection");
                System.Console.WriteLine("Connection to server not established; exiting");
                return false;
            }
            return true;
        }

        private static void Run()
        {
            //
            Random rand = new Random();
            //
            int currentIteration = 1;
            List<Command> nextAction = new List<Command>(0);
            System.Console.WriteLine("");
            System.Console.WriteLine("Starting to run simulation");
            while (currentIteration <= MaxIterations && !Client.Stop)
            {
                System.Console.Write("Turn: " + currentIteration);
                string gameStatus = ServerConnection.Recieve();
                //nextAction = logicComponent.getCommand(gameStatus);
                //SendCommand(nextAction);
                //Thread.Sleep(rand.Next(5)*500);
                ServerConnection.Send("Nothing");
                currentIteration++;
            }
            System.Console.WriteLine("End of game");
        }

        private static void SendCommand(List<Command> command)
        {
            try
            {
                ServerConnection.Send(Serializer.fromCommandListToString(command));
            }
            catch (System.Net.Sockets.SocketException e)
            {
                System.Console.WriteLine("Error while sending command to the server");
                System.Console.WriteLine("Connection lost: " + e);
                Stop = true;
            }
            catch (Exception fe)
            {
                System.Console.WriteLine("Something went wrong");
                System.Console.WriteLine("Game over " + fe);
                Stop = true;
            }
        }

        private static void SendCommand(Command command)
        {
            try
            {
                ServerConnection.Send(Serializer.fromCommandToString(command));
            }
            catch (System.Net.Sockets.SocketException e)
            {
                System.Console.WriteLine("Error while sending command to the server");
                System.Console.WriteLine("Connection lost: " + e);
                Stop = true;
            }
            catch (Exception fe)
            {
                System.Console.WriteLine("Something went wrong");
                System.Console.WriteLine("Game over " + fe);
                Stop = true;
            }
        }

        private static string RecieveResult()
        {
            try
            {
                //System.Console.WriteLine("Getting information from previous turn");
                return ServerConnection.Recieve();
            }
            catch (System.Net.Sockets.SocketException e)
            {
                System.Console.WriteLine("Error in the middle of the simulation");
                System.Console.WriteLine("Connection lost: " + e);
                return null;
            }
            catch (Exception fe)
            {
                System.Console.WriteLine("Something went wrong");
                System.Console.WriteLine("Game over " + fe);
                return null;
            }
        }

        private static void RecieveInitialInformation() {

            if (!ServerConnection.Recieve().Equals("Data"))
            {
                System.Console.WriteLine("Wrong data recieved");
            }
            else 
            {
                System.Console.WriteLine("Server running, getting initial data");
            }

            string initValues = ServerConnection.Recieve();
            string mapValues = ServerConnection.Recieve();

            System.Console.WriteLine("All data recieved");

            /*XmlDocument doc = new XmlDocument();
            doc.LoadXml(Regex.Replace(initValues, @"\p{C}+", ""));

            XmlElement root = doc.DocumentElement;

            int playerId = Int32.Parse(root.SelectSingleNode("id").InnerText);
            playerNumber = playerId;
            int ore = Int32.Parse(root.SelectSingleNode("ore").InnerText);

            XmlNode position = root.SelectSingleNode("InitXY");

            int x_pos = Int32.Parse(position.SelectSingleNode("x").InnerText);
            int y_pos = Int32.Parse(position.SelectSingleNode("y").InnerText);

            KeyValuePair<int, int> startingPosition = new KeyValuePair<int, int>(x_pos, y_pos);

            int maxTurn = Int32.Parse(root.SelectSingleNode("maxTurn").InnerText);
            MaxIterations = maxTurn;

            doc = new XmlDocument();
            doc.LoadXml(Regex.Replace(mapValues, @"\p{C}+", ""));

            root = doc.DocumentElement;

            int mapLength = Int32.Parse(root.SelectSingleNode("length").InnerText);
            XmlNode landscapeRoot = root.SelectSingleNode("terrain");

            int[,] map = new int[mapLength, mapLength];

            foreach (XmlNode child in landscapeRoot.ChildNodes)
            {
                int x = Int32.Parse(child.SelectSingleNode("x").InnerText);
                int y = Int32.Parse(child.SelectSingleNode("y").InnerText);
                int z = Int32.Parse(child.SelectSingleNode("type").InnerText);
                map[x, y] = z;
            }

            logicComponent = new Logic(map, playerNumber, ore, startingPosition);*/

        }

        public static void Main(String[] args)
        {
            if (Connect())
            {
                ServerConnection.Send("run");
                Run();
            }
            System.Console.WriteLine("Press any key to close the application...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }
}
