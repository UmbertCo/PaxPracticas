using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Management;

namespace ConectorPAXGenericoPDF
{
    public class clsOperacionTimbrado
    {
        public ConectorPAXGenericoPDF.wsLicenciaASMX.wsLicenciaASMX wsLicencia = new ConectorPAXGenericoPDF.wsLicenciaASMX.wsLicenciaASMX();
        public RegistryKey clave;

        public void fnGeneracionTimbrado(string sTipoServicio)
        {
            try
            {
                bool bValida = false;

                //Se verifica la licencia.
                bValida = fnValidaLicenciaLlavePC();

                //Si contiene licencia
                if (bValida == true)
                {
                    switch (sTipoServicio)
                    {
                        case "GT":
                            clsTimbradoGeneracion TimbradoGeneracion = new clsTimbradoGeneracion();
                            TimbradoGeneracion.fnTimbradoGeneracion(sTipoServicio);
                            break;
                        case "GE":
                            clsTimbradoGeneracion TimbradoGeneracionEnvio = new clsTimbradoGeneracion();
                            TimbradoGeneracionEnvio.fnTimbradoGeneracion(sTipoServicio);
                            break;
                    }
                    string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["Reimpresion"];
                    if (Directory.EnumerateFiles(path).Any())
                    {
                        string filtro = "*.xml";
                        string[] Files = null;
                        Files = Directory.GetFiles(path, filtro);
                        if (Files.Length > 0)
                        {
                            Reimpresion RE = new Reimpresion();
                            RE.fnEnviarTXT();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    DateTime Fecha = DateTime.Today;
                    string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                    if (!File.Exists(path))
                    {
                        StreamWriter sr4 = new StreamWriter(path);
                        sr4.WriteLine(DateTime.Now + " " + ex.Message);
                        sr4.Close();
                    }
                    else
                    {
                        System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                        sw4.WriteLine(DateTime.Now + " " + ex.Message);
                        sw4.Close();
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Valida registro licencia
        /// </summary>
        private bool fnValidaLicenciaLlavePC()
        {
            bool bValida = true;
            string Cadena = string.Empty;
            try
            {
                //Se inicializan las variables en el registro para utilizarlas en el servicio.
                //Obtiene llave PC
                string SerialNumber = ObtenerSerialNumber();
                //Se busca registro anterior en PC
                this.clave = Registry.CurrentUser.OpenSubKey(@"Software\DDNSCDMON\PRODUCCION", true);
                if (this.clave == null)
                {
                    //Se obtiene el usuario el cual contiene licencia
                    string usuario = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["usuario"];
                    string Origen = "ConectorPAXGenerico";
                    //Por primera vez verifica licencias suficientes del usuario en caso de contener se registra en base de datos
                    Cadena = wsLicencia.fnObtenerLlaveVersionUsuario(usuario, SerialNumber, Origen, DateTime.Today);
                    Cadena = Utilerias.Encriptacion.Base64.DesencriptarBase64(Cadena);
                    string[] cad = Cadena.Split('|');

                    int Validez = Convert.ToInt32(cad[4]);
                    //Si contiene suficientes licencias
                    if (Validez == 1)
                    {
                        //Se guarda llave en registro de PC
                        this.clave = Registry.CurrentUser.CreateSubKey(@"Software\DDNSCDMON\PRODUCCION");
                        this.clave.SetValue("LLAVE", SerialNumber);
                        this.clave.Close();
                        this.clave = null;
                    }
                    else
                    {
                        bValida = false;
                        throw new Exception("Licencias insuficientes, contacte a su proveedor");
                    }

                    //this.clave = Registry.CurrentUser.OpenSubKey(@"Software\DDNSCDMON\PAXREGISTRO", true);
                }
                else
                {
                    //Se obtiene la llave registrada en PC
                    string sLlaveRegistro = this.clave.GetValue("LLAVE").ToString();
                    string sLlaveDesen = Utilerias.Encriptacion.Base64.DesencriptarBase64(sLlaveRegistro);
                    //Se valida que la llave registrada sea igual a la llave PC la cual contiene el sistema
                    if (Utilerias.Encriptacion.Base64.DesencriptarBase64(SerialNumber) != Utilerias.Encriptacion.Base64.DesencriptarBase64(sLlaveRegistro))
                    {
                        bValida = false;
                        throw new Exception("La licencia no coincide con la registrada");
                    }
                }

            }
            catch (Exception ex)
            {
                try
                {
                    bValida = false;
                    DateTime Fecha = DateTime.Today;
                    string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                    if (!File.Exists(path))
                    {
                        StreamWriter sr4 = new StreamWriter(path);
                        sr4.WriteLine(DateTime.Now + " " + ex.Message);
                        sr4.Close();
                    }
                    else
                    {
                        System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                        sw4.WriteLine(DateTime.Now + " " + ex.Message);
                        sw4.Close();
                    }
                }
                catch
                {
                }
            }
            return bValida;
        }

        /// <summary>
        /// Obtiene el numero serial de la pc local
        /// </summary>
        /// <returns></returns>
        public string ObtenerSerialNumber()
        {
            ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
            scope.Connect();
            string SerialNumber = string.Empty;
            ManagementObject wmiClass = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());

            foreach (PropertyData propData in wmiClass.Properties)
            {
                if (propData.Name == "SerialNumber")
                {
                    SerialNumber = Convert.ToString(propData.Value);
                }

            }
            SerialNumber = Utilerias.Encriptacion.Base64.EncriptarBase64(SerialNumber);
            return SerialNumber;
        }
    }
}
