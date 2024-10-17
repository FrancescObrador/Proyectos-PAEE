using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

namespace MontiorWPFBlend
{
    public class Config
    {
        public string lang { get; set; }
        public string color { get; set; }
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
            lang = "es";
            color = "#FF0000";
        }

        public void Load()
        {
            lang = Read("lang");
            color = Read("color");
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
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
