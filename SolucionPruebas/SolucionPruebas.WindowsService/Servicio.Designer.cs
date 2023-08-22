namespace SolucionPruebas.WindowsService
{
    partial class Servicio
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
            this.MyFileSystemWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.MyFileSystemWatcher)).BeginInit();
            // 
            // MyFileSystemWatcher
            // 
            this.MyFileSystemWatcher.EnableRaisingEvents = true;
            this.MyFileSystemWatcher.Filter = "*.text";
            this.MyFileSystemWatcher.NotifyFilter = ((System.IO.NotifyFilters)((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.Size)));
            this.MyFileSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.MyFileSystemWatcher_Created);
            // 
            // Servicio
            // 
            this.ServiceName = "Service1";
            ((System.ComponentModel.ISupportInitialize)(this.MyFileSystemWatcher)).EndInit();

        }

        #endregion

        private System.IO.FileSystemWatcher MyFileSystemWatcher;
    }
}
