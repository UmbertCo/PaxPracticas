namespace PruebaGob
{
    partial class Principal
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnverificarFactura = new System.Windows.Forms.Button();
            this.btnresponseFactura = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(135, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Métodos";
            // 
            // btnverificarFactura
            // 
            this.btnverificarFactura.Location = new System.Drawing.Point(27, 112);
            this.btnverificarFactura.Name = "btnverificarFactura";
            this.btnverificarFactura.Size = new System.Drawing.Size(139, 80);
            this.btnverificarFactura.TabIndex = 1;
            this.btnverificarFactura.Text = "verificarFactura";
            this.btnverificarFactura.UseVisualStyleBackColor = true;
            this.btnverificarFactura.Click += new System.EventHandler(this.btnverificarFactura_Click);
            // 
            // btnresponseFactura
            // 
            this.btnresponseFactura.Location = new System.Drawing.Point(194, 112);
            this.btnresponseFactura.Name = "btnresponseFactura";
            this.btnresponseFactura.Size = new System.Drawing.Size(139, 80);
            this.btnresponseFactura.TabIndex = 2;
            this.btnresponseFactura.Text = "responseFactura";
            this.btnresponseFactura.UseVisualStyleBackColor = true;
            this.btnresponseFactura.Click += new System.EventHandler(this.btnresponseFactura_Click);
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 261);
            this.Controls.Add(this.btnresponseFactura);
            this.Controls.Add(this.btnverificarFactura);
            this.Controls.Add(this.label1);
            this.Name = "Principal";
            this.Text = "Principal";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Principal_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnverificarFactura;
        private System.Windows.Forms.Button btnresponseFactura;
    }
}