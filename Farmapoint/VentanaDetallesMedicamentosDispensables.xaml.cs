using System.Windows;

namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para VentanaDetallesMedicamentosDispensables.xaml
    /// </summary>
    public partial class VentanaDetallesMedicamentosDispensables : Window
    {
        private string codigoSns;

        public VentanaDetallesMedicamentosDispensables(CRecetaDispensable recetaDispensable, string codigoSns)
        {
            InitializeComponent();
            txt.Text = recetaDispensable.ToString();
            this.codigoSns = codigoSns;
        }

        private void btn_volver_Click(object sender, RoutedEventArgs e)
        {
            Window2 ventanaLogeado = new Window2(codigoSns);
            this.Close();
            ventanaLogeado.Show();
        }

        private void DataRow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void label_nombre_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void label_apellido_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void label_codigosns_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void grdDatos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
