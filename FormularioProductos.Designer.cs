namespace inventario_proyecto
{
    partial class FormularioProductos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormularioProductos));
            this.btnPDF = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.btnGenerarpdf = new System.Windows.Forms.Button();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBoxCerrar = new System.Windows.Forms.PictureBox();
            this.AggCate = new System.Windows.Forms.Button();
            this.panelServicio = new System.Windows.Forms.Panel();
            this.AggIngreso = new System.Windows.Forms.Button();
            this.AggProv = new System.Windows.Forms.Button();
            this.AggUsuarios = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.panelMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCerrar)).BeginInit();
            this.panelServicio.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPDF
            // 
            this.btnPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPDF.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnPDF.FlatAppearance.BorderSize = 0;
            this.btnPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPDF.Location = new System.Drawing.Point(800, 388);
            this.btnPDF.Name = "btnPDF";
            this.btnPDF.Size = new System.Drawing.Size(140, 53);
            this.btnPDF.TabIndex = 4;
            this.btnPDF.Text = "PDF";
            this.btnPDF.UseVisualStyleBackColor = false;
            // 
            // btnAgregar
            // 
            this.btnAgregar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgregar.BackColor = System.Drawing.Color.Transparent;
            this.btnAgregar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgregar.FlatAppearance.BorderSize = 0;
            this.btnAgregar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.ForeColor = System.Drawing.Color.White;
            this.btnAgregar.Location = new System.Drawing.Point(0, 5);
            this.btnAgregar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(176, 32);
            this.btnAgregar.TabIndex = 1;
            this.btnAgregar.Text = "Productos";
            this.btnAgregar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditar.BackColor = System.Drawing.Color.Transparent;
            this.btnEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.ForeColor = System.Drawing.Color.White;
            this.btnEditar.Location = new System.Drawing.Point(-4, 44);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(176, 32);
            this.btnEditar.TabIndex = 2;
            this.btnEditar.Text = "Presentaciones";
            this.btnEditar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // dgvProductos
            // 
            this.dgvProductos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.Location = new System.Drawing.Point(19, 55);
            this.dgvProductos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.Size = new System.Drawing.Size(1235, 393);
            this.dgvProductos.TabIndex = 0;
            this.dgvProductos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductos_CellContentClick);
            // 
            // btnGenerarpdf
            // 
            this.btnGenerarpdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerarpdf.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnGenerarpdf.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerarpdf.FlatAppearance.BorderSize = 0;
            this.btnGenerarpdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarpdf.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarpdf.ForeColor = System.Drawing.Color.White;
            this.btnGenerarpdf.Location = new System.Drawing.Point(1067, 455);
            this.btnGenerarpdf.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGenerarpdf.Name = "btnGenerarpdf";
            this.btnGenerarpdf.Size = new System.Drawing.Size(187, 65);
            this.btnGenerarpdf.TabIndex = 4;
            this.btnGenerarpdf.Text = "PDF";
            this.btnGenerarpdf.UseVisualStyleBackColor = false;
            this.btnGenerarpdf.Click += new System.EventHandler(this.btnGenerarpdf_Click);
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(66)))), ((int)(((byte)(82)))));
            this.panelMenu.Controls.Add(this.button1);
            this.panelMenu.Controls.Add(this.pictureBoxCerrar);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(1269, 48);
            this.panelMenu.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(66)))), ((int)(((byte)(82)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = global::inventario_proyecto.Properties.Resources.icons8_clasificar_abajo_24;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(16, 11);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(187, 32);
            this.button1.TabIndex = 6;
            this.button1.Text = "Servicios";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseEnter += new System.EventHandler(this.btnMouseEnter);
            this.button1.MouseLeave += new System.EventHandler(this.btnMouseLeave);
            // 
            // pictureBoxCerrar
            // 
            this.pictureBoxCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxCerrar.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxCerrar.Image")));
            this.pictureBoxCerrar.Location = new System.Drawing.Point(1228, 4);
            this.pictureBoxCerrar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBoxCerrar.Name = "pictureBoxCerrar";
            this.pictureBoxCerrar.Size = new System.Drawing.Size(25, 34);
            this.pictureBoxCerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCerrar.TabIndex = 5;
            this.pictureBoxCerrar.TabStop = false;
            this.pictureBoxCerrar.Click += new System.EventHandler(this.pictureBoxCerrar_Click);
            // 
            // AggCate
            // 
            this.AggCate.BackColor = System.Drawing.Color.Transparent;
            this.AggCate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AggCate.FlatAppearance.BorderSize = 0;
            this.AggCate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.AggCate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AggCate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AggCate.ForeColor = System.Drawing.Color.White;
            this.AggCate.Location = new System.Drawing.Point(-4, 85);
            this.AggCate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AggCate.Name = "AggCate";
            this.AggCate.Size = new System.Drawing.Size(176, 32);
            this.AggCate.TabIndex = 8;
            this.AggCate.Text = "Categorias";
            this.AggCate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AggCate.UseVisualStyleBackColor = false;
            this.AggCate.Click += new System.EventHandler(this.AggCate_Click);
            // 
            // panelServicio
            // 
            this.panelServicio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(66)))), ((int)(((byte)(82)))));
            this.panelServicio.Controls.Add(this.AggCate);
            this.panelServicio.Controls.Add(this.btnEditar);
            this.panelServicio.Controls.Add(this.btnAgregar);
            this.panelServicio.Location = new System.Drawing.Point(19, 46);
            this.panelServicio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelServicio.Name = "panelServicio";
            this.panelServicio.Size = new System.Drawing.Size(172, 121);
            this.panelServicio.TabIndex = 7;
            this.panelServicio.Visible = false;
            // 
            // AggIngreso
            // 
            this.AggIngreso.BackColor = System.Drawing.Color.DodgerBlue;
            this.AggIngreso.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AggIngreso.FlatAppearance.BorderSize = 0;
            this.AggIngreso.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AggIngreso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AggIngreso.ForeColor = System.Drawing.Color.White;
            this.AggIngreso.Location = new System.Drawing.Point(370, 456);
            this.AggIngreso.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AggIngreso.Name = "AggIngreso";
            this.AggIngreso.Size = new System.Drawing.Size(187, 65);
            this.AggIngreso.TabIndex = 8;
            this.AggIngreso.Text = "Ingresar Mercancia";
            this.AggIngreso.UseVisualStyleBackColor = false;
            this.AggIngreso.Click += new System.EventHandler(this.AggIngreso_Click);
            // 
            // AggProv
            // 
            this.AggProv.BackColor = System.Drawing.Color.DodgerBlue;
            this.AggProv.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AggProv.FlatAppearance.BorderSize = 0;
            this.AggProv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AggProv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AggProv.ForeColor = System.Drawing.Color.White;
            this.AggProv.Location = new System.Drawing.Point(19, 456);
            this.AggProv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AggProv.Name = "AggProv";
            this.AggProv.Size = new System.Drawing.Size(187, 65);
            this.AggProv.TabIndex = 9;
            this.AggProv.Text = "Proveedores";
            this.AggProv.UseVisualStyleBackColor = false;
            this.AggProv.Click += new System.EventHandler(this.AggProv_Click);
            // 
            // AggUsuarios
            // 
            this.AggUsuarios.BackColor = System.Drawing.Color.DodgerBlue;
            this.AggUsuarios.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.AggUsuarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AggUsuarios.FlatAppearance.BorderSize = 0;
            this.AggUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AggUsuarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AggUsuarios.ForeColor = System.Drawing.Color.White;
            this.AggUsuarios.Location = new System.Drawing.Point(730, 456);
            this.AggUsuarios.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AggUsuarios.Name = "AggUsuarios";
            this.AggUsuarios.Size = new System.Drawing.Size(187, 65);
            this.AggUsuarios.TabIndex = 10;
            this.AggUsuarios.Text = "Usuarios";
            this.AggUsuarios.UseVisualStyleBackColor = false;
            this.AggUsuarios.Click += new System.EventHandler(this.AggUsuarios_Click);
            // 
            // FormularioProductos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1269, 571);
            this.Controls.Add(this.AggUsuarios);
            this.Controls.Add(this.AggProv);
            this.Controls.Add(this.AggIngreso);
            this.Controls.Add(this.panelServicio);
            this.Controls.Add(this.btnGenerarpdf);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.dgvProductos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormularioProductos";
            this.Text = "Gestión de Productos";
            this.Load += new System.EventHandler(this.FormularioProductos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            this.panelMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCerrar)).EndInit();
            this.panelServicio.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnPDF;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.DataGridView dgvProductos;
        private System.Windows.Forms.Button btnGenerarpdf;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.PictureBox pictureBoxCerrar;
        private System.Windows.Forms.Button AggCate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelServicio;
        private System.Windows.Forms.Button AggIngreso;
        private System.Windows.Forms.Button AggProv;
        private System.Windows.Forms.Button AggUsuarios;
    }
}