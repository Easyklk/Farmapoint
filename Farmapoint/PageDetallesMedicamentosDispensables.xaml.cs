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
using System.Data;
using System.Data.OleDb;
using System.Windows.Threading;

namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para PageDetallesMedicamentosDispensables.xaml
    /// </summary>
    public partial class PageDetallesMedicamentosDispensables : Page
    {
        private OleDbDataAdapter adapter = new OleDbDataAdapter();
        private OleDbCommand command = new OleDbCommand();
        private DataSet d = new DataSet();
        private string codigoSns;
        private CRecetaDispensable recetaDispensable;
        private CRecetaDispensada recetaDispensada;
        private string tipoAportacion;
        private decimal importeTotal = 0m;

        public PageDetallesMedicamentosDispensables(CRecetaDispensable recetaDispensable, string codigoSns)
        {
            InitializeComponent();
            this.codigoSns = codigoSns;
            this.recetaDispensable = recetaDispensable;
            calcular_Importe();
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
            PageMedicamentosDispensables pageMedicamentosDispensables = new PageMedicamentosDispensables(codigoSns);
            this.NavigationService.Navigate(pageMedicamentosDispensables);
        }

        private void DataRow_Loaded(object sender, RoutedEventArgs e)
        {
            grdDatos.ItemsSource = null;
            OleDbConnection conexion = ConexionDb.AbrirConexion();
            rellenarDatos(conexion, adapter, command, d);
            grdDatos.ItemsSource = d.Tables["CRecetaCDA"].DefaultView;
            grdDatos.SelectAll();
            DataRowView drv = grdDatos.SelectedItem as DataRowView;
            DataRow dr = drv.Row;
            grdDatos.Columns[0].Visibility = Visibility.Hidden;
            grdDatos.Columns[1].Visibility = Visibility.Hidden;
            grdDatos.Columns[2].Visibility = Visibility.Hidden;
            grdDatos.Columns[3].Header = "Nombre Producto";
            dr.SetField(3, recetaDispensable.propNombre_Producto_Prescrito);
            grdDatos.Columns[4].Header = "Num_Envases";
            dr.SetField(4, recetaDispensable.propNum_Envases);
            grdDatos.Columns[5].Header = "Precio Unitario";
            grdDatos.Columns[6].Visibility = Visibility.Hidden;
            grdDatos.Columns[7].Visibility = Visibility.Hidden;
            grdDatos.Columns[8].Header = "Tipo Aportación";
            dr.SetField(8, tipoAportacion);
            grdDatos.Columns[9].Visibility = Visibility.Hidden;
            grdDatos.Columns[10].Visibility = Visibility.Hidden;
            grdDatos.Columns[11].Header = "Observaciones";
            conexion.Close();
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
            label_nombre.Text = Obtener_Paciente().propNombre;
            label_apellido.Text = Obtener_Paciente().propApellidos;
            reloj();
            label_saldo.Text = Obtener_Paciente().propSaldo.ToString() + "€";
            label_sns.Text = codigoSns;
        }

        private void grdDatos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DataRowView row = grdDatos.SelectedItem as DataRowView;
            if (row != null)
            {
                recetaDispensada = new CRecetaDispensada
                {
                    propId_Repositorio = Convert.ToString(row.Row.ItemArray[0] is DBNull ? 0 : row.Row.ItemArray[0]),
                    propIdentificador_Receta = Convert.ToString(row.Row.ItemArray[1] is DBNull ? 0 : row.Row.ItemArray[1]),
                    propCodigo_Producto_Dispensado = Convert.ToString(row.Row.ItemArray[2] is DBNull ? 0 : row.Row.ItemArray[2]),
                    propNombre_Producto_Dispensado = Convert.ToString(row.Row.ItemArray[3] is DBNull ? 0 : row.Row.ItemArray[3]),
                    propNum_Envases = int.Parse(row.Row.ItemArray[4].ToString()),
                    propPrecio_Unitario = Decimal.Parse(row.Row.ItemArray[5].ToString()),
                    propAportacion_Unitaria = Decimal.Parse(row.Row.ItemArray[6].ToString()),
                    propTipo_Contingencia = Convert.ToString(row.Row.ItemArray[7] is DBNull ? 0 : row.Row.ItemArray[7]),
                    propTipo_Aportacion = Convert.ToString(row.Row.ItemArray[8] is DBNull ? 0 : row.Row.ItemArray[8]),
                    propCodigo_Causa_Sustitucion = Convert.ToString(row.Row.ItemArray[9] is DBNull ? 0 : row.Row.ItemArray[9]),
                    propDescripcion_Causa_Sustitucion = Convert.ToString(row.Row.ItemArray[10] is DBNull ? 0 : row.Row.ItemArray[10]),
                    propObservaciones = Convert.ToString(row.Row.ItemArray[11] is DBNull ? 0 : row.Row.ItemArray[11])
                };
            }
            if (recetaDispensable != null)
            {
                if (Obtener_Paciente().propSaldo >= importeTotal)
                {
                    btn_dispensar.IsEnabled = true;
                }
                else
                {
                    label_error.Text = "¡SALDO INSUFICIENTE!";
                }
            }
        }

        private void btn_dispensar_Click(object sender, RoutedEventArgs e)
        {
            string id_paciente = Obtener_Paciente().propId_Paciente;
            string cite = Obtener_Paciente().propCite;
            string id_consulta = "";
            decimal saldo = Obtener_Paciente().propSaldo;

            OleDbConnection con = ConexionDb.AbrirConexion();
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

                command.ExecuteNonQuery();
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE CPaciente SET Saldo='" + (saldo - importeTotal) + "' WHERE Codigo_SNS='" + codigoSns + "';";
                command.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            PageMedicamentosDispensables pageMedicamentosDispensables = new PageMedicamentosDispensables(codigoSns);
            this.NavigationService.Navigate(pageMedicamentosDispensables);
        }

        private void grdDatos_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
            }
        }

        private void calcular_Importe()
        {
            int num_Envases = 0;
            decimal precio_Unitario = 0.0m;
            decimal descuento = 0m;
            OleDbConnection con = ConexionDb.AbrirConexion();
            string consultaCRecetaDispensada = "SELECT CRecetaDispensada.Num_Envases, CRecetaDispensada.Precio_Unitario FROM((CRecetaDispensable INNER JOIN CRecetaDispensada " +
                    "ON CRecetaDispensable.Identificador_Receta = CRecetaDispensada.Identificador_Receta) " +
                    "INNER JOIN CRecetaCDA ON CRecetaDispensable.Identificador_Receta = CRecetaCDA.Identificador_Receta) " +
                    "INNER JOIN(CPaciente INNER JOIN CBusquedaReferenciasIN ON CPaciente.ID_Paciente = CBusquedaReferenciasIN.ID_Paciente) " +
                    "ON CRecetaDispensable.Identificador_Receta = CBusquedaReferenciasIN.Receta_Dispensable " +
                    "WHERE(((CRecetaDispensada.Identificador_Receta) = '" + recetaDispensable.propIdentificador_Receta + "'));";
            OleDbCommand commandCRecetaDispensada = new OleDbCommand(consultaCRecetaDispensada, con);
            OleDbDataReader readerCRecetaDispensada = commandCRecetaDispensada.ExecuteReader();
            if (readerCRecetaDispensada.Read())
            {
                num_Envases = int.Parse(recetaDispensable.propNum_Envases.ToString());
                precio_Unitario = decimal.Parse(readerCRecetaDispensada["Precio_Unitario"].ToString());
            }
            con.Close();

            tipoAportacion = Obtener_Paciente().propTipo_aportacion;
            switch (tipoAportacion)
            {
                case "TSI001":
                    descuento = 0.00m;
                    break;
                case "TSI002_00":
                    descuento = 0.10m;
                    break;
                case "TSI002_01":
                    descuento = 0.10m;
                    break;
                case "TSI002_02":
                    descuento = 0.10m;
                    break;
                case "TSI003":
                    descuento = 0.40m;
                    break;
                case "TSI004":
                    descuento = 0.50m;
                    break;
                case "TSI005":
                    descuento = 0.60m;
                    break;
                case "TSI005_03":
                    descuento = 0.60m;
                    break;
                case "TSI006":
                    descuento = 0.30m;
                    break;
                case "F003":
                    descuento = 0.40m;
                    break;
                case "F004":
                    descuento = 0.50m;
                    break;
                case "NOFAR":
                    descuento = 1.00m;
                    break;
            }

            decimal precio_descontado = precio_Unitario * descuento;
            precio_descontado = precio_Unitario - precio_descontado;
            importeTotal = num_Envases * precio_descontado;
            importe.Text = Decimal.Round(importeTotal, 2).ToString() + "€";
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_salir_click(object sender, RoutedEventArgs e)
        {
            PageBusquedaPaciente pageBusquedaPaciente = new PageBusquedaPaciente();
            this.NavigationService.Navigate(pageBusquedaPaciente);
        }
    }
}
