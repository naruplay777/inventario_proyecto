using System;
using System.Collections.Generic;

namespace inventario_proyecto
{
    public class Producto
    {
        // Propiedades que corresponden a la tabla "productos"
        public int Id { get; set; } // id_producto
        public string Nombre { get; set; } // nombre_producto
        public int CategoriaId { get; set; } // id_categoria
        public decimal StockActual { get; set; } = 0; // stock_actual
        public decimal StockMinimo { get; set; } = 0; // stock_minimo

        // Propiedades opcionales para relaciones con otras tablas
        public string CategoriaNombre { get; set; } // Nombre de la categoría

        // Lista de presentaciones relacionadas con este producto
        public List<Presentacion> Presentaciones { get; set; } = new List<Presentacion>();

        // Constructor vacío
        public Producto() { }

        // Constructor con parámetros
        public Producto(int id, string nombre, int categoriaId, decimal stockActual, decimal stockMinimo)
        {
            Id = id;
            Nombre = nombre;
            CategoriaId = categoriaId;
            StockActual = stockActual;
            StockMinimo = stockMinimo;
        }

        // Método ToString para mostrar información del producto de manera legible
        public override string ToString()
        {
            return $"{Nombre} - {CategoriaNombre} - Stock Actual: {StockActual}";
        }

        // Método para agregar una presentación a la lista de presentaciones
        public void AgregarPresentacion(Presentacion presentacion)
        {
            if (presentacion != null && presentacion.EsValida())
            {
                Presentaciones.Add(presentacion);
            }
        }
    }
}
