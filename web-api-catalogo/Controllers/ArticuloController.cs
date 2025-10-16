using System;
using System.Net;
using System.Web.Http;
using System.Data.SqlClient;
using Dominio;
using Negocio;
using web_api_catalogo.Models;

namespace web_api_catalogo.Controllers
{
    [RoutePrefix("api/articulos")]
    public class ArticuloController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var negocio = new ArticuloNegocio();
            var lista = negocio.Listar();
            return Ok(lista);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var negocio = new ArticuloNegocio();
            var art = negocio.ObtenerPorId(id);
            if (art == null) return NotFound();
            return Ok(art);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] ArticuloDTO dto)
        {
            if (dto == null) return BadRequest("Payload requerido.");
            if (string.IsNullOrWhiteSpace(dto.Codigo)) return BadRequest("Codigo requerido.");
            if (string.IsNullOrWhiteSpace(dto.Nombre)) return BadRequest("Nombre requerido.");
            if (dto.IdMarca <= 0) return BadRequest("IdMarca invalido.");
            if (dto.IdCategoria <= 0) return BadRequest("IdCategoria invalido.");
            if (dto.Precio < 0) return BadRequest("Precio invalido.");

            try
            {
                var negocio = new ArticuloNegocio();
                var id = negocio.Agregar(new Articulo
                {
                    Codigo = dto.Codigo.Trim(),
                    Nombre = dto.Nombre.Trim(),
                    Descripcion = dto.Descripcion?.Trim(),
                    Marca = new Marca { Id = dto.IdMarca },
                    Categoria = new Categoria { Id = dto.IdCategoria },
                    Precio = dto.Precio
                });
                var creado = negocio.ObtenerPorId(id);
                return Created($"api/articulos/{id}", creado);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return Content(HttpStatusCode.Conflict, "Codigo duplicado.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, [FromBody] ArticuloDTO dto)
        {
            if (dto == null) return BadRequest("Payload requerido.");
            if (string.IsNullOrWhiteSpace(dto.Nombre)) return BadRequest("Nombre requerido.");
            if (dto.IdMarca <= 0) return BadRequest("IdMarca invalido.");
            if (dto.IdCategoria <= 0) return BadRequest("IdCategoria invalido.");
            if (dto.Precio < 0) return BadRequest("Precio invalido.");

            var negocio = new ArticuloNegocio();
            var existente = negocio.ObtenerPorId(id);
            if (existente == null) return NotFound();

            try
            {
                if (!string.IsNullOrWhiteSpace(dto.Codigo))
                    existente.Codigo = dto.Codigo.Trim();
                existente.Nombre = dto.Nombre.Trim();
                existente.Descripcion = dto.Descripcion?.Trim();
                existente.Marca = new Marca { Id = dto.IdMarca };
                existente.Categoria = new Categoria { Id = dto.IdCategoria };
                existente.Precio = dto.Precio;

                negocio.Modificar(existente);
                var actualizado = negocio.ObtenerPorId(id);
                return Ok(actualizado);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return Content(HttpStatusCode.Conflict, "Codigo duplicado.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var negocio = new ArticuloNegocio();
            var existente = negocio.ObtenerPorId(id);
            if (existente == null) return NotFound();

            try
            {
                negocio.Eliminar(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("{id:int}/imagenes")]
        public IHttpActionResult AgregarImagenes(int id, [FromBody] AgregarImagenesDto dto)
        {
            if (dto == null) return BadRequest("Payload requerido.");
            if (dto.IdArticulo != id) return BadRequest("IdArticulo inconsistente.");
            if (dto.ImagenesUrls == null || dto.ImagenesUrls.Count == 0)
                return BadRequest("Debe enviar al menos una imagen.");

            var artNeg = new ArticuloNegocio();
            var existente = artNeg.ObtenerPorId(id);
            if (existente == null) return NotFound();

            try
            {
                var imgNeg = new ImagenNegocio();
                imgNeg.AgregarVarias(id, dto.ImagenesUrls);
                var actualizado = artNeg.ObtenerPorId(id);
                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("buscar")]
        public IHttpActionResult Buscar(string texto = null, int? idMarca = null, int? idCategoria = null, decimal? precioMin = null, decimal? precioMax = null)
        {
            var negocio = new ArticuloNegocio();
            var lista = negocio.Buscar(texto, idMarca, idCategoria, precioMin, precioMax);
            return Ok(lista);
        }
    }
}
