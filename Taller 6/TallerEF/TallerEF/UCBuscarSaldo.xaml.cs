using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TallerEF
{
    /// <summary>
    /// Interaction logic for UCBuscarSaldo.xaml
    /// </summary>
    public partial class UCBuscarSaldo : UserControl
    {
        private TallerEFContext _context = new TallerEFContext();
        private CollectionViewSource clienteViewSource;

        public UCBuscarSaldo()
        {
            InitializeComponent();
            clienteViewSource = (CollectionViewSource)FindResource(nameof(clienteViewSource));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new TallerEFContext();
            _context.CuentaCliente.Load();
            clienteViewSource.Source = _context.Cliente.Local.ToObservableCollection();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(searchBar.Text)) return;
            if (!decimal.TryParse(searchBar.Text, out _))
            {
                searchBar.Text = string.Empty;
                return;
            }

            // Devuelvo un cliente si cualquiera de sus cuentas empieza con el saldo que se busca
            clienteViewSource.Source = _context.Cliente.Where(cliente => cliente.Cuentas.Any(cuenta => cuenta.Saldo.ToString().StartsWith(searchBar.Text))).ToList();
        }
    }
}
