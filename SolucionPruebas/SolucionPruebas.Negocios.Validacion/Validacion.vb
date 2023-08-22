Imports System
Imports System.Xml
Imports System.Xml.Linq


Public Class Validacion

    Public Function ValidarId(ByVal pnId As Integer) As Boolean
        Dim xdValidacion = XDocument.Load("Validacion.xml")



        'Dentro(From valida In xdValidacion.<Validacion>.<Perfiles>.<IdPerfil>.<ParaAdminstrar>, pnId)


    End Function

    Public Function Dentro(ByVal elemento As XElement, ByVal pnId As Int32) As Boolean
        Dim bResultado As Boolean = False
        Dim query = elemento.Value = pnId

        If query <> vbNull Then
            bResultado = True
        End If

        Return bResultado
    End Function



End Class
