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
using MechTactics.GameElements.Attributes;

namespace MechTactics
{
    public class Player
    {
        public bool isActive { get; set; }
        public List<GameObject> objectList;
        protected List<GameAttribute> attributesList;

        public Player(int playerNumber)
        {
        }

        public void setInitialParams(GameAttribute[] attributes)
        {

        }

        public void InitGame()
        {
        }

        public void createUnit(int id, char type, int x, int y)
        {
        }
        public void createBuilding(int id, char type, int x, int y)
        {
        }

        public void startTurn()
        {
            foreach (GameObject element in this.objectList)
            {
                element.IsActive = true;
                element.HasMoved = false;
            }
        }

        public void endTurn()
        {
            foreach (GameObject element in this.objectList)
            {
                element.IsActive = false;
            }
        }

        public GameObject getObject(int id)
        {
            foreach (GameObject element in objectList)
            {
                if (element.Id == id)
                    return element;
            }
            return null;
        }

        public bool remove(int id)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                GameObject element = objectList.ElementAt(i);
                if (element.Id == id)
                {
                    this.objectList.Remove(element);
                    return true;
                }
            }
            return false;
        }

        public int getNumberOfObjects()
        {
            return objectList.Count;
        }

        public void clearElements()
        {
            objectList.Clear();
        }

        public void move(int id, int x, int y)
        {
        }
    }
}
