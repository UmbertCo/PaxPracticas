namespace XtraReportsPractica
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
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
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnMostrarReporte = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMostrarReporte
            // 
            this.btnMostrarReporte.Location = new System.Drawing.Point(89, 60);
            this.btnMostrarReporte.Name = "btnMostrarReporte";
            this.btnMostrarReporte.Size = new System.Drawing.Size(207, 77);
            this.btnMostrarReporte.TabIndex = 0;
            this.btnMostrarReporte.Text = "Mostrar Reporte";
            this.btnMostrarReporte.UseVisualStyleBackColor = true;
            this.btnMostrarReporte.Click += new System.EventHandler(this.btnMostrarReporte_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 304);
            this.Controls.Add(this.btnMostrarReporte);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMostrarReporte;
    }
}

