using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MonitorWPFPersonalizacion
{
    /// <summary>
    /// Interaction logic for WindowConfig.xaml
    /// </summary>
    public partial class WindowConfig : Window
    {

        public WindowConfig()
        {
            InitializeComponent();

            Config.GetInstance().Load();

            //Load language
            var lang = Config.GetInstance().lang;
            if(lang != null)
            {
                if (lang.Equals("es"))
                {
                    rbLangES.IsChecked = true;
                }
                else if (lang.Equals("en"))
                {
                    rbLangEN.IsChecked = true;
                }
            }

            // Load color
            var color = Config.GetInstance().color;
            if (color != null) 
            {
                cpColor.SelectedColor = (Color?)ColorConverter.ConvertFromString(color);
            }

            // Load window pos
            this.Left = Convert.ToDouble(Config.GetInstance().left);
            this.Top = Convert.ToDouble(Config.GetInstance().top);

            lblPosition.Content = "Posición: (" + Config.GetInstance().left + "," + Config.GetInstance().top + ")";

        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            Close();

            // Guardar lang
            if ((bool)rbLangES.IsChecked)
            {
                Config.GetInstance().lang = "es";
            }
            else if ((bool)rbLangEN.IsChecked)
            {
                Config.GetInstance().lang = "en";
            }

            // Guardar color
            string color = cpColor.SelectedColor.Value.ToString();
            Config.GetInstance().color = color;

            // Guardar posicion de la nueva ventana
            Config.GetInstance().left = this.Left.ToString();
            Config.GetInstance().top = this.Top.ToString();
            
            Config.GetInstance().Save();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
