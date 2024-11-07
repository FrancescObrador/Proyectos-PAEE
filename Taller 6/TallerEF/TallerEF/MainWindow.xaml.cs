using Microsoft.EntityFrameworkCore;
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

namespace TallerEF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly TallerEFContext _context = new TallerEFContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Se crea la DB. Elimina esto en prod
            _context.Database.EnsureCreated();

            //Se cargan los clientes
            _context.Cliente.Load();

            // Se carga el numero de clientes en la etiqueta
            lblNumeroClientes.Content = _context.Cliente.Count();
        }
    }
}