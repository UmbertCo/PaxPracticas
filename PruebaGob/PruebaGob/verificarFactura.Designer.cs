namespace PruebaGob
{
    partial class verificarFactura
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtPeriodo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNoOperacion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtImporteRecibo = new System.Windows.Forms.TextBox();
            this.btnverificarFactura = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Periodo";
            // 
            // txtPeriodo
            // 
            this.txtPeriodo.Location = new System.Drawing.Point(15, 47);
            this.txtPeriodo.Name = "txtPeriodo";
            this.txtPeriodo.Size = new System.Drawing.Size(232, 20);
            this.txtPeriodo.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "No. Operación";
            // 
            // txtNoOperacion
            // 
            this.txtNoOperacion.Location = new System.Drawing.Point(15, 107);
            this.txtNoOperacion.Name = "txtNoOperacion";
            this.txtNoOperacion.Size = new System.Drawing.Size(232, 20);
            this.txtNoOperacion.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Importe del recibo";
            // 
            // txtImporteRecibo
            // 
            this.txtImporteRecibo.Location = new System.Drawing.Point(15, 166);
            this.txtImporteRecibo.Name = "txtImporteRecibo";
            this.txtImporteRecibo.Size = new System.Drawing.Size(232, 20);
            this.txtImporteRecibo.TabIndex = 7;
            // 
            // btnverificarFactura
            // 
            this.btnverificarFactura.Location = new System.Drawing.Point(293, 30);
            this.btnverificarFactura.Name = "btnverificarFactura";
            this.btnverificarFactura.Size = new System.Drawing.Size(159, 50);
            this.btnverificarFactura.TabIndex = 8;
            this.btnverificarFactura.Text = "verificarFactura";
            this.btnverificarFactura.UseVisualStyleBackColor = true;
            this.btnverificarFactura.Click += new System.EventHandler(this.btnverificarFactura_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Enabled = false;
            this.txtResultado.Location = new System.Drawing.Point(293, 119);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.Size = new System.Drawing.Size(686, 216);
            this.txtResultado.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(290, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Resultado";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(484, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Método que genera la factura";
            // 
            // verificarFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 347);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btnverificarFactura);
            this.Controls.Add(this.txtImporteRecibo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNoOperacion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPeriodo);
            this.Controls.Add(this.label2);
            this.Name = "verificarFactura";
            this.Text = "Verificar Factura";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.verificarFactura_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPeriodo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNoOperacion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtImporteRecibo;
        private System.Windows.Forms.Button btnverificarFactura;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

