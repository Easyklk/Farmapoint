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
        private CRecetaDispensable recetaDispensable;

        public VentanaDetallesMedicamentosDispensables(CRecetaDispensable recetaDispensable, string codigoSns)
        {
            InitializeComponent();
            this.codigoSns = codigoSns;
            this.recetaDispensable = recetaDispensable;
        }

        private void rellenarDatos(OleDbConnection conexion, OleDbDataAdapter adapter, OleDbCommand command, DataSet d)
        {
            try
            {
                d.Clear();  
                string qry = "SELECT CRecetaDispensada.* FROM((CRecetaDispensable INNER JOIN CRecetaDispensada " +
                    "ON CRecetaDispensable.Identificador_Receta = CRecetaDispensada.Identificador_Receta) " +
                    "INNER JOIN CRecetaCDA ON CRecetaDispensable.Identificador_Receta = CRecetaCDA.Identificador_Receta) " +
                    "INNER JOIN(CPaciente INNER JOIN CBusquedaReferenciasIN ON CPaciente.ID_Paciente = CBusquedaReferenciasIN.ID_Paciente) " +
                    "ON CRecetaDispensable.Identificador_Receta = CBusquedaReferenciasIN.Receta_Dispensable " +
                    "WHERE(((CRecetaDispensada.Identificador_Receta) = '" + recetaDispensable.propIdentificador_Receta + "'));";
                command.CommandText = qry;
                command.Connection = conexion;
                adapter.SelectCommand = command;
                adapter.Fill(d, "CRecetaCDA");
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
            grdDatos.ItemsSource = null;
            rellenarDatos(conexion, adapter, command, d);
            grdDatos.ItemsSource = d.Tables["CRecetaCDA" ].DefaultView;
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
