using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainChompCorruptor;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Windows.Forms;

namespace ChainChomp
{
    public class Plugin
    {
        public IChainChompCorruptor corruptor;
        PluginSettings settings = new PluginSettings();
        static string tmpPath = ChainChompApplication.tempPath + @"plugin\";

        public Plugin(IChainChompCorruptor newInstance)
        {
            corruptor = newInstance;
            settings.pluginName = newInstance.Name;
            settings.pluginAuthor = newInstance.Author;
            settings.pluginVersion = newInstance.Version;
        }

        public Plugin(IChainChompCorruptor newInstance, string path) : this(newInstance)
        {
            LoadSettings(path, true);
        }

        public Plugin(string path)
        {
            LoadSettings(path);
            corruptor = PluginManager.Current.CreateMatchingCorruptor(settings);
            if (settings != null)
            {
                settings.Restore(corruptor);
            }
            else
            {
                MessageBox.Show("Attempted to load a Corruptor that is not installed.", "Error", MessageBoxButtons.OK);

            }
            
        }

        public override string ToString()
        {
            return settings.pluginName;
        }

        public bool SaveSettings(string path)
        {
            //grab settings from plugin
            settings.Store(corruptor);

            return FileIO.Write(path, settings, typeof(PluginSettings));
        }

        public bool LoadSettings(string path, bool applyImmediately = false)
        {
            PluginSettings newSettings = (PluginSettings)FileIO.Read(path, typeof(PluginSettings));

            if (newSettings != null && newSettings.pluginName == settings.pluginName || corruptor == null)
            {
                settings = newSettings;
                if (applyImmediately)
                {
                    settings.Restore(corruptor);
                }

            }
            else
            {
                return false;
            }

            return true;
        }

    }

}
