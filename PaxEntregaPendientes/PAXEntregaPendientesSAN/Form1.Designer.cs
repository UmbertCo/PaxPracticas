namespace PAXEntregaPendientesSAN
{
    partial class frmEntregaPendientesSAN
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
            this.lbComprobantes = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSeleccionarComprobantes = new System.Windows.Forms.Button();
            this.btnPrueba = new System.Windows.Forms.Button();
            this.btnEntregarArchivos = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.dgvComprobantes = new System.Windows.Forms.DataGridView();
            this.cbOrigen = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComprobantes)).BeginInit();
            this.SuspendLayout();
            // 
            // lbComprobantes
            // 
            this.lbComprobantes.FormattingEnabled = true;
            this.lbComprobantes.Location = new System.Drawing.Point(12, 27);
            this.lbComprobantes.Name = "lbComprobantes";
            this.lbComprobantes.Size = new System.Drawing.Size(440, 95);
            this.lbComprobantes.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Selecciona CFDI\'s:";
            // 
            // btnSeleccionarComprobantes
            // 
            this.btnSeleccionarComprobantes.Location = new System.Drawing.Point(330, 3);
            this.btnSeleccionarComprobantes.Name = "btnSeleccionarComprobantes";
            this.btnSeleccionarComprobantes.Size = new System.Drawing.Size(122, 23);
            this.btnSeleccionarComprobantes.TabIndex = 24;
            this.btnSeleccionarComprobantes.Text = "Seleccionar xml ...";
            this.btnSeleccionarComprobantes.UseVisualStyleBackColor = true;
            this.btnSeleccionarComprobantes.Click += new System.EventHandler(this.btnSeleccionarComprobantes_Click);
            // 
            // btnPrueba
            // 
            this.btnPrueba.Location = new System.Drawing.Point(165, 127);
            this.btnPrueba.Name = "btnPrueba";
            this.btnPrueba.Size = new System.Drawing.Size(132, 27);
            this.btnPrueba.TabIndex = 26;
            this.btnPrueba.Text = "Prueba Entrega";
            this.btnPrueba.UseVisualStyleBackColor = true;
            this.btnPrueba.Click += new System.EventHandler(this.btnPrueba_Click);
            // 
            // btnEntregarArchivos
            // 
            this.btnEntregarArchivos.Location = new System.Drawing.Point(303, 127);
            this.btnEntregarArchivos.Name = "btnEntregarArchivos";
            this.btnEntregarArchivos.Size = new System.Drawing.Size(143, 28);
            this.btnEntregarArchivos.TabIndex = 25;
            this.btnEntregarArchivos.Text = "Entrega Archivos";
            this.btnEntregarArchivos.UseVisualStyleBackColor = true;
            this.btnEntregarArchivos.Click += new System.EventHandler(this.btnEntregarArchivos_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(12, 161);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(440, 62);
            this.txtResultado.TabIndex = 27;
            // 
            // dgvComprobantes
            // 
            this.dgvComprobantes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComprobantes.Location = new System.Drawing.Point(12, 229);
            this.dgvComprobantes.Name = "dgvComprobantes";
            this.dgvComprobantes.Size = new System.Drawing.Size(440, 237);
            this.dgvComprobantes.TabIndex = 28;
            // 
            // cbOrigen
            // 
            this.cbOrigen.FormattingEnabled = true;
            this.cbOrigen.Items.AddRange(new object[] {
            "R",
            "C"});
            this.cbOrigen.Location = new System.Drawing.Point(12, 131);
            this.cbOrigen.Name = "cbOrigen";
            this.cbOrigen.Size = new System.Drawing.Size(121, 21);
            this.cbOrigen.TabIndex = 29;
            // 
            // frmEntregaPendientesSAN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 478);
            this.Controls.Add(this.cbOrigen);
            this.Controls.Add(this.dgvComprobantes);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btnPrueba);
            this.Controls.Add(this.btnEntregarArchivos);
            this.Controls.Add(this.btnSeleccionarComprobantes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbComprobantes);
            this.Name = "frmEntregaPendientesSAN";
            this.Text = "Entrega Pendientes SAN";
            this.Load += new System.EventHandler(this.frmEntregaPendientesSAN_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComprobantes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbComprobantes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSeleccionarComprobantes;
        private System.Windows.Forms.Button btnPrueba;
        private System.Windows.Forms.Button btnEntregarArchivos;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.DataGridView dgvComprobantes;
        private System.Windows.Forms.ComboBox cbOrigen;
    }
}

