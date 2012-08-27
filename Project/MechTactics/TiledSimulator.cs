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
using MechTactics.GameElements;
using MechTactics.Interfaces;

namespace MechTactics
{
    public class TiledSimulator : ISimulator
    {
        public List<Player> players;
        public TileMap map;
        public int mapSize;
        public int maxHeight;
        public int numberOfPlayers;
        public int idCount = 100;
        private AStarPathFinder pathfinder;
        public int turnResult = 0;
        public bool readable = false;
        public bool running = false;

        public TiledSimulator()
        {
            this.players = new List<Player>(0);
        }

        public void addPlayer(Player player)
        {
            this.players.Add(player);
        }

        public void executeCommand(Command command)
        {
            lock (this)
            {
                if (command.ObjectId == -1)
                    return;
            }
        }

        private void move(Unit gameObject, int x, int y)
        {
            Path path = pathfinder.findPath(gameObject, gameObject.Move, x, y);
            if (path != null)
            {
                gameObject.HasMoved=true;
                this.players.ElementAt(gameObject.Team).move(gameObject.Id, x, y);
            }
        }

        private void attack(GameObject gameObject,int x,int y)
        {
        }

        private bool create(int cost, GameObject gameObject, char commandType, int x, int y)
        {
            return false;
        }

        public void flush()
        {
            

        }

        public void setMap(TileMap map, int maxHeight)
        {
            this.map = map;
            this.maxHeight = maxHeight;
            this.pathfinder = new AStarPathFinder(map);
        }

        public void clearElements()
        {
            map.clearElements();
            for(int j = 0; j < players.Count; j++)
            {
                Player player = players.ElementAt(j);
                player.clearElements();
            }
        }

        public void clearMap()
        {
            map.clearElements();
        }

        public Snapshot getSnapshot()
        {
            return null;
        }

        public Snapshot getSnapshot(Client client)
        {
            return null;
        }

        public int getResult() 
        {
            return turnResult;
        }
    }
}
