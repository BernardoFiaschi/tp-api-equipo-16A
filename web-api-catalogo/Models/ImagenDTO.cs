using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_api_catalogo.Models
{
    public class ImagenDTO
    {
        public int IdArticulo { get; set; }
        public List<string> ImagenesUrls { get; set; }
    }
}