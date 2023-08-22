namespace P_DescargarComprobantesBOT.webForms
{
    partial class frmConsulta
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
            this.gvColsulta = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblRegistros = new System.Windows.Forms.Label();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.lblFechaInicial = new System.Windows.Forms.Label();
            this.dtFin = new System.Windows.Forms.DateTimePicker();
            this.dtIni = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.gvColsulta)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvColsulta
            // 
            this.gvColsulta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvColsulta.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gvColsulta.Location = new System.Drawing.Point(0, 67);
            this.gvColsulta.Name = "gvColsulta";
            this.gvColsulta.Size = new System.Drawing.Size(1173, 565);
            this.gvColsulta.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblRegistros);
            this.panel1.Controls.Add(this.btnConsultar);
            this.panel1.Controls.Add(this.lblFechaFin);
            this.panel1.Controls.Add(this.lblFechaInicial);
            this.panel1.Controls.Add(this.dtFin);
            this.panel1.Controls.Add(this.dtIni);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MaximumSize = new System.Drawing.Size(1173, 77);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1173, 77);
            this.panel1.TabIndex = 5;
            // 
            // lblRegistros
            // 
            this.lblRegistros.AutoSize = true;
            this.lblRegistros.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegistros.Location = new System.Drawing.Point(1024, 12);
            this.lblRegistros.Name = "lblRegistros";
            this.lblRegistros.Size = new System.Drawing.Size(118, 24);
            this.lblRegistros.TabIndex = 16;
            this.lblRegistros.Text = "#Registros: 0";
            // 
            // btnConsultar
            // 
            this.btnConsultar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnConsultar.Location = new System.Drawing.Point(284, 31);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(105, 34);
            this.btnConsultar.TabIndex = 15;
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.UseVisualStyleBackColor = true;
            this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFin.Location = new System.Drawing.Point(155, 12);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(110, 24);
            this.lblFechaFin.TabIndex = 14;
            this.lblFechaFin.Text = "Fecha Final";
            // 
            // lblFechaInicial
            // 
            this.lblFechaInicial.AutoSize = true;
            this.lblFechaInicial.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaInicial.Location = new System.Drawing.Point(30, 12);
            this.lblFechaInicial.Name = "lblFechaInicial";
            this.lblFechaInicial.Size = new System.Drawing.Size(116, 24);
            this.lblFechaInicial.TabIndex = 13;
            this.lblFechaInicial.Text = "Fecha Inicial";
            // 
            // dtFin
            // 
            this.dtFin.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFin.CustomFormat = "dd-MM-yyyy";
            this.dtFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtFin.Location = new System.Drawing.Point(159, 39);
            this.dtFin.Name = "dtFin";
            this.dtFin.Size = new System.Drawing.Size(119, 26);
            this.dtFin.TabIndex = 12;
            // 
            // dtIni
            // 
            this.dtIni.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtIni.CustomFormat = "dd-MM-yyyy";
            this.dtIni.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtIni.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtIni.Location = new System.Drawing.Point(34, 39);
            this.dtIni.Name = "dtIni";
            this.dtIni.Size = new System.Drawing.Size(119, 26);
            this.dtIni.TabIndex = 11;
            // 
            // frmConsulta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 632);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gvColsulta);
            this.Name = "frmConsulta";
            this.Text = "Consulta";
            this.ResizeEnd += new System.EventHandler(this.frmConsulta_ResizeEnd);
          
            this.Move += new System.EventHandler(this.frmConsulta_Move);
            ((System.ComponentModel.ISupportInitialize)(this.gvColsulta)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gvColsulta;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblRegistros;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.Label lblFechaInicial;
        private System.Windows.Forms.DateTimePicker dtFin;
        private System.Windows.Forms.DateTimePicker dtIni;
    }
}