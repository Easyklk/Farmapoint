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
    /// Lógica de interacción para PageInformacionPaciente.xaml
    /// </summary>
    public partial class PageInformacionPaciente : Page
    {
        private string codigoSns;
        public PageInformacionPaciente(string codigoSns)
        {
            this.codigoSns = codigoSns;
            InitializeComponent();
        }

        private CPaciente Obtener_Paciente()
        {
            CPaciente cPaciente = new CPaciente();
            OleDbConnection con = ConexionDb.AbrirConexion();
            string str = "SELECT * " +
                    "FROM CPaciente WHERE Codigo_SNS = '" + codigoSns + "'";
            OleDbCommand command = new OleDbCommand(str, con);
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                cPaciente.propId_Paciente = reader["ID_Paciente"].ToString();
                cPaciente.propCite = reader["CITE"].ToString();
                cPaciente.propCodigo_sns = reader["Codigo_SNS"].ToString();
                cPaciente.propTsi = reader["TSI"].ToString();
                cPaciente.propNombre = reader["Nombre"].ToString();
                cPaciente.propApellidos = reader["Apellidos"].ToString();
                cPaciente.propFecha_Nacimiento = reader["Fecha_Nacimiento"].ToString();
                cPaciente.propId_Mutua = reader["ID_Mutua"].ToString();
                cPaciente.propTipo_aportacion = reader["Tipo_Aportacion"].ToString();
                cPaciente.propSaldo = Decimal.Parse(reader["Saldo"].ToString());
            }
            con.Close();
            return cPaciente;
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
            label_nombrePaciente.Text = Obtener_Paciente().propNombre;
            label_apellidosPaciente.Text = Obtener_Paciente().propApellidos;
            label_snsPaciente.Text = Obtener_Paciente().propCodigo_sns;
            label_saldoPaciente.Text = Obtener_Paciente().propSaldo.ToString() + "€";
        }

        private void btn_volver_Click(object sender, RoutedEventArgs e)
        {
            PageMenuPaciente pageMenuPaciente = new PageMenuPaciente(codigoSns);
            this.NavigationService.Navigate(pageMenuPaciente);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
