using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MechTactics.Abstracts;

namespace MechTactics.GameElements.RealMap
{
    public class RealMap : BaseMap
    {
        public List<BaseRealGameObject> elements;

        public RealMap()
        { }

        public BaseRealGameObject getObjectAt(double x, double y)
        {

            foreach (BaseRealGameObject gameObject in elements)
            {
                if (gameObject.Pres_x == x && gameObject.Pres_y == y)
                {
                    return gameObject;
                }
            }
            return null;
        }

        public void remove(BaseRealGameObject element)
        {
            this.elements.Remove(element);
            this.terrain[element.X, element.Y].elements.Remove(element);
        }

        public void add(BaseRealGameObject element)
        {
            this.elements.Add(element);
            this.terrain[element.X, element.Y].elements.Add(element);
        }

        public bool isInRange(BaseRealGameObject BaseRealGameObject, int x, int y)
        {
            return isInRange(BaseRealGameObject, x, y, 0);
        }

        public bool isInRange(BaseRealGameObject BaseRealGameObject, int x, int y, int mod)
        {
            int dx = Math.Abs(BaseGameObject.X - x);
            int dy = Math.Abs(BaseGameObject.Y - y);

            if ((dx + dy) <= (((Unit)BaseGameObject).Move + mod))
            {
                if (true)
                { }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void clearElements()
        {
            elements.Clear();
        }
    }
}
