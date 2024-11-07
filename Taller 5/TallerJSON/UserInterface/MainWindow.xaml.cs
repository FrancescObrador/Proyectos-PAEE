using DataAccess;
using ExportPDF;
using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
        private List<Provincia> listToShow = new List<Provincia>();

        public event PropertyChangedEventHandler? PropertyChanged;

        private int currentPage = 1;
        private int pageSize = 5;
        private int totalPages = 0;

        public MainWindow()
        {
            InitializeComponent();
            btnPrevious.IsEnabled = false;
        }

        private void orderProvincias(bool porNombre)
        {
            if (porNombre)
            {
                listToShow = provincias.OrderBy(o => o.Nombre).ToList();
            }
            else
            {
                listToShow = provincias.OrderBy(o => o.ID).ToList();
            }
        }

        private void showProvincias()
        {
            provinciasStackPanel.Children.Clear();

            var provinciasToShow = listToShow.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            foreach (Provincia provincia in provinciasToShow)
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
                if (b.Name != "cargarButton")
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
                    if(MessageBox.Show("Datos exportados a PDF en " + saveFileDialog.FileName + "\n\n ¿Deseas abrirlo?",
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
                    listToShow = provincias;
                    totalPages = (int)Math.Ceiling((decimal)(provincias.Count/(float)pageSize));
                    lblPage.Content = $"1/{totalPages}";
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

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (provincias == null)
            {
                return;
            }

            // Si no hay nada en el buscador se muestran todas la provincias
            if (String.IsNullOrWhiteSpace(searchBox.Text))
            {
                listToShow = provincias;
                currentPage = 1;
                totalPages = (int)Math.Ceiling((decimal)(provincias.Count / (float)pageSize));
                ChangePage();
                return;
            }

            listToShow = provincias.Where(provincia =>
            {
                /*
                 * filtrado por contenido del termino en el nombre de provincia
                 * para que el usuario pueda buscar Alicante por "Ali" o "ali"
                 * o para que pueda buscar Ciudad Real por "Real" o "real"
                 */
                //return provincia.Nombre.ToLower().Contains(searchBox.Text.ToLower());

                // Filtrado solo si empieza por el termino introducido
                return provincia.Nombre.ToLower().StartsWith(searchBox.Text.ToLower());
            }).ToList();

            currentPage = 1;
            totalPages = (int)Math.Ceiling(listToShow.Count / (double)pageSize);
            ChangePage();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ChangePage();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                ChangePage();
            } 
        }

        private void ChangePage()
        {
            btnPrevious.IsEnabled = currentPage > 1;
            btnNext.IsEnabled = currentPage < totalPages;

            lblPage.Content = $"{currentPage}/{totalPages}";

            showProvincias();
        }

        /// Visto como hacerlo en: https://stackoverflow.com/questions/22598587/export-list-of-object-to-xml-file-in-c-sharp
        private void exportXML()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo XML|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                FileInfo file = new FileInfo(saveFileDialog.FileName);
                StreamWriter sw = file.AppendText();
                var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Provincia>));
                xmlSerializer.Serialize(sw, provincias);
                sw.Close();
            }
        }

        private void exportarXMLButton_Click(object sender, RoutedEventArgs e)
        {
            this.exportXML();
        }
    }
}