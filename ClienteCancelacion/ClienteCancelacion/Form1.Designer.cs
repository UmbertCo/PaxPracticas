namespace ClienteCancelacion
{
    partial class Form1
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
            this.btnGenerarUUID = new System.Windows.Forms.Button();
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbRFC = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lstUUID = new System.Windows.Forms.ListBox();
            this.btnAgregarUUID = new System.Windows.Forms.Button();
            this.btnBorrar = new System.Windows.Forms.Button();
            this.btnCertificado = new System.Windows.Forms.Button();
            this.btnLlave = new System.Windows.Forms.Button();
            this.btnCancelarUUID = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPasswordCer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnGenerarUUID
            // 
            this.btnGenerarUUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarUUID.Location = new System.Drawing.Point(346, 19);
            this.btnGenerarUUID.Name = "btnGenerarUUID";
            this.btnGenerarUUID.Size = new System.Drawing.Size(104, 22);
            this.btnGenerarUUID.TabIndex = 0;
            this.btnGenerarUUID.Text = "Generar UUID";
            this.btnGenerarUUID.UseVisualStyleBackColor = true;
            this.btnGenerarUUID.Click += new System.EventHandler(this.btnGenerarUUID_Click);
            // 
            // txtUUID
            // 
            this.txtUUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUUID.Location = new System.Drawing.Point(124, 19);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.Size = new System.Drawing.Size(216, 20);
            this.txtUUID.TabIndex = 1;
            this.txtUUID.Text = "2fc142cf-7043-495a-b3b2-3d3b85f8919e";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(48, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 61;
            this.label4.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(37, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 58;
            this.label1.Text = "RFC Emisor:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(71, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 57;
            this.label7.Text = "UUID:";
            // 
            // cmbRFC
            // 
            this.cmbRFC.Enabled = false;
            this.cmbRFC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRFC.Location = new System.Drawing.Point(124, 47);
            this.cmbRFC.Name = "cmbRFC";
            this.cmbRFC.Size = new System.Drawing.Size(216, 20);
            this.cmbRFC.TabIndex = 62;
            this.cmbRFC.Text = "AAA010101AAA";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(124, 101);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(216, 20);
            this.txtPassword.TabIndex = 65;
            this.txtPassword.Text = "w4XDjcObxIHDt8OcxLPEj8SZxY3Cr8KRwqnDqsKJwpLDn8KHxIPEkQgf77+VBO++oO++j++/h++9pw==";
            // 
            // lstUUID
            // 
            this.lstUUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstUUID.ForeColor = System.Drawing.Color.Black;
            this.lstUUID.FormattingEnabled = true;
            this.lstUUID.Location = new System.Drawing.Point(124, 155);
            this.lstUUID.Name = "lstUUID";
            this.lstUUID.Size = new System.Drawing.Size(216, 108);
            this.lstUUID.TabIndex = 66;
            // 
            // btnAgregarUUID
            // 
            this.btnAgregarUUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarUUID.Location = new System.Drawing.Point(346, 157);
            this.btnAgregarUUID.Name = "btnAgregarUUID";
            this.btnAgregarUUID.Size = new System.Drawing.Size(104, 22);
            this.btnAgregarUUID.TabIndex = 67;
            this.btnAgregarUUID.Text = "Agregar";
            this.btnAgregarUUID.UseVisualStyleBackColor = true;
            this.btnAgregarUUID.Click += new System.EventHandler(this.btnAgregarUUID_Click);
            // 
            // btnBorrar
            // 
            this.btnBorrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBorrar.Location = new System.Drawing.Point(346, 185);
            this.btnBorrar.Name = "btnBorrar";
            this.btnBorrar.Size = new System.Drawing.Size(104, 22);
            this.btnBorrar.TabIndex = 68;
            this.btnBorrar.Text = "Borrar";
            this.btnBorrar.UseVisualStyleBackColor = true;
            this.btnBorrar.Click += new System.EventHandler(this.btnBorrar_Click);
            // 
            // btnCertificado
            // 
            this.btnCertificado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCertificado.Location = new System.Drawing.Point(346, 213);
            this.btnCertificado.Name = "btnCertificado";
            this.btnCertificado.Size = new System.Drawing.Size(104, 22);
            this.btnCertificado.TabIndex = 69;
            this.btnCertificado.Text = "Cargar Certificado";
            this.btnCertificado.UseVisualStyleBackColor = true;
            this.btnCertificado.Click += new System.EventHandler(this.btnCertificado_Click);
            // 
            // btnLlave
            // 
            this.btnLlave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLlave.Location = new System.Drawing.Point(346, 241);
            this.btnLlave.Name = "btnLlave";
            this.btnLlave.Size = new System.Drawing.Size(104, 22);
            this.btnLlave.TabIndex = 70;
            this.btnLlave.Text = "Llave";
            this.btnLlave.UseVisualStyleBackColor = true;
            this.btnLlave.Click += new System.EventHandler(this.btnLlave_Click);
            // 
            // btnCancelarUUID
            // 
            this.btnCancelarUUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelarUUID.Location = new System.Drawing.Point(124, 270);
            this.btnCancelarUUID.Name = "btnCancelarUUID";
            this.btnCancelarUUID.Size = new System.Drawing.Size(143, 23);
            this.btnCancelarUUID.TabIndex = 71;
            this.btnCancelarUUID.Text = "Cancelar UUID";
            this.btnCancelarUUID.UseVisualStyleBackColor = true;
            this.btnCancelarUUID.Click += new System.EventHandler(this.btnCancelarUUID_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(124, 299);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(610, 264);
            this.richTextBox1.TabIndex = 72;
            this.richTextBox1.Text = "";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(124, 73);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(216, 20);
            this.txtUsuario.TabIndex = 64;
            this.txtUsuario.Text = "WSDL_PAX";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(55, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 60;
            this.label3.Text = "Usuario :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 73;
            this.label2.Text = "Password Cer:";
            // 
            // txtPasswordCer
            // 
            this.txtPasswordCer.Enabled = false;
            this.txtPasswordCer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPasswordCer.Location = new System.Drawing.Point(124, 129);
            this.txtPasswordCer.Name = "txtPasswordCer";
            this.txtPasswordCer.Size = new System.Drawing.Size(216, 20);
            this.txtPasswordCer.TabIndex = 74;
            this.txtPasswordCer.Text = "12345678a";
            // 
            // Form1
            // 
            this.AccessibleDescription = " ";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 575);
            this.Controls.Add(this.txtPasswordCer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnCancelarUUID);
            this.Controls.Add(this.btnLlave);
            this.Controls.Add(this.btnCertificado);
            this.Controls.Add(this.btnBorrar);
            this.Controls.Add(this.btnAgregarUUID);
            this.Controls.Add(this.lstUUID);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.cmbRFC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtUUID);
            this.Controls.Add(this.btnGenerarUUID);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerarUUID;
        private System.Windows.Forms.TextBox txtUUID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox cmbRFC;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ListBox lstUUID;
        private System.Windows.Forms.Button btnAgregarUUID;
        private System.Windows.Forms.Button btnBorrar;
        private System.Windows.Forms.Button btnCertificado;
        private System.Windows.Forms.Button btnLlave;
        private System.Windows.Forms.Button btnCancelarUUID;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPasswordCer;
    }
}

