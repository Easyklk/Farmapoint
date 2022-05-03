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
            grdDatos.SelectAll();
            DataRowView drv = grdDatos.SelectedItem as DataRowView;
            DataRow dr = drv.Row;
            grdDatos.Columns[0].Visibility = Visibility.Hidden;
            grdDatos.Columns[1].Visibility = Visibility.Hidden;
            grdDatos.Columns[2].Header = "Fecha Prescripcion";
            grdDatos.Columns[3].Header = "Codigo Producto";
            grdDatos.Columns[4].Header = "Nombre Producto";
            grdDatos.Columns[5].Header = "Marca Comercial";
            grdDatos.Columns[6].Header = "Nº Envases";
            //dr.SetField(6, );
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
            Window1 ventanaLogeado = new Window1();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label_sns.Text = codigoSns;
            label_nombre.Text = Obtener_Paciente().propNombre;
            label_apellido.Text = Obtener_Paciente().propApellidos;
            label_saldo.Text = Obtener_Paciente().propSaldo.ToString() + "€";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
