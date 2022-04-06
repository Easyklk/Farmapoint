using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private OleDbConnection conexion = ConexionDb.AbrirConexion();
        private OleDbDataAdapter adapter = new OleDbDataAdapter();
        private OleDbCommand command = new OleDbCommand();
        private List<CRecetaDispensable> listaRecestasDispensables = new List<CRecetaDispensable>();
        private DataSet d = new DataSet();
        private string codigoSns;
        private CRecetaDispensable recetaDispensable;

        public Window2(string CodigoSns)
        {
            InitializeComponent();
            this.codigoSns = CodigoSns;
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
                MessageBox.Show(ex.ToString());
            }
        }

        private void DataRow_Loaded(object sender, RoutedEventArgs e)
        {
            grdDatos.ItemsSource = null;
            rellenarDatos(conexion, adapter, command, d);
            grdDatos.ItemsSource = d.Tables["CRecetaDispensable" + "CBusquedaReferenciasOUT" + "CPaciente"].DefaultView;
        }
        private void Button_Volver(object sender, RoutedEventArgs e)
        {
            Window1 ventanaLogeado = new Window1();
            this.Close();
            ventanaLogeado.Show();
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
        }

        private void label_codigosns_Loaded(object sender, RoutedEventArgs e)
        {
            label_sns.Text = codigoSns;
        }

        private void grdDatos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DataRowView row = grdDatos.SelectedItem as DataRowView;
            recetaDispensable = new CRecetaDispensable
            {
                propId_Repositorio = (string)row.Row.ItemArray[0],
                propIdentificador_Receta = (string)row.Row.ItemArray[1],
                propFecha_Prescripcion = row.Row.ItemArray[2].ToString(),
                propCodigo_Producto_Prescrito = (string)row.Row.ItemArray[3],
                propNombre_Producto_Prescrito = (string)row.Row.ItemArray[4],
                propEs_Marca_Comercial = (bool)row.Row.ItemArray[5],
                propNum_Envases = short.Parse(row.Row.ItemArray[6].ToString()),
                propCodigo_Centro_Prescriptor = (string)row.Row.ItemArray[7],
                propTipo_Centro_Prescriptor = (string)row.Row.ItemArray[8],
                propEspecialidad_Medico = (string)row.Row.ItemArray[9],
                propNombre_Medico = (string)row.Row.ItemArray[10]
            };
            if (recetaDispensable != null)
            {
                mostrarReceta.IsEnabled = true;
            }
        }

        private void mostrar_RecetaDispensable(object sender, RoutedEventArgs e)
        {
            VentanaDetallesMedicamentosDispensables ventanaDetallesMedicamentosDispensables = new VentanaDetallesMedicamentosDispensables(recetaDispensable);
            this.Hide();
            ventanaDetallesMedicamentosDispensables.Show();
        }
    }
}
