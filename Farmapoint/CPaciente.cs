namespace Farmapoint
{
    public class CPaciente
    {
        private string id_paciente;
        private string cite;
        private string codigo_sns;
        private string tsi;
        private string nombre;
        private string apellidos;
        private string fecha_nacimiento;
        private string id_mutua;
        private string tipo_aportacion;
        private decimal saldo;

        public CPaciente()
        {

        }

        public CPaciente(string id_paciente, string cite, string codigo_sns, string tsi, string nombre, string apellidos, string fecha_nacimiento, string id_mutua, string tipo_aportacion, decimal saldo)
        {
            this.id_paciente = id_paciente;
            this.cite = cite;
            this.codigo_sns = codigo_sns;
            this.tsi = tsi;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.fecha_nacimiento = fecha_nacimiento;
            this.id_mutua = id_mutua;
            this.tipo_aportacion = tipo_aportacion;
            this.saldo = saldo;
        }
        public string propId_Paciente
        {
            get { return id_paciente; }
            set { id_paciente = value; }
        }
        public string propCite
        {
            get { return cite; }
            set { cite = value; }
        }
        public string propCodigo_sns
        {
            get { return codigo_sns; }
            set { codigo_sns = value; }
        }
        public string propTsi
        {
            get { return tsi; }
            set { tsi = value; }
        }
        public string propNombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        public string propApellidos
        {
            get { return apellidos; }
            set { apellidos = value; }
        }
        public string propFecha_Nacimiento
        {
            get { return fecha_nacimiento; }
            set { fecha_nacimiento = value; }
        }
        public string propId_Mutua
        {
            get { return id_mutua; }
            set { id_mutua = value; }
        }
        public string propTipo_aportacion
        {
            get { return tipo_aportacion; }
            set { tipo_aportacion = value; }
        }
        public decimal propSaldo
        {
            get { return saldo; }
            set { saldo = value; }
        }
    }
}
