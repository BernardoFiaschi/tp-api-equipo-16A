namespace tp_api_equipo_16A.DTOs
{
    public class ProductoUpdateDTO
    {
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public int? MarcaId { get; set; }
        public int? CategoriaId { get; set; }
    }
}
