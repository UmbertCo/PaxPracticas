namespace GeneradorMasivoComprobantes
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
            this.cbTipoDoc = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbEmisores = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtFecha = new System.Windows.Forms.DateTimePicker();
            this.lblConceptos = new System.Windows.Forms.Label();
            this.nConceptos = new System.Windows.Forms.NumericUpDown();
            this.nComprobantesGenerar = new System.Windows.Forms.NumericUpDown();
            this.lbComprobantes = new System.Windows.Forms.Label();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cbSerie = new System.Windows.Forms.ComboBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lbEstatus = new System.Windows.Forms.Label();
            this.cbEstatus = new System.Windows.Forms.ComboBox();
            this.chArchivos = new System.Windows.Forms.CheckBox();
            this.lblRutaXML = new System.Windows.Forms.Label();
            this.chTimbrar = new System.Windows.Forms.CheckBox();
            this.chGuardarZip = new System.Windows.Forms.CheckBox();
            this.lnkCarpetas = new System.Windows.Forms.LinkLabel();
            this.chImpuestoRetenido = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboISR = new System.Windows.Forms.ComboBox();
            this.lblIVA = new System.Windows.Forms.Label();
            this.cboIVA = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRFCEmisor = new System.Windows.Forms.TextBox();
            this.chEditarEmisor = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nConceptos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nComprobantesGenerar)).BeginInit();
            this.SuspendLayout();
            // 
            // cbTipoDoc
            // 
            this.cbTipoDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTipoDoc.FormattingEnabled = true;
            this.cbTipoDoc.Items.AddRange(new object[] {
            "Factura",
            "Recibo de honorarios",
            "Recibo de Nómina",
            "Layout Factura",
            "Layout Nómina"});
            this.cbTipoDoc.Location = new System.Drawing.Point(36, 177);
            this.cbTipoDoc.Name = "cbTipoDoc";
            this.cbTipoDoc.Size = new System.Drawing.Size(171, 21);
            this.cbTipoDoc.TabIndex = 0;
            this.cbTipoDoc.SelectedIndexChanged += new System.EventHandler(this.cbTipoDoc_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tipo de documento:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Razón social:";
            // 
            // cbEmisores
            // 
            this.cbEmisores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmisores.FormattingEnabled = true;
            this.cbEmisores.Location = new System.Drawing.Point(35, 40);
            this.cbEmisores.Name = "cbEmisores";
            this.cbEmisores.Size = new System.Drawing.Size(397, 21);
            this.cbEmisores.TabIndex = 3;
            this.cbEmisores.SelectedIndexChanged += new System.EventHandler(this.cbEmisores_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 287);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fecha:";
            // 
            // dtFecha
            // 
            this.dtFecha.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFecha.Location = new System.Drawing.Point(36, 312);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Size = new System.Drawing.Size(171, 20);
            this.dtFecha.TabIndex = 6;
            // 
            // lblConceptos
            // 
            this.lblConceptos.AutoSize = true;
            this.lblConceptos.Location = new System.Drawing.Point(38, 355);
            this.lblConceptos.Name = "lblConceptos";
            this.lblConceptos.Size = new System.Drawing.Size(100, 13);
            this.lblConceptos.TabIndex = 7;
            this.lblConceptos.Text = "Número conceptos:";
            // 
            // nConceptos
            // 
            this.nConceptos.Location = new System.Drawing.Point(41, 386);
            this.nConceptos.Maximum = new decimal(new int[] {
            3000000,
            0,
            0,
            0});
            this.nConceptos.Name = "nConceptos";
            this.nConceptos.Size = new System.Drawing.Size(166, 20);
            this.nConceptos.TabIndex = 8;
            // 
            // nComprobantesGenerar
            // 
            this.nComprobantesGenerar.Location = new System.Drawing.Point(41, 454);
            this.nComprobantesGenerar.Maximum = new decimal(new int[] {
            3000000,
            0,
            0,
            0});
            this.nComprobantesGenerar.Name = "nComprobantesGenerar";
            this.nComprobantesGenerar.Size = new System.Drawing.Size(166, 20);
            this.nComprobantesGenerar.TabIndex = 10;
            this.nComprobantesGenerar.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbComprobantes
            // 
            this.lbComprobantes.AutoSize = true;
            this.lbComprobantes.Location = new System.Drawing.Point(38, 425);
            this.lbComprobantes.Name = "lbComprobantes";
            this.lbComprobantes.Size = new System.Drawing.Size(126, 13);
            this.lbComprobantes.TabIndex = 9;
            this.lbComprobantes.Text = "Comprobantes a generar:";
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(584, 40);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(138, 43);
            this.btnGenerar.TabIndex = 11;
            this.btnGenerar.Text = "Cargar certificado emisor";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 496);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Serie:";
            // 
            // cbSerie
            // 
            this.cbSerie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerie.FormattingEnabled = true;
            this.cbSerie.Location = new System.Drawing.Point(41, 521);
            this.cbSerie.Name = "cbSerie";
            this.cbSerie.Size = new System.Drawing.Size(166, 21);
            this.cbSerie.TabIndex = 15;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(584, 123);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(138, 43);
            this.btnCancelar.TabIndex = 16;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lbEstatus
            // 
            this.lbEstatus.AutoSize = true;
            this.lbEstatus.Location = new System.Drawing.Point(33, 220);
            this.lbEstatus.Name = "lbEstatus";
            this.lbEstatus.Size = new System.Drawing.Size(45, 13);
            this.lbEstatus.TabIndex = 18;
            this.lbEstatus.Text = "Estatus:";
            // 
            // cbEstatus
            // 
            this.cbEstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEstatus.FormattingEnabled = true;
            this.cbEstatus.Items.AddRange(new object[] {
            "P",
            "A",
            "C",
            "G"});
            this.cbEstatus.Location = new System.Drawing.Point(36, 245);
            this.cbEstatus.Name = "cbEstatus";
            this.cbEstatus.Size = new System.Drawing.Size(171, 21);
            this.cbEstatus.TabIndex = 19;
            // 
            // chArchivos
            // 
            this.chArchivos.AutoSize = true;
            this.chArchivos.Location = new System.Drawing.Point(393, 666);
            this.chArchivos.Name = "chArchivos";
            this.chArchivos.Size = new System.Drawing.Size(132, 17);
            this.chArchivos.TabIndex = 20;
            this.chArchivos.Text = "Generar archivos XML";
            this.chArchivos.UseVisualStyleBackColor = true;
            this.chArchivos.CheckedChanged += new System.EventHandler(this.cbArchivos_CheckedChanged);
            // 
            // lblRutaXML
            // 
            this.lblRutaXML.AutoSize = true;
            this.lblRutaXML.Enabled = false;
            this.lblRutaXML.Location = new System.Drawing.Point(273, 694);
            this.lblRutaXML.Name = "lblRutaXML";
            this.lblRutaXML.Size = new System.Drawing.Size(35, 13);
            this.lblRutaXML.TabIndex = 21;
            this.lblRutaXML.Text = "label4";
            // 
            // chTimbrar
            // 
            this.chTimbrar.AutoSize = true;
            this.chTimbrar.Checked = true;
            this.chTimbrar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chTimbrar.Location = new System.Drawing.Point(717, 666);
            this.chTimbrar.Name = "chTimbrar";
            this.chTimbrar.Size = new System.Drawing.Size(105, 17);
            this.chTimbrar.TabIndex = 22;
            this.chTimbrar.Text = "Timbrar Archivos";
            this.chTimbrar.UseVisualStyleBackColor = true;
            this.chTimbrar.CheckedChanged += new System.EventHandler(this.chTimbrar_CheckedChanged);
            // 
            // chGuardarZip
            // 
            this.chGuardarZip.AutoSize = true;
            this.chGuardarZip.Location = new System.Drawing.Point(276, 666);
            this.chGuardarZip.Name = "chGuardarZip";
            this.chGuardarZip.Size = new System.Drawing.Size(99, 17);
            this.chGuardarZip.TabIndex = 23;
            this.chGuardarZip.Text = "Guardar en ZIP";
            this.chGuardarZip.UseVisualStyleBackColor = true;
            this.chGuardarZip.CheckedChanged += new System.EventHandler(this.chGuardarZip_CheckedChanged);
            // 
            // lnkCarpetas
            // 
            this.lnkCarpetas.AutoSize = true;
            this.lnkCarpetas.Location = new System.Drawing.Point(276, 719);
            this.lnkCarpetas.Name = "lnkCarpetas";
            this.lnkCarpetas.Size = new System.Drawing.Size(99, 13);
            this.lnkCarpetas.TabIndex = 24;
            this.lnkCarpetas.TabStop = true;
            this.lnkCarpetas.Text = "Configurar carpetas";
            this.lnkCarpetas.Visible = false;
            this.lnkCarpetas.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCarpetas_LinkClicked);
            // 
            // chImpuestoRetenido
            // 
            this.chImpuestoRetenido.AutoSize = true;
            this.chImpuestoRetenido.Location = new System.Drawing.Point(541, 666);
            this.chImpuestoRetenido.Name = "chImpuestoRetenido";
            this.chImpuestoRetenido.Size = new System.Drawing.Size(159, 17);
            this.chImpuestoRetenido.TabIndex = 25;
            this.chImpuestoRetenido.Text = "Agregar impuestos retenidos";
            this.chImpuestoRetenido.UseVisualStyleBackColor = true;
            this.chImpuestoRetenido.CheckedChanged += new System.EventHandler(this.chImpuestoRetenido_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 566);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Impuesto retenido: ISR";
            // 
            // cboISR
            // 
            this.cboISR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboISR.Enabled = false;
            this.cboISR.FormattingEnabled = true;
            this.cboISR.Items.AddRange(new object[] {
            "No",
            "Sí"});
            this.cboISR.Location = new System.Drawing.Point(41, 585);
            this.cboISR.Name = "cboISR";
            this.cboISR.Size = new System.Drawing.Size(163, 21);
            this.cboISR.TabIndex = 27;
            this.cboISR.SelectedIndexChanged += new System.EventHandler(this.cboISR_SelectedIndexChanged);
            // 
            // lblIVA
            // 
            this.lblIVA.AutoSize = true;
            this.lblIVA.Location = new System.Drawing.Point(41, 633);
            this.lblIVA.Name = "lblIVA";
            this.lblIVA.Size = new System.Drawing.Size(114, 13);
            this.lblIVA.TabIndex = 28;
            this.lblIVA.Text = "Impuesto retenido: IVA";
            // 
            // cboIVA
            // 
            this.cboIVA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIVA.Enabled = false;
            this.cboIVA.FormattingEnabled = true;
            this.cboIVA.Items.AddRange(new object[] {
            "No",
            "Sí"});
            this.cboIVA.Location = new System.Drawing.Point(41, 661);
            this.cboIVA.Name = "cboIVA";
            this.cboIVA.Size = new System.Drawing.Size(163, 21);
            this.cboIVA.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Nombre";
            // 
            // txtNombre
            // 
            this.txtNombre.Enabled = false;
            this.txtNombre.Location = new System.Drawing.Point(36, 106);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(171, 20);
            this.txtNombre.TabIndex = 31;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(227, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "-";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(260, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "RFC Emisor";
            // 
            // txtRFCEmisor
            // 
            this.txtRFCEmisor.Enabled = false;
            this.txtRFCEmisor.Location = new System.Drawing.Point(258, 106);
            this.txtRFCEmisor.Name = "txtRFCEmisor";
            this.txtRFCEmisor.Size = new System.Drawing.Size(174, 20);
            this.txtRFCEmisor.TabIndex = 34;
            // 
            // chEditarEmisor
            // 
            this.chEditarEmisor.AutoSize = true;
            this.chEditarEmisor.Location = new System.Drawing.Point(451, 108);
            this.chEditarEmisor.Name = "chEditarEmisor";
            this.chEditarEmisor.Size = new System.Drawing.Size(92, 17);
            this.chEditarEmisor.TabIndex = 35;
            this.chEditarEmisor.Text = "Nuevo Emisor";
            this.chEditarEmisor.UseVisualStyleBackColor = true;
            this.chEditarEmisor.CheckedChanged += new System.EventHandler(this.chEditarEmisor_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(845, 748);
            this.Controls.Add(this.chEditarEmisor);
            this.Controls.Add(this.txtRFCEmisor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboIVA);
            this.Controls.Add(this.lblIVA);
            this.Controls.Add(this.cboISR);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chImpuestoRetenido);
            this.Controls.Add(this.lnkCarpetas);
            this.Controls.Add(this.chGuardarZip);
            this.Controls.Add(this.chTimbrar);
            this.Controls.Add(this.lblRutaXML);
            this.Controls.Add(this.chArchivos);
            this.Controls.Add(this.cbEstatus);
            this.Controls.Add(this.lbEstatus);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.cbSerie);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.nComprobantesGenerar);
            this.Controls.Add(this.lbComprobantes);
            this.Controls.Add(this.nConceptos);
            this.Controls.Add(this.lblConceptos);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbEmisores);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbTipoDoc);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Generador Masivo de Comprobantes";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.nConceptos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nComprobantesGenerar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTipoDoc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbEmisores;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtFecha;
        private System.Windows.Forms.Label lblConceptos;
        private System.Windows.Forms.NumericUpDown nConceptos;
        private System.Windows.Forms.NumericUpDown nComprobantesGenerar;
        private System.Windows.Forms.Label lbComprobantes;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbSerie;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lbEstatus;
        private System.Windows.Forms.ComboBox cbEstatus;
        private System.Windows.Forms.CheckBox chArchivos;
        private System.Windows.Forms.Label lblRutaXML;
        private System.Windows.Forms.CheckBox chTimbrar;
        private System.Windows.Forms.CheckBox chGuardarZip;
        private System.Windows.Forms.LinkLabel lnkCarpetas;
        private System.Windows.Forms.CheckBox chImpuestoRetenido;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboISR;
        private System.Windows.Forms.Label lblIVA;
        private System.Windows.Forms.ComboBox cboIVA;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtRFCEmisor;
        private System.Windows.Forms.CheckBox chEditarEmisor;
    }
}

