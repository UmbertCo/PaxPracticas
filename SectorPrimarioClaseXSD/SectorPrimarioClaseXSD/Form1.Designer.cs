namespace SectorPrimarioClaseXSD
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog4 = new System.Windows.Forms.OpenFileDialog();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.btnGenerarPeticion = new System.Windows.Forms.Button();
            this.txtPeticion = new System.Windows.Forms.RichTextBox();
            this.cbTipo = new System.Windows.Forms.ComboBox();
            this.cbEmisor = new System.Windows.Forms.ComboBox();
            this.cbAdquiriente = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(328, 202);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(304, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generar Consulta";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(18, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(149, 64);
            this.button2.TabIndex = 1;
            this.button2.Text = "Certificado Emisor";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(173, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(149, 64);
            this.button3.TabIndex = 2;
            this.button3.Text = "Certificado PAC";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(18, 82);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(149, 64);
            this.button4.TabIndex = 3;
            this.button4.Text = "Llave Emisor";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(173, 82);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(149, 64);
            this.button5.TabIndex = 4;
            this.button5.Text = "Llave PAC";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.FileName = "openFileDialog3";
            // 
            // openFileDialog4
            // 
            this.openFileDialog4.FileName = "openFileDialog4";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(119, 235);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(203, 20);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "AHO0505163C7";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "RFC Emisor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "RFC Adquiriente";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "RFC PAC";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 261);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Fecha";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(119, 261);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(203, 20);
            this.textBox4.TabIndex = 12;
            this.textBox4.Text = "2015-08-20T05:04:48";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(328, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(567, 184);
            this.richTextBox1.TabIndex = 13;
            this.richTextBox1.Text = "";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(119, 287);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(203, 30);
            this.button7.TabIndex = 16;
            this.button7.Text = "Regenerar Fecha";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnGenerarPeticion
            // 
            this.btnGenerarPeticion.Location = new System.Drawing.Point(784, 498);
            this.btnGenerarPeticion.Name = "btnGenerarPeticion";
            this.btnGenerarPeticion.Size = new System.Drawing.Size(111, 26);
            this.btnGenerarPeticion.TabIndex = 19;
            this.btnGenerarPeticion.Text = "GenerarPeticion";
            this.btnGenerarPeticion.UseVisualStyleBackColor = true;
            this.btnGenerarPeticion.Click += new System.EventHandler(this.btnGenerarPeticion_Click);
            // 
            // txtPeticion
            // 
            this.txtPeticion.Location = new System.Drawing.Point(328, 261);
            this.txtPeticion.Name = "txtPeticion";
            this.txtPeticion.Size = new System.Drawing.Size(567, 231);
            this.txtPeticion.TabIndex = 20;
            this.txtPeticion.Text = "";
            // 
            // cbTipo
            // 
            this.cbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTipo.FormattingEnabled = true;
            this.cbTipo.Items.AddRange(new object[] {
            "XPATH",
            "ENVELOPED",
            "XPATH(Dinamico)"});
            this.cbTipo.Location = new System.Drawing.Point(774, 206);
            this.cbTipo.Name = "cbTipo";
            this.cbTipo.Size = new System.Drawing.Size(121, 21);
            this.cbTipo.TabIndex = 21;
            this.cbTipo.SelectedIndexChanged += new System.EventHandler(this.cbTipo_SelectedIndexChanged);
            // 
            // cbEmisor
            // 
            this.cbEmisor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmisor.FormattingEnabled = true;
            this.cbEmisor.Items.AddRange(new object[] {
            "DUDB290712KN0",
            "CACX871014U63"});
            this.cbEmisor.Location = new System.Drawing.Point(119, 183);
            this.cbEmisor.Name = "cbEmisor";
            this.cbEmisor.Size = new System.Drawing.Size(203, 21);
            this.cbEmisor.TabIndex = 22;
            // 
            // cbAdquiriente
            // 
            this.cbAdquiriente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAdquiriente.FormattingEnabled = true;
            this.cbAdquiriente.Items.AddRange(new object[] {
            "CACX871014U63",
            "DUDB290712KN0"});
            this.cbAdquiriente.Location = new System.Drawing.Point(119, 209);
            this.cbAdquiriente.Name = "cbAdquiriente";
            this.cbAdquiriente.Size = new System.Drawing.Size(203, 21);
            this.cbAdquiriente.TabIndex = 23;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(18, 375);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(197, 108);
            this.textBox1.TabIndex = 24;
            this.textBox1.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 347);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "XPATH DINAMICO:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 543);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cbAdquiriente);
            this.Controls.Add(this.cbEmisor);
            this.Controls.Add(this.cbTipo);
            this.Controls.Add(this.txtPeticion);
            this.Controls.Add(this.btnGenerarPeticion);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.OpenFileDialog openFileDialog3;
        private System.Windows.Forms.OpenFileDialog openFileDialog4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btnGenerarPeticion;
        private System.Windows.Forms.RichTextBox txtPeticion;
        private System.Windows.Forms.ComboBox cbTipo;
        private System.Windows.Forms.ComboBox cbEmisor;
        private System.Windows.Forms.ComboBox cbAdquiriente;
        private System.Windows.Forms.RichTextBox textBox1;
        private System.Windows.Forms.Label label5;
    }
}