using System.Windows;

namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window1 ventanaLogeado = new Window1();
            ventanaLogeado.Show();

            //OleDbConnection conexion = ConexionDb.AbrirConexion();
            //if (string.IsNullOrEmpty(Txt_numFarmacia.Text) && string.IsNullOrEmpty(Txt_User.Text))
            //{
            //    Txt_numFarmacia.BorderBrush = System.Windows.Media.Brushes.Red;
            //    Txt_User.BorderBrush = System.Windows.Media.Brushes.Red;
            //    login_error.Text = "¡Rellene todos los campos!";
            //}
            //else if (string.IsNullOrEmpty(Txt_numFarmacia.Text))
            //{
            //    Txt_User.BorderBrush = System.Windows.Media.Brushes.Black;
            //    Txt_numFarmacia.BorderBrush = System.Windows.Media.Brushes.Red;
            //    login_error.Text = "¡Rellene el campo Nº Farmacia!";
            //}
            //else if (string.IsNullOrEmpty(Txt_User.Text))
            //{
            //    Txt_numFarmacia.BorderBrush = System.Windows.Media.Brushes.Black;
            //    Txt_User.BorderBrush = System.Windows.Media.Brushes.Red;
            //    login_error.Text = "¡Rellene el campo Usuario!";
            //}
            //else
            //{
            //    OleDbCommand adapter = new OleDbCommand("SELECT * FROM CSOAPHeadUsr WHERE Usuario = '" + Txt_User.Text + "'" + "AND Farmacia= '" + Txt_numFarmacia.Text + "'", conexion);
            //    OleDbDataReader dr = adapter.ExecuteReader();
            //    DataSet d = new DataSet();
            //    if (dr.Read())
            //    {
            //        conexion.Close();
            //        this.Hide();

            //        Window1 ventanaLogeado = new Window1();
            //        ventanaLogeado.Show();
            //    }
            //    else
            //    {
            //        Txt_numFarmacia.BorderBrush = System.Windows.Media.Brushes.Red;
            //        Txt_User.BorderBrush = System.Windows.Media.Brushes.Red;
            //        login_error.Text = "¡Nº Farmacia o Usuario incorrectos!";
            //    }
            //}

        }
    }
}
