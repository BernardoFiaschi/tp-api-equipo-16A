using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_catalogo.Models
{
    public class ArticuloDTO
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
        public decimal Precio { get; set; }
        public List<int> IdImagenes { get; set; } = new List<int>();
    }
}