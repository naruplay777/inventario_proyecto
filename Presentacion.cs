using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventario_proyecto
{
    public class Presentacion
    {
        // Propiedades
        public int Id { get; set; } // ID de la presentación
        public string Descripcion { get; set; } // Descripción de la presentación

        // Constructor vacío
        public Presentacion() { }

        // Constructor con parámetros
        public Presentacion(int id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }

        // Método ToString para mostrar información en forma legible
        public override string ToString()
        {
            return $"{Descripcion}";
        }

        // Método para validar si la presentación es válida (ejemplo)
        public bool EsValida()
        {
            return !string.IsNullOrEmpty(Descripcion);
        }
    }
}
