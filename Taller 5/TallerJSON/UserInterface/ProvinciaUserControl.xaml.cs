using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using DataAccess;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for PronvinciaUserControl.xaml
    /// </summary>
    public partial class ProvinciaUserControl : UserControl
    {
        private Provincia provincia;
        public Provincia Provincia 
        {
            set
            {
                this.provincia = value;
                this.idTextBox.Text = value.ID.ToString();
                this.nombreTextBox.Text = value.Nombre;
                this.poblacionTextBox.Text = value.Poblacion.ToString();
                this.superficieTextBox.Text = value.Superficie.ToString();
                try
                {
                    string path = Path.Combine(Environment.CurrentDirectory, "escudos", value.Escudo);
                    
                    // Si el archivo no existe o el path falla pongo el escudo de alicante
                    if (!File.Exists(path))
                    {
                        path = Path.Combine(Environment.CurrentDirectory, "escudos", "alacant.png");
                        MessageBox.Show($"El escudo de {value.Escudo} no existe", $"Error con escudo {value.Escudo}", MessageBoxButton.OK, MessageBoxImage.Warning); ;
                    }

                    this.escudoImage.Source = new BitmapImage(new Uri(path));
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Algún error ha ocurrido durante la carga de una provincia", "Error de carga", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            get
            {
                return this.provincia;
            }
        }

        public ProvinciaUserControl()
        {
            InitializeComponent();
        }

        private void idTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                provincia.ID = Convert.ToInt32(idTextBox.Text);
            }
            catch (FormatException)
            {
                provincia.ID = 0;
                idTextBox.Text= "0";
            }
        }

        private void nombreTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            provincia.Nombre = nombreTextBox.Text;
        }

        private void populationTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            provincia.Poblacion = Convert.ToInt32(poblacionTextBox.Text);
        }

        private void sizeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            provincia.Superficie = (float)Convert.ToDouble(superficieTextBox.Text);
        }
    }
}
