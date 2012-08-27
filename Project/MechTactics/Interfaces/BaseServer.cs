using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace MechTactics.Interfaces
{
    public abstract class BaseServer : IServer
    {
        public const int SERVER_LISTENING = 0;
        public const int SERVER_GAME_BODY= 1;
        public const int SERVER_SENDING_INITIAL_DATA = 2;
        public const int SERVER_GAME_LOOP = 3;
        public const int SERVER_STOPPING = 4;

        //Sends the message to the UI Component
        protected Launcher.recieveMessageCallback sendMessage;

        //Will recieve and handle the message from the Clients
        public delegate void commandRecievedCallback(Command command);
        private commandRecievedCallback updateCommand;

        protected List<Client> clients;
        protected TcpListener listener;
        protected StreamWriter writer;
        protected StreamReader reader;
        protected Thread gameThread;
        protected Thread networkThread;
        protected ISimulator sim;
        protected Loader loader;
        //protected event EventHandler Update;
        protected bool Async = false;
        protected bool stop = true;
        protected bool saving = false;
        protected bool socketOpen = false;
        protected int numberOfPlayers = 0;

        public BaseServer( )
        {
            this.updateCommand = new commandRecievedCallback(this.executeMessage);
        }

        public bool isAsync()
        {
            return Async;
        }

        public void startServer()
        {
            this.sendMessage(SERVER_LISTENING, "Starting to listen");
            socketOpen = true;
            clients = new List<Client>(0);
            listener = new TcpListener(IPAddress.Loopback, 4654);
            listener.Start();
            string savedGame = DateTime.Now.ToString("hh.mm.ss") + ".msg";
            writer = new StreamWriter(savedGame);
            writer.AutoFlush = true;
            int counter = 0;
            while (socketOpen)
            {
                Client client = new Client(listener, counter);
                clients.Add(client);
                client.connectToServer();
                if (client.isConnected)
                {
                    counter++;
                    this.sendMessage(SERVER_LISTENING, "Player " + counter + " connected");
                    if (numberOfPlayers != 0)
                    {
                        if (counter >= numberOfPlayers)
                        {
                            socketOpen = false;
                            break;
                        }
                    }
                }
            }
            this.numberOfPlayers = counter;
            this.sendMessage(SERVER_LISTENING, "Players: " + counter + ", all clients connected");
            saving = true;
        }

        public void startListening()
        {
            this.sendMessage(SERVER_LISTENING, "Creating listener thread");
            networkThread = new Thread(new ThreadStart(this.startServer));
            networkThread.Start();
        }

        public void stopListening()
        {
            this.socketOpen = false;
            clients.ElementAt(clients.Count - 1).disconectFromServer();
            clients.RemoveAt(clients.Count - 1);
            this.sendMessage(SERVER_LISTENING, "Listening stopped");
        }

        public void startGame()
        {
            this.sendMessage(SERVER_GAME_BODY, "Preparing game thread");
            stop = false;
            gameThread = new Thread(new ThreadStart(gameBody));
            gameThread.Start();
            this.sendMessage(SERVER_GAME_BODY, "Game thread started");
        }

        protected void sendInitialData()
        {
            if (saving)
            {
                //saveData(Serializer.fromMapToString(sim.map));
                for (int i = 0; i < clients.Count; i++)
                {
                    Client client = clients.ElementAt(i);
                    client.sendString("Data");
                    client.Player.setInitialParams(loader.getInitialDataForPlayer(i));
                    client.sendString(Serializer.fromStatstoString(loader.getInitialDataForPlayer(i)));
                    saveData(Serializer.fromStatstoString(loader.getInitialDataForPlayer(i)));
                    //client.sendString(Serializer.fromMapToString(sim.map));
                }

                for (int i = 0; i < clients.Count; i++)
                {
                    Client client = clients.ElementAt(i);
                    if (client.recieveString().Equals("run"))
                    {
                        this.sendMessage(SERVER_GAME_LOOP, "Data sent");
                    }
                    else
                    {
                        this.sendMessage(SERVER_GAME_LOOP, "Error while sending data to client");
                        client.disconectFromServer();
                    }
                }
            }
            else
            {
                this.sendMessage(SERVER_GAME_LOOP, "Not in connection mode, data already loaded");
            }
        }

        protected void gameBody()
        {
            this.sendInitialData();
            this.gameLoop();
        }

        public void endGame()
        {
            stop = true;
            closeConnections();
            gameThread.Join();
        }

        protected void closeConnections()
        {
            if (socketOpen)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    Client client = clients.ElementAt(i);
                    client.disconectFromServer();
                    client.connection.tcp.Client.Dispose();
                    client.connection.tcp.Close();
                    client.connection.tcp = null;
                }
                this.listener.Stop();
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                }
            }
            if (!saving)
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            socketOpen = false;
        }

        /*event EventHandler IServer.MessageSend
        {
            add
            {
                lock (Update)
                {
                    Update += value;
                }
            }
            remove
            {
                lock (Update)
                {
                    Update -= value;
                }
            }
        }*/

        /*protected virtual void OnMessageSend(EventArgs e)
        {

        }*/

        protected void saveData(string data)
        {
            writer.Write(StringCompressor.CompressString(data) + ";");
        }

        protected string loadData()
        {
            string temp = "";

            while (true)
            {
                int charFound = reader.Read();
                char singleChar = Convert.ToChar(charFound);
                if (singleChar.Equals(';'))
                    break;
                else
                    temp = temp + singleChar;
            }
            temp = StringCompressor.DecompressString(temp);
            temp.Replace("\n", "");
            return temp;
        }

        public void setRecieveMessageCallback(Launcher.recieveMessageCallback updateText)
        {
            this.sendMessage = updateText;
        }

        protected void executeMessage(Command command)
        {
            try
            {
                this.sim.executeCommand(command);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error in the lock in BaseServer");
            }
        }

        public bool isRunning() 
        {
            return !stop;
        }

        public bool isListening()
        {
            return socketOpen;
        }

        protected abstract void startLoadingData(string filePath);
        protected abstract void gameLoop();
    }
}