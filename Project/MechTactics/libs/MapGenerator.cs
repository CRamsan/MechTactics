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
using MechTactics.GameElements.RealMap;
using MechTactics.GameElements;

namespace MechTactics
{
    public static class MapGenerator
    {
        public static BaseMap CreateMap(bool tiled, int players, int region_size, int number_regions, int max_height, double modifier, int seed)
        {
            if (tiled)
                return GetTiledMap(players, region_size, number_regions, max_height, modifier, seed);
            else
                return GetRealMap(players, region_size, number_regions, max_height, seed);
        }

        private static RealMap GetRealMap(int players, int region_size, int number_regions, int max_height, int seed)
        {
            throw new NotImplementedException();
        }

        private static TileMap GetTiledMap(int players, int region_size, int number_regions, int max_height, double modifier, int seed)
        {
            return new TileMap(InitializeLandscape(players, region_size, number_regions, max_height, modifier, seed));
        }

        private static int[,] InitializeLandscape(int players, int region_size, int number_regions, int max_height, double modifier, int seed)
        {
            int[,] map = new int[region_size * number_regions, region_size * number_regions];
            int[] xf = new int[(region_size * number_regions)];
            int[] yf = new int[(region_size * number_regions)];

            Random rand = new Random(seed);

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
            for (int i = 0; i < region_size * number_regions; i++)
            {
                for (int j = 0; j < region_size * number_regions; j++)
                {
                    int a = xf[i];
                    int b = yf[j];
                    int c = rand.Next(max_height) + 1;
                    map[i, j] = ((a + b + c) / 3);  //The height in a given tile is the average between the two functions and a independant modifier
                }
            }

            //Terrain leveling
            for (int i = 1; i < region_size * number_regions - 1; i++)
            {
                for (int j = 1; j < region_size * number_regions - 1; j++)
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
            for (int i = 0; i < number_regions; i++)
            {
                for (int j = 0; j < number_regions; j++)
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
                    int wz = rand.Next((number_regions * region_size) - (region_size));
                    int hz = rand.Next((number_regions * region_size) - (region_size));

                    //We will draw a square with starting points (af, bf)
                    int af = 0;
                    //generating map based on x and y and a random value
                    for (int k = wz; k < (wz + region_size); k++)
                    {
                        int bf = 0;
                        for (int l = hz; l < (hz + region_size); l++)
                        {
                            if ((k < number_regions * region_size) && (l < number_regions * region_size))
                            {
                                map[k, l] = Convert.ToInt32((map[k, l] + Convert.ToInt32((mf[af] * modifier * (double)positive)) + Convert.ToInt32((mf[bf] * modifier * (double)positive))));
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


            CleanRegion(players, region_size, number_regions, max_height, modifier, map);

            return map;
        }

        private static List<Tuple<int, int>> CleanRegion(int players, int region_size, int number_regions, int max_height, double modifier, int[,] map)
        {
            List<Tuple<int, int>> initialPositions = new List<Tuple<int, int>>(players);
            int player_n = 0;
            int center = (region_size + 1) / 2;
            for (int i = 0; i < number_regions; i++)
            {
                for (int j = 0; j < number_regions; j++)
                {
                    if ((i == 0 && j == 0) || (i == number_regions - 1 && j == number_regions - 1) || (i == number_regions - 1 && j == 0) || (i == 0 && j == number_regions - 1))
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
                                                initialPositions.Add(new Tuple<int, int>(k, l));
                                            }
                                            player_n++;
                                            break;

                                        case 4:
                                            map[k, l] = 0;
                                            initialPositions.Add(new Tuple<int, int>(k, l));
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
            return initialPositions;
        }
    }
}
