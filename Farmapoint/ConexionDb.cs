﻿using System.Data.OleDb;

namespace Farmapoint
{
    class ConexionDb
    {
        public static OleDbConnection AbrirConexion()
        {
            OleDbConnection conexion = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:/Users/Practica/Documents/Farmapointdb.accdb");
            conexion.Open();
            return conexion;
        }
    }
}
