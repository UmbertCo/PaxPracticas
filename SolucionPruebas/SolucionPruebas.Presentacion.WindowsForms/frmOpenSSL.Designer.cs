namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmOpenSSL
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
            this.btnGenerarSelloOpenSSLCL = new System.Windows.Forms.Button();
            this.txtLlavePrivada = new System.Windows.Forms.TextBox();
            this.btnArchivoLlavePrivada = new System.Windows.Forms.Button();
            this.lblLlavePrivada = new System.Windows.Forms.Label();
            this.btnGenerarSelloRSA = new System.Windows.Forms.Button();
            this.txtArchivoXml = new System.Windows.Forms.TextBox();
            this.btnSeleccionarArchivo = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.btnGenerarSelloOpenSSLNetPfx = new System.Windows.Forms.Button();
            this.btnSeleccionarPfx = new System.Windows.Forms.Button();
            this.lblPfx = new System.Windows.Forms.Label();
            this.txtPfx = new System.Windows.Forms.TextBox();
            this.btnCrearPfxOpenSSLNet = new System.Windows.Forms.Button();
            this.txtLlavePublica = new System.Windows.Forms.TextBox();
            this.lblLlavePublica = new System.Windows.Forms.Label();
            this.btnSeleccionarLlavePublica = new System.Windows.Forms.Button();
            this.btnCrearPfxClase = new System.Windows.Forms.Button();
            this.lblComprobante = new System.Windows.Forms.Label();
            this.btnGenerarSelloOpenSSLNet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGenerarSelloOpenSSLCL
            // 
            this.btnGenerarSelloOpenSSLCL.Location = new System.Drawing.Point(33, 255);
            this.btnGenerarSelloOpenSSLCL.Name = "btnGenerarSelloOpenSSLCL";
            this.btnGenerarSelloOpenSSLCL.Size = new System.Drawing.Size(155, 23);
            this.btnGenerarSelloOpenSSLCL.TabIndex = 0;
            this.btnGenerarSelloOpenSSLCL.Text = "Generar Sello OpenSSL CL";
            this.btnGenerarSelloOpenSSLCL.UseVisualStyleBackColor = true;
            this.btnGenerarSelloOpenSSLCL.Click += new System.EventHandler(this.btnGenerarSelloOpenSSLCL_Click);
            // 
            // txtLlavePrivada
            // 
            this.txtLlavePrivada.Location = new System.Drawing.Point(33, 24);
            this.txtLlavePrivada.Name = "txtLlavePrivada";
            this.txtLlavePrivada.Size = new System.Drawing.Size(388, 20);
            this.txtLlavePrivada.TabIndex = 2;
            // 
            // btnArchivoLlavePrivada
            // 
            this.btnArchivoLlavePrivada.Location = new System.Drawing.Point(427, 22);
            this.btnArchivoLlavePrivada.Name = "btnArchivoLlavePrivada";
            this.btnArchivoLlavePrivada.Size = new System.Drawing.Size(165, 23);
            this.btnArchivoLlavePrivada.TabIndex = 4;
            this.btnArchivoLlavePrivada.Text = "Seleccionar PEM Llave Privada";
            this.btnArchivoLlavePrivada.UseVisualStyleBackColor = true;
            this.btnArchivoLlavePrivada.Click += new System.EventHandler(this.btnArchivoLlavePrivada_Click);
            // 
            // lblLlavePrivada
            // 
            this.lblLlavePrivada.AutoSize = true;
            this.lblLlavePrivada.Location = new System.Drawing.Point(2, 8);
            this.lblLlavePrivada.Name = "lblLlavePrivada";
            this.lblLlavePrivada.Size = new System.Drawing.Size(98, 13);
            this.lblLlavePrivada.TabIndex = 6;
            this.lblLlavePrivada.Text = "PEM Llave Privada";
            // 
            // btnGenerarSelloRSA
            // 
            this.btnGenerarSelloRSA.Location = new System.Drawing.Point(194, 255);
            this.btnGenerarSelloRSA.Name = "btnGenerarSelloRSA";
            this.btnGenerarSelloRSA.Size = new System.Drawing.Size(156, 23);
            this.btnGenerarSelloRSA.TabIndex = 7;
            this.btnGenerarSelloRSA.Text = "Generar Sello Clase Pfx RSA";
            this.btnGenerarSelloRSA.UseVisualStyleBackColor = true;
            this.btnGenerarSelloRSA.Click += new System.EventHandler(this.btnGenerarSelloRSA_Click);
            // 
            // txtArchivoXml
            // 
            this.txtArchivoXml.Location = new System.Drawing.Point(32, 181);
            this.txtArchivoXml.Name = "txtArchivoXml";
            this.txtArchivoXml.Size = new System.Drawing.Size(388, 20);
            this.txtArchivoXml.TabIndex = 14;
            // 
            // btnSeleccionarArchivo
            // 
            this.btnSeleccionarArchivo.Location = new System.Drawing.Point(426, 181);
            this.btnSeleccionarArchivo.Name = "btnSeleccionarArchivo";
            this.btnSeleccionarArchivo.Size = new System.Drawing.Size(165, 23);
            this.btnSeleccionarArchivo.TabIndex = 15;
            this.btnSeleccionarArchivo.Text = "Seleccionar Archivo XML";
            this.btnSeleccionarArchivo.UseVisualStyleBackColor = true;
            this.btnSeleccionarArchivo.Click += new System.EventHandler(this.btnSeleccionarArchivo_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(33, 313);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.Size = new System.Drawing.Size(559, 223);
            this.txtResultado.TabIndex = 16;
            // 
            // btnGenerarSelloOpenSSLNetPfx
            // 
            this.btnGenerarSelloOpenSSLNetPfx.Location = new System.Drawing.Point(365, 254);
            this.btnGenerarSelloOpenSSLNetPfx.Name = "btnGenerarSelloOpenSSLNetPfx";
            this.btnGenerarSelloOpenSSLNetPfx.Size = new System.Drawing.Size(202, 23);
            this.btnGenerarSelloOpenSSLNetPfx.TabIndex = 17;
            this.btnGenerarSelloOpenSSLNetPfx.Text = "Generar Sello OpenSSL.Net Pfx";
            this.btnGenerarSelloOpenSSLNetPfx.UseVisualStyleBackColor = true;
            this.btnGenerarSelloOpenSSLNetPfx.Click += new System.EventHandler(this.btnGenerarSelloOpenSSLNetPfx_Click);
            // 
            // btnSeleccionarPfx
            // 
            this.btnSeleccionarPfx.Location = new System.Drawing.Point(426, 221);
            this.btnSeleccionarPfx.Name = "btnSeleccionarPfx";
            this.btnSeleccionarPfx.Size = new System.Drawing.Size(165, 23);
            this.btnSeleccionarPfx.TabIndex = 11;
            this.btnSeleccionarPfx.Text = "Seleccionar Pfx";
            this.btnSeleccionarPfx.UseVisualStyleBackColor = true;
            this.btnSeleccionarPfx.Click += new System.EventHandler(this.btnSeleccionarPfx_Click);
            // 
            // lblPfx
            // 
            this.lblPfx.AutoSize = true;
            this.lblPfx.Location = new System.Drawing.Point(1, 204);
            this.lblPfx.Name = "lblPfx";
            this.lblPfx.Size = new System.Drawing.Size(22, 13);
            this.lblPfx.TabIndex = 13;
            this.lblPfx.Text = "Pfx";
            // 
            // txtPfx
            // 
            this.txtPfx.Location = new System.Drawing.Point(32, 223);
            this.txtPfx.Name = "txtPfx";
            this.txtPfx.Size = new System.Drawing.Size(388, 20);
            this.txtPfx.TabIndex = 9;
            // 
            // btnCrearPfxOpenSSLNet
            // 
            this.btnCrearPfxOpenSSLNet.Location = new System.Drawing.Point(194, 284);
            this.btnCrearPfxOpenSSLNet.Name = "btnCrearPfxOpenSSLNet";
            this.btnCrearPfxOpenSSLNet.Size = new System.Drawing.Size(156, 23);
            this.btnCrearPfxOpenSSLNet.TabIndex = 18;
            this.btnCrearPfxOpenSSLNet.Text = "Crear Pfx OpenSSL.Net";
            this.btnCrearPfxOpenSSLNet.UseVisualStyleBackColor = true;
            this.btnCrearPfxOpenSSLNet.Click += new System.EventHandler(this.btnCrearPfxOpenSSLNet_Click);
            // 
            // txtLlavePublica
            // 
            this.txtLlavePublica.Location = new System.Drawing.Point(34, 64);
            this.txtLlavePublica.Name = "txtLlavePublica";
            this.txtLlavePublica.Size = new System.Drawing.Size(387, 20);
            this.txtLlavePublica.TabIndex = 19;
            // 
            // lblLlavePublica
            // 
            this.lblLlavePublica.AutoSize = true;
            this.lblLlavePublica.Location = new System.Drawing.Point(2, 47);
            this.lblLlavePublica.Name = "lblLlavePublica";
            this.lblLlavePublica.Size = new System.Drawing.Size(97, 13);
            this.lblLlavePublica.TabIndex = 20;
            this.lblLlavePublica.Text = "PEM Llave Pública";
            // 
            // btnSeleccionarLlavePublica
            // 
            this.btnSeleccionarLlavePublica.Location = new System.Drawing.Point(427, 64);
            this.btnSeleccionarLlavePublica.Name = "btnSeleccionarLlavePublica";
            this.btnSeleccionarLlavePublica.Size = new System.Drawing.Size(165, 23);
            this.btnSeleccionarLlavePublica.TabIndex = 21;
            this.btnSeleccionarLlavePublica.Text = "Seleccionar PEM Llave Publica";
            this.btnSeleccionarLlavePublica.UseVisualStyleBackColor = true;
            this.btnSeleccionarLlavePublica.Click += new System.EventHandler(this.btnSeleccionarLlavePublica_Click);
            // 
            // btnCrearPfxClase
            // 
            this.btnCrearPfxClase.Location = new System.Drawing.Point(32, 284);
            this.btnCrearPfxClase.Name = "btnCrearPfxClase";
            this.btnCrearPfxClase.Size = new System.Drawing.Size(156, 23);
            this.btnCrearPfxClase.TabIndex = 22;
            this.btnCrearPfxClase.Text = "Crear Pfx con clase";
            this.btnCrearPfxClase.UseVisualStyleBackColor = true;
            this.btnCrearPfxClase.Click += new System.EventHandler(this.btnCrearPfxClase_Click);
            // 
            // lblComprobante
            // 
            this.lblComprobante.AutoSize = true;
            this.lblComprobante.Location = new System.Drawing.Point(1, 165);
            this.lblComprobante.Name = "lblComprobante";
            this.lblComprobante.Size = new System.Drawing.Size(70, 13);
            this.lblComprobante.TabIndex = 23;
            this.lblComprobante.Text = "Comprobante";
            // 
            // btnGenerarSelloOpenSSLNet
            // 
            this.btnGenerarSelloOpenSSLNet.Location = new System.Drawing.Point(366, 284);
            this.btnGenerarSelloOpenSSLNet.Name = "btnGenerarSelloOpenSSLNet";
            this.btnGenerarSelloOpenSSLNet.Size = new System.Drawing.Size(201, 23);
            this.btnGenerarSelloOpenSSLNet.TabIndex = 24;
            this.btnGenerarSelloOpenSSLNet.Text = "Generar Sello OpenSSL.Net";
            this.btnGenerarSelloOpenSSLNet.UseVisualStyleBackColor = true;
            this.btnGenerarSelloOpenSSLNet.Click += new System.EventHandler(this.btnGenerarSelloOpenSSLNet_Click);
            // 
            // frmOpenSSL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 548);
            this.Controls.Add(this.btnGenerarSelloOpenSSLNet);
            this.Controls.Add(this.lblComprobante);
            this.Controls.Add(this.btnCrearPfxClase);
            this.Controls.Add(this.btnSeleccionarLlavePublica);
            this.Controls.Add(this.lblLlavePublica);
            this.Controls.Add(this.txtLlavePublica);
            this.Controls.Add(this.btnCrearPfxOpenSSLNet);
            this.Controls.Add(this.btnGenerarSelloOpenSSLNetPfx);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btnSeleccionarArchivo);
            this.Controls.Add(this.txtArchivoXml);
            this.Controls.Add(this.lblPfx);
            this.Controls.Add(this.btnSeleccionarPfx);
            this.Controls.Add(this.txtPfx);
            this.Controls.Add(this.btnGenerarSelloRSA);
            this.Controls.Add(this.lblLlavePrivada);
            this.Controls.Add(this.btnArchivoLlavePrivada);
            this.Controls.Add(this.txtLlavePrivada);
            this.Controls.Add(this.btnGenerarSelloOpenSSLCL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmOpenSSL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open SSL";
            this.Click += new System.EventHandler(this.btnGenerarSelloOpenSSLNetPfx_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerarSelloOpenSSLCL;
        private System.Windows.Forms.TextBox txtLlavePrivada;
        private System.Windows.Forms.Button btnArchivoLlavePrivada;
        private System.Windows.Forms.Label lblLlavePrivada;
        private System.Windows.Forms.Button btnGenerarSelloRSA;
        private System.Windows.Forms.TextBox txtArchivoXml;
        private System.Windows.Forms.Button btnSeleccionarArchivo;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Button btnGenerarSelloOpenSSLNetPfx;
        private System.Windows.Forms.Button btnSeleccionarPfx;
        private System.Windows.Forms.Label lblPfx;
        private System.Windows.Forms.TextBox txtPfx;
        private System.Windows.Forms.Button btnCrearPfxOpenSSLNet;
        private System.Windows.Forms.TextBox txtLlavePublica;
        private System.Windows.Forms.Label lblLlavePublica;
        private System.Windows.Forms.Button btnSeleccionarLlavePublica;
        private System.Windows.Forms.Button btnCrearPfxClase;
        private System.Windows.Forms.Label lblComprobante;
        private System.Windows.Forms.Button btnGenerarSelloOpenSSLNet;
    }
}