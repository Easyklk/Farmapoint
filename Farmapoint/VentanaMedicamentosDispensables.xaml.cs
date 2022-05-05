using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para Window2.xaml
    /// </summary>
    public partial class VentanaMedicamentosDispensables : Window
    {
        private OleDbConnection conexion = ConexionDb.AbrirConexion();
        private OleDbDataAdapter adapter = new OleDbDataAdapter();
        private OleDbCommand command = new OleDbCommand();
        private DataSet d = new DataSet();
        private string codigoSns;
        private CRecetaDispensable recetaDispensable;

        public VentanaMedicamentosDispensables(string codigoSns)
        {
            this.codigoSns = codigoSns;
            InitializeComponent();
        }

        private void rellenarDatos(OleDbConnection conexion, OleDbDataAdapter adapter, OleDbCommand command, DataSet d)
        {
            comprobarFecha();
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

        private void grdDatos_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            }
        }

        private void Button_Volver(object sender, RoutedEventArgs e)
        {
            VentanaBusquedaPaciente ventanaLogeado = new VentanaBusquedaPaciente();
            this.Hide();
            ventanaLogeado.Show();
        }

        private void grdDatos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DataRowView row = grdDatos.SelectedItem as DataRowView;
            if (row != null)
            {
                recetaDispensable = new CRecetaDispensable
                {
                    propId_Repositorio = Convert.ToString(row.Row.ItemArray[0] is DBNull ? 0 : row.Row.ItemArray[0]),
                    propIdentificador_Receta = Convert.ToString(row.Row.ItemArray[1] is DBNull ? 0 : row.Row.ItemArray[1]),
                    propFecha_Prescripcion = row.Row.ItemArray[2].ToString(),
                    propCodigo_Producto_Prescrito = Convert.ToString(row.Row.ItemArray[3] is DBNull ? 0 : row.Row.ItemArray[3]),
                    propNombre_Producto_Prescrito = Convert.ToString(row.Row.ItemArray[4] is DBNull ? 0 : row.Row.ItemArray[4]),
                    propEs_Marca_Comercial = (bool)row.Row.ItemArray[5],
                    propNum_Envases = short.Parse(row.Row.ItemArray[6].ToString()),
                    propCodigo_Centro_Prescriptor = Convert.ToString(row.Row.ItemArray[7] is DBNull ? 0 : row.Row.ItemArray[7]),
                    propTipo_Centro_Prescriptor = Convert.ToString(row.Row.ItemArray[8] is DBNull ? 0 : row.Row.ItemArray[8]),
                    propEspecialidad_Medico = Convert.ToString(row.Row.ItemArray[9] is DBNull ? 0 : row.Row.ItemArray[9]),
                    propNombre_Medico = Convert.ToString(row.Row.ItemArray[10] is DBNull ? 0 : row.Row.ItemArray[10]),
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
            OleDbConnection con = ConexionDb.AbrirConexion();
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

        private CPaciente Obtener_Paciente()
        {
            CPaciente cPaciente = new CPaciente();
            OleDbConnection con = ConexionDb.AbrirConexion();
            string str = "SELECT * " +
                    "FROM CPaciente WHERE Codigo_SNS = '" + codigoSns + "'";
            command = new OleDbCommand(str, con);
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
            dis.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dis.Tick += (a, b) =>
            {
                label_fecha.Text = DateTime.Now.ToString();
            };
            dis.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            reloj();
            label_sns.Text = codigoSns;
            label_nombre.Text = Obtener_Paciente().propNombre;
            label_apellido.Text = Obtener_Paciente().propApellidos;
            label_saldo.Text = Obtener_Paciente().propSaldo.ToString() + "€";
        }

        private void comprobarFecha()
        {
            string id_paciente = Obtener_Paciente().propId_Paciente;
            string cite = Obtener_Paciente().propCite;
            string id_consulta = "";

            OleDbConnection con = ConexionDb.AbrirConexion();
            string consultaFechaPrescripcion = "SELECT CRecetaCDA.duracionTratamiento, CRecetaCDA.Identificador_Receta " +
                "FROM CPaciente INNER JOIN((CReceta INNER JOIN CDetalleRecetaIN ON CReceta.Identificador_Receta = CDetalleRecetaIN.Receta) " +
                "INNER JOIN CRecetaCDA ON CReceta.Identificador_Receta = CRecetaCDA.Identificador_Receta) ON CPaciente.ID_Paciente = CDetalleRecetaIN.ID_Paciente " +
                " WHERE(((CPaciente.Codigo_SNS) = '" + codigoSns + "'));";
            OleDbCommand commandFechaPrescripcion = new OleDbCommand(consultaFechaPrescripcion, con);
            OleDbDataReader readerConsulta = commandFechaPrescripcion.ExecuteReader();
            if (readerConsulta.HasRows)
            {
                while (readerConsulta.Read())
                {
                    string duracionTratamiento = readerConsulta["duracionTratamiento"].ToString();

                    if (duracionTratamiento.CompareTo(DateTime.Today.ToString("d")) == -1)
                    {
                        string identificadorReceta = readerConsulta["Identificador_Receta"].ToString();
                        string consultaBusquedaPacienteOUT = "SELECT CBusquedaPacienteOUT.ID_Consulta " +
                       "FROM CPaciente INNER JOIN CBusquedaPacienteOUT ON CPaciente.ID_Paciente = CBusquedaPacienteOUT.Paciente " +
                       "WHERE (((CPaciente.ID_Paciente) = '" + id_paciente + "')); ";
                        OleDbCommand commandBusquedaPaciente = new OleDbCommand(consultaBusquedaPacienteOUT, con);
                        OleDbDataReader readerConsultaBusqeudaPaciente = commandBusquedaPaciente.ExecuteReader();
                        if (readerConsultaBusqeudaPaciente.Read())
                        {
                            id_consulta = readerConsultaBusqeudaPaciente["ID_Consulta"].ToString();
                        }
                        command.Connection = con;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "INSERT INTO CNotificarDispensacionIN (ID_Paciente, CITE, ID_Consulta, Codigo_SNS, RecetaDispensada, Localizador_Hoja)" +
                                              "VALUES ('" + id_paciente + "','" + cite + "'," + id_consulta + ", '" + codigoSns + "','" + identificadorReceta + "'," + 0 + ")";
                        command.ExecuteNonQuery();

                        command.CommandType = CommandType.Text;
                        command.CommandText = "UPDATE CRecetaDispensable SET Dispensada=TRUE WHERE Identificador_Receta='" + identificadorReceta + "';";
                        command.ExecuteNonQuery();
                    }
                }
            }
            con.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
