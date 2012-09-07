using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MechTactics;

namespace MechTacticsClient.Interfaces
{
    interface ILogic
    {
        void setAttributes(String[] attributes);
        List<Command> getCommand(Snapshot snapshot);
    }
}
