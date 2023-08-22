namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmTimbrado
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
            this.txtArchivo = new System.Windows.Forms.TextBox();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.btnHashCadena = new System.Windows.Forms.Button();
            this.btnCadenaBase64 = new System.Windows.Forms.Button();
            this.txtCadena = new System.Windows.Forms.TextBox();
            this.rbProduccion = new System.Windows.Forms.RadioButton();
            this.rbTest = new System.Windows.Forms.RadioButton();
            this.rbPruebas = new System.Windows.Forms.RadioButton();
            this.rbLocal = new System.Windows.Forms.RadioButton();
            this.cbTipoComprobante = new System.Windows.Forms.ComboBox();
            this.cbSellarPrueba = new System.Windows.Forms.CheckBox();
            this.rbDP = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // txtArchivo
            // 
            this.txtArchivo.Location = new System.Drawing.Point(30, 12);
            this.txtArchivo.Name = "txtArchivo";
            this.txtArchivo.Size = new System.Drawing.Size(439, 20);
            this.txtArchivo.TabIndex = 0;
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(483, 59);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(75, 23);
            this.btnEnviar.TabIndex = 1;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // txtResultado
            // 
            this.txtResultado.Location = new System.Drawing.Point(30, 90);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(439, 239);
            this.txtResultado.TabIndex = 2;
            // 
            // btnHashCadena
            // 
            this.btnHashCadena.Location = new System.Drawing.Point(483, 2);
            this.btnHashCadena.Name = "btnHashCadena";
            this.btnHashCadena.Size = new System.Drawing.Size(101, 23);
            this.btnHashCadena.TabIndex = 3;
            this.btnHashCadena.Text = "Hash a Cadena";
            this.btnHashCadena.UseVisualStyleBackColor = true;
            this.btnHashCadena.Click += new System.EventHandler(this.btnHashCadena_Click);
            // 
            // btnCadenaBase64
            // 
            this.btnCadenaBase64.Location = new System.Drawing.Point(483, 30);
            this.btnCadenaBase64.Name = "btnCadenaBase64";
            this.btnCadenaBase64.Size = new System.Drawing.Size(101, 23);
            this.btnCadenaBase64.TabIndex = 4;
            this.btnCadenaBase64.Text = "Base64 a Cadena";
            this.btnCadenaBase64.UseVisualStyleBackColor = true;
            this.btnCadenaBase64.Click += new System.EventHandler(this.btnCadenaBase64_Click);
            // 
            // txtCadena
            // 
            this.txtCadena.Location = new System.Drawing.Point(483, 90);
            this.txtCadena.Multiline = true;
            this.txtCadena.Name = "txtCadena";
            this.txtCadena.Size = new System.Drawing.Size(202, 239);
            this.txtCadena.TabIndex = 5;
            // 
            // rbProduccion
            // 
            this.rbProduccion.AutoSize = true;
            this.rbProduccion.Location = new System.Drawing.Point(30, 39);
            this.rbProduccion.Name = "rbProduccion";
            this.rbProduccion.Size = new System.Drawing.Size(79, 17);
            this.rbProduccion.TabIndex = 6;
            this.rbProduccion.Text = "Produccion";
            this.rbProduccion.UseVisualStyleBackColor = true;
            this.rbProduccion.CheckedChanged += new System.EventHandler(this.rbProduccion_CheckedChanged);
            // 
            // rbTest
            // 
            this.rbTest.AutoSize = true;
            this.rbTest.Checked = true;
            this.rbTest.Location = new System.Drawing.Point(115, 39);
            this.rbTest.Name = "rbTest";
            this.rbTest.Size = new System.Drawing.Size(46, 17);
            this.rbTest.TabIndex = 7;
            this.rbTest.TabStop = true;
            this.rbTest.Text = "Test";
            this.rbTest.UseVisualStyleBackColor = true;
            this.rbTest.CheckedChanged += new System.EventHandler(this.rbTest_CheckedChanged);
            // 
            // rbPruebas
            // 
            this.rbPruebas.AutoSize = true;
            this.rbPruebas.Location = new System.Drawing.Point(167, 39);
            this.rbPruebas.Name = "rbPruebas";
            this.rbPruebas.Size = new System.Drawing.Size(64, 17);
            this.rbPruebas.TabIndex = 8;
            this.rbPruebas.Text = "Pruebas";
            this.rbPruebas.UseVisualStyleBackColor = true;
            this.rbPruebas.CheckedChanged += new System.EventHandler(this.rbPruebas_CheckedChanged);
            // 
            // rbLocal
            // 
            this.rbLocal.AutoSize = true;
            this.rbLocal.Location = new System.Drawing.Point(238, 39);
            this.rbLocal.Name = "rbLocal";
            this.rbLocal.Size = new System.Drawing.Size(51, 17);
            this.rbLocal.TabIndex = 9;
            this.rbLocal.TabStop = true;
            this.rbLocal.Text = "Local";
            this.rbLocal.UseVisualStyleBackColor = true;
            this.rbLocal.CheckedChanged += new System.EventHandler(this.rbLocal_CheckedChanged);
            // 
            // cbTipoComprobante
            // 
            this.cbTipoComprobante.FormattingEnabled = true;
            this.cbTipoComprobante.Items.AddRange(new object[] {
            "Factura",
            "Recibo de Nomina"});
            this.cbTipoComprobante.Location = new System.Drawing.Point(30, 63);
            this.cbTipoComprobante.Name = "cbTipoComprobante";
            this.cbTipoComprobante.Size = new System.Drawing.Size(201, 21);
            this.cbTipoComprobante.TabIndex = 10;
            // 
            // cbSellarPrueba
            // 
            this.cbSellarPrueba.AutoSize = true;
            this.cbSellarPrueba.Location = new System.Drawing.Point(380, 40);
            this.cbSellarPrueba.Name = "cbSellarPrueba";
            this.cbSellarPrueba.Size = new System.Drawing.Size(89, 17);
            this.cbSellarPrueba.TabIndex = 11;
            this.cbSellarPrueba.Text = "Sellar Prueba";
            this.cbSellarPrueba.UseVisualStyleBackColor = true;
            // 
            // rbDP
            // 
            this.rbDP.AutoSize = true;
            this.rbDP.Location = new System.Drawing.Point(296, 39);
            this.rbDP.Name = "rbDP";
            this.rbDP.Size = new System.Drawing.Size(40, 17);
            this.rbDP.TabIndex = 12;
            this.rbDP.TabStop = true;
            this.rbDP.Text = "DP";
            this.rbDP.UseVisualStyleBackColor = true;
            this.rbDP.CheckedChanged += new System.EventHandler(this.rbDP_CheckedChanged);
            // 
            // frmTimbrado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 360);
            this.Controls.Add(this.rbDP);
            this.Controls.Add(this.cbSellarPrueba);
            this.Controls.Add(this.cbTipoComprobante);
            this.Controls.Add(this.rbLocal);
            this.Controls.Add(this.rbPruebas);
            this.Controls.Add(this.rbTest);
            this.Controls.Add(this.rbProduccion);
            this.Controls.Add(this.txtCadena);
            this.Controls.Add(this.btnCadenaBase64);
            this.Controls.Add(this.btnHashCadena);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btnEnviar);
            this.Controls.Add(this.txtArchivo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmTimbrado";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTimbrado";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtArchivo;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Button btnHashCadena;
        private System.Windows.Forms.Button btnCadenaBase64;
        private System.Windows.Forms.TextBox txtCadena;
        private System.Windows.Forms.RadioButton rbProduccion;
        private System.Windows.Forms.RadioButton rbTest;
        private System.Windows.Forms.RadioButton rbPruebas;
        private System.Windows.Forms.RadioButton rbLocal;
        private System.Windows.Forms.ComboBox cbTipoComprobante;
        private System.Windows.Forms.CheckBox cbSellarPrueba;
        private System.Windows.Forms.RadioButton rbDP;
    }
}