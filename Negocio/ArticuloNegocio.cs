using Dominio.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ArticuloNegocio
    {
        //consultas a db
        string consultaPrincipal = @"
                SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.Precio,
                       M.Id AS IdMarca, M.Descripcion AS Marca,
                       C.Id AS IdCategoria, C.Descripcion AS Categoria
                FROM ARTICULOS A
                LEFT JOIN MARCAS M ON A.IdMarca = M.Id
                LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id
                ORDER BY A.Nombre";
        string consultaPorId = @"
                SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.Precio,
                       M.Id AS IdMarca, M.Descripcion AS Marca,
                       C.Id AS IdCategoria, C.Descripcion AS Categoria
                FROM ARTICULOS A
                LEFT JOIN MARCAS M ON A.IdMarca = M.Id
                LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id
                WHERE A.Id = @id";
        string consultaAgregar = @"
                INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio)
                OUTPUT INSERTED.Id
                VALUES (@cod, @nom, @desc, @idMarca, @idCat, @precio);";
        string consultaModificar = @"
                UPDATE ARTICULOS
                SET Codigo=@cod, Nombre=@nom, Descripcion=@desc,
                    IdMarca=@idMarca, IdCategoria=@idCat, Precio=@precio
                WHERE Id=@id";


        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDB datos = new AccesoDB();

            try
            {
                
                datos.setearConsulta(consultaPrincipal);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    var art = new Articulo
                    {
                        Id = (int)datos.Lector["Id"],
                        Codigo = datos.Lector["Codigo"] as string,
                        Nombre = datos.Lector["Nombre"] as string,
                        Descripcion = datos.Lector["Descripcion"] as string,
                        Precio = Convert.ToDecimal(datos.Lector["Precio"]),
                        Marca = new Marca
                        {
                            Id = datos.Lector["IdMarca"] != DBNull.Value ? (int)datos.Lector["IdMarca"] : 0,
                            Descripcion = datos.Lector["Marca"] as string
                        },
                        Categoria = new Categoria
                        {
                            Id = datos.Lector["IdCategoria"] != DBNull.Value ? (int)datos.Lector["IdCategoria"] : 0,
                            Descripcion = datos.Lector["Categoria"] as string
                        }
                    };
                    lista.Add(art);
                }
                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public Articulo ObtenerPorId(int id)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaPorId);
                                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                Articulo art = null;
                if (datos.Lector.Read())
                {
                    art = new Articulo
                    {
                        Id = (int)datos.Lector["Id"],
                        Codigo = datos.Lector["Codigo"] as string,
                        Nombre = datos.Lector["Nombre"] as string,
                        Descripcion = datos.Lector["Descripcion"] as string,
                        Precio = Convert.ToDecimal(datos.Lector["Precio"]),
                        Marca = new Marca
                        {
                            Id = datos.Lector["IdMarca"] != DBNull.Value ? (int)datos.Lector["IdMarca"] : 0,
                            Descripcion = datos.Lector["Marca"] as string
                        },
                        Categoria = new Categoria
                        {
                            Id = datos.Lector["IdCategoria"] != DBNull.Value ? (int)datos.Lector["IdCategoria"] : 0,
                            Descripcion = datos.Lector["Categoria"] as string
                        }
                    };
                }
                return art;
            }
            finally { datos.cerrarConexion(); }
        }

        public int Agregar(Articulo a)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaAgregar);
                datos.setearParametro("@cod", (object)a.Codigo ?? DBNull.Value);
                datos.setearParametro("@nom", a.Nombre);
                datos.setearParametro("@desc", (object)a.Descripcion ?? DBNull.Value);
                datos.setearParametro("@idMarca", a.Marca?.Id > 0 ? a.Marca.Id : (object)DBNull.Value);
                datos.setearParametro("@idCat", a.Categoria?.Id > 0 ? a.Categoria.Id : (object)DBNull.Value);
                datos.setearParametro("@precio", a.Precio);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                    return (int)datos.Lector[0];

                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            }
            finally { datos.cerrarConexion(); }
        }

        public void Modificar(Articulo a)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaModificar);
                datos.setearParametro("@cod", a.Codigo ?? (object)DBNull.Value);
                datos.setearParametro("@nom", a.Nombre);
                datos.setearParametro("@desc", (object)a.Descripcion ?? DBNull.Value);
                datos.setearParametro("@idMarca", a.Marca?.Id > 0 ? a.Marca.Id : (object)DBNull.Value);
                datos.setearParametro("@idCat", a.Categoria?.Id > 0 ? a.Categoria.Id : (object)DBNull.Value);
                datos.setearParametro("@precio", a.Precio);
                datos.setearParametro("@id", a.Id);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Eliminar(int id)
        {
            var datos = new AccesoDB();
            try
            {
                // Primero borro imágenes (1:N)
                datos.setearConsulta("DELETE FROM IMAGENES WHERE IdArticulo=@id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // Luego el artículo
                datos = new AccesoDB();
                datos.setearConsulta("DELETE FROM ARTICULOS WHERE Id=@id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

    }
}
