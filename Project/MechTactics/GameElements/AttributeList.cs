using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MechTactics.GameElements.Attributes;

namespace MechTactics.GameElements
{
    public class AttributeList
    {
        private List<GameAttribute> list;

        public AttributeList() 
        {
            this.list = new List<GameAttribute>(0);
        }

        public AttributeList(String attributes)
        {
        }

        public AttributeList(List<GameAttribute> attributeList)
        {
            this.list = attributeList;
        }

        public void addAttribute(GameAttribute attribute)
        {
            this.list.Add(attribute);
        }

        public string toString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (GameAttribute attribute in list)
            {
                attribute.ToString();
            }
            return sb.ToString();
        }

        public List<GameAttribute> List
        {
            get { return list; }
            set { list = value; }
        }

    }
}
