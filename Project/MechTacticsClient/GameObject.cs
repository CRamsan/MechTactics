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

namespace MechTacticsClient
{
    public class GameObject
    {
        private int id;
        protected int team;
        protected bool isUnit;
        protected bool isActive;
        protected int x;
        protected int y;
        protected char type;
        protected int health;
        protected int damage;
        protected int armor;
        protected int move;
        protected int range;
        protected bool hasMoved;
        protected bool wasAttacked;
        protected bool dead;

        public GameObject()
        { }

        public GameObject(int id, int team, char type, int x, int y, bool isUnit)
        {
            this.id = id;
            this.team = team;
            this.type = type;
            this.x = x;
            this.y = y;
            this.isUnit = isUnit;
            this.isActive = false;
            this.hasMoved = true;
            this.wasAttacked = false;
        }

        public GameObject(int id, int team, char type, int move, int x, int y, bool isUnit)
        {
            this.id = id;
            this.team = team;
            this.type = type;
            this.move = move;
            this.x = x;
            this.y = y;
            this.isUnit = isUnit;
            this.hasMoved = true;
            this.wasAttacked = false;
            this.isActive = false;
        }

        public void setActive(bool active)
        {
            this.isActive = active;
        }

        public bool getActive()
        {
            return isActive;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getTeam()
        {
            return team;
        }

        public void setIfUnit(bool isUnit)
        {
            this.isUnit = isUnit;
        }

        public bool checkIfUnit()
        {
            return isUnit;
        }

        public int getId()
        {
            return id;
        }

        public char getType()
        {
            return type;
        }

        public int getHealth()
        {
            return health;
        }

        public bool doDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
                return true;
            else
                return false;
        }

        public int getDamage()
        {
            return damage;
        }

        public int getArmor()
        {
            return armor;
        }

        public int getMove()
        {
            return move;
        }

        public int getRange()
        {
            return range;
        }

        public bool getHasMoved()
        {
            return this.hasMoved;
        }

        public void setLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void setHasMoved(bool moved)
        {
            this.hasMoved = moved;
        }

        public void setDead(bool dead)
        {
            this.dead = dead;
        }

        public bool isDead()
        {
            return this.dead;
        }

        public void setWasAttacked(bool wasAttacked)
        {
            this.wasAttacked = wasAttacked;
        }

        public bool getWasAttacked()
        {
            return this.wasAttacked;
        }
    }
}
