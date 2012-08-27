using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MechTactics.Interfaces;
using MechTactics.GameElements.attributes;
using MechTactics.GameElements;

namespace MechTactics
{
    public class Loader
    {
        private String file;



        public BaseServer getServer()
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

        public GameAttribute[] getInitialData() 
        {
            return null;
        }

        public GameAttribute[] getInitialDataForPlayer(int index)
        {
            return null;
        }
    }


}
