namespace DevXpressIsmael
{
    partial class frmEjemploDevXpress
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
            this.btnVerPDF = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnVerPDF
            // 
            this.btnVerPDF.Location = new System.Drawing.Point(89, 79);
            this.btnVerPDF.Name = "btnVerPDF";
            this.btnVerPDF.Size = new System.Drawing.Size(75, 23);
            this.btnVerPDF.TabIndex = 0;
            this.btnVerPDF.Text = "Ver PDF";
            this.btnVerPDF.UseVisualStyleBackColor = true;
            this.btnVerPDF.Click += new System.EventHandler(this.btnVerPDF_Click);
            // 
            // frmEjemploDevXpress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 261);
            this.Controls.Add(this.btnVerPDF);
            this.Name = "frmEjemploDevXpress";
            this.Text = "Ejemplo DevXpress";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnVerPDF;
    }
}

