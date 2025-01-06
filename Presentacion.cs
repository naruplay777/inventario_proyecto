using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventario_proyecto
{
    public class Presentacion
    {
        // Propiedades correspondientes a las columnas de la tabla 'presentaciones'
        public int Id { get; set; } // id_presentacion
        public string Descripcion { get; set; } // descripcion_presentacion
        public decimal CantidadPorPresentacion { get; set; } // cantidad_por_presentacion
        public string UnidadMedida { get; set; } // unidad_medida
        public decimal CostoPorPresentacion { get; set; } // costo_por_presentacion
        public DateTime FechaRegistro { get; set; } // fecha_registro
        public int ProductoId { get; set; } // id_producto

        // Constructor vacío
        public Presentacion() { }

        // Constructor con parámetros
        public Presentacion(int id, string descripcion, decimal cantidadPorPresentacion, string unidadMedida, decimal costoPorPresentacion, DateTime fechaRegistro, int productoId)
        {
            Id = id;
            Descripcion = descripcion;
            CantidadPorPresentacion = cantidadPorPresentacion;
            UnidadMedida = unidadMedida;
            CostoPorPresentacion = costoPorPresentacion;
            FechaRegistro = fechaRegistro;
            ProductoId = productoId;
        }

        // Método ToString para mostrar información en forma legible
        public override string ToString()
        {
            return $"{Descripcion} - {CantidadPorPresentacion} {UnidadMedida} - Costo: {CostoPorPresentacion}";
        }

        // Método para validar si la presentación es válida
        public bool EsValida()
        {
            return !string.IsNullOrEmpty(Descripcion) && CantidadPorPresentacion > 0 && CostoPorPresentacion > 0;
        }
    }
}