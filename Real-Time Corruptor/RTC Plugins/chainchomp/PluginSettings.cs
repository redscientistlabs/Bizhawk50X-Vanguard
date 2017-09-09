using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using ChainChompCorruptor;
using System.Reflection;
using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;

namespace ChainChomp
{
    [DataContract]
    class PluginSettings
    {
        [DataMember]
        public Dictionary<string, object> data = new Dictionary<string, object>();

        [DataMember]
        public string pluginName;

        [DataMember]
        public string pluginAuthor;

        [DataMember]
        public string pluginVersion;

        public void Store(IChainChompCorruptor corruptor)
        {
            Type t = corruptor.GetType();
            data = new Dictionary<string, dynamic>();

            foreach (PropertyInfo prop in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (Attribute.IsDefined(prop, typeof(SaveableSetting)))
                {
                    dynamic value = prop.GetValue(corruptor);
                    string name = prop.Name;

                    data.Add(name, value);
                }
            }
        }

        public void Restore(IChainChompCorruptor corruptor)
        {
            Type t = corruptor.GetType();
            foreach (PropertyInfo prop in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (Attribute.IsDefined(prop, typeof(SaveableSetting)))
                {
                    object value = data[prop.Name];

                    prop.SetValue(corruptor, Convert.ChangeType(value, prop.PropertyType));
                }
            }
        }

        // hacky list-enabled settings. This took too long to get working to delete. Maybe it will come in use some day...

        //public void Store(IChainChompCorruptor corruptor, string path)
        //{
        //    Type t = corruptor.GetType();
        //    data = new Dictionary<string, dynamic>();

        //    foreach(PropertyInfo prop in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        //    {
        //        if (Attribute.IsDefined(prop, typeof(SaveableSetting)))
        //        {
        //            dynamic value = prop.GetValue(corruptor);
        //            string name = prop.Name;

        //            if (value is ICollection)
        //            {
        //                //save subitem
        //                PluginSubSettings subset = new PluginSubSettings();
        //                subset.Store(value);
        //                FileIO.Write(string.Format("{0}{1}.special", path, name), subset, typeof(PluginSubSettings));
        //                data.Add(name, string.Format("SUBITEM:{0}.special", name));
        //            }
        //            else
        //            {
        //                data.Add(name, value);
        //            }
        //        }
        //    }
        //}

        //public void Restore(IChainChompCorruptor corruptor, string path)
        //{
        //    Type t = corruptor.GetType();
        //    foreach (PropertyInfo prop in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        //    {
        //        if (Attribute.IsDefined(prop, typeof(SaveableSetting)))
        //        {
        //            object value = data[prop.Name];

        //            if (value.ToString().IndexOf("SUBITEM:") == 0)
        //            {
        //                string subPath = path + value.ToString().Substring(8);
        //                PluginSubSettings subSettings = (PluginSubSettings)FileIO.Read(subPath, typeof(PluginSubSettings));
        //                ICollection collection = subSettings.data;
        //                prop.SetValue(corruptor, Activator.CreateInstance(prop.PropertyType));
        //                var x = prop.GetValue(corruptor);
        //                foreach (dynamic d in collection)
        //                {
        //                    Type colT = collection.GetType();
        //                    (x as IList).Add(d);
        //                }
        //                prop.SetValue(corruptor, x);
                        
        //            }
        //            else
        //            {
        //                prop.SetValue(corruptor, Convert.ChangeType(value, prop.PropertyType));

        //            }
        //        }
        //    }
        //}
    }
}
