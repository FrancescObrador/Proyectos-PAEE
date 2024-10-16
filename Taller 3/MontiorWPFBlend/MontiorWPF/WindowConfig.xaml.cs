using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MontiorWPFBlend
{
    /// <summary>
    /// Interaction logic for WindowConfig.xaml
    /// </summary>
    public partial class WindowConfig : Window
    {
        private static TraceSource logger = new TraceSource("MonitorWPF");

        public WindowConfig()
        {
            InitializeComponent();

            Config.GetInstance().Load();
            var lang = Config.GetInstance().lang;
            if(lang != null)
            {
                logger.TraceEvent(TraceEventType.Information, 1, "WindowConfig: Iniciando con idioma: " + lang);
                if (lang.Equals("ES"))
                {
                    rbLangES.IsChecked = true;
                }
                else if (lang.Equals("EN"))
                {
                    rbLangEN.IsChecked = true;
                }
            }

            var color = Config.GetInstance().color;
            if (color != null) 
            {
                logger.TraceEvent(TraceEventType.Information, 1, "WindowConfig: Iniciando con color: " + color);
                cpColor.SelectedColor = (Color?)ColorConverter.ConvertFromString(color);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            logger.TraceEvent(TraceEventType.Information, 1, "WindowConfig: ventana cerrada");

            if ((bool)rbLangES.IsChecked)
            {
                logger.TraceEvent(TraceEventType.Information, 1, "WindowConfig: lenguaje a guardar: ES");
                Config.GetInstance().lang = "ES";
            }
            else if((bool)rbLangEN.IsChecked)
            {
                logger.TraceEvent(TraceEventType.Information, 1, "WindowConfig: lenguaje a guardar: EN");
                Config.GetInstance().lang = "EN";
            }

            string color = cpColor.SelectedColor.Value.ToString();
            Config.GetInstance().color = color;
            logger.TraceEvent(TraceEventType.Information, 1, "WindowConfig: color a guardar: " + color);
            Config.GetInstance().Save();
        }
    }
}
