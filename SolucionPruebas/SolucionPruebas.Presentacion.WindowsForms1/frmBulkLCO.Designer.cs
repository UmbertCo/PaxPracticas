namespace SolucionPruebas.Presentacion.WindowsForms1
{
    partial class frmBulkLCO
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
            this.btnBulk = new System.Windows.Forms.Button();
            this.bwLCO = new System.ComponentModel.BackgroundWorker();
            this.lblProgresoValor = new System.Windows.Forms.Label();
            this.pbBulk = new System.Windows.Forms.ProgressBar();
            this.lblProgreso = new System.Windows.Forms.Label();
            this.lblInicio = new System.Windows.Forms.Label();
            this.lblInicioValor = new System.Windows.Forms.Label();
            this.btnCancelarBulk = new System.Windows.Forms.Button();
            this.lblArchivo = new System.Windows.Forms.Label();
            this.lblArchivoValor = new System.Windows.Forms.Label();
            this.lblEstatus = new System.Windows.Forms.Label();
            this.lblEstatusValor = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnBulk
            // 
            this.btnBulk.Location = new System.Drawing.Point(9, 32);
            this.btnBulk.Name = "btnBulk";
            this.btnBulk.Size = new System.Drawing.Size(106, 23);
            this.btnBulk.TabIndex = 1;
            this.btnBulk.Text = "Empezar Bulk";
            this.btnBulk.UseVisualStyleBackColor = true;
            this.btnBulk.Click += new System.EventHandler(this.btnBulk_Click);
            // 
            // lblProgresoValor
            // 
            this.lblProgresoValor.AutoSize = true;
            this.lblProgresoValor.Location = new System.Drawing.Point(62, 160);
            this.lblProgresoValor.Name = "lblProgresoValor";
            this.lblProgresoValor.Size = new System.Drawing.Size(83, 13);
            this.lblProgresoValor.TabIndex = 2;
            this.lblProgresoValor.Text = "lblProgresoValor";
            // 
            // pbBulk
            // 
            this.pbBulk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbBulk.Location = new System.Drawing.Point(0, 204);
            this.pbBulk.Name = "pbBulk";
            this.pbBulk.Size = new System.Drawing.Size(284, 36);
            this.pbBulk.Step = 1;
            this.pbBulk.TabIndex = 3;
            // 
            // lblProgreso
            // 
            this.lblProgreso.AutoSize = true;
            this.lblProgreso.Location = new System.Drawing.Point(4, 160);
            this.lblProgreso.Name = "lblProgreso";
            this.lblProgreso.Size = new System.Drawing.Size(52, 13);
            this.lblProgreso.TabIndex = 4;
            this.lblProgreso.Text = "Progreso:";
            // 
            // lblInicio
            // 
            this.lblInicio.AutoSize = true;
            this.lblInicio.Location = new System.Drawing.Point(6, 133);
            this.lblInicio.Name = "lblInicio";
            this.lblInicio.Size = new System.Drawing.Size(104, 13);
            this.lblInicio.TabIndex = 5;
            this.lblInicio.Text = "Número de registros:";
            // 
            // lblInicioValor
            // 
            this.lblInicioValor.AutoSize = true;
            this.lblInicioValor.Location = new System.Drawing.Point(116, 133);
            this.lblInicioValor.Name = "lblInicioValor";
            this.lblInicioValor.Size = new System.Drawing.Size(66, 13);
            this.lblInicioValor.TabIndex = 6;
            this.lblInicioValor.Text = "lblInicioValor";
            // 
            // btnCancelarBulk
            // 
            this.btnCancelarBulk.Enabled = false;
            this.btnCancelarBulk.Location = new System.Drawing.Point(132, 32);
            this.btnCancelarBulk.Name = "btnCancelarBulk";
            this.btnCancelarBulk.Size = new System.Drawing.Size(96, 23);
            this.btnCancelarBulk.TabIndex = 7;
            this.btnCancelarBulk.Text = "Cancelar Bulk";
            this.btnCancelarBulk.UseVisualStyleBackColor = true;
            this.btnCancelarBulk.Click += new System.EventHandler(this.btnCancelarBulk_Click);
            // 
            // lblArchivo
            // 
            this.lblArchivo.AutoSize = true;
            this.lblArchivo.Location = new System.Drawing.Point(7, 82);
            this.lblArchivo.Name = "lblArchivo";
            this.lblArchivo.Size = new System.Drawing.Size(46, 13);
            this.lblArchivo.TabIndex = 8;
            this.lblArchivo.Text = "Archivo:";
            // 
            // lblArchivoValor
            // 
            this.lblArchivoValor.AutoSize = true;
            this.lblArchivoValor.Location = new System.Drawing.Point(62, 82);
            this.lblArchivoValor.Name = "lblArchivoValor";
            this.lblArchivoValor.Size = new System.Drawing.Size(77, 13);
            this.lblArchivoValor.TabIndex = 9;
            this.lblArchivoValor.Text = "lblArchivoValor";
            // 
            // lblEstatus
            // 
            this.lblEstatus.AutoSize = true;
            this.lblEstatus.Location = new System.Drawing.Point(7, 109);
            this.lblEstatus.Name = "lblEstatus";
            this.lblEstatus.Size = new System.Drawing.Size(45, 13);
            this.lblEstatus.TabIndex = 10;
            this.lblEstatus.Text = "Estatus:";
            // 
            // lblEstatusValor
            // 
            this.lblEstatusValor.AutoSize = true;
            this.lblEstatusValor.Location = new System.Drawing.Point(62, 109);
            this.lblEstatusValor.Name = "lblEstatusValor";
            this.lblEstatusValor.Size = new System.Drawing.Size(76, 13);
            this.lblEstatusValor.TabIndex = 11;
            this.lblEstatusValor.Text = "lblEstatusValor";
            // 
            // frmBulkLCO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 240);
            this.Controls.Add(this.lblEstatusValor);
            this.Controls.Add(this.lblEstatus);
            this.Controls.Add(this.lblArchivoValor);
            this.Controls.Add(this.lblArchivo);
            this.Controls.Add(this.btnCancelarBulk);
            this.Controls.Add(this.lblInicioValor);
            this.Controls.Add(this.lblInicio);
            this.Controls.Add(this.lblProgreso);
            this.Controls.Add(this.pbBulk);
            this.Controls.Add(this.lblProgresoValor);
            this.Controls.Add(this.btnBulk);
            this.Name = "frmBulkLCO";
            this.Text = "Bulk LCO";
            this.Load += new System.EventHandler(this.frmBulkLCO_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBulk;
        private System.ComponentModel.BackgroundWorker bwLCO;
        private System.Windows.Forms.Label lblProgresoValor;
        private System.Windows.Forms.ProgressBar pbBulk;
        private System.Windows.Forms.Label lblProgreso;
        private System.Windows.Forms.Label lblInicio;
        private System.Windows.Forms.Label lblInicioValor;
        private System.Windows.Forms.Button btnCancelarBulk;
        private System.Windows.Forms.Label lblArchivo;
        private System.Windows.Forms.Label lblArchivoValor;
        private System.Windows.Forms.Label lblEstatus;
        private System.Windows.Forms.Label lblEstatusValor;
    }
}