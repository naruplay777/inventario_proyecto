﻿namespace inventario_proyecto
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.formularioP = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(642, 12);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(146, 59);
            this.btnTestConnection.TabIndex = 0;
            this.btnTestConnection.Text = "Probar conexión";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click_1);
            // 
            // formularioP
            // 
            this.formularioP.Location = new System.Drawing.Point(642, 96);
            this.formularioP.Name = "formularioP";
            this.formularioP.Size = new System.Drawing.Size(146, 23);
            this.formularioP.TabIndex = 2;
            this.formularioP.Text = "Formulario Producto";
            this.formularioP.UseVisualStyleBackColor = true;
            this.formularioP.Click += new System.EventHandler(this.formularioP_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(359, 89);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.formularioP);
            this.Controls.Add(this.btnTestConnection);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button formularioP;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

