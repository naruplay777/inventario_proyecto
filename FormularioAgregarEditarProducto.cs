using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            // Cargar categorías y presentaciones desde la base de datos
            cmbCategoria.DataSource = dbHelper.ObtenerCategorias();
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "Id";

            cmbPresentacion.DataSource = dbHelper.ObtenerPresentaciones();
            cmbPresentacion.DisplayMember = "Nombre";
            cmbPresentacion.ValueMember = "Id";

            // Si estamos editando un producto, cargamos los datos
            if (Producto != null)
            {
                txtNombre.Text = Producto.Nombre;
                txtPrecio.Text = Producto.Precio.ToString();
                cmbCategoria.SelectedValue = Producto.CategoriaId;
                cmbPresentacion.SelectedValue = Producto.PresentacionId;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Verificar si los campos están vacíos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            // Si el producto es null, lo inicializamos
            Producto = Producto ?? new Producto();

            // Asignar los valores de los campos a las propiedades del producto
            Producto.Nombre = txtNombre.Text;
            Producto.Precio = decimal.Parse(txtPrecio.Text);
            Producto.CategoriaId = (int)cmbCategoria.SelectedValue;
            Producto.PresentacionId = (int)cmbPresentacion.SelectedValue;

            // Si el producto no tiene Id, es nuevo, entonces insertamos
            if (Producto.Id == 0)
            {
                dbHelper.InsertarProducto(Producto);
            }
            else
            {
                dbHelper.ActualizarProducto(Producto);
            }

            // Cerrar el formulario y devolver el resultado
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Cancelar el formulario y cerrar
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
