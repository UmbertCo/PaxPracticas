Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO

Public Class AccesoDisco

#Region "Métodos Públicos"

    Public Shared Function RecuperaListaArchivos(ByVal directorioRaiz As String) As IList
        Dim listaArchivos = Directory.GetFiles(directorioRaiz).ToList()
        Return listaArchivos
    End Function

    Public Shared Function RecuperaArchivos(ByVal rutaAbsoluta As String) As Stream
        Return File.OpenRead(rutaAbsoluta)
    End Function

    Public Shared Sub MoverArchivo(ByVal rutaAbsoluta As String, ByVal rutaDestino As String)
        Dim nombreArchivo = Path.GetFileName(rutaAbsoluta)
        File.Move(rutaAbsoluta, String.Format("{0}\\{1}", rutaDestino, nombreArchivo))
    End Sub

    Public Shared Sub GuardarArchivoLog(ByVal rutaAbsoluta As String, ByVal contenidoArchivo As List(Of String))
        File.WriteAllLines(rutaAbsoluta, contenidoArchivo.ToArray())
    End Sub

    Public Shared Sub GuardarArchivoTexto(ByVal rutaAbsoluta As String, ByVal contenidoArchivo As String)
        File.WriteAllText(rutaAbsoluta, contenidoArchivo)
    End Sub

#End Region

End Class
