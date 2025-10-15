using Dominio.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CategoriaNegocio
    {
        string consultaPrincipal = "SELECT Id, Descripcion FROM CATEGORIAS ORDER BY Descripcion";
        string consultaAgregar = "INSERT INTO CATEGORIAS (Descripcion) VALUES (@d)";
        string consultaModificar = "UPDATE CATEGORIAS SET Descripcion=@d WHERE Id=@id";
        string consultaEliminar = "DELETE FROM CATEGORIAS WHERE Id=@id";
        public List<Categoria> Listar()
        {
            var lista = new List<Categoria>();
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaPrincipal);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                    lista.Add(new Categoria { Id = (int)datos.Lector["Id"], Descripcion = datos.Lector["Descripcion"] as string });
                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        public void Agregar(Categoria c)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaAgregar);
                datos.setearParametro("@d", c.Descripcion);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Modificar(Categoria c)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaModificar);
                datos.setearParametro("@d", c.Descripcion);
                datos.setearParametro("@id", c.Id);
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
