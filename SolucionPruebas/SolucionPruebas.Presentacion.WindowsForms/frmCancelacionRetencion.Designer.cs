namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmCancelacionRetencion
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
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.lblUUID = new System.Windows.Forms.Label();
            this.lblRFC = new System.Windows.Forms.Label();
            this.txtRFC = new System.Windows.Forms.TextBox();
            this.cbTest = new System.Windows.Forms.CheckBox();
            this.cbPreproductivo = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtUUID
            // 
            this.txtUUID.Location = new System.Drawing.Point(26, 29);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.Size = new System.Drawing.Size(243, 20);
            this.txtUUID.TabIndex = 0;
            // 
            // lblUUID
            // 
            this.lblUUID.AutoSize = true;
            this.lblUUID.Location = new System.Drawing.Point(23, 13);
            this.lblUUID.Name = "lblUUID";
            this.lblUUID.Size = new System.Drawing.Size(34, 13);
            this.lblUUID.TabIndex = 1;
            this.lblUUID.Text = "UUID";
            // 
            // lblRFC
            // 
            this.lblRFC.AutoSize = true;
            this.lblRFC.Location = new System.Drawing.Point(26, 56);
            this.lblRFC.Name = "lblRFC";
            this.lblRFC.Size = new System.Drawing.Size(28, 13);
            this.lblRFC.TabIndex = 2;
            this.lblRFC.Text = "RFC";
            // 
            // txtRFC
            // 
            this.txtRFC.Location = new System.Drawing.Point(26, 73);
            this.txtRFC.Name = "txtRFC";
            this.txtRFC.Size = new System.Drawing.Size(128, 20);
            this.txtRFC.TabIndex = 3;
            // 
            // cbTest
            // 
            this.cbTest.AutoSize = true;
            this.cbTest.Location = new System.Drawing.Point(308, 29);
            this.cbTest.Name = "cbTest";
            this.cbTest.Size = new System.Drawing.Size(47, 17);
            this.cbTest.TabIndex = 4;
            this.cbTest.Text = "Test";
            this.cbTest.UseVisualStyleBackColor = true;
            this.cbTest.CheckedChanged += new System.EventHandler(this.cbTest_CheckedChanged);
            // 
            // cbPreproductivo
            // 
            this.cbPreproductivo.AutoSize = true;
            this.cbPreproductivo.Location = new System.Drawing.Point(308, 51);
            this.cbPreproductivo.Name = "cbPreproductivo";
            this.cbPreproductivo.Size = new System.Drawing.Size(96, 17);
            this.cbPreproductivo.TabIndex = 5;
            this.cbPreproductivo.Text = "Pre-Productivo";
            this.cbPreproductivo.UseVisualStyleBackColor = true;
            this.cbPreproductivo.CheckedChanged += new System.EventHandler(this.cbPreproductivo_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(308, 73);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(77, 17);
            this.checkBox3.TabIndex = 6;
            this.checkBox3.Text = "Productivo";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(308, 108);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(26, 153);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(378, 197);
            this.textBox1.TabIndex = 8;
            // 
            // frmCancelacionRetencion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 370);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.cbPreproductivo);
            this.Controls.Add(this.cbTest);
            this.Controls.Add(this.txtRFC);
            this.Controls.Add(this.lblRFC);
            this.Controls.Add(this.lblUUID);
            this.Controls.Add(this.txtUUID);
            this.Name = "frmCancelacionRetencion";
            this.Text = "frmCancelacionRetencion";
            this.Load += new System.EventHandler(this.frmCancelacionRetencion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUUID;
        private System.Windows.Forms.Label lblUUID;
        private System.Windows.Forms.Label lblRFC;
        private System.Windows.Forms.TextBox txtRFC;
        private System.Windows.Forms.CheckBox cbTest;
        private System.Windows.Forms.CheckBox cbPreproductivo;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox textBox1;
    }
}