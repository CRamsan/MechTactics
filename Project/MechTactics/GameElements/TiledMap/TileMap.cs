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
using MechTactics.Abstracts;

namespace MechTactics.GameElements
{
    public class TileMap : BaseMap
    {
        public Tile[,] terrain;
        public List<GameObject> elements;


        public TileMap(int[,] _map)
        {
            Width = _map.GetLength(0);
            Height= _map.GetLength(1); 
            terrain = new Tile[Width, Height];
            for (int i = 0; i < _map.GetLength(0); i++)                                   //Iterate the
            {
                for (int j = 0; j < _map.GetLength(0); j++)                                  //matrix of
                {
                    terrain[i, j] = new Tile(i, j, _map[i, j]);
                }
            }
            elements = new List<GameObject>();
        }

        public List<GameObject> getObjectsAt(int x, int y)
        {
            return terrain[x, y].elements;
        }

        public void remove(GameObject element)
        {
            this.elements.Remove(element);
            this.terrain[element.X, element.Y].elements.Remove(element);
        }

        public void add(GameObject element)
        {
            this.elements.Add(element);
            this.terrain[element.X, element.Y].elements.Add(element);
        }

        public bool isEmpty(int x, int y)
        {
            try
            {
                if (terrain[x, y].elements.Count == 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public bool isInRange(GameObject gameObject, int x, int y)
        {
            return isInRange(gameObject, x, y, 0);
        }

        public bool isInRange(GameObject gameObject, int x, int y, int mod)
        {
            int dx = Math.Abs(gameObject.X - x);
            int dy = Math.Abs(gameObject.Y - y);

            if ((dx + dy) <= (((Unit)gameObject).Move + mod))
            {
                if(true)
                {}
                return true;
            }
            else
            {
                return false;
            }
        }

        public void clearElements()
        {
            elements.Clear();
        }
    }
}
