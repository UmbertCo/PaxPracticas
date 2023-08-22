using Microsoft.Win32;
using ConectorPDFSharp.Properties;
using ConectorPDFSharp.wsLicenciaASMX;
using System;
using System.Management;

public class clsOperacionTimbrado
{
    //Licencia


    public clsOperacionTimbrado()
    {

    }

    public void fnGeneracionTimbrado()
    {
        DateTime Fecha = DateTime.Now;
        try
        {

            clsTimbradoGeneracion TimbradoGeneracion = new clsTimbradoGeneracion();
            TimbradoGeneracion.fnTimbradoGeneracion();


        }
        catch (Exception ex)
        {
            clsLog.EscribirLog("Error fnGeneracionTimbrado - " + ex.Message);
        }
    }


}

