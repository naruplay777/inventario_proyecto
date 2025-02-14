using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;

namespace inventario_proyecto
{
    public partial class Formularios : Form
    {



        private void btnReporteProveedoresActivos_Click(object sender, EventArgs e)
        {
            GenerarReporteProveedoresActivos();
        }

        private void btnReporteUsuariosRegistrados_Click(object sender, EventArgs e)
        {
            GenerarReporteUsuariosRegistrados();
        }

        private void btnReporteCategoriasPresentaciones_Click(object sender, EventArgs e)
        {
            GenerarReporteCategoriasPresentaciones();
        }

        private void btnReporteInventarioActual_Click(object sender, EventArgs e)
        {
            GenerarReporteInventarioActual();
        }

        private void btnReporteEntradasMercancia_Click(object sender, EventArgs e)
        {
            GenerarReporteEntradasMercancia();
        }

        private void btnReporteSalidasMercancia_Click(object sender, EventArgs e)
        {
            GenerarReporteSalidasMercancia();
        }

        private void btnReporteBajoStock_Click(object sender, EventArgs e)
        {
            GenerarReporteBajoStock();
        }

        private void btnReporteComprasProveedor_Click(object sender, EventArgs e)
        {
            GenerarReporteComprasProveedor();
        }

        private void btnReporteProductosSinMovimiento_Click(object sender, EventArgs e)
        {
            GenerarReporteProductosSinMovimiento();
        }

