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
    public partial class EgresoMercanciaForm : Form
    {
        public event EventHandler MercanciaEgresada; // Evento personalizado

        private ComboBox cbProducto;
        private Label label1;
        private DataGridView dgvProductos;
        private Label label2;
        private TextBox txtCantidad;
        private Label label3;
        private TextBox txtPrecio;
        private Button btnEliminar;
        private Button btnAgregar;
        private Button btnRegistrar;
        private string connectionString = "Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"; // Reemplaza con tu cadena de conexión

        public EgresoMercanciaForm()
        {
            InitializeComponent();
            ConfigurarDataGridView();
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
            if (dgvProductos.Rows.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int usuarioId = 1; // Reemplazar con el usuario logueado

            using (MySqlConnection conn = ConexionDB.GetConnection())
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insertar en la tabla 'salidas'
                    string querySalida = "INSERT INTO salidas (usuario_id, fecha_salida) VALUES (@usuarioId, NOW())";
                    MySqlCommand cmdSalida = new MySqlCommand(querySalida, conn, transaction);
                    cmdSalida.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmdSalida.ExecuteNonQuery();

                    // Obtener el ID de la salida recién insertada
                    long salidaId = cmdSalida.LastInsertedId;

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

                        // Insertar en la tabla 'detalle_salidas'
                        string queryDetalleSalida = "INSERT INTO detalle_salidas (salida_id, producto_id, cantidad, precio_unitario) VALUES (@salidaId, @productoId, @cantidad, @precio)";
                        MySqlCommand cmdDetalleSalida = new MySqlCommand(queryDetalleSalida, conn, transaction);
                        cmdDetalleSalida.Parameters.AddWithValue("@salidaId", salidaId);
                        cmdDetalleSalida.Parameters.AddWithValue("@productoId", productoId);
                        cmdDetalleSalida.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdDetalleSalida.Parameters.AddWithValue("@precio", precioUnitario);
                        cmdDetalleSalida.ExecuteNonQuery();

                        // Actualizar el inventario
                        string queryInventario = "UPDATE inventario SET stock_actual = stock_actual - @cantidad WHERE producto_id = @productoId";
                        MySqlCommand cmdInventario = new MySqlCommand(queryInventario, conn, transaction);
                        cmdInventario.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdInventario.Parameters.AddWithValue("@productoId", productoId);
                        cmdInventario.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Egreso registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvProductos.Rows.Clear();

                    // Disparar el evento personalizado
                    MercanciaEgresada?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error al registrar el egreso: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitializeComponent()
        {
            this.cbProducto = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnRegistrar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.SuspendLayout();
            // 
            // cbProducto
            // 
            this.cbProducto.FormattingEnabled = true;
            this.cbProducto.Location = new System.Drawing.Point(171, 42);
            this.cbProducto.Name = "cbProducto";
            this.cbProducto.Size = new System.Drawing.Size(121, 21);
            this.cbProducto.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Producto";
            // 
            // dgvProductos
            // 
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.Location = new System.Drawing.Point(171, 105);
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.Size = new System.Drawing.Size(409, 150);
            this.dgvProductos.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(309, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Cantidad";
            // 
            // txtCantidad
            // 
            this.txtCantidad.Location = new System.Drawing.Point(312, 43);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Size = new System.Drawing.Size(121, 20);
            this.txtCantidad.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(459, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Precio";
            // 
            // txtPrecio
            // 
            this.txtPrecio.Location = new System.Drawing.Point(459, 42);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(121, 20);
            this.txtPrecio.TabIndex = 12;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(413, 76);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.TabIndex = 15;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Location = new System.Drawing.Point(226, 76);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(75, 23);
            this.btnAgregar.TabIndex = 14;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Location = new System.Drawing.Point(40, 162);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(75, 23);
            this.btnRegistrar.TabIndex = 16;
            this.btnRegistrar.Text = "Registrar";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // EgresoMercanciaForm
            // 
            this.ClientSize = new System.Drawing.Size(692, 261);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPrecio);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCantidad);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbProducto);
            this.Name = "EgresoMercanciaForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
