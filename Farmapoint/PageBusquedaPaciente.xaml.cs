using System;
using System.Collections.Generic;
using System.Data.OleDb;
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
using System.Collections;
using System.IO;
using System.Windows.Threading;

namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para PageBusquedaPaciente.xaml
    /// </summary>
    public partial class PageBusquedaPaciente : Page
    {
        public PageBusquedaPaciente()
        {
            InitializeComponent();
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
            reloj();
        }

        private void Button_TarjetaSanitaria(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader streamReader = new StreamReader("c:\\Datos2.txt");
                ArrayList arrayList = new ArrayList();
                string str;
                do
                {
                    str = streamReader.ReadLine();
                    if (str != null)
                        arrayList.Add((object)str);
                }
                while (str != null);
                streamReader.Close();
                try
                {
                    File.Delete("c:\\Datos2.txt");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MessageBox.Show((string)arrayList[0]);
                //this.LblNombre.Text = Conversions.ToString(arrayList[0]);
                //this.LblTipoUsr.Text = Conversions.ToString(arrayList[1]);
                //this.LblFechaCad.Text = Conversions.ToString(arrayList[2]);
                //this.LblCpAuton.Text = Conversions.ToString(arrayList[3]);
                //this.LblFechaNac.Text = Conversions.ToString(arrayList[4]);
                //this.LblSexo.Text = Conversions.ToString(arrayList[5]);
                //this.LblNivelDependencia.Text = Conversions.ToString(arrayList[6]);
                //this.LblGradoDependencia.Text = Conversions.ToString(arrayList[7]);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.ToString(), "Error");
                MessageBox.Show(ex.ToString());
            }


            //this.Hide();
            //Window2 ventanaTarjetaSanitaria = new Window2();
            //ventanaTarjetaSanitaria.Show();
        }

        private void Button_CodigoSns(object sender, RoutedEventArgs e)
        {
            OleDbConnection conexion = ConexionDb.AbrirConexion();
            OleDbCommand adapter = new OleDbCommand("SELECT * FROM CPaciente WHERE Codigo_SNS = '" + Txt_SNS.Text + "'", conexion);
            OleDbDataReader dr = adapter.ExecuteReader();

            if (string.IsNullOrEmpty(Txt_SNS.Text))
            {
                Txt_SNS.BorderBrush = System.Windows.Media.Brushes.Red;
                sns_error.Text = "¡Este campo no puede estar vacio!";
            }
            else
            {
                if (dr.Read())
                {
                    conexion.Close();
                    PageMenuPaciente pageMenuPaciente = new PageMenuPaciente(Txt_SNS.Text);
                    this.NavigationService.Navigate(pageMenuPaciente);
                }
                else
                {
                    Txt_SNS.BorderBrush = System.Windows.Media.Brushes.Red;
                    sns_error.Text = "¡El codigo SNS no existe en la base de datos!";
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();

        }
    }
}