        private void GenerarReporteProveedoresActivos()
        {
            string query = "SELECT proveedor_id AS 'ID', nombre AS 'Nombre', direccion AS 'Dirección', telefono AS 'Teléfono', email AS 'Email', fecha_registro AS 'Fecha de Registro' FROM proveedores WHERE activo = 1";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"))
            {
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Crear la ruta del archivo PDF en la carpeta "reportes/supervision" dentro de la carpeta del ejecutable
                string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes", "supervision");
                Directory.CreateDirectory(reportesPath); // Crear la carpeta si no existe

                // Generar un nombre de archivo único con marca de tiempo
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Path.Combine(reportesPath, $"ReporteProveedoresActivos_{timestamp}.pdf");

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Título del PDF
                    doc.AddAuthor("Inventario Proyecto");
                    doc.AddTitle("Reporte de Proveedores Activos");

                    // Definimos fuente
                    iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagen", "1000036045.jpg");
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen

                    // Agregar la imagen al documento
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título
                    Paragraph title = new Paragraph("Reporte de Proveedores Activos", standardFont);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento
                    doc.Add(title);

                    // Agregar un salto de línea
                    doc.Add(Chunk.NEWLINE);

                    // Tabla
                    PdfPTable table = new PdfPTable(dt.Columns.Count);
                    table.WidthPercentage = 100;

                    // Encabezados
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, standardFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    // Datos
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            table.AddCell(new Phrase(item.ToString(), standardFont));
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                    pw.Close();
                }

                MessageBox.Show("Reporte de Proveedores Activos generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerarReporteUsuariosRegistrados()
        {
            string query = "SELECT usuario_id AS 'ID', nombre AS 'Nombre', email AS 'Email', rol AS 'Rol', fecha_registro AS 'Fecha de Registro' FROM usuarios";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"))
            {
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Crear la ruta del archivo PDF en la carpeta "reportes/supervision" dentro de la carpeta del ejecutable
                string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes", "supervision");
                Directory.CreateDirectory(reportesPath); // Crear la carpeta si no existe

                // Generar un nombre de archivo único con marca de tiempo
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Path.Combine(reportesPath, $"ReporteUsuariosRegistrados_{timestamp}.pdf");

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Título del PDF
                    doc.AddAuthor("Inventario Proyecto");
                    doc.AddTitle("Reporte de Usuarios Registrados");

                    // Definimos fuente
                    iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagen", "1000036045.jpg");
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen

                    // Agregar la imagen al documento
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título
                    Paragraph title = new Paragraph("Reporte de Usuarios Registrados", standardFont);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento
                    doc.Add(title);

                    // Agregar un salto de línea
                    doc.Add(Chunk.NEWLINE);

                    // Tabla
                    PdfPTable table = new PdfPTable(dt.Columns.Count);
                    table.WidthPercentage = 100;

                    // Encabezados
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, standardFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    // Datos
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            table.AddCell(new Phrase(item.ToString(), standardFont));
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                    pw.Close();
                }

                MessageBox.Show("Reporte de Usuarios Registrados generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerarReporteCategoriasPresentaciones()
        {
            string queryCategorias = @"
        SELECT 
            categoria_id AS 'ID Categoría', 
            nombre AS 'Nombre Categoría', 
            descripcion AS 'Descripción Categoría'
        FROM 
            categorias
        WHERE 
            activo = 1";

            string queryPresentaciones = @"
        SELECT 
            presentacion_id AS 'ID Presentación', 
            nombre AS 'Nombre Presentación', 
            descripcion AS 'Descripción Presentación', 
            factor AS 'Factor'
        FROM 
            presentaciones
        WHERE 
            activo = 1";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"))
            {
                conn.Open();

                // Obtener datos de categorías
                MySqlDataAdapter daCategorias = new MySqlDataAdapter(queryCategorias, conn);
                DataTable dtCategorias = new DataTable();
                daCategorias.Fill(dtCategorias);

                // Obtener datos de presentaciones
                MySqlDataAdapter daPresentaciones = new MySqlDataAdapter(queryPresentaciones, conn);
                DataTable dtPresentaciones = new DataTable();
                daPresentaciones.Fill(dtPresentaciones);

                // Crear la ruta del archivo PDF en la carpeta "reportes/supervision" dentro de la carpeta del ejecutable
                string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes", "supervision");
                Directory.CreateDirectory(reportesPath); // Crear la carpeta si no existe

                // Generar un nombre de archivo único con marca de tiempo
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Path.Combine(reportesPath, $"ReporteCategoriasPresentaciones_{timestamp}.pdf");

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Título del PDF
                    doc.AddAuthor("Inventario Proyecto");
                    doc.AddTitle("Reporte de Categorías y Presentaciones");

                    // Definimos fuente
                    iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagen", "1000036045.jpg");
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen

                    // Agregar la imagen al documento
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título
                    Paragraph title = new Paragraph("Reporte de Categorías y Presentaciones", standardFont);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento
                    doc.Add(title);

                    // Agregar un salto de línea
                    doc.Add(Chunk.NEWLINE);

                    // Tabla de Categorías
                    Paragraph categoriasTitle = new Paragraph("Categorías", standardFont);
                    categoriasTitle.Alignment = Element.ALIGN_LEFT;
                    doc.Add(categoriasTitle);
                    doc.Add(Chunk.NEWLINE);

                    PdfPTable tableCategorias = new PdfPTable(dtCategorias.Columns.Count);
                    tableCategorias.WidthPercentage = 100;

                    // Encabezados de Categorías
                    foreach (DataColumn column in dtCategorias.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, standardFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        tableCategorias.AddCell(cell);
                    }

                    // Datos de Categorías
                    foreach (DataRow row in dtCategorias.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            tableCategorias.AddCell(new Phrase(item.ToString(), standardFont));
                        }
                    }

                    doc.Add(tableCategorias);
                    doc.Add(Chunk.NEWLINE);

                    // Tabla de Presentaciones
                    Paragraph presentacionesTitle = new Paragraph("Presentaciones", standardFont);
                    presentacionesTitle.Alignment = Element.ALIGN_LEFT;
                    doc.Add(presentacionesTitle);
                    doc.Add(Chunk.NEWLINE);

                    PdfPTable tablePresentaciones = new PdfPTable(dtPresentaciones.Columns.Count);
                    tablePresentaciones.WidthPercentage = 100;

                    // Encabezados de Presentaciones
                    foreach (DataColumn column in dtPresentaciones.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, standardFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        tablePresentaciones.AddCell(cell);
                    }

                    // Datos de Presentaciones
                    foreach (DataRow row in dtPresentaciones.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            tablePresentaciones.AddCell(new Phrase(item.ToString(), standardFont));
                        }
                    }

                    doc.Add(tablePresentaciones);
                    doc.Close();
                    pw.Close();
                }

                MessageBox.Show("Reporte de Categorías y Presentaciones generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerarReporteInventarioActual()
        {
            string query = @"
        SELECT 
            p.producto_id AS 'ID Producto', 
            p.nombre AS 'Nombre', 
            p.descripcion AS 'Descripción', 
            c.nombre AS 'Categoría', 
            p.unidad_base AS 'Unidad Base', 
            i.stock_actual AS 'Stock Actual', 
            p.stock_minimo AS 'Stock Mínimo'
        FROM 
            productos p
        LEFT JOIN 
            inventario i ON p.producto_id = i.producto_id
        JOIN 
            categorias c ON p.categoria_id = c.categoria_id
        WHERE 
            p.activo = 1";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"))
            {
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Crear la ruta del archivo PDF en la carpeta "reportes/operativos" dentro de la carpeta del ejecutable
                string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes", "operativos");
                Directory.CreateDirectory(reportesPath); // Crear la carpeta si no existe

                // Generar un nombre de archivo único con marca de tiempo
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Path.Combine(reportesPath, $"ReporteInventarioActual_{timestamp}.pdf");

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Título del PDF
                    doc.AddAuthor("Inventario Proyecto");
                    doc.AddTitle("Reporte de Inventario Actual");

                    // Definimos fuente
                    iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagen", "1000036045.jpg");
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen

                    // Agregar la imagen al documento
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título
                    Paragraph title = new Paragraph("Reporte de Inventario Actual", standardFont);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento
                    doc.Add(title);

                    // Agregar un salto de línea
                    doc.Add(Chunk.NEWLINE);

                    // Tabla
                    PdfPTable table = new PdfPTable(dt.Columns.Count);
                    table.WidthPercentage = 100;

                    // Encabezados
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, standardFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    // Datos
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            table.AddCell(new Phrase(item.ToString(), standardFont));
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                    pw.Close();
                }

                MessageBox.Show("Reporte de Inventario Actual generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void GenerarReporteEntradasMercancia()
        {
            string query = @"
        SELECT 
            e.entrada_id AS 'ID Entrada', 
            e.fecha_entrada AS 'Fecha de Entrada', 
            p.nombre AS 'Proveedor', 
            d.producto_id AS 'ID Producto', 
            pr.nombre AS 'Producto', 
            d.cantidad AS 'Cantidad', 
            d.precio_unitario AS 'Precio Unitario'
        FROM 
            entradas e
        JOIN 
            proveedores p ON e.proveedor_id = p.proveedor_id
        JOIN 
            detalle_entradas d ON e.entrada_id = d.entrada_id
        JOIN 
            productos pr ON d.producto_id = pr.producto_id
        WHERE 
            e.fecha_entrada >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH)";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"))
            {
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Crear la ruta del archivo PDF en la carpeta "reportes/operativos" dentro de la carpeta del ejecutable
                string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes", "operativos");
                Directory.CreateDirectory(reportesPath); // Crear la carpeta si no existe

                // Generar un nombre de archivo único con marca de tiempo
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Path.Combine(reportesPath, $"ReporteEntradasMercancia_{timestamp}.pdf");

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Título del PDF
                    doc.AddAuthor("Inventario Proyecto");
                    doc.AddTitle("Reporte de Entradas de Mercancía");

                    // Definimos fuente
                    iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagen", "1000036045.jpg");
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen

                    // Agregar la imagen al documento
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título
                    Paragraph title = new Paragraph("Reporte de Entradas de Mercancía", standardFont);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento
                    doc.Add(title);

                    // Agregar un salto de línea
                    doc.Add(Chunk.NEWLINE);

                    // Tabla
                    PdfPTable table = new PdfPTable(dt.Columns.Count);
                    table.WidthPercentage = 100;

                    // Encabezados
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, standardFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    // Datos
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            table.AddCell(new Phrase(item.ToString(), standardFont));
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                    pw.Close();
                }

