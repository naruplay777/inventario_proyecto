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

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

        }

        private void CargarProductos()
        {
            DBHelper dbHelper = new DBHelper("Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;");
            List<Producto> productos = dbHelper.ObtenerProductos();

            dgvProductos.DataSource = productos; // Asignar la lista al DataGridView

            dgvProductos.Columns["CategoriaNombre"].HeaderText = "Categoría";
            dgvProductos.Columns["PresentacionDescripcion"].HeaderText = "Presentación";

            dgvProductos.Columns["CategoriaId"].Visible = false;
            dgvProductos.Columns["PresentacionId"].Visible = false;
        }

        private void FormularioProductos_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }
    }
}
