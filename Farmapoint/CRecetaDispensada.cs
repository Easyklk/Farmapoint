namespace Farmapoint
{
    class CRecetaDispensada
    {
        private string id_Repositorio;
        private string identificador_Receta;
        private string codigo_Producto_Dispensado;
        private string nombre_Producto_Dispensado;
        private int num_Envases;
        private decimal precio_Unitario;
        private decimal aportacion_Unitaria;
        private string tipo_Contigencia;
        private string tipo_Aportacion;
        private string codigo_Causa_Sustitucion;
        private string descripcion_Causa_Sustitucion;
        private string observaciones;

        public CRecetaDispensada()
        {

        }

        public CRecetaDispensada(string id_Repositorio)
        {
            this.id_Repositorio = id_Repositorio;
        }

        public CRecetaDispensada(string id_Repositorio, string identificador_Receta, string codigo_Producto_Dispensado, string nombre_Producto_Dispensado, int num_Envases, decimal precio_Unitario, decimal aportacion_Unitaria, string tipo_Contigencia, string tipo_Aportacion, string codigo_Causa_Sustitucion, string descripcion_Causa_Sustitucion, string observaciones) : this(id_Repositorio)
        {
            this.identificador_Receta = identificador_Receta;
            this.codigo_Producto_Dispensado = codigo_Producto_Dispensado;
            this.nombre_Producto_Dispensado = nombre_Producto_Dispensado;
            this.num_Envases = num_Envases;
            this.precio_Unitario = precio_Unitario;
            this.aportacion_Unitaria = aportacion_Unitaria;
            this.tipo_Contigencia = tipo_Contigencia;
            this.tipo_Aportacion = tipo_Aportacion;
            this.codigo_Causa_Sustitucion = codigo_Causa_Sustitucion;
            this.descripcion_Causa_Sustitucion = descripcion_Causa_Sustitucion;
            this.observaciones = observaciones;
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
        public string propCodigo_Producto_Dispensado
        {
            get { return codigo_Producto_Dispensado; }
            set { codigo_Producto_Dispensado = value; }
        }
        public string propNombre_Producto_Dispensado
        {
            get { return nombre_Producto_Dispensado; }
            set { nombre_Producto_Dispensado = value; }
        }
        public int propNum_Envases
        {
            get { return num_Envases; }
            set { num_Envases = value; }
        }

        public decimal propPrecio_Unitario
        {
            get { return propPrecio_Unitario; }
            set { propPrecio_Unitario = value; }
        }
        public decimal propAportacion_Unitaria
        {
            get { return aportacion_Unitaria; }
            set { aportacion_Unitaria = value; }
        }
        public string propTipo_Contingencia
        {
            get { return tipo_Contigencia; }
            set { tipo_Contigencia = value; }
        }
        public string propTipo_Aportacion
        {
            get { return tipo_Aportacion; }
            set { tipo_Contigencia = value; }
        }
        public string propCodigo_Causa_Sustitucion
        {
            get { return codigo_Causa_Sustitucion; }
            set { codigo_Causa_Sustitucion = value; }
        }
        public string propDescripcion_Causa_Sustitucion
        {
            get { return descripcion_Causa_Sustitucion; }
            set { descripcion_Causa_Sustitucion = value; }
        }
        public string propObservaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }

        public override string ToString()
        {
            return "ID_Repositorio: " + id_Repositorio + " Identificador Receta: " + identificador_Receta + "\n" + " Codigo de productor dispensado: "
                + codigo_Producto_Dispensado + " Nombre de producto dispensado: " + nombre_Producto_Dispensado + "\n" + " Numero de envases: "
                + num_Envases + " Precio unitario: " + precio_Unitario + "\n Aportacion unitaria: " + aportacion_Unitaria + " Tipo contingencia: "
                + tipo_Contigencia + "\nTipo aportacion: " + tipo_Aportacion + " Codigo causa sustitucion: " + codigo_Causa_Sustitucion
                + " Observacion: " + observaciones;
        }

    }
}