                MessageBox.Show("Reporte de Entradas de Mercancía generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerarReporteSalidasMercancia()
        {
            string query = @"
        SELECT 
            s.salida_id AS 'ID Salida', 
            s.fecha_salida AS 'Fecha de Salida', 
            u.nombre AS 'Usuario', 
            d.producto_id AS 'ID Producto', 
            p.nombre AS 'Producto', 
            d.cantidad AS 'Cantidad', 
            d.precio_unitario AS 'Precio Unitario'
        FROM 
            salidas s
        JOIN 
            usuarios u ON s.usuario_id = u.usuario_id
        JOIN 
            detalle_salidas d ON s.salida_id = d.salida_id
        JOIN 
            productos p ON d.producto_id = p.producto_id
        WHERE 
            s.fecha_salida >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH)";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"))
            {
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Crear la ruta del archivo PDF en la carpeta "reportes/operativos" dentro de la carpeta del ejecutable
                string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes", "operativos");
                Directory.CreateDirectory(reportesPath); // Crear la carpeta si no existe

                // Generar un nombre de archivo único con marca de tiempo
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Path.Combine(reportesPath, $"ReporteSalidasMercancia_{timestamp}.pdf");

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Título del PDF
                    doc.AddAuthor("Inventario Proyecto");
                    doc.AddTitle("Reporte de Salidas de Mercancía");

                    // Definimos fuente
                    iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagen", "1000036045.jpg");
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen

                    // Agregar la imagen al documento
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título
                    Paragraph title = new Paragraph("Reporte de Salidas de Mercancía", standardFont);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento
                    doc.Add(title);

                    // Agregar un salto de línea
                    doc.Add(Chunk.NEWLINE);

                    // Tabla
                    PdfPTable table = new PdfPTable(dt.Columns.Count);
                    table.WidthPercentage = 100;

                    // Encabezados
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, standardFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    // Datos
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            table.AddCell(new Phrase(item.ToString(), standardFont));
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                    pw.Close();
                }

