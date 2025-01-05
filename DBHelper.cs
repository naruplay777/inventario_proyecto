using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public class DBHelper
    {
        private string connectionString;

        // Constructor que inicializa la cadena de conexión con un valor predeterminado
        public DBHelper()
        {
            // Cadena de conexión actualizada
            this.connectionString = "Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;";
        }

        // Constructor que permite pasar una cadena de conexión personalizada
        public DBHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Crear un nuevo producto
        public bool InsertarProducto(Producto producto)
        {
            string query = "INSERT INTO productos (nombre, descripcion, categoria_id, presentacion_id, precio, stock) " +
                           "VALUES (@nombre, @descripcion, @categoria_id, @presentacion_id, @precio, @stock)";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@categoria_id", producto.CategoriaId);
                    cmd.Parameters.AddWithValue("@presentacion_id", producto.PresentacionId);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    connection.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al insertar producto: {ex.Message}");
                    return false;
                }
            }
        }

        // Obtener todos los productos con detalles
        public List<Producto> ObtenerProductos()
        {
            string query = "SELECT p.id, p.nombre, p.descripcion, p.categoria_id, c.nombre AS categoria_nombre, " +
                           "p.presentacion_id, pr.descripcion AS presentacion_descripcion, p.precio, p.stock " +
                           "FROM productos p " +
                           "JOIN categorias c ON p.categoria_id = c.id " +
                           "JOIN presentaciones pr ON p.presentacion_id = pr.id";
            List<Producto> productos = new List<Producto>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new Producto
                            {
                                Id = reader.GetInt32("id"),
                                Nombre = reader.GetString("nombre"),
                                Descripcion = reader.GetString("descripcion"),
                                CategoriaId = reader.GetInt32("categoria_id"),
                                CategoriaNombre = reader.GetString("categoria_nombre"),
                                PresentacionId = reader.GetInt32("presentacion_id"),
                                PresentacionDescripcion = reader.GetString("presentacion_descripcion"),
                                Precio = reader.GetDecimal("precio"),
                                Stock = reader.GetInt32("stock")
                            });
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al obtener productos: {ex.Message}");
                }
            }
            return productos;
        }

        // Actualizar un producto
        public bool ActualizarProducto(Producto producto)
        {
            string query = "UPDATE productos SET nombre = @nombre, descripcion = @descripcion, " +
                           "categoria_id = @categoria_id, presentacion_id = @presentacion_id, precio = @precio, stock = @stock " +
                           "WHERE id = @id";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", producto.Id);
                    cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@categoria_id", producto.CategoriaId);
                    cmd.Parameters.AddWithValue("@presentacion_id", producto.PresentacionId);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@stock", producto.Stock);

                    connection.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al actualizar producto: {ex.Message}");
                    return false;
                }
            }
        }

        // Eliminar un producto
        public bool EliminarProducto(int productoId)
        {
            string query = "DELETE FROM productos WHERE id = @id";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", productoId);

                    connection.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al eliminar producto: {ex.Message}");
                    return false;
                }
            }
        }
    }
}