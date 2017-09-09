using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainChompCorruptor;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Runtime.Serialization;

namespace ChainChomp
{
    public class Chain
    {
        private List<Plugin> plugins;
        public int startOffset = 0;
        public int endOffset = 65535;
        public string name = "New Chain";
        public List<int> headerIndices;

        public Chain()
        {
            plugins = new List<Plugin>();

        }

        public Chain(string path) : this()
        {
            LoadChain(path);
        }


        public List<Plugin> GetPlugins()
        {
            return plugins;
        }

        public ROMSample RunChain(ROMSample sample)
        {
            IEnumerable<byte> input = sample.data.AsParallel();
            plugins.ForEach(x => input = x.corruptor.Corrupt(input));
            sample.data = input;

            return sample;
        }

        public Plugin AddPlugin(int index)
        {
            Plugin p = PluginManager.Current.CreatePlugin(index);
            plugins.Add(p);

            return p;
        }

        public void RemoveCorruptor(int index)
        {
            plugins.RemoveAt(index);
        }

        public void RemoveCorruptor(Plugin p)
        {
            plugins.Remove(p);
        }

        public void RemoveCorruptor(Control c)
        {
            Plugin p = plugins.First(i => i.corruptor.GetUserControl() as Control == c);
            plugins.Remove(p);
        }

        public List<IComponent> GetControls()
        {
            List<IComponent> ctrls = new List<IComponent>();
            plugins.ForEach(i => ctrls.Add(i.corruptor.GetUserControl()));

            return ctrls;
        }

        public void ShiftPlugin(Plugin pluginToShift, int direction)
        {
            direction = direction < 0 ? -1 : 1; //enforce single element-shifting only
            int n = plugins.IndexOf(pluginToShift);
            int i = n + direction;
            if (i > -1 && i < plugins.Count())
            {
                Plugin tmp = plugins[i];
                plugins[i] = pluginToShift;
                plugins[n] = tmp;
            }

        }

        public bool SaveChain(string path, List<int> headerIndices)
        {
            //save plugins to temp
            ChainChompApplication.FlushTempDir();

            for (int i = 0; i < plugins.Count(); i++ )
            {
                if (!plugins[i].SaveSettings(ChainChompApplication.tempPath + plugins[i].ToString() + i + ".donut"))
                {
                    return false;
                }
            }

            //save offsets & backgrounds
            ChainSettings settings = new ChainSettings();
            settings.Store(startOffset, endOffset, headerIndices);
            if (!FileIO.Write(ChainChompApplication.tempPath + "vanilla.dome", settings, typeof(ChainSettings)))
            {
                return false;
            }

            //zip files to destination
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            ZipFile.CreateFromDirectory(ChainChompApplication.tempPath, path);
            name = Path.GetFileName(path);

            return true;
        }

        public void LoadChain(string path)
        {
            //extract to 
			DirectoryInfo temp = ChainChompApplication.ExtractToTemp(path);
			if (temp == null)
				return;

            foreach(FileInfo file in temp.EnumerateFiles("*.donut"))
            {
                Plugin p = new Plugin(file.FullName);
                plugins.Add(p);
            }

            ChainSettings settings = (ChainSettings)FileIO.Read(ChainChompApplication.tempPath + "vanilla.dome", typeof(ChainSettings));
            if (settings != null)
            {
                startOffset = settings.start;
                endOffset = settings.end;
                name = Path.GetFileName(path);
                headerIndices = settings.headerIndices;
            }
         
        }

    }
}
