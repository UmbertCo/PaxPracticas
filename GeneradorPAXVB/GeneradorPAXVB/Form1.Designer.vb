<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.fbdCFDIOrigen = New System.Windows.Forms.FolderBrowserDialog()
        Me.GenerarXML = New System.Windows.Forms.Button()
        Me.GenerarXmlClase = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnGenerarQR = New System.Windows.Forms.Button()
        Me.ofdDocumento = New System.Windows.Forms.OpenFileDialog()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GenerarXML
        '
        Me.GenerarXML.Location = New System.Drawing.Point(21, 320)
        Me.GenerarXML.Name = "GenerarXML"
        Me.GenerarXML.Size = New System.Drawing.Size(148, 23)
        Me.GenerarXML.TabIndex = 0
        Me.GenerarXML.Text = "Timbrar XML con Sello"
        Me.GenerarXML.UseVisualStyleBackColor = True
        '
        'GenerarXmlClase
        '
        Me.GenerarXmlClase.Location = New System.Drawing.Point(175, 320)
        Me.GenerarXmlClase.Name = "GenerarXmlClase"
        Me.GenerarXmlClase.Size = New System.Drawing.Size(135, 23)
        Me.GenerarXmlClase.TabIndex = 1
        Me.GenerarXmlClase.Text = "Generar XML con Clase"
        Me.GenerarXmlClase.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(53, 45)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(100, 50)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'btnGenerarQR
        '
        Me.btnGenerarQR.Location = New System.Drawing.Point(329, 320)
        Me.btnGenerarQR.Name = "btnGenerarQR"
        Me.btnGenerarQR.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerarQR.TabIndex = 3
        Me.btnGenerarQR.Text = "Generar QR"
        Me.btnGenerarQR.UseVisualStyleBackColor = True
        '
        'ofdDocumento
        '
        Me.ofdDocumento.FileName = "OpenFileDialog1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(416, 366)
        Me.Controls.Add(Me.btnGenerarQR)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.GenerarXmlClase)
        Me.Controls.Add(Me.GenerarXML)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents fbdCFDIOrigen As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents GenerarXML As System.Windows.Forms.Button
    Friend WithEvents GenerarXmlClase As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnGenerarQR As System.Windows.Forms.Button
    Friend WithEvents ofdDocumento As System.Windows.Forms.OpenFileDialog

End Class
