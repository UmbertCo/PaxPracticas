using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Xml;
using System.Text;
using System.Data;

public partial class Timbrado_Addendas_webAddendaFIDEAPECH : System.Web.UI.Page
{
    DataTable dtCptosCto1 = new DataTable();
    DataTable dtCptosCto2 = new DataTable();
    DataTable dtCptosCto3 = new DataTable();
    DataTable dtCptosCto4 = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    { 
        if (!(Page.IsPostBack))
        {
            fnLimpiarCampos();
            fnLimpiarCamposCto1();
            fnLimpiarCamposCto2();
            fnLimpiarCamposCto3();
            fnLimpiarCamposCto4();

            txtImporte1.Attributes.Add("onchange", "return calcularCantidadPago();");
            txtImporte2.Attributes.Add("onchange", "return calcularCantidadPago();");
            txtImporte3.Attributes.Add("onchange", "return calcularCantidadPago();");
            txtImporte4.Attributes.Add("onchange", "return calcularCantidadPago();");
            txtSalCapCre1.Attributes.Add("onchange", "return calcularSaldoCredito1()");
            txtCap1Cpto1.Attributes.Add("onchange", "return calcularCapitalCto1()");
            txtNor1Cpto1.Attributes.Add("onchange", "return calcularCapitalCto1()");
            txtMor1Cpto1.Attributes.Add("onchange", "return calcularCapitalCto1()");
            txtSalCapCre2.Attributes.Add("onchange", "return calcularSaldoCredito2()");
            txtCap1Cpto2.Attributes.Add("onchange", "return calcularCapitalCto2()");
            txtNor1Cpto2.Attributes.Add("onchange", "return calcularCapitalCto2()");
            txtMor1Cpto2.Attributes.Add("onchange", "return calcularCapitalCto2()");
            txtSalCapCre3.Attributes.Add("onchange", "return calcularSaldoCredito3()");
            txtCap1Cpto3.Attributes.Add("onchange", "return calcularCapitalCto3()");
            txtNor1Cpto3.Attributes.Add("onchange", "return calcularCapitalCto3()");
            txtMor1Cpto3.Attributes.Add("onchange", "return calcularCapitalCto3()");
            txtSalCapCre4.Attributes.Add("onchange", "return calcularSaldoCredito4()");
            txtCap1Cpto4.Attributes.Add("onchange", "return calcularCapitalCto4()");
            txtNor1Cpto4.Attributes.Add("onchange", "return calcularCapitalCto4()");
            txtMor1Cpto4.Attributes.Add("onchange", "return calcularCapitalCto4()");

            //Abre panel CollapsiblePanelExtender Contrato 1 
            txtNoContrato1.Attributes.Add("onkeypress", "validaEnter()");
            txtImporte1.Attributes.Add("onkeypress", "validaEnter()");
            txtSalCapCre1.Attributes.Add("onkeypress", "validaEnter()");
            txtSalCap1.Attributes.Add("onkeypress", "validaEnter()");
            txtFec1Cpto1.Attributes.Add("onkeypress", "validaEnter()");
            txtDia1Cpto1.Attributes.Add("onkeypress", "validaEnter()");
            txtCap1Cpto1.Attributes.Add("onkeypress", "validaEnter()");
            txtNor1Cpto1.Attributes.Add("onkeypress", "validaEnter()");
            txtTas1_1Cpto1.Attributes.Add("onkeypress", "validaEnter()");
            txtMor1Cpto1.Attributes.Add("onkeypress", "validaEnter()");
            txtTas2_1Cpto1.Attributes.Add("onkeypress", "validaEnter()");
            txtSubTot1Cpto1.Attributes.Add("onkeypress", "validaEnter()");

            //Solo lectura
            txtSalCap1.Attributes.Add("readonly", "readonly");
            txtSubTot1Cpto1.Attributes.Add("readonly", "readonly");
            txtSalCap2.Attributes.Add("readonly", "readonly");
            txtSubTot1Cpto2.Attributes.Add("readonly", "readonly");
            txtSalCap3.Attributes.Add("readonly", "readonly");
            txtSubTot1Cpto3.Attributes.Add("readonly", "readonly");
            txtSalCap4.Attributes.Add("readonly", "readonly");
            txtSubTot1Cpto4.Attributes.Add("readonly", "readonly");
            txtTotSubTot.Attributes.Add("readonly", "readonly");

            ////Ocultar txt temporales para posicionar tabla
            txtTemp0.Attributes.Add("style", "visibility:hidden");
            txtTemp1.Attributes.Add("style", "visibility:hidden");
            txtTemp2.Attributes.Add("style", "visibility:hidden");
            txtTemp3.Attributes.Add("style", "visibility:hidden");
            txtTemp4.Attributes.Add("style", "visibility:hidden");

            //Crea tabla para Conceptos del Contrato
            //*********************************************
            DataTable dtConceptoCto1 = new DataTable();
            DataTable dtConceptoCto2 = new DataTable();
            DataTable dtConceptoCto3 = new DataTable();
            DataTable dtConceptoCto4 = new DataTable();
            DataColumn[] keysCpto1 = new DataColumn[1];
            DataColumn[] keysCpto2 = new DataColumn[1];
            DataColumn[] keysCpto3 = new DataColumn[1];
            DataColumn[] keysCpto4 = new DataColumn[1];

            //Crear y agregar columnas
            DataColumn colCpto1 = new DataColumn();
            colCpto1.DataType = System.Type.GetType("System.Int32");
            colCpto1.ColumnName = "id_Concepto";
            colCpto1.AutoIncrement = true;
            colCpto1.AutoIncrementSeed = 1;
            colCpto1.AutoIncrementStep = 1;
            dtConceptoCto1.Columns.Add(colCpto1);

            keysCpto1[0] = colCpto1;
            dtConceptoCto1.PrimaryKey = keysCpto1;

            dtConceptoCto1.Columns.Add("Fecha");
            dtConceptoCto1.Columns.Add("Dias");
            dtConceptoCto1.Columns.Add("Capital", typeof(double));
            dtConceptoCto1.Columns.Add("IntNorm", typeof(double));
            dtConceptoCto1.Columns.Add("TasaUno", typeof(double));
            dtConceptoCto1.Columns.Add("IntMorat", typeof(double));
            dtConceptoCto1.Columns.Add("TasaDos", typeof(double));
            dtConceptoCto1.Columns.Add("SubTotal", typeof(double));

            ViewState["dtConceptoCto1"] = dtConceptoCto1;
            ViewState["id_Concepto1"] = string.Empty;

            DataColumn colCpto2 = new DataColumn();
            colCpto2.DataType = System.Type.GetType("System.Int32");
            colCpto2.ColumnName = "id_Concepto";
            colCpto2.AutoIncrement = true;
            colCpto2.AutoIncrementSeed = 1;
            colCpto2.AutoIncrementStep = 1;
            dtConceptoCto2.Columns.Add(colCpto2);

            keysCpto2[0] = colCpto2;
            dtConceptoCto2.PrimaryKey = keysCpto2;

            dtConceptoCto2.Columns.Add("Fecha");
            dtConceptoCto2.Columns.Add("Dias");
            dtConceptoCto2.Columns.Add("Capital", typeof(double));
            dtConceptoCto2.Columns.Add("IntNorm", typeof(double));
            dtConceptoCto2.Columns.Add("TasaUno", typeof(double));
            dtConceptoCto2.Columns.Add("IntMorat", typeof(double));
            dtConceptoCto2.Columns.Add("TasaDos", typeof(double));
            dtConceptoCto2.Columns.Add("SubTotal", typeof(double));

            ViewState["dtConceptoCto2"] = dtConceptoCto2;
            ViewState["id_Concepto2"] = string.Empty;

            DataColumn colCpto3 = new DataColumn();
            colCpto3.DataType = System.Type.GetType("System.Int32");
            colCpto3.ColumnName = "id_Concepto";
            colCpto3.AutoIncrement = true;
            colCpto3.AutoIncrementSeed = 1;
            colCpto3.AutoIncrementStep = 1;
            dtConceptoCto3.Columns.Add(colCpto3);

            keysCpto3[0] = colCpto3;
            dtConceptoCto3.PrimaryKey = keysCpto3;

            dtConceptoCto3.Columns.Add("Fecha");
            dtConceptoCto3.Columns.Add("Dias");
            dtConceptoCto3.Columns.Add("Capital", typeof(double));
            dtConceptoCto3.Columns.Add("IntNorm", typeof(double));
            dtConceptoCto3.Columns.Add("TasaUno", typeof(double));
            dtConceptoCto3.Columns.Add("IntMorat", typeof(double));
            dtConceptoCto3.Columns.Add("TasaDos", typeof(double));
            dtConceptoCto3.Columns.Add("SubTotal", typeof(double));

            ViewState["dtConceptoCto3"] = dtConceptoCto3;
            ViewState["id_Concepto3"] = string.Empty;

            DataColumn colCpto4 = new DataColumn();
            colCpto4.DataType = System.Type.GetType("System.Int32");
            colCpto4.ColumnName = "id_Concepto";
            colCpto4.AutoIncrement = true;
            colCpto4.AutoIncrementSeed = 1;
            colCpto4.AutoIncrementStep = 1;
            dtConceptoCto4.Columns.Add(colCpto4);

            keysCpto4[0] = colCpto4;
            dtConceptoCto4.PrimaryKey = keysCpto4;

            dtConceptoCto4.Columns.Add("Fecha");
            dtConceptoCto4.Columns.Add("Dias");
            dtConceptoCto4.Columns.Add("Capital", typeof(double));
            dtConceptoCto4.Columns.Add("IntNorm", typeof(double));
            dtConceptoCto4.Columns.Add("TasaUno", typeof(double));
            dtConceptoCto4.Columns.Add("IntMorat", typeof(double));
            dtConceptoCto4.Columns.Add("TasaDos", typeof(double));
            dtConceptoCto4.Columns.Add("SubTotal", typeof(double));

            ViewState["dtConceptoCto4"] = dtConceptoCto4;
            ViewState["id_Concepto4"] = string.Empty;
            //*********************************************

            //Cerrar paneles
            cpeContrato2.Collapsed = true;
            cpeContrato3.Collapsed = true;
            cpeContrato4.Collapsed = true;

            btnCerrar.Attributes.Add("onclick", "window.close();");
        }
    }

