using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainChomp
{
    public class PluginMoveEventArgs : EventArgs
    {
        public Plugin movingPlugin;

        public PluginMoveEventArgs()
        {

        }

        public PluginMoveEventArgs(Plugin p)
        {
            movingPlugin = p;
        }

    }

}
