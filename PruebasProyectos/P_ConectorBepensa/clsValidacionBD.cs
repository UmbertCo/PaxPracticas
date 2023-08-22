using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace P_ConectorBepensa
{
    class clsValidacionBD
    {
        public string sRutaINS { set; get; }
        public string sRutaSEL { set; get; }
        public string sConnectionString { set; get; }
        public XmlDocument xdDocumento { set; get; }
        public clsTbl_Facturas tblFacturas { set; get; }
        clsCorrerSP_Scripts spAccesoBDINS;
        clsCorrerSP_Scripts spAccesoBDSEL;

        public clsValidacionBD(XmlDocument pxdDocumento,String psConnectionString, String psRutaINS, String psRutaSEL) 
        {
            xdDocumento = pxdDocumento;
            sRutaINS = psRutaINS;
            sRutaSEL = psRutaSEL;
            tblFacturas = new clsTbl_Facturas(pxdDocumento);
            sConnectionString = psConnectionString;
            spAccesoBDINS = new clsCorrerSP_Scripts(sConnectionString, sRutaINS, EnuTipoScript.INS);
            spAccesoBDSEL = new clsCorrerSP_Scripts(sConnectionString, sRutaSEL, EnuTipoScript.SEL);

            spAccesoBDINS.tfParametrosEntrada = tblFacturas;

            spAccesoBDSEL.tfParametrosEntrada = tblFacturas;
        }

        public bool fnInsertarComprobante() 
        {
            
            
            spAccesoBDINS.fnProc();

            if (spAccesoBDINS.nResultado == 0)
                return false;
            else
                return true;
        

        }

        public bool fnExisteComprobante() 
        {

            

            spAccesoBDSEL.fnProc();

            if (spAccesoBDSEL.nResultado == 0)
                return false;
            else
                return true;
        
        }
    }
}
