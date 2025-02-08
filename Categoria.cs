using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventario_proyecto
{
    public class Categoria
    {
        // Propiedades
        public int Id { get; set; } // ID de la categoría
        public string Nombre { get; set; } // Nombre de la categoría

        // Constructor vacío
        public Categoria() { }

        // Constructor con parámetros
        public Categoria(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        // Método ToString para mostrar información en forma legible
        public override string ToString()
        {
            return $"{Nombre}";
        }

        // Método para validar si la categoría es válida (ejemplo)
        public bool EsValida()
        {
            return !string.IsNullOrEmpty(Nombre);
        }
    }
}
