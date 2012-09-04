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

namespace MechTactics
{
    public class Path
    {
        /** The list of steps building up this path */
        private List<Step> steps = new List<Step>();
        private int cost;

        /**
         * Create an empty path
         */
        public Path()
        {

        }

        /**
         * Get the length of the path, i.e. the number of steps
         * 
         * @return The number of steps in this path
         */
        public int getLength()
        {
            return steps.Count;
        }

        /**
         * Get the cost of the path.
         * @return The cost of this path
         */
        public int getCost()
        {
            this.cost = 0;
            for (int i = 0; i < this.getLength(); i++)
            {
                this.cost += getStep(i).getCost();
            }
            return this.cost;
        }

        /**
         * Get the step at a given index in the path
         * 
         * @param index The index of the step to retrieve. Note this should
         * be >= 0 and < getLength();
         * @return The step information, the position on the map.
         */
        public Step getStep(int index)
        {
            return steps.ElementAt(index);
        }

        /**
         * Get the x coordinate for the step at the given index
         * 
         * @param index The index of the step whose x coordinate should be retrieved
         * @return The x coordinate at the step
         */
        public int getX(int index)
        {
            return getStep(index).getX();
        }

        /**
         * Get the y coordinate for the step at the given index
         * 
         * @param index The index of the step whose y coordinate should be retrieved
         * @return The y coordinate at the step
         */
        public int getY(int index)
        {
            return getStep(index).getY();
        }

        /**
         * Append a step to the path.  
         * 
         * @param x The x coordinate of the new step
         * @param y The y coordinate of the new step
         */
        public void appendStep(int x, int y, int cost)
        {
            steps.Add(new Step(x, y, cost));
        }

        /**
         * Prepend a step to the path.  
         * 
         * @param x The x coordinate of the new step
         * @param y The y coordinate of the new step
         */
        public void prependStep(int x, int y, int cost)
        {
            steps.Add(new Step(x, y, cost));
        }

        /**
         * Check if this path contains the given step
         * 
         * @param x The x coordinate of the step to check for
         * @param y The y coordinate of the step to check for
         * @return True if the path contains the given step
         */
        public bool contains(int x, int y, int cost)
        {
            return steps.Contains(new Step(x, y, cost));
        }

        /**
         * Check if a step was the last step attached
         * 
         * @param x The x coordinate of the step to check for
         * @param y The y coordinate of the step to check for
         * @return True if the step is the last step
         */
        public bool isLast(int x, int y, int cost)
        {
            if (steps.Count() == 0)
                return false;
            else if (steps.ElementAt(steps.Count() - 1).equals(new Step(x, y, cost)))
                return true;
            else
                return false;
        }

        /**
         * A single step within the path
         * 
         * @author Cesar Ramirez
         */
        public class Step
        {
            /** The x coordinate at the given step */
            private int x;
            /** The y coordinate at the given step */
            private int y;
            /** The cost at the given step */
            private int cost;

            /**
             * Create a new step
             * 
             * @param x The x coordinate of the new step
             * @param y The y coordinate of the new step
             */
            public Step(int x, int y, int cost)
            {
                this.x = x;
                this.y = y;
                this.cost = cost;
            }

            /**
             * Get the x coordinate of the new step
             * 
             * @return The x coodindate of the new step
             */
            public int getX()
            {
                return x;
            }

            /**
             * Get the y coordinate of the new step
             * 
             * @return The y coodindate of the new step
             */
            public int getY()
            {
                return y;
            }

            public int getCost()
            {
                return cost;
            }

            /**
             * @see Object#hashCode()
             */
            public int hashCode()
            {
                return x * y;
            }

            /**
             * @see Object#equals(Object)
             */
            public bool equals(Object other)
            {
                Step temp = (Step)other;
                try
                {
                    if (this.x == temp.getX() && this.y == temp.getY())
                        return true;
                    else
                        return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}
