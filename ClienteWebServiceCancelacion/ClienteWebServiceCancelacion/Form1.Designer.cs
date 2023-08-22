namespace ClienteWebServiceCancelacion
{
    partial class Form1
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
            this.btnBorrar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFecha = new System.Windows.Forms.TextBox();
            this.cmbRFC = new System.Windows.Forms.ComboBox();
            this.btnGenerarUUID = new System.Windows.Forms.Button();
            this.txtUUID = new System.Windows.Forms.TextBox();
            this.btnAgregarUUID = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCancelarUUID = new System.Windows.Forms.Button();
            this.lstUUID = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnCertificado = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBorrar
            // 
            this.btnBorrar.Location = new System.Drawing.Point(331, 200);
            this.btnBorrar.Name = "btnBorrar";
            this.btnBorrar.Size = new System.Drawing.Size(75, 23);
            this.btnBorrar.TabIndex = 53;
            this.btnBorrar.Text = "Borrar";
            this.btnBorrar.UseVisualStyleBackColor = true;
            this.btnBorrar.Click += new System.EventHandler(this.btnBorrar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 52;
            this.label2.Text = "Fecha Documento:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 51;
            this.label1.Text = "RFC Emisor:";
            // 
            // txtFecha
            // 
            this.txtFecha.Location = new System.Drawing.Point(157, 74);
            this.txtFecha.Name = "txtFecha";
            this.txtFecha.Size = new System.Drawing.Size(168, 20);
            this.txtFecha.TabIndex = 42;
            // 
            // cmbRFC
            // 
            this.cmbRFC.FormattingEnabled = true;
            this.cmbRFC.Items.AddRange(new object[] {
            "APR0412108C5",
            "AAQM610917QJA",
            "ASI100623H46",
            "BAJF541014RB3",
            "BA├æI7902011Y4",
            "CAY100623984",
            "CCO060626237",
            "CGA030903UC3",
            "CHO1006237R4",
            "CON100623S70",
            "CPA060516BG5",
            "CVA0110295B2",
            "CVM881213IL9",
            "ESI920427886",
            "EVA890922GK3",
            "FALG390202EA4",
            "FRC990917E47",
            "FUVI671020BV8",
            "GMO050628GW0",
            "GOYA780416GM0",
            "HELF701004LEA",
            "HIRR610331P91",
            "HIRS501128S26",
            "HISA4702173X8",
            "HOAE240126AM0",
            "HOAF441105FJ3",
            "HOAL550504PV8",
            "HUFR700623VBA",
            "HUOA160730589",
            "HUPL960723R17",
            "IACF180101JEA",
            "IACM561105K85",
            "IADA280804DN8",
            "IAJ830608UHA",
            "IAP031014RJ2",
            "IARD671015SB2",
            "IGA920319UN8",
            "IIIT181004AV6",
            "IOF910910LI6",
            "IUFT6111159L3",
            "IUNM4109097QA",
            "KAR0606068Z5",
            "LAM9805184I8",
            "LAN060505E67",
            "LAN7008173R5",
            "LAN8507268IA",
            "LOLR7012027G5",
            "M&L9911232Q7",
            "MAG041126GT8",
            "MAR980114GQA",
            "MARR7404132X6",
            "MSE061107IA8",
            "OOOA520911GS2",
            "PAT100623413",
            "PEPA801007BG3",
            "PEPP801006S87",
            "PEPP851016824",
            "PI├æ0405277S1",
            "PSY081020C3A",
            "PZA000413788",
            "SAGC610412AD9",
            "SOGJ700308AT8",
            "SUL010720JN8",
            "TCM970625MB1",
            "TME960709LR2",
            "TOSS700531U46",
            "TOTI2810017H5",
            "TOZO170718D34",
            "TUCA080409JM2",
            "TUCA2107035N9",
            "TUCA5703119R5",
            "TUCA650305GV5",
            "TUEJ391209C45",
            "TUEM470405V4A",
            "TUMG620310R95",
            "TUML7207199X3",
            "TUNO700901493",
            "UIGA170616SI7",
            "UIGF0512033L0",
            "ULC051129GC0",
            "UOEH360828VE4",
            "UOMA310324LV3",
            "URU070122S28",
            "UUMR180302HY1",
            "VAAM130719H60",
            "VACS740221QW6",
            "VEBD700616Q44",
            "VOC990129I26",
            "ZUN100623663",
            "AAA010101AAA"});
            this.cmbRFC.Location = new System.Drawing.Point(157, 46);
            this.cmbRFC.Name = "cmbRFC";
            this.cmbRFC.Size = new System.Drawing.Size(168, 21);
            this.cmbRFC.TabIndex = 41;
            this.cmbRFC.Text = "AAA010101AAA";
            // 
            // btnGenerarUUID
            // 
            this.btnGenerarUUID.Location = new System.Drawing.Point(331, 15);
            this.btnGenerarUUID.Name = "btnGenerarUUID";
            this.btnGenerarUUID.Size = new System.Drawing.Size(75, 23);
            this.btnGenerarUUID.TabIndex = 40;
            this.btnGenerarUUID.Text = "Generar";
            this.btnGenerarUUID.UseVisualStyleBackColor = true;
            this.btnGenerarUUID.Click += new System.EventHandler(this.btnGenerarUUID_Click);
            // 
            // txtUUID
            // 
            this.txtUUID.Location = new System.Drawing.Point(157, 18);
            this.txtUUID.Name = "txtUUID";
            this.txtUUID.Size = new System.Drawing.Size(168, 20);
            this.txtUUID.TabIndex = 39;
            this.txtUUID.Text = "2fc142cf-7043-495a-b3b2-3d3b85f8919e";
            // 
            // btnAgregarUUID
            // 
            this.btnAgregarUUID.Location = new System.Drawing.Point(331, 171);
            this.btnAgregarUUID.Name = "btnAgregarUUID";
            this.btnAgregarUUID.Size = new System.Drawing.Size(75, 23);
            this.btnAgregarUUID.TabIndex = 44;
            this.btnAgregarUUID.Text = "Agregar";
            this.btnAgregarUUID.UseVisualStyleBackColor = true;
            this.btnAgregarUUID.Click += new System.EventHandler(this.btnAgregarUUID_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(86, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 46;
            this.label7.Text = "UUID:";
            // 
            // btnCancelarUUID
            // 
            this.btnCancelarUUID.Location = new System.Drawing.Point(221, 298);
            this.btnCancelarUUID.Name = "btnCancelarUUID";
            this.btnCancelarUUID.Size = new System.Drawing.Size(104, 23);
            this.btnCancelarUUID.TabIndex = 45;
            this.btnCancelarUUID.Text = "Cancelar UUID";
            this.btnCancelarUUID.UseVisualStyleBackColor = true;
            this.btnCancelarUUID.Click += new System.EventHandler(this.btnCancelarUUID_Click);
            // 
            // lstUUID
            // 
            this.lstUUID.FormattingEnabled = true;
            this.lstUUID.Location = new System.Drawing.Point(157, 171);
            this.lstUUID.Name = "lstUUID";
            this.lstUUID.Size = new System.Drawing.Size(168, 121);
            this.lstUUID.TabIndex = 43;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 54;
            this.label3.Text = "Usuario :";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(157, 104);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(168, 20);
            this.txtUsuario.TabIndex = 55;
            this.txtUsuario.Text = "WSDL_PAX";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 56;
            this.label4.Text = "Password :";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(157, 136);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(168, 20);
            this.txtPassword.TabIndex = 57;
            this.txtPassword.Text = "w4XDjcObxIHDt8OcxLPEj8SZxY3Cr8KRwqnDqsKJwpLDn8KHxIPEkQgf77+VBO++oO++j++/h++9pw==";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnCertificado
            // 
            this.btnCertificado.Location = new System.Drawing.Point(331, 229);
            this.btnCertificado.Name = "btnCertificado";
            this.btnCertificado.Size = new System.Drawing.Size(75, 23);
            this.btnCertificado.TabIndex = 58;
            this.btnCertificado.Text = "Certificado";
            this.btnCertificado.UseVisualStyleBackColor = true;
            this.btnCertificado.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(331, 258);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 59;
            this.button1.Text = "LLave";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(157, 327);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(593, 220);
            this.richTextBox1.TabIndex = 60;
            this.richTextBox1.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(582, 299);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(168, 22);
            this.button2.TabIndex = 61;
            this.button2.Text = "Enviar Comprobantes";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(76, 327);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 62;
            this.button3.Text = "Dar Formato";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 558);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCertificado);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBorrar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFecha);
            this.Controls.Add(this.cmbRFC);
            this.Controls.Add(this.btnGenerarUUID);
            this.Controls.Add(this.txtUUID);
            this.Controls.Add(this.btnAgregarUUID);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnCancelarUUID);
            this.Controls.Add(this.lstUUID);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBorrar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFecha;
        private System.Windows.Forms.ComboBox cmbRFC;
        private System.Windows.Forms.Button btnGenerarUUID;
        private System.Windows.Forms.TextBox txtUUID;
        private System.Windows.Forms.Button btnAgregarUUID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCancelarUUID;
        private System.Windows.Forms.ListBox lstUUID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnCertificado;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

