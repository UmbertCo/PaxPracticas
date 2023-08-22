namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmGenerarLayouts
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
            this.btnGenerar = new System.Windows.Forms.Button();
            this.txtLayout = new System.Windows.Forms.TextBox();
            this.btnArchivo = new System.Windows.Forms.Button();
            this.btnRutaArchivos = new System.Windows.Forms.Button();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.btnGenerarRuta = new System.Windows.Forms.Button();
            this.txtNumeroLayouts = new System.Windows.Forms.TextBox();
            this.lblNumeroLayouts = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(302, 126);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(75, 23);
            this.btnGenerar.TabIndex = 0;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // txtLayout
            // 
            this.txtLayout.Location = new System.Drawing.Point(26, 100);
            this.txtLayout.Name = "txtLayout";
            this.txtLayout.Size = new System.Drawing.Size(351, 20);
            this.txtLayout.TabIndex = 1;
            // 
            // btnArchivo
            // 
            this.btnArchivo.Location = new System.Drawing.Point(383, 98);
            this.btnArchivo.Name = "btnArchivo";
            this.btnArchivo.Size = new System.Drawing.Size(29, 23);
            this.btnArchivo.TabIndex = 2;
            this.btnArchivo.Text = "...";
            this.btnArchivo.UseVisualStyleBackColor = true;
            this.btnArchivo.Click += new System.EventHandler(this.btnArchivo_Click);
            // 
            // btnRutaArchivos
            // 
            this.btnRutaArchivos.Location = new System.Drawing.Point(383, 190);
            this.btnRutaArchivos.Name = "btnRutaArchivos";
            this.btnRutaArchivos.Size = new System.Drawing.Size(29, 23);
            this.btnRutaArchivos.TabIndex = 4;
            this.btnRutaArchivos.Text = "...";
            this.btnRutaArchivos.UseVisualStyleBackColor = true;
            this.btnRutaArchivos.Click += new System.EventHandler(this.btnRutaArchivos_Click);
            // 
            // txtRuta
            // 
            this.txtRuta.Location = new System.Drawing.Point(26, 192);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.Size = new System.Drawing.Size(351, 20);
            this.txtRuta.TabIndex = 3;
            // 
            // btnGenerarRuta
            // 
            this.btnGenerarRuta.Location = new System.Drawing.Point(279, 218);
            this.btnGenerarRuta.Name = "btnGenerarRuta";
            this.btnGenerarRuta.Size = new System.Drawing.Size(98, 23);
            this.btnGenerarRuta.TabIndex = 5;
            this.btnGenerarRuta.Text = "Generar con ruta";
            this.btnGenerarRuta.UseVisualStyleBackColor = true;
            this.btnGenerarRuta.Click += new System.EventHandler(this.btnGenerarRuta_Click);
            // 
            // txtNumeroLayouts
            // 
            this.txtNumeroLayouts.Location = new System.Drawing.Point(26, 48);
            this.txtNumeroLayouts.Name = "txtNumeroLayouts";
            this.txtNumeroLayouts.Size = new System.Drawing.Size(184, 20);
            this.txtNumeroLayouts.TabIndex = 6;
            // 
            // lblNumeroLayouts
            // 
            this.lblNumeroLayouts.AutoSize = true;
            this.lblNumeroLayouts.Location = new System.Drawing.Point(23, 22);
            this.lblNumeroLayouts.Name = "lblNumeroLayouts";
            this.lblNumeroLayouts.Size = new System.Drawing.Size(149, 13);
            this.lblNumeroLayouts.TabIndex = 7;
            this.lblNumeroLayouts.Text = "Número de Layouts a Generar";
            // 
            // frmGenerarLayouts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 295);
            this.Controls.Add(this.lblNumeroLayouts);
            this.Controls.Add(this.txtNumeroLayouts);
            this.Controls.Add(this.btnGenerarRuta);
            this.Controls.Add(this.btnRutaArchivos);
            this.Controls.Add(this.txtRuta);
            this.Controls.Add(this.btnArchivo);
            this.Controls.Add(this.txtLayout);
            this.Controls.Add(this.btnGenerar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmGenerarLayouts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generar Layouts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.TextBox txtLayout;
        private System.Windows.Forms.Button btnArchivo;
        private System.Windows.Forms.Button btnRutaArchivos;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.Button btnGenerarRuta;
        private System.Windows.Forms.TextBox txtNumeroLayouts;
        private System.Windows.Forms.Label lblNumeroLayouts;
    }
}