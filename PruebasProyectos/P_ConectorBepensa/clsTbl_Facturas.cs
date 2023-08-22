using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace P_ConectorBepensa
{
    class clsTbl_Facturas
    {
        public String  _sMoneda;
        public String  _sFolio;
        public String  _sSerie;
        public String  _sTipoDeComprobante;
        public double  _sTotal;
        public double  _sDescuento;
        public double  _sSubtotal;
        public String  _sNombreEmisor;
        public String  _sRFCEmisor;
        public String  _sNombreReceptor;
        public String  _sRFCReceptor;
        public double  _sTotalImpuestosTrasladados;
        public double  _sImporteliVA;
        public double  _sTasaIVA;
        public String  _sSatUID;
        public DateTime _sFechatimbrado;
        public String _sXmlCompleto;


        public String sMoneda { set { _sMoneda = value; } get { return _sMoneda; } }
        public String sFolio { set { _sFolio = value; } get { return _sFolio; } }
        public String sSerie { set { _sSerie = value; } get { return _sSerie; } }
        public String sTipoDeComprobante { set { _sTipoDeComprobante = value; } get { return _sTipoDeComprobante; } }
        public double sTotal { set { _sTotal = value; } get { return _sTotal; } }
        public double sDescuento { set { _sDescuento = value; } get { return _sDescuento; } }
        public double sSubtotal { set { _sSubtotal = value; } get { return _sSubtotal; } }
        public String sNombreEmisor { set { _sNombreEmisor = value; } get { return _sNombreEmisor; } }
        public String sRFCEmisor { set { _sRFCEmisor = value; } get { return _sRFCEmisor; } }
        public String sNombreReceptor { set { _sNombreReceptor = value; } get { return _sNombreReceptor; } }
        public String sRFCReceptor { set { _sRFCReceptor = value; } get { return _sRFCReceptor; } }
        public double sTotalImpuestosTrasladados { set { _sTotalImpuestosTrasladados = value; } get { return _sTotalImpuestosTrasladados; } }
        public double sImporteliVA { set { _sImporteliVA = value; } get { return _sImporteliVA; } }
        public double sTasaIVA { set { _sTasaIVA = value; } get { return _sTasaIVA; } }
        public String sSatUID { set { _sSatUID = value; } get { return _sSatUID; } }
        public DateTime sFechatimbrado { set { _sFechatimbrado = value; } get { return _sFechatimbrado; } }
        public String sXmlCompleto { set { _sXmlCompleto = value; } get { return _sXmlCompleto; } }
        

        private XmlDocument xdDoc;
        public XmlDocument xdDocument { get { return xdDoc; } }

        public clsTbl_Facturas(XmlDocument pxdDocumento) 
        {

            xdDoc = pxdDocumento;

            fnProc();
                    
        }

        private void fnProc() 
        {
            XmlNamespaceManager xnms = new XmlNamespaceManager(xdDoc.NameTable);
            xnms.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            xnms.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            

            //try
            //{ sMoneda = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", xnms).ToString(); }
            //catch { sMoneda = string.Empty; }

            //try
            //{ sFolio = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", xnms).ToString(); }
            //catch { sFolio = string.Empty; }

            //try
            //{ sSerie = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", xnms).ToString(); }
            //catch { sSerie = string.Empty; }

            //try
            //{ sTipoDeComprobante = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@tipoDeComprobante", xnms).ToString(); }
            //catch { sTipoDeComprobante = string.Empty; }

            //try
            //{ sDescuento = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@descuento", xnms).ToString()); }
            //catch { sDescuento =0; }

            //try
            //{ sTotal = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@subTotal", xnms).ToString()); }
            //catch { sSubtotal = 0; }

            //try
            //{ sSubtotal = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@subTotal", xnms).ToString()); }
            //catch { sSubtotal =0; }

            //try
            //{ sNombreEmisor = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", xnms).ToString(); }
            //catch { sNombreEmisor = string.Empty; }

            //try
            //{ sRFCEmisor = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", xnms).ToString(); }
            //catch { sRFCEmisor = string.Empty; }

            //try
            //{ sNombreReceptor = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", xnms).ToString(); }
            //catch { sNombreReceptor = string.Empty; }
           
            //try
            //{ sRFCReceptor = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", xnms).ToString(); }
            //catch { sFolio = string.Empty; }

            //try
            //{ sTotalImpuestosTrasladados = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados/@totalImpuestosTrasladados", xnms).ToString()); }
            //catch { sTotalImpuestosTrasladados = 0; }

            //try
            //{ sImporteliVA = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado[@impuesto ='IVA']/@importe", xnms).ToString()); }
            //catch { sImporteliVA = 0; }
            
            //try
            //{ sTasaIVA = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado[@impuesto ='IVA']/@tasa", xnms).ToString()); }
            //catch { sTasaIVA = 0; }

            //try
            //{ sSatUID = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", xnms).ToString(); }
            //catch { sSatUID = string.Empty; }

            //try
            //{ sFechatimbrado = DateTime.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", xnms).ToString()); }
            //catch {  }

            //try
            //{ sXmlCompleto = xdDoc.OuterXml; }
            //catch { sXmlCompleto = string.Empty; }


            try
            { sMoneda = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", xnms).ToString(); }
            catch { }

            try
            { sFolio = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", xnms).ToString(); }
            catch { }

            try
            { sSerie = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", xnms).ToString(); }
            catch { }

            try
            { sTipoDeComprobante = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@tipoDeComprobante", xnms).ToString(); }
            catch { }

            try
            { sDescuento = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@descuento", xnms).ToString()); }
            catch { }

            try
            { sTotal = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@subTotal", xnms).ToString()); }
            catch { }

            try
            { sSubtotal = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@subTotal", xnms).ToString()); }
            catch { }

            try
            { sNombreEmisor = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", xnms).ToString(); }
            catch { }

            try
            { sRFCEmisor = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", xnms).ToString(); }
            catch { }

            try
            { sNombreReceptor = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", xnms).ToString(); }
            catch { }

            try
            { sRFCReceptor = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", xnms).ToString(); }
            catch { }

            try
            { sTotalImpuestosTrasladados = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados/@totalImpuestosTrasladados", xnms).ToString()); }
            catch { }

            try
            { sImporteliVA = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado[@impuesto ='IVA']/@importe", xnms).ToString()); }
            catch { }

            try
            { sTasaIVA = double.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado[@impuesto ='IVA']/@tasa", xnms).ToString()); }
            catch { }

            try
            { sSatUID = xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", xnms).ToString(); }
            catch { }

            try
            { sFechatimbrado = DateTime.Parse(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", xnms).ToString()); }
            catch { sFechatimbrado = DateTime.Now; }

            try
            { sXmlCompleto = xdDoc.OuterXml; }
            catch { }
           
        
        }

        public object fnObtenerParametro(string sNombre)
        {
            string sValor = "";

            switch (sNombre.ToUpper()) 
            {
                case "MONEDA":
                   return sMoneda;
                    break;
                case "FOLIO":
                    return sFolio;
                    break;
                case "SERIE":
                    return sSerie;
                    break;
                case "TIPODECOMPROBANTE":
                    return sTipoDeComprobante;
                    break;
                case "TOTAL":
                    return sTotal;
                    break;
                case "DESCUENTO":
                    return sDescuento;
                    break;
                case "SUBTOTAL":
                    return sSubtotal;
                    break;
                case "NOMBREEMISOR":
                    return sNombreEmisor;
                    break;
                case "RFCEMISOR":
                    return sRFCEmisor;
                    break;
                case "NOMBRERECEPTOR":
                    return sNombreReceptor;
                    break;
                case "RFCRECEPTOR":
                    return sRFCReceptor;
                    break;
                case "TOTALIMPUESTOSTRASLADADOS":
                    return sTotalImpuestosTrasladados;
                    break;
                case "IMPORTELIVA":
                    return sImporteliVA;
                    break;
                case "TASAIVA":
                    return sTasaIVA;
                    break;
                case "SATUID":
                    return sSatUID;
                    break;
                case "FECHATIMBRADO":
                    return sFechatimbrado;
                    break;
                case "XMLCOMPLETO":
                   return sXmlCompleto;
                    break;
            
            
            
            }


            return sValor;
        
        }
    }
}
