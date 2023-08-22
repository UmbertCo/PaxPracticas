namespace PaxApplicationPoolService
{
    partial class ProjectInstaller
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

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstallerPaxApplicationPool = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerPaxApplicationPool = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerPaxApplicationPool
            // 
            this.serviceProcessInstallerPaxApplicationPool.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstallerPaxApplicationPool.Password = null;
            this.serviceProcessInstallerPaxApplicationPool.Username = null;
            // 
            // serviceInstallerPaxApplicationPool
            // 
            this.serviceInstallerPaxApplicationPool.Description = "Servicio encargado de reiniciar Applications Pool de IIS en caso de estar detenid" +
    "as.";
            this.serviceInstallerPaxApplicationPool.ServiceName = "PAXFacturacion Application Pool Service";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerPaxApplicationPool,
            this.serviceInstallerPaxApplicationPool});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerPaxApplicationPool;
        private System.ServiceProcess.ServiceInstaller serviceInstallerPaxApplicationPool;
    }
}