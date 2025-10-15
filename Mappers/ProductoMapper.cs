using System.Linq;
using tp_api_equipo_16A.DTOs;
using Dominio.Objetos; 

namespace tp_api_equipo_16A.Mappers
{

    public static class ProductoMapper
    {

        public static ProductoDTO ToDTO(Articulo a)
        {
            return new ProductoDTO
            {
                Id = a.Id,
                Codigo = a.Codigo,
                Nombre = a.Nombre,
                Descripcion = a.Descripcion,
                Precio = a.Precio,

                Marca = a.Marca != null
                    ? new ItemRef { Id = a.Marca.Id, Descripcion = a.Marca.Descripcion }
                    : null,

                Categoria = a.Categoria != null
                    ? new ItemRef { Id = a.Categoria.Id, Descripcion = a.Categoria.Descripcion }
                    : null,

                Imagenes = a.Imagenes != null
                    ? a.Imagenes
                        .Where(i => i != null && !string.IsNullOrWhiteSpace(i.ImagenUrl))
                        .Select(i => i.ImagenUrl!)
                        .ToList()
                    : new System.Collections.Generic.List<string>()
            };
        }

        public static Articulo FromCreate(ProductoCreateDTO dto)
        {
            return new Articulo
            {
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion ?? string.Empty,
                Precio = dto.Precio,
                Marca = new Marca { Id = dto.MarcaId },
                Categoria = new Categoria { Id = dto.CategoriaId },
                Imagenes = new System.Collections.Generic.List<Imagen>()
            };
        }

        public static void ApplyUpdate(Articulo destino, ProductoUpdateDTO dto)
        {
            if (dto.Codigo != null) destino.Codigo = dto.Codigo;
            if (dto.Nombre != null) destino.Nombre = dto.Nombre;
            if (dto.Descripcion != null) destino.Descripcion = dto.Descripcion;
            if (dto.Precio.HasValue) destino.Precio = dto.Precio.Value;

            if (dto.MarcaId.HasValue)
            {
                destino.Marca ??= new Marca();
                destino.Marca.Id = dto.MarcaId.Value;
            }

            if (dto.CategoriaId.HasValue)
            {
                destino.Categoria ??= new Categoria();
                destino.Categoria.Id = dto.CategoriaId.Value;
            }
        }
    }
}