                MessageBox.Show("Reporte de Salidas de Mercancía generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerarReporteBajoStock()
        {
            string query = @"
        SELECT 
            p.producto_id AS 'ID Producto', 
            p.nombre AS 'Nombre', 
            p.descripcion AS 'Descripción', 
            c.nombre AS 'Categoría', 
            p.unidad_base AS 'Unidad Base', 
            i.stock_actual AS 'Stock Actual', 
            p.stock_minimo AS 'Stock Mínimo'
        FROM 
            productos p
        LEFT JOIN 
            inventario i ON p.producto_id = i.producto_id
        JOIN 
            categorias c ON p.categoria_id = c.categoria_id
        WHERE 
            p.activo = 1 AND i.stock_actual <= p.stock_minimo";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"))
            {
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Crear la ruta del archivo PDF en la carpeta "reportes/decisiones" dentro de la carpeta del ejecutable
                string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes", "decisiones");
                Directory.CreateDirectory(reportesPath); // Crear la carpeta si no existe

                // Generar un nombre de archivo único con marca de tiempo
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Path.Combine(reportesPath, $"ReporteBajoStock_{timestamp}.pdf");

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Título del PDF
                    doc.AddAuthor("Inventario Proyecto");
                    doc.AddTitle("Reporte de Productos con Bajo Stock");

                    // Definimos fuente
                    iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagen", "1000036045.jpg");
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen

                    // Agregar la imagen al documento
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título
                    Paragraph title = new Paragraph("Reporte de Productos con Bajo Stock", standardFont);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento
                    doc.Add(title);

                    // Agregar un salto de línea
                    doc.Add(Chunk.NEWLINE);

                    // Tabla
                    PdfPTable table = new PdfPTable(dt.Columns.Count);
                    table.WidthPercentage = 100;

                    // Encabezados
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, standardFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    // Datos
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            table.AddCell(new Phrase(item.ToString(), standardFont));
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                    pw.Close();
                }

