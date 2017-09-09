using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChainChomp
{
    public static class ImageEmuLibrary
    {
        private static string libraryPath = ChainChompApplication.appDataPath + "imageEmu.lbs";

        public static List<string[]> images = new List<string[]>();
        public static List<string> emus = new List<string>();

        public static void SaveSettings()
        {
            if (File.Exists(libraryPath))
            {
                File.Delete(libraryPath);
            }
            //create settings obj
            LibrarySettings settings = new LibrarySettings();
            settings.Store();

            FileIO.Write(libraryPath, settings, typeof(LibrarySettings));
        }

        public static void LoadSettings()
        {
            if (!File.Exists(libraryPath))
            {
                SaveSettings();
            }

            //preare settings obj
            LibrarySettings settings = (LibrarySettings)FileIO.Read(libraryPath, typeof(LibrarySettings));
            if (settings != null)
            {
                settings.Restore();
            }
        }
    }
}
