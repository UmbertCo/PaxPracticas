namespace DevExpressSample
{
    partial class frmDevExpressReporting
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
            this.btnCC = new System.Windows.Forms.Button();
            this.btnPR = new System.Windows.Forms.Button();
            this.btnDP = new System.Windows.Forms.Button();
            this.btnProfecoNomina = new System.Windows.Forms.Button();
            this.btnProfecoFactura = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCC
            // 
            this.btnCC.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCC.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCC.Location = new System.Drawing.Point(0, 0);
            this.btnCC.Margin = new System.Windows.Forms.Padding(4);
            this.btnCC.Name = "btnCC";
            this.btnCC.Size = new System.Drawing.Size(522, 50);
            this.btnCC.TabIndex = 0;
            this.btnCC.Text = "Exportar CC";
            this.btnCC.UseVisualStyleBackColor = true;
            this.btnCC.Click += new System.EventHandler(this.btnCC_Click);
            // 
            // btnPR
            // 
            this.btnPR.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPR.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPR.Location = new System.Drawing.Point(0, 50);
            this.btnPR.Margin = new System.Windows.Forms.Padding(4);
            this.btnPR.Name = "btnPR";
            this.btnPR.Size = new System.Drawing.Size(522, 50);
            this.btnPR.TabIndex = 1;
            this.btnPR.Text = "Exportar PR";
            this.btnPR.UseVisualStyleBackColor = true;
            this.btnPR.Click += new System.EventHandler(this.btnPR_Click);
            // 
            // btnDP
            // 
            this.btnDP.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDP.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDP.Location = new System.Drawing.Point(0, 100);
            this.btnDP.Margin = new System.Windows.Forms.Padding(4);
            this.btnDP.Name = "btnDP";
            this.btnDP.Size = new System.Drawing.Size(522, 50);
            this.btnDP.TabIndex = 2;
            this.btnDP.Text = "Exportar DP";
            this.btnDP.UseVisualStyleBackColor = true;
            this.btnDP.Click += new System.EventHandler(this.btnDP_Click);
            // 
            // btnProfecoNomina
            // 
            this.btnProfecoNomina.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProfecoNomina.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProfecoNomina.Location = new System.Drawing.Point(0, 150);
            this.btnProfecoNomina.Margin = new System.Windows.Forms.Padding(4);
            this.btnProfecoNomina.Name = "btnProfecoNomina";
            this.btnProfecoNomina.Size = new System.Drawing.Size(522, 50);
            this.btnProfecoNomina.TabIndex = 3;
            this.btnProfecoNomina.Text = "Exportar Profeco Nomina";
            this.btnProfecoNomina.UseVisualStyleBackColor = true;
            this.btnProfecoNomina.Click += new System.EventHandler(this.btnProfecoNomina_Click);
            // 
            // btnProfecoFactura
            // 
            this.btnProfecoFactura.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProfecoFactura.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProfecoFactura.Location = new System.Drawing.Point(0, 200);
            this.btnProfecoFactura.Margin = new System.Windows.Forms.Padding(4);
            this.btnProfecoFactura.Name = "btnProfecoFactura";
            this.btnProfecoFactura.Size = new System.Drawing.Size(522, 50);
            this.btnProfecoFactura.TabIndex = 4;
            this.btnProfecoFactura.Text = "Exportar Profeco Factura";
            this.btnProfecoFactura.UseVisualStyleBackColor = true;
            this.btnProfecoFactura.Click += new System.EventHandler(this.btnProfecoFactura_Click);
            // 
            // frmDevExpressReporting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 251);
            this.Controls.Add(this.btnProfecoFactura);
            this.Controls.Add(this.btnProfecoNomina);
            this.Controls.Add(this.btnDP);
            this.Controls.Add(this.btnPR);
            this.Controls.Add(this.btnCC);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmDevExpressReporting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sample DevExpress Reporting";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCC;
        private System.Windows.Forms.Button btnPR;
        private System.Windows.Forms.Button btnDP;
        private System.Windows.Forms.Button btnProfecoNomina;
        private System.Windows.Forms.Button btnProfecoFactura;


    }
}

