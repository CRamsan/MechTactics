using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;
using MechTactics.Interfaces;
using System.Windows.Forms;
namespace MechTactics.Abstracts
{
    public abstract class BaseServer : IServer
    {
        /*
         * States for the server
         */
        public const int SERVER_HALT_EXCEPTION = -2;
        public const int SERVER_HALT = -1;
        public const int SERVER_LISTENING = 0;
        public const int SERVER_PREPARING= 1;
        public const int SERVER_SENDING_INITIAL_DATA = 2;
        public const int SERVER_GAME_LOOP = 3;
        public const int SERVER_STOPPING = 4;

        /*
         * Sends the message to the UI Component.
         */
        protected Launcher.recieveMessageCallback sendMessage;

        /*
         * Will recieve and handle the message from the Clients.
         */
        public delegate void commandRecievedCallback(Command command);
        private commandRecievedCallback updateCommand;

        /*
         * Clients are an abstraction of each client connected to the server.
         */
        protected List<Client> clients;

        /*
         * The list of threads that will send and recieve information for each client. 
         */
        protected List<Thread> connectionList;


        /*
         * The generic listener used for this server.
         */
        protected TcpListener listener;

        /*
         * Reader and writer used for loading and saving data.
         */
        protected StreamWriter writer;
        protected StreamReader reader;
        
        /*
         * Thread used for listening for clients, once the game starts this thread is stopped.
         */
        protected Thread lobbyThread;
        
        /*
         * Thread were all the game logic and magic happens.
         */
        protected Thread gameThread;
           
        /*
         * Interface to the Simulator, the gamne could either be played in a continuous or discrete world.
         */
        protected ISimulator sim;

        /*
         * Flags for managing the behavious of the game server. 
         */
        protected bool Async = false;
        protected bool stop = true;
        protected bool saving = false;
        protected bool socketOpen = false;
        protected int numberOfPlayers = 0;
        protected int serverState = SERVER_HALT;

        /*
         * Creates a server. This server is inactive. 
         */
        public BaseServer( )
        {
            this.serverState = SERVER_HALT;
            this.updateCommand = new commandRecievedCallback(this.executeRecievedCommand);
        }

        ////////////////
        //Interface
        ////////////////

        /// <summary>
        /// Returns an integer representing the state of the server.
        /// </summary>
        /// <returns></returns>
        public int getServerState()
        {
            return serverState;
        }

        /// <summary>
        /// Returns wether the server is running in a real time or turn based mode.
        /// </summary>
        /// <returns></returns>
        public bool isAsync()
        {
            return Async;
        }

        /// <summary>
        /// Returns true if the server is active.
        /// </summary>
        /// <returns></returns>
        public bool isRunning()
        {
            return socketOpen || !stop;
        }

        /// <summary>
        /// Returns true if the server is listenning for clients, false otherwise. 
        /// </summary>
        /// <returns></returns>
        public bool isListening()
        {
            return socketOpen;
        }

        /// <summary>
        /// Returns the value sent by the simulator about it been discrete or continuous. 
        /// </summary>
        /// <returns></returns>
        public bool isSimulatorDiscrete()
        {
            return sim.isDiscrete();
        }

        /// <summary>
        /// Start a thread which will accept the incoming clients.
        /// </summary>
        public void startListening()
        {
            this.sendMessage(SERVER_LISTENING, "Creating listener thread");
            lobbyThread = new Thread(new ThreadStart(this.listenForClients));
            lobbyThread.Start();
        }

        /// <summary>
        /// Will close the socket flag(stopping the lobbyThread), this method
        /// is important because it will clean the extar client which was created but not used.
        /// </summary>
        public void finishListening()
        {
            this.socketOpen = false;
            clients.ElementAt(clients.Count - 1).disconectFromServer();
            clients.RemoveAt(clients.Count - 1);
            this.sendMessage(SERVER_LISTENING, "Listening ended");
        }

