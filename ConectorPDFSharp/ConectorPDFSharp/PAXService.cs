using System;
using Microsoft.Win32;
using System.Management;
using System.ServiceProcess;
using ConectorPDFSharp.Properties;
using ConectorPDFSharp.wsLicenciaASMX;
using System.IO;
using System.Diagnostics;

namespace ConectorPDFSharp
{
    public partial class PAXService : ServiceBase
    {

        private System.Timers.Timer Timer = null;
        private wsLicenciaASMXSoapClient wsLicencia = new wsLicenciaASMXSoapClient();
        private RegistryKey clave;
        private Boolean bValida = true;

        public PAXService()
        {
            InitializeComponent();
            //Debugger.Launch();
            Timer = new System.Timers.Timer();
            try { bValida = fnValidaLicenciaLlavePC(); }
            catch (Exception ex) { clsLog.EscribirLog("Error al validar lincencia " + ex.Message); }
        }

        protected override void OnStart(string[] args)
        {
            if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
                System.Diagnostics.Debugger.Launch();

            Timer = new System.Timers.Timer();
            Timer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);
            Timer.Enabled = true;
        }

        void myTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int timeToGo;
            //Revisar el tiempo para iniciar el proceso
            timeToGo = Convert.ToInt32(Settings.Default.Intervalo);
            Timer.Interval = timeToGo;
            //Si contiene licencia
            if (bValida == true)
            {
                fnIniciarTimbrado();
            }
            else
            {
                clsLog.EscribirLog("No se cuenta con una licencia, contacte a su proveedor.");
            }
        }


        protected override void OnStop()
        {
            Timer.Enabled = false;
        }

        private void fnIniciarTimbrado()
        {
            Timer.Enabled = false;
            try
            {
                //Verifica si existen carpetas en el cual se almacena los archivos correspondientes
                if (!(Directory.Exists(Settings.Default.RutaDocZips)))
                    Directory.CreateDirectory(Settings.Default.RutaDocZips);
                if (!(Directory.Exists(Settings.Default.LogError)))
                    Directory.CreateDirectory(Settings.Default.LogError);
                if (!(Directory.Exists(Settings.Default.LogTimbrados)))
                    Directory.CreateDirectory(Settings.Default.LogTimbrados);
                if (!(Directory.Exists(Settings.Default.RutaDocs)))
                    Directory.CreateDirectory(Settings.Default.RutaDocs);
                if (!(Directory.Exists(Settings.Default.RutaTxtGen)))
                    Directory.CreateDirectory(Settings.Default.RutaTxtGen);
                if (!(Directory.Exists(Settings.Default.RutaDocInv)))
                    Directory.CreateDirectory(Settings.Default.RutaDocInv);
                if (!(Directory.Exists(Settings.Default.RutaCertificados)))
                    Directory.CreateDirectory(Settings.Default.RutaCertificados);
                if (!(Directory.Exists(Settings.Default.RutaDesconexion)))
                    Directory.CreateDirectory(Settings.Default.RutaDesconexion);
                if (!(Directory.Exists(Settings.Default.RutaPDF)))
                    Directory.CreateDirectory(Settings.Default.RutaPDF);
                if (!(Directory.Exists(Settings.Default.RutaDocCan)))
                    Directory.CreateDirectory(Settings.Default.RutaDocCan);
                if (!(Directory.Exists(Settings.Default.LogCancelacion)))
                    Directory.CreateDirectory(Settings.Default.LogCancelacion);
               

                clsOperacionTimbrado OperacionTimbrado = new clsOperacionTimbrado();
                clsTimbradoCancelacion TimbradoCancelacion = new clsTimbradoCancelacion();

                OperacionTimbrado.fnGeneracionTimbrado();
                TimbradoCancelacion.fnTimbradoCancelacion();

            }
            catch (Exception ex)
            {
                clsLog.EscribirLog("Error Directorios - " + ex.Message);
            }
            finally
            {
                Timer.Enabled = true;
            }
        }

        /// <summary>
        /// Obtiene el numero serial de la pc local
        /// </summary>
        /// <returns></returns>
        private string fnObtenerSerialNumber()
        {
            string sSerialNumber = string.Empty;

            try
            {
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
            }
            catch (Exception ex)
            {
                clsLog.EscribirLog("Error al Obtener No. Serie - fnObtenerSerialNumber - " + ex.Message);
            }

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
                if (this.clave == null)
                {
                    //Se obtiene el usuario el cual contiene licencia
                    string usuario = Settings.Default.Usuario;
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
                    }
                    else
                    {
                        throw new Exception("Licencias insuficientes, contacte a su proveedor.");
                    }
                }
                else
                {
                    //Se obtiene la llave registrada en PC
                    string sLlaveRegistro = this.clave.GetValue("LLAVE").ToString();
                    string sLlaveDesen = Utilerias.Encriptacion.Base64.DesencriptarBase64(sLlaveRegistro);
                    //Se valida que la llave registrada sea igual a la llave PC la cual contiene el sistema
                    if (Utilerias.Encriptacion.Base64.DesencriptarBase64(sSerialNumber) != Utilerias.Encriptacion.Base64.DesencriptarBase64(sLlaveRegistro))
                    {
                        throw new Exception("La licencia no coincide con la registrada.");
                    }
                }
                bValida = true;
            }
            catch (Exception ex)
            {
                clsLog.EscribirLog("Error al Validar Licencia - fnValidaLicenciaLlavePC - " + ex.Message);
                throw ex;
            }
            return bValida;
        }

    }
}
