using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TallerEF.Modelo;

namespace TallerEF
{
    /// <summary>
    /// Interaction logic for UCTodoEnUno.xaml
    /// </summary>
    public partial class UCTodoEnUno : UserControl
    {
        private TallerEFContext _context = new TallerEFContext();
        private CollectionViewSource clienteViewSource;

        public UCTodoEnUno()
        {
            InitializeComponent();
            clienteViewSource = (CollectionViewSource)FindResource(nameof(clienteViewSource));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Cliente.Load();
            clienteViewSource.Source = _context.Cliente.Local.ToObservableCollection();
        }


        private void btnCrearCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente nuevoCliente = new Cliente
                {
                    Nombre = ClienteNombreTextBox.Text,
                    Identificacion = ClienteIdentificacionTextBox.Text
                };

                // Validación de campos
                if (string.IsNullOrWhiteSpace(nuevoCliente.Nombre) || string.IsNullOrWhiteSpace(nuevoCliente.Identificacion))
                {
                    MessageBox.Show("Por favor, rellena todos los campos.");
                    return;
                }

                _context.Cliente.Add(nuevoCliente);
                _context.SaveChanges(); 

                MessageBox.Show("Cliente creado con éxito", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                clienteViewSource.Source = _context.Cliente.Local.ToObservableCollection(); 
            }
            catch (Exception)
            {
                MessageBox.Show("Error al crear el cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCrearCuentaCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente clienteSeleccionado = (Cliente)CuentaClienteComboBox.SelectedItem;

                CuentaCliente nuevaCuenta = new CuentaCliente
                {
                    Nombre = CuentaClienteNombreTextBox.Text,
                    Descripcion = CuentaClienteDescripcionTextBox.Text,
                    Saldo = decimal.Parse(CuentaClienteSaldoTextBox.Text),
                    Cliente = clienteSeleccionado
                };

                // Validacion de campos
                if (string.IsNullOrWhiteSpace(nuevaCuenta.Nombre) || string.IsNullOrWhiteSpace(nuevaCuenta.Descripcion) || nuevaCuenta.Saldo <= 0)
                {
                    MessageBox.Show("Por favor, rellene todos los campos correctamente.");
                    return;
                }

                _context.CuentaCliente.Add(nuevaCuenta);
                _context.SaveChanges(); 

                MessageBox.Show("Cuenta creada con éxito", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al crear la cuenta del cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnActualizarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente clienteSeleccionado = (Cliente)ClienteComboBox.SelectedItem;

                clienteSeleccionado.Nombre = ClienteNombreTextBox.Text;
                clienteSeleccionado.Identificacion = ClienteIdentificacionTextBox.Text;

                // Validacion de campos
                if (string.IsNullOrWhiteSpace(clienteSeleccionado.Nombre) || string.IsNullOrWhiteSpace(clienteSeleccionado.Identificacion))
                {
                    MessageBox.Show("Por favor, rellene todos los campos.");
                    return;
                }

                _context.Update(clienteSeleccionado);
                _context.SaveChanges(); 

                MessageBox.Show("Cliente actualizado correctamente", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                clienteViewSource.Source = _context.Cliente.Local.ToObservableCollection(); 
            }
            catch (Exception)
            {
                MessageBox.Show("Error al actualizar el cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEliminarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente clienteSeleccionado = (Cliente)ClienteComboBox.SelectedItem;

                MessageBoxResult result = MessageBox.Show($"¿Está seguro de que desea eliminar a {clienteSeleccionado.Nombre}?", "Confirmación", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _context.Cliente.Remove(clienteSeleccionado);
                    _context.SaveChanges();

                    MessageBox.Show("Cliente eliminado correctamente", "Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                    clienteViewSource.Source = _context.Cliente.Local.ToObservableCollection(); 
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al eliminar el cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
