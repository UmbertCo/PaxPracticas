namespace Canonicalizacion_Tests
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCargarPFX = new System.Windows.Forms.Button();
            this.oFD_PFX = new System.Windows.Forms.OpenFileDialog();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.btnCargarXML = new System.Windows.Forms.Button();
            this.oFD_XML = new System.Windows.Forms.OpenFileDialog();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblXPATH = new System.Windows.Forms.Label();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cbTipo = new System.Windows.Forms.ComboBox();
            this.cbXPATH = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCargarPFX
            // 
            this.btnCargarPFX.Location = new System.Drawing.Point(56, 45);
            this.btnCargarPFX.Name = "btnCargarPFX";
            this.btnCargarPFX.Size = new System.Drawing.Size(148, 52);
            this.btnCargarPFX.TabIndex = 0;
            this.btnCargarPFX.Text = "CARGAR PFX";
            this.btnCargarPFX.UseVisualStyleBackColor = true;
            this.btnCargarPFX.Click += new System.EventHandler(this.btnCargarPFX_Click);
            // 
            // btnProcesar
            // 
            this.btnProcesar.Location = new System.Drawing.Point(622, 306);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(112, 48);
            this.btnProcesar.TabIndex = 1;
            this.btnProcesar.Text = "PROCESAR";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // btnCargarXML
            // 
            this.btnCargarXML.Location = new System.Drawing.Point(56, 122);
            this.btnCargarXML.Name = "btnCargarXML";
            this.btnCargarXML.Size = new System.Drawing.Size(148, 56);
            this.btnCargarXML.TabIndex = 2;
            this.btnCargarXML.Text = "CARGAR XML";
            this.btnCargarXML.UseVisualStyleBackColor = true;
            this.btnCargarXML.Click += new System.EventHandler(this.btnCargarXML_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(56, 232);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(148, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(53, 216);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(102, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Contraseña de PFX:";
            // 
            // lblXPATH
            // 
            this.lblXPATH.AutoSize = true;
            this.lblXPATH.Location = new System.Drawing.Point(53, 274);
            this.lblXPATH.Name = "lblXPATH";
            this.lblXPATH.Size = new System.Drawing.Size(46, 13);
            this.lblXPATH.TabIndex = 6;
            this.lblXPATH.Text = "XPATH:";
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(53, 324);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(126, 13);
            this.lblTipo.TabIndex = 7;
            this.lblTipo.Text = "Tipo (PARA ID DE XML):";
            // 
            // cbTipo
            // 
            this.cbTipo.FormattingEnabled = true;
            this.cbTipo.Items.AddRange(new object[] {
            "SelloReceptor",
            "SelloPrestadorAutorizado"});
            this.cbTipo.Location = new System.Drawing.Point(56, 351);
            this.cbTipo.Name = "cbTipo";
            this.cbTipo.Size = new System.Drawing.Size(121, 21);
            this.cbTipo.TabIndex = 8;
            // 
            // cbXPATH
            // 
            this.cbXPATH.FormattingEnabled = true;
            this.cbXPATH.Items.AddRange(new object[] {
            "not(ancestor-or-self::ds:Signature)",
            "not(ancestor-or-self::ds:Signature[@Id=\'SelloPrestadorAutorizado\'])"});
            this.cbXPATH.Location = new System.Drawing.Point(58, 290);
            this.cbXPATH.Name = "cbXPATH";
            this.cbXPATH.Size = new System.Drawing.Size(121, 21);
            this.cbXPATH.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 400);
            this.Controls.Add(this.cbXPATH);
            this.Controls.Add(this.cbTipo);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.lblXPATH);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnCargarXML);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.btnCargarPFX);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCargarPFX;
        private System.Windows.Forms.OpenFileDialog oFD_PFX;
        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.Button btnCargarXML;
        private System.Windows.Forms.OpenFileDialog oFD_XML;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblXPATH;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cbTipo;
        private System.Windows.Forms.ComboBox cbXPATH;
    }
}

