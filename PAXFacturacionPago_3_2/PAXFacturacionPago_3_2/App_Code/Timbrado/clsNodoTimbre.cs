using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.IO;

/// <summary>
/// Clase encargada de preparar el objeto  representación del nodo timbre del comprobante
/// </summary>
public class clsNodoTimbre
{
	private TimbreFiscalDigital gNodoTimbre;
	private wslServicioPAC gServicio;
	private clsOperacionTimbradoSellado gTimbrado;
	private clsValCertificado gCertificado;
    private clsHSMComunicacion gHSM; 

	/// <summary>
	/// Crea una nueva instancia de clsNodoTimbre inicializando sus objetos de control
	/// </summary>
	public clsNodoTimbre()
	{

	}

    /// <summary>
    /// Genera el nodo TimbreFiscalDigital y se lo agrega al comprobante version 3.2
    /// </summary>
    /// <param name="pComprobante">Objeto del comprobante el cual será timbrado</param>
    public void GenerarNodoTimbre32(Comprobante pComprobante, ref string sCOriginal, string NodoComplemento, string NodoAddenda)
    {

        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsNodoTimbre.AcceptAllCertificatePolicy);

        byte[] bLlave;
        byte[] bCertificado;
        string sPassword = string.Empty;

        gNodoTimbre = new TimbreFiscalDigital();

        //verificamos que el comprobante tenga su sello de contribuyente
        if (string.IsNullOrEmpty(pComprobante.sello))
            throw new Exception("El comprobante no puede ser firmado sin antes tener un sello de contribuyente");

        //Llenamos los datos del nodo timbre
        gNodoTimbre.UUID = Guid.NewGuid().ToString();
        gNodoTimbre.FechaTimbrado = Convert.ToDateTime(DateTime.Now.ToString("s")); //Convert.ToDateTime(DateTime.Now.AddYears(1).AddDays(-22).AddMonths(-11).ToString("s"));
        gNodoTimbre.selloCFD = pComprobante.sello;

        //********Obtienen el numero del certificado del HSM********************

        if (clsComun.ObtenerParamentro("TipoTimbrado") == "HSM")
        {

            //Servicio HSM3
            wslServicioPAC gServicio = new wslServicioPAC();

            try
            {
                gHSM = new clsHSMComunicacion();
                gTimbrado = new clsOperacionTimbradoSellado();
                gNodoTimbre.noCertificadoSAT = gHSM.fnObtenerNumeroCertificado(gHSM.fnHSMLogin());
                gHSM.fnHSMLogOut();

                //Si no se Recupero el Certificado de HSM1 o HSM2
                if (gNodoTimbre.noCertificadoSAT == string.Empty)
                {
                    bLlave = gServicio.HSM3_KEY();
                    bCertificado = gServicio.HSM3_CER();
                    sPassword = gServicio.HSM3_PAS();

                    gTimbrado = new clsOperacionTimbradoSellado(bLlave, System.Text.Encoding.Unicode.GetBytes(sPassword));
                    gCertificado = new clsValCertificado(bCertificado);
                    gNodoTimbre.noCertificadoSAT = gCertificado.ObtenerNoCertificado();
                }
            }
            catch (Exception)
            {
                //Si falla HSM1 o HSM2
                bLlave = gServicio.HSM3_KEY();
                bCertificado = gServicio.HSM3_CER();
                sPassword = gServicio.HSM3_PAS();

                gTimbrado = new clsOperacionTimbradoSellado(bLlave, System.Text.Encoding.Unicode.GetBytes(sPassword));
                gCertificado = new clsValCertificado(bCertificado);
                gNodoTimbre.noCertificadoSAT = gCertificado.ObtenerNoCertificado();
            }

        }
        else
        {
            //Obtenemos los CSD del PAC otorgados por el SAT

            gServicio = new wslServicioPAC();

            bLlave = gServicio.ObtenerLlavePAC();
            bCertificado = gServicio.ObtenerCertificado();
            sPassword = gServicio.ObtenerPassword();

            //Preparamos los objetos de manejo tanto de la llave como del certificado
            gTimbrado = new clsOperacionTimbradoSellado(bLlave, System.Text.Encoding.Unicode.GetBytes(sPassword));
            gCertificado = new clsValCertificado(bCertificado);

            gNodoTimbre.noCertificadoSAT = gCertificado.ObtenerNoCertificado();
        }

