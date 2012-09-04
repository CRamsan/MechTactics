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
    class Logic
    {
        private int[,] map;
        private int playerNumber;
        private int ore;
        private KeyValuePair<int, int> startingPosition;
        private KeyValuePair<int, int> enemyStartingPosition;
        private List<GameObject> myUnits;
        private List<GameObject> enemyUnits;

        private List<GameObject> defense = new List<GameObject>(0);
        private List<GameObject> attack = new List<GameObject>(0);

        private Random rand = new Random();
        private bool single_command_mode = false;

        private int turn = 1;

        private int mode = 1;
        //0 = hold and save money
        //1 = build and organize
        //2 = rush and defend
        private int counter = 0;

        public Logic(int[,] _map, int _playerNumber, int _ore, KeyValuePair<int, int> _startingPosition)
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
            attack.Clear();
            defense.Clear();
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
                    int range = 0;
                    int move = 0;
                    bool isUnit = false;
                    switch (type)
                    {
                        case 'L':
                            move = 10;
                            range = 1;
                            isUnit = true;
                            break;
                        case 'M':
                            move = 30;
                            range = 1;
                            isUnit = true;
                            break;
                        case 'N':
                            range = 1;
                            isUnit = true;
                            move = 20;
                            break;
                        case 'O':
                            range = 4;
                            move = 15;
                            isUnit = true;
                            break;
                        case 'P':
                            move = 35;
                            range = 1;
                            isUnit = true;
                            break;
                        case 'Q':
                            move = 30;
                            range = 1;
                            isUnit = true;
                            break;
                        case 'R':
                            move = 20;
                            range = 6;
                            isUnit = true;
                            break;
                        case 'S':
                            range = 1;
                            move = 20;
                            isUnit = true;
                            break;
                    }
                    GameObject e = new GameObject(id, team, type, move, x, y, isUnit);
                    if (team == playerNumber)
                    {
                        myUnits.Add(e);
                        if (type == 'P' || type == 'Q' || type == 'R' || type == 'S')
                        {
                            attack.Add(e);
                        }
                        else if (type == 'M' || type == 'N' || type == 'O')
                        {
                            defense.Add(e);
                        }
                    }
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
            turn++;
            if (turn == 1)
            {
                foreach (GameObject enemyObject in enemyUnits)
                {
                    if (enemyObject.getId() == 'A')
                    {
                        enemyStartingPosition = new KeyValuePair<int, int>(enemyObject.getX(), enemyObject.getY());
                    }
                }
            }
            counter++;
            System.Console.WriteLine("Mode set to " + mode);
            List<Command> commandList = new List<Command>(0);


            foreach (GameObject objectToMove in defense)
            {
                Command nextAction = new Command();
                int x = objectToMove.getX();
                int y = objectToMove.getY();
                int team = objectToMove.getTeam();

                int id = objectToMove.getId();
                char type = objectToMove.getType();
                int move = objectToMove.getMove();

                bool attacking = false;

                KeyValuePair<int,int> objective = enemyAtRange(objectToMove);

                if (objective.Key != -1)
                    attacking = true;

                if (attacking)
                {
                    System.Console.WriteLine("Defences Attacking");
                    nextAction = new Command(playerNumber, id, 'X', objective);
                    commandList.Add(nextAction);
                }

            }

            foreach (GameObject objectToMove in attack)
            {
                Command nextAction = new Command();
                int x = objectToMove.getX();
                int y = objectToMove.getY();
                int team = objectToMove.getTeam();

                int id = objectToMove.getId();
                char type = objectToMove.getType();
                int move = objectToMove.getMove();
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

                bool attacking = false;

                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (enemyAtLocation(enemyUnits, x, y + 1, playerNumber))
                            {
                                attacking = true;
                                dx = x;
                                dy = y + 1;
                            }
                            break;
                        case 1:
                            if (enemyAtLocation(enemyUnits, x, y - 1, playerNumber))
                            {
                                attacking = true;
                                dx = x;
                                dy = y - 1;
                            }
                            break;
                        case 2:
                            if (enemyAtLocation(enemyUnits, x - 1, y, playerNumber))
                            {
                                attacking = true;
                                dx = x - 1;
                                dy = y;
                            }
                            break;
                        case 3:
                            if (enemyAtLocation(enemyUnits, x + 1, y, playerNumber))
                            {
                                attacking = true;
                                dx = x + 1;
                                dy = y;
                            }
                            break;
                    }
                }

                if (attacking)
                {
                    System.Console.WriteLine("Offences Attacking");
                    nextAction = new Command(playerNumber, id, 'X', new KeyValuePair<int, int>(dx, dy));
                    commandList.Add(nextAction);
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
                        System.Console.WriteLine("Offences Moving");
                        nextAction = new Command(playerNumber, id, 'Y', new KeyValuePair<int, int>(dx, dy));
                        commandList.Add(nextAction);
                    }
                }
            }

            foreach (GameObject objectToMove in myUnits)
            {
                Command nextAction = new Command();
                int x = objectToMove.getX();
                int y = objectToMove.getY();
                int team = objectToMove.getTeam();

                int id = objectToMove.getId();
                char type = objectToMove.getType();
                int move = objectToMove.getMove();

                switch (mode)
                {
                    case 0:
                        if (!objectToMove.checkIfUnit())
                        {
                        }
                        if (counter >= 50)
                        {
                            mode = 1;
                            counter = 0;
                        }
                        break;
                    case 1:
                        if (!objectToMove.checkIfUnit())
                        {
                            int commandType = rand.Next(3);
                            char buildOrder = 'Z';
                            switch (objectToMove.getType())
                            {
                                case 'D':
                                    switch (commandType)
                                    {
                                        case 0:
                                            System.Console.WriteLine("Building Tank");
                                            buildOrder = 'P';
                                            break;
                                        case 1:
                                            System.Console.WriteLine("Building Heavy Tank");
                                            buildOrder = 'Q';
                                            break;
                                        case 2:
                                            System.Console.WriteLine("Building Artillery");
                                            buildOrder = 'R';
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
                                            buildOrder = 'S';
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                            }
                            nextAction = new Command(playerNumber, id, buildOrder, getNextLocation(objectToMove));
                        }
                        if (counter >= 20)
                        {
                            mode = 2;
                            counter = 0;
                        }
                        break;
                    case 2:
                        if (!objectToMove.checkIfUnit())
                        {
                            int commandType = rand.Next(3);
                            int dx = x + (rand.Next(3) - 1);
                            int dy = y + (rand.Next(3) - 1);
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
                        }
                        if (counter >= 20)
                        {
                            mode = 0;
                            counter = 0;
                        }
                        break;
                }
                if (nextAction.playerId != -1)
                { commandList.Add(nextAction); }

            }
            return commandList;
        }

        public KeyValuePair<int, int> enemyAtRange(GameObject gameObject)
        {
            int mod = 0;
            if (map[gameObject.getX(), gameObject.getY()] >= 9)
                mod = 2;

            foreach (GameObject enemy in enemyUnits)
            {
                if (isInRange(gameObject, enemy.getX(), enemy.getY(), mod))
                {
                    return new KeyValuePair<int, int>(enemy.getX(), enemy.getY());
                }
            }

            return new KeyValuePair<int, int>(-1, -1);
        }

        public bool isInRange(GameObject gameObject, int x, int y, int mod)
        {
            int dx = Math.Abs(gameObject.getX() - x);
            int dy = Math.Abs(gameObject.getY() - y);

            if ((dx + dy) <= (gameObject.getRange() + mod))
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public KeyValuePair<int, int> getBestMove(GameObject element)
        {
            KeyValuePair<int, int> moveto;
            int key = 0;
            int value = 0;
            int range = element.getMove();

            if (element.getTeam() == 0)
            {
                key = element.getX() + 2;
            }
            else
            {
                key = element.getX() - 2;
            }
            if (element.getTeam() == 0)
            {
                value = element.getY() + 2;
            }
            else
            {
                value = element.getY() - 2;
            }

            int cont = 0;
            if (!locationIsEmpty(enemyUnits, key, value))
            {
                do
                {
                    value += (rand.Next(5) - 2);
                    key += (rand.Next(5) - 2);
                    cont++;
                }
                while (!locationIsEmpty(enemyUnits, value, key) && cont < 4);
            }

            int x = element.getX();
            int y = element.getY();
            moveto = new KeyValuePair<int, int>(key, value);

            return moveto;
        }

        public KeyValuePair<int, int> getNextLocation(GameObject element)
        {
            int x = element.getX();
            int y = element.getY();

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!(i == j || j == (i * -1)))
                    {
                        if (locationIsEmpty(myUnits, x + i, y + j))
                        {
                            if (locationIsEmpty(enemyUnits, x + i, y + j))
                            {
                                return new KeyValuePair<int, int>(x + i, y + j);
                            }
                        }
                    }
                }
            }

            return new KeyValuePair<int, int>(-1, -1);
        }
    }
}
