using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class webComplementos : System.Web.UI.Page
{
    static Page sPage = new Page();
    private static Hashtable htComplementos = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page objp = new Page();
        

        if (objp.Session["Complemento"] != null)
        {
            string sContenidoComplemento = objp.Session["Complemento"].ToString();

            HtmlGenericControl hcDiv = ((HtmlGenericControl)this.Page.FindControl("divComplemento"));

            Control ctrl = Page.ParseControl(sContenidoComplemento);
            hcDiv.Controls.Add(ctrl);
        }

        if (!IsPostBack)
        {
            fnIniciarlizarTablaComplementos();
        }
    }

    [System.Web.Services.WebMethod]
    public static string fnAgregarComplemento(string psRutaComplemento, string psNombreComplemento, string pnNumeroComplementos)
    {
        string sResultado = string.Empty;
        try
        {
            fnIniciarlizarTablaComplementos();

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(psRutaComplemento);

            XsltSettings settings = new XsltSettings(true, true);

            string sRutaArchivoTransformacion = (string)htComplementos[psNombreComplemento];

            XslCompiledTransform transform = new XslCompiledTransform(true);
            transform.Load(sRutaArchivoTransformacion, settings, new XmlUrlResolver());

            //transform.x

            // load xslt arguments to load specific page from xml file
            // this can be used if you have multiple pages
            // in your xml file and you loading them one at a time
            XsltArgumentList xslarg = new XsltArgumentList();
            xslarg.AddParam("NombreObjetos", "", string.Format("Com_{0}_{1}_", psNombreComplemento, pnNumeroComplementos.ToString()));
            xslarg.AddParam("NumeroComplemento", "", pnNumeroComplementos.ToString());
            xslarg.AddParam("NombreComplemento", "", psNombreComplemento.ToLower());
            xslarg.AddParam("Resource", "", @"D:\Forms Dinamicos\App_GlobalResources\resCorpusCFDIEs.en-Us.resx");


            // get transformed results
            StringWriter sw = new StringWriter();
            //xsl.Transform(xdoc, xslarg, sw);
            transform.Transform(xdoc, xslarg, sw);
            sResultado = sw.ToString().Replace("xmlns:asp=\"remove\"",
                     "").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&amp;", "&").Replace("&apos;", "'");

            sResultado = sResultado.Replace("xmlns:cc1=\"remove\"",
                     "").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&amp;", "&").Replace("&apos;", "'");

            sResultado = sResultado.Replace("xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"", "");

            // free up the memory of objects that are not used anymore
            sw.Close();


            //sResultado = "<%@ Register Assembly=\"AjaxControlToolkit\" Namespace=\"AjaxControlToolkit\" TagPrefix=\"cc1\" %>" + sResultado;
        }
        catch (Exception ex)
        {

        }
        return sResultado;
    }

    [System.Web.Services.WebMethod]
    public static void fnEstablecerComplementoSesion(string psComplemento)
    {
        Page objp = new Page();
        objp.Session["Complemento"] = psComplemento;
    }

    private static void fnIniciarlizarTablaComplementos()
    {
        htComplementos = new Hashtable();
        htComplementos.Add("aieps", @"D:\Forms Dinamicos\Plantillas\Plantilla_AcreditamientoIEPS10.xslt");
        htComplementos.Add("aerolineas", @"D:\Forms Dinamicos\Plantillas\Plantilla_Aerolineas.xslt");
        //htComplementos.Add("aerolineas", @"D:\Forms Dinamicos\Plantillas\Plantilla_base.xslt");
        htComplementos.Add("destruccion", @"D:\Forms Dinamicos\Plantillas\Plantilla_CertificadoDeDestruccion.xslt");
        htComplementos.Add("CFDIRegistroFiscal", @"D:\Forms Dinamicos\Plantillas\Plantilla_RegistroFiscal.xslt");
        htComplementos.Add("consumodecombustibles", @"D:\Forms Dinamicos\Plantillas\Plantilla_consumocombustible.xslt");
        htComplementos.Add("catComExt", @"D:\Forms Dinamicos\Plantillas\Plantilla_ComExt.xslt");
        htComplementos.Add("Detallista", @"D:\Forms Dinamicos\Plantillas\Plantilla_base.xslt");
        htComplementos.Add("Divisas", @"D:\Forms Dinamicos\Plantillas\Plantilla_Divisas.xslt");
        htComplementos.Add("Donatarias", @"D:\Forms Dinamicos\Plantillas\Plantilla_donat.xslt");
        htComplementos.Add("ECC", @"D:\Forms Dinamicos\Plantillas\Plantilla_base.xslt");
        htComplementos.Add("ECC11", @"D:\Forms Dinamicos\Plantillas\Plantilla_ecc11.xslt");
        htComplementos.Add("IEDU", @"D:\Forms Dinamicos\Plantillas\Plantilla_IEDU.xslt");
        htComplementos.Add("ImpLocal", @"D:\Forms Dinamicos\Plantillas\Plantilla_impLocal.xslt");
        htComplementos.Add("ine", @"D:\Forms Dinamicos\Plantillas\Plantilla_INE11.xslt");
        htComplementos.Add("LeyendasFiscales", @"D:\Forms Dinamicos\Plantillas\Plantilla_leyendasFisc.xslt");
        htComplementos.Add("Nomina", @"D:\Forms Dinamicos\Plantillas\Plantilla_base.xslt");
        htComplementos.Add("Nomina12", @"D:\Forms Dinamicos\Plantillas\Plantilla_base.xslt");
        htComplementos.Add("notariospublicos", @"D:\Forms Dinamicos\Plantillas\Plantilla_notariospublicos.xslt");
        htComplementos.Add("ObrasDeArteAntiguedades", @"D:\Forms Dinamicos\Plantillas\Plantilla_obrasarteantiguedades.xslt");
        htComplementos.Add("PagoEnEspecie", @"D:\Forms Dinamicos\Plantillas\Plantilla_pagoenespecie.xslt");
        htComplementos.Add("Pagos10", @"D:\Forms Dinamicos\Plantillas\Plantilla_base.xslt");
        htComplementos.Add("PFIC", @"D:\Forms Dinamicos\Plantillas\Plantilla_pfic.xslt");
        htComplementos.Add("renysustvehiculos", @"D:\Forms Dinamicos\Plantillas\Plantilla_renovacionysustitucionvehiculos.xslt");
        htComplementos.Add("ServicioParcialConstruccion", @"D:\Forms Dinamicos\Plantillas\Plantilla_servicioparcialconstruccion.xslt");
        htComplementos.Add("SPEI", @"D:\Forms Dinamicos\Plantillas\Plantilla_spei.xslt");
        htComplementos.Add("Terceros", @"D:\Forms Dinamicos\Plantillas\Plantilla_terceros.xslt");
        htComplementos.Add("TuristaPasajeroExtranjero", @"D:\Forms Dinamicos\Plantillas\Plantilla_TuristaPasajeroExtranjero.xslt");
        htComplementos.Add("ValesDeDespensa", @"D:\Forms Dinamicos\Plantillas\Plantilla_valesdedespensa.xslt");
        htComplementos.Add("VehiculoUsado", @"D:\Forms Dinamicos\Plantillas\Plantilla_vehiculousado.xslt");
        htComplementos.Add("ventavehiculos", @"D:\Forms Dinamicos\Plantillas\Plantilla_PRUEBA.xslt");
        htComplementos.Add("InfoAduaVU", @"D:\Forms Dinamicos\Plantillas\NodoOpcional\Plantilla_InfoAdua_vehiculousado.xslt");
        htComplementos.Add("InfoAduaVV", @"D:\Forms Dinamicos\Plantillas\NodoOpcional\Plantilla_InfoAdua_ventavehiculo.xslt");
        htComplementos.Add("InfoPartVV", @"D:\Forms Dinamicos\Plantillas\NodoOpcional\Plantilla_Parts_ventavehiculo.xslt");
        htComplementos.Add("InfoAduaPartVV", @"D:\Forms Dinamicos\Plantillas\NodoOpcional\Plantilla_InfoAduaPart_ventavehiculo.xslt");
    }
}