        //********Obtienen el numero del certificado del HSM********************

        //Generamos el primer XML necesario para generar la cadena original
        XmlDocument xDocTimbrado = new XmlDocument();
        xDocTimbrado = gTimbrado.fnGenerarXML(gNodoTimbre);

        //Generamos la cadena original
        XPathNavigator navNodoTimbre = xDocTimbrado.CreateNavigator();
        string sCadenaOriginal = gTimbrado.fnConstruirCadenaTimbrado(navNodoTimbre, "cadenaoriginal_TFD_1_0");
        sCOriginal = sCadenaOriginal;
        //Generamos el sello del SAT, se lo agregamos al objeto y generamos el XML final
        //***********Genera la firma con el HSM*************************************
        
        XmlDocument Complemento = new XmlDocument();
        if (NodoComplemento != null)
        {
            // Format the document to ignore white spaces.
            Complemento.PreserveWhitespace = false;
            Complemento.LoadXml(NodoComplemento);
        }

    

     


        if (clsComun.ObtenerParamentro("TipoTimbrado") == "HSM")
        {

            try
            {
                gNodoTimbre.selloSAT = gHSM.fnFirmar(sCadenaOriginal, gHSM.fnHSMLogin());
                gHSM.fnHSMLogOut();

                if (gNodoTimbre.selloSAT == string.Empty)
                {
                    gNodoTimbre.selloSAT = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
                }
            }
            catch (Exception)
            {
                gNodoTimbre.selloSAT = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
            }

        }
        else
        {
            gNodoTimbre.selloSAT = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);

        }

        xDocTimbrado = gTimbrado.fnGenerarXML(gNodoTimbre);

        //***********Genera la firma con el HSM*************************************

        //Agregamos el nodo XML del timbre fiscal digital, al área del complemento del Comprobante
        ComprobanteComplemento complementoTimbreFiscalDigital = new ComprobanteComplemento();
       

        if (NodoComplemento != null)
        {

            complementoTimbreFiscalDigital.Any = new XmlElement[] { Complemento.DocumentElement, xDocTimbrado.DocumentElement };
            //}
        }
        else
        {
        
                complementoTimbreFiscalDigital.Any = new XmlElement[] { xDocTimbrado.DocumentElement };
       
        }

        //Por ultimo agregamos el complemento al comprobante
        pComprobante.Complemento = complementoTimbreFiscalDigital;
       

      
    }

    /// <summary>
    /// Genera el nodo TimbreFiscalDigital y se lo agrega al comprobante version 3.0
    /// </summary>
    /// <param name="pComprobante">Objeto del comprobante el cual será timbrado</param>
    public void GenerarNodoTimbre30(Comprobante30 pComprobante, ref string sCOriginal, string NodoComplemento, string NodoAddenda)
    {

        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsNodoTimbre.AcceptAllCertificatePolicy);

        gNodoTimbre = new TimbreFiscalDigital();

        //verificamos que el comprobante tenga su sello de contribuyente
        if (string.IsNullOrEmpty(pComprobante.sello))
            throw new Exception("El comprobante no puede ser firmado sin antes tener un sello de contribuyente");

        //Llenamos los datos del nodo timbre
        gNodoTimbre.UUID = Guid.NewGuid().ToString();
        gNodoTimbre.FechaTimbrado = Convert.ToDateTime(DateTime.Now.ToString("s"));//Convert.ToDateTime(DateTime.Now.AddYears(1).AddDays(-22).AddMonths(-11).ToString("s"));
        gNodoTimbre.selloCFD = pComprobante.sello;

        //********Obtienen el numero del certificado del HSM********************

        if (clsComun.ObtenerParamentro("TipoTimbrado") == "HSM")
        {

            gHSM = new clsHSMComunicacion();
            gTimbrado = new clsOperacionTimbradoSellado();
            gNodoTimbre.noCertificadoSAT = gHSM.fnObtenerNumeroCertificado(gHSM.fnHSMLogin());
            gHSM.fnHSMLogOut();

        }
        else
        {
            //Obtenemos los CSD del PAC otorgados por el SAT

            gServicio = new wslServicioPAC();

            byte[] bLlave = gServicio.ObtenerLlavePAC();
            byte[] bCertificado = gServicio.ObtenerCertificado();
            string sPassword = gServicio.ObtenerPassword();

            //Preparamos los objetos de manejo tanto de la llave como del certificado
            gTimbrado = new clsOperacionTimbradoSellado(bLlave, System.Text.Encoding.Unicode.GetBytes(sPassword));
            gCertificado = new clsValCertificado(bCertificado);

            gNodoTimbre.noCertificadoSAT = gCertificado.ObtenerNoCertificado();
        }

        //********Obtienen el numero del certificado del HSM********************

        //Generamos el primer XML necesario para generar la cadena original
        XmlDocument xDocTimbrado = new XmlDocument();
        xDocTimbrado = gTimbrado.fnGenerarXML(gNodoTimbre);

        //Generamos la cadena original
        XPathNavigator navNodoTimbre = xDocTimbrado.CreateNavigator();
        string sCadenaOriginal = gTimbrado.fnConstruirCadenaTimbrado(navNodoTimbre, "cadenaoriginal_TFD_1_0");
        sCOriginal = sCadenaOriginal;
        //Generamos el sello del SAT, se lo agregamos al objeto y generamos el XML final
        //***********Genera la firma con el HSM*************************************

        XmlDocument Complemento = new XmlDocument();
        if (NodoComplemento != null)
        {
            // Format the document to ignore white spaces.
            Complemento.PreserveWhitespace = false;
            Complemento.LoadXml(NodoComplemento);
        }

        

        if (clsComun.ObtenerParamentro("TipoTimbrado") == "HSM")
        {

            gNodoTimbre.selloSAT = gHSM.fnFirmar(sCadenaOriginal, gHSM.fnHSMLogin());
            gHSM.fnHSMLogOut();

        }
        else
        {
            gNodoTimbre.selloSAT = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);

        }

        xDocTimbrado = gTimbrado.fnGenerarXML(gNodoTimbre);

        //***********Genera la firma con el HSM*************************************

        //Agregamos el nodo XML del timbre fiscal digital, al área del complemento del Comprobante
        ComprobanteComplemento30 complementoTimbreFiscalDigital = new ComprobanteComplemento30();


        if (NodoComplemento != null)
        {

            complementoTimbreFiscalDigital.Any = new XmlElement[] { xDocTimbrado.DocumentElement, Complemento.DocumentElement };
            //}
        }
        else
        {

            complementoTimbreFiscalDigital.Any = new XmlElement[] { xDocTimbrado.DocumentElement };

        }



        //Por ultimo agregamos el complemento al comprobante
        pComprobante.Complemento = complementoTimbreFiscalDigital;



    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    /// <summary>
    /// genera nodo addenda version 3.2
    /// </summary>
    /// <param name="pComprobante"></param>
    /// <param name="xDoc"></param>
    /// <param name="NodoAddenda"></param>
    public void GenerarNodoAddenda32(Comprobante pComprobante, string NodoAddenda)
    {
        XmlDocument Addenda = new XmlDocument();

        if (NodoAddenda != null)
        {
            // Format the document to ignore white spaces.
            Addenda.PreserveWhitespace = false;
            Addenda.LoadXml(NodoAddenda);

            ComprobanteAddenda ComplementoAddenda = new ComprobanteAddenda();
            if (NodoAddenda != null)
            {
                ComplementoAddenda.Any = new XmlElement[] { Addenda.DocumentElement };

                pComprobante.Addenda = ComplementoAddenda;
               
            }
        
        }

    }

    /// <summary>
    /// genera nodo addenda version 3.0
    /// </summary>
    /// <param name="pComprobante"></param>
    /// <param name="xDoc"></param>
    /// <param name="NodoAddenda"></param>
    public void GenerarNodoAddenda30(Comprobante30 pComprobante, string NodoAddenda)
    {
        XmlDocument Addenda = new XmlDocument();

        if (NodoAddenda != null)
        {
            // Format the document to ignore white spaces.
            Addenda.PreserveWhitespace = false;
            Addenda.LoadXml(NodoAddenda);

            ComprobanteAddenda30 ComplementoAddenda = new ComprobanteAddenda30();
            if (NodoAddenda != null)
            {
                ComplementoAddenda.Any = new XmlElement[] { Addenda.DocumentElement };

                pComprobante.Addenda = ComplementoAddenda;

            }

        }

    }

    
}
