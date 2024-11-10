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
using TallerEF.Modelo;


namespace TallerEF
{
    /// <summary>
    /// Interaction logic for UCBuscarId.xaml
    /// </summary>
    public partial class UCBuscarId : UserControl
    {
        private TallerEFContext _context = new TallerEFContext();
        private Cliente cliente = new Cliente();


        public UCBuscarId()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _context = new TallerEFContext();
            _context.Cliente.Load();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(searchBar.Text)) return;
            if (!int.TryParse(searchBar.Text, out _))
            {
                searchBar.Text = string.Empty;
                return;
            }

            clienteGrid.DataContext = _context.Cliente.Where(cliente => cliente.Id == Convert.ToInt32(searchBar.Text)).SingleOrDefault();

        }
    }
}
