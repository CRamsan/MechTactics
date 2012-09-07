using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MechTactics.Abstracts
{
    public class BaseMap
    {
        private List<Tuple<int, int>> initialPositions;

        public List<Tuple<int, int>> InitialPositions
        {
            get { return initialPositions; }
            set { initialPositions = value; }
        }

        public Tuple<int, int> GetInitalPosition(int index)
        {
            return initialPositions.ElementAt(index);
        }

        private int width;

        protected int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height;

        protected int Height
        {
            get { return height; }
            set { height = value; }
        }
        private int numberOfPlayers;

        protected int NumberOfPlayers
        {
            get { return numberOfPlayers; }
            set { numberOfPlayers = value; }
        }
        private bool isPrecise;

        protected bool IsPrecise
        {
            get { return isPrecise; }
            set { isPrecise = value; }
        }
    }
}
