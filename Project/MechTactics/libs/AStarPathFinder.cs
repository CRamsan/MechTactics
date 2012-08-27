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
using MechTactics;
using MechTactics.GameElements;

namespace MechTactics
{
    public class AStarPathFinder
    {
        /** Implements the comparer for sorting Lists of Node*/
        protected NodeComparer comparer;
        /** The set of nodes that have been searched through */
        protected List<Node> closed = new List<Node>();
        /** The set of nodes that we do not yet consider fully searched */
        protected SortedList open = new SortedList();
        /** The map being searched */
        protected TileMap map;
        /** The complete set of nodes across the map */
        protected Node[,] nodes;

        /**
        * Create a path finder 
        * 
        * @param map The map to be searched
        * @param allowDiagMovement True if the search should try diaganol movement
        */
        public AStarPathFinder(TileMap map)
        {
            this.map = map;
            int width = map.terrain.GetLength(0);
            int height = map.terrain.GetLength(0);

            nodes = new Node[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node(x, y);
                }
            }
        }

        /**
         * @see PathFinder#findPath(Mover, int, int, int, int, int)
         */
        public Path findPath(Unit mover, int maxDistance, int tx, int ty)
        {
            comparer = new NodeComparer();
            open.setComparer(comparer);
            int sx = mover.X;
            int sy = mover.Y;
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int y = 0; y < nodes.GetLength(1); y++)
                {
                    nodes[x, y].cost = map.terrain[x, y].z;
                }
            }

            if (!this.isValidLocation(mover, tx, ty))
            {
                return null;
            }
            else
            {
                // If the distance of a straight line draw from the origin to the destination tile 
                // is longer than the maxDistance to move, we can't get there.
                if (Math.Abs(tx - sx) > maxDistance || Math.Abs(ty - sy) > maxDistance || Math.Abs(tx - sx) + Math.Abs(ty - sy) > maxDistance)
                {
                    return null;
                }
            }

            // The initial state for A*. The closed group is empty. Only the starting
            // tile is in the open list.
            closed.Clear();
            open.clear();
            nodes[sx, sy].depth = 0;
            open.add(nodes[sx, sy]);

            //We haven't found any tile that reaches the destiny yet
            nodes[tx, ty].parent = null;

            //This loop will iterate until we run out of tiles that can
            //be evaluated.
            while ((open.size() != 0))
            {
                Node current = getFirstInOpen();

                if (current == nodes[tx, ty])
                {
                    break;
                }

                removeFromOpen(current);
                addToClosed(current);

                //We get the distance from the current tile to the destination tile
                int currentDistanceX = (tx - current.x);
                int currentDistanceY = (ty - current.y);

                //This array will store the coordinates of the next movements
                //depending of the X or Y distance to the destination
                int[] order = new int[4];
                if (Math.Abs(currentDistanceX) > Math.Abs(currentDistanceY))
                {
                    if (currentDistanceX < 0)
                    {
                        order[0] = 3;
                        order[1] = 2;
                        order[2] = 4;
                        order[3] = 1;
                    }
                    else
                    {
                        order[0] = 1;
                        order[1] = 3;
                        order[2] = 2;
                        order[3] = 4;
                    }
                }
                else
                {
                    if (currentDistanceY < 0)
                    {
                        order[0] = 4;
                        order[1] = 1;
                        order[2] = 3;
                        order[3] = 2;
                    }
                    else
                    {
                        order[0] = 2;
                        order[1] = 1;
                        order[2] = 3;
                        order[3] = 4;
                    }
                }

                int x = 0;
                int y = 0;

                // search through all the neighbors of the current node evaluating
                // them as next steps
                for (int i = 0; i < 4; i++)
                {
                    switch (order[i])
                    {
                        case 1:
                            x = 1;
                            y = 0;
                            break;
                        case 2:
                            x = 0;
                            y = 1;
                            break;
                        case 3:
                            x = -1;
                            y = 0;
                            break;
                        case 4:
                            y = -1;
                            x = 0;
                            break;
                    }
                    // not a neighbor, its the current tile
                    if ((x == 0) && (y == 0))
                    {
                        continue;
                    }

                    // if we're not allowing diagonal movement then only 
                    // one of x or y can be set
                    if ((x != 0) && (y != 0))
                    {
                        continue;
                    }

                    // determine the location of the neighbor and evaluate it
                    int Nx = x + current.x;
                    int Ny = y + current.y;


                    // The neighbor tile will only evaluated if is a valid place and if is not outside of the movement range
                    // This last check will be done again.
                    if (isValidLocation(mover, Nx, Ny) && (Math.Abs(tx - Nx) + Math.Abs(ty - Ny) <= maxDistance))
                    {
                        // the cost to get to this node is cost the cost to reach this node. 
                        Node neighbour = nodes[Nx, Ny];
                        int nextStepCost = (int)(neighbour.cost + current.depth);

                        // If the current evaluation finds that this movement have less cost than
                        // previously evaluated, then the tile can be evaluated again to find a better path.
                        if (nextStepCost < neighbour.depth)
                        {
                            if (inOpenList(neighbour))
                            {
                                removeFromOpen(neighbour);
                            }
                            if (inClosedList(neighbour))
                            {
                                removeFromClosed(neighbour);
                            }
                        }

                        if (!inOpenList(neighbour) && !(inClosedList(neighbour)))
                        {
                            neighbour.heuristic = (float)Math.Sqrt((currentDistanceX * currentDistanceX) + (currentDistanceY * currentDistanceY));
                            // If moving to the neighbor tile doenst exceeds our movement range, then can be added.
                            // This check is done twice in case this tile has been reevaluated previously for a better path.
                            if (neighbour.cost + current.depth <= maxDistance)
                            {

                                neighbour.setParent(current);
                                addToOpen(neighbour);
                            }
                        }
                    }
                }
            }

            // since we've got an empty open list or we've run out of search 
            // there was no path. Just return null
            if (nodes[tx, ty].parent == null)
            {
                return null;
            }

            // At this point we've definitely found a path so we can uses the parent
            // references of the nodes to find out way from the target location back
            // to the start recording the nodes on the way.
            Path path = new Path();
            Node target = nodes[tx, ty];
            while (target != nodes[sx, sy])
            {
                path.prependStep(target.x, target.y, target.cost);
                target = target.parent;
            }
            path.prependStep(sx, sy, 0);

            // thats it, we have our path 
            return path;
        }

        /**
         * Get the first element from the open list. This is the next
         * one to be searched.
         * 
         * @return The first element in the open list
         */
        protected Node getFirstInOpen()
        {
            return (Node)open.first();
        }

        /**
         * Add a node to the open list
         * 
         * @param node The node to be added to the open list
         */
        protected void addToOpen(Node node)
        {
            open.add(node);
        }

        /**
         * Check if a node is in the open list
         * 
         * @param node The node to check for
         * @return True if the node given is in the open list
         */
        protected bool inOpenList(Node node)
        {
            return open.contains(node);
        }

        /**
         * Remove a node from the open list
         * 
         * @param node The node to remove from the open list
         */
        protected void removeFromOpen(Node node)
        {
            open.remove(node);
        }

        /**
         * Add a node to the closed list
         * 
         * @param node The node to add to the closed list
         */
        protected void addToClosed(Node node)
        {
            closed.Add(node);
        }

        /**
         * Check if the node supplied is in the closed list
         * 
         * @param node The node to search for
         * @return True if the node specified is in the closed list
         */
        protected bool inClosedList(Node node)
        {
            return closed.Contains(node);
        }

        /**
         * Remove a node from the closed list
         * 
         * @param node The node to remove from the closed list
         */
        protected void removeFromClosed(Node node)
        {
            closed.Remove(node);
        }

        /**
         * Check if a given location is valid for the supplied mover
         * 
         * @param mover The mover that would hold a given location
         * @param sx The starting x coordinate
         * @param sy The starting y coordinate
         * @param x The x coordinate of the location to check
         * @param y The y coordinate of the location to check
         * @return True if the location is valid for the given mover
         */
        protected bool isValidLocation(Unit mover, int x, int y)
        {
            //The tile must be inside the limits of the map.
            bool valid = (x >= 0) && (y >= 0) && (x < map.terrain.GetLength(0)) && (y < map.terrain.GetLength(0));

            //If is inside the limits of the map, will be checked if the tile is aviable for use.
            if (valid)
            {
                valid = this.map.isEmpty(x, y);
            }

            return valid;
        }

        /**
         *
         * @author Cesar Ramirez
         */
        public class SortedList
        {
            /** The list of elements */
            private List<Node> list = new List<Node>();
            private NodeComparer comparer;

            public void setComparer(NodeComparer comparer)
            {
                this.comparer = comparer;
            }

            /**
             * Retrieve the first element from the list
             *  
             * @return The first element from the list
             */
            public Object first()
            {
                return list.ElementAt(0);
            }

            /**
             * Empty the list
             */
            public void clear()
            {
                list.Clear();
            }

            /**
             * Add an element to the list - causes sorting
             * 
             * @param o The element to add
             */
            public void add(Node o)
            {
                list.Add(o);
                list.Sort(comparer);
            }

            /**
             * Remove an element from the list
             * 
             * @param o The element to remove
             */
            public void remove(Object o)
            {
                Node temp = (Node)o;
                list.Remove(temp);
            }

            /**
             * Get the number of elements in the list
             * 
             * @return The number of element in the list
             */
            public int size()
            {
                return list.Count;
            }

            /**
             * Check if an element is in the list
             * 
             * @param o The element to search for
             * @return True if the element is in the list
             */
            public bool contains(Object o)
            {
                return list.Contains(o);
            }
        }

        /**
         * A single node in the search graph
         */
        public class Node
        {
            /** The x coordinate of the node */
            public int x;
            /** The y coordinate of the node */
            public int y;
            /** The search depth of this node */
            public int depth;
            /** The move cost of this node */
            public int cost;
            /** The heuristic cost of this node */
            public float heuristic;
            /** The parent of this node, how we reached it in the search */
            public Node parent;


            /**
             * Create a new node
             * 
             * @param x The x coordinate of the node
             * @param y The y coordinate of the node
             */
            public Node(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            /**
             * Set the parent of this node
             * 
             * @param parent The parent node which lead us to this node
             * @return The depth we have no reached in searching
             */
            public int setParent(Node parent)
            {
                this.depth = (int)(parent.depth + this.cost);
                this.parent = parent;

                return depth;
            }
        }

        public class NodeComparer : IComparer<Node>
        {

            public int Compare(Node self, Node other)
            {
                Node o = other;

                float f = self.depth;
                float of = o.depth;

                if (f < of)
                {
                    return -1;
                }
                else if (f > of)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
