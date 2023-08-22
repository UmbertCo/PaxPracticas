using System;
using System.IO;
using System.Xml;
using System.Web;
using System.Net;
using System.Linq;
using System.Data;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SolucionPruebas.Presentacion.ConsoleApplication
{
    class clsValCertificado
    {
        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
        private Byte[] gbCertificado;
        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
        // Retorna o establece el certificado como arreglo de bytes
        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
        public Byte[] CertificadoBytes
        {
            get { return gbCertificado; }
            set { gbCertificado = value; }
        }
        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
        private X509Certificate2 certificado;

        private X509Certificate2 certificadoPAC;
        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
        // Retorna el certificado como un objeto de .NET
        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
        public X509Certificate2 Certificado
        {
            get
            {
                return certificado;
            }
        }

        public X509Certificate2 CertificadoPAC
        {
            get
            {
                return certificadoPAC;
            }
            set
            {
                certificadoPAC = value;
            }
        }
        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
        // Crea una nueva instancia de .NET tomando los datos del arreglo de bytes enviado
        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
        public clsValCertificado(Byte[] pbCertificado)
        {
            try
            {
                certificado = new X509Certificate2(pbCertificado);
                gbCertificado = pbCertificado;
            }
            catch
            {
                try
                {
                    certificado = new X509Certificate2(fnDesencriptarCertificado(pbCertificado));
                    gbCertificado = fnDesencriptarCertificado(pbCertificado);
                }
                catch (Exception ex)
                {
                    throw new CryptographicException("El certificado esta bloqueado");
                }
            }

            if (certificado.Verify())
                throw new CryptographicException("El certificado no pasó la verificación");


        }

        private Byte[] fnDesencriptarCertificado(Byte[] pbCertificadoEncriptado)
        {
            //return Utilerias.Encriptacion.DES3.Desencriptar(pbCertificadoEncriptado);
            return (pbCertificadoEncriptado);
        }

        public DataTable RevisaExistenciaCertificadoFechas(String no_serie, String estado_cer)
        {
            DataTable gdtAuxiliar = new DataTable();

            String cadenaCon = "Data Source=10.60.69.4;Initial Catalog=CFDI_Crypto;Persist Security Info=True;User ID=corpusgood;Password=F4cturax10n";

            using (SqlConnection conexion = new SqlConnection(cadenaCon))
            {
                try
                {
                    conexion.Open();

                    using (SqlCommand command = new SqlCommand("usp_Ctp_RevisarExistenciaFechasCSD", conexion))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("no_serie", no_serie);
                        command.Parameters.AddWithValue("sEstado", estado_cer);
                        command.ExecuteNonQuery();
                        adapter.Fill(gdtAuxiliar);
                        adapter.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                }
                finally
                {
                    conexion.Close();
                }
                return gdtAuxiliar;

            }
        }

        public Boolean ComprobarFechas()
        {

            if (certificado.NotBefore.CompareTo(DateTime.Today) > 0
                || certificado.NotAfter.CompareTo(DateTime.Today) < 0)
                return false;

            return true;
        }
    }
}
