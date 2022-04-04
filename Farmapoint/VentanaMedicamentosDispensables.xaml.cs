using System;
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
        DataSet d = new DataSet();
        private string codigoSns;

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
                string qry = "SELECT CPaciente.Nombre, CPaciente.Apellidos, CPaciente.Codigo_SNS AS SNS, CDetalleRecetaIN.Receta " +
                    "FROM CPaciente INNER JOIN(CDetalleRecetaIN INNER JOIN CBusquedaPacienteOUT " +
                    "ON (CDetalleRecetaIN.ID_Paciente = CBusquedaPacienteOUT.Paciente) " +
                    "AND(CDetalleRecetaIN.ID_Consulta = CBusquedaPacienteOUT.ID_Consulta)) " +
                    "ON(CDetalleRecetaIN.ID_Paciente = CPaciente.ID_Paciente) " +
                    "AND(CPaciente.ID_Paciente = CBusquedaPacienteOUT.Paciente) " +
                    "WHERE CPaciente.Codigo_SNS = '" + codigoSns + "'";
                command.CommandText = qry;
                command.Connection = conexion;
                adapter.SelectCommand = command;
                adapter.Fill(d, "CDetalleRecetaIN" + "CPaciente" + "CReceta");
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
            grdDatos.ItemsSource = d.Tables["CDetalleRecetaIN" + "CPaciente" + "CReceta"].DefaultView;
        }
        private void Button_Volver(object sender, RoutedEventArgs e)
        {
            Window1 ventanaLogeado = new Window1();
            this.Close();
            ventanaLogeado.Show();
        }

        private void grdDatos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
