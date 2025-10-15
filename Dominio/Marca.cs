using System;

namespace Dominio
{
    public class Marca
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public Marca() { }

        public Marca(int id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }

        public override string ToString() => Descripcion;
    }
}

