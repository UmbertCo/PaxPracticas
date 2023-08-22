namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmZip
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
            this.btnGenerarZip = new System.Windows.Forms.Button();
            this.txtArchivo1 = new System.Windows.Forms.TextBox();
            this.txtArchivo2 = new System.Windows.Forms.TextBox();
            this.ofdArchivo = new System.Windows.Forms.OpenFileDialog();
            this.btnArchivo1 = new System.Windows.Forms.Button();
            this.btnAchivo2 = new System.Windows.Forms.Button();
            this.fbdArchivo = new System.Windows.Forms.FolderBrowserDialog();
            this.lbArchivos = new System.Windows.Forms.ListBox();
            this.btnGenerarZipChilkat = new System.Windows.Forms.Button();
            this.btnChilkatZip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGenerarZip
            // 
            this.btnGenerarZip.Location = new System.Drawing.Point(36, 204);
            this.btnGenerarZip.Name = "btnGenerarZip";
            this.btnGenerarZip.Size = new System.Drawing.Size(146, 23);
            this.btnGenerarZip.TabIndex = 0;
            this.btnGenerarZip.Text = "Generar Zip ICSharpCode";
            this.btnGenerarZip.UseVisualStyleBackColor = true;
            this.btnGenerarZip.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtArchivo1
            // 
            this.txtArchivo1.Location = new System.Drawing.Point(36, 27);
            this.txtArchivo1.Name = "txtArchivo1";
            this.txtArchivo1.Size = new System.Drawing.Size(434, 20);
            this.txtArchivo1.TabIndex = 1;
            // 
            // txtArchivo2
            // 
            this.txtArchivo2.Location = new System.Drawing.Point(36, 51);
            this.txtArchivo2.Name = "txtArchivo2";
            this.txtArchivo2.Size = new System.Drawing.Size(434, 20);
            this.txtArchivo2.TabIndex = 2;
            // 
            // ofdArchivo
            // 
            this.ofdArchivo.FileName = "openFileDialog1";
            // 
            // btnArchivo1
            // 
            this.btnArchivo1.Location = new System.Drawing.Point(476, 24);
            this.btnArchivo1.Name = "btnArchivo1";
            this.btnArchivo1.Size = new System.Drawing.Size(97, 23);
            this.btnArchivo1.TabIndex = 3;
            this.btnArchivo1.Text = "URL archivo 1";
            this.btnArchivo1.UseVisualStyleBackColor = true;
            this.btnArchivo1.Click += new System.EventHandler(this.btnArchivo1_Click);
            // 
            // btnAchivo2
            // 
            this.btnAchivo2.Location = new System.Drawing.Point(477, 51);
            this.btnAchivo2.Name = "btnAchivo2";
            this.btnAchivo2.Size = new System.Drawing.Size(96, 23);
            this.btnAchivo2.TabIndex = 4;
            this.btnAchivo2.Text = "URL archivo 2";
            this.btnAchivo2.UseVisualStyleBackColor = true;
            this.btnAchivo2.Click += new System.EventHandler(this.btnAchivo2_Click);
            // 
            // lbArchivos
            // 
            this.lbArchivos.FormattingEnabled = true;
            this.lbArchivos.Location = new System.Drawing.Point(36, 88);
            this.lbArchivos.Name = "lbArchivos";
            this.lbArchivos.ScrollAlwaysVisible = true;
            this.lbArchivos.Size = new System.Drawing.Size(869, 95);
            this.lbArchivos.TabIndex = 5;
            // 
            // btnGenerarZipChilkat
            // 
            this.btnGenerarZipChilkat.Location = new System.Drawing.Point(188, 204);
            this.btnGenerarZipChilkat.Name = "btnGenerarZipChilkat";
            this.btnGenerarZipChilkat.Size = new System.Drawing.Size(75, 23);
            this.btnGenerarZipChilkat.TabIndex = 6;
            this.btnGenerarZipChilkat.Text = "Zip Chilkat";
            this.btnGenerarZipChilkat.UseVisualStyleBackColor = true;
            this.btnGenerarZipChilkat.Click += new System.EventHandler(this.btnGenerarZipChilkat_Click);
            // 
            // btnChilkatZip
            // 
            this.btnChilkatZip.Location = new System.Drawing.Point(269, 204);
            this.btnChilkatZip.Name = "btnChilkatZip";
            this.btnChilkatZip.Size = new System.Drawing.Size(75, 23);
            this.btnChilkatZip.TabIndex = 7;
            this.btnChilkatZip.Text = "Zip";
            this.btnChilkatZip.UseVisualStyleBackColor = true;
            this.btnChilkatZip.Click += new System.EventHandler(this.btnChilkatZip_Click);
            // 
            // frmZip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 262);
            this.Controls.Add(this.btnChilkatZip);
            this.Controls.Add(this.btnGenerarZipChilkat);
            this.Controls.Add(this.lbArchivos);
            this.Controls.Add(this.btnAchivo2);
            this.Controls.Add(this.btnArchivo1);
            this.Controls.Add(this.txtArchivo2);
            this.Controls.Add(this.txtArchivo1);
            this.Controls.Add(this.btnGenerarZip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmZip";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zip";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerarZip;
        private System.Windows.Forms.TextBox txtArchivo1;
        private System.Windows.Forms.TextBox txtArchivo2;
        private System.Windows.Forms.OpenFileDialog ofdArchivo;
        private System.Windows.Forms.Button btnArchivo1;
        private System.Windows.Forms.Button btnAchivo2;
        private System.Windows.Forms.FolderBrowserDialog fbdArchivo;
        private System.Windows.Forms.ListBox lbArchivos;
        private System.Windows.Forms.Button btnGenerarZipChilkat;
        private System.Windows.Forms.Button btnChilkatZip;
    }
}