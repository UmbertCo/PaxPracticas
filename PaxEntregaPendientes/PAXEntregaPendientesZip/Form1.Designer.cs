namespace PAXEntregaPendientesZip
{
    partial class frmEntregaPendientes
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
            this.rbPendiente = new System.Windows.Forms.RadioButton();
            this.rbActivo = new System.Windows.Forms.RadioButton();
            this.btnSeleccionarZip = new System.Windows.Forms.Button();
            this.lbZips = new System.Windows.Forms.ListBox();
            this.lbComprobantes = new System.Windows.Forms.ListBox();
            this.btnSeleccionarComprobantes = new System.Windows.Forms.Button();
            this.dgvComprobantes = new System.Windows.Forms.DataGridView();
            this.btnPrueba = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.btnSeleccionarOrigen = new System.Windows.Forms.Button();
            this.txtOrigen = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.btnEntregarArchivos = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComprobantes)).BeginInit();
            this.SuspendLayout();
            // 
            // rbPendiente
            // 
            this.rbPendiente.AutoSize = true;
            this.rbPendiente.Location = new System.Drawing.Point(122, 159);
            this.rbPendiente.Name = "rbPendiente";
            this.rbPendiente.Size = new System.Drawing.Size(73, 17);
            this.rbPendiente.TabIndex = 40;
            this.rbPendiente.TabStop = true;
            this.rbPendiente.Text = "Pendiente";
            this.rbPendiente.UseVisualStyleBackColor = true;
            // 
            // rbActivo
            // 
            this.rbActivo.AutoSize = true;
            this.rbActivo.Location = new System.Drawing.Point(15, 161);
            this.rbActivo.Name = "rbActivo";
            this.rbActivo.Size = new System.Drawing.Size(55, 17);
            this.rbActivo.TabIndex = 39;
            this.rbActivo.TabStop = true;
            this.rbActivo.Text = "Activo";
            this.rbActivo.UseVisualStyleBackColor = true;
            // 
            // btnSeleccionarZip
            // 
            this.btnSeleccionarZip.Location = new System.Drawing.Point(785, 154);
            this.btnSeleccionarZip.Name = "btnSeleccionarZip";
            this.btnSeleccionarZip.Size = new System.Drawing.Size(122, 23);
            this.btnSeleccionarZip.TabIndex = 38;
            this.btnSeleccionarZip.Text = "Seleccionar zip ...";
            this.btnSeleccionarZip.UseVisualStyleBackColor = true;
            this.btnSeleccionarZip.Click += new System.EventHandler(this.btnSeleccionarZip_Click);
            // 
            // lbZips
            // 
            this.lbZips.FormattingEnabled = true;
            this.lbZips.Location = new System.Drawing.Point(467, 54);
            this.lbZips.Name = "lbZips";
            this.lbZips.Size = new System.Drawing.Size(440, 95);
            this.lbZips.TabIndex = 37;
            // 
            // lbComprobantes
            // 
            this.lbComprobantes.FormattingEnabled = true;
            this.lbComprobantes.Location = new System.Drawing.Point(15, 54);
            this.lbComprobantes.Name = "lbComprobantes";
            this.lbComprobantes.Size = new System.Drawing.Size(440, 95);
            this.lbComprobantes.TabIndex = 36;
            // 
            // btnSeleccionarComprobantes
            // 
            this.btnSeleccionarComprobantes.Location = new System.Drawing.Point(333, 154);
            this.btnSeleccionarComprobantes.Name = "btnSeleccionarComprobantes";
            this.btnSeleccionarComprobantes.Size = new System.Drawing.Size(122, 23);
            this.btnSeleccionarComprobantes.TabIndex = 35;
            this.btnSeleccionarComprobantes.Text = "Seleccionar xml ...";
            this.btnSeleccionarComprobantes.UseVisualStyleBackColor = true;
            this.btnSeleccionarComprobantes.Click += new System.EventHandler(this.btnSeleccionarComprobantes_Click);
            // 
            // dgvComprobantes
            // 
            this.dgvComprobantes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComprobantes.Location = new System.Drawing.Point(12, 286);
            this.dgvComprobantes.Name = "dgvComprobantes";
            this.dgvComprobantes.Size = new System.Drawing.Size(895, 322);
            this.dgvComprobantes.TabIndex = 34;
            // 
            // btnPrueba
            // 
            this.btnPrueba.Location = new System.Drawing.Point(12, 184);
            this.btnPrueba.Name = "btnPrueba";
            this.btnPrueba.Size = new System.Drawing.Size(167, 27);
            this.btnPrueba.TabIndex = 33;
            this.btnPrueba.Text = "Prueba Entrega";
            this.btnPrueba.UseVisualStyleBackColor = true;
            this.btnPrueba.Click += new System.EventHandler(this.btnPrueba_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(12, 218);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(895, 62);
            this.txtResultado.TabIndex = 32;
            // 
            // lblOrigen
            // 
            this.lblOrigen.AutoSize = true;
            this.lblOrigen.Location = new System.Drawing.Point(119, 15);
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.Size = new System.Drawing.Size(41, 13);
            this.lblOrigen.TabIndex = 31;
            this.lblOrigen.Text = "Origen:";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(12, 12);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 30;
            this.lblUsuario.Text = "Usuario:";
            // 
            // btnSeleccionarOrigen
            // 
            this.btnSeleccionarOrigen.Location = new System.Drawing.Point(550, 26);
            this.btnSeleccionarOrigen.Name = "btnSeleccionarOrigen";
            this.btnSeleccionarOrigen.Size = new System.Drawing.Size(111, 23);
            this.btnSeleccionarOrigen.TabIndex = 29;
            this.btnSeleccionarOrigen.Text = "Seleccionar Origen ";
            this.btnSeleccionarOrigen.UseVisualStyleBackColor = true;
            this.btnSeleccionarOrigen.Click += new System.EventHandler(this.btnSeleccionarOrigen_Click);
            // 
            // txtOrigen
            // 
            this.txtOrigen.Location = new System.Drawing.Point(122, 30);
            this.txtOrigen.Name = "txtOrigen";
            this.txtOrigen.Size = new System.Drawing.Size(422, 20);
            this.txtOrigen.TabIndex = 28;
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(15, 28);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(100, 20);
            this.txtUsuario.TabIndex = 27;
            this.txtUsuario.Text = "solser_2014";
            // 
            // btnEntregarArchivos
            // 
            this.btnEntregarArchivos.Location = new System.Drawing.Point(185, 183);
            this.btnEntregarArchivos.Name = "btnEntregarArchivos";
            this.btnEntregarArchivos.Size = new System.Drawing.Size(249, 28);
            this.btnEntregarArchivos.TabIndex = 26;
            this.btnEntregarArchivos.Text = "Entrega Archivos";
            this.btnEntregarArchivos.UseVisualStyleBackColor = true;
            this.btnEntregarArchivos.Click += new System.EventHandler(this.btnEntregarArchivos_Click);
            // 
            // frmEntregaPendientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 618);
            this.Controls.Add(this.rbPendiente);
            this.Controls.Add(this.rbActivo);
            this.Controls.Add(this.btnSeleccionarZip);
            this.Controls.Add(this.lbZips);
            this.Controls.Add(this.lbComprobantes);
            this.Controls.Add(this.btnSeleccionarComprobantes);
            this.Controls.Add(this.dgvComprobantes);
            this.Controls.Add(this.btnPrueba);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.lblOrigen);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.btnSeleccionarOrigen);
            this.Controls.Add(this.txtOrigen);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.btnEntregarArchivos);
            this.Name = "frmEntregaPendientes";
            this.Text = "Entrega Pendientes";
            this.Load += new System.EventHandler(this.frmEntregaPendientes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComprobantes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbPendiente;
        private System.Windows.Forms.RadioButton rbActivo;
        private System.Windows.Forms.Button btnSeleccionarZip;
        private System.Windows.Forms.ListBox lbZips;
        private System.Windows.Forms.ListBox lbComprobantes;
        private System.Windows.Forms.Button btnSeleccionarComprobantes;
        private System.Windows.Forms.DataGridView dgvComprobantes;
        private System.Windows.Forms.Button btnPrueba;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Label lblOrigen;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Button btnSeleccionarOrigen;
        private System.Windows.Forms.TextBox txtOrigen;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Button btnEntregarArchivos;
    }
}

