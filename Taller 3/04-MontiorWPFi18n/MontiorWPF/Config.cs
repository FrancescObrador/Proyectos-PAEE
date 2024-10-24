using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Numerics;

namespace MonitorWPFi18n
{
    public class Config
    {
        public string lang { get; set; }
        public string color { get; set; }
        public string left { get; set; }
        public string top { get; set; }

        private static Config _instance;

        private Config() { }

        public static Config GetInstance()
        {
            if(_instance == null )
            {
                _instance = new Config();
            }
            return _instance;
        }

        public void InitDefaults()
        {
            lang = "en-US";
            color = "#FF0000";
            left = "0";
            top = "0";
        }

        public void Load()
        {
            lang = Read("lang");
            color = Read("color");
            left = Read("left");
            top = Read("top");
        }

        public string Read(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private void Write(Configuration configFile, string key, string value)
        {
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
        }

        public void Save()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Write(configFile, "lang", lang);
            Write(configFile, "color", color);
            Write(configFile, "left", left);
            Write(configFile, "top", top);
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
