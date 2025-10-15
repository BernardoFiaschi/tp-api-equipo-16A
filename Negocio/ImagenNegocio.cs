using Dominio.Objetos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ImagenNegocio
    {
        //consultas a db
        string consultaPorId = "SELECT Id, IdArticulo, ImagenUrl FROM IMAGENES WHERE Id=@id";
        string consultaPorArticulo = "SELECT Id, IdArticulo, ImagenUrl FROM IMAGENES WHERE IdArticulo=@id";
        string consultaAgregar = "INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@art, @url)";
        string consultaEliminar = "DELETE FROM IMAGENES WHERE Id=@id";
        string consultaEliminarPorArticulo = "DELETE FROM IMAGENES WHERE IdArticulo=@id";


        public List<Imagen> ListarPorArticulo(int idArticulo)
        {
            var lista = new List<Imagen>();
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaPorArticulo);
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
                datos.setearConsulta(consultaAgregar);
                datos.setearParametro("@art", img.IdArticulo);
                datos.setearParametro("@url", img.ImagenUrl);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Eliminar(int idImagen)
        {
            var datos = new AccesoDB();
            try
            {
                datos.setearConsulta(consultaEliminar);
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
                datos.setearConsulta(consultaEliminarPorArticulo);
                datos.setearParametro("@id", idArticulo);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }
        private static readonly string _dirImagenes =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagenes");

        /// Expone la carpeta (por si la UI necesita componer rutas completas)
        public static string DirectorioImagenes => _dirImagenes;

        /// Si en BD guardaste sólo el nombre (recomendado), esto arma la ruta completa.
        public static string CombinarRuta(string almacenado)
        {
            if (string.IsNullOrWhiteSpace(almacenado)) return null;
            if (Path.IsPathRooted(almacenado)) return almacenado; // ya es full path local
            if (almacenado.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return almacenado; // URL remota
            return Path.Combine(_dirImagenes, almacenado);
        }

        /// Copia una imagen local a /imagenes con nombre único y devuelve el NOMBRE a guardar en BD
        public string GuardarImagenLocalRetornarNombre(string rutaOrigen)
        {
            if (string.IsNullOrWhiteSpace(rutaOrigen) || !File.Exists(rutaOrigen))
                throw new FileNotFoundException("No se encontró el archivo de imagen seleccionado.", rutaOrigen);

            Directory.CreateDirectory(_dirImagenes);

            var nombre = Path.GetFileName(rutaOrigen);
            var nombreSinExt = Path.GetFileNameWithoutExtension(nombre);
            var ext = Path.GetExtension(nombre);

            // Genero nombre único si ya existe
            string destino = Path.Combine(_dirImagenes, nombre);
            if (File.Exists(destino))
                destino = Path.Combine(_dirImagenes, $"{nombreSinExt}_{DateTime.Now:yyyyMMddHHmmssfff}{ext}");

            File.Copy(rutaOrigen, destino, overwrite: false);

            // 🚩 Recomendación: guardar en BD SOLO el nombre (no ruta absoluta)
            return Path.GetFileName(destino);
        }

        /// Atajo: copia y crea el registro en BD devolviendo el objeto Imagen
        public Imagen AgregarLocal(int idArticulo, string rutaOrigen)
        {
            var nombreParaBD = GuardarImagenLocalRetornarNombre(rutaOrigen);

            var img = new Imagen
            {
                IdArticulo = idArticulo,
                ImagenUrl = nombreParaBD
            };
            Agregar(img);
            return img;
        }
    }
}
