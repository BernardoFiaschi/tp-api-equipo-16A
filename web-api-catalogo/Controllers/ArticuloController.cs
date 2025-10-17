using System;
using System.Net;
using System.Web.Http;
using System.Data.SqlClient;
using Dominio;
using Negocio;
using web_api_catalogo.Models;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Mvc;

namespace web_api_catalogo.Controllers
{
    public class ArticuloController : ApiController
    {
        public List<Articulo> Get()
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                return negocio.Listar();
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            
        }

        public Articulo Get(int id)
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                Articulo art = new Articulo();

                art = negocio.ObtenerPorId(id);

                if (art == null)
                {
                    return art;
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                }

            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            
        }
        public HttpResponseMessage Post([FromBody] ArticuloDTO dto)
        {
            if (dto == null) return Request.CreateResponse(HttpStatusCode.BadRequest, "Articulo requerido.");
            if (string.IsNullOrWhiteSpace(dto.Codigo)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Codigo requerido.");
            if (string.IsNullOrWhiteSpace(dto.Nombre)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Nombre requerido.");
            if (dto.IdMarca <= 0) return Request.CreateResponse(HttpStatusCode.BadRequest, "La marca no existe.");
            if (dto.IdCategoria <= 0) return Request.CreateResponse(HttpStatusCode.BadRequest, "La categoria no existe.");
            if (dto.Precio < 0) return Request.CreateResponse(HttpStatusCode.BadRequest, "Precio invalido.");

            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                Articulo articulo = new Articulo
                {
                    Codigo = dto.Codigo,
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Marca = new Marca { Id = dto.IdMarca },
                    Categoria = new Categoria { Id = dto.IdCategoria },
                    Precio = dto.Precio
                };
                negocio.Agregar(articulo);

                return Request.CreateResponse(HttpStatusCode.OK, "Articulo agregado correctamente.");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }
        
       
        public HttpResponseMessage Put(int id, [FromBody] ArticuloDTO dto)
        {
            if (dto == null) return Request.CreateResponse(HttpStatusCode.BadRequest, "Articulo requerido.");
            if (string.IsNullOrWhiteSpace(dto.Codigo)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Codigo requerido.");
            if (string.IsNullOrWhiteSpace(dto.Nombre)) return Request.CreateResponse(HttpStatusCode.BadRequest, "Nombre requerido.");
            if (dto.IdMarca <= 0) return Request.CreateResponse(HttpStatusCode.BadRequest, "La marca no existe.");
            if (dto.IdCategoria <= 0) return Request.CreateResponse(HttpStatusCode.BadRequest, "La categoria no existe.");
            if (dto.Precio < 0) return Request.CreateResponse(HttpStatusCode.BadRequest, "Precio invalido.");

            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo existente = negocio.ObtenerPorId(id);
            if (existente == null) return Request.CreateResponse(HttpStatusCode.NotFound, "No hay articulo con ese ID.");

            try
            {
                existente.Codigo = dto.Codigo;
                existente.Nombre = dto.Nombre;
                existente.Descripcion = dto.Descripcion;
                existente.Marca = new Marca { Id = dto.IdMarca };
                existente.Categoria = new Categoria { Id = dto.IdCategoria };
                existente.Precio = dto.Precio;

                negocio.Modificar(existente);
                var actualizado = negocio.ObtenerPorId(id);

                return Request.CreateResponse(HttpStatusCode.OK, "Articulo modificado correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString()); 
            }
        }
        
        public HttpResponseMessage Delete(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo existente = negocio.ObtenerPorId(id);
            if (existente == null) return Request.CreateResponse(HttpStatusCode.NotFound, "No hay articulo con ese ID");

            try
            {
                negocio.Eliminar(id);

                return Request.CreateResponse(HttpStatusCode.OK, "Articulo eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.ToString());
            }
        }

        public HttpResponseMessage Post([FromBody]ImagenDTO dto)
        {
            if (dto == null) return Request.CreateResponse(HttpStatusCode.BadRequest, "Debe ingresar una imagen y articulo"); 
            if (dto.ImagenesUrls == null || dto.ImagenesUrls.Count == 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Debe ingresar al menos una imagen");

            var artNeg = new ArticuloNegocio();
            var existente = artNeg.ObtenerPorId(dto.IdArticulo);
            if (existente == null) return Request.CreateResponse(HttpStatusCode.NotFound, "No se encontro el articulo");

            try
            {
                var imgNeg = new ImagenNegocio();
                imgNeg.AgregarVarias(dto.IdArticulo, dto.ImagenesUrls);
                var actualizado = artNeg.ObtenerPorId(dto.IdArticulo);
                return Request.CreateResponse(HttpStatusCode.OK, "Imagenes agregadas correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }
        

        public List<Articulo> Get(string texto = null, int? idMarca = null, int? idCategoria = null, decimal? precioMin = null, decimal? precioMax = null)
        {
            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                List<Articulo> lista = negocio.Buscar(texto, idMarca, idCategoria, precioMin, precioMax);

                return lista;
            }
            catch (Exception)
            {

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
