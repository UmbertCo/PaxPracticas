namespace GeneradorMasivoComprobantes
{
    partial class CargaCertificado
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
            this.btnSeleccionarCer = new System.Windows.Forms.Button();
            this.btnSeleccionarKey = new System.Windows.Forms.Button();
            this.txtCer = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblProceso = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lnkCarpeta = new System.Windows.Forms.LinkLabel();
            this.lblTipo = new System.Windows.Forms.Label();
            this.lblCodificacion = new System.Windows.Forms.Label();
            this.lblInformacion = new System.Windows.Forms.Label();
            this.lblTamano = new System.Windows.Forms.Label();
            this.lblTipoValue = new System.Windows.Forms.Label();
            this.lblCodificacionValue = new System.Windows.Forms.Label();
            this.lblTamanoValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSeleccionarCer
            // 
            this.btnSeleccionarCer.Location = new System.Drawing.Point(23, 21);
            this.btnSeleccionarCer.Name = "btnSeleccionarCer";
            this.btnSeleccionarCer.Size = new System.Drawing.Size(180, 25);
            this.btnSeleccionarCer.TabIndex = 27;
            this.btnSeleccionarCer.Text = "Selecciona Certificado";
            this.btnSeleccionarCer.UseVisualStyleBackColor = true;
            this.btnSeleccionarCer.Click += new System.EventHandler(this.btnSeleccionarCer_Click);
            // 
            // btnSeleccionarKey
            // 
            this.btnSeleccionarKey.Location = new System.Drawing.Point(23, 78);
            this.btnSeleccionarKey.Name = "btnSeleccionarKey";
            this.btnSeleccionarKey.Size = new System.Drawing.Size(180, 25);
            this.btnSeleccionarKey.TabIndex = 28;
            this.btnSeleccionarKey.Text = "Selecciona KEY";
            this.btnSeleccionarKey.UseVisualStyleBackColor = true;
            this.btnSeleccionarKey.Click += new System.EventHandler(this.btnSeleccionarKey_Click);
            // 
            // txtCer
            // 
            this.txtCer.Enabled = false;
            this.txtCer.Location = new System.Drawing.Point(240, 21);
            this.txtCer.Name = "txtCer";
            this.txtCer.Size = new System.Drawing.Size(232, 20);
            this.txtCer.TabIndex = 29;
            this.txtCer.TextChanged += new System.EventHandler(this.txtCer_TextChanged);
            // 
            // txtKey
            // 
            this.txtKey.Enabled = false;
            this.txtKey.Location = new System.Drawing.Point(240, 78);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(232, 20);
            this.txtKey.TabIndex = 30;
            this.txtKey.TextChanged += new System.EventHandler(this.txtKey_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Contraseña:";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(91, 133);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(124, 20);
            this.txtPass.TabIndex = 32;
            this.txtPass.Text = "12345678a";
            this.txtPass.UseSystemPasswordChar = true;
            this.txtPass.TextChanged += new System.EventHandler(this.txtPass_TextChanged);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Enabled = false;
            this.btnAceptar.Location = new System.Drawing.Point(252, 133);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(97, 43);
            this.btnAceptar.TabIndex = 33;
            this.btnAceptar.Text = "Generar Comprobantes";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(375, 133);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(97, 43);
            this.btnCancelar.TabIndex = 34;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblProceso
            // 
            this.lblProceso.AutoSize = true;
            this.lblProceso.Location = new System.Drawing.Point(92, 211);
            this.lblProceso.Name = "lblProceso";
            this.lblProceso.Size = new System.Drawing.Size(153, 13);
            this.lblProceso.TabIndex = 35;
            this.lblProceso.Text = "Esperando archivos .cer y .key";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // lnkCarpeta
            // 
            this.lnkCarpeta.AutoSize = true;
            this.lnkCarpeta.Location = new System.Drawing.Point(275, 190);
            this.lnkCarpeta.Name = "lnkCarpeta";
            this.lnkCarpeta.Size = new System.Drawing.Size(178, 13);
            this.lnkCarpeta.TabIndex = 36;
            this.lnkCarpeta.TabStop = true;
            this.lnkCarpeta.Text = "Abrir carpeta de archivos generados";
            this.lnkCarpeta.Visible = false;
            this.lnkCarpeta.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCarpeta_LinkClicked);
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipo.Location = new System.Drawing.Point(20, 273);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(36, 13);
            this.lblTipo.TabIndex = 37;
            this.lblTipo.Text = "Tipo:";
            this.lblTipo.Visible = false;
            // 
            // lblCodificacion
            // 
            this.lblCodificacion.AutoSize = true;
            this.lblCodificacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodificacion.Location = new System.Drawing.Point(147, 273);
            this.lblCodificacion.Name = "lblCodificacion";
            this.lblCodificacion.Size = new System.Drawing.Size(81, 13);
            this.lblCodificacion.TabIndex = 38;
            this.lblCodificacion.Text = "Codificación:";
            this.lblCodificacion.Visible = false;
            // 
            // lblInformacion
            // 
            this.lblInformacion.AutoSize = true;
            this.lblInformacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInformacion.Location = new System.Drawing.Point(20, 248);
            this.lblInformacion.Name = "lblInformacion";
            this.lblInformacion.Size = new System.Drawing.Size(191, 16);
            this.lblInformacion.TabIndex = 39;
            this.lblInformacion.Text = "Información de certificado:";
            this.lblInformacion.Visible = false;
            // 
            // lblTamano
            // 
            this.lblTamano.AutoSize = true;
            this.lblTamano.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTamano.Location = new System.Drawing.Point(330, 273);
            this.lblTamano.Name = "lblTamano";
            this.lblTamano.Size = new System.Drawing.Size(56, 13);
            this.lblTamano.TabIndex = 40;
            this.lblTamano.Text = "Tamaño:";
            this.lblTamano.Visible = false;
            // 
            // lblTipoValue
            // 
            this.lblTipoValue.AutoSize = true;
            this.lblTipoValue.Location = new System.Drawing.Point(62, 273);
            this.lblTipoValue.Name = "lblTipoValue";
            this.lblTipoValue.Size = new System.Drawing.Size(29, 13);
            this.lblTipoValue.TabIndex = 41;
            this.lblTipoValue.Text = "FIEL";
            this.lblTipoValue.Visible = false;
            // 
            // lblCodificacionValue
            // 
            this.lblCodificacionValue.AutoSize = true;
            this.lblCodificacionValue.Location = new System.Drawing.Point(234, 273);
            this.lblCodificacionValue.Name = "lblCodificacionValue";
            this.lblCodificacionValue.Size = new System.Drawing.Size(40, 13);
            this.lblCodificacionValue.TabIndex = 42;
            this.lblCodificacionValue.Text = "SH256";
            this.lblCodificacionValue.Visible = false;
            // 
            // lblTamanoValue
            // 
            this.lblTamanoValue.AutoSize = true;
            this.lblTamanoValue.Location = new System.Drawing.Point(393, 272);
            this.lblTamanoValue.Name = "lblTamanoValue";
            this.lblTamanoValue.Size = new System.Drawing.Size(31, 13);
            this.lblTamanoValue.TabIndex = 43;
            this.lblTamanoValue.Text = "1024";
            this.lblTamanoValue.Visible = false;
            // 
            // CargaCertificado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 322);
            this.Controls.Add(this.lblTamanoValue);
            this.Controls.Add(this.lblCodificacionValue);
            this.Controls.Add(this.lblTipoValue);
            this.Controls.Add(this.lblTamano);
            this.Controls.Add(this.lblInformacion);
            this.Controls.Add(this.lblCodificacion);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.lnkCarpeta);
            this.Controls.Add(this.lblProceso);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.txtCer);
            this.Controls.Add(this.btnSeleccionarKey);
            this.Controls.Add(this.btnSeleccionarCer);
            this.Name = "CargaCertificado";
            this.Text = "CargaCertificado";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSeleccionarCer;
        private System.Windows.Forms.Button btnSeleccionarKey;
        private System.Windows.Forms.TextBox txtCer;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        public System.Windows.Forms.Label lblProceso;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.LinkLabel lnkCarpeta;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.Label lblCodificacion;
        private System.Windows.Forms.Label lblInformacion;
        private System.Windows.Forms.Label lblTamano;
        private System.Windows.Forms.Label lblTipoValue;
        private System.Windows.Forms.Label lblCodificacionValue;
        private System.Windows.Forms.Label lblTamanoValue;
    }
}