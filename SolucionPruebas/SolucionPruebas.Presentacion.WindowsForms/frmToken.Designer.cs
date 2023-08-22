namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmToken
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
            this.btnConsumirServicio = new System.Windows.Forms.Button();
            this.btnConsumirServicioToken = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConsumirServicio
            // 
            this.btnConsumirServicio.Location = new System.Drawing.Point(79, 45);
            this.btnConsumirServicio.Name = "btnConsumirServicio";
            this.btnConsumirServicio.Size = new System.Drawing.Size(75, 35);
            this.btnConsumirServicio.TabIndex = 0;
            this.btnConsumirServicio.Text = "Consumir Servicio";
            this.btnConsumirServicio.UseVisualStyleBackColor = true;
            this.btnConsumirServicio.Click += new System.EventHandler(this.btnConsumirServicio_Click);
            // 
            // btnConsumirServicioToken
            // 
            this.btnConsumirServicioToken.Location = new System.Drawing.Point(161, 45);
            this.btnConsumirServicioToken.Name = "btnConsumirServicioToken";
            this.btnConsumirServicioToken.Size = new System.Drawing.Size(99, 35);
            this.btnConsumirServicioToken.TabIndex = 1;
            this.btnConsumirServicioToken.Text = "Consumir Servicio Token";
            this.btnConsumirServicioToken.UseVisualStyleBackColor = true;
            this.btnConsumirServicioToken.Click += new System.EventHandler(this.btnConsumirServicioToken_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(13, 97);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.Size = new System.Drawing.Size(319, 152);
            this.txtResultado.TabIndex = 2;
            // 
            // frmToken
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 261);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btnConsumirServicioToken);
            this.Controls.Add(this.btnConsumirServicio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmToken";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmToken";
            this.Load += new System.EventHandler(this.frmToken_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConsumirServicio;
        private System.Windows.Forms.Button btnConsumirServicioToken;
        private System.Windows.Forms.TextBox txtResultado;
    }
}