namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmAcusePAC
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
            this.txtCadena = new System.Windows.Forms.TextBox();
            this.btnBase64Cadena = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.btnCadenaBase64 = new System.Windows.Forms.Button();
            this.lblOrigen = new System.Windows.Forms.Label();
            this.lblResultado = new System.Windows.Forms.Label();
            this.btnCadenaHash = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtCadena
            // 
            this.txtCadena.Location = new System.Drawing.Point(12, 33);
            this.txtCadena.Multiline = true;
            this.txtCadena.Name = "txtCadena";
            this.txtCadena.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCadena.Size = new System.Drawing.Size(439, 158);
            this.txtCadena.TabIndex = 9;
            // 
            // btnBase64Cadena
            // 
            this.btnBase64Cadena.Location = new System.Drawing.Point(474, 77);
            this.btnBase64Cadena.Name = "btnBase64Cadena";
            this.btnBase64Cadena.Size = new System.Drawing.Size(101, 23);
            this.btnBase64Cadena.TabIndex = 8;
            this.btnBase64Cadena.Text = "Base64 a Cadena";
            this.btnBase64Cadena.UseVisualStyleBackColor = true;
            this.btnBase64Cadena.Click += new System.EventHandler(this.btnBase64Cadena_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(12, 224);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(739, 215);
            this.txtResultado.TabIndex = 7;
            // 
            // btnCadenaBase64
            // 
            this.btnCadenaBase64.Location = new System.Drawing.Point(474, 37);
            this.btnCadenaBase64.Name = "btnCadenaBase64";
            this.btnCadenaBase64.Size = new System.Drawing.Size(101, 23);
            this.btnCadenaBase64.TabIndex = 10;
            this.btnCadenaBase64.Text = "Cadena a Base64";
            this.btnCadenaBase64.UseVisualStyleBackColor = true;
            this.btnCadenaBase64.Click += new System.EventHandler(this.btnCadenaBase64_Click);
            // 
            // lblOrigen
            // 
            this.lblOrigen.AutoSize = true;
            this.lblOrigen.Location = new System.Drawing.Point(12, 13);
            this.lblOrigen.Name = "lblOrigen";
            this.lblOrigen.Size = new System.Drawing.Size(38, 13);
            this.lblOrigen.TabIndex = 11;
            this.lblOrigen.Text = "Origen";
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(15, 205);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(55, 13);
            this.lblResultado.TabIndex = 12;
            this.lblResultado.Text = "Resultado";
            // 
            // btnCadenaHash
            // 
            this.btnCadenaHash.Location = new System.Drawing.Point(474, 119);
            this.btnCadenaHash.Name = "btnCadenaHash";
            this.btnCadenaHash.Size = new System.Drawing.Size(101, 23);
            this.btnCadenaHash.TabIndex = 13;
            this.btnCadenaHash.Text = "Cadena a Hash";
            this.btnCadenaHash.UseVisualStyleBackColor = true;
            this.btnCadenaHash.Click += new System.EventHandler(this.btnCadenaHash_Click);
            // 
            // frmAcusePAC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 451);
            this.Controls.Add(this.btnCadenaHash);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.lblOrigen);
            this.Controls.Add(this.btnCadenaBase64);
            this.Controls.Add(this.txtCadena);
            this.Controls.Add(this.btnBase64Cadena);
            this.Controls.Add(this.txtResultado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAcusePAC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Acuse PAC";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCadena;
        private System.Windows.Forms.Button btnBase64Cadena;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Button btnCadenaBase64;
        private System.Windows.Forms.Label lblOrigen;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Button btnCadenaHash;
    }
}