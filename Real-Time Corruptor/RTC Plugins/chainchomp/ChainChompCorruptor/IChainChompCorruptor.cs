using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Reflection;


namespace ChainChompCorruptor
{
    public interface IChainChompCorruptor
    {
        string Name { get; }
        string Author { get; }
        string Version { get; }
        string PresetExt { get; }
        IEnumerable<byte> Corrupt(IEnumerable<byte> input);
        IComponent GetUserControl();
        IComponent GetBackgroundContainer();
        IEnumerable<byte> GetFactoryPresets();
    }

    public static class PluginUtility
    {
        public static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }
    }
}
