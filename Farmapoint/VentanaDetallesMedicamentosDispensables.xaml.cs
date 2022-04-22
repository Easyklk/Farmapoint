using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;

namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para VentanaDetallesMedicamentosDispensables.xaml
    /// </summary>
    public partial class VentanaDetallesMedicamentosDispensables : Window
    {
        private OleDbDataAdapter adapter = new OleDbDataAdapter();
        private OleDbCommand command = new OleDbCommand();
        private DataSet d = new DataSet();
        private string codigoSns;
        private CRecetaDispensable recetaDispensable;
        private CRecetaDispensada recetaDispensada;

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
            OleDbConnection conexion = ConexionDb.AbrirConexion();
            rellenarDatos(conexion, adapter, command, d);
            grdDatos.ItemsSource = d.Tables["CRecetaCDA"].DefaultView;
            grdDatos.Columns[0].Visibility = Visibility.Hidden;
            grdDatos.Columns[1].Visibility = Visibility.Hidden;
            grdDatos.Columns[2].Header = "Codigo Producto";
            grdDatos.Columns[3].Header = "Nombre Producto";
            grdDatos.Columns[4].Header = "Nº Envases";
            grdDatos.Columns[5].Header = "Precio Unitario";
            grdDatos.Columns[6].Header = "Aportación Unitaria";
            grdDatos.Columns[7].Header = "Tipo Contingencia";
            grdDatos.Columns[8].Header = "Tipo Aportación";
            grdDatos.Columns[9].Header = "Codigo Causa Sustitución";
            grdDatos.Columns[10].Header = "Descripción Causa Sustitución";
            grdDatos.Columns[11].Header = "Observaciones";
            conexion.Close();
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
                recetaDispensada = new CRecetaDispensada
                {
                    propId_Repositorio = (string)row.Row.ItemArray[0],
                    propIdentificador_Receta = (string)row.Row.ItemArray[1],
                    propCodigo_Producto_Dispensado = (string)row.Row.ItemArray[2],
                    propNombre_Producto_Dispensado = (string)row.Row.ItemArray[3],
                    propNum_Envases = int.Parse(row.Row.ItemArray[4].ToString()),
                    propPrecio_Unitario = Decimal.Parse(row.Row.ItemArray[5].ToString()),
                    propAportacion_Unitaria = Decimal.Parse(row.Row.ItemArray[6].ToString()),
                    propTipo_Contingencia = (string)row.Row.ItemArray[7],
                    propTipo_Aportacion = (string)row.Row.ItemArray[8],
                    propCodigo_Causa_Sustitucion = (string)row.Row.ItemArray[9],
                    propDescripcion_Causa_Sustitucion = Convert.ToString(row.Row.ItemArray[10] is DBNull ? 0 : row.Row.ItemArray[10]),
                    propObservaciones = Convert.ToString(row.Row.ItemArray[11] is DBNull ? 0 : row.Row.ItemArray[11])
                };
            }
            if (recetaDispensable != null)
            {
                btn_dispensar.IsEnabled = true;
            }
        }

        private void btn_dispensar_Click(object sender, RoutedEventArgs e)
        {
            string id_paciente = "";
            string cite = "";
            string id_consulta = "";
            OleDbConnection con = ConexionDb.AbrirConexion();
            string consultaCPaciente = "SELECT ID_Paciente, CITE " +
                    "FROM CPaciente WHERE Codigo_SNS = '" + codigoSns + "'";
            OleDbCommand commandCPaciente = new OleDbCommand(consultaCPaciente, con);
            OleDbDataReader readerPaciente = commandCPaciente.ExecuteReader();
            if (readerPaciente.Read())
            {
                id_paciente = readerPaciente["ID_Paciente"].ToString();
                cite = readerPaciente["CITE"].ToString();
            }
            string consultaBusquedaPacienteOUT = "SELECT CBusquedaPacienteOUT.ID_Consulta " +
           "FROM CPaciente INNER JOIN CBusquedaPacienteOUT ON CPaciente.ID_Paciente = CBusquedaPacienteOUT.Paciente " +
           "WHERE (((CPaciente.ID_Paciente) = '" + id_paciente + "')); ";
            OleDbCommand commandBusquedaPaciente = new OleDbCommand(consultaBusquedaPacienteOUT, con);
            OleDbDataReader readerConsulta = commandBusquedaPaciente.ExecuteReader();

            if (readerConsulta.Read())
            {
                id_consulta = readerConsulta["ID_Consulta"].ToString();
            }
            try
            {
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO CNotificarDispensacionIN (ID_Paciente, CITE, ID_Consulta, Codigo_SNS, RecetaDispensada, Localizador_Hoja)" +
                                      "VALUES ('" + id_paciente + "','" + cite + "'," + id_consulta + ", '" + codigoSns + "','" + recetaDispensada.propIdentificador_Receta + "'," + 0 + ")";
                command.ExecuteNonQuery();
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE CRecetaDispensable SET Dispensada=TRUE WHERE Identificador_Receta='" + recetaDispensada.propIdentificador_Receta + "';";
                command.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            DataRowView row = grdDatos.SelectedItem as DataRowView;
            row.Row.Delete();
            btn_dispensar.IsEnabled = false;

            this.Close();
            Window2 VentanaMedicamentoDispensable = new Window2(codigoSns);
            VentanaMedicamentoDispensable.Show();
        }

        private void grdDatos_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            }
        }
    }
}
