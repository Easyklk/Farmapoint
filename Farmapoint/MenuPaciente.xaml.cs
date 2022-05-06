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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Farmapoint
{
    /// <summary>
    /// Lógica de interacción para MenuPaciente.xaml
    /// </summary>
    public partial class MenuPaciente : Window
    {
        private string codigoSns;

        public MenuPaciente(string codigoSns)
        {
            this.codigoSns = codigoSns;
            InitializeComponent();
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
        }

        private void informacion_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            VentanaInformacionPaciente ventanaInformacionPaciente = new VentanaInformacionPaciente(codigoSns);
            ventanaInformacionPaciente.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void recetas_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            VentanaMedicamentosDispensables ventanaMedicamentoDispensable = new VentanaMedicamentosDispensables(codigoSns);
            ventanaMedicamentoDispensable.Show();
        }
    }
}
