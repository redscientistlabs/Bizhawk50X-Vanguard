using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChainChomp
{
    [DataContract]
    class PluginSubSettings
    {
        [DataMember]
        public List<object> data = new List<object>();

        public void Store(ICollection collection)
        {
            foreach (object obj in collection)
            {
                data.Add(obj);
            }
        }
    }
}
