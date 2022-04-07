using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para VentanaDetallesMedicamentosDispensables.xaml
    /// </summary>
    public partial class VentanaDetallesMedicamentosDispensables : Window
    {
        private OleDbConnection conexion = ConexionDb.AbrirConexion();
        private OleDbDataAdapter adapter = new OleDbDataAdapter();
        private OleDbCommand command = new OleDbCommand();
        private DataSet d = new DataSet();
        private string codigoSns;

        public VentanaDetallesMedicamentosDispensables(CRecetaDispensable recetaDispensable, string codigoSns)
        {
            InitializeComponent();
            this.codigoSns = codigoSns;
        }

        private void rellenarDatos(OleDbConnection conexion, OleDbDataAdapter adapter, OleDbCommand command, DataSet d)
        {
            try
            {
                d.Clear();
                string qry = "SELECT CRecetaDispensable.* " +
                    "FROM(CRecetaDispensable LEFT JOIN CBusquedaReferenciasOUT " +
                    "ON CRecetaDispensable.Identificador_Receta = CBusquedaReferenciasOUT.RecetasDispensable) " +
                    "INNER JOIN(CPaciente INNER JOIN CBusquedaReferenciasIN ON CPaciente.ID_Paciente = CBusquedaReferenciasIN.ID_Paciente) " +
                    "ON CRecetaDispensable.Identificador_Receta = CBusquedaReferenciasIN.Receta_Dispensable " +
                    "WHERE CPaciente.Codigo_SNS = '" + codigoSns + "'";
                command.CommandText = qry;
                command.Connection = conexion;
                adapter.SelectCommand = command;
                adapter.Fill(d, "CRecetaDispensable" + "CBusquedaReferenciasOUT" + "CPaciente");
                conexion.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            OleDbConnection con = ConexionDb.AbrirConexion();
            string str = "SELECT Nombre " +
                    "FROM CPaciente WHERE Codigo_SNS = '" + codigoSns + "'";
            command = new OleDbCommand(str, con);
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                label_nombre.Text = reader["Nombre"].ToString();
            }
            con.Close();
        }

        private void label_apellido_Loaded(object sender, RoutedEventArgs e)
        {
            OleDbConnection con = ConexionDb.AbrirConexion();
            string str = "SELECT Apellidos " +
                    "FROM CPaciente WHERE Codigo_SNS = '" + codigoSns + "'";
            command = new OleDbCommand(str, con);
            OleDbDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                label_apellido.Text = reader["Apellidos"].ToString();
            }
            con.Close();
        }

        private void label_codigosns_Loaded(object sender, RoutedEventArgs e)
        {
            label_sns.Text = codigoSns;
        }

        private void grdDatos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
