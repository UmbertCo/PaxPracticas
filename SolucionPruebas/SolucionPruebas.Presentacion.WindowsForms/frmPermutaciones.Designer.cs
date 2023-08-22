namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmPermutaciones
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
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.btnGenerarConsultas = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(12, 137);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.Size = new System.Drawing.Size(609, 502);
            this.txtResultado.TabIndex = 0;
            // 
            // btnGenerarConsultas
            // 
            this.btnGenerarConsultas.Location = new System.Drawing.Point(494, 87);
            this.btnGenerarConsultas.Name = "btnGenerarConsultas";
            this.btnGenerarConsultas.Size = new System.Drawing.Size(127, 23);
            this.btnGenerarConsultas.TabIndex = 1;
            this.btnGenerarConsultas.Text = "Permutar parametros";
            this.btnGenerarConsultas.UseVisualStyleBackColor = true;
            this.btnGenerarConsultas.Click += new System.EventHandler(this.btnGenerarConsultas_Click);
            // 
            // frmPermutaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 651);
            this.Controls.Add(this.btnGenerarConsultas);
            this.Controls.Add(this.txtResultado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPermutaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Permutaciones Consulta";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Button btnGenerarConsultas;
    }
}