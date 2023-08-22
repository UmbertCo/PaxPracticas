namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmComprobantesActualizados
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
            this.ofdArchivo = new System.Windows.Forms.OpenFileDialog();
            this.btnArchivo = new System.Windows.Forms.Button();
            this.txtArchivo = new System.Windows.Forms.TextBox();
            this.btnRevisar = new System.Windows.Forms.Button();
            this.dgvDocumento = new System.Windows.Forms.DataGridView();
            this.dgvEntradas = new System.Windows.Forms.DataGridView();
            this.txtFechaCreacionFinal = new System.Windows.Forms.TextBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.txtFechaCreacionInicio = new System.Windows.Forms.TextBox();
            this.lblFechaCreacionInicio = new System.Windows.Forms.Label();
            this.lblFechaCreacionFinal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocumento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntradas)).BeginInit();
            this.SuspendLayout();
            // 
            // ofdArchivo
            // 
            this.ofdArchivo.FileName = "ofdArchivo";
            // 
            // btnArchivo
            // 
            this.btnArchivo.Location = new System.Drawing.Point(342, 15);
            this.btnArchivo.Name = "btnArchivo";
            this.btnArchivo.Size = new System.Drawing.Size(27, 23);
            this.btnArchivo.TabIndex = 0;
            this.btnArchivo.Text = "...";
            this.btnArchivo.UseVisualStyleBackColor = true;
            this.btnArchivo.Click += new System.EventHandler(this.btnArchivo_Click);
            // 
            // txtArchivo
            // 
            this.txtArchivo.Location = new System.Drawing.Point(28, 17);
            this.txtArchivo.Name = "txtArchivo";
            this.txtArchivo.Size = new System.Drawing.Size(308, 20);
            this.txtArchivo.TabIndex = 1;
            // 
            // btnRevisar
            // 
            this.btnRevisar.Location = new System.Drawing.Point(377, 15);
            this.btnRevisar.Name = "btnRevisar";
            this.btnRevisar.Size = new System.Drawing.Size(75, 23);
            this.btnRevisar.TabIndex = 2;
            this.btnRevisar.Text = "Revisar";
            this.btnRevisar.UseVisualStyleBackColor = true;
            this.btnRevisar.Click += new System.EventHandler(this.btnRevisar_Click);
            // 
            // dgvDocumento
            // 
            this.dgvDocumento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDocumento.Location = new System.Drawing.Point(28, 187);
            this.dgvDocumento.Name = "dgvDocumento";
            this.dgvDocumento.ReadOnly = true;
            this.dgvDocumento.Size = new System.Drawing.Size(913, 150);
            this.dgvDocumento.TabIndex = 3;
            this.dgvDocumento.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvDocumento_RowPostPaint);
            // 
            // dgvEntradas
            // 
            this.dgvEntradas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEntradas.Location = new System.Drawing.Point(28, 43);
            this.dgvEntradas.Name = "dgvEntradas";
            this.dgvEntradas.ReadOnly = true;
            this.dgvEntradas.Size = new System.Drawing.Size(333, 128);
            this.dgvEntradas.TabIndex = 4;
            this.dgvEntradas.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvEntradas_RowPostPaint);
            // 
            // txtFechaCreacionFinal
            // 
            this.txtFechaCreacionFinal.Location = new System.Drawing.Point(488, 82);
            this.txtFechaCreacionFinal.Name = "txtFechaCreacionFinal";
            this.txtFechaCreacionFinal.Size = new System.Drawing.Size(100, 20);
            this.txtFechaCreacionFinal.TabIndex = 5;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Location = new System.Drawing.Point(377, 147);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(75, 23);
            this.btnFiltrar.TabIndex = 6;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // txtFechaCreacionInicio
            // 
            this.txtFechaCreacionInicio.Location = new System.Drawing.Point(488, 56);
            this.txtFechaCreacionInicio.Name = "txtFechaCreacionInicio";
            this.txtFechaCreacionInicio.Size = new System.Drawing.Size(100, 20);
            this.txtFechaCreacionInicio.TabIndex = 7;
            // 
            // lblFechaCreacionInicio
            // 
            this.lblFechaCreacionInicio.AutoSize = true;
            this.lblFechaCreacionInicio.Location = new System.Drawing.Point(377, 62);
            this.lblFechaCreacionInicio.Name = "lblFechaCreacionInicio";
            this.lblFechaCreacionInicio.Size = new System.Drawing.Size(110, 13);
            this.lblFechaCreacionInicio.TabIndex = 8;
            this.lblFechaCreacionInicio.Text = "Inicio Fecha Creación";
            // 
            // lblFechaCreacionFinal
            // 
            this.lblFechaCreacionFinal.AutoSize = true;
            this.lblFechaCreacionFinal.Location = new System.Drawing.Point(380, 88);
            this.lblFechaCreacionFinal.Name = "lblFechaCreacionFinal";
            this.lblFechaCreacionFinal.Size = new System.Drawing.Size(107, 13);
            this.lblFechaCreacionFinal.TabIndex = 9;
            this.lblFechaCreacionFinal.Text = "Final Fecha Creacion";
            // 
            // frmComprobantesActualizados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 366);
            this.Controls.Add(this.lblFechaCreacionFinal);
            this.Controls.Add(this.lblFechaCreacionInicio);
            this.Controls.Add(this.txtFechaCreacionInicio);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.txtFechaCreacionFinal);
            this.Controls.Add(this.dgvEntradas);
            this.Controls.Add(this.dgvDocumento);
            this.Controls.Add(this.btnRevisar);
            this.Controls.Add(this.txtArchivo);
            this.Controls.Add(this.btnArchivo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmComprobantesActualizados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Comprobantes Actualizados";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocumento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntradas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdArchivo;
        private System.Windows.Forms.Button btnArchivo;
        private System.Windows.Forms.TextBox txtArchivo;
        private System.Windows.Forms.Button btnRevisar;
        private System.Windows.Forms.DataGridView dgvDocumento;
        private System.Windows.Forms.DataGridView dgvEntradas;
        private System.Windows.Forms.TextBox txtFechaCreacionFinal;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.TextBox txtFechaCreacionInicio;
        private System.Windows.Forms.Label lblFechaCreacionInicio;
        private System.Windows.Forms.Label lblFechaCreacionFinal;
    }
}