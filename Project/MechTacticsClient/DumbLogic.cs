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

namespace MechTacticsClient  
{
    class DumbLogic
    {
        private int[,] map;
        private int playerNumber;
        private int ore;
        private KeyValuePair<int, int> startingPosition;

        private List<GameObject> myUnits;
        private List<GameObject> enemyUnits;
        private Random rand = new Random();
        private bool single_command_mode = false;

        public DumbLogic(int[,] _map, int _playerNumber, int _ore, KeyValuePair<int, int> _startingPosition)
        {
            playerNumber = _playerNumber;
            map = _map;
            ore = _ore;
            startingPosition = _startingPosition;
            myUnits = new List<GameObject>(0);
            enemyUnits = new List<GameObject>(0);
        }

        private void update(string gameStatus)
        {
            myUnits.Clear();
            enemyUnits.Clear();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Regex.Replace(gameStatus, @"\p{C}+", ""));
                XmlNode root = doc.LastChild;
                XmlNode oreRoot = root.SelectSingleNode("ore");
                ore = Int32.Parse(oreRoot.InnerText);

                XmlNode objectsRoot = root.SelectSingleNode("objects");

                foreach (XmlNode Components in objectsRoot.ChildNodes)
                {
                    int id = Int32.Parse(Components.SelectSingleNode("objectId").InnerText);
                    char type = Char.Parse(Components.SelectSingleNode("objectType").InnerText);
                    int team = Int32.Parse(Components.SelectSingleNode("objectTeam").InnerText);
                    int x = Int32.Parse(Components.SelectSingleNode("objectX").InnerText);
                    int y = Int32.Parse(Components.SelectSingleNode("objectY").InnerText);
                    int move = 0;
                    bool isUnit = false;
                    switch (type)
                    {
                        case 'L':
                            move = 10;
                            isUnit = true;
                            break;
                        case 'M':
                            move = 30;
                            isUnit = true;
                            break;
                        case 'N':
                            isUnit = true;
                            move = 20;
                            break;
                        case 'O':
                            move = 15;
                            isUnit = true;
                            break;
                        case 'P':
                            move = 35;
                            isUnit = true;
                            break;
                        case 'Q':
                            move = 30;
                            isUnit = true;
                            break;
                        case 'R':
                            move = 20;
                            isUnit = true;
                            break;
                        case 'S':
                            move = 20;
                            isUnit = true;
                            break;
                    }
                    GameObject e = new GameObject(id, team, type, move, x, y, isUnit);
                    if (team == playerNumber)
                        myUnits.Add(e);
                    else
                        enemyUnits.Add(e);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception");
            }
        }

