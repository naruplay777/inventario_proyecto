using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventario_proyecto
{
    public class Producto
    {

        // Propiedades básicas
        public int Id { get; set; } // ID único del producto
        public string Nombre { get; set; } // Nombre del producto
        public string Descripcion { get; set; } // Descripción opcional
        public int CategoriaId { get; set; } // ID de la categoría a la que pertenece
        public int PresentacionId { get; set; } // ID de la presentación asociada
        public decimal Precio { get; set; } // Precio actual del producto
        public int Stock { get; set; } // Cantidad en inventario

        // Propiedades para relaciones (opcionales pero útiles para mostrar en UI)
        public string CategoriaNombre { get; set; } // Nombre de la categoría
        public string PresentacionDescripcion { get; set; } // Descripción de la presentación

        // Métodos opcionales para lógica
        public override string ToString()
        {
            return $"{Nombre} - {CategoriaNombre} - {PresentacionDescripcion}";
        }
    }
}
