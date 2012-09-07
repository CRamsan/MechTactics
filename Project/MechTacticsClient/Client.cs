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
using MechTactics.GameElements;
using MechTactics;

namespace MechTacticsClient
{
    class Client
    {
        public const string LOCALHOST = "127.0.0.1";

        private static int MaxIterations = 100;
        private static SimpleLogic logicComponent;
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
            catch (Exception)
            {
                System.Console.WriteLine("Error while initializing game connection");
                System.Console.WriteLine("Wrong data recieved; exiting");
                return false;
            }
            return true;
        }

        private static void Run()
        {
            int currentIteration = 1;
            List<Command> nextAction = new List<Command>(0);
            System.Console.WriteLine("");
            System.Console.WriteLine("Starting to run simulation");
            logicComponent = new SimpleLogic();
            while (currentIteration <= MaxIterations && !Client.Stop)
            {
                System.Console.Write("Turn: " + currentIteration);
                Snapshot gameStatus = new Snapshot(ServerConnection.Recieve());
                nextAction = logicComponent.getCommand(gameStatus);
                SendCommand(nextAction);
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
                System.Console.WriteLine("Getting information from previous turn");
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

            if (!ServerConnection.Recieve().Equals(Constants.INITIAL_DATA_TAG))
            {
                System.Console.WriteLine("Wrong data recieved");
            }
            else 
            {
                throw new Exception();
            }

            string initValues = ServerConnection.Recieve();
            string mapValues = ServerConnection.Recieve();

            System.Console.WriteLine("All data recieved");
        }

        public static void Main(String[] args)
        {
            if (Connect())
            {
                ServerConnection.Send(Constants.START_GAME_TAG);
                Run();
            }
            System.Console.WriteLine("Press any key to close the application...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }
}