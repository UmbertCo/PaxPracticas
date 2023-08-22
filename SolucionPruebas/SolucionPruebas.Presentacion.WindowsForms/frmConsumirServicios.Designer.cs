namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmConsumirServicios
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
            this.btnConsumirServicioJson = new System.Windows.Forms.Button();
            this.txtFolio = new System.Windows.Forms.TextBox();
            this.lblNumeroFolio = new System.Windows.Forms.Label();
            this.txtResultados = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConsumirServicioJson
            // 
            this.btnConsumirServicioJson.Location = new System.Drawing.Point(160, 46);
            this.btnConsumirServicioJson.Name = "btnConsumirServicioJson";
            this.btnConsumirServicioJson.Size = new System.Drawing.Size(101, 23);
            this.btnConsumirServicioJson.TabIndex = 0;
            this.btnConsumirServicioJson.Text = "Servicio Json";
            this.btnConsumirServicioJson.UseVisualStyleBackColor = true;
            this.btnConsumirServicioJson.Click += new System.EventHandler(this.btnConsumirServicioJson_Click);
            // 
            // txtFolio
            // 
            this.txtFolio.Location = new System.Drawing.Point(12, 46);
            this.txtFolio.Name = "txtFolio";
            this.txtFolio.Size = new System.Drawing.Size(100, 20);
            this.txtFolio.TabIndex = 1;
            // 
            // lblNumeroFolio
            // 
            this.lblNumeroFolio.AutoSize = true;
            this.lblNumeroFolio.Location = new System.Drawing.Point(9, 20);
            this.lblNumeroFolio.Name = "lblNumeroFolio";
            this.lblNumeroFolio.Size = new System.Drawing.Size(29, 13);
            this.lblNumeroFolio.TabIndex = 2;
            this.lblNumeroFolio.Text = "Folio";
            // 
            // txtResultados
            // 
            this.txtResultados.Location = new System.Drawing.Point(12, 95);
            this.txtResultados.Multiline = true;
            this.txtResultados.Name = "txtResultados";
            this.txtResultados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultados.Size = new System.Drawing.Size(576, 254);
            this.txtResultados.TabIndex = 3;
            // 
            // frmConsumirServicios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 361);
            this.Controls.Add(this.txtResultados);
            this.Controls.Add(this.lblNumeroFolio);
            this.Controls.Add(this.txtFolio);
            this.Controls.Add(this.btnConsumirServicioJson);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmConsumirServicios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consumir Servicios";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConsumirServicioJson;
        private System.Windows.Forms.TextBox txtFolio;
        private System.Windows.Forms.Label lblNumeroFolio;
        private System.Windows.Forms.TextBox txtResultados;
    }
}