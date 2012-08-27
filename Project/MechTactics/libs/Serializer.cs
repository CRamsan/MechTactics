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
using System.Xml;
using System.Text.RegularExpressions;
using MechTactics.GameElements.attributes;

namespace MechTactics
{
    public static class Serializer
    {
        public static string fromStatstoString(int playerNumber, int startingOre, KeyValuePair<int, int> initialPosition, int maxTurn)
        {
            return "";
        }


        public static string fromStatstoString(GameAttribute[] attributes)
        {
            return "HELLOS";
        }


        
        public static string fromMapToString(TileMap map)
        {
            return "";
        }

        public static string fromElementsToString(List<GameObject> components, int ore)
        {
            return "";
        }

        public static string fromElementsToString(Snapshot snapshot)
        {
            return "S";
        }

        public static List<Command> fromStringToCommandList(string action)
        {
            return null;
        }

        public static string fromCommandToString(Command action)
        {
            return "";
        }

        public static string fromCommandListToString(List<Command> actionList)
        {
            return "";
        }

        public static Client fromStringToStats(string initData)
        {
            Client client;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Regex.Replace(initData, @"\p{C}+", ""));
                XmlElement root = doc.DocumentElement;
                int playerId = Int32.Parse(root.SelectSingleNode("id").InnerText);
                int ore = Int32.Parse(root.SelectSingleNode("ore").InnerText);
                XmlNode initxy = root.SelectSingleNode("InitXY");
                int key   = Int32.Parse(initxy.SelectSingleNode("x").InnerText);
                int value = Int32.Parse(initxy.SelectSingleNode("y").InnerText);
                KeyValuePair<int, int> position = new KeyValuePair<int, int>(key, value);
                client = new Client(playerId);
                //client.Player.setInitialParams(position, ore, null);
                return client;
            }
            catch (Exception e)
            {
                client = new Client();
                return client;
            }
        }

        public static TileMap fromStringToMap(string mapData)
        {
            TileMap map;
            try
            {
                int[,] _map;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Regex.Replace(mapData, @"\p{C}+", ""));
                XmlElement root = doc.DocumentElement;

                int length = Int32.Parse(root.SelectSingleNode("length").InnerText);
                
                _map = new int[length,length];

                XmlNode terrain = root.SelectSingleNode("terrain");
                foreach (XmlNode child in terrain.ChildNodes)
                {
                    int x = Int32.Parse(child.SelectSingleNode("x").InnerText);
                    int y = Int32.Parse(child.SelectSingleNode("y").InnerText);
                    int z = Int32.Parse(child.SelectSingleNode("type").InnerText);
                    _map[x, y] = z;
                }

                map = new TileMap(_map);
                return map;
            }
            catch (Exception e)
            {
                map = new TileMap();
                return map;
            }
        }

        /*public static int fromStringToMapMaxHeight(string mapData)
        {
            int length = 0;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Regex.Replace(mapData, @"\p{C}+", ""));
                XmlElement root = doc.DocumentElement;
                length = Int32.Parse(root.SelectSingleNode("length").InnerText);
                return length;
            }
            catch (Exception e)
            {
                return length;
            }
        }*/
    }
}