        public List<Command> getCommand(string gameStatus)
        {
            update(gameStatus);
            //System.Console.Write("Train of though: ");

            List<Command> commandList = new List<Command>(0);

            foreach (GameObject objectToMove in myUnits) 
            {
                Command nextAction = new Command();
                try
                {
                    int x = objectToMove.getX();
                    int y = objectToMove.getY();
                    int team = objectToMove.getTeam();

                    int id = objectToMove.getId();
                    char type = objectToMove.getType();
                    int move = objectToMove.getMove();

                    /*if (objectToMove.checkIfUnit())
                    {
                        System.Console.Write("Unit " + objectToMove.getId() + ", ");
                        int dx = x;
                        int dy = y;
                        System.Console.Write("at x=" + objectToMove.getX() + " y=" + objectToMove.getY() + ", ");

                        dx = x + (rand.Next(3) - 1);
                        dy = y + (rand.Next(3) - 1);

                        do
                        {
                            dx = x + (rand.Next(5) - 2);
                            dy = y + (rand.Next(5) - 2);
                        }
                        while (dx < 0 || dx >= map.GetLength(0) || dy < 0 || dy >= map.GetLength(0) || (dx == x && dy == y) || !locationIsEmpty(enemyUnits, dx, dy));
                        System.Console.Write("move to x=" + dx + " y=" + dy + "\n");
                        nextAction = new Command(playerNumber, id, 'Y', new KeyValuePair<int, int>(dx, dy));
                    }
                    else
                    { 

                    }*/
                    if (objectToMove.checkIfUnit())
                    {
                        int dx = x;
                        int dy = y;

                        dx = (rand.Next(3));
                        dy = (rand.Next(3));

                        if (playerNumber == 0)
                        {
                        }
                        else
                        {
                            dx *= -1;
                            dy *= -1;
                        }

                        x += dx;
                        y += dy;

                        bool attack = false;

                        for (int i = 0; i < 4; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    if (enemyAtLocation(enemyUnits, x, y + 1, playerNumber))
                                    {
                                        attack = true;
                                        dx = x;
                                        dy = y + 1;
                                    }
                                    break;
                                case 1:
                                    if (enemyAtLocation(enemyUnits, x, y - 1, playerNumber))
                                    {
                                        attack = true;
                                        dx = x;
                                        dy = y - 1;
                                    }
                                    break;
                                case 2:
                                    if (enemyAtLocation(enemyUnits, x - 1, y, playerNumber))
                                    {
                                        attack = true;
                                        dx = x - 1;
                                        dy = y;
                                    }
                                    break;
                                case 3:
                                    if (enemyAtLocation(enemyUnits, x + 1, y, playerNumber))
                                    {
                                        attack = true;
                                        dx = x + 1;
                                        dy = y;
                                    }
                                    break;
                            }
                        }

                        if (attack)
                        {
                            System.Console.WriteLine("Attacking");
                            nextAction = new Command(playerNumber, id, 'X', new KeyValuePair<int, int>(dx, dy));
                        }
                        else
                        {
                            do
                            {
                                dx = x + (rand.Next(5) - 2);
                                dy = y + (rand.Next(5) - 2);
                            }
                            while (dx < 0 || dx >= map.GetLength(0) || dy < 0 || dy >= map.GetLength(0) || (dx == x && dy == y));

                            bool empty = locationIsEmpty(enemyUnits, dx, dy);
                            if (empty)
                            {
                                System.Console.WriteLine("Moving");
                                nextAction = new Command(playerNumber, id, 'Y', new KeyValuePair<int, int>(dx, dy));
                            }
                        }
                    }
                    else
                    {
                        int commandType = rand.Next(3);
                        int dx = x + (rand.Next(3) - 1);
                        int dy = y + (rand.Next(3) - 1);
                        switch (objectToMove.getType())
                        {
                            case 'C':
                                switch (commandType)
                                {
                                    case 0:
                                        System.Console.WriteLine("Building Infantry");
                                        nextAction = new Command(playerNumber, id, 'M', new KeyValuePair<int, int>(dx, dy));
                                        break;
                                    case 1:
                                        System.Console.WriteLine("Building HEavy");
                                        nextAction = new Command(playerNumber, id, 'N', new KeyValuePair<int, int>(dx, dy));
                                        break;
                                    case 2:
                                        System.Console.WriteLine("Building Sniper");
                                        nextAction = new Command(playerNumber, id, 'O', new KeyValuePair<int, int>(dx, dy));
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 'D':
                                switch (commandType)
                                {
                                    case 0:
                                        System.Console.WriteLine("Building Tank");
                                        nextAction = new Command(playerNumber, id, 'P', new KeyValuePair<int, int>(dx, dy));
                                        break;
                                    case 1:
                                        System.Console.WriteLine("Building Heavy Tank");
                                        nextAction = new Command(playerNumber, id, 'Q', new KeyValuePair<int, int>(dx, dy));
                                        break;
                                    case 2:
                                        System.Console.WriteLine("Building Artillery");
                                        nextAction = new Command(playerNumber, id, 'R', new KeyValuePair<int, int>(dx, dy));
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 'E':
                                switch (commandType)
                                {
                                    case 1:
                                        System.Console.WriteLine("Building Hero");
                                        nextAction = new Command(playerNumber, id, 'S', new KeyValuePair<int, int>(dx, dy));
                                        break;
                                    default:
                                        break;
                                }
                                break;
                        }
                    }
                    if (nextAction.playerId != -1)
                    { commandList.Add(nextAction); }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Invalid Command");
                    commandList.Add(nextAction);
                }
            }
            return commandList;
        }

        public static bool enemyAtLocation(List<GameObject> visible, int x, int y, int playerNumber)
        {
            foreach (GameObject Components in visible)
            {
                if (Components.getX() == x && Components.getY() == y && Components.getTeam() != playerNumber)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool locationIsEmpty(List<GameObject> visible, int x, int y)
        {
            foreach (GameObject Components in visible)
            {
                if (Components.getX() == x && Components.getY() == y)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
