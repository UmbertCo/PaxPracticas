namespace PaxEntregaPendientes
{
    partial class frmEntregarPendientes
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
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEntregarArchivos = new System.Windows.Forms.Button();
            this.txtOrigen = new System.Windows.Forms.TextBox();
            this.btnSeleccionarOrigen = new System.Windows.Forms.Button();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.btnPrueba = new System.Windows.Forms.Button();
            this.dgvComprobantes = new System.Windows.Forms.DataGridView();
            this.btnSeleccionarComprobantes = new System.Windows.Forms.Button();
            this.lbComprobantes = new System.Windows.Forms.ListBox();
            this.lbZips = new System.Windows.Forms.ListBox();
            this.btnSeleccionarZip = new System.Windows.Forms.Button();
            this.rbActivo = new System.Windows.Forms.RadioButton();
            this.rbPendiente = new System.Windows.Forms.RadioButton();
            this.txtOrganismo = new System.Windows.Forms.TextBox();
            this.lblOrganismo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComprobantes)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(15, 24);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(100, 20);
            this.txtUsuario.TabIndex = 10;
            this.txtUsuario.Text = "solser_2014";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-83, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-83, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Usuario PAX";
            // 
            // btnEntregarArchivos
            // 
            this.btnEntregarArchivos.Location = new System.Drawing.Point(185, 179);
            this.btnEntregarArchivos.Name = "btnEntregarArchivos";
            this.btnEntregarArchivos.Size = new System.Drawing.Size(249, 28);
            this.btnEntregarArchivos.TabIndex = 7;
            this.btnEntregarArchivos.Text = "Entrega Archivos";
            this.btnEntregarArchivos.UseVisualStyleBackColor = true;
            this.btnEntregarArchivos.Click += new System.EventHandler(this.btnEntregarArchivos_Click);
            // 
            // txtOrigen
            // 
            this.txtOrigen.Location = new System.Drawing.Point(122, 26);
            this.txtOrigen.Name = "txtOrigen";
            this.txtOrigen.Size = new System.Drawing.Size(422, 20);
            this.txtOrigen.TabIndex = 12;
            // 
            // btnSeleccionarOrigen
            // 
            this.btnSeleccionarOrigen.Location = new System.Drawing.Point(550, 22);
            this.btnSeleccionarOrigen.Name = "btnSeleccionarOrigen";
            this.btnSeleccionarOrigen.Size = new System.Drawing.Size(111, 23);
            this.btnSeleccionarOrigen.TabIndex = 13;
            this.btnSeleccionarOrigen.Text = "Seleccionar Origen ";
            this.btnSeleccionarOrigen.UseVisualStyleBackColor = true;
            this.btnSeleccionarOrigen.Click += new System.EventHandler(this.btnSeleccionarOrigen_Click);
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(12, 8);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 14;
            this.lblUsuario.Text = "Usuario:";
            // 
            // lblOrigen
            // 
            this.lblOrigen.AutoSize = true;
            this.lblOrigen.Location = new System.Drawing.Point(119, 11);
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.Size = new System.Drawing.Size(41, 13);
            this.lblOrigen.TabIndex = 15;
            this.lblOrigen.Text = "Origen:";
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(12, 214);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(895, 62);
            this.txtResultado.TabIndex = 16;
            // 
            // btnPrueba
            // 
            this.btnPrueba.Location = new System.Drawing.Point(12, 180);
            this.btnPrueba.Name = "btnPrueba";
            this.btnPrueba.Size = new System.Drawing.Size(167, 27);
            this.btnPrueba.TabIndex = 17;
            this.btnPrueba.Text = "Prueba Entrega";
            this.btnPrueba.UseVisualStyleBackColor = true;
            this.btnPrueba.Click += new System.EventHandler(this.btnPrueba_Click);
            // 
            // dgvComprobantes
            // 
            this.dgvComprobantes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComprobantes.Location = new System.Drawing.Point(12, 282);
            this.dgvComprobantes.Name = "dgvComprobantes";
            this.dgvComprobantes.Size = new System.Drawing.Size(895, 322);
            this.dgvComprobantes.TabIndex = 18;
            // 
            // btnSeleccionarComprobantes
            // 
            this.btnSeleccionarComprobantes.Location = new System.Drawing.Point(333, 150);
            this.btnSeleccionarComprobantes.Name = "btnSeleccionarComprobantes";
            this.btnSeleccionarComprobantes.Size = new System.Drawing.Size(122, 23);
            this.btnSeleccionarComprobantes.TabIndex = 20;
            this.btnSeleccionarComprobantes.Text = "Seleccionar xml ...";
            this.btnSeleccionarComprobantes.UseVisualStyleBackColor = true;
            this.btnSeleccionarComprobantes.Click += new System.EventHandler(this.btnSeleccionarComprobantes_Click);
            // 
            // lbComprobantes
            // 
            this.lbComprobantes.FormattingEnabled = true;
            this.lbComprobantes.Location = new System.Drawing.Point(15, 50);
            this.lbComprobantes.Name = "lbComprobantes";
            this.lbComprobantes.Size = new System.Drawing.Size(440, 95);
            this.lbComprobantes.TabIndex = 21;
            // 
            // lbZips
            // 
            this.lbZips.FormattingEnabled = true;
            this.lbZips.Location = new System.Drawing.Point(467, 50);
            this.lbZips.Name = "lbZips";
            this.lbZips.Size = new System.Drawing.Size(440, 95);
            this.lbZips.TabIndex = 22;
            // 
            // btnSeleccionarZip
            // 
            this.btnSeleccionarZip.Location = new System.Drawing.Point(785, 150);
            this.btnSeleccionarZip.Name = "btnSeleccionarZip";
            this.btnSeleccionarZip.Size = new System.Drawing.Size(122, 23);
            this.btnSeleccionarZip.TabIndex = 23;
            this.btnSeleccionarZip.Text = "Seleccionar zip ...";
            this.btnSeleccionarZip.UseVisualStyleBackColor = true;
            this.btnSeleccionarZip.Click += new System.EventHandler(this.btnSeleccionarZip_Click);
            // 
            // rbActivo
            // 
            this.rbActivo.AutoSize = true;
            this.rbActivo.Location = new System.Drawing.Point(15, 157);
            this.rbActivo.Name = "rbActivo";
            this.rbActivo.Size = new System.Drawing.Size(55, 17);
            this.rbActivo.TabIndex = 24;
            this.rbActivo.TabStop = true;
            this.rbActivo.Text = "Activo";
            this.rbActivo.UseVisualStyleBackColor = true;
            // 
            // rbPendiente
            // 
            this.rbPendiente.AutoSize = true;
            this.rbPendiente.Location = new System.Drawing.Point(122, 155);
            this.rbPendiente.Name = "rbPendiente";
            this.rbPendiente.Size = new System.Drawing.Size(73, 17);
            this.rbPendiente.TabIndex = 25;
            this.rbPendiente.TabStop = true;
            this.rbPendiente.Text = "Pendiente";
            this.rbPendiente.UseVisualStyleBackColor = true;
            // 
            // txtOrganismo
            // 
            this.txtOrganismo.Location = new System.Drawing.Point(539, 150);
            this.txtOrganismo.Name = "txtOrganismo";
            this.txtOrganismo.Size = new System.Drawing.Size(100, 20);
            this.txtOrganismo.TabIndex = 26;
            // 
            // lblOrganismo
            // 
            this.lblOrganismo.AutoSize = true;
            this.lblOrganismo.Location = new System.Drawing.Point(467, 152);
            this.lblOrganismo.Name = "lblOrganismo";
            this.lblOrganismo.Size = new System.Drawing.Size(57, 13);
            this.lblOrganismo.TabIndex = 27;
            this.lblOrganismo.Text = "Organismo";
            // 
            // frmEntregarPendientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 616);
            this.Controls.Add(this.lblOrganismo);
            this.Controls.Add(this.txtOrganismo);
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
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEntregarArchivos);
            this.Name = "frmEntregarPendientes";
            this.Text = "Entrega de Pendientes";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComprobantes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEntregarArchivos;
        private System.Windows.Forms.TextBox txtOrigen;
        private System.Windows.Forms.Button btnSeleccionarOrigen;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblOrigen;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Button btnPrueba;
        private System.Windows.Forms.DataGridView dgvComprobantes;
        private System.Windows.Forms.Button btnSeleccionarComprobantes;
        private System.Windows.Forms.ListBox lbComprobantes;
        private System.Windows.Forms.ListBox lbZips;
        private System.Windows.Forms.Button btnSeleccionarZip;
        private System.Windows.Forms.RadioButton rbActivo;
        private System.Windows.Forms.RadioButton rbPendiente;
        private System.Windows.Forms.TextBox txtOrganismo;
        private System.Windows.Forms.Label lblOrganismo;
    }
}

