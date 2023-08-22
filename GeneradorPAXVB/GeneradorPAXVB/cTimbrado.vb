Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class cTimbrado

    Public Function fnObtenerXML(ByVal sVersion As String, ByVal sFolio As String, ByVal sSerie As String, ByVal nSubtotal As Decimal,
                                 ByVal nDescuento As Decimal, ByVal sMoneda As String, ByVal nTotal As Decimal, ByVal sMetodoPago As String,
                                 ByVal sLugarExpedicion As String, ByVal sFormaPago As String) As Comprobante

        Dim CComprobante As New Comprobante()
        Dim CComprobanteEmisor As New ComprobanteEmisor()
        Dim CComprobanteReceptor As New ComprobanteReceptor()
        Dim ListaRegimenFiscal As New List(Of ComprobanteEmisorRegimenFiscal)
        Dim CRegimenFiscal As New ComprobanteEmisorRegimenFiscal()
        Dim CUbicacionFiscal As New t_UbicacionFiscal()
        Dim CDomicilio As New t_Ubicacion()

        CComprobante.version = sVersion
        CComprobante.folio = sFolio
        CComprobante.serie = sSerie
        CComprobante.fecha = Convert.ToDateTime(Today.ToString("s"))
        CComprobante.tipoDeComprobante = ComprobanteTipoDeComprobante.ingreso
        CComprobante.subTotal = nSubtotal
        CComprobante.descuento = nDescuento
        CComprobante.Moneda = sMoneda
        CComprobante.total = nTotal
        CComprobante.metodoDePago = sMetodoPago
        CComprobante.LugarExpedicion = sLugarExpedicion
        CComprobante.formaDePago = sFormaPago

        CComprobanteEmisor.nombre = "PruebaEmisor"
        CComprobanteEmisor.rfc = "AAA010101AAA"
        CUbicacionFiscal.calle = "X"
        CUbicacionFiscal.noExterior = "000"
        CUbicacionFiscal.pais = "X"
        CUbicacionFiscal.estado = "X"
        CUbicacionFiscal.municipio = "X"
        CUbicacionFiscal.localidad = "X"
        CUbicacionFiscal.codigoPostal = "X"
        CUbicacionFiscal.colonia = "X"
        CUbicacionFiscal.referencia = "X"
        CComprobanteEmisor.DomicilioFiscal = CUbicacionFiscal
        CRegimenFiscal.Regimen = "NA"
        ListaRegimenFiscal.Add(CRegimenFiscal)
        CComprobanteEmisor.RegimenFiscal = ListaRegimenFiscal.ToArray()

        CComprobanteReceptor.nombre = "PruebaReceptor"
        CComprobanteReceptor.rfc = "AAA020202BBB"
        CDomicilio.pais = "X"
        CDomicilio.estado = "X"
        CDomicilio.localidad = "X"
        CDomicilio.codigoPostal = "X"
        CComprobanteReceptor.Domicilio = CDomicilio

        CComprobante.Emisor = CComprobanteEmisor
        CComprobante.Receptor = CComprobanteReceptor

        Return CComprobante
    End Function

    Public Function fnGenerarXML(ByVal CComprobante As Comprobante) As Xml.XmlDocument
        Dim ms As MemoryStream = New MemoryStream
        Dim sw As StreamWriter = New StreamWriter(ms, System.Text.Encoding.UTF8)
        Dim xXml As Xml.XmlDocument = New Xml.XmlDocument
        Dim sns As XmlSerializerNamespaces = New XmlSerializerNamespaces()
        sns.Add("cfdi", "http://www.sat.gob.mx/cfd/3")

        Dim serializador As XmlSerializer = New XmlSerializer(GetType(Comprobante))

        Try
            serializador.Serialize(sw, CComprobante, sns)
            ms.Seek(0, SeekOrigin.Begin)
            Dim sr As StreamReader = New StreamReader(ms)

            xXml.LoadXml(sr.ReadToEnd())

            Dim att As XmlAttribute = xXml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance")
            att.Value = "http://www.sat.gob.mx/cdf/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd"
            xXml.DocumentElement.SetAttributeNode(att)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return xXml
    End Function

End Class
