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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace inventario_proyecto
{
    public partial class IngresoMercanciaForm : Form
    {
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

            int productoId = Convert.ToInt32(cbProducto.SelectedValue);
            string nombreProducto = cbProducto.Text;
            int cantidad = Convert.ToInt32(txtCantidad.Text);
            decimal precioUnitario = Convert.ToDecimal(txtPrecio.Text);

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
                    foreach (DataGridViewRow row in dgvProductos.Rows)
                    {
                        int productoId = Convert.ToInt32(row.Cells["producto_id"].Value);
                        int cantidad = Convert.ToInt32(row.Cells["cantidad"].Value);
                        decimal precioUnitario = Convert.ToDecimal(row.Cells["precio_unitario"].Value);

                        string queryEntrada = "INSERT INTO entradas (producto_id, proveedor_id, usuario_id, cantidad, precio_unitario, fecha_entrada) VALUES (@productoId, @proveedorId, @usuarioId, @cantidad, @precio, NOW())";
                        MySqlCommand cmdEntrada = new MySqlCommand(queryEntrada, conn, transaction);
                        cmdEntrada.Parameters.AddWithValue("@productoId", productoId);
                        cmdEntrada.Parameters.AddWithValue("@proveedorId", proveedorId);
                        cmdEntrada.Parameters.AddWithValue("@usuarioId", usuarioId);
                        cmdEntrada.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdEntrada.Parameters.AddWithValue("@precio", precioUnitario);
                        cmdEntrada.ExecuteNonQuery();

                        string queryInventario = "UPDATE inventario SET stock_actual = stock_actual + @cantidad WHERE producto_id = @productoId";
                        MySqlCommand cmdInventario = new MySqlCommand(queryInventario, conn, transaction);
                        cmdInventario.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdInventario.Parameters.AddWithValue("@productoId", productoId);
                        cmdInventario.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Ingreso registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvProductos.Rows.Clear();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error al registrar el ingreso: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerarpdf_Click(object sender, EventArgs e)
        {

        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Deseas descargar el documento?", "Éxito", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                // Crear el archivo PDF  
                using (FileStream fs = new FileStream(@".\PDFGenerado.pdf", FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);

                    doc.Open();

                    // Título del PDF  
                    doc.AddAuthor("Wiredbox");
                    doc.AddTitle("CYROP");

                    // Definimos fuente  
                    iTextSharp.text.Font standarfint = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen  
                    string imagePath = "C:\\Users\\Personal\\Documents\\1000036045.jpg";
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen  

                    // Agregar la imagen al documento  
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título  
                    Paragraph title = new Paragraph("Título del documento", standarfint);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento  
                    doc.Add(title);

                    // Agregar un salto de línea  
                    doc.Add(Chunk.NEWLINE);

                    // Encabezado de columnas   
                    PdfPTable tb = new PdfPTable(6);
                    tb.WidthPercentage = 90;

                   /* // Configuración del título de columnas   
                    PdfPCell clID = new PdfPCell(new Phrase("ID", standarfint));
                    clID.BorderWidth = 1;
                    clID.BorderWidthBottom = 0.75f;

                    PdfPCell clNombre = new PdfPCell(new Phrase("Nombre", standarfint));
                    clNombre.BorderWidth = 1;
                    clNombre.BorderWidthBottom = 0.75f;

                    PdfPCell clUnidad = new PdfPCell(new Phrase("UnidadMedia", standarfint));
                    clUnidad.BorderWidth = 1;
                    clUnidad.BorderWidthBottom = 0.75f;

                    PdfPCell clActual = new PdfPCell(new Phrase("StockActual", standarfint));
                    clActual.BorderWidth = 1;
                    clActual.BorderWidthBottom = 0.75f;

                    PdfPCell clMinimo = new PdfPCell(new Phrase("StockMinimo", standarfint));
                    clMinimo.BorderWidth = 1;
                    clMinimo.BorderWidthBottom = 0.75f;

                    PdfPCell clCategoria = new PdfPCell(new Phrase("Categoria", standarfint));
                    clCategoria.BorderWidth = 1;
                    clCategoria.BorderWidthBottom = 0.75f;

                    // Agregamos las columnas a la tabla  
                    tb.AddCell(clID);
                    tb.AddCell(clNombre);
                    tb.AddCell(clUnidad);
                    tb.AddCell(clActual);
                    tb.AddCell(clMinimo);
                    tb.AddCell(clCategoria);

                    // Agregar datos aquí (ejemplo comentado)  
                    /* foreach (var mercancia in productos )  
                     {  
                         clID = new PdfPCell(new Phrase(mercancia.ID.ToString(), standarfint));  
                         tb.AddCell(clID);  
                     }*/

                    doc.Add(tb);

                    // Cerrar el documento  
                    doc.Close();
                    pw.Close();

                    MessageBox.Show("Documento generado satisfactoriamente", "Éxitos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