    private void fnLimpiarCampos()
    {
        txtCanPag.Text = "0";
        txtImporte1.Text = "0";
        txtImporte2.Text = "0";
        txtSalCapCre1.Text = "0";
        txtSalCapCre2.Text = "0";
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
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            //string sTotal = string.Format("{0:n2}", Convert.ToDouble(Session["lblTotalFid"].ToString()));
            string sTotal = string.Format("{0:n2}", Convert.ToDouble((Request.QueryString["totFact"]!= null)?Request.QueryString["totFact"].ToString():"0"));

            //Verifica si los totales son igual al total de la factura
            if (sTotal == txtTotSubTot.Text)
            {
                //Valida que se encuentre datos almenos del contrato uno
                dtCptosCto1 = (DataTable)ViewState["dtConceptoCto1"];
                if (dtCptosCto1.Rows.Count > 0)
                {
                    XmlDocument xmladenda = new XmlDocument();
                    string sArmXML = ArmarXML();
                    if (!(string.IsNullOrEmpty(sArmXML)))
                    {
                        xmladenda.LoadXml(sArmXML);
                        Session["AddendaGenerada"] += xmladenda.OuterXml;
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                        btnGuardar.Enabled = false;
                    }
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, "Capture detalle del contrato uno");
                }
            }
            else
            {
                clsComun.fnMostrarMensaje(this, "El SubTotal " + string.Format("{0:c2}", Convert.ToDouble(txtTotSubTot.Text)) + " es diferente al Total " + string.Format("{0:c2}", Convert.ToDouble(sTotal)) + " de la factura");
                //clsComun.fnMostrarMensaje(this, "El Sub-Total de la sección de 'Totales' no es igual al Total de la factura");
            }
        }
        catch (Exception ex)
        {
            clsComun.fnMostrarMensaje(this, ex.Message);
        }
    }

    public string ArmarXML()
    {
        StringBuilder sb = new StringBuilder();
        string sImporte = string.Format("{0:n2}", txtImporte1.Text).Replace(",", "");
        string sSalCapCre =  string.Format("{0:n2}", txtSalCapCre1.Text).Replace(",", "");
        string sSalCap =  string.Format("{0:n2}", txtSalCap1.Text).Replace(",", "");

        sb.Append("<Documento>");
        sb.Append("<FacturaFIDEAPECH>");
        sb.Append("<noContratoUno noContrato=\"" + txtNoContrato1.Text + "\" Importe=\"" + sImporte + "\" SaldoCptalCredito=\"" + sSalCapCre + "\" SaldoCptal=\"" + sSalCap + "\">");
        dtCptosCto1 = (DataTable)ViewState["dtConceptoCto1"];
        foreach (DataRow dr1 in dtCptosCto1.Rows)
        {
            string sCapital = string.Format("{0:n2}", Convert.ToDouble(dr1["Capital"].ToString())).Replace(",","");
            string sIntNorm = string.Format("{0:n2}", Convert.ToDouble(dr1["IntNorm"].ToString())).Replace(",", "");
            string sTasaUno = string.Format("{0:n2}", Convert.ToDouble(dr1["TasaUno"].ToString())).Replace(",", "");
            string sIntMorat = string.Format("{0:n2}", Convert.ToDouble(dr1["IntMorat"].ToString())).Replace(",", "");
            string sTasaDos = string.Format("{0:n2}", Convert.ToDouble(dr1["TasaDos"].ToString())).Replace(",", "");
            string sSubTotal = string.Format("{0:n2}", Convert.ToDouble(dr1["Subtotal"].ToString())).Replace(",", "");

            sb.Append("<Concepto Fecha=\"" + dr1["Fecha"].ToString() +
                        "\" Dias=\"" + dr1["Dias"].ToString() + "\" Capital=\"" + sCapital + "\" IntNorm=\"" + sIntNorm +
                        "\" TasaUno =\"" + sTasaUno  + "\" IntMorat=\"" + sIntMorat  + "\" TasaDos=\"" + sTasaDos
                        + "\" SubTotal=\"" + sSubTotal + "\" />");
        }
        sb.Append("</noContratoUno>");
        //Contrato 2
        dtCptosCto2 = (DataTable)ViewState["dtConceptoCto2"];
        if (dtCptosCto2.Rows.Count > 0)
        {
            if (!(string.IsNullOrEmpty(txtNoContrato2.Text)))
            {
                sImporte =  string.Format("{0:n2}", txtImporte2.Text).Replace(",", "");
                sSalCapCre =  string.Format("{0:n2}", txtSalCapCre2.Text).Replace(",", "");
                sSalCap =  string.Format("{0:n2}", txtSalCap2.Text).Replace(",", "");

                sb.Append("<noContratoDos noContrato=\"" + txtNoContrato2.Text + "\" Importe=\"" + sImporte + "\" SaldoCptalCredito=\"" + sSalCapCre + "\" SaldoCptal=\"" + sSalCap + "\">");
                foreach (DataRow dr1 in dtCptosCto2.Rows)
                {
                    string sCapital = string.Format("{0:n2}", Convert.ToDouble(dr1["Capital"].ToString())).Replace(",", "");
                    string sIntNorm = string.Format("{0:n2}", Convert.ToDouble(dr1["IntNorm"].ToString())).Replace(",", "");
                    string sTasaUno = string.Format("{0:n2}", Convert.ToDouble(dr1["TasaUno"].ToString())).Replace(",", "");
                    string sIntMorat = string.Format("{0:n2}", Convert.ToDouble(dr1["IntMorat"].ToString())).Replace(",", "");
                    string sTasaDos = string.Format("{0:n2}", Convert.ToDouble(dr1["TasaDos"].ToString())).Replace(",", "");
                    string sSubTotal = string.Format("{0:n2}", Convert.ToDouble(dr1["Subtotal"].ToString())).Replace(",", "");

                    sb.Append("<Concepto Fecha=\"" + dr1["Fecha"].ToString() +
                                "\" Dias=\"" + dr1["Dias"].ToString() + "\" Capital=\"" + sCapital + "\" IntNorm=\"" + sIntNorm +
                                "\" TasaUno =\"" + sTasaUno + "\" IntMorat=\"" + sIntMorat + "\" TasaDos=\"" + sTasaDos
                                + "\" SubTotal=\"" + sSubTotal + "\" />");
                }
                sb.Append("</noContratoDos>");
            }
            else
            {
                clsComun.fnMostrarMensaje(this, "Capture No Contrato 2");
                return string.Empty;
            }
        }
        //Contrato 3
        dtCptosCto3 = (DataTable)ViewState["dtConceptoCto3"];
        if (dtCptosCto3.Rows.Count > 0)
        {
            if (!(string.IsNullOrEmpty(txtNoContrato3.Text)))
            {
                sImporte =  string.Format("{0:n2}", txtImporte3.Text).Replace(",", "");
                sSalCapCre =  string.Format("{0:n2}", txtSalCapCre3.Text).Replace(",", "");
                sSalCap =  string.Format("{0:n2}", txtSalCap3.Text).Replace(",", "");

                sb.Append("<noContratoTres noContrato=\"" + txtNoContrato3.Text + "\" Importe=\"" + sImporte + "\" SaldoCptalCredito=\"" + sSalCapCre + "\" SaldoCptal=\"" + sSalCap + "\">");
                foreach (DataRow dr1 in dtCptosCto3.Rows)
                {
                    string sCapital = string.Format("{0:n2}", Convert.ToDouble(dr1["Capital"].ToString())).Replace(",", "");
                    string sIntNorm = string.Format("{0:n2}", Convert.ToDouble(dr1["IntNorm"].ToString())).Replace(",", "");
                    string sTasaUno = string.Format("{0:n2}", Convert.ToDouble(dr1["TasaUno"].ToString())).Replace(",", "");
                    string sIntMorat = string.Format("{0:n2}", Convert.ToDouble(dr1["IntMorat"].ToString())).Replace(",", "");
                    string sTasaDos = string.Format("{0:n2}", Convert.ToDouble(dr1["TasaDos"].ToString())).Replace(",", "");
                    string sSubTotal = string.Format("{0:n2}", Convert.ToDouble(dr1["Subtotal"].ToString())).Replace(",", "");

                    sb.Append("<Concepto Fecha=\"" + dr1["Fecha"].ToString() +
                                "\" Dias=\"" + dr1["Dias"].ToString() + "\" Capital=\"" + sCapital+ "\" IntNorm=\"" + sIntNorm +
                                "\" TasaUno =\"" + sTasaUno + "\" IntMorat=\"" + sIntMorat + "\" TasaDos=\"" + sTasaDos
                                + "\" SubTotal=\"" + sSubTotal + "\" />");
                }
                sb.Append("</noContratoTres>");
            }
            else
            {
                clsComun.fnMostrarMensaje(this, "Capture No Contrato 3");
                return string.Empty;
            }
        }
        //Contrato 4
        dtCptosCto4 = (DataTable)ViewState["dtConceptoCto4"];
        if (dtCptosCto4.Rows.Count > 0)
        {
            if (!(string.IsNullOrEmpty(txtNoContrato4.Text)))
            {
                sImporte =  string.Format("{0:n2}", txtImporte4.Text).Replace(",", "");
                sSalCapCre =  string.Format("{0:n2}", txtSalCapCre4.Text).Replace(",", "");
                sSalCap =  string.Format("{0:n2}", txtSalCap4.Text).Replace(",", "");

                sb.Append("<noContratoCuatro noContrato=\"" + txtNoContrato4.Text + "\" Importe=\"" + sImporte + "\" SaldoCptalCredito=\"" + sSalCapCre + "\" SaldoCptal=\"" + sSalCap + "\">");
                foreach (DataRow dr1 in dtCptosCto4.Rows)
                {
                    string sCapital = string.Format("{0:n2}", Convert.ToDouble(dr1["Capital"].ToString())).Replace(",", "");
                    string sIntNorm = string.Format("{0:n2}", Convert.ToDouble(dr1["IntNorm"].ToString())).Replace(",", "");
                    string sTasaUno = string.Format("{0:n2}", Convert.ToDouble(dr1["TasaUno"].ToString())).Replace(",", "");
                    string sIntMorat = string.Format("{0:n2}", Convert.ToDouble(dr1["IntMorat"].ToString())).Replace(",", "");
                    string sTasaDos = string.Format("{0:n2}", Convert.ToDouble(dr1["TasaDos"].ToString())).Replace(",", "");
                    string sSubTotal = string.Format("{0:n2}", Convert.ToDouble(dr1["Subtotal"].ToString())).Replace(",", "");

                    sb.Append("<Concepto Fecha=\"" + dr1["Fecha"].ToString() +
                                "\" Dias=\"" + dr1["Dias"].ToString() + "\" Capital=\"" + sCapital + "\" IntNorm=\"" + sIntNorm +
                                "\" TasaUno =\"" + sTasaUno + "\" IntMorat=\"" + sIntMorat + "\" TasaDos=\"" + sTasaDos
                                + "\" SubTotal=\"" + sSubTotal + "\" />");
                }
                sb.Append("</noContratoCuatro>");
            }
            else
            {
                clsComun.fnMostrarMensaje(this, "Capture No Contrato 4");
                return string.Empty;
            }
        }
        sb.Append("<Detalles>");        
        if (txtPrograma.Text != string.Empty && txtObservaciones.Text == string.Empty)
            sb.Append("<Datos Programa=\"" + txtPrograma.Text + "\" />");
        if (txtObservaciones.Text != string.Empty && txtPrograma.Text == string.Empty)
            sb.Append("<Datos Observaciones=\"" + txtObservaciones.Text + "\" />");
        if (txtObservaciones.Text != string.Empty && txtPrograma.Text != string.Empty)
            sb.Append("<Datos Programa=\"" + txtPrograma.Text + "\" Observaciones=\"" + txtObservaciones.Text + "\" />");
        string sCanPag = txtCanPag.Text.Replace(",", "");
        string sTotCap = txtTotCap.Text.Replace(",", "");
        string sTotNor = txtTotNor.Text.Replace(",", "");
        string sTotSubTot = txtTotSubTot.Text.Replace(",", "");
        sb.Append("<Total CantidadPago=\"" + sCanPag  + "\" TotalCapital=\"" + sTotCap + "\" TotalNorm=\"" + sTotNor  + "\" TotalMorat=\"" + txtTotMor.Text + "\" TotalSubTot=\"" + sTotSubTot + "\" />");
        sb.Append("</Detalles>");
        sb.Append("</FacturaFIDEAPECH>");
        sb.Append("</Documento>");

        return sb.ToString();
    }

    protected void btnAgrCpto1_Click(object sender, EventArgs e)
    {
        //Si fecha contiene valor 
        if (!(string.IsNullOrEmpty(txtFec1Cpto1.Text)))
        {
            string sImporte = string.Format("{0:n2}", Convert.ToDouble(txtImporte1.Text));
            string sSubTotCpto = string.Format("{0:n2}", Convert.ToDouble(txtSubTot1Cpto1.Text));

            //Si el importe y subtotal son iguales se guarda en tabla
            if (sImporte == sSubTotCpto)
            {
                //Se inserta en tabla el concepto capturado
                dtCptosCto1 = (DataTable)ViewState["dtConceptoCto1"];
                if (dtCptosCto1.Rows.Count < 4)
                {
                    //Se inserta el registro del concepto 
                    DataRow row = dtCptosCto1.NewRow();
                    row["Fecha"] = txtFec1Cpto1.Text;
                    row["Dias"] = txtDia1Cpto1.Text;
                    row["Capital"] = txtCap1Cpto1.Text;
                    row["IntNorm"] = txtNor1Cpto1.Text;
                    row["TasaUno"] = txtTas1_1Cpto1.Text;
                    row["IntMorat"] = txtMor1Cpto1.Text;
                    row["TasaDos"] = txtTas2_1Cpto1.Text;
                    row["SubTotal"] = txtSubTot1Cpto1.Text;
                    dtCptosCto1.Rows.Add(row);

                    //Se llena grid de conceptos
                    gvConceptosCto1.DataSource = dtCptosCto1;
                    gvConceptosCto1.DataBind();

                    fnCalculaTotales();

                }
                else
                    clsComun.fnMostrarMensaje(this, "Solo se permite agregar como máximo 4 conceptos por contrato");

                fnLimpiarCamposCto1();
            }
            else
                clsComun.fnMostrarMensaje(this, "El importe y subtotal del contrato 1 no son iguales");
        }
    }

    private void fnLimpiarCamposCto1()
    {
        txtFec1Cpto1.Text = string.Empty;
        txtDia1Cpto1.Text = string.Empty;
        txtCap1Cpto1.Text = "0";
        txtNor1Cpto1.Text = "0";
        txtTas1_1Cpto1.Text = "0";
        txtMor1Cpto1.Text = "0";
        txtTas2_1Cpto1.Text = "0";
        txtSubTot1Cpto1.Text = "0";
    }
    protected void gvConceptosCto1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        dtCptosCto1 = (DataTable)ViewState["dtConceptoCto1"];
        //Se elimina el registro seleccionado según id_impuesto
        DataRow rowDel = dtCptosCto1.Rows.Find(e.Keys["id_Concepto"].ToString());
        if (rowDel != null)
            rowDel.Delete();

        ViewState["dtConceptoCto1"] = dtCptosCto1;

        gvConceptosCto1.DataSource = dtCptosCto1;
        gvConceptosCto1.DataBind();

        fnCalculaTotales();
    }
    protected void btnAgrCpto2_Click(object sender, EventArgs e)
    {
        //Si fecha contiene valor 
        if (!(string.IsNullOrEmpty(txtFec1Cpto2.Text)))
        {
            string sImporte = string.Format("{0:n2}", Convert.ToDouble(txtImporte2.Text));
            string sSubTotCpto = string.Format("{0:n2}", Convert.ToDouble(txtSubTot1Cpto2.Text));

            //Si el importe y subtotal son iguales se guarda en tabla
            if (sImporte == sSubTotCpto)
            {
                //Se inserta en tabla el concepto capturado
                dtCptosCto2 = (DataTable)ViewState["dtConceptoCto2"];

                if (dtCptosCto2.Rows.Count < 4)
                {
                    //Se inserta el registro del concepto 
                    DataRow row = dtCptosCto2.NewRow();
                    row["Fecha"] = txtFec1Cpto2.Text;
                    row["Dias"] = txtDia1Cpto2.Text;
                    row["Capital"] = txtCap1Cpto2.Text;
                    row["IntNorm"] = txtNor1Cpto2.Text;
                    row["TasaUno"] = txtTas1_1Cpto2.Text;
                    row["IntMorat"] = txtMor1Cpto2.Text;
                    row["TasaDos"] = txtTas2_1Cpto2.Text;
                    row["SubTotal"] = txtSubTot1Cpto2.Text;
                    dtCptosCto2.Rows.Add(row);

                    //Se llena grid de conceptos
                    gvConceptosCto2.DataSource = dtCptosCto2;
                    gvConceptosCto2.DataBind();

                    fnCalculaTotales();
                }
                else
                    clsComun.fnMostrarMensaje(this, "Solo se permite agregar como máximo 4 conceptos por contrato");

                fnLimpiarCamposCto2();
            }
            else
                clsComun.fnMostrarMensaje(this, "El importe y subtotal del contrato 2 no son iguales");
        }
    }

    private void fnLimpiarCamposCto2()
    {
        txtFec1Cpto2.Text = string.Empty;
        txtDia1Cpto2.Text = string.Empty;
        txtCap1Cpto2.Text = "0";
        txtNor1Cpto2.Text = "0";
        txtTas1_1Cpto2.Text = "0";
        txtMor1Cpto2.Text = "0";
        txtTas2_1Cpto2.Text = "0";
        txtSubTot1Cpto2.Text = "0";
    }

    protected void gvConceptosCto2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        dtCptosCto2 = (DataTable)ViewState["dtConceptoCto2"];
        //Se elimina el registro seleccionado según id_impuesto
        DataRow rowDel = dtCptosCto2.Rows.Find(e.Keys["id_Concepto"].ToString());
        if (rowDel != null)
            rowDel.Delete();

        ViewState["dtConceptoCto2"] = dtCptosCto2;

        gvConceptosCto2.DataSource = dtCptosCto2;
        gvConceptosCto2.DataBind();

        fnCalculaTotales();
    }
    protected void btnAgrCpto3_Click(object sender, EventArgs e)
    {
        //Si fecha contiene valor 
        if (!(string.IsNullOrEmpty(txtFec1Cpto3.Text)))
        {
            string sImporte = string.Format("{0:n2}", Convert.ToDouble(txtImporte3.Text));
            string sSubTotCpto = string.Format("{0:n2}", Convert.ToDouble(txtSubTot1Cpto3.Text));

            //Si el importe y subtotal son iguales se guarda en tabla
            if (sImporte == sSubTotCpto)
            {
                //Se inserta en tabla el concepto capturado
                dtCptosCto3 = (DataTable)ViewState["dtConceptoCto3"];

                if (dtCptosCto3.Rows.Count < 4)
                {
                    //Se inserta el registro del concepto 
                    DataRow row = dtCptosCto3.NewRow();
                    row["Fecha"] = txtFec1Cpto3.Text;
                    row["Dias"] = txtDia1Cpto3.Text;
                    row["Capital"] = txtCap1Cpto3.Text;
                    row["IntNorm"] = txtNor1Cpto3.Text;
                    row["TasaUno"] = txtTas1_1Cpto3.Text;
                    row["IntMorat"] = txtMor1Cpto3.Text;
                    row["TasaDos"] = txtTas2_1Cpto3.Text;
                    row["SubTotal"] = txtSubTot1Cpto3.Text;
                    dtCptosCto3.Rows.Add(row);

                    //Se llena grid de conceptos
                    gvConceptosCto3.DataSource = dtCptosCto3;
                    gvConceptosCto3.DataBind();

                    fnCalculaTotales();
                }
                else
                    clsComun.fnMostrarMensaje(this, "Solo se permite agregar como máximo 4 conceptos por contrato");

                fnLimpiarCamposCto3();
            }
            else
                clsComun.fnMostrarMensaje(this, "El importe y subtotal del contrato 3 no son iguales");
        }
    }

    private void fnLimpiarCamposCto3()
    {
        txtFec1Cpto3.Text = string.Empty;
        txtDia1Cpto3.Text = string.Empty;
        txtCap1Cpto3.Text = "0";
        txtNor1Cpto3.Text = "0";
        txtTas1_1Cpto3.Text = "0";
        txtMor1Cpto3.Text = "0";
        txtTas2_1Cpto3.Text = "0";
        txtSubTot1Cpto3.Text = "0";
    }

    protected void gvConceptosCto3_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        dtCptosCto3 = (DataTable)ViewState["dtConceptoCto3"];
        //Se elimina el registro seleccionado según id_impuesto
        DataRow rowDel = dtCptosCto3.Rows.Find(e.Keys["id_Concepto"].ToString());
        if (rowDel != null)
            rowDel.Delete();

        ViewState["dtConceptoCto3"] = dtCptosCto3;

        gvConceptosCto3.DataSource = dtCptosCto3;
        gvConceptosCto3.DataBind();

        fnCalculaTotales();
    }
    protected void btnAgrCpto4_Click(object sender, EventArgs e)
    {
        //Si fecha contiene valor 
        if (!(string.IsNullOrEmpty(txtFec1Cpto4.Text)))
        {
            string sImporte = string.Format("{0:n2}", Convert.ToDouble(txtImporte4.Text));
            string sSubTotCpto = string.Format("{0:n2}", Convert.ToDouble(txtSubTot1Cpto4.Text));
            //Si el importe y subtotal son iguales se guarda en tabla
            if (sImporte == sSubTotCpto)
            {
                //Se inserta en tabla el concepto capturado
                dtCptosCto4 = (DataTable)ViewState["dtConceptoCto4"];

                if (dtCptosCto4.Rows.Count < 4)
                {
                    //Se inserta el registro del concepto 
                    DataRow row = dtCptosCto4.NewRow();
                    row["Fecha"] = txtFec1Cpto4.Text;
                    row["Dias"] = txtDia1Cpto4.Text;
                    row["Capital"] = txtCap1Cpto4.Text;
                    row["IntNorm"] = txtNor1Cpto4.Text;
                    row["TasaUno"] = txtTas1_1Cpto4.Text;
                    row["IntMorat"] = txtMor1Cpto4.Text;
                    row["TasaDos"] = txtTas2_1Cpto4.Text;
                    row["SubTotal"] = txtSubTot1Cpto4.Text;
                    dtCptosCto4.Rows.Add(row);

                    //Se llena grid de conceptos
                    gvConceptosCto4.DataSource = dtCptosCto4;
                    gvConceptosCto4.DataBind();

                    fnCalculaTotales();
                }
                else
                    clsComun.fnMostrarMensaje(this, "Solo se permite agregar como máximo 4 conceptos por contrato");

                fnLimpiarCamposCto4();
            }
            else
                clsComun.fnMostrarMensaje(this, "El importe y subtotal del contrato 4 no son iguales");
        }
    }

    private void fnLimpiarCamposCto4()
    {
        txtFec1Cpto4.Text = string.Empty;
        txtDia1Cpto4.Text = string.Empty;
        txtCap1Cpto4.Text = "0";
        txtNor1Cpto4.Text = "0";
        txtTas1_1Cpto4.Text = "0";
        txtMor1Cpto4.Text = "0";
        txtTas2_1Cpto4.Text = "0";
        txtSubTot1Cpto4.Text = "0";
    }

    protected void gvConceptosCto4_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        dtCptosCto4 = (DataTable)ViewState["dtConceptoCto4"];
        //Se elimina el registro seleccionado según id_impuesto
        DataRow rowDel = dtCptosCto4.Rows.Find(e.Keys["id_Concepto"].ToString());
        if (rowDel != null)
            rowDel.Delete();

        ViewState["dtConceptoCto4"] = dtCptosCto4;

        gvConceptosCto4.DataSource = dtCptosCto4;
        gvConceptosCto4.DataBind();

        fnCalculaTotales();
    }

    /// <summary>
    /// Calcula los totales de importes capturados en conceptos
    /// </summary>
    private void fnCalculaTotales()
    {
        //Se obtiene datatables correspondientes a los contratos
        dtCptosCto1 = (DataTable)ViewState["dtConceptoCto1"];
        dtCptosCto2 = (DataTable)ViewState["dtConceptoCto2"];
        dtCptosCto3 = (DataTable)ViewState["dtConceptoCto3"];
        dtCptosCto4 = (DataTable)ViewState["dtConceptoCto4"];

        double dTotCapital, dTotNorm, dTotMor, dTotSubTot, dCanPag, dTotCap1, dTotCap2, dTotCap3, dTotCap4;
        dTotCapital = dTotNorm = dTotMor = dTotSubTot = dCanPag = dTotCap1 = dTotCap2 = dTotCap3 = dTotCap4 = 0;
        //Calcula totales contrato uno
        foreach (DataRow dr1 in dtCptosCto1.Rows)
        {
            dTotCapital += Convert.ToDouble(dr1["Capital"].ToString());
            dTotCap1 += Convert.ToDouble(dr1["Capital"].ToString());
            dTotNorm += Convert.ToDouble(dr1["IntNorm"].ToString());
            dTotMor += Convert.ToDouble(dr1["IntMorat"].ToString());
            dTotSubTot += Convert.ToDouble(dr1["SubTotal"].ToString());
        }
        //Calcula saldo capital del contrato uno
        if (string.IsNullOrEmpty(txtSalCapCre1.Text))
            txtSalCapCre1.Text = "0";
        double dSalCap1 = Convert.ToDouble(txtSalCapCre1.Text) - dTotCap1;
        txtSalCap1.Text = string.Format("{0:n2}", dSalCap1);

        //Calcula totales contrato dos
        foreach (DataRow dr1 in dtCptosCto2.Rows)
        {
            dTotCapital += Convert.ToDouble(dr1["Capital"].ToString());
            dTotCap2 += Convert.ToDouble(dr1["Capital"].ToString());
            dTotNorm += Convert.ToDouble(dr1["IntNorm"].ToString());
            dTotMor += Convert.ToDouble(dr1["IntMorat"].ToString());
            dTotSubTot += Convert.ToDouble(dr1["SubTotal"].ToString());
        }
        //Calcula saldo capital del contrato dos
        if (string.IsNullOrEmpty(txtSalCapCre2.Text))
            txtSalCapCre2.Text = "0";
        double dSalCap2 = Convert.ToDouble(txtSalCapCre2.Text) - dTotCap2;
        txtSalCap2.Text = string.Format("{0:n2}", dSalCap2);

        //Calcula totales contrato tres
        foreach (DataRow dr1 in dtCptosCto3.Rows)
        {
            dTotCapital += Convert.ToDouble(dr1["Capital"].ToString());
            dTotCap3 += Convert.ToDouble(dr1["Capital"].ToString());
            dTotNorm += Convert.ToDouble(dr1["IntNorm"].ToString());
            dTotMor += Convert.ToDouble(dr1["IntMorat"].ToString());
            dTotSubTot += Convert.ToDouble(dr1["SubTotal"].ToString());
        }
        //Calcula saldo capital del contrato cuatro
        if (string.IsNullOrEmpty(txtSalCapCre3.Text))
            txtSalCapCre3.Text = "0";
        double dSalCap3 = Convert.ToDouble(txtSalCapCre3.Text) - dTotCap3;
        txtSalCap3.Text = string.Format("{0:n2}", dSalCap3);

        //Calcula totales contrato cuatro
        foreach (DataRow dr1 in dtCptosCto4.Rows)
        {
            dTotCapital += Convert.ToDouble(dr1["Capital"].ToString());
            dTotCap4 += Convert.ToDouble(dr1["Capital"].ToString());
            dTotNorm += Convert.ToDouble(dr1["IntNorm"].ToString());
            dTotMor += Convert.ToDouble(dr1["IntMorat"].ToString());
            dTotSubTot += Convert.ToDouble(dr1["SubTotal"].ToString());
        }
        //Calcula saldo capital del contrato uno
        if (string.IsNullOrEmpty(txtSalCapCre4.Text))
            txtSalCapCre4.Text = "0";
        double dSalCap4 = Convert.ToDouble(txtSalCapCre4.Text) - dTotCap4;
        txtSalCap4.Text = string.Format("{0:n2}", dSalCap4);

        //Muestra totales en textbox totalizados
        txtTotCap.Text = string.Format("{0:n2}", dTotCapital);
        txtTotNor.Text = string.Format("{0:n2}", dTotNorm);
        txtTotMor.Text = string.Format("{0:n2}", dTotMor);
        txtTotSubTot.Text = string.Format("{0:n2}", dTotSubTot);

        //Si el campo esta vacio asigna un valor 0
        if (string.IsNullOrEmpty(txtImporte1.Text))
            txtImporte1.Text = "0";
        if (string.IsNullOrEmpty(txtImporte2.Text))
            txtImporte2.Text = "0";
        if (string.IsNullOrEmpty(txtImporte3.Text))
            txtImporte3.Text = "0";
        if (string.IsNullOrEmpty(txtImporte4.Text))
            txtImporte4.Text = "0";
        //Suma el total cantidad de pago
        dCanPag = Convert.ToDouble(txtImporte1.Text) + Convert.ToDouble(txtImporte2.Text) + Convert.ToDouble(txtImporte3.Text) + Convert.ToDouble(txtImporte4.Text);
        txtCanPag.Text = string.Format("{0:n2}", dCanPag);
    }

    protected void gvConceptosCto1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        GridViewRowEventArgs ea = e as GridViewRowEventArgs;
        if (ea.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = ea.Row.DataItem as DataRowView;
            Object ob = drv["Capital"];
            if (!Convert.IsDBNull(ob))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[3];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                    
                }
            }
            Object ob1 = drv["IntNorm"];
            if (!Convert.IsDBNull(ob1))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob1.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[4];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }
            Object ob2 = drv["TasaUno"];
            if (!Convert.IsDBNull(ob2))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob2.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[5];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob3 = drv["IntMorat"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob3.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[6];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob4 = drv["TasaDos"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob4.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[7];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob5 = drv["SubTotal"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob5.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[8];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }
        }
    }
    protected void gvConceptosCto2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRowEventArgs ea = e as GridViewRowEventArgs;
        if (ea.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = ea.Row.DataItem as DataRowView;
            Object ob = drv["Capital"];
            if (!Convert.IsDBNull(ob))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[3];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });

                }
            }
            Object ob1 = drv["IntNorm"];
            if (!Convert.IsDBNull(ob1))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob1.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[4];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }
            Object ob2 = drv["TasaUno"];
            if (!Convert.IsDBNull(ob2))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob2.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[5];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob3 = drv["IntMorat"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob3.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[6];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob4 = drv["TasaDos"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob4.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[7];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob5 = drv["SubTotal"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob5.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[8];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }
        }
    }
    protected void gvConceptosCto3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRowEventArgs ea = e as GridViewRowEventArgs;
        if (ea.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = ea.Row.DataItem as DataRowView;
            Object ob = drv["Capital"];
            if (!Convert.IsDBNull(ob))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[3];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });

                }
            }
            Object ob1 = drv["IntNorm"];
            if (!Convert.IsDBNull(ob1))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob1.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[4];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }
            Object ob2 = drv["TasaUno"];
            if (!Convert.IsDBNull(ob2))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob2.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[5];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob3 = drv["IntMorat"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob3.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[6];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob4 = drv["TasaDos"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob4.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[7];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob5 = drv["SubTotal"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob5.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[8];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }
        }
    }
    protected void gvConceptosCto4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRowEventArgs ea = e as GridViewRowEventArgs;
        if (ea.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = ea.Row.DataItem as DataRowView;
            Object ob = drv["Capital"];
            if (!Convert.IsDBNull(ob))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[3];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });

                }
            }
            Object ob1 = drv["IntNorm"];
            if (!Convert.IsDBNull(ob1))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob1.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[4];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }
            Object ob2 = drv["TasaUno"];
            if (!Convert.IsDBNull(ob2))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob2.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[5];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob3 = drv["IntMorat"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob3.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[6];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob4 = drv["TasaDos"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob4.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[7];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }

            Object ob5 = drv["SubTotal"];
            if (!Convert.IsDBNull(ob3))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob5.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[8];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       "{0:n2}", new object[] { iParsedValue });
                }
            }
        }
    }
}