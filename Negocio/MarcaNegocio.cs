using Dominio.Objetos;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class MarcaNegocio
    {
        private const string consultaPrincipal = "SELECT Id, Descripcion FROM MARCAS ORDER BY Descripcion";
        private const string consultaAgregar = "INSERT INTO MARCAS (Descripcion) VALUES (@d)";
        private const string consultaModificar = "UPDATE MARCAS SET Descripcion=@d WHERE Id=@id";
        private const string consultaEliminar = "DELETE FROM MARCAS WHERE Id=@id";

        public List<Marca> Listar()
        {
            var lista = new List<Marca>();
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaPrincipal);
                datos.ejecutarLectura();

                var r = datos.Lector;           
                if (r != null)
                {
                    while (r.Read())
                    {
                        int id = r["Id"] is int v ? v : Convert.ToInt32(r["Id"]);
                        string descripcion = r["Descripcion"]?.ToString() ?? string.Empty;

                        lista.Add(new Marca { Id = id, Descripcion = descripcion });
                    }
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Agregar(Marca m)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaAgregar);
                datos.setearParametro("@d", m.Descripcion ?? string.Empty);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Modificar(Marca m)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaModificar);
                datos.setearParametro("@d", m.Descripcion ?? string.Empty);
                datos.setearParametro("@id", m.Id);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Eliminar(int id)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaEliminar);
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }
    }
}
