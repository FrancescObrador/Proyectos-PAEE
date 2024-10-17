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
        private static TraceSource logger = new TraceSource("MonitorWPFLogging");
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
            logger.TraceEvent(TraceEventType.Information, 1, "Config: cargando configuración");
            lang = Read("lang");
            color = Read("color");
            logger.TraceEvent(TraceEventType.Information, 1, "Config: color leído: " + color);
            logger.TraceEvent(TraceEventType.Information, 1, "Config: idioma leído: " + lang);
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
            logger.TraceEvent(TraceEventType.Information, 1, "Config: Intentando guardar: " + lang);
            Write(configFile, "lang", lang);
            logger.TraceEvent(TraceEventType.Information, 1, "Config: Intentando guardar: " + color);
            Write(configFile, "color", color);
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
