using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using ChainChompCorruptor;
using System.IO.Compression;

namespace ChainChomp
{
    class PluginManager
    {
        private static PluginManager current;

        private List<IChainChompCorruptor> _plugins;
        private string pluginPath = Application.StartupPath + @"\corruptors";

        private PluginManager()
        {
            //populate dictionary
            _plugins = new List<IChainChompCorruptor>();
            ICollection<IChainChompCorruptor> loadedPlugins = LoadPlugins(pluginPath);
            loadedPlugins.ToList().ForEach(item =>
            {
                //found a plugin! stick it in the list
                _plugins.Add(item);

                //check for factory presets
                string path = ChainChompApplication.factoryPresetsPath + item.Name + @"\";
                //create dir
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                byte[] presets = (byte[])item.GetFactoryPresets();
                if (presets != null)
                {

                    string tmp = ChainChompApplication.tempPath + "tmp";
                    //extract zip (cannot use ZipFile.ExtractTo because we are overwriting
                    ChainChompApplication.FlushTempDir();
                    FileIO.WriteAllBytes(tmp, presets);

                    //overwrite factory presets
                    ZipArchive z = null;
                    try
                    {
                        z = ZipFile.OpenRead(tmp);
                        z.Entries.ToList().ForEach(e => e.ExtractToFile(path + e.FullName, true));
                        z.Dispose();
                    }
                    catch (IOException e)
                    {
                        MessageBox.Show("Factory presets could not be created");
                        z.Dispose();
                    }

                }
            });
        }

        public static PluginManager Current
        {
            get
            {
                if (current == null)
                {
                    current = new PluginManager();
                }

                return current;
            }
        }

        //usage methods
        public IChainChompCorruptor GetCorruptor(int index)
        {
            return _plugins[index];
        }

        public IChainChompCorruptor GetCorruptor(ComboBoxItem selection)
        {
            return _plugins[(int)selection.Value];
        }

        public ComboBoxItem[] GetCorruptorNames()
        {
            ComboBoxItem[] result = new ComboBoxItem[_plugins.Count];
            for (int i = 0; i < _plugins.Count; i++)
            {
                result[i] = new ComboBoxItem(_plugins[i].Name, i);
            }
            return result;
        }

        public IChainChompCorruptor FindMatchingPlugin(PluginSettings settings)
        {
            return _plugins.FirstOrDefault(n => n.Name == settings.pluginName && n.Author == settings.pluginAuthor);
        }

        public Plugin LoadPlugin(string path)
        {
            Plugin p = new Plugin(path);
            return p;
        }

        public Plugin CreatePlugin(int index)
        {
            Plugin p = new Plugin(CreateCorruptor(index));
            return p;
        }

        public IChainChompCorruptor CreateCorruptor(int index)
        {
            Object obj = _plugins[index];
            Type t = obj.GetType();

            return (IChainChompCorruptor)Activator.CreateInstance(t);
        }

        public IChainChompCorruptor CreateMatchingCorruptor(PluginSettings settings)
        {
            IChainChompCorruptor x = FindMatchingPlugin(settings);
            return x != null ? CreateCorruptor(_plugins.IndexOf(x)) : null;
        }

        //plugin loader
        private ICollection<IChainChompCorruptor> LoadPlugins(string path)
        {
            //aggregate files
            string[] dllFileNames = new string[0];
            if (Directory.Exists(path))
            {
                dllFileNames = Directory.GetFiles(path, "*.dll");
            }

            //load assemblies
            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dll in dllFileNames)
            {
				AssemblyName assN;
				try
				{
					assN = AssemblyName.GetAssemblyName(dll);
				}
				catch (FileLoadException e)
				{
					MessageBox.Show(e.Message);
					continue;
				}

				Console.WriteLine(assN.FullName);
				Assembly ass;
				try
				{
					ass = Assembly.Load(assN);
				}
				catch (FileLoadException e)
				{
					MessageBox.Show(e.Message);
					continue;
				}
                assemblies.Add(ass);
            }

            //search assemblies for corruptors
            Type corruptorType = typeof(IChainChompCorruptor);
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(corruptorType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }

            //create instances
            ICollection<IChainChompCorruptor> plugins = new List<IChainChompCorruptor>(pluginTypes.Count);
            foreach (Type type in pluginTypes)
            {
                IChainChompCorruptor corruptor = (IChainChompCorruptor)Activator.CreateInstance(type);
                plugins.Add(corruptor);

            }

            return plugins;
        }


    }
}
