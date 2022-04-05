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
        private OleDbDataReader reader;
        private List<CRecetaDispensable> listaRecestasDispensables = new List<CRecetaDispensable>();
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
            CRecetaDispensable recetaDispensable = new CRecetaDispensable();
            recetaDispensable.propId_Repositorio = (String)row.Row.ItemArray[0];
            recetaDispensable.propIdentificador_Receta = (String)row.Row.ItemArray[1];
            recetaDispensable.propFecha_Prescripcion = row.Row.ItemArray[2].ToString();
            recetaDispensable.propCodigo_Producto_Prescrito = (String)row.Row.ItemArray[3];
            recetaDispensable.propNombre_Producto_Prescrito = (String)row.Row.ItemArray[4];
            recetaDispensable.propEs_Marca_Comercial = (bool)row.Row.ItemArray[5];
            recetaDispensable.propNum_Envases = Int16.Parse(row.Row.ItemArray[6].ToString());
            recetaDispensable.propCodigo_Centro_Prescriptor = (String)row.Row.ItemArray[7];
            recetaDispensable.propTipo_Centro_Prescriptor = (String)row.Row.ItemArray[8];
            recetaDispensable.propEspecialidad_Medico = (String)row.Row.ItemArray[9];
            recetaDispensable.propNombre_Medico = (String)row.Row.ItemArray[10];
           
            if (listaRecestasDispensables.Count != 0)
            {
                //foreach (CRecetaDispensable receta in listaRecestasDispensables)
                //{
                //    if (receta.propId_Repositorio.CompareTo(recetaDispensable.propId_Repositorio)!=0)
                //    {
                //        //listaRecestasDispensables.Add(recetaDispensable);
                //    }
                //}
                for (int i=0; i<listaRecestasDispensables.Count;i++)
                {
                    if (listaRecestasDispensables[i].propId_Repositorio.CompareTo(recetaDispensable.propId_Repositorio) != 0)
                    {
                        listaRecestasDispensables.Add(recetaDispensable);
                    }
                }
            }
            else
            {
                listaRecestasDispensables.Add(recetaDispensable);
            }

            MessageBox.Show(" Nº"+ listaRecestasDispensables.Count);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
