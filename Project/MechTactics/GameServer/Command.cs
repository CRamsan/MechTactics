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
    public class Command
    {
        public const String TAG = "command";

        private int type;
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private int playerId;
        public int PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }

        private int objectId;
        public int ObjectId
        {
            get { return objectId; }
            set { objectId = value; }
        }

        private int objectiveX;
        public int ObjectiveX
        {
            get { return objectiveX; }
            set { objectiveX = value; }
        }

        private int objectiveY;
        public int ObjectiveY
        {
            get { return objectiveY; }
            set { objectiveY = value; }
        }

        public override string ToString()
        {
            return TAG + "#" + type.ToString() + "#" + playerId.ToString() + "#" + objectId.ToString() + "#" + objectiveX + "#" + objectiveY;
        }
    }
}
