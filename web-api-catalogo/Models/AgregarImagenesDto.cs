using System.Collections.Generic;

namespace web_api_catalogo.Models
{
    public class AgregarImagenesDto
    {
        public int IdArticulo { get; set; }
        public List<string> ImagenesUrls { get; set; }
    }
}
