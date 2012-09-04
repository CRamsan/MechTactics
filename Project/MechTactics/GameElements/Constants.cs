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

namespace MechTactics.GameElements
{
    public static class Constants
    {
        public const bool TILED_MAP = false;
        public const bool ASYNC_MODE = true;
        public const bool SAVE_GAME = false;

        //Define Units

        public const int STARTING_ORE = 100;
        public const int ORE_PER_MINE = 2;

        public const int MAX_HEALTH = 100;

        //This are the constants that represent each kind of unit or building
        //Base 'A'
        //Mine 'B'
        //Barrack 'C':
        //Factory 'D':
        //Portal 'E':
        //Worker 'L'
        //Basic infactry 'M':
        //Advanced Infantry 'N':
        //Sniper 'O':
        //Light Tank 'P':
        //Heavy Tank 'Q':
        //Artillery 'R':
        //Comando 'S':

        //For actions:
        //Attacking 'X'
        //Moving 'Y'
        //For Building/Creating use the Char that represents what you want to create

        public const int COST_BASE      = 10000;
        public const int COST_BARRACK   = 500;
        public const int COST_MINE      = 400;
        public const int COST_PORTAL    = 600;
        public const int COST_FACTORY   = 700;

        public const int COST_WORKER    = 250;
        public const int COST_INFANTRY  = 60;
        public const int COST_HINFANTRY = 75;
        public const int COST_SNIPER    = 120;
        public const int COST_LTANK     = 150;
        public const int COST_HTANK     = 180;
        public const int COST_ARTILLERY = 200;
        public const int COST_COMANDO   = 200;

        public const int DAMAGE_WORKER      = 0;
        public const int DAMAGE_INFANTRY    = 10;
        public const int DAMAGE_HINFANTRY   = 20;
        public const int DAMAGE_SNIPER      = 40;
        public const int DAMAGE_LTANK       = 30;
        public const int DAMAGE_HTANK       = 40;
        public const int DAMAGE_ARTILLERY   = 50;
        public const int DAMAGE_COMANDO     = 25;

        public const float ARMOR_WORKER     = 1f;
        public const float ARMOR_INFANTRY   = 0.5f;
        public const float ARMOR_HINFANTRY  = 0.4f;
        public const float ARMOR_SNIPER     = 0.7f;
        public const float ARMOR_LTANK      = 0.35f;
        public const float ARMOR_HTANK      = 0.3f;
        public const float ARMOR_ARTILLERY  = 0.4f;
        public const float ARMOR_COMANDO    = 0.3f;

        public const float ARMOR_BASE       = 0.05f;
        public const float ARMOR_BARRACK    = 0.2f;
        public const float ARMOR_MINE       = 0.1f;
        public const float ARMOR_FACTORY    = 0.2f;
        public const float ARMOR_PORTAL     = 0.1f;

        public const int MOVE_WORKER        = 10;
        public const int MOVE_INFANTRY      = 30;
        public const int MOVE_HINFANTRY     = 20;
        public const int MOVE_SNIPER        = 15;
        public const int MOVE_LTANK         = 35;
        public const int MOVE_HTANK         = 30;
        public const int MOVE_ARTILLERY     = 10;
        public const int MOVE_COMANDO       = 20;

        public const int RANGE_WORKER       = 1;
        public const int RANGE_INFANTRY     = 1;
        public const int RANGE_HINFANTRY    = 1;
        public const int RANGE_SNIPER       = 4;
        public const int RANGE_LTANK        = 1;
        public const int RANGE_HTANK        = 1;
        public const int RANGE_ARTILLERY    = 6;
        public const int RANGE_COMANDO      = 1;

        /*public const int MAXHEALTH_BASE = 100;
        public const int MAXHEALTH_FACTORY = 100;
        public const int MAXHEALTH_MINE = 100;
        public const int MAXHEALTH_PORTAL = 100;
        public const int MAXHEALTH_FACTORY = 100;

        public const int MAXHEALTH_INFANTRY = 100;
        public const int MAXHEALTH_HINFANTRY = 100;
        public const int MAXHEALTH_SNIPER = 100;
        public const int MAXHEALTH_LTANK = 100;
        public const int MAXHEALTH_HTANK = 100;
        public const int MAXHEALTH_ARTILLERY = 100;
        public const int MAXHEALTH_COMANDO = 100;*/
    }
}
