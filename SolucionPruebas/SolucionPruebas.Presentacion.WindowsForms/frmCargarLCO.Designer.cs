namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmCargarLCO
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
            this.btnCargarLCO = new System.Windows.Forms.Button();
            this.dgvLCO = new System.Windows.Forms.DataGridView();
            this.lbArchivos = new System.Windows.Forms.ListBox();
            this.txtRFC = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dgvResultado = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLCO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultado)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCargarLCO
            // 
            this.btnCargarLCO.Location = new System.Drawing.Point(503, 34);
            this.btnCargarLCO.Name = "btnCargarLCO";
            this.btnCargarLCO.Size = new System.Drawing.Size(75, 23);
            this.btnCargarLCO.TabIndex = 0;
            this.btnCargarLCO.Text = "Cargar";
            this.btnCargarLCO.UseVisualStyleBackColor = true;
            this.btnCargarLCO.Click += new System.EventHandler(this.btnCargarLCO_Click);
            // 
            // dgvLCO
            // 
            this.dgvLCO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLCO.Location = new System.Drawing.Point(12, 151);
            this.dgvLCO.Name = "dgvLCO";
            this.dgvLCO.Size = new System.Drawing.Size(752, 224);
            this.dgvLCO.TabIndex = 1;
            // 
            // lbArchivos
            // 
            this.lbArchivos.FormattingEnabled = true;
            this.lbArchivos.Location = new System.Drawing.Point(12, 34);
            this.lbArchivos.Name = "lbArchivos";
            this.lbArchivos.Size = new System.Drawing.Size(485, 82);
            this.lbArchivos.TabIndex = 2;
            this.lbArchivos.SelectedIndexChanged += new System.EventHandler(this.lbArchivos_SelectedIndexChanged);
            // 
            // txtRFC
            // 
            this.txtRFC.Location = new System.Drawing.Point(12, 122);
            this.txtRFC.Name = "txtRFC";
            this.txtRFC.Size = new System.Drawing.Size(180, 20);
            this.txtRFC.TabIndex = 3;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(503, 122);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar.TabIndex = 4;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // dgvResultado
            // 
            this.dgvResultado.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResultado.Location = new System.Drawing.Point(12, 381);
            this.dgvResultado.Name = "dgvResultado";
            this.dgvResultado.Size = new System.Drawing.Size(752, 108);
            this.dgvResultado.TabIndex = 5;
            // 
            // frmCargarLCO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 501);
            this.Controls.Add(this.dgvResultado);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtRFC);
            this.Controls.Add(this.lbArchivos);
            this.Controls.Add(this.dgvLCO);
            this.Controls.Add(this.btnCargarLCO);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmCargarLCO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cargar LCO";
            this.Load += new System.EventHandler(this.frmCargarLCO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLCO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultado)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCargarLCO;
        private System.Windows.Forms.DataGridView dgvLCO;
        private System.Windows.Forms.ListBox lbArchivos;
        private System.Windows.Forms.TextBox txtRFC;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView dgvResultado;
    }
}