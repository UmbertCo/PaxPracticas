using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Resources;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Timbrado_webTimbradoGeneracion : System.Web.UI.Page
{
    public string SeleccioneUnValor = "(Seleccione un valor)";
    private clsOperacionTimbradoSellado gTimbrado;
    private clsValCertificado gCertificado;
    protected DataTable tablePas;
    protected DataTable dtCreditos;
    clsInicioSesionUsuario datosUsuario;
    public DataTable TablaComplementos;
    private clsOperacionDocImpuestos gDAL;
    private string ForDec;
    private clsOperacionCuenta gOpeCuenta;
    private clsEnvioCorreoDocs cEd;
    private clsOperacionSucursales gOpeSucursal;

    DataTable dtArticulos;
    private static DataSet creditosT;
    private clsConfiguracionCreditos cCc;
    protected DataTable dtCompl;
    protected DataTable dtInfoAduanal;
    protected DataTable dtComplTerceros;


    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            if (datosUsuario == null)
                return;

            //ddlAddenda.Visible = false;
            //btnAdenda.Visible = false;
            //Carga objetos.
            fnCargarSucursalesExpEn();
            fnCargarSucursalesFiscal();
            fnReiniciarDatosReceptores();
            fnCargarTipoDocumentosGen();
            fnCargarSeries();
            fnCargarFolio();
            fnCargarPaises();
            fnCargarPaisesLugExp();
            fnCargarEstados(ddlPaisExpEn.SelectedValue);
            fnCargarEstadosLugExp(ddlPaisLugExp.SelectedValue);
            ddlMetodoPago_SelectedIndexChanged(sender, e);
            ddlIEPS_SelectedIndexChanged(sender, e);

            fLimCamposPagParciales();
            fCargarImpuestosIva(); //Carga combo de iva
            //fCargarImpuestosIEPS(); //Carga combo de IEPS
            fnCargaModuloAddendas();

            //datosUsuario.version = "3.2";
            //Habilitar y deshabilitar controles segun la version
            Session["AddendaGenerada"] = null;
            Session["TablaComplementos"] = null;
            Session["TipoDonativo"] = null;
            switch (datosUsuario.version)
            {
                case "3.0":
                    DesHabilitarCamposVersion3_2();

                    break;
                case "3.2":
                    HabilitarCamposVersion3_2();
                    if (!(ddlSucursales.SelectedValue == ""))
                        ddlSucursales_SelectedIndexChanged(sender, e); //fnObtenerDatoEmisor(sender, e);

                    break;
            }
            //Llena campos fiscalesgr
            ddlSucursalesFis_SelectedIndexChanged(sender, e);
            ddlMetodoPago_SelectedIndexChanged(sender, e);

            gOpeCuenta = new clsOperacionCuenta();

            DataTable sdrInfo = gOpeCuenta.fnObtenerDatosFiscales();

            if (sdrInfo != null && sdrInfo.Rows.Count > 0)
            {
                txtRegimenfiscal.Text = sdrInfo.Rows[0]["regimen_fiscal"].ToString();
                ViewState["rfc_Emisor"] = sdrInfo.Rows[0]["rfc"].ToString();
                ViewState["razonSocial_Emisor"] = sdrInfo.Rows[0]["razon_social"].ToString();
            }

            rdlArchivo.SelectedIndex = 0;

            //Ocultar panel Expedido En
            cpeExpedidoEn.Collapsed = true;
            cpeExpedidoEn.ClientState = "true";
            Image1.Attributes.Add("style", "visibility:hidden;");
            //Ocultar panel Impuestos Locales
            fnOcultarPnlImpuestosCompl();

            //Oculta panel Información Aduanal
            fnOcualtarPnlInfoAduanal();

            //Oculta panel complemento terceros
            fnOcultarPnlComplTerceros();

            //Oculta panel IEPS
            fnOcultarPnlIEPS();

            txtFecha_CalendarExtender.SelectedDate = DateTime.Now;
            txtFecha_CalendarExtender_Adu.SelectedDate = DateTime.Now;
            txtFechaIniT_CalendarExtender.SelectedDate = DateTime.Now;

            ViewState["GuidPathXMLsGen"] = Guid.NewGuid().ToString();
            ViewState["GuidPathZIPsGen"] = Guid.NewGuid().ToString();

            ViewState["nombreDoc"] = string.Empty;
            ViewState["retornoInsert"] = string.Empty;

            AsyncPostBackTrigger Trigger = new AsyncPostBackTrigger();
            //txtPrecioCon.Text = "0";
            //txtCantidadCon.Text = "0";           
            //txtImporteCon.Text = "0";
            fLimpiarCamposArt();
            //txtIEPScon.Text = "0";
            //ControlID = ID del control que provoca el evento.

            //  Trigger.ControlID = BotnCancelar.ID;
            //EventName = Nombre del evento, p.e: Click, SelectedIndexChange.
            //Trigger.EventName = "Click";
            ////Se añade el trigger al update panel
            //updGuardar.Triggers.Add(Trigger);


            //TabContainer1.ActiveTabIndex = 0;
            grvDetalles.DataSource = null;
            grvDetalles.DataBind();

            clsComun.fnPonerTitulo(this);

            //fnCargarEfectos();

            //Crea tabla para Complemento Impuestos Locales
            //*********************************************
            DataTable dtImpCompl = new DataTable();
            DataColumn[] keysImp = new DataColumn[1];

            //Crear y agregar columnas
            DataColumn colImp = new DataColumn();
            colImp.DataType = System.Type.GetType("System.Int32");
            colImp.ColumnName = "id_Impuesto";
            colImp.AutoIncrement = true;
            colImp.AutoIncrementSeed = 1;
            colImp.AutoIncrementStep = 1;
            dtImpCompl.Columns.Add(colImp);

            keysImp[0] = colImp;
            dtImpCompl.PrimaryKey = keysImp;

            dtImpCompl.Columns.Add("Descripcion");
            dtImpCompl.Columns.Add("Tasa", typeof(double));
            dtImpCompl.Columns.Add("Importe", typeof(double));
            dtImpCompl.Columns.Add("Tipo");
            dtImpCompl.Columns.Add("ConImporte", typeof(bool));
            dtImpCompl.Columns.Add("id_registros", typeof(int));

            ViewState["dtImpCompl"] = dtImpCompl;
            ViewState["dtImpComplGen"] = dtImpCompl;
            ViewState["id_Impuesto"] = string.Empty;
            //*********************************************

            //Crea tabla para informacón Aduanal
            //**************************************************************
            DataTable dtInfAduanal = new DataTable();
            DataColumn[] KeysInfoAduanal = new DataColumn[1];

            //Creamos las columnas y las agregamos
            DataColumn colInfAduanal = new DataColumn();
            colInfAduanal.DataType = System.Type.GetType("System.Int32");
            colInfAduanal.ColumnName = "id_aduana";
            colInfAduanal.AutoIncrement = true;
            colInfAduanal.AutoIncrementSeed = 1;
            colInfAduanal.AutoIncrementStep = 1;
            dtInfAduanal.Columns.Add(colInfAduanal);

            KeysInfoAduanal[0] = colInfAduanal;
            dtInfAduanal.PrimaryKey = KeysInfoAduanal;

            dtInfAduanal.Columns.Add("Aduana");
            dtInfAduanal.Columns.Add("DocAduana");
            dtInfAduanal.Columns.Add("FechaAduana", typeof(DateTime));
            dtInfAduanal.Columns.Add("id_registros", typeof(int));

            ViewState["dtInfAduanal"] = dtInfAduanal;
            ViewState["id_aduana"] = string.Empty;

            ////Fin Tabla Información Aduanal
            //****************************************************************

            /////////////////////////////////////////////////////////////

            //Crea tabla para Complemento terceros
            //**************************************************************
            DataTable dtComplTerceros = new DataTable();
            DataColumn[] KeysComplTerceros = new DataColumn[1];

            //Creamos las columnas y las agregamos
            DataColumn colComplTerceros = new DataColumn();
            colComplTerceros.DataType = System.Type.GetType("System.Int32");
            colComplTerceros.ColumnName = "id_complTerceros";
            colComplTerceros.AutoIncrement = true;
            colComplTerceros.AutoIncrementSeed = 1;
            colComplTerceros.AutoIncrementStep = 1;
            dtComplTerceros.Columns.Add(colComplTerceros);

            KeysComplTerceros[0] = colComplTerceros;
            dtComplTerceros.PrimaryKey = KeysComplTerceros;

            dtComplTerceros.Columns.Add("version");
            dtComplTerceros.Columns.Add("rfc");
            dtComplTerceros.Columns.Add("nombre");
            //Impuestos
            //Impuestos retención
            dtComplTerceros.Columns.Add("impuestoRetIVA");
            dtComplTerceros.Columns.Add("importeRetIVA", typeof(double));
            dtComplTerceros.Columns.Add("impuestoRetISR");
            dtComplTerceros.Columns.Add("importeRetISR", typeof(double));
            //Impuestos traslado
            dtComplTerceros.Columns.Add("impuestoTrasIVA");
            dtComplTerceros.Columns.Add("tasaTrasIVA", typeof(double));
            dtComplTerceros.Columns.Add("importeTrasIVA", typeof(double));
            dtComplTerceros.Columns.Add("impuestoTrasIEPS");
            dtComplTerceros.Columns.Add("tasaTrasIEPS", typeof(double));
            dtComplTerceros.Columns.Add("importeTrasIEPS", typeof(double));
            //Información fiscal terceros
            dtComplTerceros.Columns.Add("calle");
            dtComplTerceros.Columns.Add("noExterior");
            dtComplTerceros.Columns.Add("noInterior");
            dtComplTerceros.Columns.Add("colonia");
            dtComplTerceros.Columns.Add("localidad");
            dtComplTerceros.Columns.Add("referencia");
            dtComplTerceros.Columns.Add("municipio");
            dtComplTerceros.Columns.Add("estado");
            dtComplTerceros.Columns.Add("pais");
            dtComplTerceros.Columns.Add("codigoPostal");
            //Información aduanera
            dtComplTerceros.Columns.Add("numeroInfoAd");
            dtComplTerceros.Columns.Add("fechaInfoAd", typeof(DateTime));
            dtComplTerceros.Columns.Add("aduanaInfoAd");
            //Cuenta Predial
            dtComplTerceros.Columns.Add("numeroCtaPred");

            dtComplTerceros.Columns.Add("id_registros", typeof(int));

            ViewState["dtComplTerceros"] = dtComplTerceros;
            ViewState["id_complTerceros"] = string.Empty;

            ////Fin Tabla Información Aduanal
            //****************************************************************

            /////////////////////////////////////////////////////////////

            //Crea la tabla que se genera en linea.
            DataTable table = new DataTable();
            DataColumn[] keys = new DataColumn[1];

            // Create column 1.
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "id_Registros";
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;

            table.Columns.Add(column);
            keys[0] = column;

            table.PrimaryKey = keys;

            table.Columns.Add("Codigo");
            table.Columns.Add("Unidad");
            table.Columns.Add("Descripcion");
            table.Columns.Add("PrecioUnitario", typeof(double));
            table.Columns.Add("Cantidad", typeof(double));
            table.Columns.Add("Importe", typeof(double));
            table.Columns.Add("descIVA");
            table.Columns.Add("IVA", typeof(double));
            table.Columns.Add("IVARet", typeof(double));
            table.Columns.Add("IEPS", typeof(double));
            table.Columns.Add("ISR", typeof(double));
            table.Columns.Add("Subtotal", typeof(double));
            table.Columns.Add("Estatus", typeof(string));
            table.Columns.Add("valISR", typeof(double));
            table.Columns.Add("valIvaRet", typeof(double));
            table.Columns.Add("valIEPS", typeof(double));
            table.Columns.Add("descIEPS"); // Almacena la cantidad de IEPS
            table.Columns.Add("trasladoIEPS"); //Almacenara si el IEPS va con o sin traslado
            table.Columns.Add("categoriaIEPS"); //Almacenara la descripcion del IEPS seleccionado del dropdown list
            table.Columns.Add("indexIEPS"); //Almacenara el indice de la lista de IEPS
            table.Columns.Add("idarticulo", typeof(Int32));
            table.Columns.Add("Descuento", typeof(double));
            table.Columns.Add("PorcentajeDescuento", typeof(double));
            table.Columns.Add("IEPSConImporte", typeof(bool));
            //table.Columns.Add("ISH", typeof(double));
            //table.Columns.Add("valISH", typeof(double));
            //table.Columns.Add("CNG", typeof(double));
            //table.Columns.Add("valCNG", typeof(double));
            /////////////////////////////////////////////////////////////

            ViewState["table"] = table;
            ViewState["id_Registros"] = string.Empty;
            //ViewState["Impuestos"] = string.Empty;
            Session["psIdArticulo"] = string.Empty;
            ViewState["id_RegistroAdu"] = string.Empty;
            ViewState["TipoDescuento"] = string.Empty;
            ViewState["fechafolioFiscarlOrig"] = string.Empty;

            //fdtCrearDTImportes();

            //Agregar funcionalidad a controles.
            //txtCantidadCon.Attributes.Add("onchange", "return calcularCantidadCpto();");
            //txtPrecioCon.Attributes.Add("onchange", "return calcularPrecioCpto();");
            txtPrecioArt.Attributes.Add("onchange", "return calcularCantidadArt();");
            txtCantidadArt.Attributes.Add("onchange", "return calcularCantidadArt();");

            //Verificamos si hemos regresado de la búsqueda de receptores
            if (Session["busquedaReceptor"] != null && !string.IsNullOrEmpty(Session["busquedaReceptor"].ToString()))
            {
                //recobramos el valor y borramos la sesion
                string sParametros = Session["busquedaReceptor"].ToString();
                Session.Remove("busquedaReceptor");

                fnRealizarBusquedaDatosReceptor(sParametros);
            }
            //Revisar los creditos disponibles.
            cCc = new clsConfiguracionCreditos();
            double dCostCred = 0;
            dCostCred = cCc.fnRecuperaPrecioServicio(4); //Precio servicio Generación + Timbrado
            datosUsuario = clsComun.fnUsuarioEnSesion();
            dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');
            ViewState["dtCreditos"] = dtCreditos;
            if (dtCreditos.Rows.Count > 0) //Valida créditos de usuario
            {
                double creditos = 0;
                creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                if (creditos == 0)
                {
                    clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    ViewState["dtCreditos"] = dtCreditos;
                    double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                    if (creditos2 == 0)
                    {
                        Label121.Visible = false;
                        lblCosCre.Visible = false;
                        Label7.Visible = true;
                        modalCreditos.Show();
                    }
                    else
                    {
                        //Se valida que tenga créditos suficientes
                        if (creditos2 < dCostCred)
                        {
                            Label121.Visible = false;
                            lblCosCre.Visible = true;
                            Label7.Visible = false;
                            modalCreditos.Show();
                        }
                    }
                }
            }
            else //Valida créditos de distribuidor
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                ViewState["dtCreditos"] = dtCreditos;
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                if (creditos == 0)
                {
                    Label121.Visible = false;
                    lblCosCre.Visible = false;
                    Label7.Visible = true;
                    modalCreditos.Show();
                }
                else
                {
                    //Se valida que tenga créditos suficientes
                    if (creditos < dCostCred)
                    {
                        Label121.Visible = false;
                        lblCosCre.Visible = true;
                        Label7.Visible = false;
                        modalCreditos.Show();

                    }
                }
            }

            fnActualizarLblCreditos();

            //Verificamos que ese usuario cuente con el servicio de generación y timbrado
            clsConfiguracionPlantilla configura = new clsConfiguracionPlantilla();
            int idEstrserv = configura.fnRecuperaEstructura(datosUsuario.id_usuario);

            creditosT = fnRecuperaCreditosusuario(datosUsuario.userName);

            DataTable tblServicios = new DataTable();
            tblServicios = creditosT.Tables[2];
            bool TieneGeneracion = false;
            foreach (DataRow renglon in tblServicios.Rows)
            {

                string sDescripcion = Convert.ToString(renglon["descripcion"]);
                if (sDescripcion == "Generación y Timbrado")
                {
                    TieneGeneracion = true;
                }
            }

            if (TieneGeneracion == false)
            {
                Label121.Visible = true;
                lblCosCre.Visible = false;
                Label7.Visible = false;
                modalCreditos.Show();
            }

            cbAgrExpEn.Checked = false; //Esconder panel expedido en
            cbAgrExpEn_CheckedChanged(sender, e);

            fnLimpiarInfoFiscalTerceros();
            fnLimpiarImpuestosTerceros();
        }
        else
        {
            fnCargarDatosSucursal();
        }
    }

    private void HabilitarCamposVersion3_2()
    {
        tblVersion32.Visible = true;
        //Renglon tabla Datos fiscales
        pnlLugExp.Visible = true;
        cbOtraUbi.Visible = true;
        RenlblPaisEmi.Visible = true; //lbl Pais
        RenddlPaisEmi.Visible = true; //ddl Pais
        RenlblEdoEmi.Visible = true; //lbl Estado
        RenddlEdoEmi.Visible = true; //ddl Estado
        RenlblMpoEmi.Visible = true; //lbl Municipio
        RentxtMpoEmi.Visible = true; //txt Municipio
        RenlblLocalEmi.Visible = true; //lbl Localidad
        RentxtLocalEmi.Visible = true; //txt Localidad

        //Validadores
        //RequiredFieldValidator1.EnableClientScript = true; //Lugar de expedicion
        rfvCodigo2.EnableClientScript = true; //regimen fiscal
        rfvCodigo1.EnableClientScript = true; //tipo de cambio
        rfvPaisExpEn.EnableClientScript = true; //Pais emisor
        rfvEstadoExpEn.EnableClientScript = true; //Estado emisor
        rfvMpoExpEn.EnableClientScript = true; //Municipio emisor
        rfvCalleExpEn.EnableClientScript = true; //Calle emisor
        rfvCPExpEn.EnableClientScript = true; //Codigo Postal emisor
        //Campos
        lblTipoCambio.Visible = true;
        txttipoCambio.Visible = true;
        lblPais0.Visible = true;
        ddlPaisExpEn.Visible = true;
        lblEstado0.Visible = true;
        ddlEstadoExpEn.Visible = true;
        lblMunicipio0.Visible = true;
        txtMunicipioExpEn.Visible = true;
        lblCalle0.Visible = true;
        txtCalleExpEn.Visible = true;
        LablblCodigoPostalel6.Visible = true;
        txtCodigoPostalExpEn.Visible = true;

        ListItemComp3_2();

        cbAgrExpEn.Visible = true;
        pnlExpedidoEn.Visible = true;
        pnlExtender.Visible = true;
    }

    private void ListItemComp3_0()
    {
        ddlComplemento.Items.Clear();

        ListItem[] arr = new ListItem[8]; //If u put arr() in Place of arr() or if i put new keyword it giving me probs.

        arr[0] = new ListItem();
        arr[0].Text = "Seleccione Opción";
        arr[0].Value = "0";
        arr[1] = new ListItem();
        arr[1].Text = "Detallista";
        arr[1].Value = "1";
        arr[2] = new ListItem();
        arr[2].Text = "Divisas";
        arr[2].Value = "2";
        arr[3] = new ListItem();
        arr[3].Text = "Donativas";
        arr[3].Value = "3";
        arr[4] = new ListItem();
        arr[4].Text = "Cuentas Bancarias";
        arr[4].Value = "4";
        arr[5] = new ListItem();
        arr[5].Text = "Combustible";
        arr[5].Value = "5";
        arr[6] = new ListItem();
        arr[6].Text = "Instituciones Educativas";
        arr[6].Value = "6";
        //arr[7] = new ListItem();
        //arr[7].Text = "Otros Derechos e Impuestos";
        //arr[7].Value = "7";
        arr[7] = new ListItem();
        arr[7].Text = "PSGCFDSP";
        arr[7].Value = "8";
        ddlComplemento.Items.AddRange(arr);

    }

    private void ListItemComp3_2()
    {
        clsOperacionCuenta clsCuenta = new clsOperacionCuenta();
        ddlComplemento.DataValueField = "ruta";
        ddlComplemento.DataTextField = "Complemento";
        ddlComplemento.DataSource = clsCuenta.fnCargarComplementos();
        ddlComplemento.DataBind();

        //ddlComplemento.Items.Clear();

        ListItem[] arr = new ListItem[1]; //If u put arr() in Place of arr() or if i put new keyword it giving me probs.

        arr[0] = new ListItem();
        arr[0].Text = "Seleccione Opción";
        arr[0].Value = "0";
        //arr[1] = new ListItem();
        //arr[1].Text = "Detallista";
        //arr[1].Value = "1";
        //arr[2] = new ListItem();
        //arr[2].Text = "Divisas";
        //arr[2].Value = "2";
        //arr[3] = new ListItem();
        //arr[3].Text = "Donativas";
        //arr[3].Value = "3";
        //arr[4] = new ListItem();
        //arr[4].Text = "Cuentas Bancarias";
        //arr[4].Value = "4";
        //arr[5] = new ListItem();
        //arr[5].Text = "Combustible";
        //arr[5].Value = "5";
        //arr[6] = new ListItem();
        //arr[6].Text = "Instituciones Educativas";
        //arr[6].Value = "6";
        ////arr[7] = new ListItem();
        ////arr[7].Text = "Otros Derechos e Impuestos";
        ////arr[7].Value = "7";
        //arr[7] = new ListItem();
        //arr[7].Text = "PSGCFDSP";
        //arr[7].Value = "8";
        //arr[8] = new ListItem();
        //arr[8].Text = "Leyendas Fiscales";
        //arr[8].Value = "9";
        //arr[9] = new ListItem();
        //arr[9].Text = "PFintegranteCoordinado";
        //arr[9].Value = "10";
        //arr[10] = new ListItem();
        //arr[10].Text = "Terceros";
        //arr[10].Value = "11";
        //arr[11] = new ListItem();
        //arr[11].Text = "TuristaPasajeroExtranjero";
        //arr[11].Value = "12";
        //arr[12] = new ListItem();
        //arr[12].Text = "Venta Vehicular";
        //arr[12].Value = "13";
        ddlComplemento.Items.AddRange(arr);
        ddlComplemento.SelectedValue = "0";
    }

    private void DesHabilitarCamposVersion3_2()
    {
        tblVersion32.Visible = false;
        //Renglon tabla Datos fiscales
        pnlLugExp.Visible = false;
        cbOtraUbi.Visible = false;
        RenlblPaisEmi.Visible = false; //lbl Pais
        RenddlPaisEmi.Visible = false; //ddl Pais
        RenlblEdoEmi.Visible = false; //lbl Estado
        RenddlEdoEmi.Visible = false; //ddl Estado
        RenlblMpoEmi.Visible = false; //lbl Municipio
        RentxtMpoEmi.Visible = false; //txt Municipio
        RenlblLocalEmi.Visible = false; //lbl Localidad
        RentxtLocalEmi.Visible = false; //txt Localidad

        //Validadores
        //RequiredFieldValidator1.EnableClientScript = false; //Lugar de expedicion
        rfvCodigo2.EnableClientScript = false; //regimen fiscal
        rfvCodigo1.EnableClientScript = false; //tipo de cambio
        rfvPaisExpEn.EnableClientScript = false; //Pais emisor
        rfvEstadoExpEn.EnableClientScript = false; //Estado emisor
        rfvMpoExpEn.EnableClientScript = false; //Municipio emisor
        rfvCalleExpEn.EnableClientScript = false; //Calle emisor
        rfvCPExpEn.EnableClientScript = false; //Codigo Postal emisor

        //Campos
        lblTipoCambio.Visible = false;
        txttipoCambio.Visible = false;
        lblPais0.Visible = false;
        ddlPaisExpEn.Visible = false;
        lblEstado0.Visible = false;
        ddlEstadoExpEn.Visible = false;
        lblMunicipio0.Visible = false;
        txtMunicipioExpEn.Visible = false;
        lblCalle0.Visible = false;
        txtCalleExpEn.Visible = false;
        LablblCodigoPostalel6.Visible = false;
        txtCodigoPostalExpEn.Visible = false;

        ListItemComp3_0();

        cbAgrExpEn.Visible = false;
        pnlExpedidoEn.Visible = false;
        pnlExtender.Visible = false;
    }

    protected void ddlRfc_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Funcion para recargar los impuestos
        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ddlRfc" + "|" + "Selecciona un rfc del receptor." + "|" + txtRfcReceptor.Text);

    }

    protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Funcion para recargar las sucursales.
        fnCargarDetallesReceptorSuc();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ddlSucursal" + "|" + "Selecciona una sucursal del receptor." + "|" + ddlSucursal.SelectedValue);

    }

    protected void grvDetalles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        e.Cancel = true;

        tablePas = (DataTable)ViewState["table"];

        //Si existe impuesto complemento agregado
        dtCompl = (DataTable)ViewState["dtImpCompl"];
        if (dtCompl.Rows.Count > 0)
        {
            //Se filtra por el id registro en tabla de complemento
            DataView dv = new DataView(dtCompl);
            dv.RowFilter = "id_registros = " + tablePas.Rows[e.RowIndex]["id_registros"].ToString();
            int i = 0;
            int r = dv.Count;
            //Se elimina registros correspondientes en tabla de complemento
            for (i = 0; i < r; i++)
            {
                dv.Delete(0);
            }
        }

        //Si existe complemento terceros agregado
        dtComplTerceros = (DataTable)ViewState["dtComplTerceros"];
        if (dtComplTerceros.Rows.Count > 0)
        {
            //Se filtra por el id registro en tabla de complemento terceros
            DataView dvComplTerceros = new DataView(dtComplTerceros);
            dvComplTerceros.RowFilter = "id_registros = " + tablePas.Rows[e.RowIndex]["id_registros"].ToString();

            dvComplTerceros.Delete(0);
            //int i = 0;
            //int r = dvComplTerceros.Count;
            ////Se elimina registros correspondientes en tabla de complemento terceros
            //for (i = 0; i < r; i++)
            //{
            //    dvComplTerceros.Delete(0);
            //}
            //Se elimina registro correspondiente en tabla de complemento terceros
            grvComplTerceros.DataSource = string.Empty;
            grvComplTerceros.DataBind();
        }

        tablePas.Rows.RemoveAt(e.RowIndex);
        ViewState["table"] = tablePas;
        grvDetalles.DataSource = tablePas;
        grvDetalles.DataBind();


        //Obtener Subtotal
        lblDetSubtotal.Text = String.Format("{0:c6}", tablePas.Compute("SUM(Importe)", ""));

        if (Convert.ToDouble(txtDescuentoGlobal.Text).Equals(0))
        {
            lblDetDescuentoVal.Text = String.Format(ForDec, tablePas.Compute("SUM(Descuento)", ""));
            if (!Convert.ToDouble(txtDescuentoArt.Text).Equals(0))
            {
                txtDescuentoGlobal.Enabled = false;
                chkDescuentoGlobal.Enabled = false;
            }
        }
        else if (!Convert.ToDouble(txtDescuentoGlobal.Text).Equals(0))
        {
            if (chkDescuentoGlobal.Checked)
            {
                if (!lblDetSubtotal.Text.Equals(string.Empty))
                    lblDetDescuentoVal.Text = String.Format(ForDec, (Convert.ToDouble(lblDetSubtotal.Text.Replace("$", "").Replace(",", "")) * Convert.ToDouble(txtDescuentoGlobal.Text)) / 100);
            }
            else
                lblDetDescuentoVal.Text = String.Format(ForDec, Convert.ToDouble(txtDescuentoGlobal.Text));

            txtDescuentoGlobal.Enabled = false;
            chkDescuentoGlobal.Enabled = false;
        }
        else
        {

        }

        //Funcion para recargar los impuestos
        //fnRecargaTipoImpuestos();
        fnRecargaTipoImpuestosArticulos();

        if (grvDetalles.Rows.Count <= 0)
        {
            lblSubtotal.Visible = false;
            lblDetSubtotal.Visible = false;

            lblDetDescuento.Visible = false;
            lblDetDescuentoVal.Visible = false;

            dtsTotales.Visible = false;
            lblTotal.Visible = false;
            lblTotalVal.Visible = false;
            lblNumerosLetras.Text = string.Empty;

            txtDescuentoGlobal.Enabled = true;
            txtDescuentoGlobal.Text = "0";
            chkDescuentoGlobal.Enabled = true;
            chkDescuentoGlobal.Checked = false;
            txtDescuentoArt.Enabled = true;
        }
    }

    //protected void btnAgregar_Click(object sender, EventArgs e)
    //{

    //    DataRow findRow;

    //    tablePas = (DataTable)ViewState["table"];

    //    if (txtCodigoCon.Text != string.Empty &&
    //            txtUnidadCon.Text != string.Empty &&
    //            txtDescripcionCon.Text != string.Empty &&
    //            clsComun.fnIsDouble(txtPrecioCon.Text) == true)
    //    {


    //        if (ddlTipoDoc.SelectedIndex  == -1)
    //        {
    //            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varExDocumento, Resources.resCorpusCFDIEs.varContribuyente);
    //            return;
    //        }

    //        if ((Convert.ToDouble(txtPrecioCon.Text) * Convert.ToDouble(txtCantidadCon.Text)) == 0)
    //        {
    //            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varImporteCero, Resources.resCorpusCFDIEs.varContribuyente);
    //            return;
    //        }
    //        if (txtDescuentoCon.Text == string.Empty)
    //        {
    //            txtDescuentoCon.Text = "0";
    //        }
    //        fnCalcularImpuestoCpto();

    //        //Revisar si existe.
    //        if (ViewState["id_Registros"].ToString() != string.Empty)
    //        {


    //            findRow = tablePas.Rows.Find(ViewState["id_Registros"].ToString());

    //            if (findRow != null)
    //            {
    //                findRow["Codigo"] = txtCodigoCon.Text;
    //                findRow["Unidad"] = txtUnidadCon.Text;
    //                findRow["Descripcion"] = txtDescripcionCon.Text;
    //                findRow["PrecioUnitario"] = txtPrecioCon.Text;
    //                findRow["Cantidad"] = txtCantidadCon.Text;
    //                findRow["Importe"] = Convert.ToDouble(txtImporteCon.Text);//Convert.ToDouble(txtImporteCon.Text) - Convert.ToDouble(txtDescuentoCon.Text);

    //                double Porcentaje = Convert.ToDouble(txtPrecioCon.Text) * (Convert.ToDouble(txtDescuentoCon.Text) / 100);

    //                double Precio = Convert.ToDouble(txtPrecioCon.Text) - Porcentaje;

    //                findRow["Subtotal"] = Precio * Convert.ToDouble(txtCantidadCon.Text);
    //                findRow["descIVA"] = txtIVACon.SelectedItem.Text;
    //                findRow["IVA"] = (Convert.ToDouble(txtCantidadCon.Text) * Precio) * Convert.ToDouble(txtIVACon.SelectedValue) / 100;
    //                if (txtIvaRet.Text != string.Empty)
    //                {
    //                    if (Convert.ToDecimal(txtIvaRet.Text) != 0) //if (cbIVACon.Checked == true)
    //                    {                           
    //                        findRow["IVARet"] = (Convert.ToDouble(txtCantidadCon.Text) * Precio) * Convert.ToDouble(txtIvaRet.Text) / 100;//(Convert.ToDouble(txtIVACon.SelectedValue) * 2 / 3) / 100;
    //                        findRow["valIvaRet"] = Convert.ToDouble(txtIvaRet.Text); //Carga valor IVA Retencion
    //                    }
    //                    else
    //                    {
    //                        findRow["IVARet"] = 0;
    //                        findRow["valIvaRet"] = 0; //Carga valor IVA Retencion
    //                    }
    //                }
    //                else
    //                {
    //                    findRow["IVARet"] = 0;
    //                    findRow["valIvaRet"] = 0; //Carga valor IVA Retencion
    //                }

    //                findRow["IEPS"] = (Convert.ToDouble(txtCantidadCon.Text) * Precio) * Convert.ToDouble(txtIEPScon.Text) / 100;
    //                if (txtIEPScon.Text != string.Empty)
    //                {
    //                    findRow["valIEPS"] = txtIEPScon.Text; //Carga valor IEPS
    //                }
    //                else
    //                {
    //                    findRow["valIEPS"] = 0;
    //                }

    //                if(txtISRcpto.Text != string.Empty)
    //                {
    //                    if (Convert.ToDecimal(txtISRcpto.Text) != 0)//if (cbISRCon.Checked == true)
    //                    {
    //                        findRow["ISR"] = (Convert.ToDouble(txtCantidadCon.Text) * Precio) * Convert.ToDouble(txtISRcpto.Text) / 100;//Convert.ToDouble(clsComun.ObtenerParamentro("ISR")) / 100;
    //                        findRow["valISR"] = txtISRcpto.Text;//Carga valor ISR concepto
    //                    }
    //                    else
    //                    {
    //                        findRow["ISR"] = 0;
    //                        findRow["valISR"] = 0;//Carga valor ISR concepto
    //                    }
    //                }
    //                else
    //                {
    //                    findRow["ISR"] = 0;
    //                    findRow["valISR"] = 0;//Carga valor ISR concepto
    //                }

    //                findRow["Estatus"] = "C";

    //            }

    //            ViewState["id_Registros"] = string.Empty;
    //        }

    //        else
    //        {
    //            DataRow row = tablePas.NewRow();

    //            row["Codigo"] = txtCodigoCon.Text;
    //            row["Unidad"] = txtUnidadCon.Text;
    //            row["Descripcion"] = txtDescripcionCon.Text;
    //            row["PrecioUnitario"] = txtPrecioCon.Text;

    //            double Porcentaje = Convert.ToDouble(txtPrecioCon.Text) * (Convert.ToDouble(txtDescuentoCon.Text) / 100);
    //            double Precio = Convert.ToDouble(txtPrecioCon.Text) - Porcentaje;

    //            row["Cantidad"] = txtCantidadCon.Text;
    //            row["Importe"] = Convert.ToDouble(txtImporteCon.Text);
    //            row["Subtotal"] = Precio * Convert.ToDouble(txtCantidadCon.Text);
    //            row["descIVA"] = txtIVACon.SelectedItem.Text;
    //            row["IVA"] = (Convert.ToDouble(txtCantidadCon.Text) * Precio) * (Convert.ToDouble(txtIVACon.Text) / 100);
    //            if (txtIvaRet.Text != string.Empty)
    //            {
    //                if (Convert.ToDecimal(txtIvaRet.Text) != 0)//if (cbIVACon.Checked == true)
    //                {
    //                    row["IVARet"] = (Convert.ToDouble(txtCantidadCon.Text) * Precio) * Convert.ToDouble(txtIvaRet.Text) / 100;//(Convert.ToDouble(txtIVACon.SelectedValue) * 2 / 3) / 100;
    //                    row["valIvaRet"] = Convert.ToDouble(txtIvaRet.Text); //Carga valor IVA Retencion
    //                }
    //                else
    //                {
    //                    row["IVARet"] = 0;
    //                    row["valIvaRet"] = 0; //Carga valor IVA Retencio
    //                }
    //            }
    //            else
    //            {
    //                row["IVARet"] = 0;
    //                row["valIvaRet"] = 0; //Carga valor IVA Retencio
    //            }

    //            row["IEPS"] = (Convert.ToDouble(txtCantidadCon.Text) * Precio) * (Convert.ToDouble(txtIEPScon.Text) / 100);
    //            if (txtIEPScon.Text != string.Empty)
    //            {
    //                row["valIEPS"] = txtIEPScon.Text; //Carga valor IEPS
    //            }
    //            else
    //            {
    //                row["valIEPS"] = 0;
    //            }

    //            if(txtISRcpto.Text != string.Empty)
    //            {
    //                if (Convert.ToDecimal(txtISRcpto.Text) != 0)//if (cbISRCon.Checked == true)
    //                {
    //                    row["ISR"] = (Convert.ToDouble(txtCantidadCon.Text) * Precio) * Convert.ToDouble(txtISRcpto.Text) / 100;//Convert.ToDouble(clsComun.ObtenerParamentro("ISR")) / 100;
    //                    row["valISR"] = txtISRcpto.Text;//Carga valor ISR concepto
    //                }
    //                else
    //                {
    //                    row["ISR"] = 0;
    //                    row["valISR"] = 0;//Carga valor ISR concepto
    //                }
    //            }
    //            else
    //            {
    //                row["ISR"] = 0;
    //                row["valISR"] = 0;//Carga valor ISR concepto
    //            }

    //            row["Estatus"] = "C";

    //            tablePas.Rows.Add(row);

    //            ViewState["table"] = tablePas;
    //        }

    //        datosUsuario = clsComun.fnUsuarioEnSesion();
    //        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "btnAgregar" + "|" + "Agrega un concepto nuevo." + "|" + txtCodigoCon.Text + "|" + txtUnidadCon.Text + "|" + txtDescripcionCon.Text + "|" + txtPrecioCon.Text + "|" + txtCantidadCon.Text);

    //        //Habilitar y deshabilitar controles segun la version
    //        switch (datosUsuario.version)
    //        {
    //            case "3.0":
    //                ForDec = "{0:c2}";
    //                break;
    //            case "3.2":
    //                ForDec = "{0:c6}";
    //                break;
    //        }

    //        lblSubtotal.Visible = true;
    //        lblDetSubtotal.Visible = true;
    //        //Recupera Subtotal
    //        lblDetSubtotal.Text = String.Format(ForDec, tablePas.Compute("SUM(Importe)", ""));

    //        grvDetalles.DataSource = tablePas;
    //        grvDetalles.DataBind();

    //        //Funcion para recargar los impuestos
    //        fnRecargaTipoImpuestos();

    //        fnLimpiarControles();


    //    }
    //    else
    //    {
    //        clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
    //        return;
    //    }
    //}

    protected void grvDetalles_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["id_Registros"] = grvDetalles.SelectedDataKey.Value;

        tablePas = (DataTable)ViewState["table"];
        //dtArticulos = (DataTable)ViewState["dtArticulos"];
        //DataView dvArticulos = dtArticulos.DefaultView;

        if (tablePas.Rows[grvDetalles.SelectedIndex]["Estatus"].ToString() == "C")
        {
            //txtCodigoCon.Text = tablePas.Rows[grvDetalles.SelectedIndex]["Codigo"].ToString();
            //txtUnidadCon.Text = tablePas.Rows[grvDetalles.SelectedIndex]["Unidad"].ToString();
            //txtDescripcionCon.Text = tablePas.Rows[grvDetalles.SelectedIndex]["Descripcion"].ToString();
            //txtPrecioCon.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["PrecioUnitario"]).ToString();
            //txtCantidadCon.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["Cantidad"]).ToString();
            //txtImporteCon.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["Importe"]).ToString();
            //txtISRcpto.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["valISR"]).ToString();
            //txtIvaRet.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["valIvaRet"]).ToString();
            //txtIEPScon.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["valIEPS"]).ToString();
        }
        else
        {
            Session["psIdArticulo"] = tablePas.Rows[grvDetalles.SelectedIndex]["idarticulo"].ToString();
            //int nidArticulo = Convert.ToInt32(Session["psIdArticulo"]);
            //dvArticulos.RowFilter = "id_articulo = " + nidArticulo;
            //double dprecio = Convert.ToDouble(dvArticulos[0]["precio"]);
            txtCodigoArt.Text = tablePas.Rows[grvDetalles.SelectedIndex]["Codigo"].ToString(); //ddlArticulo.SelectedValue = tablePas.Rows[grvDetalles.SelectedIndex]["Codigo"].ToString();
            txtMedidaArt.Text = tablePas.Rows[grvDetalles.SelectedIndex]["Unidad"].ToString();
            txtDescripcionArt.Text = tablePas.Rows[grvDetalles.SelectedIndex]["Descripcion"].ToString();

            //if (tablePas.Rows[grvDetalles.SelectedIndex]["trasladoIEPS"].ToString() == "Comn traslado")
            //     txtPrecioArt.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["PrecioUnitario"]).ToString();
            // else
            //     txtPrecioArt.Text = "0";

            txtCantidadArt.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["Cantidad"]).ToString();
            txtImporteArt.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["Importe"]).ToString();
            txtISRArt.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["valISR"]).ToString();
            txtIvaRetArt.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["valIvaRet"]).ToString();
            txtIEPS.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["descIEPS"]).ToString();
            ddlIEPS.SelectedIndex = Convert.ToInt32(tablePas.Rows[grvDetalles.SelectedIndex]["indexIEPS"]);
            if (ddlIEPS.SelectedIndex <= 5)
            {
                txtPrecioArt.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["PrecioUnitario"]).ToString();
            }
            else
            {
                txtPrecioArt.Text = "0";
            }

            //ddlIEPS.SelectedItem.Text = tablePas.Rows[grvDetalles.SelectedIndex]["categoriaIEPS"].ToString();
            //ddlIEPS.SelectedItem.Value = tablePas.Rows[grvDetalles.SelectedIndex]["trasladoIEPS"].ToString();
            cbIEPS.Checked = Convert.ToBoolean(tablePas.Rows[grvDetalles.SelectedIndex]["IEPSConImporte"].ToString());
            //ddlIEPSArt.SelectedValue = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["descIEPS"]).ToString();
            //ddlMonedaArt.SelectedItem.Text = tablePas.Rows[grvDetalles.SelectedIndex]["moneda"].ToString();
            ddlIVAArt.SelectedValue = tablePas.Rows[grvDetalles.SelectedIndex]["descIVA"].ToString();
            //txtISH.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["valISH"]).ToString();
            //txtCNG.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["valCNG"]).ToString();

            if (Convert.ToString(ViewState["TipoDescuento"]).Equals("PorArticulo"))
            {
                txtDescuentoArt.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["PorcentajeDescuento"]).ToString();
            }
            else if (Convert.ToString(ViewState["TipoDescuento"]).Equals("Global"))
            {
                txtDescuentoGlobal.Enabled = true;
                chkDescuentoGlobal.Enabled = true;
                //txtDescuentoGlobal.Text = Convert.ToDecimal(tablePas.Rows[grvDetalles.SelectedIndex]["PorcentajeDescuento"]).ToString();
                //chkDescuentoGlobal.Checked = Convert.ToBoolean(tablePas.Rows[grvDetalles.SelectedIndex]["EsPorcentaje"]);
            }
            else
            {
                txtDescuentoArt.Text = "0";
                txtDescuentoGlobal.Text = "0";
                txtDescuentoArt.Enabled = true;
                txtDescuentoGlobal.Enabled = true;
                chkDescuentoGlobal.Enabled = true;
                chkDescuentoGlobal.Checked = false;
            }

            //Se verifica si existe complemento impuestos agregado
            dtCompl = (DataTable)ViewState["dtImpCompl"];
            if (dtCompl.Rows.Count > 0)
            {
                //Si existen registros se filtra impuestos según id registro de concepto
                DataView dv = new DataView(dtCompl);
                dv.RowFilter = "id_registros = " + ViewState["id_Registros"].ToString();
                grvImpuestoCompl.DataSource = dv;
                grvImpuestoCompl.DataBind();
                //Muestra panel de impuesto local
                if (dv.Count > 0)
                    fnMostrarPnlImpuestosCompl();
                else//Ocultar panel de impuesto local
                    fnOcultarPnlImpuestosCompl();
            }

            //Verificamos si exite información Aduanal
            dtInfoAduanal = (DataTable)ViewState["dtInfAduanal"];
            if (dtInfoAduanal.Rows.Count > 0)
            {
                //Si Existe información se filtra según id registro concepto
                DataView dvinfo = new DataView(dtInfoAduanal);
                dvinfo.RowFilter = "id_registros= " + ViewState["id_Registros"].ToString();
                grvInfAduanal.DataSource = dvinfo;
                grvInfAduanal.DataBind();
                //Muestra panel de información Aduanal
                if (dvinfo.Count > 0)
                    fnMostrarPnlInfoAduanal();
                else
                    fnOcualtarPnlInfoAduanal();
            }

            //Verificamos si exite Complemento terceros
            dtComplTerceros = (DataTable)ViewState["dtComplTerceros"];
            if (dtComplTerceros.Rows.Count > 0)
            {
                //Si Existe información se filtra según id registro concepto
                DataView dvComplTerceros = new DataView(dtComplTerceros);
                dvComplTerceros.RowFilter = "id_registros= " + ViewState["id_Registros"].ToString();
                grvComplTerceros.DataSource = dvComplTerceros;
                grvComplTerceros.DataBind();
                //Muestra panel de complemento terceros
                if (dvComplTerceros.Count > 0)
                    fnMostrarPnlComplTerceros();
                else
                    fnOcultarPnlComplTerceros();
            }
        }
    }

    protected void ddlTipoDoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Funcion para recargar los impuestos
        hdnItemMod.Value = "false";
        fnRecargaTipoImpuestos();
        fnCargarSeries();
        fnCargarFolio();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ddlTipoDoc" + "|" + "Selecciona un tipo de documento." + "|" + ddlTipoDoc.SelectedValue);
    }

    protected void imgModificar_Click(object sender, ImageClickEventArgs e)
    {
        clsOperacionDocImpuestos gDAL = new clsOperacionDocImpuestos();

        //Limpiar panel y mostrarlo.lblTipoImpDoc
        try
        {
            string sEfecto = ((ImageButton)sender).CommandArgument;
            ddlCambioImpuesto.DataSource = gDAL.fnCargarImpuestos(sEfecto);
            ddlCambioImpuesto.DataBind();
        }
        catch (Exception ex)
        {
            ddlCambioImpuesto.DataSource = null;
            ddlCambioImpuesto.DataBind();
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }

        mpePanel.Show();
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        //Modifica el valor del impuesto seleccionado.
        int nRow = Convert.ToInt32(hdnItemSel.Value);

        if (ddlCambioImpuesto.SelectedValue != string.Empty)
        {
            Label lbltasa = new Label();
            lbltasa = (Label)dtsTotales.Items[nRow].FindControl("lblTasaVal");
            lbltasa.Text = ddlCambioImpuesto.SelectedValue + " %";
            ((Label)dtsTotales.Items[nRow].FindControl("lblTipoImpuesto")).Text = ddlCambioImpuesto.SelectedItem.Text + ":";
            hdnItemMod.Value = "true";
            fnRecargaTipoImpuestosModificados();

            datosUsuario = clsComun.fnUsuarioEnSesion();
            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "btnAceptar" + "|" + "Se modificó el valor del impuesto en pantalla." + "|" + lbltasa.Text);

        }

    }

    protected void dtsTotales_ItemCommand(object source, DataListCommandEventArgs e)
    {
        //Guarda el valor del impuesto seleccionado
        hdnItemSel.Value = e.Item.ItemIndex.ToString();
    }

    protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {

        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ddlSucursalesExpEn" + "|" + "Selecciona un tipo de sucursal expedido en." + "|" + ddlSucursales.SelectedValue);
        //fnReiniciarDatosReceptores();
        //fnArticulosEstructura(Convert.ToInt32(ddlSucursales.SelectedValue));
        if (datosUsuario.version == "3.2")
        {
            fLimpiarExpEn();
            fnObtienDomicilioSuc(Convert.ToInt32(ddlSucursales.SelectedValue));
            cbOtraUbi.Checked = false;
            cbOtraUbi_CheckedChanged(sender, e);
        }
        fnCargarSeries();
        fnCargarFolio();
    }

    private void fnObtienDomicilioSuc(int pnSucursal)
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        gOpeSucursal = new clsOperacionSucursales();
        if (datosUsuario.version == "3.2")
        {
            DataTable asd = gOpeSucursal.fnObtenerDomicilioSuc(pnSucursal);

            try
            {

                if (asd != null && asd.Rows.Count > 0)
                {
                    ddlEstadoExpEn.SelectedValue = asd.Rows[0]["id_estado"].ToString();
                    txtMunicipioExpEn.Text = asd.Rows[0]["municipio"].ToString();
                    txtCalleExpEn.Text = asd.Rows[0]["calle"].ToString();
                    txtCodigoPostalExpEn.Text = asd.Rows[0]["codigo_postal"].ToString();
                    //txtEstadoDat.Text = txtMunicipioEmisor.Text;
                    txtNoExteriorExpEn.Text = asd.Rows[0]["numero_exterior"].ToString();
                    txtNoInteriorExpEn.Text = asd.Rows[0]["numero_interior"].ToString();
                    txtColoniaExpEn.Text = asd.Rows[0]["colonia"].ToString();
                    txtReferenciaExpEn.Text = asd.Rows[0]["referencia"].ToString();
                    txtLocalidadExpEn.Text = asd.Rows[0]["localidad"].ToString();
                }

            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                Response.Redirect("~/Default.aspx", false);
            }
        }

    }

    private void fnObtienDomicilioSucFis(int pnSucursal)
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        gOpeSucursal = new clsOperacionSucursales();
        clsOperacionCuenta gDAL = new clsOperacionCuenta();
        DataTable asd = gOpeSucursal.fnObtenerDomicilioSuc(pnSucursal);
        DataTable sdrInfo = gDAL.fnObtenerDatosFiscalesSuc(pnSucursal);
        string sDireccionFis = string.Empty;
        string sUbicacionFis = string.Empty;
        try
        {

            if (asd != null && asd.Rows.Count > 0  && sdrInfo != null && sdrInfo.Rows.Count > 0)
            {
                ddlEdoLugExp.SelectedValue = asd.Rows[0]["id_estado"].ToString();

                if (asd.Rows[0]["calle"].ToString() != string.Empty)
                    sDireccionFis += "Calle " + asd.Rows[0]["calle"].ToString() + " "; ;

                if (asd.Rows[0]["numero_interior"].ToString() != string.Empty)
                    sDireccionFis += "No. Int " + asd.Rows[0]["numero_interior"].ToString() + " ";

                if (asd.Rows[0]["numero_exterior"].ToString() != string.Empty)
                    sDireccionFis += "No. Ext " + asd.Rows[0]["numero_exterior"].ToString() + " ";

                if (asd.Rows[0]["colonia"].ToString() != string.Empty)
                    sDireccionFis += "Col. " + asd.Rows[0]["colonia"].ToString() + " ";

                lblDireccionFis.Text = sDireccionFis;

                if (asd.Rows[0]["localidad"].ToString() != string.Empty)
                    sUbicacionFis += asd.Rows[0]["localidad"].ToString() + ", ";

                if (asd.Rows[0]["municipio"].ToString() != string.Empty)
                    sUbicacionFis += asd.Rows[0]["municipio"].ToString() + ", ";

                if (asd.Rows[0]["estado"].ToString() != string.Empty)
                    sUbicacionFis += asd.Rows[0]["estado"].ToString() + ", ";

                sUbicacionFis += "México "; //la cadena "México" esta fijo por q no hay manera de obtenerlo en tablas ya que no hay registro solo estado, municipio y localidad

                if (asd.Rows[0]["codigo_postal"].ToString() != string.Empty)
                    sUbicacionFis += "C.P " + asd.Rows[0]["codigo_postal"].ToString() + " ";

                lblUbicacionFis.Text = sUbicacionFis;

                lblRFCFis.Text = "RFC " + sdrInfo.Rows[0]["rfc"].ToString();

                //if (asd["regimen_fiscal"].ToString() != string.Empty)
                //    txtRegimenfiscal.Text = asd["regimen_fiscal"].ToString();
            }
            else
            {
                lblDireccionFis.Text = string.Empty;
                lblUbicacionFis.Text = string.Empty;
                lblRFCFis.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            Response.Redirect("~/Default.aspx", false);
        }

    }

    private void fnReiniciarDatosReceptores()
    {
        Session.Add("identificadorReceptor", ddlSucursalesFis.SelectedValue);

        txtRazonReceptor.Text = string.Empty;
        txtRfcReceptor.Text = string.Empty;
        txtPais.Text = string.Empty;
        txtEstado.Text = string.Empty;
        txtMunicipio.Text = string.Empty;
        txtLocalidad.Text = string.Empty;
        txtCalle.Text = string.Empty;
        txtNoExterior.Text = string.Empty;
        txtNoInterior.Text = string.Empty;
        txtColonia.Text = string.Empty;
        txtCodigoPostal.Text = string.Empty;
    }

    protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarFolio();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ddlSerie" + "|" + "Selecciona una serie." + "|" + ddlSerie.SelectedValue);

    }

    protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Funcion para recargar el nombre.
        fnCambiarTipoMoneda();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ddlMoneda" + "|" + "Selecciona un tipo de moneda." + "|" + ddlMoneda.SelectedValue);

    }

    protected void btnCrear_Click(object sender, EventArgs e)
    {
        int bandera = 0;

        if (!string.IsNullOrEmpty(txtPais.Text) &&
        !string.IsNullOrEmpty(txtEstado.Text) &&
        !string.IsNullOrEmpty(txtMunicipio.Text) &&
        !string.IsNullOrEmpty(txtCalle.Text) &&
        !string.IsNullOrEmpty(txtRfcReceptor.Text) &&
        !string.IsNullOrEmpty(txtRazonReceptor.Text))
        {
            bool bValCre = true;
            //Valida si contiene créditos suficientes
            bValCre = fnActualizarLblCreditos();
            if (bValCre == false)
                return;

            string sFormaPago = string.Empty;
            datosUsuario = clsComun.fnUsuarioEnSesion();
            if (datosUsuario.version == "3.2") //Si es version 3.2 se valida tipo de moneda
            {
                if (ddlMoneda.SelectedValue != "MXN") //Si no es Pesos entonces se valida el tipo de cambio
                {
                    if (Convert.ToDecimal(txttipoCambio.Text) == 0) //El tipo de cambio no tiene que ser cero
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varMsjTipCam, Resources.resCorpusCFDIEs.varTituloTipCam);
                        return;
                    }
                }
            }
            //btnCrear.Enabled = false; 
            if (!clsComun.fnValidaExpresion(txtRfcReceptor.Text, @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
            {
                clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.regxRFC);
                return;
            }

            

            if (ddlFormaPago.SelectedItem.Text == "Pago en Parcialidades") //Si eligio la opción pago parciales evaluara que esten capturados los campos adicionales
            {
                txtFecFolFisOri.Text = Convert.ToString(ViewState["fechafolioFiscarlOrig"]);
                if (cbPagParPri.Checked == false) //Si no es generación por primera vez
                {
                    if (txtFolFisOri.Text == string.Empty && txtFecFolFisOri.Text == string.Empty && txtMonFolFisOri.Text == string.Empty)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgPagoParciales, Resources.resCorpusCFDIEs.varTituloTipCam);
                        return;
                    }
                    else
                        txtMonFolFisOri.Text = String.Format("{0:n6}", Convert.ToDecimal(txtMonFolFisOri.Text));

                    sFormaPago = Resources.resCorpusCFDIEs.lblPagParDe + " " + txtPagParDe.Text + " " + Resources.resCorpusCFDIEs.lblPagParA + " " + txtPagParA.Text;
                }
                else
                {
                    sFormaPago = "Parcialidades " + txtNumParcialidad.Text;
                }
            }
            else
                sFormaPago = ddlFormaPago.SelectedValue;


            if (!ddlMetodoPago.SelectedItem.Text.Equals("No Aplica") && !ddlMetodoPago.SelectedItem.Text.Equals("Efectivo") && !ddlMetodoPago.SelectedItem.Text.Equals("No identificado") && txtNumeroCuenta.Text.Replace(" ", "").Trim().Length < 4)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.regexlblNumCuenta + "(Min. 4) " + Resources.resCorpusCFDIEs.lblNumeroCuenta);
                return;
            }

            if (grvDetalles.Rows.Count > 0) // && dtsTotales.Items.Count > 0)
            {
                //Valida que no exista valores negavitos en subtotal y total
                if (Convert.ToDecimal(lblDetSubtotal.Text.Replace("$", "").Replace(",", "")) < 0 || Convert.ToDecimal(lblTotalVal.Text.Replace("$", "").Replace(",", "")) < 0)
                {
                    //Muestra un msj de aviso 
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblAvisoTotales, Resources.resCorpusCFDIEs.varContribuyente);
                    return;
                }


                //Valida que el subtotal y total sea cero cuando se selecciona el complemento donativas en especie
                string tipoDonativo = (Session["TipoDonativo"] != null) ? Session["TipoDonativo"].ToString() : string.Empty; //Tipos de donativos: "0" - Monetario, "1" - Especie
                if (ddlComplemento.SelectedValue == "Complementos/webComplementosDonatarias.aspx" && tipoDonativo == "1")
                {
                    //El total y subtotal debe ser igual a cero cuando se selecciona el complemento donativas en especie
                    if (Convert.ToDecimal(lblDetSubtotal.Text.Replace("$", "").Replace(",", "")) != 0 || Convert.ToDecimal(lblTotalVal.Text.Replace("$", "").Replace(",", "")) != 0)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgTotSubCeroDonativas, Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }
                }
                else
                {
                    //Si no se selecciona el complemento donativas en especie, verifica que no existan conceptos con importe igual a cero
                    foreach (GridViewRow renglon in grvDetalles.Rows)
                    {
                        if (Convert.ToDecimal(renglon.Cells[8].Text.ToString().Replace("$", "").Replace(",", "").Replace("-", "")) == 0)
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgConceptoCeroDonativas, Resources.resCorpusCFDIEs.varContribuyente);
                            return;
                        }
                    }

                    //Si no se selecciona el complemento donativas en especie, verifica que el total no sea cero
                    if (Convert.ToDecimal(lblTotalVal.Text.Replace("$", "").Replace(",", "")) == 0)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgTotCeroDonativas, Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }
                }


                //Recupera datos del emisor.
                clsTimbradoFuncionalidad timbrar = new clsTimbradoFuncionalidad();
                clsOperacionCuenta gDAL = new clsOperacionCuenta();
                clsValCertificado vValidadorCertificado = null;
                DataTable sdrInfo = null;
                XmlDocument xDocTimbrado = new XmlDocument();
                DataTable certificado = new DataTable();
                string resValidacion = string.Empty;
                string sSerie = string.Empty;
                string sFolio = string.Empty;
                string sRFCEmisor = string.Empty;
                string sCadenaOriginal = string.Empty;
                string sRetornoSAT = string.Empty;
                int retornoInsert = 0;
                string sCOriginal = String.Empty;
                string noCertificadoPAC = string.Empty;
                string selloPAC = string.Empty;
                string sRequest = string.Empty;
                string sResponse = string.Empty;
                string sUUID = string.Empty;

                if (ddlSucursal.SelectedIndex == -1)
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varExSucursal, Resources.resCorpusCFDIEs.varContribuyente);
                    return;
                }

                try
                {
                    ////Según version se obtiene datos fiscales
                    //switch (datosUsuario.version)
                    //{
                    //    case "3.0":
                    //        sdrInfo = gDAL.fnObtenerDatosFiscales();
                    //        break;

                    //    case "3.2":
                    sdrInfo = gDAL.fnObtenerDatosFiscalesSuc(Convert.ToInt32(ddlSucursalesFis.SelectedValue));
                    //        break;
                    //}

                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "1.-fnObtenerDatosFiscales" + "|" + "Se recuperan los datos fiscales del emisor");


                    if (sdrInfo != null && sdrInfo.Rows.Count > 0)
                    {
                        //Obtener datos del emisor, llave, certificado, y password
                        certificado = clsTimbradoFuncionalidad.ObtenerCertificado(Convert.ToInt32(sdrInfo.Rows[0]["id_rfc"].ToString()));

                        if (clsTimbradoFuncionalidad.fnReplaceCaracters(sdrInfo.Rows[0]["rfc"].ToString()) != string.Empty)
                            sRFCEmisor = clsTimbradoFuncionalidad.fnReplaceCaracters(sdrInfo.Rows[0]["rfc"].ToString());

                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "2.-ObtenerCertificado" + "|" + "Se recuperan los certificados del emisor");

                        /* Modificación 28 - 12 - 2012 Josel Moreno - Se verifica que se recupere el certificado de la base de datos */
                        if (certificado == null || certificado.Rows.Count < 1)
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoRecuperoCert);
                            return;
                        }
                        /* Fin Modificación 28 - 12 - 2012 */
                    }

                    byte[] bLlave = (byte[])certificado.Rows[0]["key"];
                    byte[] bCertificado = (byte[])certificado.Rows[0]["certificado"];
                    byte[] sPassword = (byte[])certificado.Rows[0]["password"];

                    //realizamos las validaciones de SAT sobre el archivo
                    vValidadorCertificado = new clsValCertificado(bCertificado);
                    if (sRFCEmisor != vValidadorCertificado.fnVerificarExistenciaCertificado())
                        resValidacion = Resources.resCorpusCFDIEs.valEmisionCer;
                    if (!vValidadorCertificado.fnComprobarFechas())
                        resValidacion = Resources.resCorpusCFDIEs.valFechaCer;
                    if (!vValidadorCertificado.fnCSD308())
                        resValidacion = Resources.resCorpusCFDIEs.varCSD308;
                    if (vValidadorCertificado.fnEsFiel())
                        resValidacion = Resources.resCorpusCFDIEs.valFIELCerFis;

                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "3.-vValidadorCertificado" + "|" + "Se validan los certificados del emisor");

                    if (!string.IsNullOrEmpty(resValidacion))
                    {
                        clsComun.fnMostrarMensaje(this, resValidacion);
                        return;
                    }
                    else
                    {

                        fnCertProximoVencer((certificado.Rows[0]["fecha_termino"] != null) ? certificado.Rows[0]["fecha_termino"].ToString() : string.Empty); /* Modificación 28 - 12 - 2012 Josel Moreno - Se agregó método que muestra mensaje si el certicado está próximo a vencer */

                        //Si se agrega complemento a datatable de impuestos ISH, Cargo no gravables para posteriormente agregarlo en xml
                        dtCompl = (DataTable)ViewState["dtImpCompl"];
                        //if (cbAgrCom.Checked)
                        //{
                        if (dtCompl.Rows.Count > 0)
                        {
                            DataTable dtImpuestos = new DataTable();
                            dtImpuestos = (DataTable)(ViewState["dtImpuestos"]);
                            Complementos sComplemento = new Complementos();
                            TablaComplementos = new DataTable();
                            double Retencion = 0;
                            double Total = 0;
                            TablaComplementos = sComplemento.fnImpuestosLocales("1.0", Retencion, Total, true, dtImpuestos);
                            Session["TablaComplementos"] = TablaComplementos;
                            ddlComplemento.Enabled = false;
                        }

                        ////Preparamos los objetos de manejo tanto de la llave como del certificado
                        gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                        gCertificado = new clsValCertificado(bCertificado);
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "4.-gCertificado" + "|" + "Preparamos los objetos de manejo tanto de la llave como del certificado");


                        sSerie = clsTimbradoFuncionalidad.fnReplaceCaracters(ddlSerie.SelectedItem.Text);

                        //sFolio = clsTimbradoFuncionalidad.fnReplaceCaracters(txtFolio.Text);
                        //sFolio = fnCargarFolioGen();

                        //if (sSerie == "N/A")
                        //    sSerie = string.Empty;
                        //if (sFolio == "0")
                        //    sFolio = string.Empty;

                        if (sSerie == "N/A")
                        {
                            sSerie = string.Empty;
                        }
                        else
                        {
                            if (sFolio == "0")
                            {
                                sFolio = string.Empty;
                            }
                            else
                            {
                                sFolio = "00000000";
                            }
                        }


                        string LugarExp = ddlPaisLugExp.SelectedItem.Text + ", " + ddlEdoLugExp.SelectedItem.Text; // +"," + txtMunicipioDat.Text;
                        Comprobante30 cfd30 = new Comprobante30();
                        Comprobante cfd = new Comprobante();

                        //Obtiene aduana de concepto
                        dtInfoAduanal = (DataTable)ViewState["dtInfAduanal"];
                        dtComplTerceros = (DataTable)ViewState["dtComplTerceros"];
                        tablePas = (DataTable)ViewState["table"];

                        switch (datosUsuario.version)
                        {
                            case "3.0":
                                cfd30 = timbrar.fnObtenerXML3_0(
                                                           datosUsuario.version,
                                                           sFolio,
                                                           ddlTipoDoc.SelectedValue.Split('|')[1],
                                                           Convert.ToDecimal(lblDetSubtotal.Text.Replace("$", "").Replace(",", "")),
                                                           Convert.ToDecimal(lblTotalVal.Text.Replace("$", "").Replace(",", "")),
                                                           sSerie,
                                                           ddlMoneda.SelectedValue,
                                                           txtRfcReceptor.Text,
                                                           txtRazonReceptor.Text,
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtPais.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtEstado.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtMunicipio.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtLocalidad.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtCalle.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtNoExterior.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtNoInterior.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtColonia.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtCodigoPostal.Text),
                                                           grvDetalles,
                                                           dtsTotales,
                                                           bCertificado,
                                                           datosUsuario.id_usuario,
                                                           this.Title,
                                                           Convert.ToInt32(ddlSucursalesFis.SelectedValue));
                                Session["cfdDOC"] = cfd30;
                                break;
                            case "3.2":
                                cfd = timbrar.fnObtenerXML3_2(
                                                           datosUsuario.version,
                                                           sFolio,
                                                           ddlTipoDoc.SelectedValue.Split('|')[1],
                                                           Convert.ToDecimal(lblDetSubtotal.Text.Replace("$", "").Replace(",", "")),
                                                           Convert.ToDecimal(lblDetDescuentoVal.Text.Replace("$", "").Replace(",", "")),
                                                           Convert.ToDecimal(lblTotalVal.Text.Replace("$", "").Replace(",", "")),
                                                           sSerie,
                                                           ddlMoneda.SelectedValue,
                                                           txtRfcReceptor.Text,
                                                           txtRazonReceptor.Text,
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtPais.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtEstado.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtMunicipio.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtLocalidad.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtCalle.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtNoExterior.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtNoInterior.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtColonia.Text),
                                                           clsTimbradoFuncionalidad.fnReplaceCaracters(txtCodigoPostal.Text),
                                                           grvDetalles,
                                                           dtsTotales,
                                                           bCertificado,
                                                           datosUsuario.id_usuario,
                                                           this.Title,
                                                           ddlMetodoPago.SelectedValue,
                                                           LugarExp,
                                                           txtNumeroCuenta.Text,
                                                           ddlPaisExpEn.SelectedItem.Text,
                                                           ddlEstadoExpEn.SelectedItem.Text,
                                                           txtMunicipioExpEn.Text,
                                                           txtCalleExpEn.Text,
                                                           txtCodigoPostalExpEn.Text,
                                                           txttipoCambio.Text,
                                                           txtRegimenfiscal.Text,
                                                           sFormaPago,
                                                           txtNoInteriorExpEn.Text,
                                                           txtNoExteriorExpEn.Text,
                                                           txtColoniaExpEn.Text,
                                                           txtLocalidadExpEn.Text,
                                                           txtReferenciaExpEn.Text,
                                                           cbPagParPri.Checked,
                                                           txtFolFisOri.Text,
                                                           txtSerFolFisOri.Text,
                                                           Convert.ToDateTime(txtFecFolFisOri.Text),
                                                           Convert.ToDecimal(txtMonFolFisOri.Text),
                                                           cbAgrExpEn.Checked,
                                                           ddlFormaPago.SelectedItem.Text,
                                                           Convert.ToInt32(ddlSucursalesFis.SelectedValue),
                                                           dtInfoAduanal,
                                                           dtComplTerceros,
                                    //Datatable de detalle
                                                           tablePas);//Aplico cambio de integracion
                                Session["cfdDOC"] = cfd;
                                break;
                        }


                        ////Generamos el primer XML necesario para generar la cadena original
                        if (ddlComplemento.SelectedValue != "0" || dtCompl.Rows.Count > 0)
                        {

                            if (Session["TablaComplementos"] == null)
                            {
                                bandera = 1;
                                throw new Exception(Resources.resCorpusCFDIEs.lblErrorComplemento);
                            }
                            else
                            {
                                TablaComplementos = (DataTable)Session["TablaComplementos"];
                            }
                            string TnameSpace = TablaComplementos.Rows[0]["tnamespace"].ToString();
                            switch (datosUsuario.version)
                            {
                                case "3.0":
                                    xDocTimbrado = gTimbrado.fnGenerarXML30(cfd30, TnameSpace); //Aplico cambio de integracion
                                    break;
                                case "3.2":
                                    xDocTimbrado = gTimbrado.fnGenerarXML32(cfd, TnameSpace); //Aplico cambio de integracion
                                    break;
                            }

                            // complemento instituciones educativas = 6
                            if (ddlComplemento.SelectedValue == "6")
                            {

                                XmlDocument xl2 = new XmlDocument();
                                XmlElement fin2 = xDocTimbrado.CreateElement("ComplementoConcepto");
                                fin2.InnerXml = TablaComplementos.Rows[0]["nodo"].ToString();
                                xDocTimbrado.ChildNodes[1].ChildNodes[2].ChildNodes[0].AppendChild(fin2);
                            }

                            if (ddlComplemento.SelectedItem.Text == "10")
                            {

                                XmlDocument xl2 = new XmlDocument();
                                XmlElement fin2 = xDocTimbrado.CreateElement("ComplementoConcepto");
                                fin2.InnerXml = TablaComplementos.Rows[0]["nodo"].ToString();
                                xDocTimbrado.ChildNodes[1].ChildNodes[2].ChildNodes[0].AppendChild(fin2);
                            }

                            //Complemento Concepto
                            //if (ddlComplemento.SelectedValue == "Complementos/webComplementosTerceros.aspx")
                            //{
                            //    XmlDocument xl2 = new XmlDocument();
                            //    XmlElement fin2 = xDocTimbrado.CreateElement("ComplementoConcepto");
                            //    fin2.InnerXml = TablaComplementos.Rows[0]["nodo"].ToString();
                            //    xDocTimbrado.ChildNodes[1].ChildNodes[2].ChildNodes[0].AppendChild(fin2);
                            //}
                        }
                        else
                        {
                            switch (datosUsuario.version)
                            {
                                case "3.0":
                                    xDocTimbrado = gTimbrado.fnGenerarXML30(cfd30, null);//Aplico cambio de integracion
                                    break;
                                case "3.2":
                                    xDocTimbrado = gTimbrado.fnGenerarXML32(cfd, null);
                                    break;
                            }

                        }

                        //Namespace complemento terceros
                        if (dtComplTerceros.Rows.Count > 0)
                        {
                            try
                            {
                                XmlDocument xDocTimbradoTemp = fnAgregarNameSpaceComplTerceros(xDocTimbrado);
                                xDocTimbrado = xDocTimbradoTemp;
                            }
                            catch (Exception ex)
                            {
                                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "Error al agregar el namespace de complemento de terceros", 0);
                            }
                        }

                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "8.-fnGenerarXML" + "|" + "Se contruye XML con los datos generados.");

                        string scadena = "";
                        switch (datosUsuario.version)
                        {
                            case "3.0":
                                scadena = "cadenaoriginal_3_0";
                                break;
                            case "3.2":
                                scadena = "cadenaoriginal_3_2";
                                break;
                        }
                        //Generamos la cadena original
                        XPathNavigator navNodoTimbre = xDocTimbrado.CreateNavigator();
                        sCadenaOriginal = gTimbrado.fnConstruirCadenaTimbrado(navNodoTimbre, scadena); //"cadenaoriginal_3_2"); 
                        sRequest = clsComun.fnRequestRecepcion(sCadenaOriginal, ddlTipoDoc.SelectedValue.Split('|')[1], ddlSucursalesFis.SelectedValue, datosUsuario.userName, "");
                        if (clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sRequest, DateTime.Now, "0", "Request", "E", string.Empty) == 0)
                        {
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "No se inserto el Request.");
                        }

                        if (ddlComplemento.SelectedValue != "0" || dtCompl.Rows.Count > 0)
                        {
                            sCadenaOriginal = sCadenaOriginal.TrimEnd('|');
                            string CadenaComplemento = TablaComplementos.Rows[0]["cadenaoriginal"].ToString();
                            sCadenaOriginal += CadenaComplemento;
                        }

                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "9.-fnConstruirCadenaTimbrado" + "|" + "Generamos la cadena original.");


                        clsNodoTimbre creadorTimbre = new clsNodoTimbre();
                        string sSello = string.Empty;

                        switch (datosUsuario.version)
                        {
                            case "3.0":
                                sSello = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1, true);
                                cfd30.sello = sSello;
                                break;
                            case "3.2":
                                sSello = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1, true);
                                cfd.sello = sSello;
                                break;
                        }
                        //Validar sello del emisor
                        if (!vValidadorCertificado.fnVerificarSello(sCadenaOriginal, sSello))
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado + " " + "HSM", Resources.resCorpusCFDIEs.varContribuyente);
                            return;
                        }

                        if (ddlComplemento.SelectedValue != "0" || dtCompl.Rows.Count > 0)
                        {
                            if (ddlComplemento.SelectedValue != "6" && ddlComplemento.SelectedItem.Text != "10" || dtCompl.Rows.Count > 0)
                            {
                                string NodoComplemento = TablaComplementos.Rows[0]["nodo"].ToString();
                                switch (datosUsuario.version)
                                {
                                    case "3.0":
                                        creadorTimbre.GenerarNodoTimbre30(cfd30, ref sCOriginal, NodoComplemento, null);
                                        break;
                                    case "3.2":
                                        creadorTimbre.GenerarNodoTimbre32(cfd, ref sCOriginal, NodoComplemento, null);
                                        break;
                                }

                            }
                            else
                            {
                                switch (datosUsuario.version)
                                {
                                    case "3.0":
                                        creadorTimbre.GenerarNodoTimbre30(cfd30, ref sCOriginal, null, null);
                                        break;
                                    case "3.2":
                                        creadorTimbre.GenerarNodoTimbre32(cfd, ref sCOriginal, null, null);
                                        break;
                                }

                            }
                        }
                        else
                        {
                            switch (datosUsuario.version)
                            {
                                case "3.0":
                                    creadorTimbre.GenerarNodoTimbre30(cfd30, ref sCOriginal, null, null);
                                    break;
                                case "3.2":
                                    creadorTimbre.GenerarNodoTimbre32(cfd, ref sCOriginal, null, null);
                                    break;
                            }

                        }

                        int x = 0;
                        //Revisar datos del HSM
                        switch (datosUsuario.version)
                        {
                            case "3.0":
                                x = 0;
                                if (ddlComplemento.SelectedValue != "0" || dtCompl.Rows.Count > 0)
                                {
                                    if (ddlComplemento.SelectedValue != "6" && ddlComplemento.SelectedValue != "10" || dtCompl.Rows.Count > 0)
                                    {
                                        x = 1;
                                    }
                                }
                                //Revisar que traiga los datos del HSM
                                if (cfd30.Complemento.Any[x].Attributes["UUID"].Value == string.Empty ||
                                cfd30.Complemento.Any[x].Attributes["FechaTimbrado"].Value == string.Empty ||
                                cfd30.Complemento.Any[x].Attributes["selloCFD"].Value == string.Empty ||
                                cfd30.Complemento.Any[x].Attributes["noCertificadoSAT"].Value == string.Empty ||
                                cfd30.Complemento.Any[x].Attributes["selloSAT"].Value == string.Empty)
                                {

                                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCertificado + " " + "HSM", Resources.resCorpusCFDIEs.varContribuyente);
                                    return;
                                }
                                else
                                {
                                    selloPAC = cfd30.Complemento.Any[x].Attributes["selloSAT"].Value;
                                    noCertificadoPAC = cfd30.Complemento.Any[x].Attributes["noCertificadoSAT"].Value;
                                }


                                break;

                            case "3.2":
                                x = 0;
                                if (ddlComplemento.SelectedValue != "0" || dtCompl.Rows.Count > 0)
                                {
                                    if (ddlComplemento.SelectedValue != "6" && ddlComplemento.SelectedItem.Text != "10" || dtCompl.Rows.Count > 0)
                                    {
                                        x = 1;
                                    }
                                }
                                //Revisar que traiga los datos del HSM
                                if (cfd.Complemento.Any[x].Attributes["UUID"].Value == string.Empty ||
                                cfd.Complemento.Any[x].Attributes["FechaTimbrado"].Value == string.Empty ||
                                cfd.Complemento.Any[x].Attributes["selloCFD"].Value == string.Empty ||
                                cfd.Complemento.Any[x].Attributes["noCertificadoSAT"].Value == string.Empty ||
                                cfd.Complemento.Any[x].Attributes["selloSAT"].Value == string.Empty)
                                {
                                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCertificado + " " + "HSM", Resources.resCorpusCFDIEs.varContribuyente);
                                    return;
                                }
                                else
                                {
                                    selloPAC = cfd.Complemento.Any[x].Attributes["selloSAT"].Value;
                                    noCertificadoPAC = cfd.Complemento.Any[x].Attributes["noCertificadoSAT"].Value;
                                    sUUID = cfd.Complemento.Any[x].Attributes["UUID"].Value;
                                }

                                break;
                        }

                        //Validamos sello del PAC
                        if (!clsValCertificado.fnVerificarSelloPAC(sCOriginal, selloPAC, noCertificadoPAC))
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado + " " + "HSM", Resources.resCorpusCFDIEs.varContribuyente);
                            return;
                        }

                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "10.-GenerarNodoTimbre" + "|" + "Generamos el sello.");


                        //Generamos el sello del SAT, se lo agregamos al objeto y generamos el XML final
                        if (ddlComplemento.SelectedValue != "0" || dtCompl.Rows.Count > 0)
                        {

                            TablaComplementos = (DataTable)Session["TablaComplementos"];
                            string TnameSpace = TablaComplementos.Rows[0]["tnamespace"].ToString();
                            switch (datosUsuario.version)
                            {
                                case "3.0":
                                    xDocTimbrado = gTimbrado.fnGenerarXML30(cfd30, TnameSpace); //Aplico cambio de integracion
                                    break;
                                case "3.2":
                                    xDocTimbrado = gTimbrado.fnGenerarXML32(cfd, TnameSpace); //Aplico cambio de integracion
                                    break;
                            }


                            if (ddlComplemento.SelectedValue == "6")
                            {
                                XmlDocument xl2 = new XmlDocument();
                                XmlElement fin2 = xDocTimbrado.CreateElement("ComplementoConcepto");
                                fin2.InnerXml = TablaComplementos.Rows[0]["nodo"].ToString();
                                xDocTimbrado.ChildNodes[1].ChildNodes[2].ChildNodes[0].AppendChild(fin2);
                            }
                            if (ddlComplemento.SelectedItem.Text == "10")
                            {
                                XmlDocument xl2 = new XmlDocument();
                                XmlElement fin2 = xDocTimbrado.CreateElement("ComplementoConcepto");
                                fin2.InnerXml = TablaComplementos.Rows[0]["nodo"].ToString();
                                xDocTimbrado.ChildNodes[1].ChildNodes[2].ChildNodes[0].AppendChild(fin2);
                            }

                            //if (ddlComplemento.SelectedValue == "Complementos/webComplementosTerceros.aspx")
                            //{
                            //    XmlDocument xl2 = new XmlDocument();
                            //    XmlElement fin2 = xDocTimbrado.CreateElement("cfdi:ComplementoConcepto", "http://www.sat.gob.mx/cfd/3");
                            //    fin2.InnerXml = TablaComplementos.Rows[0]["nodo"].ToString();
                            //    xDocTimbrado.ChildNodes[1].ChildNodes[2].ChildNodes[0].AppendChild(fin2);
                            //}
                        }
                        else
                        {
                            switch (datosUsuario.version)
                            {
                                case "3.0":
                                    xDocTimbrado = gTimbrado.fnGenerarXML30(cfd30, null); //Aplico cambio de integracion 
                                    break;
                                case "3.2":
                                    xDocTimbrado = gTimbrado.fnGenerarXML32(cfd, null); //Aplico cambio de integracion 
                                    break;
                            }

                        }

                        //Namespace complemento terceros
                        if (dtComplTerceros.Rows.Count > 0)
                        {
                            try
                            {
                                XmlDocument xDocTimbradoTemp = fnAgregarNameSpaceComplTerceros(xDocTimbrado);
                                xDocTimbrado = xDocTimbradoTemp;
                            }
                            catch (Exception ex)
                            {
                                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "Error al agregar el namespace de complemento de terceros", 0);
                            }
                        }

                        Session["xDocTimbrado"] = xDocTimbrado.InnerXml;
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "11.-GenerarNodoTimbre" + "|" + "Generamos el sello del SAT, se lo agregamos al objeto y generamos el XML final.");
                    }

                }
                catch (Exception ex)
                {
                    if (bandera == 1)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblErrorComplemento, Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }
                    else
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCertificado, Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }
                }

                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "12.-fnInsertarComprobante" + "|" + "Inserta comprobante generado exitosamente en BD.");

                try
                {
                    //Generar el Hash para revisar si no hay uno existente en la BD
                    string HASHTimbre = clsEnvioSAT.GetHASH(sCOriginal).ToUpper();
                    string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginal).ToUpper();


                    if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHEmisor, "Emisor"))
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }


                    if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHTimbre, "Timbre"))
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }

                    if (!clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHTimbre, "Timbre"))
                    {
                        //Asigna serie y folio
                        if (sSerie != string.Empty)
                        {
                            XmlNamespaceManager nsmSerie = new XmlNamespaceManager(xDocTimbrado.NameTable);
                            nsmSerie.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

                            sFolio = fnCargarFolioGen();
                            xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", nsmSerie).SetValue(sFolio);
                        }

                        XmlNamespaceManager nsmComprobante2 = new XmlNamespaceManager(xDocTimbrado.NameTable);
                        nsmComprobante2.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                        nsmComprobante2.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                        DateTime dFecha_Timbrado = new DateTime();
                        string sNombre_Emisor = "";
                        string sRfcReceptor = "";
                        string sNombre_Receptor = "";
                        DateTime dFecha_Emision = new DateTime();
                        Decimal nTotal = 0;
                        string sMoneda = "";

                        try { dFecha_Timbrado = Convert.ToDateTime(xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante2).Value); }
                        catch { }
                        try { sNombre_Emisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante2).Value; }
                        catch { }
                        try { sRfcReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante2).Value; }
                        catch { }
                        try { sNombre_Receptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante2).Value; }
                        catch { }
                        try { dFecha_Emision = Convert.ToDateTime(xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante2).Value); }
                        catch { }
                        try { nTotal = Convert.ToDecimal(xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante2).Value); }
                        catch { }
                        try { sMoneda = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobante2).Value; }
                        catch { }
                     

                        retornoInsert = clsTimbradoFuncionalidad.fnInsertarComprobante
                            (
                            //xDocTimbrado.DocumentElement.OuterXml, 
                            PAXCrypto.CryptoAES.EncriptaAES(xDocTimbrado.DocumentElement.OuterXml), 
                            Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]), 
                            'G',
                            DateTime.Now, 
                            Convert.ToInt32(ddlSucursalesFis.SelectedValue), 
                            datosUsuario.id_usuario, 
                            sSerie, 
                            "C", 
                            HASHTimbre, 
                            HASHEmisor,
                            sUUID,
                            dFecha_Timbrado,
                            sRFCEmisor,
                            sNombre_Emisor,
                            sRfcReceptor,
                            sNombre_Receptor,
                            dFecha_Emision,
                            sSerie,
                            sFolio,
                            //nTotal,
                            PAXCrypto.CryptoAES.EncriptaAES(Convert.ToString(nTotal)),
                            sMoneda
                            );

                        //fnActualizaCreditos();

                        if (retornoInsert > 0)
                        {
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "13.-fnInsertarComprobante" + "|" + "Insertado correctamente: " + retornoInsert + " UUID: " + sUUID.ToUpper());
                            //Inserta acuse PAC y creditos al guardar el comprobante.
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, sUUID.ToUpper(), xDocTimbrado.DocumentElement.OuterXml, DateTime.Now, "0", "Response", "E", string.Empty);

                            fnActualizaCreditos();

                            ViewState["retornoInsert"] = retornoInsert;

                            //llenar el dropdownlist de addendas, si tiene valores se muestra el boton y el dropdownlist
                            if (ddlAddenda.Items.Count > 0)
                            {
                                ddlAddenda.Visible = true;
                                btnAdenda.Visible = true;
                            }

                            grvDetalles.DataSource = null;
                            grvDetalles.DataBind();
                            dtsTotales.DataSource = null;
                            dtsTotales.DataBind();
                            //fnLimpiarControles();
                            fnCargarFolio();


                            tablePas = (DataTable)ViewState["table"];
                            tablePas.Rows.Clear();
                            ViewState["table"] = tablePas;

                            //nviar comprobante al SAT
                            //clsEnvioSAT enviarSAT = new clsEnvioSAT();
                            //sRetornoSAT = enviarSAT.fnEnviarBloqueCfdi(HASHEmisor, datosUsuario.id_usuario, xDocTimbrado, retornoInsert, datosUsuario);


                            string Adendda = Convert.ToString(Session["AddendaGenerada"]);
                            // string AddendaNamespace = Convert.ToString(Session["AddendaNamespace"]);

                            if (Adendda != String.Empty && Adendda != null && Adendda != "")
                            {
                                clsConfiguracionAddenda cAddenda = new clsConfiguracionAddenda();
                                XmlDocument xDoctimbrado = new XmlDocument();
                                clsNodoTimbre CreadorAddenda = new clsNodoTimbre();
                                int idModulo = cAddenda.fnObtieneidAddenda(ddlAddenda.SelectedItem.Text);

                                clsConfiguracionAddenda gAddenda = new clsConfiguracionAddenda();
                                XmlDocument AddendaX = new XmlDocument();
                                AddendaX.LoadXml(Adendda);
                                gAddenda.fnInsertaAddenda(retornoInsert, Convert.ToInt32(ddlSucursalesFis.SelectedValue), AddendaX, idModulo);

                            }


                            //Enviar correo con archivos XML y PDF adjuntos
                            //retornoInsert idcfd
                            Guid Gid;
                            Gid = Guid.NewGuid();
                            string snombreDoc = string.Empty;

                            if (!(datosUsuario.email == string.Empty))
                            {
                                txtCorreoEmisor.Text = datosUsuario.email;
                            }

                            int? nidRfc_Receptor, nidEstructura;
                            string sRfc_Receptor, sRazon_Social;

                            nidEstructura = null;
                            nidRfc_Receptor = Convert.ToInt32(ViewState["id_rfc_receptor"]);
                            sRfc_Receptor = string.Empty;
                            sRazon_Social = string.Empty;

                            clsOperacionClientes gOc = new clsOperacionClientes();
                            DataTable DTCorreo = gOc.fnObtenerCorreoCliente(nidRfc_Receptor, nidEstructura, sRfc_Receptor, sRazon_Social);
                            if (DTCorreo.Rows.Count > 0)
                            {
                                txtCorreoCliente.Text = DTCorreo.Rows[0]["correo"].ToString();
                            }
                            else
                                txtCorreoCliente.Text = string.Empty;

                            try
                            {
                                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
                                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                                XPathNavigator navEncabezado = xDocTimbrado.CreateNavigator();
                                try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                                catch { snombreDoc = Gid.ToString(); }
                                ViewState["nombreDoc"] = snombreDoc;
                                ViewState["retornoInsert"] = retornoInsert;

                            }
                            catch (Exception ex)
                            {
                                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                            }

                            lblSubtotal.Visible = false;
                            lblDetSubtotal.Visible = false;
                            lblDetDescuento.Visible = false;
                            lblDetDescuentoVal.Visible = false;
                            lblTotal.Visible = false;
                            lblTotalVal.Visible = false;
                            hdnItemMod.Value = "false";
                            lblNumerosLetras.Text = string.Empty;
                            //btnCrear.Enabled = true;
                            fLimCamposPagParciales();
                            cbPagParPri.Checked = false;
                            dtCompl = (DataTable)ViewState["dtImpCompl"];
                            dtCompl.Rows.Clear();
                            ViewState["dtImpCompl"] = dtCompl;

                            txtDescuentoGlobal.Text = "0";
                            txtDescuentoGlobal.Enabled = true;
                            chkDescuentoGlobal.Enabled = true;
                            chkDescuentoGlobal.Checked = false;
                            txtDescuentoArt.Enabled = true;
                            ViewState["TipoDescuento"] = string.Empty;

                            //Muestra modal para el envio de correo
                            //lblRetSat.Text = sRetornoSAT;
                            //Color vacio en txt
                            txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;
                            txtCorreoCC.BorderColor = System.Drawing.Color.Empty;
                            //Limpia sesion Addenda PAVILSA
                            Session.Remove("lblDetSubtotal");
                            Session.Remove("lblTotalVal");

                            //Se limpia tabla de complemento terceros
                            dtComplTerceros = (DataTable)ViewState["dtComplTerceros"];
                            dtComplTerceros.Rows.Clear();
                            ViewState["dtComplTerceros"] = dtComplTerceros;

                            if (ViewState["rfc_Emisor"] != null && ViewState["razonSocial_Emisor"] != null)
                                txtCorreoMsj.Text = Resources.resCorpusCFDIEs.varMensajeCorreo + "\n" + "\n" + ViewState["razonSocial_Emisor"] + "\n" + "RFC:" + "\n" + ViewState["rfc_Emisor"];
                            Session["TablaComplementos"] = null;
                            Session["AddendaGenerada"] = null;
                            Session["TipoDonativo"] = null;
                            mpeEnvioCorreo.Show();
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgCompGenerado + "<br />" + sRetornoSAT, Resources.resCorpusCFDIEs.varContribuyente);

                        }
                        else
                        {
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "13A.-fnInsertarComprobante" + "|" + "Error al insertar el comprobante:" + Resources.resCorpusCFDIEs.msgNoCompGenerado);
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                        }
                    }
                    else
                    {
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "13B.-fnInsertarComprobante" + "|" + "Error al insertar el comprobante:" + Resources.resCorpusCFDIEs.msgNoCompGenerado);
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                    }
                }
                catch (Exception ex)
                {
                    fnCargarFolio();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "13C.-fnInsertarComprobante" + "|" + "Error al insertar el comprobante:" + Resources.resCorpusCFDIEs.msgNoCompGenerado);
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);

                }

            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varConceptos, Resources.resCorpusCFDIEs.varContribuyente);
            }
        }
        else
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError, Resources.resCorpusCFDIEs.varContribuyente);
        }

    }

    # region Funciones

    /// <summary>
    /// Trae la lista filtrada de las sucursales de los emisores.
    /// </summary>
    private void fnCargarSucursalesExpEn()
    {
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();

            ddlSucursales.DataSource = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(true);
            ddlSucursales.DataBind();
            //if (ddlSucursales.Items.Count > 0)
            //{
            //    pnlRegistros.Visible = true;
            //}
            //else
            //{
            //    pnlRegistros.Visible = false;
            //}
            //fnArticulosEstructura(Convert.ToInt32(ddlSucursales.SelectedValue));
        }

        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlSucursales.DataSource = null;
            ddlSucursales.DataBind();
        }
        catch
        {
            //referencia nula
        }
    }

    /// <summary>
    /// Trae la lista filtrada de las sucursales fiscales.
    /// </summary>
    private void fnCargarSucursalesFiscal()
    {
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            ddlSucursalesFis.DataSource = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(true); //
            ddlSucursalesFis.DataBind();
        }

        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlSucursalesFis.DataSource = null;
            ddlSucursalesFis.DataBind();
        }
        catch
        {
            //referencia nula
        }
    }
    /// <summary>
    /// Recupera las series por sucursal y tipo de documento.
    /// </summary>
    private void fnCargarSeries()
    {
        try
        {
            int nSucursal = 0;
            if (cbAgrExpEn.Checked == true)
            {
                if (cbSerieFolioMatriz.Checked == false)
                    nSucursal = Convert.ToInt32(ddlSucursales.SelectedValue);
                else
                    nSucursal = Convert.ToInt32(ddlSucursalesFis.SelectedValue);
            }
            else
                nSucursal = Convert.ToInt32(ddlSucursalesFis.SelectedValue);

            DataTable table = new DataTable();

            table = clsTimbradoFuncionalidad.LlenarDropSeries(nSucursal, Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]));
            if (table.Rows.Count > 0)
            {
                ddlSerie.DataSource = table;
            }
            else
            {
                ddlSerie.Items.Clear();
                ddlSerie.Items.Add("N/A");
            }
            ddlSerie.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlSerie.DataSource = null;
            ddlSerie.DataBind();
        }
        catch (Exception ex)
        {
            //referencia nula
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlSerie.DataSource = null;
            ddlSerie.DataBind();
        }
    }

    /// <summary>
    /// Recupera los folios por tipo de sucursal y documento.
    /// </summary>
    private void fnCargarFolio()
    {
        try
        {
            int nSucursal = 0;
            if (cbAgrExpEn.Checked == true)
            {
                if (cbSerieFolioMatriz.Checked == false)
                    nSucursal = Convert.ToInt32(ddlSucursales.SelectedValue);
                else
                    nSucursal = Convert.ToInt32(ddlSucursalesFis.SelectedValue);
            }
            else
                nSucursal = Convert.ToInt32(ddlSucursalesFis.SelectedValue);

            //if (cbAgrExpEn.Checked == true)
            //    nSucursal = Convert.ToInt32(ddlSucursales.SelectedValue);
            //else
            //    nSucursal = Convert.ToInt32(ddlSucursalesFis.SelectedValue);
            DataTable table = new DataTable();
            table = clsTimbradoFuncionalidad.LlenarDropSeries(nSucursal, Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]));
            if (ddlSerie.SelectedValue != "N/A")
            {
                //txtFolio.Text = table.Rows[ddlSerie.SelectedIndex]["folio"].ToString(); /* 16 - 02 - 2013 */
                DataRow[] row = table.Select("id_serie=" + ddlSerie.SelectedValue);
                txtFolio.Text = row[0]["folio"].ToString();
            }
            else
            {
                txtFolio.Text = "0";
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            txtFolio.Text = "0";
        }
        catch (Exception ex)
        {
            //referencia nula
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            txtFolio.Text = "0";
        }
    }

    /// <summary>
    /// Recupera los folios por tipo de sucursal y documento al momento de generar documento.
    /// </summary>
    private string fnCargarFolioGen()
    {
        string sFolio = "0";
        int nSucursal = 0;
        //if (cbAgrExpEn.Checked == true)
        //    nSucursal = Convert.ToInt32(ddlSucursales.SelectedValue);
        //else
        //    nSucursal = Convert.ToInt32(ddlSucursalesFis.SelectedValue);

        if (cbAgrExpEn.Checked == true)
        {
            if (cbSerieFolioMatriz.Checked == false)
                nSucursal = Convert.ToInt32(ddlSucursales.SelectedValue);
            else
                nSucursal = Convert.ToInt32(ddlSucursalesFis.SelectedValue);
        }
        else
            nSucursal = Convert.ToInt32(ddlSucursalesFis.SelectedValue);

        try
        {
            clsOperacionSeriesFolios giSq = new clsOperacionSeriesFolios();
            DataTable table = new DataTable();
            table = clsTimbradoFuncionalidad.LlenarDropSeries(nSucursal, Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]));
            if (ddlSerie.SelectedValue != "N/A")
            {
                //sFolio = table.Rows[ddlSerie.SelectedIndex]["folio"].ToString();
                DataRow[] row = table.Select("id_serie=" + ddlSerie.SelectedValue);
                sFolio = row[0]["folio"].ToString();
                giSq.fnActualizarSerie(Convert.ToInt32(nSucursal), Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]), row[0]["serie"].ToString());
            }
            else
            {
                sFolio = "0";
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            sFolio = "0";
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            sFolio = "0";
        }

        return sFolio;
    }

    /// <summary>
    /// Trae los datos de los receptores con sucursales
    /// </summary>
    public void fnCargarDatosSucursal()
    {
        try
        {

            //Validaciones para el postback generado desde javascript
            string sControlId = Request["__EVENTTARGET"];

            if (sControlId != "linkReceptor")
                return;

            string parametros = Request["__EVENTARGUMENT"];

            if (string.IsNullOrEmpty(parametros))
                return;

            //************************************************

            fnRealizarBusquedaDatosReceptor(parametros);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }


    }

    private void fnRealizarBusquedaDatosReceptor(string parametros)
    {
        string sIdReceptor = parametros.Split(':')[0];
        txtRfcReceptor.Text = parametros.Split(':')[1];
        txtRazonReceptor.Text = parametros.Split(':')[2];

        DataTable tblDatos = new DataTable();
        //if (ddlRfc.Items.Count > 0)
        tblDatos = clsTimbradoFuncionalidad.RecuperaSucReceptor(Convert.ToInt32(sIdReceptor));

        if (tblDatos.Rows.Count > 0)
        {
            ddlSucursal.DataSource = tblDatos;
            ddlSucursal.DataBind();
            fnCargarDetallesReceptorSuc();
        }
        else
        {
            ddlSucursal.Items.Clear();
            ddlSucursal.DataBind();
            fnLimpiarControlesReceptor();
        }
    }

    /// <summary>
    /// Trae los datos por sucursal seleccion
    /// </summary>
    public void fnCargarDetallesReceptorSuc()
    {
        try
        {
            DataTable tblDatos = new DataTable();
            //if (ddlRfc.Items.Count > 0)
            if (ddlSucursal.Items.Count > 0)
            {
                tblDatos = clsTimbradoFuncionalidad.fnRecuperaDetallesReceptorSuc(Convert.ToInt32(ddlSucursal.SelectedValue));

                foreach (DataRow itemRow in tblDatos.Rows)
                {

                    txtPais.Text = itemRow["pais"].ToString();
                    txtEstado.Text = itemRow["estado"].ToString();
                    txtMunicipio.Text = itemRow["municipio"].ToString();
                    txtLocalidad.Text = itemRow["localidad"].ToString();
                    txtCalle.Text = itemRow["calle"].ToString();
                    txtNoExterior.Text = itemRow["numero_exterior"].ToString();
                    txtNoInterior.Text = itemRow["numero_interior"].ToString();
                    txtColonia.Text = itemRow["colonia"].ToString();
                    txtCodigoPostal.Text = itemRow["codigo_postal"].ToString();

                }
            }
            else
            {
                //limpiamos los controles
                txtRfcReceptor.Text = string.Empty;
                txtPais.Text = string.Empty;
                txtEstado.Text = string.Empty;
                txtMunicipio.Text = string.Empty;
                txtLocalidad.Text = string.Empty;
                txtCalle.Text = string.Empty;
                txtNoExterior.Text = string.Empty;
                txtNoInterior.Text = string.Empty;
                txtColonia.Text = string.Empty;
                txtCodigoPostal.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    /// <summary>
    /// Trae los tipos de documentos
    /// </summary>
    //public void fnCargarTipoDocumentos()
    //{
    //    datosUsuario = clsComun.fnUsuarioEnSesion();
    //    DataTable tblDatos = new DataTable();
    //    int idContribuyent = 0;

    //    if (datosUsuario != null)
    //        idContribuyent = datosUsuario.id_contribuyente;


    //    try
    //    {
    //        tblDatos = clsTimbradoFuncionalidad.fnLlenarTiposDocumentos(idContribuyent);
    //        ddlTipoDoc.DataSource = tblDatos;
    //        ddlTipoDoc.DataBind();

    //    }
    //    catch (SqlException ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
    //        ddlTipoDoc.DataSource = null;
    //        ddlTipoDoc.DataBind();
    //    }
    //}

    /// <summary>
    /// Llena el drop de selección en general para el tipo de documento al que se le asignará el impuesto
    /// </summary>
    private void fnCargarTipoDocumentosGen()
    {
        gDAL = new clsOperacionDocImpuestos();

        try
        {
            ddlTipoDoc.DataSource = gDAL.fnCargarTiposDocumentoGen();
            ddlTipoDoc.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    /// <summary>
    /// Limpiar los controles.
    /// </summary>
    //private void fnLimpiarControles()
    //{
    //    txtCodigoCon.Text = string.Empty;
    //    txtUnidadCon.Text = string.Empty;
    //    txtDescripcionCon.Text = string.Empty;
    //    txtPrecioCon.Text = "0";
    //    txtCantidadCon.Text = "0";
    //    txtDescuentoCon.Text = "0";
    //    txtIEPScon.Text = "0";
    //    txtImporteCon.Text = "0";
    //    lblNumerosLetras.Text = string.Empty;
    //    txtISRcpto.Text = "0";
    //    txtIvaRet.Text = "0";
    //}

    private void fnLimpiarControlesReceptor()
    {
        txtPais.Text = string.Empty;
        txtEstado.Text = string.Empty;
        txtMunicipio.Text = string.Empty;
        txtLocalidad.Text = string.Empty;
        txtCalle.Text = string.Empty;
        txtNoExterior.Text = string.Empty;
        txtNoInterior.Text = string.Empty;
        txtColonia.Text = string.Empty;
        txtCodigoPostal.Text = string.Empty;
    }

    private void fdtCrearDTImportes()
    {
        DataTable Impuestos = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "abreviacion";
        columna1.ColumnName = "abreviacion";
        columna1.DefaultValue = null;
        Impuestos.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "calculo";
        columna2.ColumnName = "calculo";
        columna2.DefaultValue = null;
        Impuestos.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "tasa";
        columna3.ColumnName = "tasa";
        columna3.DefaultValue = null;
        Impuestos.Columns.Add(columna3);

        DataColumn columna4 = new DataColumn();
        columna4.DataType = System.Type.GetType("System.String");
        columna4.AllowDBNull = true;
        columna4.Caption = "efecto";
        columna4.ColumnName = "efecto";
        columna4.DefaultValue = null;
        Impuestos.Columns.Add(columna4);

        ViewState["Impuestos"] = Impuestos;
    }
    /// <summary>
    /// Funcion para recargar los impuestos
    /// </summary>
    private void fnRecargaTipoImpuestos()
    {
        double IVA = 0;
        double IVA16 = 0;
        double IVA11 = 0;
        double IVA0 = 0;
        double ISR = 0;
        string descISR = string.Empty;
        double IEPS = 0;
        string descIEPS = string.Empty;
        string traslIEPS = string.Empty;
        string categoriaIEPS = string.Empty;
        double IVARet = 0;
        string descIVARet = string.Empty;
        double Subtotal = 0;
        double Descuento = 0;
        double Total = 0;
        int ban = 0;
        DataTable Impuestos = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "abreviacion";
        columna1.ColumnName = "abreviacion";
        columna1.DefaultValue = null;
        Impuestos.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "calculo";
        columna2.ColumnName = "calculo";
        columna2.DefaultValue = null;
        Impuestos.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "tasa";
        columna3.ColumnName = "tasa";
        columna3.DefaultValue = null;
        Impuestos.Columns.Add(columna3);

        DataColumn columna4 = new DataColumn();
        columna4.DataType = System.Type.GetType("System.String");
        columna4.AllowDBNull = true;
        columna4.Caption = "efecto";
        columna4.ColumnName = "efecto";
        columna4.DefaultValue = null;
        Impuestos.Columns.Add(columna4);

        datosUsuario = clsComun.fnUsuarioEnSesion();
        NumaletPago numLetras = new NumaletPago();
        numLetras.LetraCapital = true;
        DataSet resultado = new DataSet();
        tablePas = (DataTable)ViewState["table"];

        datosUsuario = clsComun.fnUsuarioEnSesion();
        //datosUsuario.version = "3.2";
        //Habilitar y deshabilitar controles segun la version
        switch (datosUsuario.version)
        {
            case "3.0":
                ForDec = "{0:c2}";
                break;
            case "3.2":
                ForDec = "{0:c6}";
                break;
        }

        try
        {
            if (grvDetalles.Rows.Count > 0)
            {
                //if (hdnItemMod.Value != "true")
                //{
                Descuento = Convert.ToDouble(lblDetDescuentoVal.Text.Replace("$", "").Replace(",", ""));

                foreach (DataRow renglon in tablePas.Rows)
                {
                    traslIEPS = renglon["trasladoIEPS"].ToString();

                    Subtotal += Convert.ToDouble(renglon["Subtotal"]);
                    if (renglon["descIVA"].ToString() == "16.00")
                        IVA16 += Convert.ToDouble(renglon["IVA"]);

                    if (renglon["descIVA"].ToString() == "11.00")
                        IVA11 += Convert.ToDouble(renglon["IVA"]);

                    if (renglon["descIVA"].ToString() == "0.00")
                    {
                        IVA0 += Convert.ToDouble(renglon["IVA"]);
                        ban = 1;
                    }

                    if (descISR == string.Empty || descISR == "0")
                        descISR = renglon["ValISR"].ToString();
                    ISR += Convert.ToDouble(renglon["ISR"]);

                    if (descIEPS == string.Empty || descIEPS == "0")
                        descIEPS = renglon["ValIEPS"].ToString();
                    IEPS += Convert.ToDouble(renglon["IEPS"]);



                    if (descIVARet == string.Empty || descIVARet == "0")
                        descIVARet = renglon["ValIVARet"].ToString();
                    IVARet += Convert.ToDouble(renglon["IVARet"]);
                    Total += Convert.ToDouble(renglon["Importe"]);
                }

                if (!txtDescuentoGlobal.Text.Equals(string.Empty) && IVA16 != 0)
                    IVA16 = (Subtotal - Descuento) * .16;
                if (!txtDescuentoGlobal.Text.Equals(string.Empty) && IVA11 != 0)
                    IVA11 = (Subtotal - Descuento) * .11;

                if (Convert.ToInt32(traslIEPS) <= 5 && Convert.ToInt32(traslIEPS) != 0)
                {

                    Total = (Total + IVA16 + IVA11 + IVA0 + IEPS) - (IVARet + ISR) - Descuento;
                }
                else if (Convert.ToInt32(traslIEPS) > 5 && Convert.ToInt32(traslIEPS) != 0)
                {
                    Total = (Total + IVA16 + IVA11 + IVA0) - (IVARet + ISR) - Descuento;
                }

                if (IVA16 != 0)
                {
                    DataRow nuevoiva = Impuestos.NewRow();
                    nuevoiva["abreviacion"] = "IVA";
                    nuevoiva["calculo"] = String.Format(ForDec, IVA16);//IVA);
                    nuevoiva["tasa"] = "16.00 %";
                    nuevoiva["efecto"] = "Traslado";
                    Impuestos.Rows.Add(nuevoiva);
                }

                if (IVA11 != 0)
                {
                    DataRow nuevoiva = Impuestos.NewRow();
                    nuevoiva["abreviacion"] = "IVA";
                    nuevoiva["calculo"] = String.Format(ForDec, IVA11);//IVA);
                    nuevoiva["tasa"] = "11.00 %";
                    nuevoiva["efecto"] = "Traslado";
                    Impuestos.Rows.Add(nuevoiva);
                }

                if (ban == 1)
                {
                    DataRow nuevoiva = Impuestos.NewRow();
                    nuevoiva["abreviacion"] = "IVA";
                    nuevoiva["calculo"] = String.Format(ForDec, IVA0);//IVA);
                    nuevoiva["tasa"] = "0.00 %";
                    nuevoiva["efecto"] = "Traslado";
                    Impuestos.Rows.Add(nuevoiva);
                }

                if (ISR != 0)
                {
                    DataRow nuevo = Impuestos.NewRow();
                    nuevo["abreviacion"] = "ISR";
                    nuevo["calculo"] = String.Format(ForDec, ISR);
                    nuevo["tasa"] = descISR + " %"; //clsComun.ObtenerParamentro("ISR") + " %";
                    nuevo["efecto"] = "Retención";
                    Impuestos.Rows.Add(nuevo);
                }

                if (IEPS != 0)
                {
                    DataRow nuevo = Impuestos.NewRow();
                    nuevo["abreviacion"] = "IEPS";
                    nuevo["calculo"] = String.Format(ForDec, IEPS);
                    nuevo["tasa"] = descIEPS + " %";
                    nuevo["efecto"] = "Traslado";
                    Impuestos.Rows.Add(nuevo);
                }

                if (IVARet != 0)
                {
                    double ivaret = Convert.ToDouble(descIVARet);//Convert.ToDouble(txtIVACon.SelectedValue);
                    //ivaret = (ivaret * 2) / 3;

                    DataRow nuevo = Impuestos.NewRow();
                    nuevo["abreviacion"] = "IVA Retenido";
                    nuevo["calculo"] = String.Format(ForDec, IVARet);
                    nuevo["tasa"] = Convert.ToString(Math.Round(ivaret, 0)) + " %";
                    nuevo["efecto"] = "Retención";

                    Impuestos.Rows.Add(nuevo);
                }

                if (tablePas.Rows.Count > 0)
                {
                    string sGrupoImp = string.Empty;
                    double dimporte = 0;

                    var impuestoIEPS = from row in tablePas.AsEnumerable()
                                       group row by row.Field<double>("valIEPS") into g
                                       select new
                                       {
                                           abreviacion = "IEPS",
                                           tasa = g.Key,
                                           calculo = g.Sum(r => r.Field<double>("IEPS"))
                                       };


                    DataTable dtResultado = impuestoIEPS.ToDataTable();

                    //Se recorre la tabla agregando los diferentes IEPS agregados
                    foreach (DataRow dr in dtResultado.Rows)
                    {
                        dimporte = Convert.ToDouble(dr["calculo"].ToString());

                        if (dimporte.Equals(0))
                        {
                            continue;
                        }

                        DataRow nuevo = Impuestos.NewRow();
                        nuevo["abreviacion"] = dr["abreviacion"].ToString();
                        nuevo["calculo"] = String.Format(ForDec, Convert.ToDecimal(dr["calculo"].ToString()));
                        double dTasa = Convert.ToDouble(dr["tasa"].ToString());
                        nuevo["tasa"] = String.Format("{0:n2}", dTasa) + " %";
                        nuevo["efecto"] = "Traslado";
                        Total += dimporte;

                        Impuestos.Rows.Add(nuevo);
                    }
                }

                //resultado = clsTimbradoFuncionalidad.fnRecuperaTipoImpuesto(datosUsuario.id_usuario, Convert.ToInt16(ddlTipoDoc.SelectedValue.Split('|')[0]), Convert.ToDecimal(tablePas.Compute("SUM(Importe)", "")));

                lblDetSubtotal.Text = String.Format(ForDec, Convert.ToDecimal(Subtotal));
                lblDetSubtotal.Visible = true;
                lblSubtotal.Visible = true;
                lblDetDescuento.Visible = true;
                lblDetDescuentoVal.Visible = true;

                lblTotalVal.Text = String.Format(ForDec, Convert.ToDecimal(Total));
                lblTotal.Visible = true;
                lblTotalVal.Visible = true;

                dtsTotales.Visible = true;
                dtsTotales.DataSource = Impuestos;
                dtsTotales.DataBind();

                fnCambiarTipoMoneda();
                //}
                //else
                //{
                //    fnRecargaTipoImpuestosModificados();
                //}
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    private void fdtCrearDTImpuestosArticulos()
    {
        DataTable Impuestos = new DataTable();

        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "abreviacion";
        columna1.ColumnName = "abreviacion";
        columna1.DefaultValue = null;
        Impuestos.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "calculo";
        columna2.ColumnName = "calculo";
        columna2.DefaultValue = null;
        Impuestos.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "tasa";
        columna3.ColumnName = "tasa";
        columna3.DefaultValue = null;
        Impuestos.Columns.Add(columna3);

        DataColumn columna4 = new DataColumn();
        columna4.DataType = System.Type.GetType("System.String");
        columna4.AllowDBNull = true;
        columna4.Caption = "efecto";
        columna4.ColumnName = "efecto";
        columna4.DefaultValue = null;
        Impuestos.Columns.Add(columna4);

        ViewState["ImpuestosArt"] = Impuestos;
    }


    /// <summary>
    /// Funcion encargada de recalcular los impuestos al vuelo
    /// </summary>
    private void fnRecargaTipoImpuestosModificados()
    {

        DataTable resultado = new DataTable();
        DataTable resultadoSinFormato = new DataTable();
        Label lbltasa = new Label();
        Label lblabreviasion = new Label();
        Label lblefecto = new Label();

        double tasa = 0;
        double subTotal = 0;
        double calculo = 0;
        double dTraslado = 0;
        double dRetencion = 0;
        double dDescuento = 0;
        bool ret = false;
        bool tra = false;

        resultado.Columns.Add("abreviacion");
        resultado.Columns.Add("calculo");
        resultado.Columns.Add("tasa");
        resultado.Columns.Add("efecto");

        resultadoSinFormato.Columns.Add("abreviacion");
        resultadoSinFormato.Columns.Add("calculo", typeof(double));
        resultadoSinFormato.Columns.Add("tasa");
        resultadoSinFormato.Columns.Add("efecto");

        try
        {
            if (grvDetalles.Rows.Count > 0)
            {
                dDescuento = Convert.ToDouble(lblDetDescuentoVal.Text.Replace("$", "").Replace(",", ""));

                foreach (DataListItem item in dtsTotales.Items)
                {
                    //Recupera los valores a recalcular
                    lbltasa = (Label)dtsTotales.Items[item.ItemIndex].FindControl("lblTasaVal");
                    lblabreviasion = (Label)dtsTotales.Items[item.ItemIndex].FindControl("lblTipoImpuesto");
                    lblefecto = (Label)dtsTotales.Items[item.ItemIndex].FindControl("lblTipoImpDoc");

                    //verificamos el efecto
                    if (lblefecto.Text == "Traslado")
                        tra = true;
                    else
                        ret = true;

                    //Realiza el calculo de los impuestos
                    tasa = Convert.ToDouble(lbltasa.Text.Replace(" %", ""));
                    subTotal = Convert.ToDouble(lblDetSubtotal.Text.Replace("$", "").Replace(",", ""));
                    calculo = (subTotal * tasa) / 100;
                    //Genera los registros para el datalist
                    resultado.Rows.Add(lblabreviasion.Text.Replace(":", ""), String.Format("{0:c6}", calculo), lbltasa.Text, lblefecto.Text);
                    resultadoSinFormato.Rows.Add(lblabreviasion.Text.Replace(":", ""), calculo, lbltasa.Text, lblefecto.Text);
                }
                //Obtiene el total con los impuestos recalculados
                if (tra)
                    dTraslado = Convert.ToDouble(resultadoSinFormato.Compute("SUM(calculo)", "efecto ='Traslado'"));
                if (ret)
                    dRetencion = Convert.ToDouble(resultadoSinFormato.Compute("SUM(calculo)", "efecto ='Retención'"));

                double total = subTotal - dDescuento - dRetencion + dTraslado;

                lblTotalVal.Text = String.Format("{0:c6}", total);
                lblTotal.Visible = true;
                lblTotalVal.Visible = true;

                dtsTotales.Visible = true;
                dtsTotales.DataSource = resultado;
                dtsTotales.DataBind();

                fnCambiarTipoMoneda();
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }


    private void fnCambiarTipoMoneda()
    {
        try
        {
            double dTotal = Convert.ToDouble(lblTotalVal.Text.ToString().Replace("$", "").Replace(",", ""));

            NumaletPago numLetras = new NumaletPago();
            numLetras.LetraCapital = true;

            switch (ddlMoneda.SelectedValue)
            {
                case "MXN":
                    numLetras.TipoMoneda = NumaletPago.Moneda.Peso;
                    break;

                case "USD":
                    numLetras.TipoMoneda = NumaletPago.Moneda.Dolar;
                    break;

                case "XEU":
                    numLetras.TipoMoneda = NumaletPago.Moneda.Euro;
                    break;

            }

            lblNumerosLetras.Text = numLetras.ToCustomString(dTotal);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    #endregion

    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx", false);
        }

    }

    //============= Panel Modal para la busqueda de receptores ========================

    protected void btnConsulta_Click(object sender, EventArgs e)
    {
        fnTraerReceptores();
        linkModal_ModalPopupExtender.Show();
    }

    /// <summary>
    /// realiza la búsqueda de los receptores disponibles
    /// </summary>
    private void fnTraerReceptores()
    {
        clsTimbradoFuncionalidad gDAL = new clsTimbradoFuncionalidad();

        try
        {
            string sIdEstructura = ddlSucursalesFis.SelectedValue;

            gdvReceptores.DataSource = gDAL.fnLlenarGridReceptores(sIdEstructura, txtRfcConsulta.Text, txtRazonSocialConsulta.Text);
            gdvReceptores.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void gdvReceptores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvReceptores.PageIndex = e.NewPageIndex;
        fnTraerReceptores();
        linkModal_ModalPopupExtender.Show();
    }

    protected void gdvReceptores_SelectedIndexChanged(object sender, EventArgs e)
    {
        string parametros = gdvReceptores.SelectedDataKey.Values["id_rfc_receptor"].ToString()
            + ":" + gdvReceptores.SelectedDataKey.Values["rfc_receptor"].ToString()
            + ":" + gdvReceptores.SelectedDataKey.Values["nombre_receptor"].ToString();

        ViewState["id_rfc_receptor"] = gdvReceptores.SelectedDataKey.Values["id_rfc_receptor"].ToString();

        gdvReceptores.DataSource = null;
        gdvReceptores.DataBind();

        fnRealizarBusquedaDatosReceptor(parametros);
    }

    //===========================================================================
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

    protected void btnAcepCreditos_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx", false);
    }

    protected void imgbtnComplemento_Click(object sender, ImageClickEventArgs e)
    {

    }

    //protected void btnPSG_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Complementos sComplemento = new Complementos();
    //        TablaComplementos = new DataTable();
    //        DateTime FechaPSG = Convert.ToDateTime(txtFechaIni2.Text);
    //        TablaComplementos = sComplemento.fnPSGECFD(tbNombrePSG.Text, tbRFCPSG.Text, tbCertificadoPSG.Text, FechaPSG, tbAutorizacionPSG.Text, tbSelloPSG.Text);
    //        ViewState["TablaComplementos"] = TablaComplementos;
    //        ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //       clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }
    //}
    //protected void btnLocales_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Complementos sComplemento = new Complementos();
    //        TablaComplementos = new DataTable();
    //        double Retencion = Convert.ToDouble(tbRetencionesLocales.Text);
    //        double Total = Convert.ToDouble(tbTotTrasLocales.Text);
    //        TablaComplementos = sComplemento.fnImpuestosLocales(tbVersionLocales.Text, Retencion, Total, false);
    //        ViewState["TablaComplementos"] = TablaComplementos;
    //        ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //       clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }
    //}
    //protected void btnIEDU_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Complementos sComplemento = new Complementos();
    //        TablaComplementos = new DataTable();
    //        TablaComplementos = sComplemento.fnIEDU(tbVersionIEDU.Text, tbNombreIEDU.Text, tbCurpIEDU.Text, tbNivelEduIEDU.SelectedValue, tbAutorizacionIEDU.Text, tbRFCIEDU.Text);
    //        ViewState["TablaComplementos"] = TablaComplementos;
    //        ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }
    //}
    //protected void btnECC_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Complementos sComplemento = new Complementos();
    //        TablaComplementos = new DataTable();
    //        double Total = Convert.ToDouble(tbTotalECC.Text);
    //        double Cantidad = Convert.ToDouble(tbCantidadECC.Text);
    //        DateTime FechaECC = Convert.ToDateTime(txtFechaIniECC.Text);
    //        double ValorUnitario = Convert.ToDouble(tbvalorunitarioECC.Text);
    //        double Importe = Convert.ToDouble(tbImporteECC.Text);
    //        double Tasa = Convert.ToDouble(tbtasaECC.Text);
    //        TablaComplementos = sComplemento.fnECC(tbtipooperacionECC.Text, tbCuentaECC.Text, Total, tbIdentificadorECC.Text, FechaECC, tbRFCECC.Text,
    //        tbClaveEstacionECC.Text, Cantidad, tbNombreCombustibleECC.Text, tbFolioECC.Text, ValorUnitario, Importe, ddlImpuestoECC.SelectedValue, Tasa);
    //        ViewState["TablaComplementos"] = TablaComplementos;
    //        ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }
    //}
    //protected void btnECB_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Complementos sComplemento = new Complementos();
    //        TablaComplementos = new DataTable();
    //        double Importe = Convert.ToDouble(tbImporteECB.Text);
    //        int Cuenta = Convert.ToInt32(tbCuentaECB.Text);
    //        DateTime FechaECB = Convert.ToDateTime(txtFechaIni0.Text);
    //        TablaComplementos = sComplemento.fnECB(tbVersionECB.Text, Cuenta, tbNombreECB.Text, tbPeriodoECB.Text, FechaECB, tbDecripcionECB.Text,
    //            Importe, tbRfcECB.Text);
    //        ViewState["TablaComplementos"] = TablaComplementos;
    //        ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }

    //}
    //protected void btnDonativas_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Complementos sComplemento = new Complementos();
    //        TablaComplementos = new DataTable();
    //        DateTime FechaDON = Convert.ToDateTime(txtFechaIni.Text);
    //        TablaComplementos = sComplemento.fnDonativas(tbVersionDon.Text, FechaDON, tbAutorizacionDon.Text, tbLeyendaDon.Text);
    //        ViewState["TablaComplementos"] = TablaComplementos;
    //        ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }
    //}
    //protected void btnDivisas_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Complementos sComplemento = new Complementos();
    //        TablaComplementos = new DataTable();
    //        TablaComplementos = sComplemento.fnDivisas(tbVersionDivisa.Text, ddloperacionDivisa.SelectedValue);
    //        ViewState["TablaComplementos"] = TablaComplementos;
    //        ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }
    //}
    //protected void btnDetallista_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Complementos sComplemento = new Complementos();
    //        TablaComplementos = new DataTable();
    //        double MontoTotal = Convert.ToDouble(tbMontototalDET.Text);
    //        double MontoDesc = Convert.ToDouble(tbmontodescDET.Text);
    //        TablaComplementos = sComplemento.fnDetallista(tbVersionDET.Text, ddldetalleDET.SelectedValue,
    //        TBParty1DET.Text, TBParty2DET.SelectedValue, ddlCargosDET.SelectedValue, ddltipodescuentoDET.SelectedValue, MontoTotal, MontoDesc, tbgln1Det.Text, tbgln2DET.Text, ddltipotransDET.SelectedValue, ddldocumentstatusDET.SelectedValue);
    //        ViewState["TablaComplementos"] = TablaComplementos;
    //        ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }
    //}

    //protected void btnPFIntegrante_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //    Complementos sComplemento = new Complementos();
    //    TablaComplementos = new DataTable();
    //    TablaComplementos = sComplemento.fnPFintegranteCoordinado(tbVersionPF.Text, txtclavevehicularPF.Text, tbPlacaPF.Text, tbRFCPF.Text);
    //    ViewState["TablaComplementos"] = TablaComplementos;
    //    ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }
    //}


    //protected void btnTerceros_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //    Complementos sComplemento = new Complementos();
    //    TablaComplementos = new DataTable();
    //    TablaComplementos = sComplemento.fnTerceros(tbVersionTerceros.Text, tbRFCTerceros.Text,ddlRetTer.SelectedValue,tbimporteretTer.Text,
    //        ddlTraTer.SelectedValue,tbtasaTraTer.Text,tbimportetraTer.Text, txtcalleT.Text, txtNumExtT.Text, txtNumIntT.Text, txtColoniaT.Text,
    //        txtLocalidadT.Text, txtReferenciaT.Text, txtMunicipioT.Text, txtEstadoT.Text, txtPaisT.Text, txtCodigoT.Text, txtnumeroT.Text,
    //        Convert.ToDateTime(txtFechaIniT.Text),txtaduanaT.Text,txtPredialT.Text,txtnombreT.Text);
    //    ViewState["TablaComplementos"] = TablaComplementos;
    //    ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }
    //}

    //protected void btnExtranjero_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //    Complementos sComplemento = new Complementos();
    //    TablaComplementos = new DataTable();
    //    DateTime FechaEXT = Convert.ToDateTime(txtFechaIni3.Text);
    //    FechaEXT = DateTime.Now;
    //    TablaComplementos = sComplemento.fnTuristaPasajeroExtranjero(tbVersionExt.Text, FechaEXT, ddltipotransitoExt.SelectedValue, ddlviaExt.SelectedValue,
    //    tbIdentificacionExt.Text, tbnumeroidEXT.Text, tbNacionalidadEXT.Text, tbIdentificadorTransporteEXT.Text);
    //    ViewState["TablaComplementos"] = TablaComplementos;
    //    ddlComplemento.Enabled = false;
    //    }
    //    catch
    //    {
    //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //    }
    //}

    //protected void btnVehicular_Click(object sender, EventArgs e)
    //{
    //      try
    //    {
    //    Complementos sComplemento = new Complementos();
    //    TablaComplementos = new DataTable();
    //    TablaComplementos = sComplemento.fnVentaVehicular(tbVersionVehicular.Text, tbClaveVehicularVehicular.Text);
    //    ViewState["TablaComplementos"] = TablaComplementos;
    //    ddlComplemento.Enabled = false;
    //    }
    //      catch
    //      {
    //          clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //      }
    //}

    //protected void btnLFicales_Click(object sender, EventArgs e)
    //{
    //      try
    //    {
    //    Complementos sComplemento = new Complementos();
    //    TablaComplementos = new DataTable();
    //    TablaComplementos = sComplemento.fnLeyendasFiscales(tbVersionLeyFis.Text, tbDiposicionFiscal.Text, txtnormaLeyFis.Text, tbLeyendaFis.Text);
    //    ViewState["TablaComplementos"] = TablaComplementos;
    //    ddlComplemento.Enabled = false;
    //    }
    //      catch
    //      {
    //          clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
    //      }
    //}

    protected void imgbtnCambiarComplemento_Click(object sender, ImageClickEventArgs e)
    {
        TablaComplementos = new DataTable();
        ddlComplemento.Enabled = true;
    }



    /// <summary>
    /// Actauliza los creditos disponibles.
    /// </summary>
    private void fnActualizaCreditos()
    {
        DataTable tlbCreditos = new DataTable();
        int idCredito = 0;
        int idEstructura = 0;
        double Creditos = 0;
        int nRetorno = 0;

        tlbCreditos = (DataTable)ViewState["dtCreditos"];

        if (tlbCreditos.Rows.Count > 0)
        {

            idCredito = Convert.ToInt32(tlbCreditos.Rows[0]["id_creditos"]);
            idEstructura = Convert.ToInt32(tlbCreditos.Rows[0]["id_estructura"]);
            Creditos = Convert.ToDouble(tlbCreditos.Rows[0]["creditos"]);

            nRetorno = clsTimbradoFuncionalidad.fnActualizarCreditos(idCredito, idEstructura, Creditos, "GT");

            clsTimbradoFuncionalidad.fnActualizarCreditosHistorico(idCredito, idEstructura, Creditos);
            datosUsuario = clsComun.fnUsuarioEnSesion();
            dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
            if (dtCreditos.Rows.Count > 0)
            {
                fnActualizarLblCreditos();
            }
            else
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                if (creditos > 0)
                {
                    fnActualizarLblCreditos();
                }
            }
        }


    }

    /// <summary>
    /// Actualizar etiqueta de Creditos.
    /// </summary>
    private bool fnActualizarLblCreditos()
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
        cCc = new clsConfiguracionCreditos();
        bool bRespuesta = true;
        double dCostCred = cCc.fnRecuperaPrecioServicio(4); //Precio servicio generación + timbrado
        if (dtCreditos.Rows.Count > 0)
        {
            double TCreditos = 0;
            TCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

            if (TCreditos == 0)
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                if (creditos == 0)
                {
                    lblCredValor.Text = "0";

                    Label121.Visible = false;
                    lblCosCre.Visible = false;
                    Label7.Visible = true;
                    modalCreditos.Show();

                    bRespuesta = false;
                }
                else
                {
                    double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                    lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                    //Se valida que tenga créditos suficientes
                    if (dCreditos < dCostCred)
                    {
                        Label121.Visible = false;
                        lblCosCre.Visible = true;
                        Label7.Visible = false;
                        modalCreditos.Show();

                        bRespuesta = false;
                    }
                }
            }
            else
            {
                double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                //Se valida que tenga créditos suficientes
                if (dCreditos < dCostCred)
                {
                    Label121.Visible = false;
                    lblCosCre.Visible = true;
                    Label7.Visible = false;
                    modalCreditos.Show();

                    bRespuesta = false;
                }
            }
        }
        else
        {
            clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
            if (creditos == 0)
            {
                lblCredValor.Text = "0";

                Label121.Visible = false;
                lblCosCre.Visible = false;
                Label7.Visible = true;
                modalCreditos.Show();

                bRespuesta = false;
            }
            else
            {
                double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                //Se valida que tenga créditos suficientes
                if (dCreditos < dCostCred)
                {
                    Label121.Visible = false;
                    lblCosCre.Visible = true;
                    Label7.Visible = false;
                    modalCreditos.Show();

                    bRespuesta = false;
                }
            }
        }

        return bRespuesta;
    }

    //protected void ddlArticulo_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (!(ddlArticulo.SelectedValue == ""))
    //        {
    //            clsOperacionArticulos gART = new clsOperacionArticulos();
    //            DataTable Tarticulos = new DataTable();
    //            Tarticulos = gART.fnObtieneArticulo(Convert.ToInt32(ddlArticulo.SelectedValue));
    //            foreach (DataRow renglon in Tarticulos.Rows)
    //            {
    //                txtMedidaArt.Text = Convert.ToString(renglon["medida"]);
    //                txtMonedaArt.Text = Convert.ToString(renglon["moneda"]);
    //                txtIVAArt.Text = Convert.ToString(renglon["iva"]);
    //                txtIEPSArt.Text = Convert.ToString(renglon["ieps"]);
    //                txtPrecioArt.Text = Convert.ToString(renglon["precio"]);
    //                txtISRArt.Text = Convert.ToString(renglon["isr"]);
    //                txtIvaRetArt.Text = Convert.ToString(renglon["ivaretenido"]);
    //            }
    //            txtCantidadArt.Enabled = true;
    //            txtImporteArt.Text = "0";
    //            txtCantidadArt.Text = "0";
    //        }
    //        else
    //        {
    //            txtMedidaArt.Text = string.Empty;
    //            txtMonedaArt.Text = string.Empty;
    //            txtIVAArt.Text = "0";
    //            //cbISRArt.Checked = false;
    //            //cbISRCon.Checked = false;
    //            txtISRArt.Text = "0";
    //            txtIvaRetArt.Text = "0";
    //            txtIEPSArt.Text = "0";
    //            txtPrecioArt.Text = "0";
    //            txtImporteArt.Text = "0";
    //            txtCantidadArt.Text = string.Empty;
    //            txtCantidadArt.Enabled = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
    //    }
    //}

    //public void fnArticulosEstructura(int pidEstructura)
    //{
    //    try
    //    {
    //        clsOperacionArticulos gART = new clsOperacionArticulos();
    //        ddlArticulo.DataSource = gART.fnObtieneArticulosEstructura(pidEstructura);
    //        ddlArticulo.DataBind();


    //    }
    //    catch(Exception ex)
    //    {          
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
    //        ddlArticulo.DataSource = null;
    //        ddlArticulo.DataBind();
    //    }
    //}

    //protected void txtCantidadArt_TextChanged(object sender, EventArgs e)
    //{
    //    fnCalcularImpuestos();
    //}

    public void fnCalcularImpuestos()
    {
        try
        {
            if (txtCantidadArt.Text == string.Empty)
            {
                txtCantidadArt.Text = "0";
            }
            if (txtPrecioArt.Text == string.Empty)
            {
                txtPrecioArt.Text = "0";
            }
            if (txtDescuentoArt.Text == string.Empty)
            {
                txtDescuentoArt.Text = "0";
            }
            double Cantidad = Convert.ToDouble(txtCantidadArt.Text);
            //double Porcentaje = Convert.ToDouble(txtPrecioArt.Text) * (Convert.ToDouble(txtDescuentoArt.Text) / 100);
            double Precio = (Convert.ToDouble(txtPrecioArt.Text)) * Cantidad;
            double Final = Precio;
            txtImporteArt.Text = Precio.ToString(); //Final.ToString("##0.00"); 

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void btnAgregarArt_Click(object sender, EventArgs e)
    {

        string sDescripcionIEPS = ddlIEPS.SelectedItem.Text;
        string sTrasladoIEPS = ddlIEPS.SelectedItem.Value;
        string sIndiceIEPS = ddlIEPS.SelectedIndex.ToString();

        double dDescuento = 0;
        if (txtDescripcionArt.Text != "" &&
            clsComun.fnIsDouble(txtPrecioArt.Text) == true)
        {
            //Verifica el complemento donativas en especie
            //El importe de los conceptos debe ser igual a cero
            string tipoDonativo = (Session["TipoDonativo"] != null) ? Session["TipoDonativo"].ToString() : string.Empty; //Tipos de donativos: "0" - Monetario, "1" - Especie

            if (ddlComplemento.SelectedValue == "Complementos/webComplementosDonatarias.aspx" && tipoDonativo == "1")
            {
                if ((Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text)) != 0)
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgImporteCeroDonativas, Resources.resCorpusCFDIEs.varContribuyente);
                    return;
                }
            }
            else if ((Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text)) == 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varImporteCero, Resources.resCorpusCFDIEs.varContribuyente);
                return;
            }

            

            if (txtDescuentoArt.Text == string.Empty)
                txtDescuentoArt.Text = "0";

            //if (txtIEPSArt.Text == string.Empty)
            //    txtIEPSArt.Text = "0";

            if (txtIvaRetArt.Text == string.Empty)
                txtIvaRetArt.Text = "0";

            if (txtISRArt.Text == string.Empty)
                txtISRArt.Text = "0";

            tablePas = (DataTable)ViewState["table"];
            //try
            //{
            //    DataView dv = new DataView(tablePas);
            //    dv.RowFilter = "codigo = '" + ViewState["codigoArt"] + "'";
            //    if (dv.Count > 0) //Si el articulo ya existe en la lista detalle se avisa de su existencia
            //    {
            //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varExisteArticulo, Resources.resCorpusCFDIEs.varContribuyente);
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            if (cbAgrCatArt.Checked) //Si selecciona agregar artículo al catalogo
                btnGuardarArt_Click(sender, e);

            fnCalcularImpuestos();

            //Obteniendo valores del dropdown list de IEPS

            //Revisar si existe en gridview detalle. 
            if (ViewState["id_Registros"].ToString() != string.Empty)
            {
                DataRow findRow;

                findRow = tablePas.Rows.Find(ViewState["id_Registros"].ToString());

                if (findRow != null)
                {
                    if (Session["psIdArticulo"] != string.Empty && Session["psIdArticulo"] != null)
                        findRow["idarticulo"] = Session["psIdArticulo"];

                    findRow["Codigo"] = txtCodigoArt.Text;//ViewState["codigoArt"]; //ddlArticulo.SelectedValue;
                    findRow["Unidad"] = txtMedidaArt.Text;
                    findRow["Descripcion"] = txtDescripcionArt.Text; //ddlArticulo.SelectedItem.Text;

                    if (!Convert.ToDouble(txtDescuentoArt.Text).Equals(0))
                    {
                        dDescuento = Convert.ToDouble(txtDescuentoArt.Text);
                        ViewState["TipoDescuento"] = "PorArticulo";
                    }
                    else if (!Convert.ToDouble(txtDescuentoGlobal.Text).Equals(0))
                    {
                        ViewState["TipoDescuento"] = "Global";
                    }

                    findRow["PorcentajeDescuento"] = dDescuento;
                    double Porcentaje = (Convert.ToDouble(txtPrecioArt.Text) * dDescuento) / 100;
                    findRow["Descuento"] = Porcentaje * Convert.ToDouble(txtCantidadArt.Text);

                    double Precio = Convert.ToDouble(txtPrecioArt.Text) - Porcentaje;

                    if (Convert.ToInt32(sTrasladoIEPS) <= 5)
                    {
                        findRow["PrecioUnitario"] = txtPrecioArt.Text;
                        findRow["Importe"] = Convert.ToDouble(txtImporteArt.Text);
                        findRow["Subtotal"] = Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text);
                    }
                    else if (Convert.ToInt32(sTrasladoIEPS) > 5)
                    {
                        if (cbIEPS.Checked)
                        {
                            // findRow["PrecioUnitario"] = (Convert.ToDouble(txtIEPS.Text) * Convert.ToDouble(txtCantidadArt.Text)) + Convert.ToDouble(txtPrecioArt.Text);
                            findRow["PrecioUnitario"] = (Convert.ToDouble(txtIEPS.Text) + Convert.ToDouble(txtPrecioArt.Text));
                            findRow["Importe"] = (Convert.ToDouble(txtIEPS.Text) * Convert.ToDouble(txtCantidadArt.Text)) + (Convert.ToDouble(txtCantidadArt.Text) * Convert.ToDouble(txtPrecioArt.Text));
                        }
                        else
                        {
                            findRow["PrecioUnitario"] = ((Precio) * (Convert.ToDouble(txtIEPS.Text) / 100)) + Convert.ToDouble(txtPrecioArt.Text);
                            findRow["Importe"] = Convert.ToDouble(findRow["PrecioUnitario"]) * Convert.ToDouble(txtCantidadArt.Text);
                        }

                        findRow["Subtotal"] = findRow["Importe"];
                    }

                    findRow["Cantidad"] = txtCantidadArt.Text;
                    //findRow["Importe"] = Convert.ToDouble(txtImporteArt.Text);
                    //findRow["Subtotal"] = Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text);

                    findRow["descIVA"] = ddlIVAArt.SelectedItem.Text;
                    if (ddlIVAArt.SelectedValue != "Exento")
                        findRow["IVA"] = (Convert.ToDouble(txtCantidadArt.Text) * Precio) * Convert.ToDouble(ddlIVAArt.SelectedItem.Text) / 100;
                    else
                        findRow["IVA"] = 0;

                    if (Convert.ToDecimal(txtIvaRetArt.Text) != 0 && txtIvaRetArt.Text != string.Empty) //if (cbIVAArt.Checked == true)
                    {
                        findRow["IVARet"] = (Convert.ToDouble(txtCantidadArt.Text) * Precio) * Convert.ToDouble(txtIvaRetArt.Text) / 100;//(Convert.ToDouble(txtIVAArt.Text) * 2 / 3) / 100;
                        findRow["valIvaRet"] = txtIvaRetArt.Text;
                    }
                    else
                    {
                        findRow["IVARet"] = 0;
                        findRow["valIvaRet"] = 0;
                    }

                    //Calculo del IEPS
                    findRow["descIEPS"] = txtIEPS.Text;
                    findRow["IEPSConImporte"] = false;

                    findRow["categoriaIEPS"] = sDescripcionIEPS;
                    findRow["trasladoIEPS"] = sTrasladoIEPS;
                    findRow["indexIEPS"] = sIndiceIEPS;

                    //Carga valor IEPS
                    if (string.IsNullOrEmpty(txtIEPS.Text) || Convert.ToDecimal(txtIEPS.Text).Equals(0) || sTrasladoIEPS.Equals("0") || Convert.ToInt32(sTrasladoIEPS) > 5)
                        findRow["IEPS"] = 0;
                    else
                    {
                        if (cbIEPS.Checked)
                            findRow["IEPS"] = Convert.ToDouble(txtIEPS.Text) * Convert.ToDouble(txtCantidadArt.Text); //(Convert.ToDouble(txtCantidadArt.Text) * Precio) * Convert.ToDouble(txtIEPSArt.Text) / 100;
                        else
                            findRow["IEPS"] = (Convert.ToDouble(txtCantidadArt.Text) * Precio) * (Convert.ToDouble(txtIEPS.Text) / 100);
                    }

                    //Carga el porcentaje del IEPS
                    if (string.IsNullOrEmpty(txtIEPS.Text) || Convert.ToDecimal(txtIEPS.Text).Equals(0) || sTrasladoIEPS.Equals("0") || Convert.ToInt32(sTrasladoIEPS) > 5)
                        findRow["valIEPS"] = 0;
                    else
                    {
                        if (cbIEPS.Checked)
                            findRow["valIEPS"] = (((Convert.ToDouble(txtIEPS.Text) * Convert.ToDouble(txtCantidadArt.Text)) * 100) / (Convert.ToDouble(txtCantidadArt.Text) * Convert.ToDouble(txtPrecioArt.Text)));
                        else
                            findRow["valIEPS"] = Convert.ToDouble(txtIEPS.Text);
                    }

                    findRow["IEPSConImporte"] = cbIEPS.Checked;

                    if (Convert.ToDecimal(txtISRArt.Text) != 0 && txtISRArt.Text != string.Empty) //if (cbISRArt.Checked == true)
                    {
                        findRow["ISR"] = (Convert.ToDouble(txtCantidadArt.Text) * Precio) * Convert.ToDouble(txtISRArt.Text) / 100; //Convert.ToDouble(clsComun.ObtenerParamentro("ISR")) / 100;
                        findRow["valISR"] = txtISRArt.Text;
                    }
                    else
                    {
                        findRow["ISR"] = 0;
                        findRow["valISR"] = 0;
                    }

                    dtCompl = (DataTable)ViewState["dtImpCompl"];
                    if (dtCompl.Rows.Count > 0)
                    {
                        //Se buscan registros para recalcular importe
                        foreach (DataRow dr in dtCompl.Rows)
                        {
                            if (dr["id_registros"].ToString() == ViewState["id_Registros"].ToString())
                            {
                                if (!Convert.ToBoolean(dr["ConImporte"].ToString()))
                                {
                                    //Se recalcula el importe de impuesto complemento
                                    dr["Importe"] = (Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text)) * Convert.ToDouble(dr["Tasa"].ToString()) / 100;
                                }
                                else
                                {
                                    //Se calcula la tasa de impuesto complemento
                                    dr["Tasa"] = (Convert.ToDouble(dr["Importe"].ToString()) * 100) / (Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text));
                                }
                            }
                        }
                    }

                    findRow["Estatus"] = "A";
                }

                ViewState["id_Registros"] = string.Empty;
            }

            else
            {
                DataRow row = tablePas.NewRow();
                if (Session["psIdArticulo"] != string.Empty && Session["psIdArticulo"] != null)
                    row["idarticulo"] = Session["psIdArticulo"];

                row["Codigo"] = txtCodigoArt.Text;//ViewState["codigoArt"]; //ddlArticulo.SelectedValue;
                row["Unidad"] = txtMedidaArt.Text;
                row["Descripcion"] = txtDescripcionArt.Text; //ddlArticulo.SelectedItem.Text;

                if (!Convert.ToDouble(txtDescuentoArt.Text).Equals(0))
                {
                    dDescuento = Convert.ToDouble(txtDescuentoArt.Text);
                    ViewState["TipoDescuento"] = "PorArticulo";
                }
                else if (!Convert.ToDouble(txtDescuentoGlobal.Text).Equals(0))
                {
                    ViewState["TipoDescuento"] = "Global";
                }

                row["PorcentajeDescuento"] = dDescuento;
                double Porcentaje = (Convert.ToDouble(txtPrecioArt.Text) * dDescuento) / 100;
                row["Descuento"] = Porcentaje * Convert.ToDouble(txtCantidadArt.Text);

                double Precio = Convert.ToDouble(txtPrecioArt.Text) - Porcentaje;

                if (Convert.ToInt32(sTrasladoIEPS) <= 5)
                {
                    row["PrecioUnitario"] = txtPrecioArt.Text;
                    row["Importe"] = Convert.ToDouble(txtImporteArt.Text);
                    row["Subtotal"] = Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text);
                }
                else if (Convert.ToInt32(sTrasladoIEPS) > 5)
                {
                    if (cbIEPS.Checked)
                    {
                        // row["PrecioUnitario"] = (Convert.ToDouble(txtIEPS.Text) * Convert.ToDouble(txtCantidadArt.Text)) + Convert.ToDouble(txtPrecioArt.Text);
                        row["PrecioUnitario"] = (Convert.ToDouble(txtIEPS.Text) + Convert.ToDouble(txtPrecioArt.Text));
                        row["Importe"] = (Convert.ToDouble(txtIEPS.Text) * Convert.ToDouble(txtCantidadArt.Text)) + (Convert.ToDouble(txtCantidadArt.Text) * Convert.ToDouble(txtPrecioArt.Text));
                    }
                    else
                    {
                        row["PrecioUnitario"] = ((Precio) * (Convert.ToDouble(txtIEPS.Text) / 100)) + Convert.ToDouble(txtPrecioArt.Text);
                        row["Importe"] = Convert.ToDouble(row["PrecioUnitario"]) * Convert.ToDouble(txtCantidadArt.Text);
                    }

                    row["Subtotal"] = row["Importe"];
                }

                row["Cantidad"] = txtCantidadArt.Text;
                //row["Importe"] = Convert.ToDouble(txtImporteArt.Text);
                //row["Subtotal"] = Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text);

                row["descIVA"] = ddlIVAArt.SelectedItem.Text;
                if (ddlIVAArt.SelectedValue != "Exento")
                    row["IVA"] = (Convert.ToDouble(txtCantidadArt.Text) * Precio) * Convert.ToDouble(ddlIVAArt.SelectedItem.Text) / 100;
                else
                    row["IVA"] = 0;

                if (Convert.ToDecimal(txtIvaRetArt.Text) != 0 && txtIvaRetArt.Text != string.Empty) //if (cbIVAArt.Checked == true)
                {
                    row["IVARet"] = (Convert.ToDouble(txtCantidadArt.Text) * Precio) * Convert.ToDouble(txtIvaRetArt.Text) / 100;//(Convert.ToDouble(txtIVAArt.Text) * 2 / 3) / 100;
                    row["valIvaRet"] = txtIvaRetArt.Text;
                }
                else
                {
                    row["IVARet"] = 0;
                    row["valIvaRet"] = 0;
                }

                //Calculo del IEPS
                row["descIEPS"] = txtIEPS.Text;
                row["IEPSConImporte"] = false;

                row["categoriaIEPS"] = sDescripcionIEPS;
                row["trasladoIEPS"] = sTrasladoIEPS;
                row["indexIEPS"] = sIndiceIEPS;

                if (string.IsNullOrEmpty(txtIEPS.Text) || Convert.ToDecimal(txtIEPS.Text).Equals(0) || sTrasladoIEPS.Equals("0") || Convert.ToInt32(sTrasladoIEPS) > 5)
                    row["IEPS"] = 0;
                else
                {
                    if (cbIEPS.Checked)
                        row["IEPS"] = Convert.ToDouble(txtIEPS.Text) * Convert.ToDouble(txtCantidadArt.Text); //(Convert.ToDouble(txtCantidadArt.Text) * Precio) * Convert.ToDouble(txtIEPSArt.Text) / 100;
                    else
                        row["IEPS"] = (Convert.ToDouble(txtCantidadArt.Text) * Precio) * (Convert.ToDouble(txtIEPS.Text) / 100);
                }

                if (string.IsNullOrEmpty(txtIEPS.Text) || Convert.ToDecimal(txtIEPS.Text).Equals(0) || sTrasladoIEPS.Equals("0") || Convert.ToInt32(sTrasladoIEPS) > 5)
                    row["valIEPS"] = 0;
                else
                {

                    if (cbIEPS.Checked)
                        row["valIEPS"] = (((Convert.ToDouble(txtIEPS.Text) * Convert.ToDouble(txtCantidadArt.Text)) * 100) / (Convert.ToDouble(txtCantidadArt.Text) * Convert.ToDouble(txtPrecioArt.Text)));
                    else
                        row["valIEPS"] = Convert.ToDouble(txtIEPS.Text);
                }

                row["IEPSConImporte"] = cbIEPS.Checked;

                if (Convert.ToDecimal(txtISRArt.Text) != 0 && txtISRArt.Text != string.Empty)//if (cbISRArt.Checked == true)
                {
                    row["ISR"] = (Convert.ToDouble(txtCantidadArt.Text) * Precio) * Convert.ToDouble(txtISRArt.Text) / 100;
                    row["valISR"] = txtISRArt.Text;
                }
                else
                {
                    row["ISR"] = 0;
                    row["valISR"] = 0;
                }

                row["Estatus"] = "A";

                tablePas.Rows.Add(row);

                //Se verifica si existe complementos agregados en la tabla
                dtCompl = (DataTable)ViewState["dtImpCompl"];
                if (dtCompl.Rows.Count > 0)
                {
                    //Se obtiene id de ultmo registro insertado para relacionarlo al complemento agregado
                    int id_reg = Convert.ToInt32(tablePas.Rows[tablePas.Rows.Count - 1]["id_registros"]);

                    //Se buscan registros sin asignar artículo
                    foreach (DataRow dr in dtCompl.Rows)
                    {
                        if (dr["id_registros"].ToString() == "0")
                        {
                            dr["id_registros"] = id_reg.ToString();

                            if (!Convert.ToBoolean(dr["ConImporte"].ToString()))
                            {
                                //Se recalcula el importe de impuesto complemento
                                dr["Importe"] = (Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text)) * Convert.ToDouble(dr["Tasa"].ToString()) / 100;
                            }
                            else
                            {
                                //Se calcula la tasa de impuesto complemento
                                dr["Tasa"] = (Convert.ToDouble(dr["Importe"].ToString()) * 100) / (Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text));
                            }
                        }
                    }

                    ViewState["dtImpCompl"] = dtCompl;
                }

                //Verificamos si existe información Aduanal agregada
                dtInfoAduanal = (DataTable)ViewState["dtInfAduanal"];
                if (dtInfoAduanal.Rows.Count > 0)
                {
                    //Se obtiene id de ultmo registro insertado para relacionarlo al complemento agregado
                    int id_reg = Convert.ToInt32(tablePas.Rows[tablePas.Rows.Count - 1]["id_registros"]);

                    foreach (DataRow dr in dtInfoAduanal.Rows)
                    {
                        if (dr["id_registros"].ToString() == "0")
                        {
                            dr["id_registros"] = id_reg.ToString();
                        }
                    }
                    ViewState["dtInfAduanal"] = dtInfoAduanal;
                }

                //Verificamos si existe complemento de terceros
                dtComplTerceros = (DataTable)ViewState["dtComplTerceros"];
                if (dtComplTerceros.Rows.Count > 0)
                {
                    //Se obtiene id de ultmo registro insertado para relacionarlo al complemento agregado
                    int id_reg = Convert.ToInt32(tablePas.Rows[tablePas.Rows.Count - 1]["id_registros"]);

                    foreach (DataRow dr in dtComplTerceros.Rows)
                    {
                        if (dr["id_registros"].ToString() == "0")
                        {
                            dr["id_registros"] = id_reg.ToString();
                        }
                    }
                    ViewState["dtComplTerceros"] = dtComplTerceros;
                }


                ViewState["table"] = tablePas;
            }

            datosUsuario = clsComun.fnUsuarioEnSesion();
            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "btnAgregarArt" + "|" +
                "Agrega un concepto nuevo." + "|" + txtCodigoArt.Text + "|" + txtMedidaArt.Text + "|" + txtDescripcionArt.Text + "|"
                + txtPrecioArt.Text + "|" + txtCantidadArt.Text);

            //Habilitar y deshabilitar controles segun la version
            switch (datosUsuario.version)
            {
                case "3.0":
                    ForDec = "{0:c2}";
                    break;
                case "3.2":
                    ForDec = "{0:c6}";
                    break;
            }

            lblSubtotal.Visible = true;
            lblDetSubtotal.Visible = true;
            //Recupera Subtotal
            lblDetSubtotal.Text = String.Format(ForDec, tablePas.Compute("SUM(Importe)", ""));

            if (Convert.ToDouble(txtDescuentoGlobal.Text).Equals(0))
            {
                lblDetDescuentoVal.Text = String.Format(ForDec, tablePas.Compute("SUM(Descuento)", ""));
                if (!Convert.ToDouble(txtDescuentoArt.Text).Equals(0))
                {
                    txtDescuentoGlobal.Enabled = false;
                    chkDescuentoGlobal.Enabled = false;
                }
            }
            else if (!Convert.ToDouble(txtDescuentoGlobal.Text).Equals(0))
            {
                if (chkDescuentoGlobal.Checked)
                {
                    if (!lblDetSubtotal.Text.Equals(string.Empty))
                        lblDetDescuentoVal.Text = String.Format(ForDec, (Convert.ToDouble(lblDetSubtotal.Text.Replace("$", "").Replace(",", "")) * Convert.ToDouble(txtDescuentoGlobal.Text)) / 100);
                }
                else
                    lblDetDescuentoVal.Text = String.Format(ForDec, Convert.ToDouble(txtDescuentoGlobal.Text));

                txtDescuentoGlobal.Enabled = false;
                chkDescuentoGlobal.Enabled = false;
            }
            else
            {

            }

            grvDetalles.DataSource = tablePas;
            grvDetalles.DataBind();

            fnRecargaTipoImpuestosArticulos();

            //Se limpia grid complementos
            grvImpuestoCompl.DataSource = string.Empty;
            grvImpuestoCompl.DataBind();

            fLimpiarCamposArt();
            txtImpLoc.Text = string.Empty;
            txtImpTasa.Text = "0";
            txtImpImporte.Text = "0";

            //Limpiamos campos info Aduanal
            txtAduana.Text = string.Empty;
            txtDocAduana.Text = string.Empty;
            txtFecha_CalendarExtender_Adu.SelectedDate = DateTime.Now;
            grvInfAduanal.DataSource = string.Empty;
            grvInfAduanal.DataBind();

            //Se limpia grid complementos terceros
            txtFechaIniT_CalendarExtender.SelectedDate = DateTime.Now;
            grvComplTerceros.DataSource = string.Empty;
            grvComplTerceros.DataBind();

            //limpiar panel IEPS
            txtIEPS.Text = "0";
            ddlIEPS.SelectedValue = "0";

            //Ocultar panel Impuestos Locales
            fnOcultarPnlImpuestosCompl();

            //Ocultamos panel Información Aduanal
            fnOcualtarPnlInfoAduanal();

            //Ocultar panel Complemento Terceros
            fnOcultarPnlComplTerceros();

            //Ocultar panel IEPS
            fnOcultarPnlIEPS();

        }
        else
        {
            //clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError, Resources.resCorpusCFDIEs.varContribuyente);
            return;
        }
    }

    private void fnRecargaTipoImpuestosArticulos()
    {
        //Cálculo de impuestos de árticulos
        double IVA16, IVA11, IVA0, ISR, valIEPS, IEPS, IVARet, Subtotal, Descuento, Total;
        IVA16 = IVA11 = IVA0 = ISR = valIEPS = IEPS = IVARet = Subtotal = Total = 0;
        string descISR, descIVARet, valISH, valCNG;
        descISR = descIVARet = descIVARet = valISH = valCNG = string.Empty;
        int ban = 0;


        string descIEPS = string.Empty;
        string traslIEPS = string.Empty;
        string categoriaIEPS = string.Empty;

        DataTable Impuestos = new DataTable();
        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "abreviacion";
        columna1.ColumnName = "abreviacion";
        columna1.DefaultValue = null;
        Impuestos.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "calculo";
        columna2.ColumnName = "calculo";
        columna2.DefaultValue = null;
        Impuestos.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.String");
        columna3.AllowDBNull = true;
        columna3.Caption = "tasa";
        columna3.ColumnName = "tasa";
        columna3.DefaultValue = null;
        Impuestos.Columns.Add(columna3);

        DataColumn columna4 = new DataColumn();
        columna4.DataType = System.Type.GetType("System.String");
        columna4.AllowDBNull = true;
        columna4.Caption = "efecto";
        columna4.ColumnName = "efecto";
        columna4.DefaultValue = null;
        Impuestos.Columns.Add(columna4);

        datosUsuario = clsComun.fnUsuarioEnSesion();
        NumaletPago numLetras = new NumaletPago();
        numLetras.LetraCapital = true;
        DataSet resultado = new DataSet();
        tablePas = (DataTable)ViewState["table"];

        datosUsuario = clsComun.fnUsuarioEnSesion();
        string ForNum = string.Empty;
        //Habilitar y deshabilitar controles segun la version
        switch (datosUsuario.version)
        {
            case "3.0":
                ForDec = "{0:c2}";
                ForNum = "{0:n2}";
                break;
            case "3.2":
                ForDec = "{0:c6}";
                ForNum = "{0:n6}";
                break;
        }

        try
        {
            if (grvDetalles.Rows.Count > 0)
            {
                //if (hdnItemMod.Value != "true")
                //{
                Descuento = Convert.ToDouble(lblDetDescuentoVal.Text.Replace("$", "").Replace(",", ""));

                foreach (DataRow renglon in tablePas.Rows)
                {
                    traslIEPS = renglon["trasladoIEPS"].ToString();

                    Subtotal += Convert.ToDouble(renglon["Subtotal"]);
                    if (renglon["descIVA"].ToString() == "16.00")
                        IVA16 += Convert.ToDouble(renglon["IVA"]);

                    if (renglon["descIVA"].ToString() == "11.00")
                        IVA11 += Convert.ToDouble(renglon["IVA"]);

                    if (renglon["descIVA"].ToString() == "0.00")
                    {
                        IVA0 += Convert.ToDouble(renglon["IVA"]);
                        ban = 1;
                    }

                    if (descISR == string.Empty || descISR == "0")
                        descISR = renglon["ValISR"].ToString();
                    ISR += Convert.ToDouble(renglon["ISR"]);

                    if (descIVARet == string.Empty || descIVARet == "0")
                        descIVARet = renglon["ValIVARet"].ToString();
                    IVARet += Convert.ToDouble(renglon["IVARet"]);
                    if (descIVARet == string.Empty || descIVARet == "0")
                        descIVARet = renglon["ValIVARet"].ToString();
                    //if (valISH == string.Empty || valISH == "0")
                    //    valISH = renglon["valISH"].ToString();
                    //ISH += Convert.ToDouble(renglon["ISH"]);
                    //if (valCNG == string.Empty || valCNG == "0")
                    //    valCNG = renglon["valCNG"].ToString();
                    //CNG += Convert.ToDouble(renglon["CNG"]);

                    Total += Convert.ToDouble(renglon["Importe"]);

                }

                if (!Convert.ToDouble(txtDescuentoGlobal.Text).Equals(0) && IVA16 != 0)
                    IVA16 = (Subtotal - Descuento) * .16;
                if (!Convert.ToDouble(txtDescuentoGlobal.Text).Equals(0) && IVA11 != 0)
                    IVA11 = (Subtotal - Descuento) * .11;

                if (Convert.ToInt32(traslIEPS) <= 5)
                {
                    Total = (Total + IVA16 + IVA11 + IVA0 + IEPS) - (IVARet + ISR) - Descuento;
                }
                else if (Convert.ToInt32(traslIEPS) > 5 && Convert.ToInt32(traslIEPS) != 0)
                {
                    Total = (Total + IVA16 + IVA11 + IVA0) - (IVARet + ISR) - Descuento;
                }

                if (IVA16 != 0)
                {
                    DataRow nuevoiva = Impuestos.NewRow();
                    nuevoiva["abreviacion"] = "IVA";
                    nuevoiva["calculo"] = String.Format(ForDec, IVA16);
                    nuevoiva["tasa"] = "16.00 %";
                    nuevoiva["efecto"] = "Traslado";
                    Impuestos.Rows.Add(nuevoiva);
                }

                if (IVA11 != 0)
                {
                    DataRow nuevoiva = Impuestos.NewRow();
                    nuevoiva["abreviacion"] = "IVA";
                    nuevoiva["calculo"] = String.Format(ForDec, IVA11);//IVA);
                    nuevoiva["tasa"] = "11.00 %";
                    nuevoiva["efecto"] = "Traslado";
                    Impuestos.Rows.Add(nuevoiva);
                }

                if (ban == 1)
                {
                    DataRow nuevoiva = Impuestos.NewRow();
                    nuevoiva["abreviacion"] = "IVA";
                    nuevoiva["calculo"] = String.Format(ForDec, IVA0);//IVA);
                    nuevoiva["tasa"] = "0.00 %";
                    nuevoiva["efecto"] = "Traslado";
                    Impuestos.Rows.Add(nuevoiva);
                }

                if (ISR != 0)
                {
                    DataRow nuevo = Impuestos.NewRow();
                    nuevo["abreviacion"] = "ISR";
                    nuevo["calculo"] = String.Format(ForDec, ISR);
                    nuevo["tasa"] = descISR + " %";//clsComun.ObtenerParamentro("ISR") + " %";
                    nuevo["efecto"] = "Retención";
                    Impuestos.Rows.Add(nuevo);
                }

                if (IVARet != 0)
                {
                    double ivaret = Convert.ToDouble(descIVARet);//Convert.ToDouble(txtIVAArt.Text);
                    //ivaret = (ivaret * 2)/3;
                    DataRow nuevo = Impuestos.NewRow();
                    nuevo["abreviacion"] = "IVA Retenido";
                    nuevo["calculo"] = String.Format(ForDec, IVARet);
                    nuevo["tasa"] = String.Format("{0:n6}", ivaret) + " %";
                    //nuevo["tasa"] = descIVARet + " %";
                    nuevo["efecto"] = "Retención";
                    Impuestos.Rows.Add(nuevo);
                }

                //Si existe impuesto complemento
                dtCompl = (DataTable)ViewState["dtImpCompl"];
                if (dtCompl.Rows.Count > 0)
                {
                    string sGrupoImp = string.Empty;
                    int nB = 0;
                    double dimporte = 0;
                    //Se recorre la tabla buscando los complementos agregados
                    foreach (DataRow dr in dtCompl.Rows)
                    {
                        foreach (DataRow drImp in Impuestos.Rows)
                        {
                            //Se busca si ya se agrego el complemento a la tabla de impuestos
                            if (dr["Descripcion"].ToString().ToUpper() == drImp["abreviacion"].ToString().ToUpper())
                            {
                                nB = 1;
                                break;
                            }
                            else
                            {
                                nB = 0;
                            }
                        }

                        if (nB == 0)
                        {
                            //Se agrega el complemento con el total de importe a la taba de impuestos
                            dimporte = Convert.ToDouble((dtCompl.Compute("SUM(Importe)", "Descripcion = '" + dr["Descripcion"].ToString() + "'")));

                            DataRow nuevo = Impuestos.NewRow();
                            nuevo["abreviacion"] = dr["Descripcion"].ToString();
                            nuevo["calculo"] = String.Format(ForDec, dimporte);
                            double dTasa = Convert.ToDouble(dr["tasa"].ToString());
                            nuevo["tasa"] = String.Format("{0:n6}", dTasa) + " %";
                            if (dr["Tipo"].ToString() == "T")
                            {
                                nuevo["efecto"] = "Traslado";
                                Total += dimporte;
                            }
                            else
                            {
                                nuevo["efecto"] = "Retención";
                                Total -= dimporte;
                            }
                            Impuestos.Rows.Add(nuevo);
                        }
                    }
                }

                if (tablePas.Rows.Count > 0)
                {
                    string sGrupoImp = string.Empty;
                    double dimporte = 0;

                    var impuestoIEPS = from row in tablePas.AsEnumerable()
                                       where Convert.ToInt32(row.Field<string>("trasladoIEPS")) <= 5
                                       group row by row.Field<double>("valIEPS") into g
                                       select new
                                       {
                                           abreviacion = "IEPS",
                                           tasa = g.Key,
                                           calculo = g.Sum(r => r.Field<double>("IEPS"))
                                       };


                    DataTable dtResultado = impuestoIEPS.ToDataTable();

                    //Se recorre la tabla agregando los diferentes IEPS agregados
                    foreach (DataRow dr in dtResultado.Rows)
                    {
                        dimporte = Convert.ToDouble(dr["calculo"].ToString());

                        if (dimporte.Equals(0))
                        {
                            continue;
                        }

                        DataRow nuevo = Impuestos.NewRow();
                        nuevo["abreviacion"] = dr["abreviacion"].ToString();
                        nuevo["calculo"] = String.Format(ForDec, Convert.ToDecimal(dr["calculo"].ToString()));
                        double dTasa = Convert.ToDouble(dr["tasa"].ToString());
                        nuevo["tasa"] = String.Format("{0:n6}", dTasa) + " %";
                        nuevo["efecto"] = "Traslado";
                        Total += dimporte;

                        Impuestos.Rows.Add(nuevo);
                    }
                }

                //if (ISH != 0)
                //{
                //    DataRow nuevo = Impuestos.NewRow();
                //    nuevo["abreviacion"] = "ISH";
                //    nuevo["calculo"] = String.Format(ForDec, ISH);
                //    nuevo["tasa"] = valISH + " %";
                //    nuevo["efecto"] = "Traslado";
                //    Impuestos.Rows.Add(nuevo);
                //}

                //if (CNG != 0) //Cargo no gravable
                //{
                //    DataRow nuevo = Impuestos.NewRow();
                //    nuevo["abreviacion"] = "Cargos No Gravables";
                //    nuevo["calculo"] = String.Format(ForDec, CNG);
                //    nuevo["tasa"] = valCNG + " %";
                //    nuevo["efecto"] = "Traslado";
                //    Impuestos.Rows.Add(nuevo);
                //}


                //resultado = clsTimbradoFuncionalidad.fnRecuperaTipoImpuesto(datosUsuario.id_usuario,
                //                                                                Convert.ToInt16(ddlTipoDoc.SelectedValue.Split('|')[0]), Convert.ToDecimal(tablePas.Compute("SUM(Importe)", "")));

                lblDetSubtotal.Text = String.Format(ForDec, Convert.ToDecimal(Subtotal));
                lblDetSubtotal.Visible = true;
                lblSubtotal.Visible = true;
                lblDetDescuento.Visible = true;
                lblDetDescuentoVal.Visible = true;

                lblTotalVal.Text = String.Format(ForDec, Convert.ToDecimal(Total));
                lblTotal.Visible = true;
                lblTotalVal.Visible = true;

                dtsTotales.Visible = true;
                dtsTotales.DataSource = Impuestos;
                dtsTotales.DataBind();

                ViewState["dtImpuestos"] = Impuestos; //Se guarda datatable impuestos para usarse en complementos

                //for (int i = 0; i < this.dtsTotales.Items.Count; i++) //Buscar control IEPS y ocultar
                //{
                //    Label lblTasa = (Label)dtsTotales.Items[i].FindControl("lblTasa");
                //    Label lblTasaVal = (Label)dtsTotales.Items[i].FindControl("lblTasaVal");
                //    Label lblTipoImpDoc = (Label)dtsTotales.Items[i].FindControl("lblTipoImpDoc");
                //    Label lblTipoImpuesto = (Label)dtsTotales.Items[i].FindControl("lblTipoImpuesto");
                //    Label lblCalculo = (Label)dtsTotales.Items[i].FindControl("lblCalculo");

                //    if (lblTipoImpuesto.Text == "IEPS")
                //    {
                //        lblTasa.Visible = false;
                //        lblTasaVal.Visible = false;
                //        lblTipoImpDoc.Visible = false;
                //        lblTipoImpuesto.Visible = false;
                //        lblCalculo.Visible = false;
                //    }

                //}
                fnCambiarTipoMoneda();

                //}
                //else
                //{
                //    fnRecargaTipoImpuestosModificados();
                //}
            }
            //Valida Totales en caso de seleccionar addenda pavilsa
            fnValidaAddendaPAVILSA();
            //Valida Totales en caso de seleccionar addenda pavilsa
            fnValidaAddendaFIDEAPECH();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    /// <summary>
    /// Valida subtotal y total para addenda pavilsa
    /// </summary>
    private void fnValidaAddendaPAVILSA()
    {
        //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
        //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
        //Se toma nuevo valor de subtotal y total en caso de seleccionar addenda
        try
        {
            if (ddlAddenda.SelectedValue == "Addendas/webAddendaPAVILSA.aspx")
            {
                //Si SubTotal y Total es diferente de vacio
                if (lblDetSubtotal.Text != string.Empty && lblTotalVal.Text != string.Empty)
                {
                    //Si SubTotal y Total es mayor a cero
                    if (Convert.ToDecimal(lblDetSubtotal.Text.Replace("$", "").Replace(",", "")) > 0 && Convert.ToDecimal(lblTotalVal.Text.Replace("$", "").Replace(",", "")) > 0)
                    {
                        //Session["lblDetSubtotal"] = Convert.ToDecimal(lblDetSubtotal.Text.Replace("$", ""));
                        //Session["lblTotalVal"] = Convert.ToDecimal(lblTotalVal.Text.Replace("$", ""));
                        //Realiza validación
                        object sender = new object();
                        EventArgs e = new EventArgs();
                        ddlAddenda_SelectedIndexChanged(sender, e);
                    }
                    else
                        ddlAddenda.SelectedValue = "0";
                }
                else
                    ddlAddenda.SelectedValue = "0";
            }
            else
            {
                Session.Remove("lblDetSubtotal");
                Session.Remove("lblTotalVal");
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
        //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
    }

    /// <summary>
    /// Valida subtotal y total para addenda fideapech
    /// </summary>
    private void fnValidaAddendaFIDEAPECH()
    {
        //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
        //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
        //Se toma nuevo valor de subtotal y total en caso de seleccionar addenda
        try
        {
            if (ddlAddenda.SelectedValue == "Addendas/webAddendaFIDEAPECH.aspx")
            {
                //Si SubTotal y Total es diferente de vacio
                if (lblTotalVal.Text != string.Empty)
                {
                    //Si SubTotal y Total es mayor a cero
                    if (Convert.ToDecimal(lblTotalVal.Text.Replace("$", "").Replace(",", "")) > 0)
                    {
                        //Realiza validación
                        object sender = new object();
                        EventArgs e = new EventArgs();
                        ddlAddenda_SelectedIndexChanged(sender, e);
                    }
                    else
                        ddlAddenda.SelectedValue = "0";
                }
                else
                    ddlAddenda.SelectedValue = "0";
            }
            else
            {
                Session.Remove("lblTotalFid");
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
        //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
    }

    private void fLimpiarCamposArt()
    {
        txtDescripcionArt.Text = string.Empty; //ddlArticulo.SelectedIndex = 0;
        txtMedidaArt.Text = string.Empty;
        ddlMonedaArt.SelectedItem.Value = "MXN";
        ddlIVAArt.SelectedValue = "16.00";
        txtCodigoArt.Text = string.Empty;
        txtPrecioArt.Text = "0";
        txtImporteArt.Text = "0";
        txtCantidadArt.Text = "0";
        txtDescuentoArt.Text = "0";
        txtISRArt.Text = "0";
        txtIvaRetArt.Text = "0";
        txtIEPS.Text = "0";
        // ddlIEPS.SelectedItem.Value = "0";
        ddlIEPS.SelectedIndex = 0;
        cbIEPS.Checked = true;
        Session["psIdArticulo"] = "";
        cbAgrCatArt.Checked = false;
    }
    protected void AgregaOpcionSeleccione(System.Object sender, System.EventArgs e)
    {
        ((DropDownList)sender).Items.Insert(0, new ListItem(SeleccioneUnValor, string.Empty));
    }
    //protected void ddlArticulo_DataBound(object sender, EventArgs e)
    //{
    //    AgregaOpcionSeleccione(sender, e);
    //}
    //protected void txtCantidadCon_TextChanged(object sender, EventArgs e)
    //{
    //    fnCalcularImpuestoCpto();
    //}

    //private void fnCalcularImpuestoCpto()
    //{
    //    try
    //    {
    //        if (txtCantidadCon.Text == string.Empty)
    //        {
    //            txtCantidadCon.Text = "0";
    //        }
    //        if (txtPrecioCon.Text == string.Empty)
    //        {
    //            txtPrecioCon.Text = "0";
    //        }
    //        if (txtDescuentoCon.Text == string.Empty)
    //        {
    //            txtDescuentoCon.Text = "0";
    //        }
    //        double Cantidad = Convert.ToDouble(txtCantidadCon.Text);
    //        double Porcentaje = Convert.ToDouble(txtPrecioCon.Text) * (Convert.ToDouble(txtDescuentoCon.Text) / 100);
    //        double Precio = (Convert.ToDouble(txtPrecioCon.Text) - Porcentaje) * Cantidad;


    //        //double Final = Precio;

    //        txtImporteCon.Text = Precio.ToString(); ; //.ToString("##0.00");
    //    }
    //    catch (Exception ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
    //    }
    //}

    protected void ddlEfectoCON_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    protected void ddlEfectoART_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    //protected void txtPrecioCon_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        double Cantidad = Convert.ToDouble(txtCantidadCon.Text);
    //        double Porcentaje = Convert.ToDouble(txtPrecioCon.Text) * (Convert.ToDouble(txtDescuentoCon.Text) / 100);
    //        double Precio = (Convert.ToDouble(txtPrecioCon.Text) - Porcentaje) * Cantidad;
    //        txtImporteCon.Text = Convert.ToString(Precio);
    //    }
    //    catch (Exception ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
    //    }
    //}
    //protected void txtPrecioArt_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        double Cantidad = Convert.ToDouble(txtCantidadArt.Text);
    //        double Porcentaje = Convert.ToDouble(txtPrecioArt.Text) * (Convert.ToDouble(txtDescuentoArt.Text) / 100);
    //        double Precio = (Convert.ToDouble(txtPrecioArt.Text) - Porcentaje) * Cantidad;
    //        txtImporteArt.Text = Convert.ToString(Precio);
    //    }
    //    catch (Exception ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
    //    }

    //}


    protected void btnAdenda_Click(object sender, EventArgs e)
    {
        mpeAdenda.Show();
    }
    protected void BotinGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtAdenda.Text != string.Empty && txtNamespace.Text != string.Empty && txtxsd.Text != string.Empty)
            {

                string Adenda = txtAdenda.Text;
                string NameSpace = txtNamespace.Text + " " + txtxsd.Text;
                XmlDocument xmladenda = new XmlDocument();
                xmladenda.LoadXml(txtAdenda.Text);
                Session["adenda"] = xmladenda.InnerXml;
                Session["AddendaNamespace"] = NameSpace;

            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError, Resources.resCorpusCFDIEs.lblContribuyente);
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }


    /// <summary>
    /// Método encargado de llenar el drop de los países
    /// </summary>
    private void fnCargarPaises()
    {
        ddlPaisExpEn.DataSource = clsComun.fnLlenarDropPaises();
        ddlPaisExpEn.DataBind();
    }

    /// <summary>
    /// Método encargado de llenar el drop de los países
    /// </summary>
    private void fnCargarPaisesLugExp()
    {
        ddlPaisLugExp.DataSource = clsComun.fnLlenarDropPaises();
        ddlPaisLugExp.DataBind();
    }

    /// <summary>
    /// Método encargado de llenar el drop de los estados
    /// </summary>
    /// <param name="psIdPais"></param>
    private void fnCargarEstados(string psIdPais)
    {
        ddlEstadoExpEn.DataSource = clsComun.fnLlenarDropEstados(psIdPais);
        ddlEstadoExpEn.DataBind();
    }

    /// <summary>
    /// Método encargado de llenar el drop de los estados
    /// </summary>
    /// <param name="psIdPais"></param>
    private void fnCargarEstadosLugExp(string psIdPaisLugExp)
    {
        ddlEdoLugExp.DataSource = clsComun.fnLlenarDropEstados(psIdPaisLugExp);
        ddlEdoLugExp.DataBind();
    }

    protected void grvDetalles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();

        switch (datosUsuario.version)
        {
            case "3.0":
                ForDec = "{0:c2}";
                break;
            case "3.2":
                ForDec = "{0:c6}";
                break;
        }

        GridViewRowEventArgs ea = e as GridViewRowEventArgs;
        if (ea.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = ea.Row.DataItem as DataRowView;
            Object ob = drv["Importe"];
            if (!Convert.IsDBNull(ob))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[8];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       ForDec, new object[] { iParsedValue });
                }
            }
            Object ob2 = drv["PrecioUnitario"];
            if (!Convert.IsDBNull(ob2))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob2.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[6];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       ForDec, new object[] { iParsedValue });
                }
            }

            //Object ob3 = drv["Descripcion"];
            //if (!Convert.IsDBNull(ob3))
            //{
            //        TableCell cell = ea.Row.Cells[5];
            //        cell.Text = cell.Text.Replace("\n", "<br />");

            //}
        }
    }

    //private void fnObtenerDatoEmisor(object sender, EventArgs e)
    //{
    //    gOpeCuenta = new clsOperacionCuenta();
    //    SqlDataReader sdrEmisor = gOpeCuenta.fnObtenerDatosFiscalesEx();

    //    try
    //    {
    //        if (sdrEmisor != null && sdrEmisor.HasRows && sdrEmisor.Read())
    //        {
    //            ddlPaisExpEn.SelectedValue = sdrEmisor["id_pais"].ToString();
    //            fnCargarEstados(ddlPaisExpEn.SelectedValue);
    //            ddlEstadoExpEn.SelectedValue = sdrEmisor["id_estado"].ToString();
    //            txtMunicipioExpEn.Text = sdrEmisor["municipio"].ToString();
    //            txtCalleExpEn.Text = sdrEmisor["calle"].ToString();
    //            txtCodigoPostalExpEn.Text = sdrEmisor["codigo_postal"].ToString();
    //        }
    //    }
    //    catch (SqlException ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
    //        Response.Redirect("~/Default.aspx");
    //    }

    //}
    protected void ddlPaisEmisor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sIdPais = (sender as DropDownList).SelectedValue;
        fnCargarEstados(sIdPais);
    }
    protected void BotnCancelar_Click(object sender, EventArgs e)
    {
        TablaComplementos = new DataTable();
        ddlComplemento.Enabled = true;
    }
    protected void cbOtraUbi_CheckedChanged(object sender, EventArgs e)
    {
        if (cbOtraUbi.Checked == true)
        {
            //fLimpiarEmisor();
            fHabilitarEmisor();
        }
        else
            fInHabilitarEmisor();
    }

    private void fInHabilitarEmisor()
    {

        ddlPaisExpEn.Enabled = false;
        ddlEstadoExpEn.Enabled = false;
        txtMunicipioExpEn.Enabled = false;
        txtCalleExpEn.Enabled = false;
        txtCodigoPostalExpEn.Enabled = false;
        //txtEstadoDat.Enabled = false;
        txtNoExteriorExpEn.Enabled = false;
        txtNoInteriorExpEn.Enabled = false;
        txtColoniaExpEn.Enabled = false;
        txtReferenciaExpEn.Enabled = false;
        txtLocalidadExpEn.Enabled = false;
    }

    private void fHabilitarEmisor()
    {
        ddlPaisExpEn.Enabled = true;
        ddlEstadoExpEn.Enabled = true;
        txtMunicipioExpEn.Enabled = true;
        txtCalleExpEn.Enabled = true;
        txtCodigoPostalExpEn.Enabled = true;
        //txtEstadoDat.Enabled = true;
        txtNoExteriorExpEn.Enabled = true;
        txtNoInteriorExpEn.Enabled = true;
        txtColoniaExpEn.Enabled = true;
        txtReferenciaExpEn.Enabled = true;
        txtLocalidadExpEn.Enabled = true;
    }

    private void fLimpiarExpEn()
    {
        ddlPaisExpEn.SelectedValue = "1";
        ddlEstadoExpEn.SelectedValue = "1";
        txtMunicipioExpEn.Text = string.Empty;
        txtCalleExpEn.Text = string.Empty;
        txtCodigoPostalExpEn.Text = string.Empty;
        //txtEstadoDat.Text = string.Empty;
        txtNoExterior.Text = string.Empty;
        txtNoInterior.Text = string.Empty;
        txtLocalidadExpEn.Text = string.Empty;
        txtColoniaExpEn.Text = string.Empty;
        txtReferenciaExpEn.Text = string.Empty;

    }

    protected void btnConsultarArt_Click(object sender, EventArgs e)
    {
        fnTraerArticulos();
        linkModalArt_ModalPopupExtender.Show();
    }

    /// <summary>
    /// realiza la búsqueda de los artículos
    /// </summary>
    private void fnTraerArticulos()
    {
        clsOperacionArticulos gOa = new clsOperacionArticulos();

        try
        {
            string sIdEstructura = ddlSucursalesFis.SelectedValue;

            gvArticulos.DataSource = gOa.fnLlenarGridArticulos(sIdEstructura, txtArtConsulta.Text, txtDesArtConsulta.Text);
            gvArticulos.DataBind();
            ViewState["dtArticulos"] = gvArticulos.DataSource;

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void gvArticulos_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            gvArticulos.PageIndex = e.NewPageIndex;
            fnTraerArticulos();
            linkModalArt_ModalPopupExtender.Show();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void gvArticulos_SelectedIndexChanged(object sender, EventArgs e)
    {
        clsOperacionArticulos gOa = new clsOperacionArticulos();
        dtArticulos = (DataTable)ViewState["dtArticulos"];

        if (!(dtArticulos.Rows[gvArticulos.SelectedRow.DataItemIndex]["id_articulo"].ToString() == ""))
        {
            Session["psIdArticulo"] = dtArticulos.Rows[gvArticulos.SelectedRow.DataItemIndex]["id_articulo"].ToString();
            txtCodigoArt.Text = dtArticulos.Rows[gvArticulos.SelectedRow.DataItemIndex]["clave"].ToString();
            clsOperacionArticulos gART = new clsOperacionArticulos();
            DataTable Tarticulos = new DataTable();
            Tarticulos = gART.fnObtieneArticulo(Convert.ToInt32(dtArticulos.Rows[gvArticulos.SelectedRow.DataItemIndex]["id_articulo"].ToString()));
            foreach (DataRow renglon in Tarticulos.Rows)
            {
                txtCodigoArt.Text = Convert.ToString(renglon["clave"]);
                txtDescripcionArt.Text = Convert.ToString(renglon["descripcion"]);
                txtMedidaArt.Text = Convert.ToString(renglon["medida"]);
                ddlMonedaArt.SelectedItem.Text = Convert.ToString(renglon["moneda"]);
                if (!(Convert.ToString(renglon["iva"]) == string.Empty))
                    ddlIVAArt.SelectedValue = Convert.ToString(renglon["iva"]);
                else
                    ddlIVAArt.SelectedValue = "Exento";
                if (Convert.ToDouble(renglon["ieps"]) != 0)
                {
                    try
                    {
                        txtIEPS.Text = string.Format("{0:n6}", Convert.ToDouble(renglon["ieps"]));
                    }
                    catch (Exception ex)
                    {
                        txtIEPS.Text = "0";
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                    }
                }
                else
                    txtIEPS.Text = "0";
                txtPrecioArt.Text = Convert.ToString(renglon["precio"]);
                txtISRArt.Text = Convert.ToString(renglon["isr"]);
                txtIvaRetArt.Text = Convert.ToString(renglon["ivaretenido"]);
            }
            //txtCantidadArt.Enabled = true;
            txtImporteArt.Text = "0";
            txtCantidadArt.Text = "0";
        }
        else
        {
            txtMedidaArt.Text = string.Empty;
            ddlMonedaArt.SelectedItem.Text = "MXN";
            ddlIVAArt.SelectedItem.Text = "16.00";
            //cbISRArt.Checked = false;
            //cbISRCon.Checked = false;
            txtISRArt.Text = "0";
            txtIvaRetArt.Text = "0";
            txtIEPS.Text = "0";
            txtPrecioArt.Text = "0";
            txtImporteArt.Text = "0";
            txtCantidadArt.Text = string.Empty;
            //txtCantidadArt.Enabled = false;
            txtCodigoArt.Text = string.Empty;
        }
        //ViewState["dtArticulos"] = dtArticulos;
        dtArticulos.Rows.Clear();
        gvArticulos.DataSource = null;
        gvArticulos.DataBind();
    }
    protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMetodoPago.SelectedItem.Text.Equals("No Aplica") || ddlMetodoPago.SelectedItem.Text.Equals("Efectivo") || ddlMetodoPago.SelectedItem.Text.Equals("No identificado") || ddlMetodoPago.SelectedItem.Text.Equals("NA"))
        {
            txtNumeroCuenta.Text = "NoAplica";
            txtNumeroCuenta.Enabled = false;
        }
        else
        {
            txtNumeroCuenta.Text = string.Empty;
            txtNumeroCuenta.Enabled = true;
        }
    }

    protected void ddlIEPS_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    private void btnGuardarArt_Click(object sender, EventArgs e)
    {
        try
        {
            clsOperacionArticulos gDAL = new clsOperacionArticulos();
            if (Convert.ToDouble(txtPrecioArt.Text) > 0) //Precio mayor a cero
            {
                //if ((Session["psIdArticulo"] == string.Empty))
                //{

                double Precio = Convert.ToDouble(txtPrecioArt.Text);
                string IVA = null;
                if (ddlIVAArt.SelectedValue == "Exento")
                    IVA = null; //Excento
                else
                    IVA = Convert.ToString(Convert.ToDouble(ddlIVAArt.SelectedValue));
                double IEPS, ISR, IVARet, ISH, CNG;
                IEPS = ISR = IVARet = ISH = CNG = 0;

                //if ((!string.IsNullOrEmpty(txtIEPS.Text) && !Convert.ToDecimal(txtIEPS.Text).Equals(0)))
                //{
                //    IEPS = Convert.ToDouble(txtIEPS.Text);
                //}
                if (!(txtISRArt.Text == string.Empty))
                {
                    ISR = Convert.ToDouble(txtISRArt.Text);
                }
                if (!(txtIvaRetArt.Text == string.Empty))
                {
                    IVARet = Convert.ToDouble(txtIvaRetArt.Text);
                }

                //if (!(txtISH.Text == string.Empty))
                //    ISH = Convert.ToDouble(txtISH.Text);

                //if (!(txtCNG.Text == string.Empty))
                //    CNG = Convert.ToDouble(txtCNG.Text);

                gDAL.fnUpdateArticulo(0, txtDescripcionArt.Text, txtMedidaArt.Text, Precio, IVA, IEPS, ISR, IVARet, ddlMoneda.SelectedValue,
                    "A", Convert.ToInt32(ddlSucursalesFis.SelectedValue), txtCodigoArt.Text);


                //fnCargarSucursalesEmisor();

                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgGuardarArticulo, Resources.resCorpusCFDIEs.lblContribuyente);
                //}
                //else
                //{
                //    int psIdArticulo = Convert.ToInt32(Session["psIdArticulo"]);
                //    double Precio = Convert.ToDouble(txtPrecioArt.Text);
                //    double IVA = Convert.ToDouble(ddlIVAArt.SelectedValue);
                //    double IEPS = 0;
                //    double ISR = 0;
                //    double IVARet = 0;
                //    if (!(ddlIEPSArt.SelectedValue == "0"))
                //    {
                //        IEPS = Convert.ToDouble(ddlIEPSArt.SelectedValue);
                //    }
                //    if (!(txtISRArt.Text == string.Empty))
                //    {
                //        ISR = Convert.ToDouble(txtISRArt.Text);
                //    }
                //    if (!(txtIvaRetArt.Text == string.Empty))
                //    {
                //        IVARet = Convert.ToDouble(txtIvaRetArt.Text);
                //    }
                //    gDAL.fnUpdateArticulo(psIdArticulo, txtDescripcionArt.Text, txtMedidaArt.Text, Precio, IVA, IEPS, ISR, IVARet, ddlMoneda.SelectedValue,
                //       "A", Convert.ToInt32(ddlSucursalesFis.SelectedValue), txtCodigoArt.Text);

                //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgActualizarArticulo, Resources.resCorpusCFDIEs.lblContribuyente);
                //}
            }
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valMayorCero, Resources.resCorpusCFDIEs.lblContribuyente);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void btnCancelarArt_Click(object sender, EventArgs e)
    {
        dtArticulos = (DataTable)ViewState["dtArticulos"];
        if (dtArticulos != null)
        {
            dtArticulos.Rows.Clear(); //Limpia grid de artículos
            gvArticulos.DataSource = dtArticulos;
            gvArticulos.DataBind();
        }
        linkModalArt_ModalPopupExtender.Hide();
    }
    protected void btnCancelarPar_Click(object sender, EventArgs e)
    {
        mpePagoParcial.Hide();
    }
    protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFormaPago.SelectedItem.Text == "Pago en Parcialidades")
        {
            cbPagParPri_CheckedChanged(sender, e);
            imgbtnPagPar.Visible = true;
            mpePagoParcial.Show();
        }
        else
        {
            imgbtnPagPar.Visible = false;
            mpePagoParcial.Hide();
            fLimCamposPagParciales();
            cbPagParPri.Checked = false;
        }
    }
    protected void btnAceptarPar_Click(object sender, EventArgs e)
    {
        if (cbPagParPri.Checked == false) //Si no es pago parcial por primera vez agrega el rango de pagos a la descripcion del concepto
        {

            if (Convert.ToInt32(txtPagParDe.Text) != 0 && Convert.ToInt32(txtPagParA.Text) != 0) //Compara si los pagos parciales son diferente de 0
            {

                if (Convert.ToInt32(txtPagParDe.Text) <= Convert.ToInt32(txtPagParA.Text)) //Compara si el pago inicial es menor al final
                {

                    if (Convert.ToDecimal(txtMonFolFisOri.Text) != 0) //Si el monto original es diferente de cero
                    {
                        ViewState["fechafolioFiscarlOrig"] = txtFecFolFisOri.Text;
                        string sDescArt = string.Empty; //Se concatena la informacion adicional para hacer referencia a la factura del pago parcial
                        sDescArt = "Pago " + txtPagParDe.Text + " " + Resources.resCorpusCFDIEs.lblPagParA + " " + txtPagParA.Text + " de la factura del " + txtFecFolFisOri.Text + " " +
                                   "con folio fiscal original " + txtFolFisOri.Text + " ";
                        if (txtSerFolFisOri.Text != string.Empty)
                        {
                            sDescArt += "con serie folio original " + txtSerFolFisOri.Text + " ";
                        }

                        sDescArt += "por un monto total original de " + String.Format("{0:C2}", Convert.ToDecimal(txtMonFolFisOri.Text));

                        txtDescripcionArt.Text = sDescArt;

                        mpePagoParcial.Hide();
                    }
                    else
                    {
                        LblMensaje.Text = Resources.resCorpusCFDIEs.msgvalMontoOrig;
                        mpePagoParcial.Show();
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgvalMontoOrig, Resources.resCorpusCFDIEs.lblContribuyente);   
                        ModalMensaje.Show(); 
                    }

                }
                else
                {
                    LblMensaje.Text = Resources.resCorpusCFDIEs.msgCompPagDePagA;
                    mpePagoParcial.Show();
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgCompPagDePagA, Resources.resCorpusCFDIEs.lblContribuyente);
                    ModalMensaje.Show(); 
                }

            }
            else
            {
                LblMensaje.Text = Resources.resCorpusCFDIEs.msgValPagosPar;
                mpePagoParcial.Show();
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgValPagosPar, Resources.resCorpusCFDIEs.lblContribuyente);
                ModalMensaje.Show(); 
            }

        }
        else
        {
            ViewState["fechafolioFiscarlOrig"] = txtFecFolFisOri.Text;
            if (Convert.ToInt32(txtNumParcialidad.Text) != 0) //Si captura número de parcialidades
            {
                txtDescripcionArt.Text = string.Empty;
                mpePagoParcial.Hide();
            }
            else
            {
                LblMensaje.Text = Resources.resCorpusCFDIEs.msgValNumPar;
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgValNumPar, Resources.resCorpusCFDIEs.lblContribuyente);
                mpePagoParcial.Show();
                ModalMensaje.Show();                
            }
        }

    }

    protected void cbPagParPri_CheckedChanged(object sender, EventArgs e)
    {
        if (cbPagParPri.Checked) //Si es parcialidad por primera vez
        {
            txtNumParcialidad.Enabled = true;
            txtPagParDe.Enabled = false;
            txtPagParA.Enabled = false;
            txtFolFisOri.Enabled = false;
            txtSerFolFisOri.Enabled = false;
            txtFecFolFisOri.Enabled = false;
            txtMonFolFisOri.Enabled = false;
            //Desactiva validadores
            rfvFolFisOri.EnableClientScript = false;
            rfvFecFolOri.EnableClientScript = false;
            revFecFolFisOri.EnableClientScript = false;
            rfvMonFolFisOri.EnableClientScript = false;
            regxMonFolFisOri.EnableClientScript = false;

            rfvFolFisOri.Visible = false;
            rfvFecFolOri.Visible = false;
            revFecFolFisOri.Visible = false;
            rfvMonFolFisOri.Visible = false;
            regxMonFolFisOri.Visible = false;

            fLimCamposPagParciales();
            //mpePagoParcial.Hide();
        }
        else
        {
            txtNumParcialidad.Enabled = false;
            txtPagParDe.Enabled = true;
            txtPagParA.Enabled = true;
            txtFolFisOri.Enabled = true;
            txtSerFolFisOri.Enabled = true;
            txtFecFolFisOri.Enabled = true;
            txtMonFolFisOri.Enabled = true;
            //Activa validadores
            rfvFolFisOri.EnableClientScript = true;
            rfvFecFolOri.EnableClientScript = true;
            revFecFolFisOri.EnableClientScript = true;
            rfvMonFolFisOri.EnableClientScript = true;
            regxMonFolFisOri.EnableClientScript = true;

            rfvFolFisOri.Visible = true;
            rfvFecFolOri.Visible = true;
            revFecFolFisOri.Visible = true;
            rfvMonFolFisOri.Visible = true;
            regxMonFolFisOri.Visible = true;
        }
        mpePagoParcial.Show();
    }

    private void fLimCamposPagParciales()
    {
        txtNumParcialidad.Text = "0";
        txtPagParDe.Text = "0";
        txtPagParA.Text = "0";
        txtFolFisOri.Text = string.Empty;
        txtSerFolFisOri.Text = string.Empty;
        txtMonFolFisOri.Text = "0";
        txtFecha_CalendarExtender.SelectedDate = DateTime.Now;
        //txtFecha_CalendarExtender_Adu.SelectedDate = DateTime.Now;
    }
    protected void imgbtnPagPar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        mpePagoParcial.Show();
    }

    protected void cbAgrExpEn_CheckedChanged(object sender, EventArgs e)
    {
        if (cbAgrExpEn.Checked)
        {
            cpeExpedidoEn.Collapsed = false;
            cpeExpedidoEn.ClientState = "false";
            cpeExpedidoEn.ExpandControlID = pnlExtender.ID;
            cpeExpedidoEn.CollapseControlID = pnlExtender.ID;

            rfvSucursales.EnableClientScript = true;
            rfvPaisExpEn.EnableClientScript = true;
            rfvEstadoExpEn.EnableClientScript = true;
            rfvMpoExpEn.EnableClientScript = true;
            rfvCalleExpEn.EnableClientScript = true;
            rfvCPExpEn.EnableClientScript = true;

            pnlExpedidoEn.Attributes.Add("style", "visibility:show;");
        }
        else
        {
            cpeExpedidoEn.Collapsed = true;
            cpeExpedidoEn.ClientState = "true";
            cpeExpedidoEn.ExpandControlID = "";
            cpeExpedidoEn.CollapseControlID = "";

            rfvSucursales.EnableClientScript = false;
            rfvPaisExpEn.EnableClientScript = false;
            rfvEstadoExpEn.EnableClientScript = false;
            rfvMpoExpEn.EnableClientScript = false;
            rfvCalleExpEn.EnableClientScript = false;
            rfvCPExpEn.EnableClientScript = false;


            pnlExpedidoEn.Attributes.Add("style", "visibility:hidden;");
        }
        fnCargarSeries();
        fnCargarFolio();
    }
    protected void btnAceptarCor_Click(object sender, EventArgs e)
    {
        clsConfiguracionPlantilla PlantillaC = new clsConfiguracionPlantilla();
        Guid Gid;
        Gid = Guid.NewGuid();
        cEd = new clsEnvioCorreoDocs();
        try
        {
            string snombreDoc = string.Empty;
            if (ViewState["nombreDoc"] != null)
                snombreDoc = ViewState["nombreDoc"].ToString();
            else
                snombreDoc = "General";

            bool bEnvio;

            string retornoInsert = string.Empty;


            string sMensaje = string.Empty;
            sMensaje = "<table>";
            sMensaje = sMensaje + "<tr><td>" + txtCorreoMsj.Text.Replace("\n", @"<br />") + "</td><td></td></tr>";
            sMensaje = sMensaje + "</table>";

            //string sMensaje = txtCorreoMsj.Text;

            string sCC = txtCorreoCC.Text;
            string sMailTo = txtCorreoEmisor.Text;
            datosUsuario = clsComun.fnUsuarioEnSesion();
            if (txtCorreoCliente.Text != string.Empty)
                sMailTo += "," + txtCorreoCliente.Text;

            string sCorCli = string.Empty; //Valida si el campo esta lleno
            if (txtCorreoCliente.Text != string.Empty)
                sCorCli = cEd.fValidaEmail(txtCorreoCliente.Text); //Valida formato de correo

            string sCorCC = string.Empty;
            if (txtCorreoCC.Text != string.Empty)
                sCorCC = cEd.fValidaEmail(txtCorreoCC.Text); //Valida formato de correo

            if (sCorCli != string.Empty || sCorCC != string.Empty) //Si esta vacio entonces los correos estan escritos correctamente
            {
                string sCadena = string.Empty;
                if (sCorCli != string.Empty) //Pinta el borde del textbox cliente
                    txtCorreoCliente.BorderColor = System.Drawing.Color.Red;
                else
                    txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;

                if (sCorCC != string.Empty) //Pinta el borde del textbox CC
                    txtCorreoCC.BorderColor = System.Drawing.Color.Red;
                else
                    txtCorreoCC.BorderColor = System.Drawing.Color.Empty;

                if (sCorCli == string.Empty && sCorCC != string.Empty)
                    sCadena = sCorCC;
                else
                    sCadena = sCorCli;

                if (sCorCC != string.Empty && sCorCC != string.Empty)
                    sCadena = sCorCli + ", " + sCorCC;

                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.txtCorreo + " " + sCadena, Resources.resCorpusCFDIEs.varContribuyente);

                if (ViewState["rfc_Emisor"] != null && ViewState["razonSocial_Emisor"] != null)
                    txtCorreoMsj.Text = Resources.resCorpusCFDIEs.varMensajeCorreo + "\n" + "\n" + ViewState["razonSocial_Emisor"] + "\n" + "RFC:" + "\n" + ViewState["rfc_Emisor"];

                mpeEnvioCorreo.Show();
                return;
            }


            if (sMensaje == string.Empty)
                sMensaje = "Comprobantes PAX Facturación";

            if (ViewState["retornoInsert"] != null)
            {
                retornoInsert = ViewState["retornoInsert"].ToString(); //idCFDI
                clsConfiguracionPlantilla plantillas = new clsConfiguracionPlantilla();
                string plantilla = plantillas.fnRecuperaPlantillaNombre(datosUsuario.plantilla);

                //Verifica si se envia el comprobante en ZIP o no.
                if (rdlArchivo.SelectedIndex == 1)
                {

                    bEnvio = cEd.fnPdfEnvioCorreo(plantilla, retornoInsert, ddlTipoDoc.SelectedItem.Text,
                                      clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLsGen"] + "\\" + snombreDoc + ".pdf",
                                      datosUsuario.id_contribuyente, datosUsuario.id_rfc, datosUsuario.color, sMailTo,
                                      "PAXFacturacion", sMensaje, Convert.ToString(ViewState["GuidPathZIPsGen"]),
                                      Convert.ToString(ViewState["GuidPathXMLsGen"]), snombreDoc, sCC);
                }
                else
                {
                    bEnvio = cEd.fnPdfEnvioCorreoSinZIP(plantilla, retornoInsert, ddlTipoDoc.SelectedItem.Text,
                                      clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLsGen"] + "\\" + snombreDoc + ".pdf",
                                      datosUsuario.id_contribuyente, datosUsuario.id_rfc, datosUsuario.color, sMailTo,
                                      "PAXFacturacion", sMensaje, Convert.ToString(ViewState["GuidPathZIPsGen"]),
                                      Convert.ToString(ViewState["GuidPathXMLsGen"]), snombreDoc, sCC);
                }

            }
            else
                bEnvio = false;

            if (bEnvio == true)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgEnvioCorreo, Resources.resCorpusCFDIEs.varContribuyente);
                mpeEnvioCorreo.Hide();
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgErrorEnvioCorreo, Resources.resCorpusCFDIEs.varContribuyente);

                if (ViewState["rfc_Emisor"] != null && ViewState["razonSocial_Emisor"] != null)
                    txtCorreoMsj.Text = Resources.resCorpusCFDIEs.varMensajeCorreo + "\n" + "\n" + ViewState["razonSocial_Emisor"] + "\n" + "RFC:" + "\n" + ViewState["rfc_Emisor"];

                mpeEnvioCorreo.Show();
            }

            txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;
            txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;


        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
        }
    }
    protected void btnCancelarCor_Click(object sender, EventArgs e)
    {
        ViewState["nombreDoc"] = string.Empty;
        ViewState["retornoInsert"] = string.Empty;
    }
    protected void ddlSucursalesFis_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSucursalesFis.SelectedValue != string.Empty)
        {
            fnCargarSeries();
            fnCargarFolio();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ddlSucursalesFis" + "|" + "Selecciona un tipo de sucursal fiscal." + "|" + ddlSucursalesFis.SelectedValue);
            fnReiniciarDatosReceptores();

            fnObtienDomicilioSucFis(Convert.ToInt32(ddlSucursalesFis.SelectedValue));

            //Carga plantilla configurada
            clsConfiguracionPlantilla Plantilla = new clsConfiguracionPlantilla();
            DataTable Plantillas = new DataTable();
            int nPlantilla;
            int idEstructura = Convert.ToInt32(ddlSucursalesFis.SelectedValue);

            Plantillas = Plantilla.fnObtieneConfiguracionPlantilla(idEstructura);
            nPlantilla = Plantilla.fnRecuperaPlantillaRecursiva(idEstructura);

            if (Plantillas.Rows.Count > 0)
            {
                datosUsuario.plantilla = Convert.ToInt32(Plantillas.Rows[0]["id_plantilla"]);
                datosUsuario.color = Convert.ToString(Plantillas.Rows[0]["color"]);
            }
            else if (nPlantilla != 0)
            {
                datosUsuario.plantilla = nPlantilla;

            }
        }
    }

    private void fCargarImpuestosIva()
    {
        DataTable dtImpIva = new DataTable();

        int i = 0;
        string descIva = string.Empty;

        DataColumn col1 = new DataColumn();
        col1.DataType = System.Type.GetType("System.String");
        col1.Caption = "DesIva";
        col1.ColumnName = "DesIva";
        col1.AllowDBNull = true;
        dtImpIva.Columns.Add(col1);

        DataColumn col2 = new DataColumn();
        col2.DataType = System.Type.GetType("System.String");
        col2.Caption = "ValIva";
        col2.ColumnName = "ValIva";
        col2.AllowDBNull = true;
        dtImpIva.Columns.Add(col2);

        for (i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    descIva = "16.00";
                    break;
                case 1:
                    descIva = "11.00";
                    break;
                case 2:
                    descIva = "0.00";
                    break;
                case 3:
                    descIva = "Exento";
                    break;
            }
            DataRow ren = dtImpIva.NewRow();
            ren["DesIva"] = descIva;
            ren["ValIva"] = descIva;
            dtImpIva.Rows.Add(ren);
        }

        ddlIVAArt.DataSource = dtImpIva;
        ddlIVAArt.DataBind();

        ddlIVAArt.SelectedValue = "16.00";
    }

    //private void fCargarImpuestosIEPS()
    //{
    //    DataTable dtImpIEPS = new DataTable();

    //    int i = 0;
    //    string descIEPS = string.Empty;

    //    DataColumn col1 = new DataColumn();
    //    col1.DataType = System.Type.GetType("System.String");
    //    col1.Caption = "DesIEPS";
    //    col1.ColumnName = "DesIEPS";
    //    col1.AllowDBNull = true;
    //    dtImpIEPS.Columns.Add(col1);

    //    DataColumn col2 = new DataColumn();
    //    col2.DataType = System.Type.GetType("System.String");
    //    col2.Caption = "ValIEPS";
    //    col2.ColumnName = "ValIEPS";
    //    col2.AllowDBNull = true;
    //    dtImpIEPS.Columns.Add(col2);

    //    for (i = 0; i < 4; i++)
    //    {
    //        switch (i)
    //        {
    //            case 0:
    //                descIEPS = "0.439200";
    //                break;
    //            case 1:
    //                descIEPS = "0.360000";
    //                break;
    //            case 2:
    //                descIEPS = "0.298800";
    //                break;
    //            case 3:
    //                descIEPS = "0";
    //                break;
    //        }
    //        DataRow ren = dtImpIEPS.NewRow();
    //        ren["DesIEPS"] = descIEPS;
    //        ren["ValIEPS"] = descIEPS;
    //        dtImpIEPS.Rows.Add(ren);
    //    }

    //    ddlIEPSArt.DataSource = dtImpIEPS;
    //    ddlIEPSArt.DataBind();

    //    ddlIEPSArt.SelectedValue = "0";
    //}

    protected void btnCancelarModal_Click(object sender, EventArgs e)
    {
        gdvReceptores.DataSource = null;
        gdvReceptores.DataBind();
        linkModal_ModalPopupExtender.Hide();
    }

    protected void gvArticulos_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        GridViewRowEventArgs ea = e as GridViewRowEventArgs;
        if (ea.Row.RowType == DataControlRowType.DataRow)
        {

            TableCell cellIeps = ea.Row.Cells[5];
            Label lblIeps = (Label)cellIeps.FindControl("lbleieps");
            lblIeps.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
               "{0:n6}", Convert.ToDouble(lblIeps.Text));

            TableCell cellIsr = ea.Row.Cells[6];
            Label lblIsr = (Label)cellIsr.FindControl("lbleisr");
            lblIsr.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
               "{0:n6}", Convert.ToDouble(lblIsr.Text));

            TableCell cellIvaret = ea.Row.Cells[7];
            Label lblIvaret = (Label)cellIvaret.FindControl("lbleivaretenido");
            lblIvaret.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
               "{0:n6}", Convert.ToDouble(lblIvaret.Text));

        }
    }

    private void fnOcultarPnlImpuestosCompl()
    {
        cpeImpuestosCompl.Collapsed = true;
        cpeImpuestosCompl.ClientState = "true";
    }

    private void fnOcultarPnlIEPS()
    {
        cpeIEPS.Collapsed = true;
        cpeIEPS.ClientState = "true";
        ddlIEPS.SelectedIndex = 0;
    }

    private void fnMostrarPnlImpuestosCompl()
    {
        cpeImpuestosCompl.Collapsed = false;
        cpeImpuestosCompl.ClientState = "false";
    }
    /// <summary>
    /// Oculta el panel Extender de información Aduanal
    /// </summary>
    private void fnOcualtarPnlInfoAduanal()
    {
        cpeInformacionAduanal.Collapsed = true;
        cpeInformacionAduanal.ClientState = "true";
    }
    /// <summary>
    /// Muestra el panel Extender de información aduanal
    /// </summary>
    private void fnMostrarPnlInfoAduanal()
    {
        cpeInformacionAduanal.Collapsed = false;
        cpeInformacionAduanal.ClientState = "false";
    }

    /// <summary>
    /// Oculta el panel Extender de complemento terceros
    /// </summary>
    private void fnOcultarPnlComplTerceros()
    {
        cpeComplementoTerceros.Collapsed = true;
        cpeComplementoTerceros.ClientState = "true";
    }
    /// <summary>
    /// Muestra el panel Extender de complemento terceros
    /// </summary>
    private void fnMostrarPnlComplTerceros()
    {
        cpeComplementoTerceros.Collapsed = false;
        cpeComplementoTerceros.ClientState = "false";
    }

    protected void ddlSucursal_SelectedIndexChanged1(object sender, EventArgs e)
    {
        fnCargarDetallesReceptorSuc();
    }

    private DataSet fnRecuperaCreditosusuario(string clave_usuario)
    {

        DataSet creditos = new DataSet();
        clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();
        try
        {
            creditos = Creditos.fnRecuperaCreditos(clave_usuario);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

        return creditos;
    }

    protected void ddlAddenda_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Si addenda seleccionada es PAVILSA
            if (ddlAddenda.SelectedValue == "Addendas/webAddendaPAVILSA.aspx")
            {
                //Si SubTotal y Total es diferente de vacio
                if (lblDetSubtotal.Text != string.Empty && lblTotalVal.Text != string.Empty)
                {
                    //Si SubTotal y Total es mayor a cero
                    if (Convert.ToDecimal(lblDetSubtotal.Text.Replace("$", "").Replace(",", "")) > 0 && Convert.ToDecimal(lblTotalVal.Text.Replace("$", "").Replace(",", "")) > 0)
                    {
                        Session["lblDetSubtotal"] = Convert.ToDecimal(lblDetSubtotal.Text.Replace("$", ""));
                        Session["lblTotalVal"] = Convert.ToDecimal(lblTotalVal.Text.Replace("$", ""));

                        btnAdenda.Attributes.Add("onclick", "javascript:url();");
                    }
                    else
                    {
                        ddlAddenda.SelectedValue = "0";
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varConceptos, Resources.resCorpusCFDIEs.varContribuyente);
                    }
                }
                else
                {
                    ddlAddenda.SelectedValue = "0";
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varConceptos, Resources.resCorpusCFDIEs.varContribuyente);
                }
            }
            else if (ddlAddenda.SelectedValue == "Addendas/webAddendaFIDEAPECH.aspx")
            {
                //Si Total es diferente de vacio
                if (lblTotalVal.Text != string.Empty)
                {
                    //Session["lblTotalFid"] = Convert.ToDecimal(lblTotalVal.Text.Replace("$", "")); Modificación 22 - 12 - 2012

                    btnAdenda.Attributes.Add("onclick", "javascript:url();");
                }
                else
                {
                    ddlAddenda.SelectedValue = "0";
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varConceptos, Resources.resCorpusCFDIEs.varContribuyente);
                }
            }
            else
            {
                btnAdenda.Attributes.Add("onclick", "javascript:url();");
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void btnAdenda_Click1(object sender, EventArgs e)
    {

    }
    public void fnCargaModuloAddendas()
    {
        try
        {
            clsConfiguracionAddenda cAddenda = new clsConfiguracionAddenda();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            DataTable dtAddenda = new DataTable();
            dtAddenda = cAddenda.fnObtieneAddendaConfiguracion(Convert.ToInt32(ddlSucursalesFis.SelectedValue), datosUsuario.id_usuario);
            if (dtAddenda.Rows.Count > 0)
            {
                DataRow drFila = dtAddenda.NewRow();
                drFila["modulo"] = 0;
                drFila["nombre"] = Resources.resCorpusCFDIEs.varSeleccione;

                dtAddenda.Rows.InsertAt(drFila, 0);

                ddlAddenda.DataSource = dtAddenda;
                ddlAddenda.DataValueField = "modulo";
                ddlAddenda.DataTextField = "nombre";
                ddlAddenda.DataBind();
                ddlAddenda.Visible = true;
                btnAdenda.Visible = true;
            }
            else
            {
                ddlAddenda.Visible = false;
                btnAdenda.Visible = false;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void btnAdenda_Click2(object sender, EventArgs e)
    {

    }
    protected void btnAgegarInfoAduanal_Click(object sender, EventArgs e)
    {
        try
        {
            //Valida que no exista complemento de terceros para poder agregar la información aduanera
            if (grvComplTerceros.Rows.Count > 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgExisteComplTerceros);
                return;
            }

            if (txtCantidadArt.Text != "" && txtCantidadArt.Text != "0")
            {
                if (txtPrecioArt.Text != "" && txtPrecioArt.Text != "0")
                {
                    if (txtAduana.Text != "" && txtDocAduana.Text != string.Empty)
                    {
                        if (txtDocAduana.Text != "" && txtDocAduana.Text != string.Empty)
                        {
                            //Insertamos datos en la tabla Información Aduanal
                            dtInfoAduanal = (DataTable)ViewState["dtInfAduanal"];

                            DataView dvInfoAduana = new DataView(dtInfoAduanal);

                            //Si la información se va a editar
                            if (ViewState["id_aduana"].ToString() != string.Empty)
                            {
                                DataRow findRow;
                                //Realizamos la busqueda del Renglon

                                findRow = dtInfoAduanal.Rows.Find(ViewState["id_aduana"].ToString());
                                if (findRow != null)
                                {
                                    findRow["Aduana"] = txtAduana.Text;
                                    findRow["DocAduana"] = txtDocAduana.Text;
                                    findRow["FechaAduana"] = txtFechaAduana.Text;
                                }

                                ViewState["id_aduana"] = string.Empty;
                            }
                            else
                            {
                                if (grvInfAduanal.Rows.Count <= 0)
                                {
                                    //Si es nuevo se inserta la información Aduanal
                                    DataRow row = dtInfoAduanal.NewRow();

                                    row["Aduana"] = txtAduana.Text;
                                    row["DocAduana"] = txtDocAduana.Text;
                                    row["FechaAduana"] = txtFechaAduana.Text;

                                    if (ViewState["id_Registros"].ToString() == string.Empty)
                                        row["id_registros"] = "0";
                                    else
                                        row["id_registros"] = ViewState["id_Registros"].ToString();

                                    dtInfoAduanal.Rows.Add(row);
                                }
                                else
                                    clsComun.fnMostrarMensaje(this, "El Articulo ya contiene informacion aduanal");// Resources.resCorpusCFDIEs.revDocAduanal);
                            }

                            if (ViewState["id_Registros"].ToString() == string.Empty)
                                dvInfoAduana.RowFilter = "id_registros = 0";
                            else
                                dvInfoAduana.RowFilter = "id_registros = " + ViewState["id_Registros"].ToString();

                            //Se llena gridview con la información Aduanal
                            grvInfAduanal.DataSource = dvInfoAduana;
                            grvInfAduanal.DataBind();

                        }
                        else
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.revDocAduanal);
                    }
                    else
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.revNombreAduana);
                }
                else
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.revPrecio);
            }
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.regxCantidad);

            txtAduana.Text = String.Empty;
            txtDocAduana.Text = string.Empty;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

    }

    protected void btnAgrImpCompl_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCantidadArt.Text.Equals("") || txtCantidadArt.Text.Equals("0"))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.regxCantidad);
                return;
            }

            if (txtPrecioArt.Text.Equals("") || txtPrecioArt.Text.Equals("0"))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.revPrecio);
                return;
            }

            if (Convert.ToDouble(txtImpTasa.Text) == 0 && !chkAgregarImporte.Checked)
            {

                if ((txtImpTasa.Text.Equals("0") || txtImpTasa.Text.Equals(string.Empty)) && !chkAgregarImporte.Checked)
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valTasaReq);
                    return;
                }

                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valTasaReq);
                return;
            }

            if (Convert.ToDouble(txtImpImporte.Text) == 0 && chkAgregarImporte.Checked)
            {
                if ((txtImpImporte.Text.Equals("0") || txtImpImporte.Text.Equals(string.Empty)) && chkAgregarImporte.Checked)
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varImpImpoReq);
                    return;
                }

                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varImpImpoReq);
                return;
            }

            if ((txtImpLoc.Text.Trim().ToUpper().Equals("IVA"))
                || (txtImpLoc.Text.Trim().ToUpper().Equals("ISR"))
                || (txtImpLoc.Text.Trim().ToUpper().Equals("IEPS"))
                || (txtImpLoc.Text.Trim().ToUpper().Equals("IVA RETENIDO")))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varImpInvalido);
                return;
            }

            if ((txtImpLoc.Text.Trim().ToUpper().Equals("DESCUENTO")))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valImpLocalDescuento);
                return;
            }

            //Se inserta en tabla el impuesto capturado
            dtCompl = (DataTable)ViewState["dtImpCompl"];

            DataView dv = new DataView(dtCompl);
            //Si se esta editando el valor

            if (!chkAgregarImporte.Checked)
                txtImpImporte.Text = Convert.ToString(((Convert.ToDouble(txtCantidadArt.Text) * Convert.ToDouble(txtPrecioArt.Text)) * Convert.ToDouble(txtImpTasa.Text) / 100));
            else
                txtImpTasa.Text = string.Format("{0:0.######}", (Convert.ToDouble(txtImpImporte.Text) * 100) / (Convert.ToDouble(txtCantidadArt.Text) * Convert.ToDouble(txtPrecioArt.Text)));

            if (ViewState["id_Impuesto"].ToString() != string.Empty)
            {
                DataRow findRow;
                //Se busca el renglon
                findRow = dtCompl.Rows.Find(ViewState["id_Impuesto"].ToString());

                if (findRow != null)
                {
                    findRow["Descripcion"] = System.Text.RegularExpressions.Regex.Replace(txtImpLoc.Text.Trim(), "\\s+", " "); //Quita espacios en blanco
                    findRow["Tasa"] = System.Text.RegularExpressions.Regex.Replace(txtImpTasa.Text.Trim(), "\\s+", " ");  //Quita espacios en blanco
                    findRow["Importe"] = System.Text.RegularExpressions.Regex.Replace(txtImpImporte.Text.Trim(), "\\s+", " ");  //Quita espacios en blanco
                    findRow["ConImporte"] = chkAgregarImporte.Checked;
                    findRow["Tipo"] = ddlImpTipo.SelectedValue;
                }

                ViewState["id_Impuesto"] = string.Empty;
            }
            else
            {
                //Si es nuevo importe se inserta el registro 
                DataRow row = dtCompl.NewRow();

                row["Descripcion"] = System.Text.RegularExpressions.Regex.Replace(txtImpLoc.Text.Trim(), "\\s+", " ");
                row["Tasa"] = System.Text.RegularExpressions.Regex.Replace(txtImpTasa.Text.Trim(), "\\s+", " ");
                row["Importe"] = System.Text.RegularExpressions.Regex.Replace(txtImpImporte.Text.Trim(), "\\s+", " ");  //Quita espacios en blanco
                row["ConImporte"] = chkAgregarImporte.Checked;
                row["Tipo"] = ddlImpTipo.SelectedValue;

                if (ViewState["id_Registros"].ToString() == string.Empty)
                    row["id_registros"] = "0";
                else
                    row["id_registros"] = ViewState["id_Registros"].ToString();

                dtCompl.Rows.Add(row);
            }

            if (ViewState["id_Registros"].ToString() == string.Empty)
                dv.RowFilter = "id_registros = 0";
            else
                dv.RowFilter = "id_registros = " + ViewState["id_Registros"].ToString();

            //Se llena grid de impuestos
            grvImpuestoCompl.DataSource = dv;
            grvImpuestoCompl.DataBind();

            chkAgregarImporte.Checked = false;
            txtImpTasa.Enabled = true;
            txtImpImporte.Enabled = false;

            txtImpLoc.Text = string.Empty;
            txtImpTasa.Text = "0";
            txtImpImporte.Text = "0";

            double dImpArt = Convert.ToDouble(txtPrecioArt.Text) * Convert.ToDouble(txtCantidadArt.Text);
            txtImporteArt.Text = dImpArt.ToString();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void grvImpuestoCompl_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();

        switch (datosUsuario.version)
        {
            case "3.0":
                ForDec = "{0:c2}";
                break;
            case "3.2":
                ForDec = "{0:c6}";
                break;
        }

        GridViewRowEventArgs ea = e as GridViewRowEventArgs;
        if (ea.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = ea.Row.DataItem as DataRowView;
            Object ob = drv["Tasa"];
            if (!Convert.IsDBNull(ob))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[4];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       ForDec, new object[] { iParsedValue });
                }
            }
            Object ob2 = drv["Importe"];
            if (!Convert.IsDBNull(ob2))
            {
                double iParsedValue = 0;
                if (double.TryParse(ob2.ToString(), out iParsedValue))
                {

                    TableCell cell = ea.Row.Cells[5];
                    cell.Text = String.Format(System.Globalization.CultureInfo.CurrentCulture,
                       ForDec, new object[] { iParsedValue });
                }
            }

            TableCell cellT = ea.Row.Cells[6];
            if (cellT.Text == "T")
                cellT.Text = "Traslado";
            else
                cellT.Text = "Retención";
        }
    }

    protected void grvImpuestoCompl_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        dtCompl = (DataTable)ViewState["dtImpCompl"];
        //Se elimina el registro seleccionado según id_impuesto
        DataRow rowDel = dtCompl.Rows.Find(e.Keys["id_Impuesto"].ToString());
        if (rowDel != null)
            rowDel.Delete();

        //dtCompl.Rows.RemoveAt(e.RowIndex);
        ViewState["dtImpCompl"] = dtCompl;

        DataView dv = new DataView(dtCompl);

        if (ViewState["id_Registros"].ToString() == string.Empty)
            dv.RowFilter = "id_registros = 0";
        else
            dv.RowFilter = "id_registros = " + ViewState["id_Registros"].ToString();

        grvImpuestoCompl.DataSource = dv;
        grvImpuestoCompl.DataBind();
    }
    protected void chkAgregarImporte_CheckedChanged(object sender, EventArgs e)
    {
        txtImpTasa.Enabled = false;
        txtImpTasa.Text = "0";
        txtImpImporte.Enabled = false;
        txtImpImporte.Text = "0";

        if (chkAgregarImporte.Checked)
            txtImpImporte.Enabled = true;
        else
            txtImpTasa.Enabled = true;
    }

    protected void grvInfAduanal_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        e.Cancel = true;
        dtInfoAduanal = (DataTable)ViewState["dtInfAduanal"];

        //Eliminados el registro seleccionado según id_aduana
        DataRow rowDel = dtInfoAduanal.Rows.Find(e.Keys["id_aduana"].ToString());
        if (rowDel != null)
            rowDel.Delete();
        ViewState["dtInfAduanal"] = dtInfoAduanal;

        DataView dvInfoAduanal = new DataView(dtInfoAduanal);
        if (ViewState["id_RegistroAdu"].ToString() == string.Empty)
            dvInfoAduanal.RowFilter = "id_registros = 0";
        else
            dvInfoAduanal.RowFilter = "id_registros = " + ViewState["id_Registros"].ToString();
        grvInfAduanal.DataSource = dvInfoAduanal;
        grvInfAduanal.DataBind();
    }
    protected void grvImpuestoCompl_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["id_Impuesto"] = grvImpuestoCompl.SelectedDataKey.Value;

            dtCompl = (DataTable)ViewState["dtImpCompl"];
            DataRow findRow = dtCompl.Rows.Find(ViewState["id_Impuesto"].ToString());
            if (findRow != null)
            {
                txtImpLoc.Text = findRow["Descripcion"].ToString();
                ddlImpTipo.SelectedValue = findRow["Tipo"].ToString();
                chkAgregarImporte.Checked = Convert.ToBoolean(findRow["ConImporte"].ToString());
                chkAgregarImporte_CheckedChanged(sender, e);
                txtImpImporte.Text = findRow["Importe"].ToString();
                txtImpTasa.Text = findRow["Tasa"].ToString();
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void grvInfAduanal_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["id_aduana"] = grvInfAduanal.SelectedDataKey.Value;
            dtInfoAduanal = (DataTable)ViewState["dtInfAduanal"];
            DataRow findRow = dtInfoAduanal.Rows.Find(ViewState["id_aduana"].ToString());
            if (findRow != null)
            {
                txtAduana.Text = findRow["Aduana"].ToString();
                txtDocAduana.Text = findRow["DocAduana"].ToString();
                //txtFechaAduana.Text = Convert.ToDateTime(findRow["FechaAduana"].ToString()).ToShortDateString();
                txtFecha_CalendarExtender_Adu.SelectedDate = Convert.ToDateTime(findRow["FechaAduana"].ToString());
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void ddlComplemento_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlComplemento.SelectedValue != "0")
        {
            imgbtnComplemento1.Attributes.Add("onclick", "javascript:urlComplemento();");
            imgbtnComplemento1.Enabled = true;
            Session["TipoDonativo"] = null;
            mpeComplemento.Show();
        }
        else
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
        }


    }
    protected void cbSerieFolioMatriz_CheckedChanged(object sender, EventArgs e)
    {
        //if (cbAgrExpEn.Checked)
        //{
        //    cpeExpedidoEn.Collapsed = false;
        //    cpeExpedidoEn.ClientState = "false";
        //    cpeExpedidoEn.ExpandControlID = pnlExtender.ID;
        //    cpeExpedidoEn.CollapseControlID = pnlExtender.ID;

        //    rfvSucursales.EnableClientScript = true;
        //    rfvPaisExpEn.EnableClientScript = true;
        //    rfvEstadoExpEn.EnableClientScript = true;
        //    rfvMpoExpEn.EnableClientScript = true;
        //    rfvCalleExpEn.EnableClientScript = true;
        //    rfvCPExpEn.EnableClientScript = true;

        //    pnlExpedidoEn.Attributes.Add("style", "visibility:show;");
        //}
        //else
        //{
        //    cpeExpedidoEn.Collapsed = true;
        //    cpeExpedidoEn.ClientState = "true";
        //    cpeExpedidoEn.ExpandControlID = "";
        //    cpeExpedidoEn.CollapseControlID = "";

        //    rfvSucursales.EnableClientScript = false;
        //    rfvPaisExpEn.EnableClientScript = false;
        //    rfvEstadoExpEn.EnableClientScript = false;
        //    rfvMpoExpEn.EnableClientScript = false;
        //    rfvCalleExpEn.EnableClientScript = false;
        //    rfvCPExpEn.EnableClientScript = false;


        //    pnlExpedidoEn.Attributes.Add("style", "visibility:hidden;");
        //}
        fnCargarSeries();
        fnCargarFolio();
    }

    /* Modificación 28 - 12 - 2012 Josel Moreno Se agregó función para verificar si el certificado está próximo a vencer */
    private void fnCertProximoVencer(string sFechaTermino)
    {
        if (!String.IsNullOrEmpty(sFechaTermino))
        {
            try
            {
                DateTime dFechaTermino = DateTime.Parse(sFechaTermino);
                DateTime dFechaActual = DateTime.Now;
                TimeSpan dDiferencia = new TimeSpan();
                dDiferencia = dFechaTermino - dFechaActual;

                int nDifDias = dDiferencia.Days;   //Diferencia en días 

                if (nDifDias >= 0 && nDifDias <= 30)
                {
                    string sMensaje = string.Empty;

                    if (nDifDias == 0)
                    {
                        if (dFechaTermino.Day == dFechaActual.Day)
                            sMensaje = Resources.resCorpusCFDIEs.msgCaducCertHoy;
                        else
                            sMensaje = Resources.resCorpusCFDIEs.msgCaducCert1Dia;
                    }
                    else
                    {
                        sMensaje = String.Format(Resources.resCorpusCFDIEs.msgCaducCertDias, nDifDias);
                    }

                    clsComun.fnMostrarMensaje(this, sMensaje);
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "Ocurrió un error al verificar si el certificado está próximo a vencer - Fecha de caducidad de certificado: " + sFechaTermino + " - PAX_FacturacionPago_3_2.Timbrado.webTimbradoGeneraciona.fnCertProximoVencer()", 0);
            }
        }
    }
    /* Fin modificación 28 - 12 - 2012 */

    #region Complemento Terceros
    protected void rblComplTerceros_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (rblComplTerceros.SelectedValue)
        {
            case "1":   //Información Aduanera
                pnlInfoAduaneraT.Visible = true;
                pnlCtaPredialT.Visible = false;
                fnLimpiarCtaPredialTerceros();
                fnActivarValidaOtraInfoTerceros(true, false);
                break;
            case "2":   //Cuenta Predial
                pnlInfoAduaneraT.Visible = false;
                pnlCtaPredialT.Visible = true;
                fnLimpiarInfoAduaneraTerceros();
                fnActivarValidaOtraInfoTerceros(false, true);
                break;
            default:    //Default - Ninguno
                pnlInfoAduaneraT.Visible = false;
                pnlCtaPredialT.Visible = false;
                fnLimpiarOtraInfoTerceros();
                fnActivarValidaOtraInfoTerceros(true, true);
                break;
        }

        //fnLimpiarOtraInfoTerceros();
    }

    private void fnActivarValidaOtraInfoTerceros(bool activaInfoAduanera, bool activaCtaPredial)
    {
        //Info aduanera terceros
        rvfNoAutDon24.Enabled = activaInfoAduanera;  //Número info aduanera (requerido)
        revFechaIni0.Enabled = activaInfoAduanera;   //Fecha info aduanera (formato)
        rfvFechaIni4.Enabled = activaInfoAduanera;   //Fecha info aduanera (requerido)
        //Cuenta predial terceros
        rvfNoAutDon29.Enabled = activaCtaPredial;
    }

    protected void btnAgregarTerceros_Click(object sender, EventArgs e)
    {
        try
        {
            //Verifica que no exista información aduanera para poder agregar el complemento
            if (grvInfAduanal.Rows.Count > 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgExisteInfoAduanera);
                return;
            }

            if (txtCantidadArt.Text != "" && txtCantidadArt.Text != "0")
            {
                if (txtPrecioArt.Text != "" && txtPrecioArt.Text != "0")
                {
                    //Insertamos datos en la tabla Información Aduanal
                    dtComplTerceros = (DataTable)ViewState["dtComplTerceros"];

                    DataView dvComplTerceros = new DataView(dtComplTerceros);

                    //Si la información se va a editar
                    if (ViewState["id_complTerceros"].ToString() != string.Empty)
                    {
                        DataRow findRow;

                        //Realizamos la búsqueda del renglón
                        findRow = dtComplTerceros.Rows.Find(ViewState["id_complTerceros"].ToString());
                        if (findRow != null)
                        {
                            findRow["version"] = tbVersionTerceros.Text;
                            findRow["rfc"] = tbRFCTerceros.Text;
                            findRow["nombre"] = txtnombreT.Text;
                            //Impuestos
                            //Impuestos retención
                            if (cbIVARet.Checked)
                            {
                                findRow["impuestoRetIVA"] = "IVA";
                                findRow["importeRetIVA"] = Convert.ToDouble(tbimporterIVARet.Text);
                            }
                            else
                            {
                                findRow["impuestoRetIVA"] = string.Empty;
                                findRow["importeRetIVA"] = 0;
                            }

                            if (cbISRRet.Checked)
                            {
                                findRow["impuestoRetISR"] = "ISR";
                                findRow["importeRetISR"] = Convert.ToDouble(tbimporteISRRet.Text);
                            }
                            else
                            {
                                findRow["impuestoRetISR"] = string.Empty;
                                findRow["importeRetISR"] = 0;
                            }
                            //Impuestos traslado
                            if (cbIVATra.Checked)
                            {
                                findRow["impuestoTrasIVA"] = "IVA";
                                findRow["tasaTrasIVA"] = Convert.ToDouble(tbtasaIVATra.Text);
                                findRow["importeTrasIVA"] = Convert.ToDouble(tbimporteIVATra.Text);
                            }
                            else
                            {
                                findRow["impuestoTrasIVA"] = string.Empty;
                                findRow["tasaTrasIVA"] = 0;
                                findRow["importeTrasIVA"] = 0;
                            }
                            if (cbIEPSTra.Checked)
                            {
                                findRow["impuestoTrasIEPS"] = "IEPS";
                                findRow["tasaTrasIEPS"] = Convert.ToDouble(tbtasaIEPSTra.Text);
                                findRow["importeTrasIEPS"] = Convert.ToDouble(tbimporteIEPSTra.Text);
                            }
                            else
                            {
                                findRow["impuestoTrasIEPS"] = string.Empty;
                                findRow["tasaTrasIEPS"] = 0;
                                findRow["importeTrasIEPS"] = 0;
                            }
                            //Información fiscal terceros
                            if (cbInfoFiscalT.Checked)
                            {
                                findRow["calle"] = txtcalleT.Text;
                                findRow["noExterior"] = txtNumExtT.Text;
                                findRow["noInterior"] = txtNumIntT.Text;
                                findRow["colonia"] = txtColoniaT.Text;
                                findRow["localidad"] = txtLocalidadT.Text;
                                findRow["referencia"] = txtReferenciaT.Text;
                                findRow["municipio"] = txtMunicipioT.Text;
                                findRow["estado"] = txtEstadoT.Text;
                                findRow["pais"] = txtPaisT.Text;
                                findRow["codigoPostal"] = txtCodigoT.Text;
                            }
                            else
                            {
                                findRow["calle"] = string.Empty;
                                findRow["noExterior"] = string.Empty;
                                findRow["noInterior"] = string.Empty;
                                findRow["colonia"] = string.Empty;
                                findRow["localidad"] = string.Empty;
                                findRow["referencia"] = string.Empty;
                                findRow["municipio"] = string.Empty;
                                findRow["estado"] = string.Empty;
                                findRow["pais"] = string.Empty;
                                findRow["codigoPostal"] = string.Empty;
                            }
                            //Información aduanera
                            if (rblComplTerceros.SelectedValue == "1")
                            {
                                findRow["numeroInfoAd"] = txtnumeroT.Text;
                                findRow["fechaInfoAd"] = Convert.ToDateTime(txtFechaIniT.Text);
                                findRow["aduanaInfoAd"] = txtaduanaT.Text;
                            }
                            else
                            {
                                findRow["numeroInfoAd"] = string.Empty;
                                //findRow["fechaInfoAd"] = string.Empty;
                                findRow["aduanaInfoAd"] = string.Empty;
                            }
                            //Cuenta Predial
                            if (rblComplTerceros.SelectedValue == "2")
                            {
                                findRow["numeroCtaPred"] = txtPredialT.Text;
                            }
                            else
                            {
                                findRow["numeroCtaPred"] = string.Empty;
                            }
                        }

                        ViewState["id_complTerceros"] = string.Empty;
                    }
                    else
                    {
                        if (grvComplTerceros.Rows.Count <= 0)
                        {
                            //Si es nuevo se inserta la información Aduanal
                            DataRow row = dtComplTerceros.NewRow();

                            row["version"] = tbVersionTerceros.Text;
                            row["rfc"] = tbRFCTerceros.Text;
                            row["nombre"] = txtnombreT.Text;
                            //Impuestos
                            //Impuestos retención
                            if (cbIVARet.Checked)
                            {
                                row["impuestoRetIVA"] = "IVA";
                                row["importeRetIVA"] = Convert.ToDouble(tbimporterIVARet.Text);
                            }
                            else
                            {
                                row["impuestoRetIVA"] = string.Empty;
                                row["importeRetIVA"] = 0;
                            }

                            if (cbISRRet.Checked)
                            {
                                row["impuestoRetISR"] = "ISR";
                                row["importeRetISR"] = Convert.ToDouble(tbimporteISRRet.Text);
                            }
                            else
                            {
                                row["impuestoRetISR"] = string.Empty;
                                row["importeRetISR"] = 0;
                            }
                            //Impuestos traslado
                            if (cbIVATra.Checked)
                            {
                                row["impuestoTrasIVA"] = "IVA";
                                row["tasaTrasIVA"] = Convert.ToDouble(tbtasaIVATra.Text);
                                row["importeTrasIVA"] = Convert.ToDouble(tbimporteIVATra.Text);
                            }
                            else
                            {
                                row["impuestoTrasIVA"] = string.Empty;
                                row["tasaTrasIVA"] = 0;
                                row["importeTrasIVA"] = 0;
                            }
                            if (cbIEPSTra.Checked)
                            {
                                row["impuestoTrasIEPS"] = "IEPS";
                                row["tasaTrasIEPS"] = Convert.ToDouble(tbtasaIEPSTra.Text);
                                row["importeTrasIEPS"] = Convert.ToDouble(tbimporteIEPSTra.Text);
                            }
                            else
                            {
                                row["impuestoTrasIEPS"] = string.Empty;
                                row["tasaTrasIEPS"] = 0;
                                row["importeTrasIEPS"] = 0;
                            }
                            //Información fiscal terceros
                            if (cbInfoFiscalT.Checked)
                            {
                                row["calle"] = txtcalleT.Text;
                                row["noExterior"] = txtNumExtT.Text;
                                row["noInterior"] = txtNumIntT.Text;
                                row["colonia"] = txtColoniaT.Text;
                                row["localidad"] = txtLocalidadT.Text;
                                row["referencia"] = txtReferenciaT.Text;
                                row["municipio"] = txtMunicipioT.Text;
                                row["estado"] = txtEstadoT.Text;
                                row["pais"] = txtPaisT.Text;
                                row["codigoPostal"] = txtCodigoT.Text;
                            }
                            else
                            {
                                row["calle"] = string.Empty;
                                row["noExterior"] = string.Empty;
                                row["noInterior"] = string.Empty;
                                row["colonia"] = string.Empty;
                                row["localidad"] = string.Empty;
                                row["referencia"] = string.Empty;
                                row["municipio"] = string.Empty;
                                row["estado"] = string.Empty;
                                row["pais"] = string.Empty;
                                row["codigoPostal"] = string.Empty;
                            }
                            //Información aduanera
                            if (rblComplTerceros.SelectedValue == "1")
                            {
                                row["numeroInfoAd"] = txtnumeroT.Text;
                                row["fechaInfoAd"] = Convert.ToDateTime(txtFechaIniT.Text);
                                row["aduanaInfoAd"] = txtaduanaT.Text;
                            }
                            else
                            {
                                row["numeroInfoAd"] = string.Empty;
                                //row["fechaInfoAd"] = string.Empty;
                                row["aduanaInfoAd"] = string.Empty;
                            }
                            //Cuenta Predial
                            if (rblComplTerceros.SelectedValue == "2")
                            {
                                row["numeroCtaPred"] = txtPredialT.Text;
                            }
                            else
                            {
                                row["numeroCtaPred"] = string.Empty;
                            }

                            if (ViewState["id_Registros"].ToString() == string.Empty)
                                row["id_registros"] = "0";
                            else
                                row["id_registros"] = ViewState["id_Registros"].ToString();

                            dtComplTerceros.Rows.Add(row);
                        }
                        else
                            clsComun.fnMostrarMensaje(this, "El Articulo ya contiene el complemento de terceros");// Resources.resCorpusCFDIEs.revDocAduanal);
                    }

                    if (ViewState["id_Registros"].ToString() == string.Empty)
                        dvComplTerceros.RowFilter = "id_registros = 0";
                    else
                        dvComplTerceros.RowFilter = "id_registros = " + ViewState["id_Registros"].ToString();

                    fnLimpiarComplTeceros();
                    //Se llena grid con información de terceros
                    txtFechaIniT_CalendarExtender.SelectedDate = DateTime.Now;
                    grvComplTerceros.DataSource = dvComplTerceros;
                    grvComplTerceros.DataBind();
                }
                else
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.revPrecio);
            }
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.regxCantidad);

            //txtAduana.Text = String.Empty;
            //txtDocAduana.Text = string.Empty;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    private void fnLimpiarComplTeceros()
    {
        fnLimpiarInfoGeneralTerceros();
        fnLimpiarInfoFiscalTerceros();
        rblComplTerceros.SelectedValue = "0";
        rblComplTerceros_SelectedIndexChanged(this, null);
        fnLimpiarOtraInfoTerceros();
    }

    private void fnLimpiarInfoGeneralTerceros()
    {
        tbVersionTerceros.Text = "1.1";
        tbRFCTerceros.Text = string.Empty;
        txtnombreT.Text = string.Empty;
        fnLimpiarImpuestosTerceros();
    }

    protected void cbIVATra_CheckedChanged(object sender, EventArgs e)
    {
        fnLimpiarImpIVATraTerceros();

    }
    protected void cbIEPSTra_CheckedChanged(object sender, EventArgs e)
    {
        fnLimpiarImpIEPSTerceros();
    }
    protected void cbIVARet_CheckedChanged(object sender, EventArgs e)
    {
        fnLimpiarImpIVARetTerceros();
    }
    protected void cbISRRet_CheckedChanged(object sender, EventArgs e)
    {
        fnLimpiarImpISRTerceros();
    }

    private void fnLimpiarImpuestosTerceros()
    {
        cbIVARet.Checked = false;
        cbISRRet.Checked = false;
        cbIVATra.Checked = true;
        cbIEPSTra.Checked = false;
        cbIVARet_CheckedChanged(this, null);
        cbISRRet_CheckedChanged(this, null);
        cbIVATra_CheckedChanged(this, null);
        cbIEPSTra_CheckedChanged(this, null);
    }

    private void fnLimpiarImpIVATraTerceros()
    {
        tbimporteIVATra.Text = "0";
        tbtasaIVATra.Text = "0";
        tbimporteIVATra.Enabled = cbIVATra.Checked;
        tbtasaIVATra.Enabled = cbIVATra.Checked;
        rvfImpIvaTraReq.Enabled = cbIVATra.Checked;
        rvftasaIvaReq.Enabled = cbIVATra.Checked;
        rvfImpIvaTraNum.Enabled = cbIVATra.Checked;
        rvftasaIvaNum.Enabled = cbIVATra.Checked;
    }

    private void fnLimpiarImpIEPSTerceros()
    {
        tbimporteIEPSTra.Text = "0";
        tbtasaIEPSTra.Text = "0";
        tbimporteIEPSTra.Enabled = cbIEPSTra.Checked;
        tbtasaIEPSTra.Enabled = cbIEPSTra.Checked;
        rvfImpIEPSTraReq.Enabled = cbIEPSTra.Checked;
        rvfImpIEPSTraNum.Enabled = cbIEPSTra.Checked;
        rvftasaIEPSReq.Enabled = cbIEPSTra.Checked;
        rvftasaIEPSNum.Enabled = cbIEPSTra.Checked;
    }

    private void fnLimpiarImpIVARetTerceros()
    {
        tbimporterIVARet.Text = "0";
        tbimporterIVARet.Enabled = cbIVARet.Checked;
        rvfImpIvaRetReq.Enabled = cbIVARet.Checked;
        rvfImpIvaRetNum.Enabled = cbIVARet.Checked;
    }

    private void fnLimpiarImpISRTerceros()
    {
        tbimporteISRRet.Text = "0";
        tbimporteISRRet.Enabled = cbISRRet.Checked;
        rvfImpRetISR.Enabled = cbISRRet.Checked;
        rvfImpRetISRNum.Enabled = cbISRRet.Checked;
    }

    private void fnLimpiarOtraInfoTerceros()
    {
        //Información aduanera
        fnLimpiarInfoAduaneraTerceros();

        //Cuenta predial
        fnLimpiarCtaPredialTerceros();
    }

    private void fnLimpiarInfoAduaneraTerceros()
    {
        txtnumeroT.Text = string.Empty;
        txtFechaIniT.Text = string.Empty;
        txtaduanaT.Text = string.Empty;
    }

    private void fnLimpiarCtaPredialTerceros()
    {
        txtPredialT.Text = string.Empty;
    }

    private void fnLimpiarInfoFiscalTerceros()
    {
        txtcalleT.Text = string.Empty;
        txtNumExtT.Text = string.Empty;
        txtNumIntT.Text = string.Empty;
        txtColoniaT.Text = string.Empty;
        txtReferenciaT.Text = string.Empty;
        txtLocalidadT.Text = string.Empty;
        txtMunicipioT.Text = string.Empty;
        txtEstadoT.Text = string.Empty;
        txtPaisT.Text = string.Empty;
        txtCodigoT.Text = string.Empty;
        cbInfoFiscalT.Checked = false;
        cbInfoFiscalT_CheckedChanged(this, null);
    }

    protected void cbInfoFiscalT_CheckedChanged(object sender, EventArgs e)
    {
        bool activo = cbInfoFiscalT.Checked;
        fnActivarInfoFiscalTerceros(activo);
        fnActivarValidaInfoFiscalTerceros(activo);
    }

    private void fnActivarValidaInfoFiscalTerceros(bool activo)
    {
        rvfNoAutDon14.Enabled = activo;
        rvfNoAutDon19.Enabled = activo;
        rvfNoAutDon22.Enabled = activo;
        rvfNoAutDon23.Enabled = activo;
        rfvCodigoPostal.Enabled = activo;
    }

    private void fnActivarInfoFiscalTerceros(bool activar)
    {
        txtcalleT.Enabled = activar;
        txtNumExtT.Enabled = activar;
        txtNumIntT.Enabled = activar;
        txtColoniaT.Enabled = activar;
        txtReferenciaT.Enabled = activar;
        txtLocalidadT.Enabled = activar;
        txtMunicipioT.Enabled = activar;
        txtEstadoT.Enabled = activar;
        txtPaisT.Enabled = activar;
        txtCodigoT.Enabled = activar;
    }

    protected void grvComplTerceros_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        e.Cancel = true;
        dtComplTerceros = (DataTable)ViewState["dtComplTerceros"];

        //Eliminados el registro seleccionado según id_aduana
        DataRow rowDel = dtComplTerceros.Rows.Find(e.Keys["id_complTerceros"].ToString());
        if (rowDel != null)
            rowDel.Delete();
        ViewState["dtComplTerceros"] = dtComplTerceros;

        DataView dvComplTerceros = new DataView(dtComplTerceros);
        if (ViewState["id_Registros"].ToString() == string.Empty)
            dvComplTerceros.RowFilter = "id_registros = 0";
        else
            dvComplTerceros.RowFilter = "id_registros = " + ViewState["id_Registros"].ToString();

        if (ViewState["id_complTerceros"] != null)
            ViewState["id_complTerceros"] = string.Empty;
        txtFechaIniT_CalendarExtender.SelectedDate = DateTime.Now;
        grvComplTerceros.DataSource = dvComplTerceros;
        grvComplTerceros.DataBind();
    }

    protected void grvComplTerceros_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["id_complTerceros"] = grvComplTerceros.SelectedDataKey.Value;
            dtComplTerceros = (DataTable)ViewState["dtComplTerceros"];
            DataRow findRow = dtComplTerceros.Rows.Find(ViewState["id_complTerceros"].ToString());
            if (findRow != null)
            {
                tbVersionTerceros.Text = findRow["version"].ToString();
                tbRFCTerceros.Text = findRow["rfc"].ToString();
                txtnombreT.Text = findRow["nombre"].ToString();
                //Impuestos
                //Impuestos retención
                if (findRow["impuestoRetIVA"].ToString() == "IVA")
                {
                    cbIVARet.Checked = true;
                    tbimporterIVARet.Enabled = true;
                    tbimporterIVARet.Text = findRow["importeRetIVA"].ToString();
                }
                else
                {
                    cbIVARet.Checked = false;
                    tbimporterIVARet.Enabled = false;
                    tbimporterIVARet.Text = "0";
                }

                if (findRow["impuestoRetISR"].ToString() == "ISR")
                {
                    cbISRRet.Checked = true;
                    tbimporteISRRet.Enabled = true;
                    tbimporteISRRet.Text = findRow["importeRetISR"].ToString();
                }
                else
                {
                    cbISRRet.Checked = false;
                    tbimporteISRRet.Enabled = false;
                    tbimporteISRRet.Text = "0";
                }

                //Impuestos trasladados
                if (findRow["impuestoTrasIVA"].ToString() == "IVA")
                {
                    cbIVATra.Checked = true;
                    tbtasaIVATra.Enabled = true;
                    tbimporteIVATra.Enabled = true;
                    tbtasaIVATra.Text = findRow["tasaTrasIVA"].ToString();
                    tbimporteIVATra.Text = findRow["importeTrasIVA"].ToString();
                }
                else
                {
                    cbIVATra.Checked = false;
                    tbtasaIVATra.Enabled = false;
                    tbimporteIVATra.Enabled = false;
                    tbtasaIVATra.Text = "0";
                    tbimporteIVATra.Text = "0";
                }

                if (findRow["impuestoTrasIEPS"].ToString() == "IEPS")
                {
                    cbIEPSTra.Checked = true;
                    tbtasaIEPSTra.Enabled = true;
                    tbimporteIEPSTra.Enabled = true;
                    tbtasaIEPSTra.Text = findRow["tasaTrasIEPS"].ToString();
                    tbimporteIEPSTra.Text = findRow["importeTrasIEPS"].ToString();
                }
                else
                {
                    cbIEPSTra.Checked = false;
                    tbtasaIEPSTra.Enabled = false;
                    tbimporteIEPSTra.Enabled = false;
                    tbtasaIEPSTra.Text = "0";
                    tbimporteIEPSTra.Text = "0";
                }
                //Información fiscal terceros
                if (!String.IsNullOrEmpty(findRow["calle"].ToString()))
                {
                    cbInfoFiscalT.Checked = true;
                    txtcalleT.Text = findRow["calle"].ToString();
                    txtNumExtT.Text = findRow["noExterior"].ToString();
                    txtNumIntT.Text = findRow["noInterior"].ToString();
                    txtColoniaT.Text = findRow["colonia"].ToString();
                    txtLocalidadT.Text = findRow["localidad"].ToString();
                    txtReferenciaT.Text = findRow["referencia"].ToString();
                    txtMunicipioT.Text = findRow["municipio"].ToString();
                    txtEstadoT.Text = findRow["estado"].ToString();
                    txtPaisT.Text = findRow["pais"].ToString();
                    txtCodigoT.Text = findRow["codigoPostal"].ToString();
                }
                else
                {
                    fnLimpiarInfoFiscalTerceros();
                    cbInfoFiscalT.Checked = false;
                }
                cbInfoFiscalT_CheckedChanged(sender, e);

                //Información aduanera
                bool otraInfo = false;

                if (!String.IsNullOrEmpty(findRow["numeroInfoAd"].ToString()))
                {
                    txtnumeroT.Text = findRow["numeroInfoAd"].ToString();
                    txtFechaIniT_CalendarExtender.SelectedDate = Convert.ToDateTime(findRow["fechaInfoAd"].ToString());
                    txtaduanaT.Text = findRow["aduanaInfoAd"].ToString();
                    rblComplTerceros.SelectedValue = "1";
                    otraInfo = true;
                }
                else
                {
                    fnLimpiarInfoAduaneraTerceros();
                }
                //Cuenta Predial
                if (!String.IsNullOrEmpty(findRow["numeroCtaPred"].ToString()))
                {
                    txtPredialT.Text = findRow["numeroCtaPred"].ToString();
                    rblComplTerceros.SelectedValue = "2";
                    otraInfo = true;
                }
                else
                {
                    fnLimpiarCtaPredialTerceros();
                }

                //Seleccciona la opción ninguno si no hay información adicional (Información aduanera o cuenta predial)
                if (!otraInfo)
                {
                    rblComplTerceros.SelectedValue = "0";
                }
                rblComplTerceros_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    private XmlDocument fnAgregarNameSpaceComplTerceros(XmlDocument pXmlDoc)
    {
        string schema = pXmlDoc.DocumentElement.GetAttribute("xsi:schemaLocation");
        pXmlDoc.DocumentElement.SetAttribute("xsi:schemaLocation", schema + " http://www.sat.gob.mx/terceros http://www.sat.gob.mx/sitio_internet/cfd/terceros/terceros11.xsd");

        pXmlDoc.DocumentElement.SetAttribute("xmlns:terceros", "http://www.sat.gob.mx/terceros");

        return pXmlDoc;
    }

    protected void grvComplTerceros_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        //GridView fields
        #region índices
        //Columna           Indice
        //editar 		    - 0
        //eliminar	        - 1
        //id_registros	    - 2
        //rfc		        - 3
        //nombre		    - 4
        //impuestoTrasIVA	- 5
        //importeTrasIVA	- 6
        //tasaTrasIVA	    - 7
        //impuestoTrasIEPS  - 8
        //importeTrasIEPS	- 9
        //tasaTrasIEPS	    - 10
        //impuestoRetIVA	- 11
        //importeRetIVA	    - 12
        //impuestoRetISR 	- 13 
        //importeRetISR	    - 14
        //calle		        - 15
        //noExterior	    - 16
        //noInterior	    - 17
        //colonia		    - 18
        //referencia	    - 19
        //localidad	        - 20
        //municipio     	- 21
        //estado		    - 22
        //pais		        - 23
        //codigoPostal	    - 24
        //aduanaInfoAd	    - 25
        //numeroInfoAd	    - 26
        //fechaInfoAd	    - 27
        //numeroCtaPred	    - 28
        //id_complTerceros  - 29
        #endregion

        if (e.Row != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dvComplTerceros = (DataRowView)e.Row.DataItem;

                #region Ocultar/mostrar Columnas
                //Mostrar/Ocultar columnas del grid view
                if (String.IsNullOrEmpty(dvComplTerceros["nombre"].ToString()))
                {
                    //grvComplTerceros.Columns[4].Visible = (String.IsNullOrEmpty(dvComplTerceros["nombre"].ToString())) ? false : true;
                    grvComplTerceros.Columns[4].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[4].Visible = true;
                    e.Row.Cells[4].Text = dvComplTerceros["nombre"].ToString();
                }

                //Impuestos tercero
                if (String.IsNullOrEmpty(dvComplTerceros["impuestoTrasIVA"].ToString()))
                {
                    //grvComplTerceros.Columns[6].Visible = (String.IsNullOrEmpty(dvComplTerceros["impuestoTrasIVA"].ToString())) ? false : true;
                    //grvComplTerceros.Columns[7].Visible = (String.IsNullOrEmpty(dvComplTerceros["impuestoTrasIVA"].ToString())) ? false : true;
                    grvComplTerceros.Columns[6].Visible = false;
                    grvComplTerceros.Columns[7].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[6].Visible = true;
                    grvComplTerceros.Columns[7].Visible = true;
                    e.Row.Cells[6].Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:c2}", Convert.ToDouble(dvComplTerceros["importeTrasIVA"].ToString()));
                    e.Row.Cells[7].Text = dvComplTerceros["tasaTrasIVA"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["impuestoTrasIEPS"].ToString()))
                {
                    //grvComplTerceros.Columns[9].Visible = (String.IsNullOrEmpty(dvComplTerceros["impuestoTrasIEPS"].ToString())) ? false : true;
                    //grvComplTerceros.Columns[10].Visible = (String.IsNullOrEmpty(dvComplTerceros["impuestoTrasIEPS"].ToString())) ? false : true;
                    grvComplTerceros.Columns[9].Visible = false;
                    grvComplTerceros.Columns[10].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[9].Visible = true;
                    grvComplTerceros.Columns[10].Visible = true;
                    e.Row.Cells[9].Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:c2}", Convert.ToDouble(dvComplTerceros["importeTrasIEPS"].ToString()));
                    e.Row.Cells[10].Text = dvComplTerceros["tasaTrasIEPS"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["impuestoRetIVA"].ToString()))
                {
                    //grvComplTerceros.Columns[12].Visible = (String.IsNullOrEmpty(dvComplTerceros["impuestoRetIVA"].ToString())) ? false : true;
                    grvComplTerceros.Columns[12].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[12].Visible = true;
                    e.Row.Cells[12].Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:c2}", Convert.ToDouble(dvComplTerceros["importeRetIVA"].ToString()));
                }

                if (String.IsNullOrEmpty(dvComplTerceros["impuestoRetISR"].ToString()))
                {
                    //grvComplTerceros.Columns[14].Visible = (String.IsNullOrEmpty(dvComplTerceros["impuestoRetISR"].ToString())) ? false : true;
                    grvComplTerceros.Columns[14].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[14].Visible = true;
                    e.Row.Cells[14].Text = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:c2}", Convert.ToDouble(dvComplTerceros["importeRetISR"].ToString()));
                }


                //Información fiscal terceros
                if (String.IsNullOrEmpty(dvComplTerceros["calle"].ToString()))
                {
                    //grvComplTerceros.Columns[15].Visible = (String.IsNullOrEmpty(dvComplTerceros["calle"].ToString())) ? false : true;
                    grvComplTerceros.Columns[15].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[15].Visible = true;
                    e.Row.Cells[15].Text = dvComplTerceros["calle"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["noExterior"].ToString()))
                {
                    //grvComplTerceros.Columns[16].Visible = (String.IsNullOrEmpty(dvComplTerceros["noExterior"].ToString())) ? false : true;
                    grvComplTerceros.Columns[16].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[16].Visible = true;
                    e.Row.Cells[16].Text = dvComplTerceros["noExterior"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["noInterior"].ToString()))
                {
                    //grvComplTerceros.Columns[17].Visible = (String.IsNullOrEmpty(dvComplTerceros["noInterior"].ToString())) ? false : true;
                    grvComplTerceros.Columns[17].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[17].Visible = true;
                    e.Row.Cells[17].Text = dvComplTerceros["noInterior"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["colonia"].ToString()))
                {
                    //grvComplTerceros.Columns[18].Visible = (String.IsNullOrEmpty(dvComplTerceros["colonia"].ToString())) ? false : true;
                    grvComplTerceros.Columns[18].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[18].Visible = true;
                    e.Row.Cells[18].Text = dvComplTerceros["colonia"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["referencia"].ToString()))
                {
                    //grvComplTerceros.Columns[19].Visible = (String.IsNullOrEmpty(dvComplTerceros["referencia"].ToString())) ? false : true;
                    grvComplTerceros.Columns[19].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[19].Visible = true;
                    e.Row.Cells[19].Text = dvComplTerceros["referencia"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["localidad"].ToString()))
                {
                    //grvComplTerceros.Columns[20].Visible = (String.IsNullOrEmpty(dvComplTerceros["localidad"].ToString())) ? false : true;
                    grvComplTerceros.Columns[20].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[20].Visible = true;
                    e.Row.Cells[20].Text = dvComplTerceros["localidad"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["municipio"].ToString()))
                {
                    //grvComplTerceros.Columns[21].Visible = (String.IsNullOrEmpty(dvComplTerceros["municipio"].ToString())) ? false : true;
                    grvComplTerceros.Columns[21].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[21].Visible = true;
                    e.Row.Cells[21].Text = dvComplTerceros["municipio"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["estado"].ToString()))
                {
                    //grvComplTerceros.Columns[22].Visible = (String.IsNullOrEmpty(dvComplTerceros["estado"].ToString())) ? false : true;
                    grvComplTerceros.Columns[22].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[22].Visible = true;
                    e.Row.Cells[22].Text = dvComplTerceros["estado"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["pais"].ToString()))
                {
                    //grvComplTerceros.Columns[23].Visible = (String.IsNullOrEmpty(dvComplTerceros["pais"].ToString())) ? false : true;
                    grvComplTerceros.Columns[23].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[23].Visible = true;
                    e.Row.Cells[23].Text = dvComplTerceros["pais"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["codigoPostal"].ToString()))
                {
                    //grvComplTerceros.Columns[24].Visible = (String.IsNullOrEmpty(dvComplTerceros["codigoPostal"].ToString())) ? false : true;
                    grvComplTerceros.Columns[24].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[24].Visible = true;
                    e.Row.Cells[24].Text = dvComplTerceros["codigoPostal"].ToString();
                }

                //Información aduanera tercero
                if (String.IsNullOrEmpty(dvComplTerceros["aduanaInfoAd"].ToString()))
                {
                    //grvComplTerceros.Columns[25].Visible = (String.IsNullOrEmpty(dvComplTerceros["aduanaInfoAd"].ToString())) ? false : true;
                    grvComplTerceros.Columns[25].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[25].Visible = false;
                    e.Row.Cells[25].Text = dvComplTerceros["aduanaInfoAd"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["numeroInfoAd"].ToString()))
                {
                    //grvComplTerceros.Columns[26].Visible = (String.IsNullOrEmpty(dvComplTerceros["numeroInfoAd"].ToString())) ? false : true;
                    grvComplTerceros.Columns[26].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[26].Visible = true;
                    e.Row.Cells[26].Text = dvComplTerceros["numeroInfoAd"].ToString();
                }

                if (String.IsNullOrEmpty(dvComplTerceros["fechaInfoAd"].ToString()))
                {
                    //grvComplTerceros.Columns[27].Visible = (String.IsNullOrEmpty(dvComplTerceros["fechaInfoAd"].ToString())) ? false : true;
                    grvComplTerceros.Columns[27].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[27].Visible = true;
                    e.Row.Cells[27].Text = Convert.ToDateTime(dvComplTerceros["fechaInfoAd"].ToString()).ToShortDateString();
                }

                //Cuenta predial
                if (String.IsNullOrEmpty(dvComplTerceros["numeroCtaPred"].ToString()))
                {
                    //grvComplTerceros.Columns[28].Visible = (String.IsNullOrEmpty(dvComplTerceros["numeroCtaPred"].ToString())) ? false : true;
                    grvComplTerceros.Columns[28].Visible = false;
                }
                else
                {
                    grvComplTerceros.Columns[28].Visible = false;
                    e.Row.Cells[28].Text = dvComplTerceros["numeroCtaPred"].ToString();
                }

                #endregion
            }
        }
    }

    #endregion

    protected void txtDescuentoGlobal_TextChanged(object sender, EventArgs e)
    {
        txtDescuentoArt.Enabled = false;
        if (!txtDescuentoGlobal.Text.Equals(string.Empty))
        {
            if (Convert.ToDouble(txtDescuentoGlobal.Text).Equals(0))
            {
                txtDescuentoArt.Enabled = true;
                chkDescuentoGlobal.Checked = false;
            }
            else
                txtDescuentoArt.Text = "0";


        }
        else
        {
            txtDescuentoGlobal.Text = "0";
            chkDescuentoGlobal.Checked = false;
            if (Convert.ToString(ViewState["id_Registros"]).Equals(string.Empty))
                txtDescuentoArt.Enabled = true;
        }
    }
    protected void BtnMensaje_Click(object sender, EventArgs e)
    {
        ModalMensaje.Hide();
    }
}



