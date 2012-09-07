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
using MechTactics.GameElements.Attributes;

namespace MechTactics.GameElements
{
    public class GameObject
    {
        protected int id;
        protected int team;
        protected int x;
        protected int y;
        protected float pres_x;
        protected float pres_y;
        protected bool isPrecise;
        protected bool isActive;
        protected char type;
        protected double health;
        protected double speed;
        protected bool hasMoved;
        protected List<GameAttribute> attributesList;

        public GameObject(int id, int team, char type)
        { }

        public bool healthModifier(double modifier)
        {
            health += modifier;
            if (health <= 0)
                return true;
            else
                return false;
        }

        public bool healthModifier(double modifier, char type)
        {
            if (type == 'A')
            {
                //TODO
            }
            return healthModifier(modifier);
        }


        public void setLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public bool IsPrecise
        {
            get { return isPrecise; }
            set { isPrecise = value; }
        }

        public char Type
        {
            get { return type; }
            set { type = value; }
        }

        public double Health
        {
            get { return health; }
            set { health = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int Team
        {
            get { return team; }
            set { team = value; }
        }

        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public bool HasMoved
        {
            get { return hasMoved; }
            set { hasMoved = value; }
        }
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

    }
}
