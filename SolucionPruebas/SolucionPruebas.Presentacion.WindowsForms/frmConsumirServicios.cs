using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmConsumirServicios : Form
    {
        public frmConsumirServicios()
        {
            InitializeComponent();
        }

        private void btnConsumirServicioJson_Click(object sender, EventArgs e)
        {
            XmlDocument xAddenda = new XmlDocument();
            string sAddenda = string.Empty;
            try
            {
                sAddenda = fnConsumirServicioJson("http://201.174.72.60:1337/?folio=" + txtFolio.Text);
                if (sAddenda.Equals(string.Empty))
                {
                    return;
                }

                using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(sAddenda), XmlDictionaryReaderQuotas.Max))
                {
                    XElement xml = XElement.Load(reader);
                    xAddenda.LoadXml(xml.ToString());
                }

                foreach (XmlNode xNodo in xAddenda.ChildNodes[0].ChildNodes[0])
                {
                    xNodo.Attributes.RemoveNamedItem("type");
                }

                txtResultados.Text = xAddenda.InnerXml;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string fnConsumirServicioJson(string sUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sUrl);
            string sResultado = string.Empty;
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    sResultado = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    throw new Exception(ex.Message);
                }
                else
                { 
                    WebResponse errorResponse = ex.Response;
                    string sError = string.Empty;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        sError = reader.ReadToEnd();
                    }
                    throw new Exception(sError);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Erro al consumir el servicio JSON: " + ex.Message);
            }
            return sResultado;
        }
    }
}
