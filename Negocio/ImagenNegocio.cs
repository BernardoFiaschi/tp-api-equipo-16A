using Dominio;
using System;
using System.Collections.Generic;

namespace Negocio
{
    public class ImagenNegocio
    {
        public List<Imagen> ListarPorArticulo(int idArticulo)
        {
            var lista = new List<Imagen>();
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta("SELECT Id, IdArticulo, ImagenUrl FROM IMAGENES WHERE IdArticulo=@id");
                datos.setearParametro("@id", idArticulo);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    lista.Add(new Imagen
                    {
                        Id = (int)datos.Lector["Id"],
                        IdArticulo = (int)datos.Lector["IdArticulo"],
                        ImagenUrl = datos.Lector["ImagenUrl"] as string
                    });
                }
                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        public void Agregar(Imagen img)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@idArt, @url)");
                datos.setearParametro("@idArt", img.IdArticulo);
                datos.setearParametro("@url", img.ImagenUrl ?? (object)DBNull.Value);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Eliminar(int idImagen)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta("DELETE FROM IMAGENES WHERE Id=@id");
                datos.setearParametro("@id", idImagen);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void EliminarPorArticulo(int idArticulo)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta("DELETE FROM IMAGENES WHERE IdArticulo=@id");
                datos.setearParametro("@id", idArticulo);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void AgregarVarias(int idArticulo, IEnumerable<string> urls)
        {
            if (urls == null) return;
            foreach (var url in urls)
            {
                if (string.IsNullOrWhiteSpace(url)) continue;
                Agregar(new Imagen { IdArticulo = idArticulo, ImagenUrl = url.Trim() });
            }
        }
    }
}
