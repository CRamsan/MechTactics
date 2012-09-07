using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PolygonIntersection;

namespace MechTactics.Abstracts
{
    public abstract class BaseRealGameObject : BaseGameObject
    {
        public BaseRealGameObject(int id, int team, char type)
        { }

        protected double pres_x;

        public double Pres_x
        {
            get { return pres_x; }
            set { pres_x = value; }
        }
        protected double pres_y;

        public double Pres_y
        {
            get { return pres_y; }
            set { pres_y = value; }
        }

        protected double speed;

        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }


        protected Polygon body;

        public Polygon Body
        {
            get { return body; }
            set { body = value; }
        }

    }
}
