namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmManifiesto
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
            this.btnEnviarCartaManifiesto = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.btnEnviarXML = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEnviarCartaManifiesto
            // 
            this.btnEnviarCartaManifiesto.Location = new System.Drawing.Point(298, 142);
            this.btnEnviarCartaManifiesto.Name = "btnEnviarCartaManifiesto";
            this.btnEnviarCartaManifiesto.Size = new System.Drawing.Size(97, 23);
            this.btnEnviarCartaManifiesto.TabIndex = 0;
            this.btnEnviarCartaManifiesto.Text = "Enviar Datos";
            this.btnEnviarCartaManifiesto.UseVisualStyleBackColor = true;
            this.btnEnviarCartaManifiesto.Click += new System.EventHandler(this.btnEnviarCartaManifiesto_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(41, 171);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.Size = new System.Drawing.Size(432, 176);
            this.txtResultado.TabIndex = 1;
            // 
            // btnEnviarXML
            // 
            this.btnEnviarXML.Location = new System.Drawing.Point(402, 142);
            this.btnEnviarXML.Name = "btnEnviarXML";
            this.btnEnviarXML.Size = new System.Drawing.Size(75, 23);
            this.btnEnviarXML.TabIndex = 2;
            this.btnEnviarXML.Text = "Enviar XML";
            this.btnEnviarXML.UseVisualStyleBackColor = true;
            this.btnEnviarXML.Click += new System.EventHandler(this.btnEnviarXML_Click);
            // 
            // frmManifiesto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 368);
            this.Controls.Add(this.btnEnviarXML);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btnEnviarCartaManifiesto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmManifiesto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmManifiesto";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEnviarCartaManifiesto;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Button btnEnviarXML;
    }
}