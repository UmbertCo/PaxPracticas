namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmValidarLayoutsNomina
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
            this.txtResultados = new System.Windows.Forms.TextBox();
            this.txtArchivo = new System.Windows.Forms.TextBox();
            this.btnValidar = new System.Windows.Forms.Button();
            this.btnSeleccionarArchivo = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnCargarCertificado = new System.Windows.Forms.Button();
            this.btnCargarLlavePrivada = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog4 = new System.Windows.Forms.OpenFileDialog();
            this.btnCargarPassword = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtResultados
            // 
            this.txtResultados.Location = new System.Drawing.Point(47, 153);
            this.txtResultados.Multiline = true;
            this.txtResultados.Name = "txtResultados";
            this.txtResultados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultados.Size = new System.Drawing.Size(534, 198);
            this.txtResultados.TabIndex = 7;
            // 
            // txtArchivo
            // 
            this.txtArchivo.Location = new System.Drawing.Point(47, 29);
            this.txtArchivo.Multiline = true;
            this.txtArchivo.Name = "txtArchivo";
            this.txtArchivo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtArchivo.Size = new System.Drawing.Size(411, 50);
            this.txtArchivo.TabIndex = 6;
            // 
            // btnValidar
            // 
            this.btnValidar.Location = new System.Drawing.Point(47, 115);
            this.btnValidar.Name = "btnValidar";
            this.btnValidar.Size = new System.Drawing.Size(75, 23);
            this.btnValidar.TabIndex = 5;
            this.btnValidar.Text = "Validar Layouts";
            this.btnValidar.UseVisualStyleBackColor = true;
            this.btnValidar.Click += new System.EventHandler(this.btnValidar_Click);
            // 
            // btnSeleccionarArchivo
            // 
            this.btnSeleccionarArchivo.Location = new System.Drawing.Point(464, 29);
            this.btnSeleccionarArchivo.Name = "btnSeleccionarArchivo";
            this.btnSeleccionarArchivo.Size = new System.Drawing.Size(134, 23);
            this.btnSeleccionarArchivo.TabIndex = 4;
            this.btnSeleccionarArchivo.Text = "Seleccionar archivo(s)";
            this.btnSeleccionarArchivo.UseVisualStyleBackColor = true;
            this.btnSeleccionarArchivo.Click += new System.EventHandler(this.btnSeleccionarArchivo_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnCargarCertificado
            // 
            this.btnCargarCertificado.Location = new System.Drawing.Point(464, 87);
            this.btnCargarCertificado.Name = "btnCargarCertificado";
            this.btnCargarCertificado.Size = new System.Drawing.Size(134, 23);
            this.btnCargarCertificado.TabIndex = 8;
            this.btnCargarCertificado.Text = "Cargar Certificado";
            this.btnCargarCertificado.UseVisualStyleBackColor = true;
            this.btnCargarCertificado.Click += new System.EventHandler(this.btnCargarCertificado_Click);
            // 
            // btnCargarLlavePrivada
            // 
            this.btnCargarLlavePrivada.Location = new System.Drawing.Point(464, 58);
            this.btnCargarLlavePrivada.Name = "btnCargarLlavePrivada";
            this.btnCargarLlavePrivada.Size = new System.Drawing.Size(134, 23);
            this.btnCargarLlavePrivada.TabIndex = 9;
            this.btnCargarLlavePrivada.Text = "Cargar Llave Privada";
            this.btnCargarLlavePrivada.UseVisualStyleBackColor = true;
            this.btnCargarLlavePrivada.Click += new System.EventHandler(this.btnCargarLlavePrivada_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog1";
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog1";
            // 
            // openFileDialog4
            // 
            this.openFileDialog4.FileName = "openFileDialog1";
            // 
            // btnCargarPassword
            // 
            this.btnCargarPassword.Location = new System.Drawing.Point(464, 115);
            this.btnCargarPassword.Name = "btnCargarPassword";
            this.btnCargarPassword.Size = new System.Drawing.Size(134, 23);
            this.btnCargarPassword.TabIndex = 10;
            this.btnCargarPassword.Text = "Cargar Password";
            this.btnCargarPassword.UseVisualStyleBackColor = true;
            this.btnCargarPassword.Click += new System.EventHandler(this.btnCargarPassword_Click);
            // 
            // frmValidarLayoutsNomina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 381);
            this.Controls.Add(this.btnCargarPassword);
            this.Controls.Add(this.btnCargarLlavePrivada);
            this.Controls.Add(this.btnCargarCertificado);
            this.Controls.Add(this.txtResultados);
            this.Controls.Add(this.txtArchivo);
            this.Controls.Add(this.btnValidar);
            this.Controls.Add(this.btnSeleccionarArchivo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmValidarLayoutsNomina";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Validar Layouts Nómina";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtResultados;
        private System.Windows.Forms.TextBox txtArchivo;
        private System.Windows.Forms.Button btnValidar;
        private System.Windows.Forms.Button btnSeleccionarArchivo;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnCargarCertificado;
        private System.Windows.Forms.Button btnCargarLlavePrivada;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
        private System.Windows.Forms.OpenFileDialog openFileDialog4;
        private System.Windows.Forms.Button btnCargarPassword;
    }
}