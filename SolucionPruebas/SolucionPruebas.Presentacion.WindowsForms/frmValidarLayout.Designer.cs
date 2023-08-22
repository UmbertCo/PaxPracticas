namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmValidarLayout
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
            this.btnCargarLlavePrivada = new System.Windows.Forms.Button();
            this.btnCargarCertificado = new System.Windows.Forms.Button();
            this.txtResultados = new System.Windows.Forms.TextBox();
            this.txtArchivo = new System.Windows.Forms.TextBox();
            this.btnValidar = new System.Windows.Forms.Button();
            this.btnSeleccionarArchivo = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnCargarLlavePrivada
            // 
            this.btnCargarLlavePrivada.Location = new System.Drawing.Point(543, 60);
            this.btnCargarLlavePrivada.Name = "btnCargarLlavePrivada";
            this.btnCargarLlavePrivada.Size = new System.Drawing.Size(134, 23);
            this.btnCargarLlavePrivada.TabIndex = 16;
            this.btnCargarLlavePrivada.Text = "Cargar Llave Privada";
            this.btnCargarLlavePrivada.UseVisualStyleBackColor = true;
            this.btnCargarLlavePrivada.Click += new System.EventHandler(this.btnCargarLlavePrivada_Click);
            // 
            // btnCargarCertificado
            // 
            this.btnCargarCertificado.Location = new System.Drawing.Point(543, 89);
            this.btnCargarCertificado.Name = "btnCargarCertificado";
            this.btnCargarCertificado.Size = new System.Drawing.Size(134, 23);
            this.btnCargarCertificado.TabIndex = 15;
            this.btnCargarCertificado.Text = "Cargar Certificado";
            this.btnCargarCertificado.UseVisualStyleBackColor = true;
            this.btnCargarCertificado.Click += new System.EventHandler(this.btnCargarCertificado_Click);
            // 
            // txtResultados
            // 
            this.txtResultados.Location = new System.Drawing.Point(27, 157);
            this.txtResultados.Multiline = true;
            this.txtResultados.Name = "txtResultados";
            this.txtResultados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultados.Size = new System.Drawing.Size(650, 198);
            this.txtResultados.TabIndex = 14;
            // 
            // txtArchivo
            // 
            this.txtArchivo.Location = new System.Drawing.Point(27, 33);
            this.txtArchivo.Multiline = true;
            this.txtArchivo.Name = "txtArchivo";
            this.txtArchivo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtArchivo.Size = new System.Drawing.Size(510, 79);
            this.txtArchivo.TabIndex = 13;
            // 
            // btnValidar
            // 
            this.btnValidar.Location = new System.Drawing.Point(27, 119);
            this.btnValidar.Name = "btnValidar";
            this.btnValidar.Size = new System.Drawing.Size(75, 23);
            this.btnValidar.TabIndex = 12;
            this.btnValidar.Text = "Validar Layouts";
            this.btnValidar.UseVisualStyleBackColor = true;
            this.btnValidar.Click += new System.EventHandler(this.btnValidar_Click);
            // 
            // btnSeleccionarArchivo
            // 
            this.btnSeleccionarArchivo.Location = new System.Drawing.Point(543, 31);
            this.btnSeleccionarArchivo.Name = "btnSeleccionarArchivo";
            this.btnSeleccionarArchivo.Size = new System.Drawing.Size(134, 23);
            this.btnSeleccionarArchivo.TabIndex = 11;
            this.btnSeleccionarArchivo.Text = "Seleccionar archivo(s)";
            this.btnSeleccionarArchivo.UseVisualStyleBackColor = true;
            this.btnSeleccionarArchivo.Click += new System.EventHandler(this.btnSeleccionarArchivo_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(543, 119);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(134, 20);
            this.txtPassword.TabIndex = 18;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog1";
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog1";
            // 
            // frmValidarLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 390);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnCargarLlavePrivada);
            this.Controls.Add(this.btnCargarCertificado);
            this.Controls.Add(this.txtResultados);
            this.Controls.Add(this.txtArchivo);
            this.Controls.Add(this.btnValidar);
            this.Controls.Add(this.btnSeleccionarArchivo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmValidarLayout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmValidarLayout";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCargarLlavePrivada;
        private System.Windows.Forms.Button btnCargarCertificado;
        private System.Windows.Forms.TextBox txtResultados;
        private System.Windows.Forms.TextBox txtArchivo;
        private System.Windows.Forms.Button btnValidar;
        private System.Windows.Forms.Button btnSeleccionarArchivo;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
    }
}