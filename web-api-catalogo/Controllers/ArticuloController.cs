using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Dominio;
using Negocio;

namespace web_api_catalogo.Controllers
{
    public class ArticuloController : ApiController 
    {
        public IEnumerable<Articulo> Get()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            return negocio.Listar();
        }

        // GET api/values/5
        public Articulo Get(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            return negocio.Listar().Find(x => x.Id == id);
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            negocio.Eliminar(id);
        }
    }
}