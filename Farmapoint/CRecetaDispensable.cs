namespace Farmapoint
{
    class CRecetaDispensable
    {
        private string id_Repositorio;
        private string identificador_Receta;
        private string fecha_Prescripcion;
        private string codigo_Producto_Prescrito;
        private string nombre_Producto_Prescrito;
        private bool es_Marca_Comercial;
        private int num_Envases;
        private string codigo_Centro_Prescriptor;
        private string tipo_Centro_Prescriptor;
        private string especialidad_Medico;
        private string nombre_Medico;

        public CRecetaDispensable()
        {

        }

        public CRecetaDispensable(string id_Repositorio)
        {
            this.id_Repositorio = id_Repositorio;
        }

        public CRecetaDispensable(string id_Repositorio, string identificador_Receta, string fecha_Prescripcion, string codigo_Producto_Prescrito, string nombre_Producto_Prescrito, bool es_Marca_Comercial, int num_Envases, string codigo_Centro_Prescriptor, string tipo_Centro_Prescriptor, string especialidad_Medico, string nombre_Medico)
        {
            this.id_Repositorio = id_Repositorio;
            this.identificador_Receta = identificador_Receta;
            this.fecha_Prescripcion = fecha_Prescripcion;
            this.codigo_Producto_Prescrito = codigo_Producto_Prescrito;
            this.nombre_Producto_Prescrito = nombre_Producto_Prescrito;
            this.es_Marca_Comercial = es_Marca_Comercial;
            this.num_Envases = num_Envases;
            this.codigo_Centro_Prescriptor = codigo_Centro_Prescriptor;
            this.tipo_Centro_Prescriptor = tipo_Centro_Prescriptor;
            this.especialidad_Medico = especialidad_Medico;
            this.nombre_Medico = nombre_Medico;
        }

        public string propId_Repositorio
        {
            get { return id_Repositorio; }
            set { id_Repositorio = value; }
        }

        public string propIdentificador_Receta
        {
            get { return identificador_Receta; }
            set { identificador_Receta = value; }
        }
        public string propFecha_Prescripcion
        {
            get { return fecha_Prescripcion; }
            set { fecha_Prescripcion = value; }
        }
        public string propCodigo_Producto_Prescrito
        {
            get { return codigo_Producto_Prescrito; }
            set { codigo_Producto_Prescrito = value; }
        }
        public string propNombre_Producto_Prescrito
        {
            get { return nombre_Producto_Prescrito; }
            set { nombre_Producto_Prescrito = value; }
        }
        public bool propEs_Marca_Comercial
        {
            get { return es_Marca_Comercial; }
            set { es_Marca_Comercial= value; }
        }
        public int propNum_Envases
        {
            get { return num_Envases; }
            set { num_Envases= value; }
        }
        public string propCodigo_Centro_Prescriptor
        {
            get { return codigo_Centro_Prescriptor; }
            set { codigo_Centro_Prescriptor = value; }
        }
        public string propTipo_Centro_Prescriptor
        {
            get { return tipo_Centro_Prescriptor; }
            set { tipo_Centro_Prescriptor = value; }
        }
        public string propEspecialidad_Medico
        {
            get { return especialidad_Medico; }
            set { especialidad_Medico = value; }
        }
        public string propNombre_Medico
        {
            get { return nombre_Medico; }
            set { nombre_Medico = value; }
        }

    }
}
