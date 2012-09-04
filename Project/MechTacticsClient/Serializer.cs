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
    public static class Serializer
    {
        public static string fromElementsToString(List<GameObject> components, int ore)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "snapshot", ""));
            XmlNode root = doc.DocumentElement;

            XmlNode Ore = doc.CreateNode(XmlNodeType.Element, "ore", "");
            Ore.InnerText = ore.ToString();
            root.AppendChild(Ore);

            XmlNode enemies = doc.CreateNode(XmlNodeType.Element, "objects", "");
            foreach (GameObject e in components)
            {
                XmlNode gameObject = doc.CreateNode(XmlNodeType.Element, "object", "");
                XmlNode objectId = doc.CreateNode(XmlNodeType.Element, "objectId", "");
                XmlNode objectType = doc.CreateNode(XmlNodeType.Element, "objectType", "");
                XmlNode objectTeam = doc.CreateNode(XmlNodeType.Element, "objectTeam", "");
                XmlNode objectY = doc.CreateNode(XmlNodeType.Element, "objectY", "");
                XmlNode objectX = doc.CreateNode(XmlNodeType.Element, "objectX", "");
                objectId.InnerText = e.getId().ToString();
                objectType.InnerText = e.getType().ToString();
                objectTeam.InnerText = e.getTeam().ToString();
                objectY.InnerText = e.getY().ToString();
                objectX.InnerText = e.getX().ToString();
                gameObject.AppendChild(objectId);
                gameObject.AppendChild(objectType);
                gameObject.AppendChild(objectTeam);
                gameObject.AppendChild(objectX);
                gameObject.AppendChild(objectY);
                enemies.AppendChild(gameObject);
            }
            root.AppendChild(enemies);
            return root.OuterXml;
        }

        public static Command fromStringToCommand(string action)
        {
            Command command;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Regex.Replace(action, @"\p{C}+", ""));
                XmlElement root = doc.DocumentElement;
                int playerId = Int32.Parse(root.SelectSingleNode("playerId").InnerText);
                int objectId = Int32.Parse(root.SelectSingleNode("objectId").InnerText);
                char type = Char.Parse(root.SelectSingleNode("type").InnerText);
                string tempKVP = root.SelectSingleNode("objective").InnerText;
                int key = Int32.Parse(tempKVP.Split(',')[0].Substring(1));
                int value = Int32.Parse(tempKVP.Split(',')[1].Substring(0, tempKVP.Split(',')[1].Length - 1));
                KeyValuePair<int, int> objective = new KeyValuePair<int, int>(key, value);
                command = new Command(playerId, objectId, type, objective);
                return command;
            }
            catch (Exception e)
            {
                command = new Command();
                return command;
            }
        }

        public static string fromCommandToString(Command action)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "command", ""));
            XmlNode root = doc.DocumentElement;

            XmlNode xml_playerId = doc.CreateNode(XmlNodeType.Element, "playerId", "");
            xml_playerId.InnerText = action.playerId.ToString();
            root.AppendChild(xml_playerId);

            XmlNode xml_objectId = doc.CreateNode(XmlNodeType.Element, "objectId", "");
            xml_objectId.InnerText = action.objectId.ToString();
            root.AppendChild(xml_objectId);

            XmlNode xml_type = doc.CreateNode(XmlNodeType.Element, "type", "");
            xml_type.InnerText = action.type.ToString();
            root.AppendChild(xml_type);

            XmlNode xml_objective = doc.CreateNode(XmlNodeType.Element, "objective", "");
            xml_objective.InnerText = action.objective.ToString();
            root.AppendChild(xml_objective);

            return root.OuterXml;
        }

        public static string fromCommandListToString(List<Command> actionList)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "commandList", ""));
            XmlNode root = doc.DocumentElement;

            XmlDocument node = new XmlDocument();
            foreach (Command action in actionList)
            {
                string temp = fromCommandToString(action);
                doc.LoadXml(temp);

                XmlNode commandNode = doc.DocumentElement;
                root.AppendChild(commandNode);
            }

            return root.OuterXml;
        }
    }
}
