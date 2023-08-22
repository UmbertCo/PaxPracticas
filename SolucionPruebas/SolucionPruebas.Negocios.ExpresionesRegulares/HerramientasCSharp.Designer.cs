namespace SolucionPruebas.Negocios.ExpresionesRegulares
{
    partial class HerramientasCSharp : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public HerramientasCSharp()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DevTool = this.Factory.CreateRibbonTab();
            this.Primero = this.Factory.CreateRibbonGroup();
            this.btnExpresionRegular = this.Factory.CreateRibbonButton();
            this.DevTool.SuspendLayout();
            this.Primero.SuspendLayout();
            // 
            // DevTool
            // 
            this.DevTool.Groups.Add(this.Primero);
            this.DevTool.Label = "DevTool";
            this.DevTool.Name = "DevTool";
            // 
            // Primero
            // 
            this.Primero.Items.Add(this.btnExpresionRegular);
            this.Primero.Label = "Primero";
            this.Primero.Name = "Primero";
            // 
            // btnExpresionRegular
            // 
            this.btnExpresionRegular.Label = "CURP";
            this.btnExpresionRegular.Name = "btnExpresionRegular";
            this.btnExpresionRegular.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnExpresionRegular_Click);
            // 
            // HerramientasCSharp
            // 
            this.Name = "HerramientasCSharp";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.DevTool);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.HerramientasCSharp_Load);
            this.DevTool.ResumeLayout(false);
            this.DevTool.PerformLayout();
            this.Primero.ResumeLayout(false);
            this.Primero.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab DevTool;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup Primero;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnExpresionRegular;
    }

    partial class ThisRibbonCollection
    {
        internal HerramientasCSharp HerramientasCSharp
        {
            get { return this.GetRibbon<HerramientasCSharp>(); }
        }
    }
}
