using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventario_proyecto
{
    public class Producto
    {
        // Propiedades que corresponden a la tabla "productos"
        public int Id { get; set; } // id_producto
        public string Nombre { get; set; } // nombre_producto
        public int CategoriaId { get; set; } // id_categoria
        public string UnidadMedida { get; set; } // unidad_medida
        public decimal StockActual { get; set; } // stock_actual
        public decimal StockMinimo { get; set; } // stock_minimo

        // Propiedades opcionales para relaciones con otras tablas (no se almacenan en la base de datos)
        public string CategoriaNombre { get; set; } // Nombre de la categoría

        // Método ToString para mostrar información del producto de manera legible
        public override string ToString()
        {
            return $"{Nombre} - {CategoriaNombre} - {UnidadMedida}";
        }
    }
}