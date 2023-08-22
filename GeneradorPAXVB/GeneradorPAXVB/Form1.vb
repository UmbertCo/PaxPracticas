Imports ThoughtWorks.QRCode
Imports ThoughtWorks.QRCode.Codec
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Threading
Imports System.Xml
Imports System.Xml.XPath

Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GenerarXML.Click
        Dim rutaArchivos As String = Application.StartupPath + "\\ArchivosGenerados"
        Dim sCadenaOriginal As String = String.Empty
        Dim xml As String
        Dim xmlDocGenera As XmlDocument = New XmlDocument
        Dim numeroCertificado As String = String.Empty
        Dim sello As String = String.Empty
        Dim sSello As String
        Dim asSaltosLinea As String() = {vbCrLf, vbLf}
        fbdCFDIOrigen.ShowDialog()

        If fbdCFDIOrigen.SelectedPath <> String.Empty Then
            Try
                Dim listaArchivos = AccesoDisco.RecuperaListaArchivos(fbdCFDIOrigen.SelectedPath)
                MessageBox.Show("Cargando archivos")

                For Each nombreArchivo In listaArchivos
                    Dim archivo As Stream = File.Open(nombreArchivo, FileMode.Open)
                    Dim sr As StreamReader = New StreamReader(archivo)
                    xml = sr.ReadToEnd()
                    archivo.Close()

                    xmlDocGenera.LoadXml(xml)

                    For Each NodoHijo As XmlNode In xmlDocGenera
                        If (NodoHijo.NodeType.Equals(XmlNodeType.XmlDeclaration)) Then
                            xmlDocGenera.RemoveChild(NodoHijo)
                        End If
                    Next

                    For Each NodoHijo As XmlNode In xmlDocGenera.FirstChild.ChildNodes
                        If (NodoHijo.Name.Equals("cfdi:Complemento")) Then
                            For Each NodoComplemento As XmlNode In NodoHijo.ChildNodes
                                If (NodoComplemento.Name.Equals("tfd:TimbreFiscalDigital")) Then
                                    NodoHijo.RemoveChild(NodoComplemento)
                                End If
                            Next
                        End If
                    Next

                    For index = 1 To 100

                        xmlDocGenera.FirstChild.Attributes.ItemOf("fecha").Value = Date.Now.ToString("s")

                        'Crear el navegador para leer el xml
                        Dim nsmComprobante As XmlNamespaceManager = New XmlNamespaceManager(xmlDocGenera.NameTable)
                        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cgd/3")
                        Dim navDocGenera As XPathNavigator = xmlDocGenera.CreateNavigator()

                        sCadenaOriginal = fnConstruirCadenaTimbrado(navDocGenera)

                        Dim rutaCer As String = "CERTIFICADOS\" + "emisor.cer"
                        Dim rutaKey As String = "CERTIFICADOS\" + "emisor.key"

                        sello = fnCrearSello(rutaCer, rutaKey, "12345678a", "pem_cer", "pem_key", sCadenaOriginal)

                        sSello = String.Empty

                        For Each elemento As String In sello.Split(New String() {vbCrLf & vbLf, vbLf}, StringSplitOptions.None)
                            sSello += elemento
                        Next

                        xmlDocGenera.FirstChild.Attributes.ItemOf("sello").Value = sSello

                        'If Not xmlDocGenera.FirstChild.LastChild.Attributes.ItemOf("UUID").Value.Equals(String.Empty) Then
                        '    xmlDocGenera.RemoveChild(xmlDocGenera.LastChild)
                        'End If
                        'navDocGenera.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(sello.Replace("\n", ""))

                        'Dim rutaAbsolutaAcuse = String.Format("{0}\\{1}.xml", rutaArchivos, Guid.NewGuid().ToString("N"))
                        Dim rutaAbsolutaAcuse = String.Format("{0}\\{1}.xml", rutaArchivos, System.IO.Path.GetFileNameWithoutExtension(nombreArchivo))
                        AccesoDisco.GuardarArchivoTexto(rutaAbsolutaAcuse, xmlDocGenera.OuterXml)

                        System.Threading.Thread.Sleep(1000)
                        '1000:

                    Next

                Next

                MessageBox.Show("ArchivosGenerados")

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub

    Private Sub GenerarXmlClase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GenerarXmlClase.Click
        Dim CComprobante As Comprobante = New Comprobante()
        Dim CTimbrado As cTimbrado = New cTimbrado()
        Dim xmlComprobante As XmlDocument = New XmlDocument
        Try
            CComprobante = CTimbrado.fnObtenerXML("3.2", "1", "AAA", 12, 0, "MXN", 12, "No Aplica", "México, Chihuahua", "Pago en una sola exhibición.")
            xmlComprobante = CTimbrado.fnGenerarXML(CComprobante)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnGenerarQR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarQR.Click
        Dim xml As String
        Dim xmlDocGenera As XmlDocument = New XmlDocument

        ofdDocumento.ShowDialog()

        If ofdDocumento.FileName = String.Empty Then
            Return
        End If

        Try
            Dim archivo As Stream = File.Open(ofdDocumento.FileName, FileMode.Open)
            Dim sr As StreamReader = New StreamReader(archivo)
            xml = sr.ReadToEnd()
            archivo.Close()

            xmlDocGenera.LoadXml(xml)

            GenerarCodigoBidimensional(xmlDocGenera)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Function fnConstruirCadenaTimbrado(ByVal xml As IXPathNavigable) As String
        Dim sCadenaOriginal As String = String.Empty
        Dim HojaTrans As String = String.Empty

        Dim archivo As Stream = File.Open("cadenaoriginal_3_2.xslt", FileMode.Open)
        Dim srCadena As StreamReader = New StreamReader(archivo)
        HojaTrans = srCadena.ReadToEnd()
        archivo.Close()
        Try
            Dim ms As New MemoryStream
            Dim args As New Xsl.XsltArgumentList
            Dim trans As New Xsl.XslCompiledTransform()
            Dim sr As StreamReader

            trans.Load(XmlReader.Create(New StringReader(HojaTrans)))
            trans.Transform(xml, args, ms)
            ms.Seek(0, SeekOrigin.Begin)
            sr = New StreamReader(ms)
            sCadenaOriginal = sr.ReadToEnd()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return sCadenaOriginal
    End Function

    Public Function fnCrearSello(ByVal psPathCert As String, ByVal psPathKey As String, ByVal psPassword As String, ByVal psNombreCert As String, ByVal psNombreKey As String, ByVal psCadenaOriginal As String) As String
        Dim sello As String = String.Empty

        File.WriteAllText("PEM\cadenaOriginal.txt", psCadenaOriginal)

        fnCrearCertificadoPEM(psPathCert, psNombreCert)
        fnCrearLlavePEM(psPathKey, psPassword, psNombreKey)

        Dim info As ProcessStartInfo
        Dim proceso As Process

        info = New ProcessStartInfo("C:\OpenSSL-Win32\bin\openssl.exe", "dgst -sha1 -sign PEM\pem_key.pem -out PEM\bin.txt PEM\cadenaOriginal.txt")
        info.CreateNoWindow = True
        info.UseShellExecute = False
        proceso = Process.Start(info)
        proceso.WaitForExit()
        proceso.Dispose()

        Dim infoSello As ProcessStartInfo
        Dim procesoSello As Process

        infoSello = New ProcessStartInfo("C:\OpenSSL-Win32\bin\openssl.exe", "enc -base64 -in PEM\bin.txt -out PEM\sello.txt")
        infoSello.CreateNoWindow = True
        infoSello.UseShellExecute = False
        procesoSello = Process.Start(infoSello)
        procesoSello.WaitForExit()
        procesoSello.Dispose()

        Dim archivo As Stream = File.Open("PEM\sello.txt", FileMode.Open)
        Dim sr As StreamReader = New StreamReader(archivo)
        sello = sr.ReadToEnd()
        archivo.Close()

        Return sello
    End Function

    Public Sub fnCrearCertificadoPEM(ByVal psPathCert As String, ByVal psNombreCert As String)
        Dim info As ProcessStartInfo
        Dim proceso As Process

        info = New ProcessStartInfo("C:\OpenSSL-Win32\bin\openssl.exe", "x509 -inform DER -in " + psPathCert + " -out " + "PEM\" + psNombreCert + ".pem")

        info.CreateNoWindow = True
        info.UseShellExecute = False
        proceso = Process.Start(info)
        proceso.WaitForExit()
        proceso.Dispose()
    End Sub

    Public Sub fnCrearLlavePEM(ByVal psPathKey As String, ByVal psPassword As String, ByVal psNombreKey As String)
        Dim info As ProcessStartInfo
        Dim proceso As Process

        info = New ProcessStartInfo("C:\OpenSSL-Win32\bin\openssl.exe", "pkcs8 -inform DER -in " + psPathKey + " -passin pass:" + psPassword + " -out " + "PEM\" + psNombreKey + ".pem")
        info.CreateNoWindow = True
        info.UseShellExecute = False
        proceso = Process.Start(info)
        proceso.WaitForExit()
        proceso.Dispose()
    End Sub

    Private Sub GenerarCodigoBidimensional(ByVal gxComprobante As XmlDocument)
        Dim sCadenaCodigo As String
        'Creamos la cadena que generará el código
        Dim nsm = New XmlNamespaceManager(gxComprobante.NameTable)
        nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3")
        nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")

        Dim navCodigo As XPathNavigator = gxComprobante.CreateNavigator()

        sCadenaCodigo = "?re=" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsm).Value + _
                        "&rr" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsm).Value + _
                        "&tt" + String.Format("{0:0000000000.000000}", navCodigo.SelectSingleNode("/cfdi:Comprobante/@total", nsm).ValueAsDouble) + _
                        "&id" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value.ToUpper()

        If sCadenaCodigo.Length < 93 AndAlso sCadenaCodigo.Length > 95 Then
            Throw New Exception("Los datos para la cadena del código CBB no cumplen con la especificación de hacienda")
        End If

        Dim ce As QRCodeEncoder = New QRCodeEncoder()
        ce.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE
        ce.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q
        ce.QRCodeScale = 4
        ce.QRCodeVersion = 0

        Dim ms As MemoryStream = New MemoryStream()
        ce.Encode(sCadenaCodigo, System.Text.Encoding.UTF8).Save(ms, ImageFormat.Jpeg)

        PictureBox1.Image = Image.FromStream(ms)
    End Sub

    Private Sub fnCargarCertificado()
        Try
            ''Obtener la Llave Privada del Emisor
            'Dim FileKey As String()
            'Dim RutaKey As String = My.MySettings.Default.rutaCertificados + "\\"
            'Dim filtroKey As String = "*.key"
            'FileKey = Directory.GetFiles(RutaKey, filtroKey)

            'For Each key As String In FileKey
            '    Dim streamkey As Stream = File.Open(FileKey.ToString(), FileMode.Open)
            '    Dim srkey As StreamReader = New StreamReader(streamkey, System.Text.Encoding.UTF8, True)

            '        using (BinaryReader br = new BinaryReader(streamkey))
            '        {
            '        gLlave = br.ReadBytes(Convert.ToInt32(streamkey.Length))
            '        }
            'Next


        Catch ex As Exception

        End Try
    End Sub

End Class



