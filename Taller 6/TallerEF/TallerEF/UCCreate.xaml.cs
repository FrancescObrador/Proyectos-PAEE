using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Shapes;
using TallerEF.Modelo;

namespace TallerEF
{
    /// <summary>
    /// Interaction logic for UCCreate.xaml
    /// </summary>
    public partial class UCCreate : UserControl
    {
        private TallerEFContext _context = new TallerEFContext();
        private CollectionViewSource clienteViewSource;
        public UCCreate()
        {
            InitializeComponent();
            clienteViewSource = (CollectionViewSource)FindResource(nameof(clienteViewSource));
        }

        private void GuardarCliente_Click(object sender, RoutedEventArgs e)
        {
            try {
                Cliente nuevoCliente = new Cliente();
                nuevoCliente.Nombre = ClienteNombreTextBox.Text;
                nuevoCliente.Identificacion = ClienteIdentificacionTextBox.Text;
                // Se añade y guarda el nuevo cliente a la tabla
                _context.Add(nuevoCliente);
                _context.SaveChanges();

                MessageBox.Show("Cliente insertado correctament", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                ClienteNombreTextBox.Text = "";
                ClienteIdentificacionTextBox.Text = "";
            } catch (Exception ex)
            {
                MessageBox.Show("Error al insertar datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GuardarCuentaCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CuentaCliente nuevaCuentaCliente = new CuentaCliente();
                nuevaCuentaCliente.Nombre = CuentaClienteNombreTextBox.Text;
                nuevaCuentaCliente.Descripcion = CuentaClienteDescripcionTextBox.Text;
                nuevaCuentaCliente.Saldo = Decimal.Parse(CuentaClienteSaldoTextBox.Text);
                nuevaCuentaCliente.Cliente = (Cliente)CuentaClienteComboBox.SelectedItem;
                _context.Add(nuevaCuentaCliente);
                _context.SaveChanges();

                MessageBox.Show("Cuenta del cliente insertada correctamente", "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);
                CuentaClienteNombreTextBox.Text = "";
                CuentaClienteDescripcionTextBox.Text = "";
                CuentaClienteSaldoTextBox.Text = "";
            } catch (Exception ex)
            {
                MessageBox.Show("Error al insertar datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new TallerEFContext();
            _context.Cliente.Load();
            clienteViewSource.Source = _context.Cliente.Local.ToObservableCollection();
        }
    }
}
