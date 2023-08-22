namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmEncriptacion
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
            this.btnGenerar = new System.Windows.Forms.Button();
            this.txtFuente = new System.Windows.Forms.TextBox();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.lblFuente = new System.Windows.Forms.Label();
            this.lblResultado = new System.Windows.Forms.Label();
            this.btnLimpiarBase64 = new System.Windows.Forms.Button();
            this.cbTipoEncriptacion = new System.Windows.Forms.ComboBox();
            this.lblTipoEncriptacion = new System.Windows.Forms.Label();
            this.lblAcccionEncriptacion = new System.Windows.Forms.Label();
            this.cbAccion = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(134, 54);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(75, 23);
            this.btnGenerar.TabIndex = 0;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // txtFuente
            // 
            this.txtFuente.Location = new System.Drawing.Point(12, 111);
            this.txtFuente.MaxLength = 40000;
            this.txtFuente.Multiline = true;
            this.txtFuente.Name = "txtFuente";
            this.txtFuente.Size = new System.Drawing.Size(649, 99);
            this.txtFuente.TabIndex = 1;
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(12, 246);
            this.txtResultado.MaxLength = 40000;
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.Size = new System.Drawing.Size(649, 99);
            this.txtResultado.TabIndex = 2;
            // 
            // lblFuente
            // 
            this.lblFuente.AutoSize = true;
            this.lblFuente.Location = new System.Drawing.Point(12, 95);
            this.lblFuente.Name = "lblFuente";
            this.lblFuente.Size = new System.Drawing.Size(40, 13);
            this.lblFuente.TabIndex = 3;
            this.lblFuente.Text = "Fuente";
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(12, 230);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(55, 13);
            this.lblResultado.TabIndex = 4;
            this.lblResultado.Text = "Resultado";
            // 
            // btnLimpiarBase64
            // 
            this.btnLimpiarBase64.Location = new System.Drawing.Point(12, 54);
            this.btnLimpiarBase64.Name = "btnLimpiarBase64";
            this.btnLimpiarBase64.Size = new System.Drawing.Size(88, 23);
            this.btnLimpiarBase64.TabIndex = 12;
            this.btnLimpiarBase64.Text = "Limpiar Datos Base64";
            this.btnLimpiarBase64.UseVisualStyleBackColor = true;
            this.btnLimpiarBase64.Click += new System.EventHandler(this.btnLimpiarBase64_Click);
            // 
            // cbTipoEncriptacion
            // 
            this.cbTipoEncriptacion.DisplayMember = "TipoEncriptacion";
            this.cbTipoEncriptacion.FormattingEnabled = true;
            this.cbTipoEncriptacion.Location = new System.Drawing.Point(134, 13);
            this.cbTipoEncriptacion.Name = "cbTipoEncriptacion";
            this.cbTipoEncriptacion.Size = new System.Drawing.Size(151, 21);
            this.cbTipoEncriptacion.TabIndex = 13;
            this.cbTipoEncriptacion.ValueMember = "IdTipoEncriptacion";
            // 
            // lblTipoEncriptacion
            // 
            this.lblTipoEncriptacion.AutoSize = true;
            this.lblTipoEncriptacion.Location = new System.Drawing.Point(9, 16);
            this.lblTipoEncriptacion.Name = "lblTipoEncriptacion";
            this.lblTipoEncriptacion.Size = new System.Drawing.Size(105, 13);
            this.lblTipoEncriptacion.TabIndex = 14;
            this.lblTipoEncriptacion.Text = "Tipo de Encriptación";
            // 
            // lblAcccionEncriptacion
            // 
            this.lblAcccionEncriptacion.AutoSize = true;
            this.lblAcccionEncriptacion.Location = new System.Drawing.Point(306, 16);
            this.lblAcccionEncriptacion.Name = "lblAcccionEncriptacion";
            this.lblAcccionEncriptacion.Size = new System.Drawing.Size(49, 13);
            this.lblAcccionEncriptacion.TabIndex = 16;
            this.lblAcccionEncriptacion.Text = "Acccion ";
            // 
            // cbAccion
            // 
            this.cbAccion.DisplayMember = "Accion";
            this.cbAccion.FormattingEnabled = true;
            this.cbAccion.Location = new System.Drawing.Point(431, 13);
            this.cbAccion.Name = "cbAccion";
            this.cbAccion.Size = new System.Drawing.Size(151, 21);
            this.cbAccion.TabIndex = 15;
            this.cbAccion.ValueMember = "IdAccion";
            // 
            // frmEncriptacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 377);
            this.Controls.Add(this.lblAcccionEncriptacion);
            this.Controls.Add(this.cbAccion);
            this.Controls.Add(this.lblTipoEncriptacion);
            this.Controls.Add(this.cbTipoEncriptacion);
            this.Controls.Add(this.btnLimpiarBase64);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.lblFuente);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.txtFuente);
            this.Controls.Add(this.btnGenerar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmEncriptacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Encriptacion";
            this.Load += new System.EventHandler(this.frmEncriptacion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.TextBox txtFuente;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Label lblFuente;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Button btnLimpiarBase64;
        private System.Windows.Forms.ComboBox cbTipoEncriptacion;
        private System.Windows.Forms.Label lblTipoEncriptacion;
        private System.Windows.Forms.Label lblAcccionEncriptacion;
        private System.Windows.Forms.ComboBox cbAccion;
    }
}