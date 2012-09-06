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
using System.Net;
using MechTactics.GameElements;
using System.IO;
using MechTactics.Abstracts;
using MechTactics.Interfaces;
using System.Threading;

namespace MechTactics
{
    public class SyncServer : BaseServer
    {

        public SyncServer( ):base()
        {
            this.Async = false;
        }
        protected override void startLoadingData(string filePath)
        {
            saving = false;
            reader = new StreamReader(filePath);
            clients = new List<Client>(0);
            string mapData = loadData();
            TileMap map = Serializer.fromStringToMap(mapData);
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Client client = Serializer.fromStringToStats(loadData());
                clients.Add(client);
                //client.Player.map = map;
                client.Player.InitGame();
                //sim.addPlayer(client.Player);
            }
            //sim.setMap(map, 10);
        }




        protected override void gameLoop()
        {
            this.sendMessage(SERVER_GAME_LOOP, "Starting game loop");
            ISimulator sim = new DiscreteSimulator();
            int activePlayers = numberOfPlayers;
            //For debbuging
            long endTime;
            long startTime;

            while (!stop)
            {
                foreach (Client client in clients)
                {
                    if (client.isConnected)
                    {
                        if (client.Player.isActive)
                        {
                            startTime = System.DateTime.Now.Millisecond;
                            this.sendMessage(SERVER_GAME_LOOP, "Client " + client.id + ":");
                            client.Player.startTurn();
                            string snapshot = sim.getSnapshot(client).ToString();
                            try
                            {
                                this.sendMessage(SERVER_GAME_LOOP, "Sending turn data");
                                if (saving)
                                    client.sendString(snapshot);
                            }
                            catch (Exception e)
                            {

                                this.sendMessage(SERVER_GAME_LOOP, "Error while sending data");
                                client.isConnected = false;
                                activePlayers--;
                                continue;
                            }
                            endTime = System.DateTime.Now.Millisecond;

                            this.sendMessage(SERVER_GAME_LOOP, "Time Sending Data: " + (endTime - startTime));

                            string action = "";
                            try
                            {
                                startTime = System.DateTime.Now.Millisecond;
                                this.sendMessage(SERVER_GAME_LOOP, "Recieving command");
                                if (saving)
                                {
                                    action = client.recieveString();
                                    saveData(action);
                                }
                                else
                                {
                                    action = loadData();
                                }
                                this.sendMessage(SERVER_GAME_LOOP, "Command recieved");
                                endTime = System.DateTime.Now.Millisecond;
                                this.sendMessage(SERVER_GAME_LOOP, "Time Waiting for Data: " + (endTime - startTime));
                            }
                            catch (Exception e)
                            {
                                this.sendMessage(SERVER_GAME_LOOP, "Error while recieving data");
                                client.isConnected = false;
                                activePlayers--;
                                continue;
                            }
                            try
                            {


                                startTime = System.DateTime.Now.Millisecond;

                                Command toExecute = Factory.fromStringToCommand(action);
                                if (toExecute.PlayerId == client.id)
                                {
                                    this.sendMessage(SERVER_GAME_LOOP, "Command sent to simulator");
                                    sim.executeCommand(toExecute);
                                }
                                else
                                {
                                    this.sendMessage(SERVER_GAME_LOOP, "Invalid command");
                                }
                                endTime = System.DateTime.Now.Millisecond;
                                this.sendMessage(SERVER_GAME_LOOP, "Time Excecuting Data: " + (endTime - startTime));
                            }
                            catch (Exception e)
                            {
                                this.sendMessage(SERVER_GAME_LOOP, "Error while excecuting data");
                                continue;
                            }
                            finally
                            {
                                client.Player.endTurn();
                            }
                            switch (sim.getResult())
                            {
                                case 0:
                                    break;
                                case 1:
                                    activePlayers--;
                                    break;
                                default:
                                    stop = true;
                                    break;
                            }
                            this.sendMessage(SERVER_GAME_LOOP, "Turn completed");
                        }
                    }
                }
                if (activePlayers <= (1))
                {
                    break;
                }
            }
        }
    }
}
