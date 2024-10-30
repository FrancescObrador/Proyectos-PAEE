using DataAccess;
using ExportPDF;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ProvinciasJSON provinciasJSON;
        private List<Provincia> provincias;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void orderProvincias(bool porNombre)
        {
            if (porNombre)
            {
                provincias = provincias.OrderBy(o => o.Nombre).ToList();
            }
            else
            {
                provincias = provincias.OrderBy(o => o.ID).ToList();
            }
        }

        private void showProvincias()
        {
            provinciasStackPanel.Children.Clear();

            foreach(Provincia provincia in provincias)
            {
                ProvinciaUserControl provinciaUserControl = new ProvinciaUserControl
                {
                    Provincia = provincia
                };

                provinciasStackPanel.Children.Add(provinciaUserControl);
            }
        }

        private void activarBotones(bool activar) 
        {
            foreach(Button b in botonesStackPanel.Children)
            {
                if(b.Name != "cargarButton")
                {
                    b.IsEnabled = activar;
                }
            }
        }

        private bool guardar()
        {
            bool res = provinciasJSON.saveProvincias(provincias);
            if (!res)
            {
                MessageBox.Show("Error guardando datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                MessageBox.Show("Datos guardados correctament en " + provinciasJSON.Path, "Datos guardados", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
        }

        // Button Events

        private void idButton_Click(object sender, RoutedEventArgs e)
        {
            orderProvincias(false);
            showProvincias();
        }

        private void nombreButton_Click(object sender, RoutedEventArgs e)
        {
            orderProvincias(true);
            showProvincias();
        }

        private void exportarButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo PDF|*.pdf";
            if(saveFileDialog.ShowDialog() == true)
            {
                ProvinciasPDF provinciasPDF = new ProvinciasPDF();
                if(provinciasPDF.SavePDF(provincias, saveFileDialog.FileName))
                {
                    if(MessageBox.Show("Datos exportados a PDF en " + saveFileDialog.FileName + "<\n\n ¿Deseas abrirlo)",
                        "Exportación correcta", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        var p = new Process();
                        p.StartInfo = new ProcessStartInfo(saveFileDialog.FileName)
                        {
                            UseShellExecute = true
                        };
                        p.Start();
                    }
                }
            }
        }

        private void cargarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivo JSON|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                // lectura del archivo json
                provinciasJSON = new ProvinciasJSON(openFileDialog.FileName);
                provincias = provinciasJSON.GetProvincias();
                if(provincias != null)
                {
                    // carga de provincias
                    showProvincias();
                    activarBotones(true);
                    statusText.Text = openFileDialog.FileName;
                }
                else
                {
                    // borrado de provincias y error
                    provinciasStackPanel.Children.Clear();
                    activarBotones(false);
                    MessageBox.Show("Error cargando datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    statusText.Text = "Error cargando datos";
                }

            }
        }

        private void guardarButton_Click(object sender, RoutedEventArgs e)
        {
            this.guardar();
        }

        private void guardarComoButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo JSON|*json";
            if(saveFileDialog.ShowDialog() == true)
            {
                provinciasJSON.Path = saveFileDialog.FileName;
                if (provinciasJSON.saveProvincias(provincias))
                {
                    statusText.Text = saveFileDialog.FileName;
                    MessageBox.Show("Datos guardados correctamente en " + saveFileDialog.FileName, "Datos guardados", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Error guardando datos en " + saveFileDialog.FileName, "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
                }

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(provincias == null)
            {
                return;
            }

            switch(MessageBox.Show("¿Quieres guardar los datos antes de salir?", "Saliendo de la aplicación", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.Yes:
                    if (guardar())
                    {
                        e.Cancel = true;
                    } 
                    else
                    {
                        // Si los datos no se guardan correctamente no cerramos la app
                        e.Cancel = false;
                    }
                    break;
                    
                case MessageBoxResult.No:
                    e.Cancel = false;
                    break;
                    
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }
    }
}