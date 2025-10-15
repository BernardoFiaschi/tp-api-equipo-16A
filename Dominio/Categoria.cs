namespace Dominio.Objetos
{
    public class Categoria
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public Categoria() { }

        public Categoria(int id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }

        public override string ToString() => Descripcion;
    }
}

