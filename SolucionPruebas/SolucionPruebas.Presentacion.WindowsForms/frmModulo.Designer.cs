namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmModulo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModulo));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiEncriptacion = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEncriptaciónTexto = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenSSL = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSelloOpenSSL = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiValidacion = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEsquema32 = new System.Windows.Forms.ToolStripMenuItem();
            this.esquema32LayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiValidacionValidarLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiZip = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGeneraciónZip = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiServicios = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiJson = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiVentas = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOutlook = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGeneración = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGeneracionSellos = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGeneracionComprobante22A32 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiGeneracionLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenXML = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenXmlExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlgoritmos = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPermutacionesParametros = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTimbrado = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTimbradoEnviar = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiActualizacionComprobante = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiActualizacionComprobantes = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmilCO = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTCPNET = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCargarLCO = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAcuses = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAcusePAC = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRegistroUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRegistro = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEncriptacion,
            this.tsmiOpenSSL,
            this.tsmiValidacion,
            this.tsmiZip,
            this.tsmiServicios,
            this.tsmiVentas,
            this.tsmiGeneración,
            this.tsmiOpenXML,
            this.tsmiAlgoritmos,
            this.tsmiTimbrado,
            this.tsmiActualizacionComprobante,
            this.tsmilCO,
            this.tsmiAcuses,
            this.tsmiRegistroUsuarios});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1068, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiEncriptacion
            // 
            this.tsmiEncriptacion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEncriptaciónTexto});
            this.tsmiEncriptacion.Name = "tsmiEncriptacion";
            this.tsmiEncriptacion.Size = new System.Drawing.Size(85, 20);
            this.tsmiEncriptacion.Text = "Encriptacion";
            // 
            // tsmiEncriptaciónTexto
            // 
            this.tsmiEncriptaciónTexto.Name = "tsmiEncriptaciónTexto";
            this.tsmiEncriptaciónTexto.Size = new System.Drawing.Size(185, 22);
            this.tsmiEncriptaciónTexto.Text = "Encriptación de texto";
            this.tsmiEncriptaciónTexto.Click += new System.EventHandler(this.tsmiEncriptacionEncriptaciónTexto_Click);
            // 
            // tsmiOpenSSL
            // 
            this.tsmiOpenSSL.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSelloOpenSSL});
            this.tsmiOpenSSL.Name = "tsmiOpenSSL";
            this.tsmiOpenSSL.Size = new System.Drawing.Size(66, 20);
            this.tsmiOpenSSL.Text = "OpenSSL";
            // 
            // tsmiSelloOpenSSL
            // 
            this.tsmiSelloOpenSSL.Name = "tsmiSelloOpenSSL";
            this.tsmiSelloOpenSSL.Size = new System.Drawing.Size(149, 22);
            this.tsmiSelloOpenSSL.Text = "Sello OpenSSL";
            this.tsmiSelloOpenSSL.Click += new System.EventHandler(this.tsmiOpenSSLSelloOpenSSL_Click);
            // 
            // tsmiValidacion
            // 
            this.tsmiValidacion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEsquema32,
            this.esquema32LayoutToolStripMenuItem,
            this.tmiValidacionValidarLayout});
            this.tsmiValidacion.Name = "tsmiValidacion";
            this.tsmiValidacion.Size = new System.Drawing.Size(74, 20);
            this.tsmiValidacion.Text = "Validacion";
            // 
            // tsmiEsquema32
            // 
            this.tsmiEsquema32.Name = "tsmiEsquema32";
            this.tsmiEsquema32.Size = new System.Drawing.Size(222, 22);
            this.tsmiEsquema32.Text = "Esquema 32";
            this.tsmiEsquema32.Click += new System.EventHandler(this.tsmiValidacionEsquema32_Click);
            // 
            // esquema32LayoutToolStripMenuItem
            // 
            this.esquema32LayoutToolStripMenuItem.Name = "esquema32LayoutToolStripMenuItem";
            this.esquema32LayoutToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.esquema32LayoutToolStripMenuItem.Text = "Esquema 32 Layout Nómina";
            this.esquema32LayoutToolStripMenuItem.Click += new System.EventHandler(this.esquema32LayoutToolStripMenuItem_Click);
            // 
            // tmiValidacionValidarLayout
            // 
            this.tmiValidacionValidarLayout.Name = "tmiValidacionValidarLayout";
            this.tmiValidacionValidarLayout.Size = new System.Drawing.Size(222, 22);
            this.tmiValidacionValidarLayout.Text = "Esquema 32 Layout";
            this.tmiValidacionValidarLayout.Click += new System.EventHandler(this.tmiValidacionValidarLayout_Click);
            // 
            // tsmiZip
            // 
            this.tsmiZip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiGeneraciónZip});
            this.tsmiZip.Name = "tsmiZip";
            this.tsmiZip.Size = new System.Drawing.Size(36, 20);
            this.tsmiZip.Text = "Zip";
            // 
            // tsmiGeneraciónZip
            // 
            this.tsmiGeneraciónZip.Name = "tsmiGeneraciónZip";
            this.tsmiGeneraciónZip.Size = new System.Drawing.Size(154, 22);
            this.tsmiGeneraciónZip.Text = "Generación Zip";
            this.tsmiGeneraciónZip.Click += new System.EventHandler(this.tsmiZipGeneraciónZip_Click);
            // 
            // tsmiServicios
            // 
            this.tsmiServicios.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiJson});
            this.tsmiServicios.Name = "tsmiServicios";
            this.tsmiServicios.Size = new System.Drawing.Size(65, 20);
            this.tsmiServicios.Text = "Servicios";
            // 
            // tsmiJson
            // 
            this.tsmiJson.Name = "tsmiJson";
            this.tsmiJson.Size = new System.Drawing.Size(102, 22);
            this.tsmiJson.Text = "JSON";
            this.tsmiJson.Click += new System.EventHandler(this.tsmiServiciosJson_Click);
            // 
            // tsmiVentas
            // 
            this.tsmiVentas.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOutlook});
            this.tsmiVentas.Name = "tsmiVentas";
            this.tsmiVentas.Size = new System.Drawing.Size(54, 20);
            this.tsmiVentas.Text = "Ventas";
            // 
            // tsmiOutlook
            // 
            this.tsmiOutlook.Name = "tsmiOutlook";
            this.tsmiOutlook.Size = new System.Drawing.Size(152, 22);
            this.tsmiOutlook.Text = "Outlook";
            this.tsmiOutlook.Click += new System.EventHandler(this.tsmiVentasOutlook_Click);
            // 
            // tsmiGeneración
            // 
            this.tsmiGeneración.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiGeneracionSellos,
            this.tsmiGeneracionComprobante22A32,
            this.tsmiGeneracionLayout});
            this.tsmiGeneración.Name = "tsmiGeneración";
            this.tsmiGeneración.Size = new System.Drawing.Size(79, 20);
            this.tsmiGeneración.Text = "Generación";
            // 
            // tsmiGeneracionSellos
            // 
            this.tsmiGeneracionSellos.Name = "tsmiGeneracionSellos";
            this.tsmiGeneracionSellos.Size = new System.Drawing.Size(193, 22);
            this.tsmiGeneracionSellos.Text = "Sellos";
            this.tsmiGeneracionSellos.Click += new System.EventHandler(this.tsmiGeneracionSellos_Click);
            // 
            // tsmiGeneracionComprobante22A32
            // 
            this.tsmiGeneracionComprobante22A32.Name = "tsmiGeneracionComprobante22A32";
            this.tsmiGeneracionComprobante22A32.Size = new System.Drawing.Size(193, 22);
            this.tsmiGeneracionComprobante22A32.Text = "Comprobante 2.2 a 3.2";
            this.tsmiGeneracionComprobante22A32.Click += new System.EventHandler(this.tsmiGeneracionComprobante22A32_Click);
            // 
            // tsmiGeneracionLayout
            // 
            this.tsmiGeneracionLayout.Name = "tsmiGeneracionLayout";
            this.tsmiGeneracionLayout.Size = new System.Drawing.Size(193, 22);
            this.tsmiGeneracionLayout.Text = "Layout";
            this.tsmiGeneracionLayout.Click += new System.EventHandler(this.tsmiGeneracionLayout_Click);
            // 
            // tsmiOpenXML
            // 
            this.tsmiOpenXML.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpenXmlExcel});
            this.tsmiOpenXML.Name = "tsmiOpenXML";
            this.tsmiOpenXML.Size = new System.Drawing.Size(72, 20);
            this.tsmiOpenXML.Text = "OpenXML";
            // 
            // tsmiOpenXmlExcel
            // 
            this.tsmiOpenXmlExcel.Name = "tsmiOpenXmlExcel";
            this.tsmiOpenXmlExcel.Size = new System.Drawing.Size(152, 22);
            this.tsmiOpenXmlExcel.Text = "Excel";
            this.tsmiOpenXmlExcel.Click += new System.EventHandler(this.tsmiOpenXmlExcel_Click);
            // 
            // tsmiAlgoritmos
            // 
            this.tsmiAlgoritmos.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPermutacionesParametros});
            this.tsmiAlgoritmos.Name = "tsmiAlgoritmos";
            this.tsmiAlgoritmos.Size = new System.Drawing.Size(78, 20);
            this.tsmiAlgoritmos.Text = "Algoritmos";
            // 
            // tsmiPermutacionesParametros
            // 
            this.tsmiPermutacionesParametros.Name = "tsmiPermutacionesParametros";
            this.tsmiPermutacionesParametros.Size = new System.Drawing.Size(232, 22);
            this.tsmiPermutacionesParametros.Text = "Permutaciones de parametros";
            this.tsmiPermutacionesParametros.Click += new System.EventHandler(this.tsmiPermutacionesParametros_Click);
            // 
            // tsmiTimbrado
            // 
            this.tsmiTimbrado.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTimbradoEnviar});
            this.tsmiTimbrado.Name = "tsmiTimbrado";
            this.tsmiTimbrado.Size = new System.Drawing.Size(71, 20);
            this.tsmiTimbrado.Text = "Timbrado";
            // 
            // tsmiTimbradoEnviar
            // 
            this.tsmiTimbradoEnviar.Name = "tsmiTimbradoEnviar";
            this.tsmiTimbradoEnviar.Size = new System.Drawing.Size(152, 22);
            this.tsmiTimbradoEnviar.Text = "Enviar";
            this.tsmiTimbradoEnviar.Click += new System.EventHandler(this.tsmiTimbradoEnviar_Click);
            // 
            // tsmiActualizacionComprobante
            // 
            this.tsmiActualizacionComprobante.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiActualizacionComprobantes});
            this.tsmiActualizacionComprobante.Name = "tsmiActualizacionComprobante";
            this.tsmiActualizacionComprobante.Size = new System.Drawing.Size(170, 20);
            this.tsmiActualizacionComprobante.Text = "Actualizacion comprobantes";
            // 
            // tsmiActualizacionComprobantes
            // 
            this.tsmiActualizacionComprobantes.Name = "tsmiActualizacionComprobantes";
            this.tsmiActualizacionComprobantes.Size = new System.Drawing.Size(145, 22);
            this.tsmiActualizacionComprobantes.Text = "Actualizacion";
            this.tsmiActualizacionComprobantes.Click += new System.EventHandler(this.tsmiActualizacionComprobantes_Click);
            // 
            // tsmilCO
            // 
            this.tsmilCO.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTCPNET,
            this.tsmiCargarLCO});
            this.tsmilCO.Name = "tsmilCO";
            this.tsmilCO.Size = new System.Drawing.Size(42, 20);
            this.tsmilCO.Text = "LCO";
            // 
            // tsmiTCPNET
            // 
            this.tsmiTCPNET.Name = "tsmiTCPNET";
            this.tsmiTCPNET.Size = new System.Drawing.Size(152, 22);
            this.tsmiTCPNET.Text = "NET.TCP";
            this.tsmiTCPNET.Click += new System.EventHandler(this.tsmiTCPNET_Click);
            // 
            // tsmiCargarLCO
            // 
            this.tsmiCargarLCO.Name = "tsmiCargarLCO";
            this.tsmiCargarLCO.Size = new System.Drawing.Size(152, 22);
            this.tsmiCargarLCO.Text = "Cargar";
            this.tsmiCargarLCO.Click += new System.EventHandler(this.tsmiCargarLCO_Click);
            // 
            // tsmiAcuses
            // 
            this.tsmiAcuses.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAcusePAC});
            this.tsmiAcuses.Name = "tsmiAcuses";
            this.tsmiAcuses.Size = new System.Drawing.Size(56, 20);
            this.tsmiAcuses.Text = "Acuses";
            // 
            // tsmiAcusePAC
            // 
            this.tsmiAcusePAC.Name = "tsmiAcusePAC";
            this.tsmiAcusePAC.Size = new System.Drawing.Size(152, 22);
            this.tsmiAcusePAC.Text = "PAC";
            this.tsmiAcusePAC.Click += new System.EventHandler(this.tsmiAcusePAC_Click);
            // 
            // tsmiRegistroUsuarios
            // 
            this.tsmiRegistroUsuarios.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRegistro});
            this.tsmiRegistroUsuarios.Name = "tsmiRegistroUsuarios";
            this.tsmiRegistroUsuarios.Size = new System.Drawing.Size(110, 20);
            this.tsmiRegistroUsuarios.Text = "Registro Usuarios";
            // 
            // tsmiRegistro
            // 
            this.tsmiRegistro.Name = "tsmiRegistro";
            this.tsmiRegistro.Size = new System.Drawing.Size(152, 22);
            this.tsmiRegistro.Text = "Registro";
            this.tsmiRegistro.Click += new System.EventHandler(this.tsmiRegistro_Click);
            // 
            // frmModulo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 486);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "frmModulo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modulo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiEncriptacion;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenSSL;
        private System.Windows.Forms.ToolStripMenuItem tsmiValidacion;
        private System.Windows.Forms.ToolStripMenuItem tsmiZip;
        private System.Windows.Forms.ToolStripMenuItem tsmiServicios;
        private System.Windows.Forms.ToolStripMenuItem tsmiVentas;
        private System.Windows.Forms.ToolStripMenuItem tsmiEncriptaciónTexto;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelloOpenSSL;
        private System.Windows.Forms.ToolStripMenuItem tsmiEsquema32;
        private System.Windows.Forms.ToolStripMenuItem tsmiGeneraciónZip;
        private System.Windows.Forms.ToolStripMenuItem tsmiJson;
        private System.Windows.Forms.ToolStripMenuItem tsmiOutlook;
        private System.Windows.Forms.ToolStripMenuItem tsmiGeneración;
        private System.Windows.Forms.ToolStripMenuItem tsmiGeneracionSellos;
        private System.Windows.Forms.ToolStripMenuItem tsmiGeneracionComprobante22A32;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenXML;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenXmlExcel;
        private System.Windows.Forms.ToolStripMenuItem tsmiGeneracionLayout;
        private System.Windows.Forms.ToolStripMenuItem tsmiAlgoritmos;
        private System.Windows.Forms.ToolStripMenuItem tsmiPermutacionesParametros;
        private System.Windows.Forms.ToolStripMenuItem tsmiTimbrado;
        private System.Windows.Forms.ToolStripMenuItem tsmiTimbradoEnviar;
        private System.Windows.Forms.ToolStripMenuItem esquema32LayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tmiValidacionValidarLayout;
        private System.Windows.Forms.ToolStripMenuItem tsmiActualizacionComprobante;
        private System.Windows.Forms.ToolStripMenuItem tsmiActualizacionComprobantes;
        private System.Windows.Forms.ToolStripMenuItem tsmilCO;
        private System.Windows.Forms.ToolStripMenuItem tsmiTCPNET;
        private System.Windows.Forms.ToolStripMenuItem tsmiAcuses;
        private System.Windows.Forms.ToolStripMenuItem tsmiAcusePAC;
        private System.Windows.Forms.ToolStripMenuItem tsmiCargarLCO;
        private System.Windows.Forms.ToolStripMenuItem tsmiRegistroUsuarios;
        private System.Windows.Forms.ToolStripMenuItem tsmiRegistro;
    }
}