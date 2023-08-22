//-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Text;
using System.Collections.Generic;
//-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
public class Complementos
{
    public static List<String> ComplementosCCE11 = new List<String>()
    {
        "implocal:ImpuestosLocales",
        "leyendasFisc:LeyendasFiscales",
        "pago20:Pagos",
        "registrofiscal:CFDIRegistroFiscal",
        "cce11:ComercioExterior",
        "tfd:TimbreFiscalDigital"
    };

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaDivisas(string version, string operacion)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<divisas:Divisas xmlns:divisas=\"http://www.sat.gob.mx/divisas\" version=\"" + version + "\" tipoOperacion=\"" +
           operacion + "\"/>");

        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaDonativas(string version, string fecha, string noAutorizacion, string leyenda)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<donat:Donatarias xmlns:donat=\"http://www.sat.gob.mx/donat\" version=\"" + version + "\" fechaAutorizacion=\"" +
           fecha + "\" noAutorizacion=\"" + noAutorizacion +
            "\" leyenda=\"" + leyenda + "\"/>");

        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaECB(string version, string numeroCuenta, string nombreCliente, string periodo, string sucursal, string fecha_ECBFiscal, string referencia_ECBFiscal, string descripcion_ECBFiscal, string importe_ECBFiscal, string moneda_ECBFiscal, string saldoInicial_ECBFiscal,
       string saldoAlCorte_ECBFiscal, string RFCenajenante_ECBFiscal, DataTable DT)
    {
        //string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        //*******EstadoDeCuentaBancario*****
        string sEstadoDeCuentaBancario = "<ecb:EstadoDeCuentaBancario xmlns:ecb=\"http://www.sat.gob.mx/ecb\" version=\"" + version +
            "\" numeroCuenta=\"" + numeroCuenta + "\" nombreCliente=\"" + nombreCliente + "\" periodo=\"" + periodo + "\"";
        if (!string.IsNullOrEmpty(sucursal))
            sEstadoDeCuentaBancario += " sucursal=\"" + sucursal + "\"";
        sEstadoDeCuentaBancario += ">";
        sb.Append(sEstadoDeCuentaBancario);

        //sb.Append("<ecb:EstadoDeCuentaBancario xmlns:ecb=\"http://www.sat.gob.mx/ecb\" version=\"" + version +
        //"\" numeroCuenta=\"" + numeroCuenta + "\" nombreCliente=\"" + nombreCliente + "\" periodo=\"" + periodo + "\">");
        //*******Movimientos***********
        sb.Append("<ecb:Movimientos>");
        if (string.IsNullOrEmpty(RFCenajenante_ECBFiscal))
        {
            foreach (DataRow dr in DT.Rows)
            {
                //*********MovimientoECB************
                string sMovimientosECB = "<ecb:MovimientoECB fecha=\"" + fecha_ECBFiscal + "\"";
                //sb.Append("<ecb:MovimientoECB fecha=\"" + fecha_ECB + "\" descripcion=\"" + descripcion_ECB + "\" importe=\"" + importe_ECB + "\"/>");

                if (!string.IsNullOrEmpty(dr["referencia"].ToString()))
                    sMovimientosECB += " referencia=\"" + dr["referencia"].ToString() + "\"";

                sMovimientosECB += " descripcion=\"" + dr["descripcion"].ToString() + "\" importe=\"" + dr["importe"].ToString() + "\"";

                if (!string.IsNullOrEmpty(moneda_ECBFiscal))
                    sMovimientosECB += " moneda=\"" + moneda_ECBFiscal + "\"";

                if (!string.IsNullOrEmpty(saldoInicial_ECBFiscal))
                    sMovimientosECB += " saldoInicial=\"" + saldoInicial_ECBFiscal + "\"";

                if (!string.IsNullOrEmpty(saldoAlCorte_ECBFiscal))
                    sMovimientosECB += " saldoAlCorte=\"" + saldoAlCorte_ECBFiscal + "\"";

                sMovimientosECB += "/>";
                sb.Append(sMovimientosECB);
                //*********FIN MovimientoECB************
            }
        }
        else
        {
            foreach (DataRow dr in DT.Rows)
            {
                //**********MovimientoECBFiscal*************
                //sb.Append("<ecb:MovimientoECBFiscal fecha=\"" + fecha + "\" descripcion=\"" + descripcion + "\" RFCenajenante=\"" + RFCenajenante + "\" Importe=\"" + importe + "\" />");
                string sMovimientosECBFiscal = "<ecb:MovimientoECBFiscal fecha=\"" + fecha_ECBFiscal + "\"";
                //sb.Append("<ecb:MovimientoECB fecha=\"" + fecha_ECB + "\" descripcion=\"" + descripcion_ECB + "\" importe=\"" + importe_ECB + "\"/>");

                if (!string.IsNullOrEmpty(referencia_ECBFiscal))
                    sMovimientosECBFiscal += " referencia=\"" + dr["referencia"].ToString() + "\"";

                sMovimientosECBFiscal += " descripcion=\"" + dr["descripcion"].ToString() + "\" RFCenajenante=\"" + RFCenajenante_ECBFiscal + "\" Importe=\"" + dr["importe"].ToString() + "\"";

                if (!string.IsNullOrEmpty(moneda_ECBFiscal))
                    sMovimientosECBFiscal += " moneda=\"" + moneda_ECBFiscal + "\"";

                if (!string.IsNullOrEmpty(saldoInicial_ECBFiscal))
                    sMovimientosECBFiscal += " saldoInicial=\"" + saldoInicial_ECBFiscal + "\"";

                if (!string.IsNullOrEmpty(saldoAlCorte_ECBFiscal))
                    sMovimientosECBFiscal += " saldoAlCorte=\"" + saldoAlCorte_ECBFiscal + "\"";

                sMovimientosECBFiscal += "/>";
                sb.Append(sMovimientosECBFiscal);
                //**********FIN MovimientoECBFiscal*************
            }
        }
        sb.Append("</ecb:Movimientos>");
        //*******FIN Movimientos***********
        sb.Append("</ecb:EstadoDeCuentaBancario>");
        //*******FIN EstadoDeCuentaBancario*****
        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaECC(string tipoOperacion, string numeroCuenta, double total, string identificador, string fecha, string rfc, string claveEstacion, double cantidad,
        string nombreCombustible, string folioOperacion, double valorUnitario, double importe, string impuesto, double tasa)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<ecc:EstadoDeCuentaCombustible xmlns:ecc=\"http://www.sat.gob.mx/ecc\" tipoOperacion=\"" + tipoOperacion +
            "\" numeroDeCuenta=\"" + numeroCuenta + "\" total=\"" + total + "\">");

        sb.Append("<ecc:Conceptos>");

        sb.Append("<ecc:ConceptoEstadoDeCuentaCombustible identificador=\"" + identificador + "\" fecha=\"" +
        fecha + "\" rfc=\"" + rfc + "\" claveEstacion=\"" + claveEstacion + "\" cantidad=\"" +
        cantidad + "\" nombreCombustible=\"" + nombreCombustible + "\" folioOperacion=\"" +
        folioOperacion + "\" valorUnitario=\"" + valorUnitario + "\" importe=\"" + importe + "\">");

        sb.Append("<ecc:Traslados>");
        sb.Append("<ecc:Traslado impuesto=\"" + impuesto + "\" tasa=\"" + tasa + "\" importe=\"" + importe + "\">");
        sb.Append("</ecc:Traslado>");
        sb.Append("</ecc:Traslados>");

        sb.Append("</ecc:ConceptoEstadoDeCuentaCombustible>");

        sb.Append("</ecc:Conceptos>");

        sb.Append("</ecc:EstadoDeCuentaCombustible>");

        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaImpLocal(string version, double TotaldeRetenciones, double TotaldeTraslados)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<implocal:ImpuestosLocales xmlns:implocal=\"http://www.sat.gob.mx/implocal\" version=\"" + version +
            "\" TotaldeRetenciones=\"" + TotaldeRetenciones + "\" TotaldeTraslados=\"" + TotaldeTraslados + "\"/>");

        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaPSGECFD(string nombre, string rfc, string noCertificado, string fechaPublicacion, string noAutorizacion, string selloDelPSGECFD)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<psgecfd:PrestadoresDeServiciosDeCFD xmlns:psgecfd=\"http://www.sat.gob.mx/psgecfd\" nombre=\"" +
           nombre + "\" rfc=\"" + rfc + "\" noCertificado=\"" + noCertificado + "\" fechaAutorizacion=\"" + fechaPublicacion +
            "\" noAutorizacion=\"" + noAutorizacion + "\" selloDelPSGECFD=\"" + selloDelPSGECFD + "\"/>");

        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaIEDU(string version, string nombrealumno, string curp,
        string niveleducativo, string autRVOE, string rfcPago)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();
        sb.Append("<iedu:instEducativas xmlns:iedu=\"http://www.sat.gob.mx/iedu\" version=\"" + version + "\" nombreAlumno=\"" +
           nombrealumno + "\" CURP=\"" + curp + "\" nivelEducativo=\"" + niveleducativo + "\" autRVOE=\"" + autRVOE + "\" rfcPago=\"" + rfcPago + "\"/>");
        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaDetallista(string version, string reference, string party1, string party2, string TotalNo, string specialservices,
      double valor, double valor2, string gln2, string gln3, string TipoBase, string documentestatus)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();
        string type = "ON";

        sb.Append("<detallista:detallista xmlns:detallista=\"http://www.sat.gob.mx/detallista\" documentStructureVersion=\"" + version + "\" documentStatus=\"" +
                  documentestatus + "\">");

        sb.Append("<detallista:requestForPaymentIdentification>");
        sb.Append("<detallista:entityType>");
        sb.Append(TipoBase);
        sb.Append("</detallista:entityType>");
        sb.Append("</detallista:requestForPaymentIdentification>");

        sb.Append("<detallista:orderIdentification>");
        sb.Append("<detallista:referenceIdentification type=\"" + type + "\">");
        sb.Append(reference);
        sb.Append("</detallista:referenceIdentification>");
        sb.Append("</detallista:orderIdentification>");

        sb.Append("<detallista:AdditionalInformation>");
        sb.Append("<detallista:referenceIdentification type=\"" + type + "\">");
        sb.Append(reference);
        sb.Append("</detallista:referenceIdentification>");
        sb.Append("</detallista:AdditionalInformation>");

        sb.Append("<detallista:buyer>");
        sb.Append("<detallista:gln>");
        sb.Append(gln2);
        sb.Append("</detallista:gln>");
        sb.Append("</detallista:buyer>");

        sb.Append("<detallista:seller>");
        sb.Append("<detallista:gln>");
        sb.Append(gln3);
        sb.Append("</detallista:gln>");
        sb.Append("<detallista:alternatePartyIdentification type=\"" + party2 + "\">");
        sb.Append(party1);
        sb.Append("</detallista:alternatePartyIdentification>");
        sb.Append("</detallista:seller>");

        sb.Append("<detallista:totalAmount>");
        sb.Append("<detallista:Amount>");
        sb.Append(valor);
        sb.Append("</detallista:Amount>");
        sb.Append("</detallista:totalAmount>");

        sb.Append("<detallista:TotalAllowanceCharge allowanceOrChargeType=\"" + TotalNo + "\">");
        sb.Append("<detallista:specialServicesType>");
        sb.Append(specialservices);
        sb.Append("</detallista:specialServicesType>");
        sb.Append("<detallista:Amount>");
        sb.Append(valor2);
        sb.Append("</detallista:Amount>");
        sb.Append("</detallista:TotalAllowanceCharge>");

        sb.Append("</detallista:detallista>");

        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaLeyendaFiscal(string versionLeyenda, string disposicionfiscal, string norma, string textoleyenda)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<leyendasFisc:LeyendasFiscales xmlns:leyendasFisc=\"http://www.sat.gob.mx/leyendasFiscales\" version=\"" + versionLeyenda + "\">");
        sb.Append("<leyendasFisc:Leyenda disposicionFiscal=\"" + disposicionfiscal + "\" norma=\"" + norma + "\" textoLeyenda=\"" + textoleyenda + "\"/>");
        sb.Append("</leyendasFisc:LeyendasFiscales>");
        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnPSGECFD(string nombre, string rfc, string noCertificado, DateTime fechaPublicacion, string noAutorizacion, string selloDelPSGECFD)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);
        string FechaAut = fechaPublicacion.ToString("s");
        string sCadenaOriginal = "|" + nombre + "|" + rfc + "|" + noCertificado + "|" + FechaAut + "|" + noAutorizacion + "||";
        string tNameSpace = "http://www.sat.gob.mx/psgecfd http://www.sat.gob.mx/sitio_internet/cfd/psgecfd/psgecfd.xsd";
        string PSGECFD = fnConstruirCadenaPSGECFD(nombre, rfc, noCertificado, FechaAut, noAutorizacion, selloDelPSGECFD);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = PSGECFD;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnImpuestosLocales(string version, double TotaldeRetenciones, double TotaldeTraslados)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);

        string sCadenaOriginal = "|" + version + "|" + TotaldeRetenciones + "|" + TotaldeTraslados + "||";
        string tNameSpace = "http://www.sat.gob.mx/implocal http://www.sat.gob.mx/sitio_internet/cfd/implocal/implocal.xsd";
        string NodoImpuestosLocales = fnConstruirCadenaImpLocal(version, TotaldeRetenciones, TotaldeTraslados);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoImpuestosLocales;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnDivisas(string versionDivisa, string operacion)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);

        string sCadenaOriginalDivisa = "|" + versionDivisa + "|" + operacion + "||";
        string tNameSpace = "http://www.sat.gob.mx/divisas http://www.sat.gob.mx/sitio_internet/cfd/divisas/Divisas.xsd";
        string NodoDivisas = fnConstruirCadenaDivisas(versionDivisa, operacion);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginalDivisa;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoDivisas;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnDonativas(string version, DateTime Fecha, string noAutorizacion, string leyenda)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);



        string FechaAut = Fecha.ToString("yyyy-MM-dd");
        string sCadenaOriginal = "|" + version + "|" + noAutorizacion + "|" + FechaAut + "||";
        string tNameSpace = "http://www.sat.gob.mx/donat http://www.sat.gob.mx/sitio_internet/cfd/donat/donat.xsd";
        string NodoDonativas = fnConstruirCadenaDonativas(version, FechaAut, noAutorizacion, leyenda);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoDonativas;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnECB(string version, string numeroCuenta, string nombreCliente, string periodo, string sucursal, DateTime fecha_ECBFiscal, string referencia_ECBFiscal, string descripcion_ECBFiscal, string importe_ECBFiscal, string moneda_ECBFiscal, string saldoInicial_ECBFiscal,
       string saldoAlCorte_ECBFiscal, string RFCenajenante_ECBFiscal, DataTable DT)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);
        string FechaAut = fecha_ECBFiscal.ToString("s");

        string sCadenaOriginal = string.Empty;
        if (string.IsNullOrEmpty(RFCenajenante_ECBFiscal))
            sCadenaOriginal = "|" + version + "|" + numeroCuenta + "|" + nombreCliente + "||";
        else
            sCadenaOriginal = "|" + version + "|" + numeroCuenta + "|" + nombreCliente + "|" + FechaAut + "|" + RFCenajenante_ECBFiscal + "|" + importe_ECBFiscal + "||";

        string tNameSpace = "ecb" + "|" + "http://www.sat.gob.mx/ecb" + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/ecb/ecb.xsd";
        string NodoECB = fnConstruirCadenaECB(version, numeroCuenta, nombreCliente, periodo, sucursal, FechaAut, referencia_ECBFiscal, descripcion_ECBFiscal, importe_ECBFiscal, moneda_ECBFiscal, saldoInicial_ECBFiscal,
                        saldoAlCorte_ECBFiscal, RFCenajenante_ECBFiscal, DT);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoECB;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnECC(string tipoOperacion, string numeroCuenta, double total, string identificador, DateTime fecha, string rfc, string claveEstacion, double cantidad,
        string nombreCombustible, string folioOperacion, double valorUnitario, double importe, string impuesto, double tasa)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);

        string FechaAut = fecha.ToString("s");

        string sCadenaOriginal = "|" + tipoOperacion + "|" + numeroCuenta + "|" + total + "|" + identificador + "|" + FechaAut + "|" + rfc +
               "|" + claveEstacion + "|" + cantidad + "|" + nombreCombustible + "|" + folioOperacion + "|" + valorUnitario + "|" + importe +
               "|" + impuesto + "|" + tasa + "|" + importe + "||";
        string tNameSpace = "http://www.sat.gob.mx/ecc http://www.sat.gob.mx/sitio_internet/cfd/ecc/ecc.xsd";
        string NodoECC = fnConstruirCadenaECC(tipoOperacion, numeroCuenta, total, identificador, FechaAut, rfc, claveEstacion, cantidad, nombreCombustible,
            folioOperacion, valorUnitario, importe, impuesto, tasa);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoECC;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnIEDU(string version, string nombrealumno, string curp,
        string niveleducativo, string autRVOE, string rfcPago)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);
        string sCadenaOriginal = "|" + version + "|" + nombrealumno + "|" + curp + "|" + niveleducativo + "|" + autRVOE + "|" + rfcPago + "||";
        string tNameSpace = "http://www.sat.gob.mx/iedu http://www.sat.gob.mx/sitio_internet/cfd/iedu/iedu.xsd";
        string NodoIEDU = fnConstruirCadenaIEDU(version, nombrealumno, curp, niveleducativo, autRVOE, rfcPago);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoIEDU;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnDetallista(string version, string reference, string party1, string party2, string TotalNo, string specialservices,
      double valor, double valor2, string gln2, string gln3, string TipoBase, string documentestatus)
    {

        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);
        string sCadenaOriginal = "|" + version + "|" + reference + "|" + gln2 + "|" + gln3 + "|" + party1 + "|" + valor + "|" + specialservices + "|" + valor2 + "||";
        string tNameSpace = "http://www.sat.gob.mx/detallista http://www.sat.gob.mx/sitio_internet/cfd/detallista/detallista.xsd";
        string NodoDetallista = fnConstruirCadenaDetallista(version, reference, party1, party2, TotalNo, specialservices, valor, valor2, gln2, gln3, TipoBase, documentestatus);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoDetallista;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnLeyendasFiscales(string versionLeyenda, string disposicionfiscal, string norma, string textoleyenda)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);

        string sCadenaOriginalDivisa = "|" + versionLeyenda + "|" + disposicionfiscal + "|" + norma + "|" + textoleyenda + "||";
        string tNameSpace = "http://www.sat.gob.mx/leyendasFiscales http://www.sat.gob.mx/sitio_internet/cfd/leyendasFiscales/leyendasFisc.xsd";
        string NodoDivisas = fnConstruirCadenaLeyendaFiscal(versionLeyenda, disposicionfiscal, norma, textoleyenda);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginalDivisa;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoDivisas;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnPFintegranteCoordinado(string version, string ClaveVehicular, string Placa, string RFCPF)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);



        string sCadenaOriginal = "|" + version + "|" + ClaveVehicular + "|" + Placa + "|" + RFCPF + "||";
        string tNameSpace = "http://www.sat.gob.mx/pfic http://www.sat.gob.mx/sitio_internet/cfd/pfic/pfic.xsd";
        string NodoDonativas = fnConstruirCadenapfic(version, ClaveVehicular, Placa, RFCPF);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoDonativas;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenapfic(string version, string ClaveVehicular, string Placa, string RFCPF)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<pfic:PFintegranteCoordinado xmlns:pfic=\"http://www.sat.gob.mx/pfic\" version=\"" + version + "\" ClaveVehicular=\"" + ClaveVehicular +
            "\" Placa=\"" + Placa + "\" RFCPF=\"" + RFCPF + "\"/>");

        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaTerceros(string version, string rfc, string impuestoRet, string importeRet, string impuestoTra, string tasaTra, string importeTra)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<terceros:PorCuentadeTerceros xmlns:terceros=\"http://www.sat.gob.mx/terceros\" version=\"" + version + "\" rfc=\"" +
           rfc + "\">");

        sb.Append("<terceros:Retenciones>");
        sb.Append("<terceros:Retencion impuesto=\"" + impuestoRet + "\" importe=\"" + importeRet + "\" >");
        sb.Append("</terceros:Retencion>");
        sb.Append("</terceros:Retenciones>");

        sb.Append("<terceros:Traslados>");
        sb.Append("<terceros:Traslado impuesto=\"" + impuestoTra + "\" tasa=\"" + tasaTra + "\"  importe=\"" + importeTra + "\" >");
        sb.Append("</terceros:Traslado>");
        sb.Append("</terceros:Traslados>");


        sb.Append("</terceros:PorCuentadeTerceros>");

        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaTuristaPasajero(string version, string fechadeTransito, string tipoTransito, string via, string TipoId, string NumeroId, string Nacionalidad, string EmpresaTransporte)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<tpe:TuristaPasajeroExtranjero xmlns:tpe=\"http://www.sat.gob.mx/TuristaPasajeroExtranjero\" version=\"" + version + "\" fechadeTransito=\"" + fechadeTransito + "\" tipoTransito=\"" + tipoTransito + "\">");
        sb.Append("<tpe:datosTransito Via=\"" + via + "\" Tipold=\"" + TipoId + "\" NumeroId=\"" + NumeroId + "\" Nacionalidad=\"" + Nacionalidad + "\" EmpresaTransporte=\"" + EmpresaTransporte + "\"/>");
        sb.Append("</tpe:TuristaPasajeroExtranjero>");
        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnTuristaPasajeroExtranjero(string version, DateTime fechadeTransito, string tipoTransito, string via, string TipoId, string NumeroId, string Nacionalidad, string EmpresaTransporte)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);

        string FechaAut = fechadeTransito.ToString("s");

        string sCadenaOriginal = "|" + version + "|" + FechaAut + "|" + tipoTransito + "|" + via + "|" + TipoId + "|" + NumeroId + "|" + Nacionalidad + "|" + EmpresaTransporte + "||";
        string tNameSpace = "http://www.sat.gob.mx/TuristaPasajeroExtranjero http://www.sat.gob.mx/sitio_internet/cfd/TuristaPasajeroExtranjero/TuristaPasajeroExtranjero.xsd";
        string NodoDonativas = fnConstruirCadenaTuristaPasajero(version, FechaAut, tipoTransito, via, TipoId, NumeroId, Nacionalidad, EmpresaTransporte);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoDonativas;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    // Contruye la cadena original a partir de un XML de CFDI
    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public string fnConstruirCadenaVehicular(string version, string ClaveVehicular)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<ventavehiculos:VentaVehiculos xmlns:ventavehiculos=\"http://www.sat.gob.mx/ventavehiculos\" version=\"" + version + "\" ClaveVehicular=\"" + ClaveVehicular + "\"/>");

        return sb.ToString();
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnVentaVehicular(string version, string ClaveVehicular)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);



        string sCadenaOriginal = "|" + version + "|" + ClaveVehicular + "||";
        string tNameSpace = "http://www.sat.gob.mx/ventavehiculos http://www.sat.gob.mx/sitio_internet/cfd/ventavehiculos/ventavehiculos.xsd";
        string NodoDonativas = fnConstruirCadenaVehicular(version, ClaveVehicular);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoDonativas;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    public DataTable fnTerceros(string version, string rfc, string impuestoRet, string importeRet, string impuestoTra, string tasaTra, string importeTra)
    {
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "cadenaoriginal";
        columna1.ColumnName = "cadenaoriginal";
        columna1.DefaultValue = null;
        table.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "tnamespace";
        columna2.ColumnName = "tnamespace";
        columna2.DefaultValue = null;
        table.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "nodo";
        columna3.ColumnName = "nodo";
        columna3.DefaultValue = null;
        table.Columns.Add(columna3);
        string sCadenaOriginal = "|" + version + "|" + rfc + "|" + impuestoRet + "|" + importeRet + "|" + impuestoTra + "|" + tasaTra + "|" + impuestoTra + "||";
        string tNameSpace = "http://www.sat.gob.mx/terceros http://www.sat.gob.mx/sitio_internet/cfd/terceros/terceros11.xsd";
        string NodoIEDU = fnConstruirCadenaTerceros(version, rfc, impuestoRet, importeRet, impuestoTra, tasaTra, importeTra);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoIEDU;
        table.Rows.Add(nuevo);

        return table;
    }

    //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
}