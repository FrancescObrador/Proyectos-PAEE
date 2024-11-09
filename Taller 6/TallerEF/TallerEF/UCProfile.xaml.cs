using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TallerEF.Modelo;

namespace TallerEF
{
    public partial class UCProfile : UserControl
    {
        private TallerEFContext _context = new TallerEFContext();
        private Cliente cliente;

        public UCProfile()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (int.TryParse(ClientIdTextBox.Text, out int clienteId))
            {
                
                cliente = _context.Cliente
                    .Where(c => c.Id == clienteId)
                    .FirstOrDefault();

                if (cliente != null)
                {
                    this.DataContext = cliente;
                    ProfileImage.Source = BitmapImageConverter.Convert(cliente.Fotografia);
                }
                else
                {
                    
                    MessageBox.Show("Cliente no encontrado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                
                MessageBox.Show("ID inválido", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
               
                ProfileImage.Source = new BitmapImage(new Uri(filePath));

                cliente.Fotografia = BitmapImageConverter.Convert(filePath);

                _context.Cliente.Update(cliente);
                _context.SaveChanges();
            }
        }
    }
}
