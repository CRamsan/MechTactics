/*Copyright (c) 2006  Laurent Cozic
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
using System.Text;
using System.Drawing;


namespace PolygonIntersection
{

    public class Polygon
    {

        private List<Vector> points = new List<Vector>();
        private List<Vector> edges = new List<Vector>();

        public void BuildEdges()
        {
            Vector p1;
            Vector p2;
            edges.Clear();
            for (int i = 0; i < points.Count; i++)
            {
                p1 = points[i];
                if (i + 1 >= points.Count)
                {
                    p2 = points[0];
                }
                else
                {
                    p2 = points[i + 1];
                }
                edges.Add(p2 - p1);
            }
        }

        public List<Vector> Edges
        {
            get { return edges; }
        }

        public List<Vector> Points
        {
            get { return points; }
        }

        public Vector Center
        {
            get
            {
                float totalX = 0;
                float totalY = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    totalX += points[i].X;
                    totalY += points[i].Y;
                }

                return new Vector(totalX / (float)points.Count, totalY / (float)points.Count);
            }
        }

        public void Offset(Vector v)
        {
            Offset(v.X, v.Y);
        }

        public void Offset(float x, float y)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Vector p = points[i];
                points[i] = new Vector(p.X + x, p.Y + y);
            }
        }

        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < points.Count; i++)
            {
                if (result != "") result += " ";
                result += "{" + points[i].ToString(true) + "}";
            }

            return result;
        }

    }

}

