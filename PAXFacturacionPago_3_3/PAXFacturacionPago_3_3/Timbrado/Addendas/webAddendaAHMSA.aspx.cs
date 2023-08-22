using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Timbrado_Addendas_webAddendaAHMSA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fnLlenarTipoDocumento();
            fnLlenarClases();
            fnLlenarSociedades();
            fnLlenarDivisiones();
            fnLlenarMonedas();
            ddlNumDivision.Enabled = fnDivisionRequerida();
            btnCerrar.Attributes.Add("onclick", "window.close();");
        }

    }


    private void fnLlenarTipoDocumento()
    {
        ddlTipoDocumento.Items.Clear();
        ddlTipoDocumento.Items.Add(new ListItem("Factura con pedido", "1"));
        ddlTipoDocumento.Items.Add(new ListItem("Factura sin pedido", "2"));
        ddlTipoDocumento.Items.Add(new ListItem("Nota de Cargo", "3"));
        ddlTipoDocumento.Items.Add(new ListItem("Nota de Crédito", "4"));
    }

    private void fnLlenarClases()
    {
        ddlClase.Items.Clear();
        ddlClase.Items.Add(new ListItem("Factura con pedido de compra", "PE"));
        ddlClase.Items.Add(new ListItem("Factura sin pedido", "PS"));
        ddlClase.Items.Add(new ListItem("Factura de pedido con anticipo", "PA"));
        ddlClase.Items.Add(new ListItem("Factura de anticipo sin pedido", "AS"));
        ddlClase.Items.Add(new ListItem("Factura con hoja de servicio o servicio de maquila", "HS"));
        ddlClase.Items.Add(new ListItem("Factura de flete de compra o transporte de maquila", "FC"));
        ddlClase.Items.Add(new ListItem("Factura de flete de venta o carta porte", "FV"));
        ddlClase.Items.Add(new ListItem("Factura de agente aduanal", "AA"));
        ddlClase.Items.Add(new ListItem("Factura de consignación", "CO"));
        ddlClase.Items.Add(new ListItem("Factura de Flete proven", "KT"));
        ddlClase.Items.Add(new ListItem("Factura de premios y castigos", "PT"));
        ddlClase.Items.Add(new ListItem("Nota de crédito", "AC"));
        ddlClase.Items.Add(new ListItem("Nota de cargo", "NC"));
    }

    private void fnLlenarSociedades()
    {
        ddlNumSociedad.Items.Clear();
        ddlNumSociedad.Items.Add(new ListItem("Altos Hornos de México", "S001"));
        ddlNumSociedad.Items.Add(new ListItem("Minera del Norte S.A.", "S003"));
        ddlNumSociedad.Items.Add(new ListItem("Servs. Corp. AHMSA S.A.", "S005"));
        ddlNumSociedad.Items.Add(new ListItem("Servs. Area Carbón S.A.", "S006"));
        ddlNumSociedad.Items.Add(new ListItem("REDINSA S.A de C.V", "S008"));
        ddlNumSociedad.Items.Add(new ListItem("Carboeléctrica de Sabinas", "S015"));
        ddlNumSociedad.Items.Add(new ListItem("Servicios ANTAIR", "S021"));
        ddlNumSociedad.Items.Add(new ListItem("Corporativo ANSAT", "S022"));
        ddlNumSociedad.Items.Add(new ListItem("ANTAIR", "S024"));
        ddlNumSociedad.Items.Add(new ListItem("Servicios Monclova S.A.", "S027"));
        ddlNumSociedad.Items.Add(new ListItem("Tecno Servicios S.A.", "S028"));
        ddlNumSociedad.Items.Add(new ListItem("Resguardo Corporativo SA", "S029"));
    }

    private void fnLlenarDivisiones()
    {
        ddlNumDivision.Items.Clear();
        ddlNumDivision.Items.Add(new ListItem("Unidad MICARE", "D002"));
        ddlNumDivision.Items.Add(new ListItem("Unidad MIMOSA", "D003"));
        ddlNumDivision.Items.Add(new ListItem("Unidad Hércules", "D018"));
        ddlNumDivision.Items.Add(new ListItem("Unidad Cerro de Mercado", "D020"));
    }

    private void fnLlenarMonedas()
    {
        ddlMoneda.Items.Clear();
        ddlMoneda.Items.Add(new ListItem("Pesos Mexicanos", "MXP"));
        ddlMoneda.Items.Add(new ListItem("Dólares Americanos", "USD"));
        ddlMoneda.Items.Add(new ListItem("Euros", "EUR"));
    }


    private string fnArmarXml()
    {
        //Addenda
        //-AddendaAHM
        string version = "1.0"; //ver si se obtiene de los parámetros o algún otro lado
        //--Documento
        int tipo = Convert.ToInt32(ddlTipoDocumento.SelectedValue);
        string clase = ddlClase.SelectedValue;
        //---Encabezado
        string numSociedad = ddlNumSociedad.SelectedValue;
        string numDivision = fnDivisionRequerida() ? ddlNumDivision.SelectedValue : string.Empty;
        string numProveedor = string.Empty;
        if (!string.IsNullOrEmpty(txtNumProveedor.Text))
        {
            if (txtNumProveedor.Text.Length >= 6)
            {
                try
                {
                    long nNumPRoveedor = Convert.ToInt64(txtNumProveedor.Text);
                    if (nNumPRoveedor > 0)
                    {
                        numProveedor = nNumPRoveedor.ToString();

                    }
                    else
                    {
                        clsComun.fnMessage(this, "El número de proveedor es incorrecto", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                        return null;
                    }
                }

                catch (Exception ex)
                {
                    clsComun.fnMessage(this, "El número de proveedor es incorrecto", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                    return null;
                }
            }
            else
            {
                clsComun.fnMessage(this, "El número de proveedor debe ser mayor a 6 dígitos", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
        }
        else
        {
            clsComun.fnMessage(this, "El número de proveedor es requerido", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
            return null;
        }
        string correo = txtCorreo.Text;
        if (!clsComun.fnValidaExpresion(correo, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
        {
            clsComun.fnMessage(this, "El correo es incorrecto", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
            return null;
        }

        string moneda = ddlMoneda.SelectedValue; //Sólo el código: MXP - Pesos Mexicanos, USD - Dólares americanos, EUR - Euros
        //---Detalle
        //---- Pedido
        string[][] pedidos = fnObtenerServicios();
        if (fnServiciosRequeridos())
        {
            if (pedidos == null)
            {
                clsComun.fnMessage(this, "Falto agregar una recepción en uno de los pedidos", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
            if (pedidos.Length.Equals(0))
            {
                clsComun.fnMessage(this, "Debe agregar al menos un pedido debido al tipo y clase que seleccionó", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
        }

        string hojaServicioNum = txtHojaServicio.Text;
        if (fnHojaServicioRequerido() && string.IsNullOrEmpty(hojaServicioNum))
        {
            clsComun.fnMessage(this, "La hoja de servicio es requerida por el tipo y clase de documento", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
            return null;
        }
        //---- Transporte
        string transporteNum = txtNumTransporte.Text;
        if (ddlTipoDocumento.SelectedValue == "2" && ddlClase.SelectedValue == "FV" && string.IsNullOrEmpty(transporteNum))
        {
            clsComun.fnMessage(this, "El número de transporte es requerida por el tipo y clase de documento", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
            return null;
        }
        if (!string.IsNullOrEmpty(transporteNum))
        {
            if (transporteNum.Length < 7)
            {
                clsComun.fnMessage(this, "El número de transporte debe ser de mínimo 7 dígitos", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
        }
        string ctaxPagNum = txtNumCtaxPag.Text;
        long ctaxPagEjercicio = 0;
        if (ddlNumSociedad.SelectedValue == "S003" &&
            ddlTipoDocumento.SelectedValue == "2" &&
            ddlClase.SelectedValue == "KT")
        {
            if (string.IsNullOrEmpty(ctaxPagNum))
            {
                clsComun.fnMessage(this, "El número de cuenta por pagar es requerido por los datos seleccionados", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
            if (string.IsNullOrEmpty(txtEjCtaxPag.Text))
            {
                clsComun.fnMessage(this, "El número de ejercicio de cuenta por pagar es requerido por los datos seleccionados", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
        }
        if (!string.IsNullOrEmpty(ctaxPagNum))
        {
            if (!ctaxPagNum.Length.Equals(10))
            {
                clsComun.fnMessage(this, "El número de cuenta por pagar debe ser igual a 10 dígitos", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
            try
            {
                long nCtaxPagNum = Convert.ToInt64(ctaxPagNum);
                if (nCtaxPagNum <= 0)
                {
                    clsComun.fnMessage(this, "El número de cuenta por pagar debe ser mayor a cero", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsComun.fnMessage(this, "El número de cuenta por pagar debe ser númerico", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
        }

        if (!string.IsNullOrEmpty(txtEjCtaxPag.Text))
        {
            if (!txtEjCtaxPag.Text.Length.Equals(4))
            {
                clsComun.fnMessage(this, "El ejercicio de cuenta por pagar debe ser igual a 4 dígitos", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }

            try
            {
                ctaxPagEjercicio = Convert.ToInt64(txtEjCtaxPag.Text);
                if (ctaxPagEjercicio <= 0)
                {
                    clsComun.fnMessage(this, "El número de ejercicio de cuenta por pagar debe ser mayor a cero", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsComun.fnMessage(this, "El número de ejercicio de cuenta por pagar debe ser númerico", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
        }


        string sFechaInicio = string.Empty;
        string sFechaFin = string.Empty;
        if (ddlTipoDocumento.SelectedValue == "2" && ddlClase.SelectedValue == "CO")
        {
            if (string.IsNullOrEmpty(txtFechaIni.Text) || string.IsNullOrEmpty(txtFechaFin.Text))
            {
                clsComun.fnMessage(this, "Ambas fechas son requeridas por el tipo y clase de documento", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
        }
        if (!string.IsNullOrEmpty(txtFechaIni.Text))
        {
            try
            {
                sFechaInicio = DateTime.ParseExact(txtFechaIni.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");
            }
            catch (Exception ex)
            {
                clsComun.fnMessage(this, "Ambas fechas son requeridas por el tipo y clase de documento", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
        }

        if (!string.IsNullOrEmpty(txtFechaFin.Text))
        {
            try
            {
                sFechaFin = DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");
            }
            catch (Exception ex)
            {
                clsComun.fnMessage(this, "Ambas fechas son requeridas por el tipo y clase de documento", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return null;
            }
        }
        string[] anexos = fnObtenerAnexos();



        XmlDocument addenda = new XmlDocument();

        XmlElement rootAddenda = addenda.CreateElement("Addenda");
        XmlAttribute atXmlns = addenda.CreateAttribute("xmlns:ahmsa", "http://www.w3.org/2000/xmlns/");
        atXmlns.Value = "http://www.ahmsa.com/xsd/AddendaAHM1";
        rootAddenda.SetAttributeNode(atXmlns);

        XmlAttribute atXsi = addenda.CreateAttribute("xsi:schemaLocation");
        atXsi.Value = "http://www.ahmsa.com/xsd/AddendaAHM1 http://www.ahmsa.com/xsd/AddendaAHM1/AddendaAHM.xsd";
        rootAddenda.SetAttributeNode(atXsi);



        XmlElement xeAHM = addenda.CreateElement("ahmsa", "AddendaAHM", "http://www.ahmsa.com/xsd/AddendaAHM1");
        xeAHM.SetAttribute("Version", version);

        XmlElement xeDocumento = addenda.CreateElement("ahmsa", "Documento", "http://www.ahmsa.com/xsd/AddendaAHM1");
        xeDocumento.SetAttribute("Tipo", tipo.ToString());
        xeDocumento.SetAttribute("Clase", clase);

        XmlElement xeEncabezado = addenda.CreateElement("ahmsa", "Encabezado", "http://www.ahmsa.com/xsd/AddendaAHM1");
        xeEncabezado.SetAttribute("NumSociedad", numSociedad);
        xeEncabezado.SetAttribute("NumDivision", numDivision);
        xeEncabezado.SetAttribute("NumProveedor", numProveedor);
        xeEncabezado.SetAttribute("Correo", correo);
        xeEncabezado.SetAttribute("Moneda", moneda);

        xeDocumento.AppendChild(xeEncabezado);

        XmlElement xeDetalle = addenda.CreateElement("ahmsa", "Detalle", "http://www.ahmsa.com/xsd/AddendaAHM1");

        foreach (string[] pedido in pedidos)
        {
            XmlElement xePedido = addenda.CreateElement("ahmsa", "Pedido", "http://www.ahmsa.com/xsd/AddendaAHM1");
            xePedido.SetAttribute("Num", pedido[0]);
            for (int i = 1; i < pedido.Length; i++)
            {
                XmlElement xeRecepcion = addenda.CreateElement("ahmsa", "Recepcion", "http://www.ahmsa.com/xsd/AddendaAHM1");
                xeRecepcion.InnerText = pedido[i];
                xePedido.AppendChild(xeRecepcion);
            }
            xeDetalle.AppendChild(xePedido);
        }

        XmlElement xeHojaServicio = addenda.CreateElement("ahmsa", "HojaServicio", "http://www.ahmsa.com/xsd/AddendaAHM1");
        xeHojaServicio.SetAttribute("Num", hojaServicioNum);
        xeDetalle.AppendChild(xeHojaServicio);

        XmlElement xeTransporte = addenda.CreateElement("ahmsa", "Transporte", "http://www.ahmsa.com/xsd/AddendaAHM1");
        xeTransporte.SetAttribute("Num", transporteNum);
        xeDetalle.AppendChild(xeTransporte);

        XmlElement xeCtaxPag = addenda.CreateElement("ahmsa", "CtaxPag", "http://www.ahmsa.com/xsd/AddendaAHM1");
        xeCtaxPag.SetAttribute("Num", ctaxPagNum);
        xeCtaxPag.SetAttribute("Ejercicio", ctaxPagEjercicio > 0 ? ctaxPagEjercicio.ToString() : "");
        xeDetalle.AppendChild(xeCtaxPag);

        XmlElement xeLiquidacion = addenda.CreateElement("ahmsa", "Liquidacion", "http://www.ahmsa.com/xsd/AddendaAHM1");
        xeLiquidacion.SetAttribute("FechaInicio", sFechaInicio);
        xeLiquidacion.SetAttribute("FechaFin", sFechaFin);
        xeDetalle.AppendChild(xeLiquidacion);

        xeDocumento.AppendChild(xeDetalle);

        if (anexos.Length > 0)
        {
            XmlElement xeAnexos = addenda.CreateElement("ahmsa", "Anexos", "http://www.ahmsa.com/xsd/AddendaAHM1");
            foreach (string anexo in anexos)
            {
                XmlElement xeAnexo = addenda.CreateElement("ahmsa", "Anexo", "http://www.ahmsa.com/xsd/AddendaAHM1");
                xeAnexo.InnerText = anexo;
                xeAnexos.AppendChild(xeAnexo);
            }
            xeDocumento.AppendChild(xeAnexos);
        }


        xeAHM.AppendChild(xeDocumento);

        rootAddenda.AppendChild(xeAHM);
        addenda.AppendChild(rootAddenda);

        return addenda.OuterXml.ToString();

    }

    private string[] fnObtenerAnexos()
    {
        string[] anexos = new string[lsbAnexos.Items.Count];
        for (int i = 0; i < lsbAnexos.Items.Count; i++)
        {
            anexos[i] = lsbAnexos.Items[i].Text;
        }
        return anexos;
    }
    private bool fnHojaServicioRequerido()
    {
        return (ddlTipoDocumento.SelectedValue == "1" && (ddlClase.SelectedValue == "HS" || ddlClase.SelectedValue == "PA"));
    }
    private string[][] fnObtenerServicios()
    {
        string[][] servicios = new string[0][];

        //---- Revisa si el Servicio selecionado necesita necesita pedido 
        if (fnServiciosRequeridos() && trvServicios.Nodes.Count <= 0)
            return servicios;

        string[] recepcion;

        //---- Revisa si el Servicio seleccionado no aplica algun pedido
        if (fnServiciosRequeridos())
        {
            //---- Servicio que tiene pedido
            servicios = new string[trvServicios.Nodes.Count][];

            for (int j = 0; j < trvServicios.Nodes.Count; j++)
            {
                TreeNode tnServicio = trvServicios.Nodes[j];
                recepcion = new string[tnServicio.ChildNodes.Count + 1];
                recepcion[0] = tnServicio.Text;

                for (int i = 0; i < tnServicio.ChildNodes.Count; i++)
                {
                    recepcion[i + 1] = tnServicio.ChildNodes[i].Text;
                }
                servicios[j] = recepcion;
            }
        }
        else if (fnServicioNoAplica())
        {
            servicios = new string[1][];
            servicios[0] = new string[] { "NA" };
        }
        else
        {
            if (trvServicios.Nodes.Count.Equals(0))
            {
                servicios = new string[1][];
                servicios[0] = new string[] { string.Empty };
            }
            else
            {
                //---- Servicio que tiene pedido
                servicios = new string[trvServicios.Nodes.Count][];

                for (int j = 0; j < trvServicios.Nodes.Count; j++)
                {
                    TreeNode tnServicio = trvServicios.Nodes[j];
                    recepcion = new string[tnServicio.ChildNodes.Count + 1];
                    recepcion[0] = tnServicio.Text;

                    for (int i = 0; i < tnServicio.ChildNodes.Count; i++)
                    {
                        recepcion[i + 1] = tnServicio.ChildNodes[i].Text;
                    }
                    servicios[j] = recepcion;
                }
            }
        }

        return servicios;
    }
    private bool fnServicioNoAplica()
    {
        if (ddlTipoDocumento.SelectedValue == "2")
        {
            if (ddlClase.SelectedValue == "PS" ||
            ddlClase.SelectedValue == "AS" ||
            ddlClase.SelectedValue == "FC" ||
            ddlClase.SelectedValue == "FV" ||
            ddlClase.SelectedValue == "AA" ||
            ddlClase.SelectedValue == "KT" ||
            ddlClase.SelectedValue == "PT")
                return true;
        }
        if (ddlTipoDocumento.SelectedValue == "3" && ddlClase.SelectedValue == "NC")
            return true;
        if (ddlTipoDocumento.SelectedValue == "4" && ddlClase.SelectedValue == "AC")
            return true;
        return false;
    }
    private bool fnServiciosRequeridos()
    {
        return (ddlTipoDocumento.SelectedValue == "1" &&
            (ddlClase.SelectedValue == "PE" ||
            ddlClase.SelectedValue == "PA" ||
            ddlClase.SelectedValue == "HS" ||
            ddlClase.SelectedValue == "FC" ||
            ddlClase.SelectedValue == "AA" ||
            ddlClase.SelectedValue == "CO"));
    }
    private bool fnDivisionRequerida()
    {
        return ddlNumSociedad.SelectedValue == "S003";
    }
    protected void btnAgrServicio_Click(object sender, EventArgs e)
    {
        string numServicio = txtNumServicio.Text;
        if (!string.IsNullOrEmpty(numServicio))
        {
            if (!numServicio.Length.Equals(10))
            {
                clsComun.fnMessage(this, "El numero de pedido debe ser igual a 10 dígitos", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return;
            }

            try
            {
                long nnumServicio = Convert.ToInt64(numServicio);
            }
            catch (Exception ex)
            {
                clsComun.fnMessage(this, "Debe ser un valor numérico", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                return;
            }
            if (trvServicios.Nodes.Count < 20)
            {
                TreeNode tnServicio = new TreeNode(numServicio);
                trvServicios.Nodes.Add(tnServicio);
                txtNumServicio.Text = string.Empty;
                trvServicios.ExpandAll();
            }
            else
            {
                clsComun.fnMessage(this, "No debe exceder de 20 pedidos", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
            }
        }
    }
    protected void btnRemServicio_Click(object sender, EventArgs e)
    {
        if (trvServicios.SelectedNode != null)
        {
            if (trvServicios.SelectedNode.Parent == null)
            {
                trvServicios.Nodes.Remove(trvServicios.SelectedNode);
                trvServicios.ExpandAll();
            }
        }
    }
    protected void btnAgrRecepcion_Click(object sender, EventArgs e)
    {
        if (trvServicios.SelectedNode != null)
        {
            string numRecepcion = txtNumRecepcion.Text;
            if (!string.IsNullOrEmpty(numRecepcion))
            {
                if (trvServicios.SelectedNode.Parent == null)
                {
                    if (!numRecepcion.Length.Equals(10))
                    {
                        clsComun.fnMessage(this, "El numero de recepción debe ser igual a 10 dígitos", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                        return;
                    }

                    try
                    {
                        long nnumRecepcion = Convert.ToInt64(numRecepcion);
                    }
                    catch (Exception ex)
                    {
                        clsComun.fnMessage(this, "Debe ser un valor numérico", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                        return;
                    }
                    if (trvServicios.SelectedNode.ChildNodes.Count < 20)
                    {
                        trvServicios.SelectedNode.ChildNodes.Add(new TreeNode(numRecepcion));
                        txtNumRecepcion.Text = string.Empty;
                        trvServicios.ExpandAll();
                    }
                    else
                    {
                        clsComun.fnMessage(this, "No debe exceder de 20 recepciones por pedido", Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                    }
                }
            }
        }
    }
    protected void btnRemRecepcion_Click(object sender, EventArgs e)
    {
        if (trvServicios.SelectedNode != null)
        {
            if (trvServicios.SelectedNode.Parent != null)
            {
                trvServicios.SelectedNode.Parent.ChildNodes.Remove(trvServicios.SelectedNode);
                //trvServicios.Nodes.Remove(trvServicios.SelectedNode);
                trvServicios.ExpandAll();
            }
        }
    }
    protected void btnAgrAnexo_Click(object sender, EventArgs e)
    {
        string anexo = txtAnexo.Text;
        if (!string.IsNullOrEmpty(anexo))
        {
            lsbAnexos.Items.Add(new ListItem(anexo));
            txtAnexo.Text = string.Empty;
        }
    }
    protected void btnQuitarAnexo_Click(object sender, EventArgs e)
    {
        if (lsbAnexos.SelectedIndex >= 0)
        {
            lsbAnexos.Items.RemoveAt(lsbAnexos.SelectedIndex);
        }
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            XmlDocument xmladenda = new XmlDocument();
            string sArmXML = fnArmarXml();
            if (!(string.IsNullOrEmpty(sArmXML)))
            {
                xmladenda.LoadXml(sArmXML);
                Session["AddendaGenerada"] = xmladenda.OuterXml;
                clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varAlta, Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                btnGuardar.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "btnGuardar_Click", "webAddendaAHMSA");
        }
    }
    protected void btnCerrar_Click(object sender, EventArgs e)
    {

    }
    protected void ddlNumSociedad_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlNumDivision.Enabled = fnDivisionRequerida();

    }
    protected void ddlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtNumServicio.Enabled = true;

        if (fnServicioNoAplica())
            txtNumServicio.Enabled = false;
    }
    protected void ddlClase_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtNumServicio.Enabled = true;

        if (fnServicioNoAplica())
            txtNumServicio.Enabled = false;
    }
}