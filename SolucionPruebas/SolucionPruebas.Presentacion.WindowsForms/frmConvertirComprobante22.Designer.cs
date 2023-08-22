namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmConvertirComprobante22
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Convertir = new System.Windows.Forms.Button();
            this.btnGenerar22 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Convertir
            // 
            this.Convertir.Location = new System.Drawing.Point(100, 53);
            this.Convertir.Name = "Convertir";
            this.Convertir.Size = new System.Drawing.Size(75, 23);
            this.Convertir.TabIndex = 0;
            this.Convertir.Text = "btnConvertir";
            this.Convertir.UseVisualStyleBackColor = true;
            this.Convertir.Click += new System.EventHandler(this.Convertir_Click);
            // 
            // btnGenerar22
            // 
            this.btnGenerar22.Location = new System.Drawing.Point(100, 96);
            this.btnGenerar22.Name = "btnGenerar22";
            this.btnGenerar22.Size = new System.Drawing.Size(75, 23);
            this.btnGenerar22.TabIndex = 1;
            this.btnGenerar22.Text = "Generar 2.2";
            this.btnGenerar22.UseVisualStyleBackColor = true;
            this.btnGenerar22.Click += new System.EventHandler(this.btnGenerar22_Click);
            // 
            // frmConvertirComprobante22
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnGenerar22);
            this.Controls.Add(this.Convertir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmConvertirComprobante22";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Convertir Comprobante 22";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button Convertir;
        private System.Windows.Forms.Button btnGenerar22;
    }
}