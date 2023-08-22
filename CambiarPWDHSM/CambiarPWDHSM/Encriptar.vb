Imports Utilerias
Imports System.Text


Public Class Encriptar

 



    Private Sub btnWSDL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        txtResultado.Text = Utilerias.Encriptacion.Base64.EncriptarBase64(Utilerias.Encriptacion.Classica.Encriptar(txtValor.Text))
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

        txtResultado.Text = Encoding.UTF8.GetString(Utilerias.Encriptacion.DES3.Desencriptar(Convert.FromBase64String(txtValor.Text)))

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If cmbTipo.Text = "Base64" Then
            txtResultado.Text = Utilerias.Encriptacion.Base64.DesencriptarBase64(txtValor.Text)
        ElseIf cmbTipo.Text = "Clasica" Then
            txtResultado.Text = Utilerias.Encriptacion.Classica.Desencriptar(txtValor.Text)

        End If
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If cmbTipo.Text = "Base64" Then
            txtResultado.Text = Utilerias.Encriptacion.Base64.EncriptarBase64(txtValor.Text)
        ElseIf cmbTipo.Text = "Clasica" Then

            txtResultado.Text = Utilerias.Encriptacion.Classica.Encriptar(txtValor.Text)
        End If
    End Sub

    Private Sub btn_Borrar_Click(sender As System.Object, e As System.EventArgs) Handles btn_Borrar.Click
        txtResultado.Text = ""
        txtValor.Text = ""
    End Sub

    Private Sub Encriptar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
