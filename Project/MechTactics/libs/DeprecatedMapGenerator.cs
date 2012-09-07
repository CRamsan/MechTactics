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
    public class DeprecatedMapGenerator
    {
        public int players;     //Numbers of bases    
        public int region_size; //Size of a block
        public int map_length;  //N number of blocks per side
        public int max_height;  //Maximun height for the map, 1 is always minimun
        public double height_modifier; //The modifier for the valley/mountain creater, this should be bigger the bigger the max_height is

        private int[,] map;          //Map, which is made by N*N blocks
        private KeyValuePair<int, int>[] initialPosition; //Array with initial location of players

        public Random rand;  //PRNG

        public DeprecatedMapGenerator(int _players, int _region_size, int _map_length, int _max_height, double _modifier)
        {
            this.players = _players;
            this.region_size = _region_size;
            this.map_length = _map_length;
            this.max_height = _max_height;
            this.height_modifier = _modifier;
            this.rand = new Random();
            this.initialPosition = new KeyValuePair<int, int>[_players];
        }

        public void CreateMap()
        {
            InitializeLandscape();
            CleanRegion();
        }

        public int[,] getMap()
        {
            return map;
        }

        private void InitializeLandscape()
        {
            map = new int[region_size * map_length, region_size * map_length];
            int[] xf = new int[(region_size * map_length)];
            int[] yf = new int[(region_size * map_length)];

            //Generating x and y function
            for (int i = 0; i < xf.Length; i++)
            {
                xf[i] = ((rand.Next(max_height) + 1));
            }
            for (int i = 0; i < yf.Length; i++)
            {
                yf[i] = ((rand.Next(max_height) + 1));
            }

            //generating map based on x and y and a random value
            for (int i = 0; i < region_size * map_length; i++)
            {
                for (int j = 0; j < region_size * map_length; j++)
                {
                    int a = xf[i];
                    int b = yf[j];
                    int c = rand.Next(max_height) + 1;
                    map[i, j] = ((a + b + c) / 3);  //The height in a given tile is the average between the two functions and a independant modifier
                }
            }

            //Terrain leveling
            for (int i = 1; i < region_size * map_length - 1; i++)
            {
                for (int j = 1; j < region_size * map_length - 1; j++)
                {
                    int a = map[i - 1, j];
                    int b = map[i + 1, j];
                    int c = map[i, j - 1];
                    int d = map[i, j + 1];
                    int e = map[i, j];
                    map[i, j] = Convert.ToInt32((a + b + c + d + e) / 5);//Also height is modified to an avarage of the surrounding tiles.
                }
            }

            //Mountain and valleys
            for (int i = 0; i < map_length; i++)
            {
                for (int j = 0; j < map_length; j++)
                {
                    //Holds the value of the sin function
                    double[] mf = new double[(region_size)];

                    //Generating sin function
                    int positive = rand.Next(2);

                    switch (positive)
                    {
                        case 0:
                            positive = -1;
                            break;
                        case 1:
                            positive = 1;
                            break;
                    }
                    for (int k = 0; k < mf.Length; k++)
                    {
                        double a = (((double)k / ((double)mf.Length))) * Math.PI;//Value between [0,PI)
                        mf[k] = (Math.Sin(a));
                    }
                    // Console.WriteLine

                    //Location where the sin will be applied
                    int wz = rand.Next((map_length * region_size) - (region_size));
                    int hz = rand.Next((map_length * region_size) - (region_size));

                    //We will draw a square with starting points (af, bf)
                    int af = 0;
                    //generating map based on x and y and a random value
                    for (int k = wz; k < (wz + region_size); k++)
                    {
                        int bf = 0;
                        for (int l = hz; l < (hz + region_size); l++)
                        {
                            if ((k < map_length * region_size) && (l < map_length * region_size))
                            {
                                map[k, l] = Convert.ToInt32((map[k, l] + Convert.ToInt32((mf[af] * height_modifier * (double)positive)) + Convert.ToInt32((mf[bf] * height_modifier * (double)positive))));
                                map[k, l] = Math.Max(map[k, l], 1);
                                map[k, l] = Math.Min(map[k, l], max_height);
                                //Console.WriteLine("[" + k + ", " + l + "] = " + map[k, l] + " Modified by " + (((mf[af] * 6.0 * (double)positive))));
                            }
                            bf++;
                        }
                        af++;
                    }

                }
            }
        }

        private void CleanRegion()
        {
            int player_n = 0;
            int center = (region_size + 1) / 2;
            for (int i = 0; i < map_length; i++)
            {
                for (int j = 0; j < map_length; j++)
                {
                    if ((i == 0 && j == 0) || (i == map_length - 1 && j == map_length - 1) || (i == map_length - 1 && j == 0) || (i == 0 && j == map_length - 1))
                    {
                        int a = 1;
                        for (int k = i * region_size; k < (i + 1) * region_size; k++)
                        {
                            int b = 1;
                            for (int l = j * region_size; l < (j + 1) * region_size; l++)
                            {
                                if ((a == center) && (b == center))
                                {
                                    switch (players)
                                    {
                                        case 2:
                                            map[k, l] = 0;
                                            if ((player_n % 3) == 0)
                                            {
                                                initialPosition[player_n / 3] = new KeyValuePair<int, int>(k, l);
                                            }
                                            player_n++;
                                            break;

                                        case 4:
                                            map[k, l] = 0;
                                            initialPosition[player_n] = new KeyValuePair<int, int>(k, l);
                                            player_n++;

                                            break;
                                    }
                                }
                                else if ((a == center) || (b == center))
                                {
                                    map[k, l] = 2;
                                }
                                else
                                {
                                    map[k, l] = 3;
                                }
                                b++;
                            }
                            a++;
                        }
                    }
                }
            }
        }

        public KeyValuePair<int, int> getInitialPosition(int i)
        {
            return initialPosition[i];
        }

        public KeyValuePair<int, int>[] getInitialPositions()
        {
            return initialPosition;
        }
    }
}
