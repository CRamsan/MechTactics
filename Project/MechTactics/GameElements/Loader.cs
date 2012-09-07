using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MechTactics.Interfaces;
using MechTactics.Abstracts;
using MechTactics.GameElements.Attributes;
using MechTactics.GameElements;

namespace MechTactics
{
    public static class Loader
    {
        public static BaseServer getServer()
        {
            if (Constants.ASYNC_MODE)
            {
                return new AsyncServer();
            }
            else
            {
                return new SyncServer();
            }
        }

        public static ISimulator getSimulator()
        {
            if (Constants.TILED_MAP)
            {
                return new DiscreteSimulator();
            }
            else
            {
                return new RealSimulator();
            }
        }

        public static GameAttribute[] getInitialData() 
        {
            return null;
        }

        public static GameAttribute[] getInitialDataForPlayer(int index)
        {
            return null;
        }
    }


}
