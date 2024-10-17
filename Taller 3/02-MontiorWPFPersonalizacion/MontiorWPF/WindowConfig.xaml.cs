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

        public WindowConfig()
        {
            InitializeComponent();

            Config.GetInstance().Load();
            var lang = Config.GetInstance().lang;
            if(lang != null)
            {
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
                cpColor.SelectedColor = (Color?)ColorConverter.ConvertFromString(color);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();

            if ((bool)rbLangES.IsChecked)
            {
                Config.GetInstance().lang = "ES";
            }
            else if((bool)rbLangEN.IsChecked)
            {
                Config.GetInstance().lang = "EN";
            }

            string color = cpColor.SelectedColor.Value.ToString();
            Config.GetInstance().color = color;
            Config.GetInstance().Save();
        }
    }
}
