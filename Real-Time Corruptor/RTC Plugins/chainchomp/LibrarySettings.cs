using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChainChomp
{
    [DataContract]
    class LibrarySettings
    {

        [DataMember]
        List<string[]> images = new List<string[]>();

        [DataMember]
        List<string> emus = new List<string>();

        public void Store()
        {
            emus.Clear();
            images.Clear();
            emus.AddRange(ImageEmuLibrary.emus);
            images.AddRange(ImageEmuLibrary.images);
        }

        public void Restore()
        {
            ImageEmuLibrary.emus.Clear();
            ImageEmuLibrary.images.Clear();
            ImageEmuLibrary.emus.AddRange(emus);
            ImageEmuLibrary.images.AddRange(images);
        }

    }
}
