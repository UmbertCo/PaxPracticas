namespace SolucionPruebas.Presentacion.WindowsForms
{
    partial class frmValidarXML
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
            this.btnSeleccionarArchivo = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnValidar = new System.Windows.Forms.Button();
            this.txtArchivo = new System.Windows.Forms.TextBox();
            this.txtResultados = new System.Windows.Forms.TextBox();
            this.rbProductivo = new System.Windows.Forms.RadioButton();
            this.rbTest = new System.Windows.Forms.RadioButton();
            this.rbDesarrollo = new System.Windows.Forms.RadioButton();
            this.rbLocal = new System.Windows.Forms.RadioButton();
            this.rbArchivo = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnSeleccionarArchivo
            // 
            this.btnSeleccionarArchivo.Location = new System.Drawing.Point(442, 23);
            this.btnSeleccionarArchivo.Name = "btnSeleccionarArchivo";
            this.btnSeleccionarArchivo.Size = new System.Drawing.Size(117, 23);
            this.btnSeleccionarArchivo.TabIndex = 0;
            this.btnSeleccionarArchivo.Text = "Seleccionar archivo";
            this.btnSeleccionarArchivo.UseVisualStyleBackColor = true;
            this.btnSeleccionarArchivo.Click += new System.EventHandler(this.btnSeleccionarArchivo_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnValidar
            // 
            this.btnValidar.Location = new System.Drawing.Point(12, 127);
            this.btnValidar.Name = "btnValidar";
            this.btnValidar.Size = new System.Drawing.Size(75, 23);
            this.btnValidar.TabIndex = 1;
            this.btnValidar.Text = "Validar XML";
            this.btnValidar.UseVisualStyleBackColor = true;
            this.btnValidar.Click += new System.EventHandler(this.btnValidar_Click);
            // 
            // txtArchivo
            // 
            this.txtArchivo.Location = new System.Drawing.Point(12, 23);
            this.txtArchivo.Multiline = true;
            this.txtArchivo.Name = "txtArchivo";
            this.txtArchivo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtArchivo.Size = new System.Drawing.Size(424, 50);
            this.txtArchivo.TabIndex = 2;
            // 
            // txtResultados
            // 
            this.txtResultados.Location = new System.Drawing.Point(12, 156);
            this.txtResultados.Multiline = true;
            this.txtResultados.Name = "txtResultados";
            this.txtResultados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultados.Size = new System.Drawing.Size(547, 237);
            this.txtResultados.TabIndex = 3;
            // 
            // rbProductivo
            // 
            this.rbProductivo.AutoSize = true;
            this.rbProductivo.Location = new System.Drawing.Point(275, 95);
            this.rbProductivo.Name = "rbProductivo";
            this.rbProductivo.Size = new System.Drawing.Size(76, 17);
            this.rbProductivo.TabIndex = 59;
            this.rbProductivo.TabStop = true;
            this.rbProductivo.Text = "Productivo";
            this.rbProductivo.UseVisualStyleBackColor = true;
            this.rbProductivo.CheckedChanged += new System.EventHandler(this.rbProductivo_CheckedChanged);
            // 
            // rbTest
            // 
            this.rbTest.AutoSize = true;
            this.rbTest.Location = new System.Drawing.Point(223, 95);
            this.rbTest.Name = "rbTest";
            this.rbTest.Size = new System.Drawing.Size(46, 17);
            this.rbTest.TabIndex = 58;
            this.rbTest.TabStop = true;
            this.rbTest.Text = "Test";
            this.rbTest.UseVisualStyleBackColor = true;
            this.rbTest.CheckedChanged += new System.EventHandler(this.rbTest_CheckedChanged);
            // 
            // rbDesarrollo
            // 
            this.rbDesarrollo.AutoSize = true;
            this.rbDesarrollo.Location = new System.Drawing.Point(136, 95);
            this.rbDesarrollo.Name = "rbDesarrollo";
            this.rbDesarrollo.Size = new System.Drawing.Size(72, 17);
            this.rbDesarrollo.TabIndex = 57;
            this.rbDesarrollo.TabStop = true;
            this.rbDesarrollo.Text = "Desarrollo";
            this.rbDesarrollo.UseVisualStyleBackColor = true;
            this.rbDesarrollo.CheckedChanged += new System.EventHandler(this.rbDesarrollo_CheckedChanged);
            // 
            // rbLocal
            // 
            this.rbLocal.AutoSize = true;
            this.rbLocal.Location = new System.Drawing.Point(79, 95);
            this.rbLocal.Name = "rbLocal";
            this.rbLocal.Size = new System.Drawing.Size(51, 17);
            this.rbLocal.TabIndex = 56;
            this.rbLocal.TabStop = true;
            this.rbLocal.Text = "Local";
            this.rbLocal.UseVisualStyleBackColor = true;
            this.rbLocal.CheckedChanged += new System.EventHandler(this.rbLocal_CheckedChanged);
            // 
            // rbArchivo
            // 
            this.rbArchivo.AutoSize = true;
            this.rbArchivo.Location = new System.Drawing.Point(12, 95);
            this.rbArchivo.Name = "rbArchivo";
            this.rbArchivo.Size = new System.Drawing.Size(61, 17);
            this.rbArchivo.TabIndex = 60;
            this.rbArchivo.TabStop = true;
            this.rbArchivo.Text = "Archivo";
            this.rbArchivo.UseVisualStyleBackColor = true;
            this.rbArchivo.CheckedChanged += new System.EventHandler(this.rbArchivo_CheckedChanged);
            // 
            // frmValidarXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 405);
            this.Controls.Add(this.rbArchivo);
            this.Controls.Add(this.rbProductivo);
            this.Controls.Add(this.rbTest);
            this.Controls.Add(this.rbDesarrollo);
            this.Controls.Add(this.rbLocal);
            this.Controls.Add(this.txtResultados);
            this.Controls.Add(this.txtArchivo);
            this.Controls.Add(this.btnValidar);
            this.Controls.Add(this.btnSeleccionarArchivo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmValidarXML";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Validación Esquema";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSeleccionarArchivo;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnValidar;
        private System.Windows.Forms.TextBox txtArchivo;
        private System.Windows.Forms.TextBox txtResultados;
        private System.Windows.Forms.RadioButton rbProductivo;
        private System.Windows.Forms.RadioButton rbTest;
        private System.Windows.Forms.RadioButton rbDesarrollo;
        private System.Windows.Forms.RadioButton rbLocal;
        private System.Windows.Forms.RadioButton rbArchivo;
    }
}

