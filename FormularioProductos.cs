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
    public partial class FormularioProductos : Form
    {
        public FormularioProductos()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (var formulario = new FormularioAgregarEditarProducto())
            {
                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    CargarProductos(); // Recarga el DataGridView
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para editar.");
                return;
            }

            var producto = (Producto)dgvProductos.SelectedRows[0].DataBoundItem;

            using (var formulario = new FormularioAgregarEditarProducto(producto))
            {
                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    CargarProductos(); // Recarga el DataGridView
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                return;
            }

            // Obtén el producto seleccionado desde el DataGridView
            var producto = (Producto)dgvProductos.SelectedRows[0].DataBoundItem;

            var resultado = MessageBox.Show($"¿Está seguro de que desea eliminar el producto '{producto.Nombre}'?",
                                             "Confirmar eliminación",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                DBHelper dbHelper = new DBHelper("Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;");
                bool eliminado = dbHelper.EliminarProducto(producto.IdProducto); // Usa IdProducto en vez de Id

                if (eliminado)
                {
                    MessageBox.Show("Producto eliminado exitosamente.");
                    CargarProductos();
                }
                else
                {
                    MessageBox.Show("Hubo un error al eliminar el producto.");
                }
            }
        }



        private void CargarProductos()
        {
            DBHelper dbHelper = new DBHelper("Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;");
            List<Producto> productos = dbHelper.ObtenerProductos();

            // Limpia el DataGridView y recarga los datos
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = productos;

            // Personaliza encabezados y oculta columnas si es necesario
            dgvProductos.Columns["CategoriaId"].Visible = false;

            if (dgvProductos.Columns.Contains("CategoriaNombre"))
            {
                dgvProductos.Columns["CategoriaNombre"].HeaderText = "Categoría";
            }

            if (dgvProductos.Columns.Contains("PresentacionDescripcion"))
            {
                dgvProductos.Columns["PresentacionDescripcion"].HeaderText = "Presentación";
            }
        }



        private void FormularioProductos_Load(object sender, EventArgs e)
        {
            CargarProductos(); // Cargar productos al iniciar el formulario
        }

        private void pictureBoxCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
