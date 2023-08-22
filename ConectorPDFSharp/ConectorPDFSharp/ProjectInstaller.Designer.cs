namespace ConectorPDFSharp
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstallerConectorPDFSharp = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerConectorPDFSharp = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerConectorPDFSharp
            // 
            this.serviceProcessInstallerConectorPDFSharp.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstallerConectorPDFSharp.Password = null;
            this.serviceProcessInstallerConectorPDFSharp.Username = null;
            // 
            // serviceInstallerConectorPDFSharp
            // 
            this.serviceInstallerConectorPDFSharp.Description = "Conector de Prueba PDFSharp";
            this.serviceInstallerConectorPDFSharp.DisplayName = "Conector de Prueba PDFSharp";
            this.serviceInstallerConectorPDFSharp.ServiceName = "Conector PDFSharp";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerConectorPDFSharp,
            this.serviceInstallerConectorPDFSharp});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerConectorPDFSharp;
        private System.ServiceProcess.ServiceInstaller serviceInstallerConectorPDFSharp;
    }
}