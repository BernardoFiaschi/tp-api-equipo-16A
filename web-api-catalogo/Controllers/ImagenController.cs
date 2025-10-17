using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using web_api_catalogo.Models;

namespace web_api_catalogo.Controllers
{
    public class ImagenController : ApiController
    {
        public HttpResponseMessage Post([FromBody] ImagenDTO dto)
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
    }
}