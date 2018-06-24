using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace huecli
{
    public class Preferences
    {
        public string PreferencesFile { get; set; }
        public string PreferencesString { get; set; }
        public FileStream fs { get; set; }

        public Preferences(string file = null)
        {
            if (file == null)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    PreferencesFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+"\\huecli-preferences.json";
                }
                else
                {
                    PreferencesFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+"/huecli-preferences.json";
                }
            }
            else
            {
                PreferencesFile = file;
            }
        }

        public void Load()
        {
            fs = File.Open(PreferencesFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }

        public void Save()
        {
            byte[] contents = new UTF8Encoding(true).GetBytes(PreferencesString);
            fs.Write(contents, 0, contents.Length);
        }

        public void Close()
        {
            fs.Close();
        }
    }
}