﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace inventario_proyecto
{
    public partial class FormularioProductos : Form
    {
        public FormularioProductos()
        {
            InitializeComponent();
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
            DBHelper dbHelper = new DBHelper("Server=localhost;Database=inventario_heladeria;Uid=root;Pwd=andrewserver;");
            List<Producto> productos = dbHelper.ObtenerProductos();

            dgvProductos.DataSource = null; // Limpia el DataGridView
            dgvProductos.DataSource = productos;

            // Personaliza encabezados y oculta columnas si es necesario
            if (dgvProductos.Columns.Contains("CategoriaNombre"))
            {
                dgvProductos.Columns["CategoriaNombre"].HeaderText = "Categoría";
            }

            if (dgvProductos.Columns.Contains("PresentacionDescripcion"))
            {
                dgvProductos.Columns["PresentacionDescripcion"].HeaderText = "Presentación";
            }

            dgvProductos.Columns["CategoriaId"].Visible = false;
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
            MessageBox.Show("Deceas decargar el documento.", "Exito", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
           FileStream fs = new FileStream("C:\\Users\\Personal\\Documents\\PDFGenerado.pdf" , FileMode.Create);
            Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
            PdfWriter pw =  PdfWriter.GetInstance(doc, fs);

            doc.Open();

            //titulo del pdf
            doc.AddAuthor("Wiredbox");
            doc.AddTitle("CYROP");

            //definimos fuente
            iTextSharp.text.Font standarfint = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            //se crea el titulo
            Paragraph title = new Paragraph("Titulo del documento");

            // Centrar el título  
            title.Alignment = Element.ALIGN_CENTER;

            // Agregar el título al documento  
            doc.Add(title);

            // Agregar un salto de línea  
            doc.Add(Chunk.NEWLINE);


            //emcabezado de columnas 

            PdfPTable tb = new PdfPTable(6);
            tb.WidthPercentage = 90;

            //Configuracion del titulo de columnas 

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

            //Agregamos las columnas a la tabla
            tb.AddCell(clID);
            tb.AddCell(clNombre);
            tb.AddCell(clUnidad);
            tb.AddCell(clActual);
            tb.AddCell(clMinimo);
            tb.AddCell(clCategoria);

            //Agregamos datos aqui se aplica lo que es con la base de dato para los datos que se agreguen 
            /* foreach (var mercancia in productos )
             {
                 clID = new PdfPCell(new Phrase(mercancia.ID.ToString, standarfint));
             }*/

            doc.Add(tb);

            doc.Close();
            pw.Close();

            MessageBox.Show("Documento generado satisfactoriamente", "Exitos", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
