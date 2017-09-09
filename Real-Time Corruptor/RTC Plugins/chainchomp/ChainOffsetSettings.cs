using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChainChomp
{
    [DataContract]
    public class ChainSettings
    {
        [DataMember]
        public int start;

        [DataMember]
        public int end;

        [DataMember]
        public List<int> headerIndices;

        public void Store(int s, int e, List<int> h)
        {
            start = s;
            end = e;
            headerIndices = h;
        }
    }
}
