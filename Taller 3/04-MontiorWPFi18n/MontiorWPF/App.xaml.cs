using System.Configuration;
using System.Data;
using System.Windows;

namespace MonitorWPFi18n
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, EventArgs e)
        {
            Config.GetInstance().Save();
        }
    }

}
