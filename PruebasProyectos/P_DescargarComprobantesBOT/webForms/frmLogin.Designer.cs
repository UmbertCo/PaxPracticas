namespace P_DescargarComprobantesBOT.webForms
{
    partial class frmLogin
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
            this.wbLogin = new System.Windows.Forms.WebBrowser();
            this.txtScript = new System.Windows.Forms.RichTextBox();
            this.txtUrl = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // wbLogin
            // 
            this.wbLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbLogin.Location = new System.Drawing.Point(0, 0);
            this.wbLogin.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbLogin.Name = "wbLogin";
            this.wbLogin.Size = new System.Drawing.Size(886, 604);
            this.wbLogin.TabIndex = 0;
            this.wbLogin.Url = new System.Uri("https://portalcfdi.facturaelectronica.sat.gob.mx/ConsultaEmisor.aspx", System.UriKind.Absolute);
            this.wbLogin.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbLogin_DocumentCompleted);
            this.wbLogin.FileDownload += new System.EventHandler(this.wbLogin_FileDownload);
            this.wbLogin.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.wbLogin_Navigated);
            this.wbLogin.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.wbLogin_Navigating);
            this.wbLogin.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.wbLogin_ProgressChanged);
            this.wbLogin.DockChanged += new System.EventHandler(this.wbLogin_DockChanged);
            this.wbLogin.RegionChanged += new System.EventHandler(this.wbLogin_RegionChanged);
            this.wbLogin.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.wbLogin_PreviewKeyDown);
            this.wbLogin.Validating += new System.ComponentModel.CancelEventHandler(this.wbLogin_Validating);
            // 
            // txtScript
            // 
            this.txtScript.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.txtScript.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtScript.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScript.ForeColor = System.Drawing.SystemColors.Window;
            this.txtScript.Location = new System.Drawing.Point(0, 474);
            this.txtScript.Name = "txtScript";
            this.txtScript.Size = new System.Drawing.Size(886, 130);
            this.txtScript.TabIndex = 1;
            this.txtScript.Text = "";
            this.txtScript.Visible = false;
            this.txtScript.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtScript_KeyUp);
            // 
            // txtUrl
            // 
            this.txtUrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUrl.Location = new System.Drawing.Point(0, 0);
            this.txtUrl.Multiline = false;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(886, 31);
            this.txtUrl.TabIndex = 2;
            this.txtUrl.Text = "";
            this.txtUrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUrl_KeyUp);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 604);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.txtScript);
            this.Controls.Add(this.wbLogin);
            this.Name = "frmLogin";
            this.Text = "Login";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogin_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmLogin_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbLogin;
        private System.Windows.Forms.RichTextBox txtScript;
        private System.Windows.Forms.RichTextBox txtUrl;

    }
}