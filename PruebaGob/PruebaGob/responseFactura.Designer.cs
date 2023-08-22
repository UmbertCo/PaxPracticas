namespace PruebaGob
{
    partial class responseFactura
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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.btnresponseFactura = new System.Windows.Forms.Button();
            this.txtImporteRecibo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNoOperacion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPeriodo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSeleccionarXML = new System.Windows.Forms.Button();
            this.txtXML = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbEstatus = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(479, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Método que regresa la factura";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(285, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Resultado";
            // 
            // txtResultado
            // 
            this.txtResultado.Enabled = false;
            this.txtResultado.Location = new System.Drawing.Point(288, 129);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.Size = new System.Drawing.Size(406, 218);
            this.txtResultado.TabIndex = 22;
            // 
            // btnresponseFactura
            // 
            this.btnresponseFactura.Enabled = false;
            this.btnresponseFactura.Location = new System.Drawing.Point(288, 38);
            this.btnresponseFactura.Name = "btnresponseFactura";
            this.btnresponseFactura.Size = new System.Drawing.Size(159, 50);
            this.btnresponseFactura.TabIndex = 21;
            this.btnresponseFactura.Text = "responseFactura";
            this.btnresponseFactura.UseVisualStyleBackColor = true;
            this.btnresponseFactura.Click += new System.EventHandler(this.btnresponseFactura_Click);
            // 
            // txtImporteRecibo
            // 
            this.txtImporteRecibo.Location = new System.Drawing.Point(15, 174);
            this.txtImporteRecibo.Name = "txtImporteRecibo";
            this.txtImporteRecibo.Size = new System.Drawing.Size(232, 20);
            this.txtImporteRecibo.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Importe del recibo";
            // 
            // txtNoOperacion
            // 
            this.txtNoOperacion.Location = new System.Drawing.Point(15, 115);
            this.txtNoOperacion.Name = "txtNoOperacion";
            this.txtNoOperacion.Size = new System.Drawing.Size(232, 20);
            this.txtNoOperacion.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "No. Operación";
            // 
            // txtPeriodo
            // 
            this.txtPeriodo.Location = new System.Drawing.Point(15, 55);
            this.txtPeriodo.Name = "txtPeriodo";
            this.txtPeriodo.Size = new System.Drawing.Size(232, 20);
            this.txtPeriodo.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Periodo";
            // 
            // btnSeleccionarXML
            // 
            this.btnSeleccionarXML.Location = new System.Drawing.Point(15, 267);
            this.btnSeleccionarXML.Name = "btnSeleccionarXML";
            this.btnSeleccionarXML.Size = new System.Drawing.Size(232, 39);
            this.btnSeleccionarXML.TabIndex = 25;
            this.btnSeleccionarXML.Text = "Seleccionar XML";
            this.btnSeleccionarXML.UseVisualStyleBackColor = true;
            this.btnSeleccionarXML.Click += new System.EventHandler(this.btnSeleccionarXML_Click);
            // 
            // txtXML
            // 
            this.txtXML.Enabled = false;
            this.txtXML.Location = new System.Drawing.Point(15, 327);
            this.txtXML.Name = "txtXML";
            this.txtXML.Size = new System.Drawing.Size(232, 20);
            this.txtXML.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Estatus       ";
            // 
            // cbEstatus
            // 
            this.cbEstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEstatus.FormattingEnabled = true;
            this.cbEstatus.Items.AddRange(new object[] {
            "Procesado",
            "Cancelado"});
            this.cbEstatus.Location = new System.Drawing.Point(18, 231);
            this.cbEstatus.Name = "cbEstatus";
            this.cbEstatus.Size = new System.Drawing.Size(229, 21);
            this.cbEstatus.TabIndex = 28;
            // 
            // responseFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 382);
            this.Controls.Add(this.cbEstatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtXML);
            this.Controls.Add(this.btnSeleccionarXML);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btnresponseFactura);
            this.Controls.Add(this.txtImporteRecibo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNoOperacion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPeriodo);
            this.Controls.Add(this.label2);
            this.Name = "responseFactura";
            this.Text = "responseFactura";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.responseFactura_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Button btnresponseFactura;
        private System.Windows.Forms.TextBox txtImporteRecibo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNoOperacion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPeriodo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSeleccionarXML;
        private System.Windows.Forms.TextBox txtXML;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbEstatus;
    }
}