namespace tp_api_equipo_16A.DTOs
{
    public class ProductoCreateDTO
    {
        public string Codigo { get; set; } = string.Empty;   
        public string Nombre { get; set; } = string.Empty;   
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }                  
        public int MarcaId { get; set; }                    
        public int CategoriaId { get; set; }               
    }
}
