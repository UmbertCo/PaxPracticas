using ConectorPDFSharp.Properties;
using ConectorPDFSharp.wcfCancelaASMX;
using ConectorPDFSharp.wsLicenciaASMX;
using System;
using System.Management;
using Microsoft.Win32;

//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


public class clsOperacionCancelar
{
    private RegistryKey clave;
    private wcfCancelaASMX wsCancelacion = new wcfCancelaASMX();
    private wsLicenciaASMXSoapClient wsLicencia = new wsLicenciaASMXSoapClient();

    public clsOperacionCancelar()
    {

    }

    public void fnGeneracionTimbrado()
    {
        DateTime Fecha = DateTime.Now;
        try
        {

            clsTimbradoCancelacion TimbradoCancelacion = new clsTimbradoCancelacion();
            TimbradoCancelacion.fnTimbradoCancelacion();
        }
        catch (Exception ex)
        {
            clsLog.EscribirLog("Error fnCancelacionTimbrado - " + ex.Message);
        }
    }

}


