using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Complementos
/// </summary>
public class Complementos
{
    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaDivisas(string version, string operacion)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<divisas:Divisas xmlns:divisas=\"http://www.sat.gob.mx/divisas\" version=\"" + version + "\" tipoOperacion=\"" +
           operacion + "\"/>");

        return sb.ToString();
    }
    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaDonativas(string version, string fecha, string noAutorizacion, string leyenda)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<donat:Donatarias xmlns:donat=\"http://www.sat.gob.mx/donat\" version=\"" + version + "\" fechaAutorizacion=\"" +
           fecha + "\" noAutorizacion=\"" + noAutorizacion +
            "\" leyenda=\"" + leyenda + "\"/>");

        return sb.ToString();
    }

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaECB(string version, int numeroCuenta, string nombreCliente, string periodo, string fecha, string descripcion, double importe, string RFCenajenante)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<ecb:EstadoDeCuentaBancario xmlns:ecb=\"http://www.sat.gob.mx/ecb\" version=\"" + version +
            "\" numeroCuenta=\"" + numeroCuenta + "\" nombreCliente=\"" + nombreCliente + "\" periodo=\"" + periodo + "\">");
        sb.Append("<ecb:Movimientos>");
        sb.Append("<ecb:MovimientoECB fecha=\"" + fecha + "\" descripcion=\"" + descripcion + "\" importe=\"" + importe + "\"/>");
        sb.Append("<ecb:MovimientoECBFiscal fecha=\"" + fecha + "\" descripcion=\"" + descripcion + "\" RFCenajenante=\"" + RFCenajenante + "\" Importe=\"" + importe + "\" />");
        sb.Append("</ecb:Movimientos>");
        sb.Append("</ecb:EstadoDeCuentaBancario>");

        return sb.ToString();
    }

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
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

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaImpLocal(string version, double TotaldeRetenciones, double TotaldeTraslados, bool AddTraslados, DataTable dtImpuestosTras)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        if (AddTraslados == false)
        {
            sb.Append("<implocal:ImpuestosLocales version=\"" + version +
                "\" TotaldeRetenciones=\"" + TotaldeRetenciones + "\" TotaldeTraslados=\"" + TotaldeTraslados + "\"/>");
        }
        else
        {

            //decimal dtotalRet, dtotalTra;
            //dtotalRet = dtotalTra = 0;
            //foreach (DataRow rImpTot in dtImpuestosTras.Rows)
            //{
            //    if (rImpTot["efecto"].ToString() == "Traslado")
            //    {
            //        if (rImpTot["abreviacion"].ToString() != "IVA" && rImpTot["abreviacion"].ToString() != "IEPS")
            //            dtotalTra += Convert.ToDecimal(rImpTot["calculo"].ToString().Replace("$", ""));
            //    }
            //    if (rImpTot["efecto"].ToString() == "Retención")
            //    {
            //        if (rImpTot["abreviacion"].ToString() != "ISR" && rImpTot["abreviacion"].ToString() != "IVA Retenido")
            //            dtotalRet += Convert.ToDecimal(rImpTot["calculo"].ToString().Replace("$", ""));
            //    }
            //}

            //Se forma xml de complemento según datos de datatable impuestos
            sb.Append("<implocal:ImpuestosLocales xmlns:implocal=\"http://www.sat.gob.mx/implocal\" version=\"" + version +
                "\" TotaldeRetenciones=\"" + String.Format("{0:n2}", TotaldeRetenciones).Replace(",", "") + "\" TotaldeTraslados=\"" + String.Format("{0:n2}", TotaldeTraslados).Replace(",", "") + "\">");
            string sRetencionXML, sTrasladoXML;
            sRetencionXML = sTrasladoXML = string.Empty;
            foreach (DataRow rImp in dtImpuestosTras.Rows)
            {
                if (rImp["efecto"].ToString() == "Retención")
                {
                    if (rImp["abreviacion"].ToString() != "ISR" && rImp["abreviacion"].ToString() != "IVA Retenido")
                    {
                        sRetencionXML += "|<implocal:RetencionesLocales ImpLocRetenido=\"" + rImp["abreviacion"].ToString() + "\" TasadeRetencion=\"" + String.Format("{0:n2}", Convert.ToDecimal(rImp["tasa"].ToString().Replace("%", ""))).Replace(",", "") + "\" Importe=\"" + String.Format("{0:n2}", Convert.ToDecimal(rImp["calculo"].ToString().Replace("$", ""))).Replace(",", "") + "\" />";
                    }
                    //sb.Append("<implocal:RetencionesLocales ImpLocRetenido=\"" + rImp["abreviacion"].ToString() + "\" TasadeRetencion=\"" + String.Format("{0:n2}", Convert.ToDecimal(rImp["tasa"].ToString().Replace("%", ""))) + "\" Importe=\"" + String.Format("{0:n2}", Convert.ToDecimal(rImp["calculo"].ToString().Replace("$", ""))) + "\" />");
                }

                if (rImp["efecto"].ToString() == "Traslado")
                {
                    if (rImp["abreviacion"].ToString() != "IVA" && rImp["abreviacion"].ToString() != "IEPS")
                    {
                        sTrasladoXML += "|<implocal:TrasladosLocales ImpLocTrasladado=\"" + rImp["abreviacion"].ToString() + "\" TasadeTraslado=\"" + String.Format("{0:n2}", Convert.ToDecimal(rImp["tasa"].ToString().Replace("%", ""))).Replace(",", "") + "\" Importe=\"" + String.Format("{0:n2}", Convert.ToDecimal(rImp["calculo"].ToString().Replace("$", ""))).Replace(",", "") + "\" />";
                    }
                    //sb.Append("<implocal:TrasladosLocales ImpLocTrasladado=\"" + rImp["abreviacion"].ToString() + "\" TasadeTraslado=\"" + String.Format("{0:n2}", Convert.ToDecimal(rImp["tasa"].ToString().Replace("%", ""))) + "\" Importe=\"" + String.Format("{0:n2}", Convert.ToDecimal(rImp["calculo"].ToString().Replace("$", ""))) + "\" />");
                }
            }
            //Se desconcatena para formar el xml de impuestos locales
            if (sRetencionXML != string.Empty && sTrasladoXML == string.Empty)
            {
                char[] cCadR = { '|' };
                string[] sCadR;
                sCadR = sRetencionXML.Split(cCadR);
                foreach (string s in sCadR)
                {
                    if (s.ToString() != string.Empty)
                        sb.Append(s.ToString());
                }
            }
            else
            {
                if (sRetencionXML == string.Empty && sTrasladoXML != string.Empty)
                {
                    char[] cCadT = { '|' };
                    string[] sCadT;
                    sCadT = sTrasladoXML.Split(cCadT);
                    foreach (string s in sCadT)
                    {
                        if (s.ToString() != string.Empty)
                            sb.Append(s.ToString());
                    }
                }
                else
                {
                    if (sRetencionXML != string.Empty && sTrasladoXML != string.Empty)
                    {
                        char[] cCad = { '|' };
                        string[] sCadR;
                        sCadR = sRetencionXML.Split(cCad);
                        foreach (string s in sCadR)
                        {
                            if (s.ToString() != string.Empty)
                                sb.Append(s.ToString());
                        }

                        string[] sCadT;
                        sCadT = sTrasladoXML.Split(cCad);
                        foreach (string s in sCadT)
                        {
                            if (s.ToString() != string.Empty)
                                sb.Append(s.ToString());
                        }
                    }
                }
            }
            sb.Append("</implocal:ImpuestosLocales>");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaPSGECFD(string nombre, string rfc, string noCertificado, string fechaPublicacion, string noAutorizacion, string selloDelPSGECFD)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<psgecfd:PrestadoresDeServiciosDeCFD xmlns:psgecfd=\"http://www.sat.gob.mx/psgecfd\" nombre=\"" +
           nombre + "\" rfc=\"" + rfc + "\" noCertificado=\"" + noCertificado + "\" fechaAutorizacion=\"" + fechaPublicacion +
            "\" noAutorizacion=\"" + noAutorizacion + "\" selloDelPSGECFD=\"" + selloDelPSGECFD + "\"/>");

        return sb.ToString();
    }

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaIEDU(string version, string nombrealumno, string curp,
        string niveleducativo, string autRVOE, string rfcPago)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();
        sb.Append("<iedu:instEducativas xmlns:iedu=\"http://www.sat.gob.mx/iedu\" version=\"" + version + "\" nombreAlumno=\"" +
           nombrealumno + "\" CURP=\"" + curp + "\" nivelEducativo=\"" + niveleducativo + "\" autRVOE=\"" + autRVOE + "\" rfcPago=\"" + rfcPago + "\"/>");
        return sb.ToString();
    }

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
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

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaLeyendaFiscal(string versionLeyenda, string disposicionfiscal, string norma, string textoleyenda)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<leyendasFisc:LeyendasFiscales xmlns:leyendasFisc=\"http://www.sat.gob.mx/leyendasFiscales\" version=\"" + versionLeyenda + "\">");
        sb.Append("<leyendasFisc:Leyenda disposicionFiscal=\"" + disposicionfiscal + "\" norma=\"" + norma + "\" textoLeyenda=\"" + textoleyenda + "\"/>");
        sb.Append("</leyendasFisc:LeyendasFiscales>");
        return sb.ToString();
    }



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

    public DataTable fnImpuestosLocales(string version, double TotaldeRetenciones, double TotaldeTraslados, bool bAddTraslados, DataTable dtImpuestosTras = null)
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

        string sCadenaOriginal;
        if (bAddTraslados == false) //Si se agrega complemente ISH, cargo no gravables
            sCadenaOriginal = "|" + version + "|" + TotaldeRetenciones + "|" + TotaldeTraslados + "||";
        else
        {
            string sTraslados = string.Empty;
            string sRetenido = string.Empty;
            //Se obtiene el total de impuesto
            foreach (DataRow rImpTot in dtImpuestosTras.Rows)
            {
                if (rImpTot["efecto"].ToString() == "Traslado")
                {
                    //IVA, IEPS
                    if (rImpTot["abreviacion"].ToString() != "IVA" && rImpTot["abreviacion"].ToString() != "IEPS")
                        TotaldeTraslados += Convert.ToDouble(rImpTot["calculo"].ToString().Replace("$", "").Replace(",", ""));
                }
                if (rImpTot["efecto"].ToString() == "Retención")
                {
                    //IVARet, ISR
                    if (rImpTot["abreviacion"].ToString() != "ISR" && rImpTot["abreviacion"].ToString() != "IVA Retenido")
                        TotaldeRetenciones += Convert.ToDouble(rImpTot["calculo"].ToString().Replace("$", "").Replace(",", ""));
                }
            }
            sCadenaOriginal = "|" + version + "|" + String.Format("{0:n2}", Convert.ToDecimal(TotaldeRetenciones)).Replace(",", "") + "|" + String.Format("{0:n2}", Convert.ToDecimal(TotaldeTraslados)).Replace(",", "") + "|";
            //Se concatena el impuesto a la cadena original
            foreach (DataRow rImp in dtImpuestosTras.Rows)
            {
                if (rImp["efecto"].ToString() == "Retención")
                {
                    if (rImp["abreviacion"].ToString() != "ISR" && rImp["abreviacion"].ToString() != "IVA Retenido")
                        sRetenido += rImp["abreviacion"].ToString() + "|" + String.Format("{0:n2}", Convert.ToDecimal(rImp["tasa"].ToString().Replace("%", ""))).Replace(",", "") + "|" + String.Format("{0:n2}", Convert.ToDecimal(rImp["calculo"].ToString().Replace("$", ""))).Replace(",", "") + "|";
                }

                if (rImp["efecto"].ToString() == "Traslado")
                {
                    if (rImp["abreviacion"].ToString() != "IVA" && rImp["abreviacion"].ToString() != "IEPS")
                        sTraslados += rImp["abreviacion"].ToString() + "|" + String.Format("{0:n2}", Convert.ToDecimal(rImp["tasa"].ToString().Replace("%", ""))).Replace(",", "") + "|" + String.Format("{0:n2}", Convert.ToDecimal(rImp["calculo"].ToString().Replace("$", ""))).Replace(",", "") + "|";
                }
            }

            //Se concatena traslados y retenciones 
            if (sRetenido != string.Empty && sTraslados == string.Empty)
                sCadenaOriginal += sRetenido + "|";
            else
            {
                if (sRetenido == string.Empty && sTraslados != string.Empty)
                    sCadenaOriginal += sTraslados + "|";
                else
                {
                    if (sRetenido != string.Empty && sTraslados != string.Empty)
                        sCadenaOriginal += sRetenido + sTraslados + "|";
                }
            }
        }
        string tNameSpace = "implocal" + "|" + "http://www.sat.gob.mx/implocal" + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/implocal/implocal.xsd"; //"http://www.sat.gob.mx/implocal http://www.sat.gob.mx/sitio_internet/cfd/implocal/implocal.xsd";
        string NodoImpuestosLocales = fnConstruirCadenaImpLocal(version, TotaldeRetenciones, TotaldeTraslados, bAddTraslados, dtImpuestosTras);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoImpuestosLocales;
        table.Rows.Add(nuevo);

        return table;
    }

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
        string sCadenaOriginal = "|" + version + "|" + noAutorizacion + "|" + FechaAut + "|" + leyenda + "||";
        string tNameSpace = "donat" + "|" + "http://www.sat.gob.mx/donat" + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/donat/donat11.xsd";
        //string tNameSpace = " ";
        leyenda = leyenda.Replace("\r\n", " ");
        leyenda = leyenda.Trim();
        string NodoDonativas = fnConstruirCadenaDonativas(version, FechaAut, noAutorizacion, leyenda);
        //Codigo agregado por David Elizalde 21/06/2014
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(NodoDonativas);
        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("donat", "http://www.sat.gob.mx/donat");

        sCadenaOriginal = fnConstruirCadenaTimbrado(xmlDoc.CreateNavigator(), "donativas_util.xslt") + "||";

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoDonativas;
        table.Rows.Add(nuevo);

        return table;
    }



    public static string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
    {
        string sCadenaOriginal = string.Empty;

        try
        {
            MemoryStream ms = new MemoryStream();
            XslCompiledTransform trans = new XslCompiledTransform();
            trans.Load(XmlReader.Create(new StringReader(clsComunAHM.ObtenerParamentro(psNombreArchivoXSLT))));
            XsltArgumentList args = new XsltArgumentList();
            trans.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            sCadenaOriginal = sr.ReadToEnd();
        }
        catch (Exception ex)
        {
            clsErrorLogAHM.fnNuevaEntrada(ex, clsErrorLogAHM.TipoErroresLog.Datos);
        }

        return sCadenaOriginal;
    }


    public DataTable fnECB(string version, int numeroCuenta, string nombreCliente, string periodo, DateTime fecha, string descripcion, double importe, string RFCenajenante)
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

        string sCadenaOriginal = "|" + version + "|" + numeroCuenta + "|" + nombreCliente + "|" + FechaAut + "|" + RFCenajenante + "|" + importe + "||";
        string tNameSpace = "http://www.sat.gob.mx/ecb http://www.sat.gob.mx/sitio_internet/cfd/ecb/ecb.xsd";
        string NodoECB = fnConstruirCadenaECB(version, numeroCuenta, nombreCliente, periodo, FechaAut, descripcion, importe, RFCenajenante);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoECB;
        table.Rows.Add(nuevo);

        return table;
    }

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

        disposicionfiscal = disposicionfiscal.Trim();
        disposicionfiscal = Regex.Replace(disposicionfiscal, " {2,}", " ");

        norma = norma.Trim();
        norma = Regex.Replace(norma, " {2,}", " ");

        textoleyenda = textoleyenda.Trim();
        textoleyenda = Regex.Replace(textoleyenda, " {2,}", " ");


        string sCadenaOriginalDivisa = "|" + versionLeyenda + "|" + disposicionfiscal + "|" + norma + "|" + textoleyenda + "||";

        string tNameSpace = "leyendasFisc" + "|" + "http://www.sat.gob.mx/leyendasFiscales" + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/leyendasFiscales/leyendasFisc.xsd";
        string NodoDivisas = fnConstruirCadenaLeyendaFiscal(versionLeyenda, disposicionfiscal, norma, textoleyenda);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(NodoDivisas);

        sCadenaOriginalDivisa = fnConstruirCadenaTimbrado(xmlDoc.CreateNavigator(), "leyendasFiscales_util.xslt") + "||";

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginalDivisa;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoDivisas;
        table.Rows.Add(nuevo);

        return table;
    }


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


    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenapfic(string version, string ClaveVehicular, string Placa, string RFCPF)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<pfic:PFintegranteCoordinado xmlns:pfic=\"http://www.sat.gob.mx/pfic\" version=\"" + version + "\" ClaveVehicular=\"" + ClaveVehicular +
            "\" Placa=\"" + Placa + "\" RFCPF=\"" + RFCPF + "\"/>");

        return sb.ToString();
    }


    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaTerceros(string version, string rfc, CheckBox cbIVARet, string importeIVARet, CheckBox cbISRRet, string importeISRRet,
        CheckBox cbIVATra, string tasaIVATra, string importeIVATra, CheckBox cbIEPSTra, string tasaIEPSTra, string importeIEPSTra,
         string calle, string noExterior, string noInterior, string colonia, string localidad,
        string referencia, string municipio, string estado, string pais, string codigoPostal,
        string numero, string fecha, string aduana, string numeroPredial, string nombre, string cantidad, string descripcion)
    {

        StringBuilder sb = new StringBuilder();
        //@namespace.PorCuentadeTerceros PorCuentaTerceros = new @namespace.PorCuentadeTerceros();

        //PorCuentaTerceros = fnTerceros(version, rfc, cbIVARet, importeIVARet, cbISRRet, importeISRRet,
        //cbIVATra, tasaIVATra, importeIVATra, cbIEPSTra, tasaIEPSTra, importeIEPSTra,
        // calle, noExterior, noInterior, colonia, localidad,
        //referencia, municipio, estado, pais, codigoPostal,
        //numero, fecha, aduana, numeroPredial);

        //XmlDocument xXml = new XmlDocument();
        //XmlSerializerNamespaces sns = new XmlSerializerNamespaces();

        //sns.Add("terceros", "http://www.sat.gob.mx/terceros");

        //XmlSerializer serializador = new XmlSerializer(typeof(@namespace.PorCuentadeTerceros));

        //string withEncoding;
        //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        //{
        //    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(ms))
        //    {
        //        serializador.Serialize(writer, PorCuentaTerceros, sns);
        //        using (System.IO.StreamReader reader = new System.IO.StreamReader(ms))
        //        {
        //            ms.Position = 0;
        //            withEncoding = reader.ReadToEnd();
        //        }
        //    }
        //}

        //string withOutEncoding = withEncoding.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");

        //xXml.LoadXml(withOutEncoding);

        //node.ParentNode.RemoveChild(node);

        sb.Append("<terceros:PorCuentadeTerceros xmlns:terceros=\"http://www.sat.gob.mx/terceros\" version=\"" + version + "\" rfc=\"" +
           rfc + "\">");
        //sb.Append("<terceros:PorCuentadeTerceros version=\"" + version + "\" rfc=\"" +
        //        rfc + "\">");

        sb.Append("<terceros:InformacionFiscalTercero calle=\"" + calle + "\" noExterior=\"" + noExterior + "\" noInterior=\"" + noInterior +
            "\" colonia=\"" + colonia + "\" localidad=\"" + localidad + "\"  referencia=\"" + referencia + "\" municipio=\"" + municipio +
            "\" estado=\"" + estado + "\" pais=\"" + pais + "\" codigoPostal=\"" + codigoPostal + "\" >");
        sb.Append("</terceros:InformacionFiscalTercero>");

        sb.Append("<terceros:InformacionAduanera numero=\"" + numero + "\" fecha=\"" + fecha + "\" aduana=\"" + aduana + "\" >");
        sb.Append("</terceros:InformacionAduanera>");

        if (numeroPredial != string.Empty)
        {
            sb.Append("<terceros:CuentaPredial numero=\"" + numeroPredial + "\">");
            sb.Append("</terceros:CuentaPredial>");
        }
        sb.Append("<terceros:Impuestos>");

        sb.Append("<terceros:Retenciones>");
        if (cbIVARet.Checked == true)
        {
            sb.Append("<terceros:Retencion impuesto=\"" + "IVA" + "\" importe=\"" + importeIVARet + "\" >");
            sb.Append("</terceros:Retencion>");
        }

        if (cbISRRet.Checked == true)
        {
            sb.Append("<terceros:Retencion impuesto=\"" + "ISR" + "\" importe=\"" + importeISRRet + "\" >");
            sb.Append("</terceros:Retencion>");
        }
        sb.Append("</terceros:Retenciones>");

        sb.Append("<terceros:Traslados>");
        if (cbIVATra.Checked == true)
        {
            sb.Append("<terceros:Traslado impuesto=\"" + "IVA" + "\" tasa=\"" + tasaIVATra + "\"  importe=\"" + importeIVATra + "\" >");
            sb.Append("</terceros:Traslado>");
        }
        if (cbIEPSTra.Checked == true)
        {
            sb.Append("<terceros:Traslado impuesto=\"" + "IEPS" + "\" tasa=\"" + tasaIEPSTra + "\"  importe=\"" + importeIEPSTra + "\" >");
            sb.Append("</terceros:Traslado>");
        }
        sb.Append("</terceros:Traslados>");

        sb.Append("</terceros:Impuestos>");

        //sb.Append("<terceros:Parte cantidad=\"" + cantidad + "\" descripcion=\"" + descripcion + "\" >");
        //sb.Append("</terceros:Parte>");

        sb.Append("</terceros:PorCuentadeTerceros>");

        return sb.ToString();
    }

    //public @namespace.PorCuentadeTerceros fnTerceros(string version, string rfc, CheckBox cbIVARet, string importeIVARet, CheckBox cbISRRet, string importeISRRet,
    //    CheckBox cbIVATra, string tasaIVATra, string importeIVATra, CheckBox cbIEPSTra, string tasaIEPSTra, string importeIEPSTra,
    //     string calle, string noExterior, string noInterior, string colonia, string localidad,
    //    string referencia, string municipio, string estado, string pais, string codigoPostal,
    //    string numero, string fecha, string aduana, string numeroPredial)
    //{

    //    List<@namespace.PorCuentadeTerceros> ListCuenta = new List<@namespace.PorCuentadeTerceros>();

    //    @namespace.PorCuentadeTerceros PorCuentaTerceros = new @namespace.PorCuentadeTerceros();

    //    PorCuentaTerceros.version = version;
    //    PorCuentaTerceros.rfc = rfc;

    //    @namespace.PorCuentadeTercerosInformacionFiscalTercero InformacionFiscalTerceros = new @namespace.PorCuentadeTercerosInformacionFiscalTercero();
    //    InformacionFiscalTerceros.calle = calle;
    //    InformacionFiscalTerceros.codigoPostal = codigoPostal;
    //    InformacionFiscalTerceros.colonia = colonia;
    //    InformacionFiscalTerceros.estado = estado;
    //    InformacionFiscalTerceros.localidad = localidad;
    //    InformacionFiscalTerceros.municipio = municipio;
    //    InformacionFiscalTerceros.noExterior = noExterior;
    //    InformacionFiscalTerceros.noInterior = noInterior;
    //    InformacionFiscalTerceros.pais = pais;
    //    InformacionFiscalTerceros.referencia = referencia;

    //    List<@namespace.PorCuentadeTercerosInformacionAduanera> ListInfoAduanera = new List<@namespace.PorCuentadeTercerosInformacionAduanera>();
    //    @namespace.PorCuentadeTercerosInformacionAduanera InformacionAduanera = new @namespace.PorCuentadeTercerosInformacionAduanera();
    //    InformacionAduanera.aduana = aduana;
    //    InformacionAduanera.fecha = Convert.ToDateTime(fecha);
    //    InformacionAduanera.numero = numero;

    //    ListInfoAduanera.Add(InformacionAduanera);

    //    List<@namespace.PorCuentadeTercerosCuentaPredial> ListCuentaPredial = new List<@namespace.PorCuentadeTercerosCuentaPredial>();
    //    @namespace.PorCuentadeTercerosCuentaPredial CuentaPredial = new @namespace.PorCuentadeTercerosCuentaPredial();
    //    if (numeroPredial != string.Empty)
    //    {
    //        //@namespace.PorCuentadeTercerosCuentaPredial CuentaPredial = new @namespace.PorCuentadeTercerosCuentaPredial();
    //        CuentaPredial.numero = numeroPredial;

    //        ListCuentaPredial.Add(CuentaPredial);
    //    }

    //    //Impuestos
    //    @namespace.PorCuentadeTercerosImpuestos TercerosImpuestos = new @namespace.PorCuentadeTercerosImpuestos();
    //    @namespace.PorCuentadeTercerosImpuestosRetencion ImpuestoRetencion = new @namespace.PorCuentadeTercerosImpuestosRetencion();
    //    @namespace.PorCuentadeTercerosImpuestosTraslado ImpuestoTraslado = new @namespace.PorCuentadeTercerosImpuestosTraslado();

    //    List<@namespace.PorCuentadeTercerosImpuestosRetencion> listTercerosImpuestosRetencion = new List<@namespace.PorCuentadeTercerosImpuestosRetencion>();
    //    List<@namespace.PorCuentadeTercerosImpuestosTraslado> listTercerosImpuestosTraslado = new List<@namespace.PorCuentadeTercerosImpuestosTraslado>();

    //    //Retencion
    //    if (cbIVARet.Checked == true)
    //    {
    //        ImpuestoRetencion.impuesto = @namespace.PorCuentadeTercerosImpuestosRetencionImpuesto.IVA;
    //        ImpuestoRetencion.importe = Convert.ToDecimal(importeIVARet);
    //    }
    //    if (cbISRRet.Checked == true)
    //    {
    //        ImpuestoRetencion.impuesto = @namespace.PorCuentadeTercerosImpuestosRetencionImpuesto.ISR;
    //        ImpuestoRetencion.importe = Convert.ToDecimal(importeISRRet);
    //    }

    //    listTercerosImpuestosRetencion.Add(ImpuestoRetencion);

    //    //Traslado
    //    if (cbIVATra.Checked == true)
    //    {
    //        ImpuestoTraslado.impuesto = @namespace.PorCuentadeTercerosImpuestosTrasladoImpuesto.IVA;
    //        ImpuestoTraslado.importe = Convert.ToDecimal(importeIVATra);
    //    }
    //    if (cbIEPSTra.Checked == true)
    //    {
    //        ImpuestoTraslado.impuesto = @namespace.PorCuentadeTercerosImpuestosTrasladoImpuesto.IEPS;
    //        ImpuestoTraslado.importe = Convert.ToDecimal(importeIEPSTra);
    //    }

    //    listTercerosImpuestosTraslado.Add(ImpuestoTraslado);

    //    TercerosImpuestos.Retenciones = listTercerosImpuestosRetencion.ToArray();
    //    TercerosImpuestos.Traslados = listTercerosImpuestosTraslado.ToArray();

    //    PorCuentaTerceros.InformacionFiscalTercero = InformacionFiscalTerceros;

    //    //if (ListCuentaPredial.Count > 0)
    //    //{
    //    //    PorCuentaTerceros.Items = ListCuentaPredial.ToArray();

    //    //    ListTerceros.Add(PorCuentaTerceros);
    //    //}

    //    PorCuentaTerceros.Items = new object[2];

    //    PorCuentaTerceros.Items[0] = InformacionAduanera;
    //    PorCuentaTerceros.Items[1] = CuentaPredial;

    //    PorCuentaTerceros.Impuestos = TercerosImpuestos;

    //    return PorCuentaTerceros;

    //}

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaTuristaPasajero(string version, string fechadeTransito, string tipoTransito, string via, string TipoId, string NumeroId, string Nacionalidad, string EmpresaTransporte)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<tpe:TuristaPasajeroExtranjero xmlns:tpe=\"http://www.sat.gob.mx/TuristaPasajeroExtranjero\" version=\"" + version + "\" fechadeTransito=\"" + fechadeTransito + "\" tipoTransito=\"" + tipoTransito + "\">");
        sb.Append("<tpe:datosTransito Via=\"" + via + "\" Tipold=\"" + TipoId + "\" NumeroId=\"" + NumeroId + "\" Nacionalidad=\"" + Nacionalidad + "\" EmpresaTransporte=\"" + EmpresaTransporte + "\"/>");
        sb.Append("</tpe:TuristaPasajeroExtranjero>");
        return sb.ToString();
    }


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




    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaVehicular(string version, string ClaveVehicular)
    {
        string sCadenaOriginal = string.Empty;
        StringBuilder sb = new StringBuilder();

        sb.Append("<ventavehiculos:VentaVehiculos xmlns:ventavehiculos=\"http://www.sat.gob.mx/ventavehiculos\" version=\"" + version + "\" ClaveVehicular=\"" + ClaveVehicular + "\"/>");

        return sb.ToString();
    }

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


    public DataTable fnTerceros(string version, string rfc, CheckBox cbImpIvaRet, string importeIVARet, CheckBox cbImpISRRet, string importeISRRet,
        CheckBox cbImpIVATra, string tasaIVATra, string importeIVATra, CheckBox cbImpIEPSTra, string tasaIEPSTra, string importeIEPSTra,
        string calle, string noExterior, string noInterior, string colonia, string localidad,
        string referencia, string municipio, string estado, string pais, string codigoPostal,
        string numero, DateTime fecha, string aduana, string numeroPredial, string nombre, string cantidad, string descripcion)
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
        string FechaAut = fecha.ToString("yyyy-MM-dd");

        //string sCadenaOriginal = "|" + version + "|" + rfc + "|" + nombre + "|" + calle + "|"+noExterior+"|"+ noInterior + "|" +colonia + "|"+ localidad + "|" +
        //    referencia + "|" + municipio + "|" + estado + "|" + pais + "|" + codigoPostal + "|" + numero + FechaAut + "|"+ aduana + "|" + numeroPredial +"|" +
        //    impuestoRet + "|" + importeRet + "|" + impuestoTra + "|" + tasaTra + "|" + importeTra + "||";

        string sCadenaRet, sCadenaTra;
        sCadenaRet = sCadenaTra = string.Empty;
        //Impuesto Retencion
        if (cbImpIvaRet.Checked == true)
            sCadenaRet += "IVA|" + importeIVARet + "|";
        if (cbImpISRRet.Checked == true)
            sCadenaRet += "ISR|" + importeISRRet + "|";
        //Impuesto Traslado
        if (cbImpIVATra.Checked == true)
            sCadenaTra += "IVA|" + tasaIVATra + "|" + importeIVATra + "|";
        if (cbImpIEPSTra.Checked == true)
            sCadenaTra += "IEPS|" + tasaIEPSTra + "|" + importeIEPSTra + "|";

        string sCadenaOriginal = "|" + version + "|" + rfc + "|" + calle + "|" + noExterior + "|";

        if (noInterior != string.Empty)
            sCadenaOriginal += noInterior + "|";

        sCadenaOriginal += colonia + "|" + localidad + "|" + referencia + "|" + municipio + "|" + estado + "|" + pais + "|" + codigoPostal + "|" + numero + "|" + FechaAut + "|" + aduana + "|";

        if (numeroPredial != string.Empty)
        {
            sCadenaOriginal += numeroPredial + "|";
        }

        sCadenaOriginal += sCadenaRet;

        sCadenaOriginal += sCadenaTra + "|";

        string tNameSpace = "terceros" + "|" + "http://www.sat.gob.mx/terceros" + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/terceros/terceros11.xsd";

        string NodoIEDU = fnConstruirCadenaTerceros(version, rfc, cbImpIvaRet, importeIVARet, cbImpISRRet, importeISRRet, cbImpIVATra, tasaIVATra, importeIVATra, cbImpIEPSTra, tasaIEPSTra, importeIEPSTra,
            calle, noExterior, noInterior, colonia, localidad,
            referencia, municipio, estado, pais, codigoPostal, numero, FechaAut, aduana, numeroPredial, nombre, cantidad, descripcion);

        DataRow nuevo = table.NewRow();
        nuevo["cadenaoriginal"] = sCadenaOriginal;
        nuevo["tnamespace"] = tNameSpace;
        nuevo["nodo"] = NodoIEDU;
        table.Rows.Add(nuevo);

        return table;
    }

    // <summary>
    /// Construye XML complemento concepto terceros
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    /// 
    public string fnComplTerceros(string version, string rfc, string nombre, string impuestoIVARet, string importeIVARet, string impuestoISRRet, string importeISRRet,
        string impuestoIVATra, string tasaIVATra, string importeIVATra, string impuestoIEPSTra, string tasaIEPSTra, string importeIEPSTra,
         string calle, string noExterior, string noInterior, string colonia, string localidad,
        string referencia, string municipio, string estado, string pais, string codigoPostal,
        string numero, DateTime fecha, string aduana, string numeroPredial, string cantidad, string descripcion)
    {

        StringBuilder sb = new StringBuilder();
        #region comentado
        //@namespace.PorCuentadeTerceros PorCuentaTerceros = new @namespace.PorCuentadeTerceros();

        //PorCuentaTerceros = fnTerceros(version, rfc, cbIVARet, importeIVARet, cbISRRet, importeISRRet,
        //cbIVATra, tasaIVATra, importeIVATra, cbIEPSTra, tasaIEPSTra, importeIEPSTra,
        // calle, noExterior, noInterior, colonia, localidad,
        //referencia, municipio, estado, pais, codigoPostal,
        //numero, fecha, aduana, numeroPredial);

        //XmlDocument xXml = new XmlDocument();
        //XmlSerializerNamespaces sns = new XmlSerializerNamespaces();

        //sns.Add("terceros", "http://www.sat.gob.mx/terceros");

        //XmlSerializer serializador = new XmlSerializer(typeof(@namespace.PorCuentadeTerceros));

        //string withEncoding;
        //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        //{
        //    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(ms))
        //    {
        //        serializador.Serialize(writer, PorCuentaTerceros, sns);
        //        using (System.IO.StreamReader reader = new System.IO.StreamReader(ms))
        //        {
        //            ms.Position = 0;
        //            withEncoding = reader.ReadToEnd();
        //        }
        //    }
        //}

        //string withOutEncoding = withEncoding.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");

        //xXml.LoadXml(withOutEncoding);

        //node.ParentNode.RemoveChild(node);
        #endregion

        sb.Append("<terceros:PorCuentadeTerceros xmlns:terceros=\"http://www.sat.gob.mx/terceros\" version=\"" + version + "\" rfc=\"" +
           rfc + "\" ");
        if (!String.IsNullOrEmpty(nombre))
            sb.Append("nombre=\"" + nombre + "\"");
        sb.Append(">");
        //sb.Append("<terceros:PorCuentadeTerceros version=\"" + version + "\" rfc=\"" +
        //        rfc + "\">");

        ///// Información fiscal //////
        if (!String.IsNullOrEmpty(calle))
        {
            sb.Append("<terceros:InformacionFiscalTercero calle=\"" + calle + "\"");
            if (!String.IsNullOrEmpty(noExterior))
                sb.Append(" noExterior=\"" + noExterior + "\"");
            if (!String.IsNullOrEmpty(noInterior))
                sb.Append(" noInterior=\"" + noInterior + "\"");
            if (!String.IsNullOrEmpty(colonia))
                sb.Append(" colonia=\"" + colonia + "\"");
            if (!String.IsNullOrEmpty(localidad))
                sb.Append(" localidad=\"" + localidad + "\"");
            if (!String.IsNullOrEmpty(referencia))
                sb.Append(" referencia=\"" + referencia + "\"");
            sb.Append(" municipio=\"" + municipio + "\" estado=\"" + estado + "\" pais=\"" + pais + "\" codigoPostal=\"" + codigoPostal + "\">");
            sb.Append("</terceros:InformacionFiscalTercero>");
        }

        ///// Información aduanera /////
        if (!String.IsNullOrEmpty(numero))
        {
            string sFechaAduana = fecha.ToString("yyyy-MM-dd");
            sb.Append("<terceros:InformacionAduanera numero=\"" + numero + "\" fecha=\"" + sFechaAduana + "\"");
            if (!String.IsNullOrEmpty(aduana))   //Aduana opcional
                sb.Append(" aduana=\"" + aduana + "\"");
            sb.Append(">");
            sb.Append("</terceros:InformacionAduanera>");
        }

        if (!String.IsNullOrEmpty(numeroPredial))
        {
            sb.Append("<terceros:CuentaPredial numero=\"" + numeroPredial + "\">");
            sb.Append("</terceros:CuentaPredial>");
        }

        #region impuestos
        ///////  Impuestos   /////
        //sb.Append("<terceros:Impuestos>");

        /////// Impuestos retenidos  /////
        //sb.Append("<terceros:Retenciones>");

        ////if (cbIVARet.Checked == true)
        //if(!String.IsNullOrEmpty(impuestoIVARet))
        //{
        //    sb.Append("<terceros:Retencion impuesto=\"" + "IVA" + "\" importe=\"" + importeIVARet + "\" >");
        //    sb.Append("</terceros:Retencion>");
        //}

        ////if (cbISRRet.Checked == true)
        //if(!String.IsNullOrEmpty(impuestoISRRet))
        //{
        //    sb.Append("<terceros:Retencion impuesto=\"" + "ISR" + "\" importe=\"" + importeISRRet + "\" >");
        //    sb.Append("</terceros:Retencion>");
        //}
        //sb.Append("</terceros:Retenciones>");

        /////// Impuestos trasladados  /////
        //sb.Append("<terceros:Traslados>");

        ////if (cbIVATra.Checked == true)
        //if(!String.IsNullOrEmpty(impuestoIVATra))
        //{
        //    sb.Append("<terceros:Traslado impuesto=\"" + "IVA" + "\" tasa=\"" + tasaIVATra + "\"  importe=\"" + importeIVATra + "\" >");
        //    sb.Append("</terceros:Traslado>");
        //}

        ////if (cbIEPSTra.Checked == true)
        //if(!String.IsNullOrEmpty(impuestoIEPSTra))
        //{
        //    sb.Append("<terceros:Traslado impuesto=\"" + "IEPS" + "\" tasa=\"" + tasaIEPSTra + "\"  importe=\"" + importeIEPSTra + "\" >");
        //    sb.Append("</terceros:Traslado>");
        //}

        //sb.Append("</terceros:Traslados>");

        //sb.Append("</terceros:Impuestos>");
        #endregion

        /////  Impuestos   /////
        if (!String.IsNullOrEmpty(impuestoIVARet) || !String.IsNullOrEmpty(impuestoISRRet) || !String.IsNullOrEmpty(impuestoIVATra) || !String.IsNullOrEmpty(impuestoIEPSTra))
        {

            sb.Append("<terceros:Impuestos>");

            ///// Impuestos retenidos  /////
            if (!String.IsNullOrEmpty(impuestoIVARet) || !String.IsNullOrEmpty(impuestoISRRet))
            {
                sb.Append("<terceros:Retenciones>");

                //if (cbIVARet.Checked == true)
                if (!String.IsNullOrEmpty(impuestoIVARet))
                {
                    sb.Append("<terceros:Retencion impuesto=\"IVA\" importe=\"" + importeIVARet + "\" >");
                    sb.Append("</terceros:Retencion>");
                }

                //if (cbISRRet.Checked == true)
                if (!String.IsNullOrEmpty(impuestoISRRet))
                {
                    sb.Append("<terceros:Retencion impuesto=\"ISR\" importe=\"" + importeISRRet + "\" >");
                    sb.Append("</terceros:Retencion>");
                }
                sb.Append("</terceros:Retenciones>");
            }

            ///// Impuestos trasladados  /////
            if (!String.IsNullOrEmpty(impuestoIVATra) || !String.IsNullOrEmpty(impuestoIEPSTra))
            {
                sb.Append("<terceros:Traslados>");

                //if (cbIVATra.Checked == true)
                if (!String.IsNullOrEmpty(impuestoIVATra))
                {
                    sb.Append("<terceros:Traslado impuesto=\"IVA\" tasa=\"" + tasaIVATra + "\"  importe=\"" + importeIVATra + "\" >");
                    sb.Append("</terceros:Traslado>");
                }

                //if (cbIEPSTra.Checked == true)
                if (!String.IsNullOrEmpty(impuestoIEPSTra))
                {
                    sb.Append("<terceros:Traslado impuesto=\"IEPS\" tasa=\"" + tasaIEPSTra + "\"  importe=\"" + importeIEPSTra + "\" >");
                    sb.Append("</terceros:Traslado>");
                }

                sb.Append("</terceros:Traslados>");
            }

            sb.Append("</terceros:Impuestos>");
        }
        else
        {
            sb.Append("<terceros:Impuestos />");
        }

        //sb.Append("<terceros:Parte cantidad=\"" + cantidad + "\" descripcion=\"" + descripcion + "\" >");
        //sb.Append("</terceros:Parte>");

        sb.Append("</terceros:PorCuentadeTerceros>");

        return sb.ToString();
    }



}

