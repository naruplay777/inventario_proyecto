using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventario_proyecto
{
    public class Producto
    {

        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Cantidad { get; set; }
        public string UnidadMedida { get; set; }  // Por ejemplo: "caja", "cesta", "kg", "litro"
        public double PrecioUnitario { get; set; }

        // Constructor
        public Producto(int idProducto, string nombre, string descripcion, double cantidad, string unidadMedida, double precioUnitario)
        {
            IdProducto = idProducto;
            Nombre = nombre;
            Descripcion = descripcion;
            Cantidad = cantidad;
            UnidadMedida = unidadMedida;
            PrecioUnitario = precioUnitario;
        }

        // Método para agregar un producto a la base de datos
        public void AgregarProducto(DBHelper dbHelper)
        {
            string query = $"INSERT INTO productos (nombre, descripcion, cantidad, unidad_medida, precio_unitario) VALUES ('{Nombre}', '{Descripcion}', {Cantidad}, '{UnidadMedida}', {PrecioUnitario})";
            dbHelper.EjecutarComando(query);
        }

        // Método para actualizar un producto en la base de datos
        public void ActualizarProducto(DBHelper dbHelper)
        {
            string query = $"UPDATE productos SET nombre='{Nombre}', descripcion='{Descripcion}', cantidad={Cantidad}, unidad_medida='{UnidadMedida}', precio_unitario={PrecioUnitario} WHERE id_producto={IdProducto}";
            dbHelper.EjecutarComando(query);
        }

        // Método para eliminar un producto
        public void EliminarProducto(DBHelper dbHelper)
        {
            string query = $"DELETE FROM productos WHERE id_producto={IdProducto}";
            dbHelper.EjecutarComando(query);
        }

    }
}
