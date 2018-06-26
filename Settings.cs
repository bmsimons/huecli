using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace huecli
{
    public class SettingsModel
    {
        public string version { get; set; }
        public List<Dictionary<string, string>> hubs { get; set; }
    }

    public class Settings
    {
        public string SettingsFile { get; set; }
        public SettingsModel SettingsDictionary { get; set; }
        private JSchema SettingsSchema { get; set; }

        public Settings(string file = null)
        {
            SettingsSchema = JSchema.Parse(@"{
                'version': '1',
                'hubs': {'type': 'array'}
            }");

            if (file == null)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    SettingsFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+"\\huecli-settings.json";
                }
                else
                {
                    SettingsFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+"/huecli-settings.json";
                }
            }
            else
            {
                SettingsFile = file;
            }

            this.Load();
        }

        private void InitSettings()
        {
            SettingsDictionary = new SettingsModel();

            SettingsDictionary.version = "1";
            SettingsDictionary.hubs = new List<Dictionary<string, string>>();
        }

        public void Load()
        {
            if (!File.Exists(SettingsFile))
            {
                File.Create(SettingsFile).Close();

                this.InitSettings();
            }
            else
            {
                string contents = File.ReadAllText(SettingsFile);
                if (contents.StartsWith("{") && contents.EndsWith("}"))
                {
                    JObject settingsObject = JObject.Parse(contents);
                    if (settingsObject.IsValid(this.SettingsSchema))
                    {
                        SettingsDictionary = JsonConvert.DeserializeObject<SettingsModel>(contents);
                    }
                    else
                    {
                        this.InitSettings();
                    }
                }
                else
                {
                    this.InitSettings();
                }
            }
        }

        public void Save()
        {
            File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(SettingsDictionary));
        }

        public List<Dictionary<string, string>> GetHubs()
        {
            return SettingsDictionary.hubs;
        }

        public void AddHub(Dictionary<string, string> hub)
        {
            for (int i = 0; i < SettingsDictionary.hubs.Count; i++)
            {
                if (SettingsDictionary.hubs[i]["alias"] == hub["alias"] || SettingsDictionary.hubs[i]["localipaddress"] == hub["localipaddress"])
                {
                    return;
                }
            }
            SettingsDictionary.hubs.Add(hub);
        }

        public string GetUsername(String hubAlias)
        {
            for (int i = 0; i < SettingsDictionary.hubs.Count; i++)
            {
                if (hubAlias == SettingsDictionary.hubs[i]["alias"])
                {
                    return SettingsDictionary.hubs[i]["username"];
                }
            }
            return "";
        }

        public string GetIPAddress(String hubAlias)
        {
            for (int i = 0; i < SettingsDictionary.hubs.Count; i++)
            {
                if (hubAlias == SettingsDictionary.hubs[i]["alias"])
                {
                    return SettingsDictionary.hubs[i]["localipaddress"];
                }
            }
            return "";
        }

        public void RemoveHub(String hubAlias)
        {
            for (int i = 0; i < SettingsDictionary.hubs.Count; i++)
            {
                if (hubAlias == SettingsDictionary.hubs[i]["alias"])
                {
                    SettingsDictionary.hubs.RemoveAt(i);
                    break;
                }
            }
        }
    }
}