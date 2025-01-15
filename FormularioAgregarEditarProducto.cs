using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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

            // Crear una nueva instancia de Producto si no existe
            Producto = Producto ?? new Producto();

            // Asignar valores al producto
            Producto.Nombre = txtNombre.Text;
            Producto.CategoriaId = (int)cmbCategoria.SelectedValue;

            // Crear la presentación asociada
            var presentacion = new Presentacion
            {
                Descripcion = cmbPresentacion.SelectedItem.ToString(),
                CostoPorPresentacion = decimal.Parse(txtPrecio.Text)
            };

            // Si el producto es nuevo (Id = 0), insertar
            if (Producto.Id == 0)
            {
                // Insertar producto junto con la presentación
                bool productoInsertado = dbHelper.InsertarProducto(Producto, presentacion);

                if (productoInsertado)
                {
                    MessageBox.Show("Producto y presentación guardados exitosamente.");
                }
                else
                {
                    MessageBox.Show("Error al guardar el producto y la presentación.");
                }
            }
            else
            {
                // Actualizar el producto existente
                bool productoActualizado = dbHelper.ActualizarProducto(Producto);
                if (!productoActualizado)
                {
                    MessageBox.Show("Error al actualizar el producto.");
                    return;
                }

                // Actualizar la presentación asociada
                presentacion.ProductoId = Producto.Id; // Asociar al producto existente
                bool presentacionActualizada = dbHelper.ActualizarPresentacion(presentacion);

                if (presentacionActualizada)
                {
                    MessageBox.Show("Producto y presentación actualizados exitosamente.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar la presentación.");
                }
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

        private void pictureCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        [DllImport("user32.Dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.Dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
