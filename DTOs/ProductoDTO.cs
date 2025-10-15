using System.Collections.Generic;

namespace tp_api_equipo_16A.DTOs
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public ItemRef? Marca { get; set; }
        public ItemRef? Categoria { get; set; }
        public List<string> Imagenes { get; set; } = new List<string>(); // solo URLs
    }
}
