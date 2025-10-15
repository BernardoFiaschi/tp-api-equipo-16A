using System;
using Microsoft.Data.SqlClient;

namespace Negocio
{
    public class AccesoDB
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader? lector;

        public SqlDataReader? Lector => lector;

        public AccesoDB()
        {
            conexion = new SqlConnection(DbConfig.ConnectionString);
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void setearProcedimiento(string sp)
        {
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = sp;
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar lectura: " + ex.Message);
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar accion: " + ex.Message);
            }
        }

        public void cerrarConexion()
        {
            try
            {
                if (lector != null && !lector.IsClosed) lector.Close();
                if (conexion != null && conexion.State == System.Data.ConnectionState.Open) conexion.Close();
                comando.Parameters.Clear();
            }
            catch
            {

            }
        }
    }
}

