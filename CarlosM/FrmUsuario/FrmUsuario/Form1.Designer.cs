namespace FrmUsuario
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtApPat = new System.Windows.Forms.TextBox();
            this.txtApMat = new System.Windows.Forms.TextBox();
            this.txtDir = new System.Windows.Forms.TextBox();
            this.txtTel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnInserta = new System.Windows.Forms.Button();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnElimina = new System.Windows.Forms.Button();
            this.cbSexo = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bntElimina = new System.Windows.Forms.Button();
            this.txtborrarID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(78, 22);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(100, 20);
            this.txtNombre.TabIndex = 1;
            // 
            // txtApPat
            // 
            this.txtApPat.Location = new System.Drawing.Point(78, 48);
            this.txtApPat.Name = "txtApPat";
            this.txtApPat.Size = new System.Drawing.Size(100, 20);
            this.txtApPat.TabIndex = 1;
            // 
            // txtApMat
            // 
            this.txtApMat.Location = new System.Drawing.Point(78, 74);
            this.txtApMat.Name = "txtApMat";
            this.txtApMat.Size = new System.Drawing.Size(100, 20);
            this.txtApMat.TabIndex = 1;
            // 
            // txtDir
            // 
            this.txtDir.Location = new System.Drawing.Point(78, 100);
            this.txtDir.Name = "txtDir";
            this.txtDir.Size = new System.Drawing.Size(100, 20);
            this.txtDir.TabIndex = 1;
            // 
            // txtTel
            // 
            this.txtTel.Location = new System.Drawing.Point(78, 126);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(100, 20);
            this.txtTel.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Apellido Pat.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Apellido Mat.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Direccion";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Telefono";
            // 
            // btnInserta
            // 
            this.btnInserta.Location = new System.Drawing.Point(78, 152);
            this.btnInserta.Name = "btnInserta";
            this.btnInserta.Size = new System.Drawing.Size(75, 23);
            this.btnInserta.TabIndex = 2;
            this.btnInserta.Text = "Insertar.";
            this.btnInserta.UseVisualStyleBackColor = true;
            this.btnInserta.Click += new System.EventHandler(this.btnInserta_Click);
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(78, 203);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(40, 20);
            this.txtId.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(54, 206);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "ID";
            // 
            // btnElimina
            // 
            this.btnElimina.Location = new System.Drawing.Point(78, 229);
            this.btnElimina.Name = "btnElimina";
            this.btnElimina.Size = new System.Drawing.Size(75, 23);
            this.btnElimina.TabIndex = 4;
            this.btnElimina.Text = "Eliminar";
            this.btnElimina.UseVisualStyleBackColor = true;
            this.btnElimina.Click += new System.EventHandler(this.btnElimina_Click);
            // 
            // cbSexo
            // 
            this.cbSexo.DisplayMember = "sexo";
            this.cbSexo.FormattingEnabled = true;
            this.cbSexo.Location = new System.Drawing.Point(243, 22);
            this.cbSexo.Name = "cbSexo";
            this.cbSexo.Size = new System.Drawing.Size(121, 21);
            this.cbSexo.TabIndex = 5;
            this.cbSexo.ValueMember = "id";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(200, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Sexo?";
            // 
            // bntElimina
            // 
            this.bntElimina.Location = new System.Drawing.Point(243, 152);
            this.bntElimina.Name = "bntElimina";
            this.bntElimina.Size = new System.Drawing.Size(75, 23);
            this.bntElimina.TabIndex = 7;
            this.bntElimina.Text = "Eliminar";
            this.bntElimina.UseVisualStyleBackColor = true;
            this.bntElimina.Click += new System.EventHandler(this.bntElimina_Click);
            // 
            // txtborrarID
            // 
            this.txtborrarID.Location = new System.Drawing.Point(243, 126);
            this.txtborrarID.Name = "txtborrarID";
            this.txtborrarID.Size = new System.Drawing.Size(37, 20);
            this.txtborrarID.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 196);
            this.Controls.Add(this.txtborrarID);
            this.Controls.Add(this.bntElimina);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbSexo);
            this.Controls.Add(this.btnElimina);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.btnInserta);
            this.Controls.Add(this.txtTel);
            this.Controls.Add(this.txtDir);
            this.Controls.Add(this.txtApMat);
            this.Controls.Add(this.txtApPat);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtApPat;
        private System.Windows.Forms.TextBox txtApMat;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.TextBox txtTel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnInserta;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnElimina;
        private System.Windows.Forms.ComboBox cbSexo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button bntElimina;
        private System.Windows.Forms.TextBox txtborrarID;
    }
}

