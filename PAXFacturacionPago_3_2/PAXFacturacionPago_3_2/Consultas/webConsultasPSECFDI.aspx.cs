using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Threading;
using System.Globalization;

public partial class Consultas_PSECFDI_webConsultasPSECFDI : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFechaIni.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
       
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnDescargarTXT);
    }

    /// <summary>
    /// Loads the language specific resources
    /// </summary>
    protected override void InitializeCulture()
    {
        if (Session["Culture"] != null)
        {
            string lang = Session["Culture"].ToString();
            if ((lang != null) || (lang != ""))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
            }
        }
        else
        {
            string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }

    protected void btnDescargar_Click(object sender, EventArgs e)
    {
        if (txtFechaIni.Text != "" && txtFechaFin.Text != "" && (chbNumeroTotAcumEmisores.Checked == true || chbNumeroTotalAcum.Checked == true || chbNumTotSolicitEmi.Checked == true))
        {
            try
            {
                DateTime fechaInicial = Convert.ToDateTime(txtFechaIni.Text);
                DateTime fechaFinal = Convert.ToDateTime(txtFechaFin.Text);

                string fechaInicialForm = fechaInicial.ToString("yyyy-MM-dd");
                string fechaFinalForm = fechaFinal.ToString("yyyy-MM-dd");

                int seleccion = 0;

                if (chbNumeroTotalAcum.Checked == true)
                {
                    seleccion = 1;
                    TotalesDe.Text = "Total CFDI";
                    btnDescargarTXT.Visible = true;
                }
                if (chbNumeroTotAcumEmisores.Checked == true)
                {
                    seleccion = 2;
                    TotalesDe.Text = "Total RFC Emisores";
                    btnDescargarTXT.Visible = false;
                }
                if (chbNumTotSolicitEmi.Checked == true)
                {
                    seleccion = 3;
                    TotalesDe.Text = "Total RFC Adquirientes";
                    btnDescargarTXT.Visible = false;
                }

                int resultadoTotales = clsComun.fnObtenerConsultaComprobantesPSECFDITotales(fechaInicialForm, fechaFinalForm, seleccion);
                int resultadoComprobantes = clsComun.fnObtenerComprobantesTotales(seleccion);
                //int resultadoComprobantes = clsComun.fnObtenerComprobantesHistorico(seleccion);

                Totales.Text = resultadoTotales.ToString();

                if (Totales.Text == "0")
                {
                    btnDescargarTXT.Visible = false;
                }
                else if (Totales.Text != "0" && seleccion == 1)
                {
                    btnDescargarTXT.Visible = true;
                }

                Acumulado.Text = resultadoComprobantes.ToString();

                mpeShowTotales.Show();
            }
            catch
            {
                Response.Write("<script>alert('Se debe utilizar el formato de fecha correspondiente')</script>");
            }
            
        }
        
    }


    protected void ServerButton_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
    }


    protected void btnDescargarTXT_Click(object sender, EventArgs e)
    {
        

        DateTime fechaInicial = Convert.ToDateTime(txtFechaIni.Text);
        DateTime fechaFinal = Convert.ToDateTime(txtFechaFin.Text);

        string fechaInicialForm = fechaInicial.ToString("yyyy-MM-dd");
        string fechaFinalForm = fechaFinal.ToString("yyyy-MM-dd");

        DataSet dsResultado = new DataSet("ConsultaPSECFDI");

        dsResultado = clsComun.fnObtenerConsultaComprobantesPSECFDI(fechaInicialForm, fechaFinalForm);

        string linea = "";

        foreach (DataTable table in dsResultado.Tables)
        {
            foreach (DataRow row in table.Rows)
            {
                linea += "|";
                foreach (DataColumn column in table.Columns)
                {
                    string columna = row[column].ToString();
                    linea += columna + "|";

                }
                //linea = linea.TrimEnd('|');;
                linea += "\r\n";
            }
        }

        MemoryStream memoryStream = new MemoryStream();
        TextWriter textWriter = new StreamWriter(memoryStream);
        textWriter.WriteLine(linea);
        textWriter.Flush(); // added this line
        byte[] bytesInStream = memoryStream.ToArray(); // simpler way of converting to array
        memoryStream.Close();

        string anio = fechaInicial.Year.ToString();
        string mes = fechaInicial.Month.ToString("d2");

        string anio2 = fechaFinal.Year.ToString();
        string mes2 = fechaFinal.Month.ToString("d2");

        Response.Clear();
        Response.ContentType = "text/plain";
        Response.AddHeader("content-disposition", "attachment;    filename=PSECFDI-" + anio + "" + mes + " " + anio2 + "" + mes2 + ".txt");
        Response.BinaryWrite(bytesInStream);
        Response.End();

        //myMemoryStream.Dispose();
    }
}