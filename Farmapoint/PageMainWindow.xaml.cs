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
using System.Data.OleDb;
using System.Windows.Threading;

namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para PageMainWindow.xaml
    /// </summary>
    public partial class PageMainWindow : Page
    {
        public PageMainWindow()
        {
            InitializeComponent();
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            OleDbConnection conexion = ConexionDb.AbrirConexion();
            if (string.IsNullOrEmpty(Txt_numFarmacia.Text) && string.IsNullOrEmpty(Txt_User.Text))
            {
                Txt_numFarmacia.BorderBrush = System.Windows.Media.Brushes.Red;
                Txt_User.BorderBrush = System.Windows.Media.Brushes.Red;
                login_error.Text = "¡Rellene todos los campos!";
            }
            else if (string.IsNullOrEmpty(Txt_numFarmacia.Text))
            {
                Txt_User.BorderBrush = System.Windows.Media.Brushes.Black;
                Txt_numFarmacia.BorderBrush = System.Windows.Media.Brushes.Red;
                login_error.Text = "¡Rellene el campo Nº Farmacia!";
            }
            else if (string.IsNullOrEmpty(Txt_User.Text))
            {
                Txt_numFarmacia.BorderBrush = System.Windows.Media.Brushes.Black;
                Txt_User.BorderBrush = System.Windows.Media.Brushes.Red;
                login_error.Text = "¡Rellene el campo Usuario!";
            }
            else
            {
                try
                {
                    OleDbCommand adapter = new OleDbCommand("SELECT * FROM CSOAPHeadUsr WHERE Usuario = '" + Txt_User.Text + "'" + "AND Farmacia= " + int.Parse(Txt_numFarmacia.Text) + ";", conexion);
                    OleDbDataReader dr = adapter.ExecuteReader();
                    if (dr.Read())
                    {
                        conexion.Close();
                        PageBusquedaPaciente pageBusquedaPaciente = new PageBusquedaPaciente();
                        this.NavigationService.Navigate(pageBusquedaPaciente);
                    }
                    else
                    {
                        Txt_numFarmacia.BorderBrush = System.Windows.Media.Brushes.Red;
                        Txt_User.BorderBrush = System.Windows.Media.Brushes.Red;
                        login_error.Text = "¡Nº Farmacia o Usuario incorrectos!";
                    }
                }
                catch (FormatException)
                {
                    login_error.Text = "¡No se permiten letras en el campo \"Nº Farmacia\"!";
                }
            }
        }

        private void reloj()
        {
            DispatcherTimer dis = new DispatcherTimer();
            dis.Interval = new TimeSpan(0, 0, 0, 0, 0);
            dis.Tick += (a, b) =>
            {
                label_fecha.Text = DateTime.Now.ToString();
            };
            dis.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            reloj();
        }
    }
}

