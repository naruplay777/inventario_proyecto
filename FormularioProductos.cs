﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class FormularioProductos : Form
    {
        private string rolUsuario;

        public FormularioProductos(string rol)
        {
            InitializeComponent();
            rolUsuario = rol;
            ConfigurarAcceso();
        }

        private void ConfigurarAcceso()
        {
            if (rolUsuario == "empleado")
            {
                btnAgregar.Enabled = false; //Productos
                btnEditar.Enabled = false; //Presentaciones
                AggIngreso.Enabled = true; //Ingresos
                AggCate.Enabled = false; //Categorias
                AggProv.Enabled = false; //Proveedores
                AggUsuarios.Enabled = false;
            }
        }

        Panel p = new Panel();
        private void btnMouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            panelMenu.Controls.Add(p);
            p.BackColor = Color.FromArgb(255, 215, 0);
            p.Size = new Size(140, 5);
            p.Location = new Point(btn.Location.X, btn.Location.Y + 26);
        }

        private void btnMouseLeave(object sender, EventArgs e)
        {
            panelMenu.Controls.Remove(p);
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
            using (var formulario = new presentacionForm())
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

            var producto = (Producto)dgvProductos.SelectedRows[0].DataBoundItem;

            var resultado = MessageBox.Show($"¿Está seguro de que desea eliminar el producto '{producto.Nombre}'?",
                                             "Confirmar eliminación",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                DBHelper dbHelper = new DBHelper("Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;");
                bool eliminado = dbHelper.EliminarProducto(producto.Id);

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
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"))
            {
                conn.Open();
                string query = @"
            SELECT 
                p.producto_id, 
                p.nombre, 
                p.descripcion, 
                p.categoria_id, 
                p.unidad_base, 
                p.stock_minimo, 
                p.activo, 
                i.stock_actual 
            FROM 
                productos p
            LEFT JOIN 
                inventario i ON p.producto_id = i.producto_id
            WHERE 
                p.activo = 1";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProductos.DataSource = dt;

                // Personaliza encabezados y oculta columnas si es necesario
                dgvProductos.Columns["producto_id"].HeaderText = "ID Producto";
                dgvProductos.Columns["producto_id"].Width = 50; // Ajustar el tamaño de la columna del ID
                dgvProductos.Columns["nombre"].HeaderText = "Nombre";
                dgvProductos.Columns["descripcion"].HeaderText = "Descripción";
                dgvProductos.Columns["categoria_id"].HeaderText = "Categoría";
                dgvProductos.Columns["unidad_base"].HeaderText = "Unidad Base";
                dgvProductos.Columns["stock_minimo"].HeaderText = "Stock Mínimo";
                dgvProductos.Columns["activo"].Visible = false;
                dgvProductos.Columns["stock_actual"].HeaderText = "Stock Actual";

                // Aplicar formato condicional a las celdas del stock actual
                foreach (DataGridViewRow row in dgvProductos.Rows)
                {
                    decimal stockActual = row.Cells["stock_actual"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["stock_actual"].Value) : 0;
                    decimal stockMinimo = row.Cells["stock_minimo"].Value != DBNull.Value ? Convert.ToDecimal(row.Cells["stock_minimo"].Value) : 0;

                    if (stockActual <= stockMinimo)
                    {
                        row.Cells["stock_actual"].Style.BackColor = Color.Red;
                        row.Cells["stock_actual"].Style.ForeColor = Color.White;
                    }
                    else if (stockActual <= stockMinimo + 10)
                    {
                        row.Cells["stock_actual"].Style.BackColor = Color.Yellow;
                        row.Cells["stock_actual"].Style.ForeColor = Color.Black;
                    }
                    else
                    {
                        row.Cells["stock_actual"].Style.BackColor = Color.Green;
                        row.Cells["stock_actual"].Style.ForeColor = Color.White;
                    }
                }
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!panelServicio.Visible)
            {
                panelServicio.Visible = true;
            }
            else
            {
                panelServicio.Visible = false;
            }
        }

        private void btnGenerarpdf_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Deseas descargar el documento?", "Éxito", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                // Crear la ruta del archivo PDF en la carpeta "reportes" dentro de la carpeta del ejecutable
                string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes");
                Directory.CreateDirectory(reportesPath); // Crear la carpeta si no existe

                // Generar un nombre de archivo único con marca de tiempo
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Path.Combine(reportesPath, $"PDFGenerado_{timestamp}.pdf");

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);

                    doc.Open();

                    // Título del PDF  
                    doc.AddAuthor("Wiredbox");
                    doc.AddTitle("Inventario");

                    // Definimos fuente  
                    iTextSharp.text.Font standarfint = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen  
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagen", "1000036045.jpg");
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen  

                    // Agregar la imagen al documento  
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título  
                    Paragraph title = new Paragraph("Reporte", standarfint);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento  
                    doc.Add(title);

                    // Agregar un salto de línea  
                    doc.Add(Chunk.NEWLINE);

                    // Encabezado de columnas   
                    PdfPTable tb = new PdfPTable(dgvProductos.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Count());
                    tb.WidthPercentage = 90;

                    // Agregar encabezados de columnas
                    foreach (DataGridViewColumn column in dgvProductos.Columns)
                    {
                        if (column.Visible)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, standarfint));
                            cell.BorderWidth = 1;
                            cell.BorderWidthBottom = 0.75f;
                            tb.AddCell(cell);
                        }
                    }

                    // Agregar datos de las filas
                    foreach (DataGridViewRow row in dgvProductos.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (dgvProductos.Columns[cell.ColumnIndex].Visible)
                            {
                                PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value?.ToString(), standarfint));
                                pdfCell.BorderWidth = 1;
                                pdfCell.BorderWidthBottom = 0.75f;
                                tb.AddCell(pdfCell);
                            }
                        }
                    }

                    doc.Add(tb);

                    // Cerrar el documento  
                    doc.Close();
                    pw.Close();

                    MessageBox.Show("Documento generado satisfactoriamente en: " + pdfPath, "Éxitos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void AggCate_Click(object sender, EventArgs e)
        {
            using (var formulario = new categoriaForm())
            {
                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    CargarProductos(); // Recarga el DataGridView
                }
            }

        }

        private void AggPresen_Click(object sender, EventArgs e)
        {
            using (var formulario = new presentacionForm())
            {
                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    CargarProductos(); // Recarga el DataGridView
                }
            }
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AggIngreso_Click(object sender, EventArgs e)
        {
            try
            {
                using (var formulario = new IngresoMercanciaForm())
                {
                    formulario.MercanciaIngresada += (s, args) => CargarProductos(); // Suscribirse al evento
                    if (formulario.ShowDialog() == DialogResult.OK)
                    {
                        CargarProductos(); // Recarga el DataGridView
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el formulario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AggProv_Click(object sender, EventArgs e)
        {
            using (var formulario = new proveedorForm())
            {
                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    CargarProductos(); // Recarga el DataGridView
                }
            }

        }

        private void AggUsuarios_Click(object sender, EventArgs e)
        {
            using (var formulario = new UsuarioForm())
            {
                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    CargarProductos(); // Recarga el DataGridView
                }
            }
        }

        private void AggEgreso_Click(object sender, EventArgs e)
        {
            using (var formulario = new EgresoMercanciaForm())
            {
                formulario.MercanciaEgresada += (s, args) => CargarProductos(); // Suscribirse al evento
                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    CargarProductos(); // Recarga el DataGridView
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var formulario = new repos())
            {
                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    CargarProductos(); // Recarga el DataGridView
                }
            }
        }

        private void carpeta_Click(object sender, EventArgs e)
        {
            string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes");
            if (Directory.Exists(reportesPath))
            {
                System.Diagnostics.Process.Start("explorer.exe", reportesPath);
            }
            else
            {
                MessageBox.Show("La carpeta de reportes no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

