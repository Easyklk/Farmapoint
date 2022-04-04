using System.Data.OleDb;
using System.Windows;

namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            //this.Closed += (sender, e) => this.Dispatcher.InvokeShutdown();
        }

        private void Button_TarjetaSanitaria(object sender, RoutedEventArgs e)
        {
            this.Hide();
            //Window2 ventanaTarjetaSanitaria = new Window2();
            //ventanaTarjetaSanitaria.Show();
        }

        private void Button_CodigoSns(object sender, RoutedEventArgs e)
        {
            OleDbConnection conexion = ConexionDb.AbrirConexion();
            OleDbCommand adapter = new OleDbCommand("SELECT * FROM CPaciente WHERE Codigo_SNS = '" + Txt_SNS.Text + "'", conexion);
            OleDbDataReader dr = adapter.ExecuteReader();

            if (string.IsNullOrEmpty(Txt_SNS.Text))
            {
                Txt_SNS.BorderBrush = System.Windows.Media.Brushes.Red;
                sns_error.Text = "¡Este campo no puede estar vacio!";
            }
            else
            {
                if (dr.Read())
                {
                    conexion.Close();
                    this.Close();
                    Window2 panelRecetas = new Window2(Txt_SNS.Text);
                    panelRecetas.Show();
                }
                else
                {
                    Txt_SNS.BorderBrush = System.Windows.Media.Brushes.Red;
                    sns_error.Text = "¡El codigo SNS no existe en la base de datos!";
                }
            }
        }
    }
}
