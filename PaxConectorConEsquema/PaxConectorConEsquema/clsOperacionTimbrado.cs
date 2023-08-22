using Microsoft.Win32;
using PaxConectorConEsquema.Properties;
using PaxConectorConEsquema.wsLicenciaASMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

public class clsOperacionTimbrado
{
    //Licencia
    private RegistryKey clave;
    private wsLicenciaASMXSoapClient wsLicencia = new wsLicenciaASMXSoapClient();
    private Boolean gbValida = false;

    public clsOperacionTimbrado()
    { 

    }

    public void fnGeneracionTimbrado(string psTipoServicio)
    {
        DateTime Fecha = DateTime.Now;
        try
        {
            Boolean bValida = false;
            //Se verifica la licencia.
            bValida = fnValidaLicenciaLlavePC();
            //bValida = true;

            //Si contiene licencia
            if (bValida == true)
            {
                switch (psTipoServicio)
                {
                    case "GT": //Timbrado
                        clsTimbradoGeneracion TimbradoGeneracion = new clsTimbradoGeneracion();
                        TimbradoGeneracion.fnTimbradoGeneracion();
                        break;
                }
            }
            else
            {
                throw new Exception("No se pudo validar la licencia");
            }
        }
        catch (Exception ex)
        {
            clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "Error al iniciar el servicio." + " " + ex.Message);
        }
    }

    /// <summary>
    /// Obtiene el numero serial de la pc local
    /// </summary>
    /// <returns></returns>
    private string fnObtenerSerialNumber()
    {
        string sSerialNumber = string.Empty;
        ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
        scope.Connect();
        ManagementObject wmiClass = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());

        foreach (PropertyData propData in wmiClass.Properties)
        {
            if (propData.Name == "SerialNumber")
            {
                sSerialNumber = Convert.ToString(propData.Value);
            }
        }

        sSerialNumber = Utilerias.Encriptacion.Base64.EncriptarBase64(sSerialNumber);

        return sSerialNumber;
    }

    /// <summary>
    /// Valida registro licencia
    /// </summary>
    /// <returns></returns>
    private bool fnValidaLicenciaLlavePC()
    {
        bool bValida = false;
        string Cadena = string.Empty;
        string sSerialNumber = string.Empty;
        try
        {
            //Se inicializan las variables en el registro para utilizarlas en el servicio.
            //Obtiene llave PC
            sSerialNumber = fnObtenerSerialNumber();
            //Se busca registro anterior en PC
            this.clave = Registry.CurrentUser.OpenSubKey(@"Software\DDNSCDMON\" + Settings.Default.Estatus, true); //PRODUCCION
            string sLlaveRegistro = "";

            try
            {
                sLlaveRegistro = this.clave.GetValue("LLAVE").ToString();
            }
            catch {

                sLlaveRegistro = "";
            }

            if (this.clave == null || sLlaveRegistro == "")
            {
                //Se obtiene el usuario el cual contiene licencia
                string usuario = Settings.Default.usuario;
                string Origen = Settings.Default.Origen;
                //Por primera vez verifica licencias suficientes del usuario en caso de contener se registra en base de datos
                Cadena = wsLicencia.fnObtenerLlaveVersionUsuario(usuario, sSerialNumber, Origen, DateTime.Today);
                Cadena = Utilerias.Encriptacion.Base64.DesencriptarBase64(Cadena);
                string[] cad = Cadena.Split('|');

                int Validez = Convert.ToInt32(cad[4]);
                //Si contiene suficientes licencias
                if (Validez == 1)
                {
                    //Se guarda llave en registro de PC
                    this.clave = Registry.CurrentUser.CreateSubKey(@"Software\DDNSCDMON\" + Settings.Default.Estatus);
                    this.clave.SetValue("LLAVE", sSerialNumber);
                    this.clave.Close();
                    this.clave = null;
                    bValida = true;
                }
                else
                {
                    bValida = false;
                    throw new Exception("Licencias insuficientes, contacte a su proveedor.");
                }
            }
            else
            {
                //Se obtiene la llave registrada en PC
                //sLlaveRegistro = this.clave.GetValue("LLAVE").ToString();
                string sLlaveDesen = Utilerias.Encriptacion.Base64.DesencriptarBase64(sLlaveRegistro);
                //Se valida que la llave registrada sea igual a la llave PC la cual contiene el sistema
                if (Utilerias.Encriptacion.Base64.DesencriptarBase64(sSerialNumber) != Utilerias.Encriptacion.Base64.DesencriptarBase64(sLlaveRegistro))
                {
                    bValida = false;
                    throw new Exception("La licencia no coincide con la registrada.");
                }
                else
                {
                    bValida = true;
                }
            }
            
        }
        catch (Exception ex)
        {
            bValida = false;
            //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Error al validar la licencia. " + ex.Message);
            throw ex;
        }
        return bValida;
    }
}

