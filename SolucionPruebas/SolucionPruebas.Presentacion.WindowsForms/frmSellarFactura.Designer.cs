﻿namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmSellarFactura
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
            this.btnSellarFactura = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSellarFactura
            // 
            this.btnSellarFactura.Location = new System.Drawing.Point(332, 318);
            this.btnSellarFactura.Name = "btnSellarFactura";
            this.btnSellarFactura.Size = new System.Drawing.Size(104, 23);
            this.btnSellarFactura.TabIndex = 0;
            this.btnSellarFactura.Text = "Sellar Factura";
            this.btnSellarFactura.UseVisualStyleBackColor = true;
            this.btnSellarFactura.Click += new System.EventHandler(this.btnSellarFactura_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(12, 12);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(424, 300);
            this.txtResultado.TabIndex = 1;
            // 
            // frmSellarFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 353);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btnSellarFactura);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSellarFactura";
            this.Text = "Sellar Factura";
            this.Load += new System.EventHandler(this.frmSellarFactura_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSellarFactura;
        private System.Windows.Forms.TextBox txtResultado;
    }
}