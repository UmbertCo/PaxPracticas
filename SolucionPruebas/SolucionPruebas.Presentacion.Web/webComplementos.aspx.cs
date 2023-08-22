using System;
using System.Data;
using System.Design;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace SolucionPruebas.Presentacion.Web
{
    public partial class webComplementos : System.Web.UI.Page
    {
        Panel pnlTextBox;
        Panel pnlDropDownList;

        private static List<Control> lControles = new List<Control>();

        protected System.Web.UI.WebControls.Panel PnlForm;

        protected void Page_Load(object sender, EventArgs e)
        {
            //string sResultado = string.Empty;

            //Literal lt;
            //Label lb;

            ////Dynamic TextBox Panel
            //pnlTextBox = new Panel();
            //pnlTextBox.ID = "pnlTextBox";
            //pnlTextBox.BorderWidth = 1;
            //pnlTextBox.Width = 300;
            //phContenedor.Controls.Add(pnlTextBox);
            //lt = new Literal();
            //lt.Text = "<br />";
            //phContenedor.Controls.Add(lt);
            //lb = new Label();
            //lb.Text = "Dynamic TextBox<br />";
            //pnlTextBox.Controls.Add(lb);

            ////Button To add TextBoxes
            //Button btnAddTxt = new Button();
            //btnAddTxt.ID = "btnAddTxt";
            //btnAddTxt.Text = "Add TextBox";
            //btnAddTxt.Click += new System.EventHandler(btnAdd_Click);
            ////this.form1.Controls.Add(btnAddTxt);
            //phContenedor.Controls.Add(btnAddTxt);


            ////Dynamic DropDownList Panel
            //pnlDropDownList = new Panel();
            //pnlDropDownList.ID = "pnlDropDownList";
            //pnlDropDownList.BorderWidth = 1;
            //pnlDropDownList.Width = 300;
            //phContenedor.Controls.Add(pnlDropDownList);
            //lt = new Literal();
            //lt.Text = "<br />";
            //phContenedor.Controls.Add(lt);
            //lb = new Label();
            //lb.Text = "Dynamic DropDownList<br />";
            //phContenedor.Controls.Add(lb);

            ////Button To add DropDownlist
            //Button btnAddDdl = new Button();
            //btnAddDdl.ID = "btnAddDdl";
            //btnAddDdl.Text = "Add DropDown";
            //btnAddDdl.Click += new System.EventHandler(btnAdd_Click);
            //phContenedor.Controls.Add(btnAddDdl);

            if (!Page.IsPostBack)
            {
                lControles = new List<Control>();
            }


            if (IsPostBack)
            {
                //fnRegenerarControles("txtDynamic", "TextBox");
                //fnRegenerarControles("ddlDynamic", "DropDownList");
                fnRegenerarControlesMemoria();
            }

            ////Dummy Button To do PostBack
            //Button btnSubmit = new Button();
            //btnSubmit.ID = "btnSubmit";
            //btnSubmit.Text = "Submit";
            //btnSubmit.Click += new System.EventHandler(btnSubmit_Click);
            //phContenedor.Controls.Add(btnSubmit);
        }

        protected void btnAgregarComplemento_Click(object sender, EventArgs e)
        {
            //int nPanel = fnEncontrarOcurrencia("pnlLeyendasFiscales");

            //Panel pLeyendaFiscal = new Panel();
            //Control ctrl = Page.ParseControl(fnGenerarPanelComplemento());
            //pLeyendaFiscal = (Panel)ctrl;
            //pLeyendaFiscal.ID = "pnlLeyendasFiscales_" + Convert.ToString(nPanel + 1);
            //pnlComplementos.Controls.Add(pLeyendaFiscal);
        }

        protected void btnValidaDesdeCliente_Click(object sender, EventArgs e)
        {
            Response.Write(Request.Form["txtApellidoPaterno"]);

            //upModalAviso.Update();

        }

        protected void btnCargarComplemento_Click(object sender, EventArgs e)
        {
            int nPanel = lControles.Count;

            Panel pLeyendaFiscal = new Panel();
            Control ctrl = Page.ParseControl(fnGenerarPanelComplemento(lControles.Count));
            pnlComplementos_Update.Controls.Add(ctrl);
            //divComplementosClonados.Controls.Add(ctrl);

            lControles.Add(ctrl);

            upModalAviso.Update();
        }

        protected void lvPrueba_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //XmlDocument xdoc = new XmlDocument();
            //xdoc.Load(@"D:\Forms Dinamicos\Plantilla_1.xml");

            //XmlDocument xdTransformacion = new XmlDocument();
            //xdTransformacion.Load(@"D:\Forms Dinamicos\Plantilla_1.xslt");

            //// load xslt to do transformation
            //XslTransform xsl = new XslTransform();
            //xsl.Load(@"D:\Forms Dinamicos\Plantilla_1.xslt");

            //// load xslt arguments to load specific page from xml file
            //// this can be used if you have multiple pages
            //// in your xml file and you loading them one at a time
            //XsltArgumentList xslarg = new XsltArgumentList();
            //xslarg.AddParam("pageid", "", "page_1");

            //// get transformed results
            //StringWriter sw = new StringWriter();
            //xsl.Transform(xdoc, xslarg, sw);
            //string result = sw.ToString().Replace("xmlns:asp=\"remove\"",
            //         "").Replace("&lt;", "<").Replace("&gt;", ">");
            //// free up the memory of objects that are not used anymore
            //sw.Close();

            //// parse the controls and add it to the page
            //Control ctrl = Page.ParseControl(result);
            //lvPrueba.Controls.Add(ctrl);
            ////pnlComplementos.Controls.Add(ctrl);

            ////foreach (ListViewDataItem item in lvPrueba.Controls)
            ////{
            ////    Panel panel = (Panel)item.FindControl("pnlControles");
            ////    PlaceHolder plcHolder = (PlaceHolder)item.FindControl("plControl");

            ////    foreach (Control cControl in ctrl.Controls)
            ////    {

            ////        plcHolder.Controls.Add(ctrl);
            ////        //panel.Controls.Add(ctrl);
            ////    }

            ////}

            //upModalAviso.Update();

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //Button btn = (Button)sender;
            //if (btn.ID == "btnAddTxt")
            //{
            //    int cnt = fnEncontrarOcurrencia("txtDynamic");
            //    TextBox txt = new TextBox();
            //    txt.ID = "txtDynamic-" + Convert.ToString(cnt + 1);
            //    phContenedor.Controls.Add(txt);

            //    Literal lt = new Literal();
            //    lt.Text = "<br />";
            //    phContenedor.Controls.Add(lt);
            //}

            //if (btn.ID == "btnAddDdl")
            //{
            //    int cnt = fnEncontrarOcurrencia("ddlDynamic");
            //    DropDownList ddl = new DropDownList();
            //    ddl.ID = "ddlDynamic-" + Convert.ToString(cnt + 1);
            //    ddl.Items.Add(new ListItem("One", "1"));
            //    ddl.Items.Add(new ListItem("Two", "2"));
            //    ddl.Items.Add(new ListItem("Three", "3"));
            //    phContenedor.Controls.Add(ddl);

            //    Literal lt = new Literal();
            //    lt.Text = "<br />";
            //    phContenedor.Controls.Add(lt);
            //}

            //upModalAviso.Update();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        private int fnEncontrarOcurrencia(string substr)
        {
            string reqstr = Request.Form.ToString();
            return ((reqstr.Length - reqstr.Replace(substr, "").Length)
                    / substr.Length);
        }

        private static string fnGenerarPanelComplemento(Int32 pnNumeroComplementos)
        {
            string sComplemento = string.Empty;
            try
            {
                //fnGenerarFormulario();

                

                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(@"D:\Forms Dinamicos\ine11.xsd");

                // load xslt to do transformation
                XslTransform xsl = new XslTransform();
                xsl.Load(@"D:\Forms Dinamicos\Plantilla_ine.xslt");

                XslCompiledTransform transform = new XslCompiledTransform(true);
                transform.Load(@"D:\Forms Dinamicos\Plantilla_ine.xslt");

                // load xslt arguments to load specific page from xml file
                // this can be used if you have multiple pages
                // in your xml file and you loading them one at a time
                XsltArgumentList xslarg = new XsltArgumentList();
                xslarg.AddParam("NombreObjetos", "", string.Format("Com_Donatarias_{0}_", pnNumeroComplementos.ToString()));
                xslarg.AddParam("NumeroComplemento", "", pnNumeroComplementos.ToString());
                xslarg.AddParam("NombreComplemento", "", "donat");

                // get transformed results
                StringWriter sw = new StringWriter();
                //xsl.Transform(xdoc, xslarg, sw);
                transform.Transform(xdoc, xslarg, sw);
                sComplemento = sw.ToString().Replace("xmlns:asp=\"remove\"",
                         "").Replace("&lt;", "<").Replace("&gt;", ">");

                sComplemento = sComplemento.Replace("xmlns:cc1=\"remove\"",
                         "").Replace("&lt;", "<").Replace("&gt;", ">");
                // free up the memory of objects that are not used anymore
                sw.Close();

                sComplemento = "<%@ Register Assembly=\"AjaxControlToolkit\" Namespace=\"AjaxControlToolkit\" TagPrefix=\"cc1\" %>" + sComplemento;
                //sComplemento = sComplemento.Replace("\n", "").Replace("\r","");
                //sComplemento = sComplemento.Replace("@NumCom", pnNumeroComplementos.ToString());
                 
            }
            catch (Exception ex)
            {

            }
            return sComplemento;
        }

        private void fnRegenerarControles(string ctrlPrefix, string ctrlType)
        {
            //string[] ctrls = Request.Form.ToString().Split('&');
            //int cnt = fnEncontrarOcurrencia(ctrlPrefix);
            //if (cnt > 0)
            //{
            //    Literal lt;
            //    for (int k = 1; k <= cnt; k++)
            //    {
            //        for (int i = 0; i < ctrls.Length; i++)
            //        {
            //            if (ctrls[i].Contains(ctrlPrefix + "-" + k.ToString()))
            //            {
            //                string ctrlName = ctrls[i].Split('=')[0];
            //                string ctrlValue = ctrls[i].Split('=')[1];

            //                //Decode the Value
            //                ctrlValue = Server.UrlDecode(ctrlValue);

            //                if (ctrlType == "TextBox")
            //                {
            //                    TextBox txt = new TextBox();
            //                    txt.ID = ctrlName;
            //                    txt.Text = ctrlValue;
            //                    phContenedor.Controls.Add(txt);
            //                    lt = new Literal();
            //                    lt.Text = "<br />";
            //                    phContenedor.Controls.Add(lt);
            //                }

            //                if (ctrlType == "DropDownList")
            //                {
            //                    DropDownList ddl = new DropDownList();
            //                    ddl.ID = ctrlName;

            //                    //Rebind Data
            //                    ddl.Items.Add(new ListItem("One", "1"));
            //                    ddl.Items.Add(new ListItem("Two", "2"));
            //                    ddl.Items.Add(new ListItem("Three", "3"));

            //                    //Select the Preselected Item
            //                    ddl.Items.FindByValue(ctrlValue).Selected = true;
            //                    phContenedor.Controls.Add(ddl);
            //                    lt = new Literal();
            //                    lt.Text = "<br />";
            //                    phContenedor.Controls.Add(lt);
            //                }

            //                break;
            //            }
            //        }
            //    }
            //}
        }

        private void fnRegenerarControlesMemoria()
        {
            try
            {
                foreach (Control cControl in lControles)
                {
                    pnlComplementos_Update.Controls.Add(cControl);
                }
            }
            catch (Exception ex)
            {

            } 
        }

        [System.Web.Services.WebMethod]
        public static void DropDownList_SelectedIndexChanged(string psNombreObjeto, string psValor, string psNumeroComplemento)
        {
            try
            {
                Control cComplemento = lControles[0];

                ((DropDownList)(cComplemento.FindControl(psNombreObjeto))).SelectedValue = psValor;

                lControles[0] = cComplemento;
            }
            catch (Exception ex)
            {

            }
        }

        [System.Web.Services.WebMethod]
        public static void TextBox_TextChanged(string psNombreObjeto, string psValor, string psNumeroComplemento)
        {
            try
            {
                Control cComplemento = lControles[0];

                ((TextBox)(cComplemento.FindControl(psNombreObjeto))).Text = psValor;

                lControles[0] = cComplemento;
            }
            catch (Exception ex)
            {

            }
        }

        static void fnCambiarID(ref HtmlGenericControl pcContenedor, int pnNumeroObjeto)
        {
            string sTipoControl = string.Empty;
            try
            {
                foreach (var control in pcContenedor.Controls)
                {
                    sTipoControl = control.GetType().ToString();

                    if (sTipoControl.Equals("System.Web.UI.WebControls.TextBox"))
                    {
                        TextBox txtTexBox = (TextBox)control;

                        if (((RequiredFieldValidator)(pcContenedor.FindControl("rfv" + txtTexBox.ID))) != null)
                        {
                            ((RequiredFieldValidator)(pcContenedor.FindControl("rfv" + txtTexBox.ID))).ControlToValidate = txtTexBox.ID + "_" + pnNumeroObjeto.ToString();
                            ((RequiredFieldValidator)(pcContenedor.FindControl("rfv" + txtTexBox.ID))).ID = ((RequiredFieldValidator)(pcContenedor.FindControl("rfv" + txtTexBox.ID))).ID + "_" + pnNumeroObjeto.ToString();
                        }

                        if (((CompareValidator)(pcContenedor.FindControl("cv" + txtTexBox.ID))) != null)
                        {
                            ((CompareValidator)(pcContenedor.FindControl("cv" + txtTexBox.ID))).ControlToValidate = txtTexBox.ID + "_" + pnNumeroObjeto.ToString();
                            ((CompareValidator)(pcContenedor.FindControl("cv" + txtTexBox.ID))).ID = ((RequiredFieldValidator)(pcContenedor.FindControl("cv" + txtTexBox.ID))).ID + "_" + pnNumeroObjeto.ToString();
                        }

                        ((TextBox)(pcContenedor.FindControl(txtTexBox.ID))).ID = txtTexBox.ID + "_" + pnNumeroObjeto.ToString();
                    }
                    else if (sTipoControl.Equals("System.Web.UI.WebControls.DropDownList"))
                    {
                        DropDownList dllComboBox = (DropDownList)control;
                        ((DropDownList)(pcContenedor.FindControl(dllComboBox.ID))).ID = dllComboBox.ID + "_" + pnNumeroObjeto.ToString();
                    }
                    else if (sTipoControl.Equals("System.Web.UI.WebControls.Panel"))
                    {
                        Panel pPanel = (Panel)control;
                        ((Panel)(pcContenedor.FindControl(pPanel.ID))).ID = pPanel.ID + "_" + pnNumeroObjeto.ToString();

                        Control cControl = (Control)control;

                        fnCambiarID(ref cControl, pnNumeroObjeto);
                    }
                    else if (sTipoControl.Equals("System.Web.UI.WebControls.Label"))
                    {
                        Label pLabel = (Label)control;
                        ((Label)(pcContenedor.FindControl(pLabel.ID))).ID = pLabel.ID + "_" + pnNumeroObjeto.ToString();
                    }
                    else if (sTipoControl.Equals("System.Web.UI.WebControls.Button"))
                    {
                        Button dllComboBox = (Button)control;
                        ((Button)(pcContenedor.FindControl(dllComboBox.ID))).ID = dllComboBox.ID + "_" + pnNumeroObjeto.ToString();
                    }
                    else if (sTipoControl.Equals("System.Web.UI.LiteralControl"))
                    {
                        LiteralControl lcControl = (LiteralControl)control;
                        if (!string.IsNullOrEmpty(lcControl.ID))
                            ((Button)(pcContenedor.FindControl(lcControl.ID))).ID = lcControl.ID + "_" + pnNumeroObjeto.ToString();
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            { 
            
            }
        }

        static void fnCambiarID(ref Control pcContenedor, int pnNumeroObjeto)
        {
            string sTipoControl = string.Empty;
            try
            {
                

                foreach (var control in pcContenedor.Controls)
                {
                    sTipoControl = control.GetType().ToString();

                    if (sTipoControl.Equals("System.Web.UI.WebControls.TextBox"))
                    {
                        TextBox txtTexBox = (TextBox)control;

                        if (((RequiredFieldValidator)(pcContenedor.FindControl("rfv" + txtTexBox.ID))) != null)
                        {
                            ((RequiredFieldValidator)(pcContenedor.FindControl("rfv" + txtTexBox.ID))).ControlToValidate = txtTexBox.ID + "_" + pnNumeroObjeto.ToString();
                            ((RequiredFieldValidator)(pcContenedor.FindControl("rfv" + txtTexBox.ID))).ID = ((RequiredFieldValidator)(pcContenedor.FindControl("rfv" + txtTexBox.ID))).ID + "_" + pnNumeroObjeto.ToString();
                        }

                        if (((CompareValidator)(pcContenedor.FindControl("cv" + txtTexBox.ID))) != null)
                        {
                            ((CompareValidator)(pcContenedor.FindControl("cv" + txtTexBox.ID))).ControlToValidate = txtTexBox.ID + "_" + pnNumeroObjeto.ToString();
                            ((CompareValidator)(pcContenedor.FindControl("cv" + txtTexBox.ID))).ID = ((CompareValidator)(pcContenedor.FindControl("cv" + txtTexBox.ID))).ID + "_" + pnNumeroObjeto.ToString();
                        }

                        ((TextBox)(pcContenedor.FindControl(txtTexBox.ID))).ID = txtTexBox.ID + "_" + pnNumeroObjeto.ToString();                        
                    }
                    else if (sTipoControl.Equals("System.Web.UI.WebControls.DropDownList"))
                    {
                        DropDownList dllComboBox = (DropDownList)control;
                        ((DropDownList)(pcContenedor.FindControl(dllComboBox.ID))).ID = dllComboBox.ID + "_" + pnNumeroObjeto.ToString();
                    }
                    else if (sTipoControl.Equals("System.Web.UI.WebControls.Panel"))
                    {
                        Panel pPanel = (Panel)control;
                        ((Panel)(pcContenedor.FindControl(pPanel.ID))).ID = pPanel.ID + "_" + pnNumeroObjeto.ToString();

                        HtmlGenericControl cControl = (HtmlGenericControl)control;

                        fnCambiarID(ref cControl, pnNumeroObjeto);
                    }
                    else if (sTipoControl.Equals("System.Web.UI.WebControls.Label"))
                    {
                        Label pLabel = (Label)control;
                        ((Label)(pcContenedor.FindControl(pLabel.ID))).ID = pLabel.ID + "_" + pnNumeroObjeto.ToString();
                    }
                    else if (sTipoControl.Equals("System.Web.UI.WebControls.Button"))
                    {
                        Button btnComboBox = (Button)control;
                        ((Button)(pcContenedor.FindControl(btnComboBox.ID))).ID = btnComboBox.ID + "_" + pnNumeroObjeto.ToString();
                    }
                    else if (sTipoControl.Equals("System.Web.UI.LiteralControl"))
                    {
                        LiteralControl lcControl = (LiteralControl)control;
                        if (!string.IsNullOrEmpty(lcControl.ID))
                            ((Button)(pcContenedor.FindControl(lcControl.ID))).ID = lcControl.ID + "_" + pnNumeroObjeto.ToString();
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        [System.Web.Services.WebMethod]
        public static string fnAgregarBoton(string psNombreObjeto, string psNombreObjetoPadre, string psNumero)
        {
            string sTipoControl = string.Empty;
            int nNumero = 0;
            string sResultado = string.Empty;
            try
            {
                Control cComplemento = lControles[0];

                HtmlGenericControl hcDiv = ((HtmlGenericControl)(cComplemento.FindControl(psNombreObjeto)));
                HtmlGenericControl hcDivPadre = ((HtmlGenericControl)(cComplemento.FindControl(psNombreObjetoPadre)));

                HtmlGenericControl hcDivNuevo = new HtmlGenericControl();

                hcDivNuevo = hcDiv;

                bool result = Int32.TryParse(psNumero, out nNumero);

                if (!result)
                {
                    nNumero = 1;
                }

                fnCambiarID(ref hcDivNuevo, nNumero);

                hcDivNuevo.ID = hcDivNuevo.ID + "_" + nNumero.ToString();
                //hcDivPadre.Controls.Add(hcDivNuevo);

                ((HtmlGenericControl)(cComplemento.FindControl(psNombreObjetoPadre))).Controls.Add(hcDivNuevo);
                
                lControles[0] = cComplemento;

                sResultado = "1";               
            }
            catch (Exception ex)
            {

            }
            return sResultado;
        }

        [System.Web.Services.WebMethod]
        public static string fnAgregarBoton1(string psObjeto, string psNombreObjetoPadre)
        {
            string sTipoControl = string.Empty;
            int nNumero = 0;
            string sResultado = string.Empty;
            try
            {
                Control cComplemento = lControles[0];

                HtmlGenericControl hcDiv = new HtmlGenericControl();
                hcDiv.InnerHtml = psObjeto;

                HtmlGenericControl hcDivPadre = ((HtmlGenericControl)(cComplemento.FindControl(psNombreObjetoPadre)));

                ((HtmlGenericControl)(cComplemento.FindControl(psNombreObjetoPadre))).Controls.Add(hcDiv);

                lControles[0] = cComplemento;

            }
            catch (Exception ex)
            {

            }
            return sResultado;
        }

        [System.Web.Services.WebMethod]
        public static string fnAgregarBoton1(string pnNumeroComplementos)
        {
            string sResultado = string.Empty;
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(@"D:\Forms Dinamicos\ine11.xsd");

                // load xslt to do transformation
                XslTransform xsl = new XslTransform();
                xsl.Load(@"D:\Forms Dinamicos\Plantilla_ine.xslt");

                XslCompiledTransform transform = new XslCompiledTransform(true);
                transform.Load(@"D:\Forms Dinamicos\Plantilla_ine.xslt");

                // load xslt arguments to load specific page from xml file
                // this can be used if you have multiple pages
                // in your xml file and you loading them one at a time
                XsltArgumentList xslarg = new XsltArgumentList();
                xslarg.AddParam("NombreObjetos", "", string.Format("Com_INE_{0}_", pnNumeroComplementos.ToString()));
                xslarg.AddParam("NumeroComplemento", "", pnNumeroComplementos.ToString());
                xslarg.AddParam("NombreComplemento", "", "ine");

                // get transformed results
                StringWriter sw = new StringWriter();
                //xsl.Transform(xdoc, xslarg, sw);
                transform.Transform(xdoc, xslarg, sw);
                sResultado = sw.ToString().Replace("xmlns:asp=\"remove\"",
                         "").Replace("&lt;", "<").Replace("&gt;", ">");

                sResultado = sResultado.Replace("xmlns:cc1=\"remove\"",
                         "").Replace("&lt;", "<").Replace("&gt;", ">");
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
        public static string fnAgregarComplemento(string psRutaComplemento, string psNombreComplemento, string pnNumeroComplementos)
        {
            string sResultado = string.Empty;
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(psRutaComplemento);

                // load xslt to do transformation
                XslTransform xsl = new XslTransform();
                xsl.Load(@"D:\Forms Dinamicos\Plantilla_ine.xslt");

                XslCompiledTransform transform = new XslCompiledTransform(true);
                transform.Load(@"D:\Forms Dinamicos\Plantilla_ine.xslt");

                // load xslt arguments to load specific page from xml file
                // this can be used if you have multiple pages
                // in your xml file and you loading them one at a time
                XsltArgumentList xslarg = new XsltArgumentList();
                xslarg.AddParam("NombreObjetos", "", string.Format("Com_{0}_{1}_", psNombreComplemento, pnNumeroComplementos.ToString()));
                xslarg.AddParam("NumeroComplemento", "", pnNumeroComplementos.ToString());
                xslarg.AddParam("NombreComplemento", "", psNombreComplemento.ToLower());

                // get transformed results
                StringWriter sw = new StringWriter();
                //xsl.Transform(xdoc, xslarg, sw);
                transform.Transform(xdoc, xslarg, sw);
                sResultado = sw.ToString().Replace("xmlns:asp=\"remove\"",
                         "").Replace("&lt;", "<").Replace("&gt;", ">");

                sResultado = sResultado.Replace("xmlns:cc1=\"remove\"",
                         "").Replace("&lt;", "<").Replace("&gt;", ">");
                // free up the memory of objects that are not used anymore
                sw.Close();


                //sResultado = "<%@ Register Assembly=\"AjaxControlToolkit\" Namespace=\"AjaxControlToolkit\" TagPrefix=\"cc1\" %>" + sResultado;
            }
            catch (Exception ex)
            {

            }
            return sResultado;
        }

        protected void bbtnValidar_Click(object sender, EventArgs e)
        {
            fnValidarXML();
        }

        private string fnValidarXML()
        {
            string sResultado = string.Empty;
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(@"D:\Forms Dinamicos\Ejemplo1.xml");

                //Cargamos la hoja de transformación
                XslTransform xsl = new XslTransform();
                xsl.Load(@"D:\Forms Dinamicos\ejemplo_modificado.xslt");

                //Cargamos la hoja de transformación
                XslCompiledTransform transform = new XslCompiledTransform(true);
                transform.Load(@"D:\Forms Dinamicos\ejemplo_modificado.xslt");

                //Si se necesitan cargar parametros para pasarlos al 
                // archivo de transformación se realizaría con este Objeto
                XsltArgumentList xslarg = new XsltArgumentList();

                // get transformed results
                StringWriter sw = new StringWriter();
                //xsl.Transform(xdoc, xslarg, sw);
                transform.Transform(xdoc, xslarg, sw);
                sResultado = sw.ToString().Replace("xmlns:asp=\"remove\"",
                         "").Replace("&lt;", "<").Replace("&gt;", ">");
                // free up the memory of objects that are not used anymore
                sw.Close();


            }
            catch (Exception ex)
            { 
            
            }
            return sResultado;
        }

        protected void btnGenerarXML_Click(object sender, EventArgs e)
        {
            DataTable dtXMLHija = new DataTable("Hijo");
            dtXMLHija.Columns.Add("id", typeof(int));
            dtXMLHija.Columns.Add("valor", typeof(string));

            DataRow drRenglonHijo = null;           

            drRenglonHijo = dtXMLHija.NewRow();
            drRenglonHijo["id"] = 1;
            drRenglonHijo["valor"] = "ismael";
            dtXMLHija.Rows.Add(drRenglonHijo);

            drRenglonHijo = dtXMLHija.NewRow();
            drRenglonHijo["id"] = 1;
            drRenglonHijo["valor"] = "carlos";
            dtXMLHija.Rows.Add(drRenglonHijo);


            DataSet dsXML = new DataSet();

            DataTable dtXML = new DataTable("Padre");
            dtXML.Columns.Add("total", typeof(string));
            dtXML.Columns.Add("subtotal", typeof(string));
            //dtXML.Columns.Add("tablahija", typeof(DataTable));

            DataRow drRenglon = null;

            drRenglon = dtXML.NewRow();
            drRenglon["total"] = 1;
            drRenglon["subtotal"] = 2;
            //drRenglon["tablahija"] = dtXMLHija;
            dtXML.Rows.Add(drRenglon);

            dsXML.Tables.Add(dtXML);
            dsXML.Tables.Add(dtXMLHija);

            XmlDataDocument xddDocumento = new XmlDataDocument(dsXML);
            string sResultado = string.Empty;

            foreach (DataTable dtResultado in xddDocumento.DataSet.Tables)
            {
                foreach (DataRow drRenglonResultado in dtResultado.Rows)
                {
                    sResultado += xddDocumento.GetElementFromRow(drRenglonResultado).OuterXml;
                }
            }
        }

        private static string fnGenerarFormulario()
        {
            string sResultado = string.Empty;
            XmlDocument xdComprobante = new XmlDocument();
            try
            {
                xdComprobante.Load(@"D:\Forms Dinamicos\ine11.xsd");
                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdComprobante.NameTable);

                nsmComprobante.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

                XPathNavigator xpnNavegador = xdComprobante.CreateNavigator();

                XPathNodeIterator xpnTipoComplejo = xpnNavegador.Select("/xs:schema/xs:element/xs:complexType/*", nsmComprobante);

                foreach (XPathNavigator xpnElementos in xpnTipoComplejo)
                {

                    if (xpnElementos.Name.Equals("xs:attribute"))
                    {
                        fnGenerarAtributos();
                    }

                    if (xpnElementos.Name.Equals("xs:sequence"))
                    {
                        fnGenerarSecuencia(xpnElementos.InnerXml, nsmComprobante);
                    }
                }
            }
            catch (Exception ex)
            { 
            
            }
            return sResultado;
        }

        private static void  fnGenerarAtributos()
        {
            try
            { 
            
            }
            catch (Exception ex)
            { 
            
            }
        }

        private static void fnGenerarSecuencia(string psDocumento, XmlNamespaceManager nsmComprobante)
        {
            XmlDocument xdDocumento = new XmlDocument();
            try
            {
                xdDocumento.LoadXml(psDocumento);
                XPathNavigator xpnNavegadorSecuencia = xdDocumento.CreateNavigator();
                XPathNodeIterator xpnElementosSecuencia = xpnNavegadorSecuencia.Select("/*", nsmComprobante);

                foreach (XPathNavigator xpnElementoSecuencia in xpnElementosSecuencia)
                {

                    if (xpnElementoSecuencia.Name.Equals("xs:element"))
                    {
                        XPathNodeIterator xpnTipoComplejoElemento = xpnElementoSecuencia.Select("/xs:element/xs:complexType", nsmComprobante);

                        foreach (XPathNavigator xpnElementos in xpnTipoComplejoElemento)
                        {
                            if (xpnElementos.Name.Equals("xs:attribute"))
                            {
                                fnGenerarAtributos();
                            }

                            if (xpnElementos.Name.Equals("xs:sequence"))
                            {
                                fnGenerarSecuencia(xpnElementos.InnerXml, nsmComprobante);
                            }
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {

            }
        }        
    }
}