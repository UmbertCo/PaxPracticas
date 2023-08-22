namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmLCO
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
            this.txtSoap = new System.Windows.Forms.TextBox();
            this.txtRespuesta = new System.Windows.Forms.TextBox();
            this.lblEncabezado = new System.Windows.Forms.Label();
            this.lblSoap = new System.Windows.Forms.Label();
            this.lblRespuesta = new System.Windows.Forms.Label();
            this.btnConsumirLCO = new System.Windows.Forms.Button();
            this.txtEncabezado = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtSoap
            // 
            this.txtSoap.Location = new System.Drawing.Point(12, 158);
            this.txtSoap.Multiline = true;
            this.txtSoap.Name = "txtSoap";
            this.txtSoap.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSoap.Size = new System.Drawing.Size(627, 155);
            this.txtSoap.TabIndex = 1;
            // 
            // txtRespuesta
            // 
            this.txtRespuesta.Location = new System.Drawing.Point(12, 351);
            this.txtRespuesta.Multiline = true;
            this.txtRespuesta.Name = "txtRespuesta";
            this.txtRespuesta.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRespuesta.Size = new System.Drawing.Size(627, 105);
            this.txtRespuesta.TabIndex = 2;
            // 
            // lblEncabezado
            // 
            this.lblEncabezado.AutoSize = true;
            this.lblEncabezado.Location = new System.Drawing.Point(12, 8);
            this.lblEncabezado.Name = "lblEncabezado";
            this.lblEncabezado.Size = new System.Drawing.Size(67, 13);
            this.lblEncabezado.TabIndex = 3;
            this.lblEncabezado.Text = "Encabezado";
            // 
            // lblSoap
            // 
            this.lblSoap.AutoSize = true;
            this.lblSoap.Location = new System.Drawing.Point(12, 142);
            this.lblSoap.Name = "lblSoap";
            this.lblSoap.Size = new System.Drawing.Size(32, 13);
            this.lblSoap.TabIndex = 4;
            this.lblSoap.Text = "Soap";
            // 
            // lblRespuesta
            // 
            this.lblRespuesta.AutoSize = true;
            this.lblRespuesta.Location = new System.Drawing.Point(12, 335);
            this.lblRespuesta.Name = "lblRespuesta";
            this.lblRespuesta.Size = new System.Drawing.Size(58, 13);
            this.lblRespuesta.TabIndex = 5;
            this.lblRespuesta.Text = "Respuesta";
            // 
            // btnConsumirLCO
            // 
            this.btnConsumirLCO.Location = new System.Drawing.Point(510, 319);
            this.btnConsumirLCO.Name = "btnConsumirLCO";
            this.btnConsumirLCO.Size = new System.Drawing.Size(129, 23);
            this.btnConsumirLCO.TabIndex = 6;
            this.btnConsumirLCO.Text = "Consumir LCO";
            this.btnConsumirLCO.UseVisualStyleBackColor = true;
            this.btnConsumirLCO.Click += new System.EventHandler(this.btnConsumirLCO_Click);
            // 
            // txtEncabezado
            // 
            this.txtEncabezado.Location = new System.Drawing.Point(12, 24);
            this.txtEncabezado.Multiline = true;
            this.txtEncabezado.Name = "txtEncabezado";
            this.txtEncabezado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEncabezado.Size = new System.Drawing.Size(358, 103);
            this.txtEncabezado.TabIndex = 0;
            // 
            // frmLCO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 468);
            this.Controls.Add(this.btnConsumirLCO);
            this.Controls.Add(this.lblRespuesta);
            this.Controls.Add(this.lblSoap);
            this.Controls.Add(this.lblEncabezado);
            this.Controls.Add(this.txtRespuesta);
            this.Controls.Add(this.txtSoap);
            this.Controls.Add(this.txtEncabezado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLCO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLCO";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSoap;
        private System.Windows.Forms.TextBox txtRespuesta;
        private System.Windows.Forms.Label lblEncabezado;
        private System.Windows.Forms.Label lblSoap;
        private System.Windows.Forms.Label lblRespuesta;
        private System.Windows.Forms.Button btnConsumirLCO;
        private System.Windows.Forms.TextBox txtEncabezado;
    }
}