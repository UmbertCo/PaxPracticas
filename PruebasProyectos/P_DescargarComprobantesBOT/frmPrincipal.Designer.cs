namespace P_DescargarComprobantesBOT
{
    partial class frmPrincipal
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
            this.lblRFC = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.lblLlave = new System.Windows.Forms.Label();
            this.lblCer = new System.Windows.Forms.Label();
            this.txtRFC = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtLlave = new System.Windows.Forms.TextBox();
            this.txtCer = new System.Windows.Forms.TextBox();
            this.btnConectar = new System.Windows.Forms.Button();
            this.btnCer = new System.Windows.Forms.Button();
            this.btnLlave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblRFC
            // 
            this.lblRFC.AutoSize = true;
            this.lblRFC.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRFC.Location = new System.Drawing.Point(62, 11);
            this.lblRFC.Name = "lblRFC";
            this.lblRFC.Size = new System.Drawing.Size(53, 24);
            this.lblRFC.TabIndex = 0;
            this.lblRFC.Text = "RFC:";
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPass.Location = new System.Drawing.Point(5, 172);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(111, 24);
            this.lblPass.TabIndex = 1;
            this.lblPass.Text = "Contraseña:";
            // 
            // lblLlave
            // 
            this.lblLlave.AutoSize = true;
            this.lblLlave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLlave.Location = new System.Drawing.Point(57, 74);
            this.lblLlave.Name = "lblLlave";
            this.lblLlave.Size = new System.Drawing.Size(59, 24);
            this.lblLlave.TabIndex = 2;
            this.lblLlave.Text = "Llave:";
            // 
            // lblCer
            // 
            this.lblCer.AutoSize = true;
            this.lblCer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCer.Location = new System.Drawing.Point(13, 125);
            this.lblCer.Name = "lblCer";
            this.lblCer.Size = new System.Drawing.Size(103, 24);
            this.lblCer.TabIndex = 3;
            this.lblCer.Text = "Certificado:";
            // 
            // txtRFC
            // 
            this.txtRFC.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtRFC.Location = new System.Drawing.Point(122, 14);
            this.txtRFC.Name = "txtRFC";
            this.txtRFC.Size = new System.Drawing.Size(237, 29);
            this.txtRFC.TabIndex = 4;
            // 
            // txtPass
            // 
            this.txtPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtPass.Location = new System.Drawing.Point(123, 174);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(237, 29);
            this.txtPass.TabIndex = 5;
            // 
            // txtLlave
            // 
            this.txtLlave.Enabled = false;
            this.txtLlave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtLlave.Location = new System.Drawing.Point(123, 74);
            this.txtLlave.Name = "txtLlave";
            this.txtLlave.Size = new System.Drawing.Size(459, 29);
            this.txtLlave.TabIndex = 6;
            // 
            // txtCer
            // 
            this.txtCer.Enabled = false;
            this.txtCer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtCer.Location = new System.Drawing.Point(122, 125);
            this.txtCer.Name = "txtCer";
            this.txtCer.Size = new System.Drawing.Size(460, 29);
            this.txtCer.TabIndex = 7;
            // 
            // btnConectar
            // 
            this.btnConectar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnConectar.Location = new System.Drawing.Point(61, 209);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(105, 34);
            this.btnConectar.TabIndex = 8;
            this.btnConectar.Text = "Conectar";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // btnCer
            // 
            this.btnCer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnCer.Location = new System.Drawing.Point(588, 123);
            this.btnCer.Name = "btnCer";
            this.btnCer.Size = new System.Drawing.Size(41, 34);
            this.btnCer.TabIndex = 9;
            this.btnCer.Text = "...";
            this.btnCer.UseVisualStyleBackColor = true;
            this.btnCer.Click += new System.EventHandler(this.btnCer_Click);
            // 
            // btnLlave
            // 
            this.btnLlave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnLlave.Location = new System.Drawing.Point(588, 72);
            this.btnLlave.Name = "btnLlave";
            this.btnLlave.Size = new System.Drawing.Size(41, 34);
            this.btnLlave.TabIndex = 10;
            this.btnLlave.Text = "...";
            this.btnLlave.UseVisualStyleBackColor = true;
            this.btnLlave.Click += new System.EventHandler(this.btnLlave_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 262);
            this.Controls.Add(this.btnLlave);
            this.Controls.Add(this.btnCer);
            this.Controls.Add(this.btnConectar);
            this.Controls.Add(this.txtCer);
            this.Controls.Add(this.txtLlave);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtRFC);
            this.Controls.Add(this.lblCer);
            this.Controls.Add(this.lblLlave);
            this.Controls.Add(this.lblPass);
            this.Controls.Add(this.lblRFC);
            this.Name = "frmPrincipal";
            this.Text = "Autenticar";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRFC;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.Label lblLlave;
        private System.Windows.Forms.Label lblCer;
        private System.Windows.Forms.TextBox txtRFC;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtLlave;
        private System.Windows.Forms.TextBox txtCer;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.Button btnCer;
        private System.Windows.Forms.Button btnLlave;
    }
}

