using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class FormularioAgregarEditarProducto : Form
    {
        public Producto Producto { get; private set; }
        private readonly DBHelper dbHelper;

        // Constructor que recibe un producto, en caso de editar
        public FormularioAgregarEditarProducto(Producto producto = null)
        {
            InitializeComponent();

            // Aquí se inicializa dbHelper con la cadena de conexión
            dbHelper = new DBHelper("Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;");

            Producto = producto;
        }

        private void FormularioAgregarEditarProducto_Load(object sender, EventArgs e)
        {
            // Cargar categorías desde la base de datos
            cmbCategoria.DataSource = dbHelper.ObtenerCategorias();
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "Id";

            // Cargar presentaciones desde la base de datos
            cmbPresentacion.DataSource = dbHelper.ObtenerPresentaciones();
            cmbPresentacion.DisplayMember = "Descripcion";
            cmbPresentacion.ValueMember = "Id";

            // Si estamos editando un producto, cargamos los datos
            if (Producto != null)
            {
                txtNombre.Text = Producto.Nombre;
                cmbCategoria.SelectedValue = Producto.CategoriaId;

                // Cargar la presentación asociada y su precio
                var presentacion = dbHelper.ObtenerPresentacionPorProducto(Producto.Id);
                txtPrecio.Text = presentacion?.CostoPorPresentacion.ToString();

                // Seleccionar la presentación que corresponde
                cmbPresentacion.SelectedValue = presentacion?.Id;
            }
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Verificar si los campos están vacíos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                cmbCategoria.SelectedIndex == -1 || cmbPresentacion.SelectedIndex == -1)
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            // Si el producto es null, lo inicializamos
            Producto = Producto ?? new Producto();

            // Asignar el nombre del producto
            Producto.Nombre = txtNombre.Text;

            // Asignar el ID de la categoría seleccionada
            Producto.CategoriaId = (int)cmbCategoria.SelectedValue;

            // Si el producto no tiene Id, es nuevo, entonces insertamos
            if (Producto.Id == 0)
            {
                // Insertar el producto en la base de datos
                dbHelper.InsertarProducto(Producto);

                // Después de insertar el producto, obtenemos el ID generado
                int productoId = dbHelper.ObtenerUltimoProductoId();

                // Crear una presentación con el precio y asociarla al producto
                var presentacion = new Presentacion
                {
                    Descripcion = cmbPresentacion.SelectedItem.ToString(), // Descripción de la presentación
                    CostoPorPresentacion = decimal.Parse(txtPrecio.Text), // Precio de la presentación
                    ProductoId = productoId // Referencia al ID del producto recién insertado
                };

                // Insertar la presentación
                dbHelper.InsertarPresentacion(presentacion);
            }
            else
            {
                // Si ya existe el producto, actualizamos el producto
                dbHelper.ActualizarProducto(Producto);

                // Y actualizamos la presentación con el precio actualizado
                var presentacion = new Presentacion
                {
                    ProductoId = Producto.Id, // El producto al que pertenece esta presentación
                    Descripcion = cmbPresentacion.SelectedItem.ToString(),
                    CostoPorPresentacion = decimal.Parse(txtPrecio.Text)
                };

                dbHelper.ActualizarPresentacion(presentacion);
            }

            // Cerrar el formulario y devolver el resultado
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public int ObtenerUltimoProductoId()
        {
            string connectionString = "Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT LAST_INSERT_ID()";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Error al obtener el ID del último producto: {ex.Message}");
                    return -1; // Retornar un valor que indica error
                }
            }
        }



        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Cancelar el formulario y cerrar
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
