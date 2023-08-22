namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmGenerarSello
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
            this.pfdCadenaOriginal = new System.Windows.Forms.OpenFileDialog();
            this.ofdXml = new System.Windows.Forms.OpenFileDialog();
            this.ofdSello = new System.Windows.Forms.OpenFileDialog();
            this.txtArchivoBIN = new System.Windows.Forms.TextBox();
            this.txtArchivoComprobante = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblBin = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(245, 152);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(75, 23);
            this.btnGenerar.TabIndex = 0;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // pfdCadenaOriginal
            // 
            this.pfdCadenaOriginal.FileName = "openFileDialog1";
            // 
            // ofdXml
            // 
            this.ofdXml.FileName = "openFileDialog1";
            // 
            // ofdSello
            // 
            this.ofdSello.FileName = "openFileDialog1";
            // 
            // txtArchivoBIN
            // 
            this.txtArchivoBIN.Location = new System.Drawing.Point(12, 41);
            this.txtArchivoBIN.Name = "txtArchivoBIN";
            this.txtArchivoBIN.Size = new System.Drawing.Size(308, 20);
            this.txtArchivoBIN.TabIndex = 1;
            // 
            // txtArchivoComprobante
            // 
            this.txtArchivoComprobante.Location = new System.Drawing.Point(15, 101);
            this.txtArchivoComprobante.Name = "txtArchivoComprobante";
            this.txtArchivoComprobante.Size = new System.Drawing.Size(308, 20);
            this.txtArchivoComprobante.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(329, 99);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // lblBin
            // 
            this.lblBin.AutoSize = true;
            this.lblBin.Location = new System.Drawing.Point(12, 23);
            this.lblBin.Name = "lblBin";
            this.lblBin.Size = new System.Drawing.Size(64, 13);
            this.lblBin.TabIndex = 5;
            this.lblBin.Text = "Archivo BIN";
            // 
            // frmGenerarSello
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 262);
            this.Controls.Add(this.lblBin);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtArchivoComprobante);
            this.Controls.Add(this.txtArchivoBIN);
            this.Controls.Add(this.btnGenerar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmGenerarSello";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GenerarSello";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.OpenFileDialog pfdCadenaOriginal;
        private System.Windows.Forms.OpenFileDialog ofdXml;
        private System.Windows.Forms.OpenFileDialog ofdSello;
        private System.Windows.Forms.TextBox txtArchivoBIN;
        private System.Windows.Forms.TextBox txtArchivoComprobante;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblBin;
    }
}