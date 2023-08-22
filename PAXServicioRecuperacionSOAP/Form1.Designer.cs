namespace PAXServicioRecuperacionSOAP
{
    partial class Form1
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

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnRequest = new System.Windows.Forms.Button();
            this.btnResponse = new System.Windows.Forms.Button();
            this.lblPrueba = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(17, 23);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(93, 78);
            this.btnIniciar.TabIndex = 0;
            this.btnIniciar.Text = "Iniciar Prueba";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnRequest
            // 
            this.btnRequest.Enabled = false;
            this.btnRequest.Location = new System.Drawing.Point(158, 23);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(93, 78);
            this.btnRequest.TabIndex = 1;
            this.btnRequest.Text = "Ver Request";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // btnResponse
            // 
            this.btnResponse.Enabled = false;
            this.btnResponse.Location = new System.Drawing.Point(299, 23);
            this.btnResponse.Name = "btnResponse";
            this.btnResponse.Size = new System.Drawing.Size(93, 78);
            this.btnResponse.TabIndex = 2;
            this.btnResponse.Text = "Ver Response";
            this.btnResponse.UseVisualStyleBackColor = true;
            this.btnResponse.Click += new System.EventHandler(this.btnResponse_Click);
            // 
            // lblPrueba
            // 
            this.lblPrueba.AutoSize = true;
            this.lblPrueba.Location = new System.Drawing.Point(14, 124);
            this.lblPrueba.Name = "lblPrueba";
            this.lblPrueba.Size = new System.Drawing.Size(55, 13);
            this.lblPrueba.TabIndex = 3;
            this.lblPrueba.Text = "Resultado";
            this.lblPrueba.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 155);
            this.Controls.Add(this.lblPrueba);
            this.Controls.Add(this.btnResponse);
            this.Controls.Add(this.btnRequest);
            this.Controls.Add(this.btnIniciar);
            this.Name = "Form1";
            this.Text = "Iniciar Prueba";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.Button btnResponse;
        private System.Windows.Forms.Label lblPrueba;

    }
}

