using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;

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
        private DataSet d = new DataSet();
        private string codigoSns;
        private CRecetaDispensable recetaDispensable;

        public Window2(string codigoSns)
        {
            InitializeComponent();
            this.codigoSns = codigoSns;
        }

        private void rellenarDatos(OleDbConnection conexion, OleDbDataAdapter adapter, OleDbCommand command, DataSet d)
        {
            try
            {
                d.Clear();
                string qry = "SELECT CRecetaDispensable.*" +
                    "FROM(CRecetaDispensable LEFT JOIN CBusquedaReferenciasOUT ON CRecetaDispensable.Identificador_Receta = CBusquedaReferenciasOUT.RecetasDispensable) " +
                    "INNER JOIN(CPaciente INNER JOIN CBusquedaReferenciasIN ON CPaciente.ID_Paciente = CBusquedaReferenciasIN.ID_Paciente) " +
                    "ON CRecetaDispensable.Identificador_Receta = CBusquedaReferenciasIN.Receta_Dispensable " +
                    "WHERE CPaciente.Codigo_SNS = '" + codigoSns + "' AND CRecetaDispensable.Dispensada = FALSE";
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

        private void DataRow_Loaded(object sender, RoutedEventArgs e)
        {
            grdDatos.ItemsSource = null;
            rellenarDatos(conexion, adapter, command, d);
            grdDatos.ItemsSource = d.Tables["CRecetaDispensable" + "CBusquedaReferenciasOUT" + "CPaciente"].DefaultView;
            grdDatos.Columns[0].Visibility = Visibility.Hidden;
            grdDatos.Columns[1].Visibility = Visibility.Hidden;
            grdDatos.Columns[2].Header = "Fecha Prescripcion";
            grdDatos.Columns[3].Header = "Codigo Producto";
            grdDatos.Columns[4].Header = "Nombre Producto";
            grdDatos.Columns[5].Header = "Marca Comercial";
            grdDatos.Columns[6].Header = "Nº Envases";
            grdDatos.Columns[7].Header = "Codigo Centro";
            grdDatos.Columns[8].Header = "Tipo Centro";
            grdDatos.Columns[9].Header = "Especialidad Medico";
            grdDatos.Columns[10].Header = "Nombre Medico";
            grdDatos.Columns[11].Header = "Dispensada";
            conexion.Close();
        }
        private void Button_Volver(object sender, RoutedEventArgs e)
        {
            Window1 ventanaLogeado = new Window1();
            this.Hide();
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
            DataRowView row = grdDatos.SelectedItem as DataRowView;
            if (row != null)
            {
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
                    propNombre_Medico = (string)row.Row.ItemArray[10],
                    propDispensada = (bool)row.Row.ItemArray[11]
                };
            }
            if (recetaDispensable != null)
            {
                btn_mostrar.IsEnabled = true;
            }
        }

        private void mostrar_RecetaDispensable(object sender, RoutedEventArgs e)
        {
            string id_paciente = "";
            OleDbConnection con = ConexionDb.AbrirConexion();
            string consultaCPaciente = "SELECT ID_Paciente " +
                    "FROM CPaciente WHERE Codigo_SNS = '" + codigoSns + "'";
            OleDbCommand commandCPaciente = new OleDbCommand(consultaCPaciente, con);
            OleDbDataReader readerPaciente = commandCPaciente.ExecuteReader();
            if (readerPaciente.Read())
            {
                id_paciente = readerPaciente["ID_Paciente"].ToString();
            }
            string str = "SELECT CNotificarDispensacionIN.* FROM CRecetaDispensable INNER JOIN CNotificarDispensacionIN ON CRecetaDispensable.Identificador_Receta = CNotificarDispensacionIN.RecetaDispensada" +
                " WHERE'" + recetaDispensable.propIdentificador_Receta + "'= CNotificarDispensacionIN.RecetaDispensada; ";
            command = new OleDbCommand(str, con);
            OleDbDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                con.Close();
                VentanaDetallesMedicamentosDispensables ventanaDetallesMedicamentosDispensables = new VentanaDetallesMedicamentosDispensables(recetaDispensable, codigoSns);
                this.Hide();
                ventanaDetallesMedicamentosDispensables.Show();
            }

        }

        private void grdDatos_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();

        }
    }
}
