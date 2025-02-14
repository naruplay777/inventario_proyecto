using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class IngresoMercanciaForm : Form
    {
        public event EventHandler MercanciaIngresada; // Evento personalizado

        private string connectionString = "Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"; // Reemplaza con tu cadena de conexión

        public IngresoMercanciaForm()
        {
            InitializeComponent();
            ConfigurarDataGridView();
            CargarProveedores();
            CargarProductos();
        }

        private class ConexionDB
        {
            private static string connectionString = "Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;";

            public static MySqlConnection GetConnection()
            {
                return new MySqlConnection(connectionString);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvProductos.Columns.Add("producto_id", "ID Producto");
            dgvProductos.Columns.Add("nombre_producto", "Nombre Producto");
            dgvProductos.Columns.Add("cantidad", "Cantidad");
            dgvProductos.Columns.Add("precio_unitario", "Precio Unitario");
        }

        private void CargarProveedores()
        {
            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                string query = "SELECT proveedor_id, nombre FROM proveedores WHERE activo = 1";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbProveedor.DataSource = dt;
                cbProveedor.DisplayMember = "nombre";
                cbProveedor.ValueMember = "proveedor_id";
            }
        }

        private void CargarProductos()
        {
            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                string query = "SELECT producto_id, nombre FROM productos WHERE activo = 1";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbProducto.DataSource = dt;
                cbProducto.DisplayMember = "nombre";
                cbProducto.ValueMember = "producto_id";
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cbProducto.SelectedValue == null || string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Debe completar todos los campos antes de agregar un producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productoId;
            if (!int.TryParse(cbProducto.SelectedValue.ToString(), out productoId))
            {
                MessageBox.Show("ID de producto no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreProducto = cbProducto.Text;
            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad))
            {
                MessageBox.Show("Cantidad no válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal precioUnitario;
            if (!decimal.TryParse(txtPrecio.Text, out precioUnitario))
            {
                MessageBox.Show("Precio unitario no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dgvProductos.Rows.Add(productoId, nombreProducto, cantidad, precioUnitario);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvProductos.SelectedRows)
            {
                dgvProductos.Rows.Remove(row);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cbProveedor.SelectedValue == null || dgvProductos.Rows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar un proveedor y agregar al menos un producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int proveedorId = Convert.ToInt32(cbProveedor.SelectedValue);
            int usuarioId = 1; // Reemplazar con el usuario logueado

            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insertar en la tabla 'entradas'
                    string queryEntrada = "INSERT INTO entradas (proveedor_id, usuario_id, fecha_entrada) VALUES (@proveedorId, @usuarioId, NOW())";
                    MySqlCommand cmdEntrada = new MySqlCommand(queryEntrada, conn, transaction);
                    cmdEntrada.Parameters.AddWithValue("@proveedorId", proveedorId);
                    cmdEntrada.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmdEntrada.ExecuteNonQuery();

                    // Obtener el ID de la entrada recién insertada
                    long entradaId = cmdEntrada.LastInsertedId;

                    foreach (DataGridViewRow row in dgvProductos.Rows)
                    {
                        if (row.IsNewRow) continue; // Saltar las filas nuevas vacías

                        int productoId = Convert.ToInt32(row.Cells["producto_id"].Value);
                        int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);
                        decimal precioUnitario = Convert.ToDecimal(row.Cells["precio_unitario"].Value);

                        // Verificar si el producto existe en la tabla productos
                        string queryVerificarProducto = "SELECT COUNT(*) FROM productos WHERE producto_id = @productoId";
                        MySqlCommand cmdVerificarProducto = new MySqlCommand(queryVerificarProducto, conn, transaction);
                        cmdVerificarProducto.Parameters.AddWithValue("@productoId", productoId);
                        int productoExiste = Convert.ToInt32(cmdVerificarProducto.ExecuteScalar());

                        if (productoExiste == 0)
                        {
                            throw new Exception($"El producto con ID {productoId} no existe en la tabla productos.");
                        }

                        // Insertar en la tabla 'detalle_entradas'
                        string queryDetalleEntrada = "INSERT INTO detalle_entradas (entrada_id, producto_id, cantidad, precio_unitario) VALUES (@entradaId, @productoId, @cantidad, @precio)";
                        MySqlCommand cmdDetalleEntrada = new MySqlCommand(queryDetalleEntrada, conn, transaction);
                        cmdDetalleEntrada.Parameters.AddWithValue("@entradaId", entradaId);
                        cmdDetalleEntrada.Parameters.AddWithValue("@productoId", productoId);
                        cmdDetalleEntrada.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdDetalleEntrada.Parameters.AddWithValue("@precio", precioUnitario);
                        cmdDetalleEntrada.ExecuteNonQuery();

                        // Actualizar el inventario
                        string queryInventario = "INSERT INTO inventario (producto_id, stock_actual) VALUES (@productoId, @cantidad) ON DUPLICATE KEY UPDATE stock_actual = stock_actual + @cantidad";
                        MySqlCommand cmdInventario = new MySqlCommand(queryInventario, conn, transaction);
                        cmdInventario.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdInventario.Parameters.AddWithValue("@productoId", productoId);
                        cmdInventario.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Ingreso registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvProductos.Rows.Clear();

                    // Disparar el evento personalizado
                    MercanciaIngresada?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error al registrar el ingreso: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}