                MessageBox.Show("Reporte de Productos con Bajo Stock generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerarReporteComprasProveedor()
        {
            string query = @"
        SELECT 
            p.proveedor_id AS 'ID Proveedor', 
            p.nombre AS 'Proveedor', 
            SUM(d.cantidad) AS 'Total Cantidad', 
            SUM(d.cantidad * d.precio_unitario) AS 'Total Compras'
        FROM 
            proveedores p
        JOIN 
            entradas e ON p.proveedor_id = e.proveedor_id
        JOIN 
            detalle_entradas d ON e.entrada_id = d.entrada_id
        GROUP BY 
            p.proveedor_id, p.nombre";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"))
            {
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Crear la ruta del archivo PDF en la carpeta "reportes/decisiones" dentro de la carpeta del ejecutable
                string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes", "decisiones");
                Directory.CreateDirectory(reportesPath); // Crear la carpeta si no existe

                // Generar un nombre de archivo único con marca de tiempo
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Path.Combine(reportesPath, $"ReporteComprasProveedor_{timestamp}.pdf");

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Título del PDF
                    doc.AddAuthor("Inventario Proyecto");
                    doc.AddTitle("Reporte de Compras por Proveedor");

                    // Definimos fuente
                    iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagen", "1000036045.jpg");
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen

                    // Agregar la imagen al documento
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título
                    Paragraph title = new Paragraph("Reporte de Compras por Proveedor", standardFont);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento
                    doc.Add(title);

                    // Agregar un salto de línea
                    doc.Add(Chunk.NEWLINE);

                    // Tabla
                    PdfPTable table = new PdfPTable(dt.Columns.Count);
                    table.WidthPercentage = 100;

                    // Encabezados
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, standardFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    // Datos
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            table.AddCell(new Phrase(item.ToString(), standardFont));
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                    pw.Close();
                }

                MessageBox.Show("Reporte de Compras por Proveedor generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerarReporteProductosSinMovimiento()
        {
            string query = @"
        SELECT 
            p.producto_id AS 'ID Producto', 
            p.nombre AS 'Nombre', 
            p.descripcion AS 'Descripción', 
            c.nombre AS 'Categoría', 
            p.unidad_base AS 'Unidad Base', 
            i.stock_actual AS 'Stock Actual'
        FROM 
            productos p
        LEFT JOIN 
            inventario i ON p.producto_id = i.producto_id
        JOIN 
            categorias c ON p.categoria_id = c.categoria_id
        WHERE 
            p.activo = 1 AND p.producto_id NOT IN (
                SELECT producto_id FROM detalle_entradas WHERE fecha_entrada >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH)
                UNION
                SELECT producto_id FROM detalle_salidas WHERE fecha_salida >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH)
            )";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;"))
            {
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Crear la ruta del archivo PDF en la carpeta "reportes/decisiones" dentro de la carpeta del ejecutable
                string reportesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reportes", "decisiones");
                Directory.CreateDirectory(reportesPath); // Crear la carpeta si no existe

                // Generar un nombre de archivo único con marca de tiempo
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Path.Combine(reportesPath, $"ReporteProductosSinMovimiento_{timestamp}.pdf");

                using (FileStream fs = new FileStream(pdfPath, FileMode.Create))
                {
                    Document doc = new Document(PageSize.LETTER, 3, 3, 3, 3);
                    PdfWriter pw = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Título del PDF
                    doc.AddAuthor("Inventario Proyecto");
                    doc.AddTitle("Reporte de Productos Sin Movimiento");

                    // Definimos fuente
                    iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Cargar la imagen
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagen", "1000036045.jpg");
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
                    img.ScaleToFit(60f, 60f); // Ajustar el tamaño de la imagen

                    // Agregar la imagen al documento
                    img.Alignment = Element.ALIGN_CENTER;
                    doc.Add(img);

                    // Se crea el título
                    Paragraph title = new Paragraph("Reporte de Productos Sin Movimiento", standardFont);
                    title.Alignment = Element.ALIGN_CENTER;

                    // Agregar el título al documento
                    doc.Add(title);

                    // Agregar un salto de línea
                    doc.Add(Chunk.NEWLINE);

                    // Tabla
                    PdfPTable table = new PdfPTable(dt.Columns.Count);
                    table.WidthPercentage = 100;

                    // Encabezados
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, standardFont));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    // Datos
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            table.AddCell(new Phrase(item.ToString(), standardFont));
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                    pw.Close();
                }

                MessageBox.Show("Reporte de Productos Sin Movimiento generado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }






        private void FormularioSupervision_Load(object sender, EventArgs e)
        {

        }


        private void FormularioSupervision_Load_1(object sender, EventArgs e)
        {

        }
    }
}
