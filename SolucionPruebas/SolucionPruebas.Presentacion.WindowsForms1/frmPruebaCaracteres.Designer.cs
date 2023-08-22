namespace SolucionPruebas.Presentacion.WindowsForms1
{
    partial class frmPruebaCaracteres
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
            this.txtCaracter = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtCaracter
            // 
            this.txtCaracter.Location = new System.Drawing.Point(50, 65);
            this.txtCaracter.Name = "txtCaracter";
            this.txtCaracter.Size = new System.Drawing.Size(100, 20);
            this.txtCaracter.TabIndex = 0;
            // 
            // frmPruebaCaracteres
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.txtCaracter);
            this.Name = "frmPruebaCaracteres";
            this.Text = "frmPruebaCaracteres";
            this.Load += new System.EventHandler(this.frmPruebaCaracteres_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCaracter;
    }
}