        /// <summary>
        /// Starts a game in a separate thread.
        /// </summary>
        public void startGame()
        {
            this.sendMessage(SERVER_PREPARING, "Preparing game thread");
            stop = false;
            gameThread = new Thread(new ThreadStart(match));
            gameThread.Start();
            this.sendMessage(SERVER_PREPARING, "Game thread started");
        }

        /// <summary>
        /// Raises the stop flag, forcing the current game to stop. Network connections are closed and 
        /// the game thread is destroyed.
        /// </summary>
        public void endGame()
        {
            stop = true;
            closeConnections();
            gameThread.Join();
        }

        /// <summary>
        /// Assign the recieveMessageCallback used for sending messages back to the UI thread.
        /// </summary>
        /// <param name="updateText"></param>
        public void setRecieveMessageCallback(Launcher.recieveMessageCallback updateText)
        {
            this.sendMessage = updateText;
        }

        /////////////////////
        // Protected Methods
        /////////////////////

        /// <summary>
        /// Listen for clients and connects them to the server.
        /// </summary>
        protected void listenForClients()
        {
            this.sendMessage(SERVER_LISTENING, "Starting to listen");
            socketOpen = true;
            clients = new List<Client>(0);
            listener = new TcpListener(IPAddress.Loopback, 4654);
            listener.Start();
            
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

        /// <summary>
        /// Sends information to each client with the information they need to start a game.
        /// </summary>
        protected void sendInitialData()
        {
            string savedGame = DateTime.Now.ToString("hh.mm.ss") + ".msg";
            writer = new StreamWriter(savedGame);
            writer.AutoFlush = true;
            if (saving)
            {
                //saveData(Serializer.fromMapToString(sim.map));
                for (int i = 0; i < clients.Count; i++)
                {
                    Client client = clients.ElementAt(i);
                    client.sendString("Data");
                    client.Player.setInitialParams(Loader.getInitialDataForPlayer(i));
                    client.sendString(Serializer.fromStatstoString(Loader.getInitialDataForPlayer(i)));
                    saveData(Serializer.fromStatstoString(Loader.getInitialDataForPlayer(i)));
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

        /// <summary>
        /// The main construc for this game server, this encapsulates an entire match.
        /// First information is send to each client with the sendInitialData() method and then 
        /// the gameLoop() method starts the game. The sendInitialData() method has already been 
        /// defined but the gameLoop() method is defined by the class that inherists BaseServer.
        /// </summary>
        protected void match()
        {
            this.sendInitialData();
            this.gameLoop();
        }

        
        /// <summary>
        /// This method is in charge of closing all network connections once a game finishes 
        /// or is stopped. This method is important becuase a common problem has been that if 
        /// the nectwork components are not closed correctly they cannot be used again until 
        /// this problem is fixed(FC the application).
        /// </summary>
        protected void closeConnections()
        {
            if (socketOpen)
            {
                socketOpen = false;
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

        /// <summary>
        /// Compress and saves a string.
        /// </summary>
        /// <param name="data"></param>
        protected void saveData(string data)
        {
            writer.Write(StringCompressor.CompressString(data) + ";");
        }

        /// <summary>
        /// Uses the reader to read from the file with saved data. 
        /// Only one token(; delimited) is read at a time.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// This method should be called only from the clients.
        /// Clients will call this method once they recieve a respond
        /// from the network client.
        /// </summary>
        /// <param name="toExcecute"></param>
        private void executeRecievedCommand(Command toExcecute)
        {
            try
            {
                new MethodInvoker(delegate { this.sim.executeCommand(toExcecute); }).BeginInvoke(null, null);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error while sending a command to the sim");
            }
        }

        /////////////////////
        // Abstract Methods
        /////////////////////

        /// <summary>
        /// Abstract method for loading data from a file.
        /// </summary>
        /// <param name="filePath"></param>
        protected abstract void startLoadingData(string filePath);

        /// <summary>
        /// Abstract method for each game loop. 
        /// This method will be defined by either the Async or Sync server.
        /// </summary>
        protected abstract void gameLoop();
    }
}