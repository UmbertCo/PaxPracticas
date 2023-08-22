using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using Utilerias.SQL;
using PAXRecepcionProveedores.App_Code.GeneradorE_MAIL;
using System.Text;
using System.Threading;
using System.Globalization;

namespace PAXRecepcionProveedores.Operacion.Comprobantes
{
    public partial class webValidacionComprobantes : System.Web.UI.Page
    {
        /// <summary>
        /// Contiene la info de los archivos subidos
        /// </summary>
        private DataTable dtComprobantes;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    clsInicioSesionUsuario sesUsuario = clsComun.fnUsuarioEnSesion();
                    clsOperacionUsuarios oOpUsuarios = new clsOperacionUsuarios();
                    //SqlDataReader sdrInfo = gOp.fnObtenerDatosUsuario();
                    int nId_Usuario = sesUsuario.id_usuario;
                    if (nId_Usuario > 0)
                    {
                        DataTable tblModulosPerfil = oOpUsuarios.fnSeleccionaModulosHijo(sesUsuario.Id_perfil, true);
                        string[] urlActual = Request.Url.AbsolutePath.Split('/');
                        int encontrado = tblModulosPerfil.AsEnumerable().Where(t => t.Field<string>("modulo").Contains(urlActual[urlActual.Length - 1])).Count();
                        if (encontrado < 1)
                            Response.Redirect("~/Default.aspx",false);
                    }
                    fnLlenarSucursales();
                    fnObtenerComprobantes();
                    string sMaxZip = clsComun.ObtenerParamentro("MaxZipFiles");

                    string sMensaje = string.Format(Resources.resCorpusCFDIEs.varZipComprobantes, sMaxZip);
                    lblMaxZip.Text = sMensaje;
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/webGlobalError.aspx");
            }
        }

        private void fnObtenerComprobantes()
        {
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            DataTable dtComprobantes = new clsOperacionComprobantes().fnObtenerComprobantesTemp(usuarioActivo.id_usuario);
            gvComprobantes.DataSource = dtComprobantes;
            gvComprobantes.DataBind();
            btnValidar.Visible = (gvComprobantes.Rows.Count > 0);
            btnLimpiar.Visible = (gvComprobantes.Rows.Count > 0);
            ViewState["odtComprobantes"] = dtComprobantes;
        }
        private void fnLlenarSucursales()
        {
            DataTable dtSucursales = new DataTable();
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            if (usuarioActivo.Id_perfil == clsComun.fnObtenerIdPerfil("proveedor"))
            {
                 dtSucursales = new clsOperacionProveedores().fnObtenerSucursalesProveedorUsuario(usuarioActivo.id_usuario);
                
            }

            if (dtSucursales.Rows.Count > 0)
            {
                ddlSucursales.DataSource = dtSucursales;
                ddlSucursales.DataValueField = "id_sucursal";
                ddlSucursales.DataTextField = "nombre";
                ddlSucursales.DataBind();
            }
            else
            {
                ListItem item = new ListItem("<"+Resources.resCorpusCFDIEs.varSucursalNoAsignada+">", "0");
                ddlSucursales.Items.Add(item);
                ddlSucursales.DataBind();
                fnActivarControles(false);
            }
        }

        private void fnActivarControles(bool activo)
        {
            btnSubir.Enabled = activo;
            btnValidar.Enabled = activo;
            fuComprobantes.Enabled = activo;
            fuPdf.Enabled = activo;
            ddlSucursales.Enabled = activo;
        }
        protected void btnSubir_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuComprobantes.HasFile)
                {


                    if (Path.GetExtension(fuComprobantes.FileName).ToUpper() == ".ZIP")
                    {
                        try
                        {
                            //Si el archivo es ZIP, obtiene los comprobantes y sus PDF's
                            Stream strZip = fuComprobantes.PostedFile.InputStream;
                            string sDuplicdos = ExtraerArchivosZip(strZip);

                            if (string.IsNullOrWhiteSpace(sDuplicdos))
                                lMensaje.Text = Resources.resCorpusCFDIEs.varArchivoSubido;
                            else
                            {
                                lMensaje.Text = sDuplicdos;
                            }
                            //DataTable tabla = (DataTable)ViewState["odtComprobantes"];
                            //gvComprobantes.DataSource = tabla;
                            //gvComprobantes.DataBind();
                            fnObtenerComprobantes();
                        }
                        catch (Exception ex)
                        {
                            lMensaje.Text = ex.Message;
                        }
                    }
                    else if (Path.GetExtension(fuComprobantes.FileName).ToUpper() == ".XML")
                    {
                        try
                        {
                            string sMaxZip = clsComun.ObtenerParamentro("MaxZipFiles");
                            DataTable tabla = new DataTable();
                            if (ViewState["odtComprobantes"] != null)
                                tabla = (DataTable)ViewState["odtComprobantes"];
                            if(tabla.Rows.Count+1 > Convert.ToInt32(sMaxZip))
                            {
                                lMensaje.Text = Resources.resCorpusCFDIEs.varComprobantesNumeroMax;
                                    return;
                            }
                            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
                            if (fuPdf.HasFile)
                            {
                                //Si es XML, verifica que también se suba un archivo PDF
                                if (!(Path.GetExtension(fuPdf.FileName).ToUpper() == ".PDF"))
                                {
                                    lMensaje.Text = Resources.resCorpusCFDIEs.varSoloPdf;
                                    return;
                                }
                                bool existe = false;
                               
                                //Verifica que el archivo no se encuentre en la lista
                                if (tabla != null)
                                {
                                    foreach (DataRow row in tabla.Rows)
                                    {
                                        if (row["nombre_xml"].ToString() == fuComprobantes.FileName.ToString())
                                        {
                                            lMensaje.Text = Resources.resCorpusCFDIEs.varArchivoLista;
                                            existe = true;
                                            break;
                                        }
                                    }
                                }

                                if (!existe)
                                {
                                    //Si no existe en la lista, verifica que el PDF tenga el mismo nombre
                                    if (Path.GetFileNameWithoutExtension(fuComprobantes.FileName) !=
                                        Path.GetFileNameWithoutExtension(fuPdf.FileName))
                                    {
                                        lMensaje.Text = Resources.resCorpusCFDIEs.varPdfXmlIgual;
                                        return;
                                    }
                                    XmlDocument xXml = new XmlDocument();
                                    xXml.Load(fuComprobantes.FileContent);
                                    if (xXml.InnerXml.ToString().StartsWith("<?xml"))
                                    {
                                        string reemplazar = xXml.InnerXml.ToString().Split('>')[0] + ">";
                                        xXml.LoadXml(xXml.InnerXml.ToString().Replace(reemplazar,""));
                                    }
                                    //Verifica que el documento no esté en la base de datos
                                    if (new clsOperacionComprobantes().fnComprobanteExiste(xXml))
                                    {
                                        lMensaje.Text = Resources.resCorpusCFDIEs.varArchivoBase;
                                        return;
                                    }

                                    new clsOperacionComprobantes().fnGuardarComprobanteTemp(
                                        xXml,
                                        usuarioActivo.id_usuario,
                                        fuPdf.FileBytes,
                                        Path.GetFileName(fuComprobantes.FileName),
                                        Path.GetFileName(fuPdf.FileName),
                                        true,
                                        string.Empty,
                                        false);

                                    //if (ViewState["odtComprobantes"] == null)
                                    //{
                                    //    //Genera la tabla, si esta no existe y agrega el comprobante
                                    //    tabla = GenerarDataTableArchivos(fuComprobantes.FileName,
                                    //       fuPdf.FileName, fuComprobantes.FileBytes, fuPdf.FileBytes);
                                    //    ViewState["odtComprobantes"] = tabla;
                                    //}
                                    //else
                                    //{
                                    //    //Si existe, agrega el comprobante
                                    //    tabla = (DataTable)ViewState["odtComprobantes"];
                                    //    DataRow drNewRow = tabla.NewRow();
                                    //    drNewRow["nombre_xml"] = Path.GetFileName(fuComprobantes.FileName);
                                    //    drNewRow["nombre_pdf"] = Path.GetFileName(fuPdf.FileName); ;
                                    //    drNewRow["validar"] = true;
                                    //    drNewRow["error"] = string.Empty;
                                    //    drNewRow["valido"] = false;
                                    //    drNewRow["xml"] = fuComprobantes.FileBytes;
                                    //    drNewRow["pdf"] = fuPdf.FileBytes;
                                    //    tabla.Rows.Add(drNewRow);
                                    //    ViewState["odtComprobantes"] = tabla;

                                    //}
                                }
                                //gvComprobantes.DataSource = tabla;
                                //gvComprobantes.DataBind();
                                fnObtenerComprobantes();
                                lMensaje.Text = "";


                            }
                            else
                            {
                                lMensaje.Text = Resources.resCorpusCFDIEs.varSeleccionePdf;
                            }
                        }
                        catch (Exception ex)
                        {
                            lMensaje.Text = Resources.resCorpusCFDIEs.varSeleccionePdf;
                        }


                    }
                    else
                    {
                        lMensaje.Text = Resources.resCorpusCFDIEs.varFallaReintentar;
                    }
                }
                
                fnActivarControles(true);
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Busca un archivo con el mismo nombre de archivo antes de agregarlo a la lista
        /// </summary>
        /// <param name="tabla">Tabla con los archivos a validar</param>
        /// <param name="info">archivo a adjuntar</param>
        /// <returns></returns>
        private bool fnBuscaDuplicado(DataTable tabla, FileInfo info)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Valida uno a uno los comprobantes mediante el webService de PAX
        /// </summary>
        private void ValidarComprobantes()
        {
            int nIdSucursal = Convert.ToInt32(ddlSucursales.SelectedValue);
            DataTable dtComprobantesl = (DataTable)ViewState["odtComprobantes"];
            int idUsuarioActual = clsComun.fnUsuarioEnSesion().id_usuario;
            if (dtComprobantesl != null && dtComprobantesl.Rows.Count > 0)
            {
                bool pruebasValidacion = clsComun.fnObtenerParametroBool("pruebas_validacion");
                foreach (DataRow row in dtComprobantesl.Rows)
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        byte[] bPdf = new byte[0];
                        if (!string.IsNullOrEmpty(row["nombre_xml"].ToString()))
                        {
                            //Verifica que el pdf y el xml tengan el mismo nombre
                            if (Path.GetFileNameWithoutExtension(row["nombre_xml"].ToString())
                                == Path.GetFileNameWithoutExtension(row["nombre_pdf"].ToString()))
                            {
                                DataTable dtComprobante = new DataTable();
                                if (Path.GetExtension(row["nombre_xml"].ToString()).ToLower() == ".xml")
                                {
                                    //doc.Load(row["url_xml"].ToString());
                                    //Si el documento es xml, lo carga en la variable
                                    int nIdComprobante = Convert.ToInt32(row["id_comprobante_temporal"]);
                                    dtComprobante = new clsOperacionComprobantes().fnObtenerComprobanteArchivosTemp(nIdComprobante);
                                    //MemoryStream ms = new MemoryStream((byte[])dtComprobante.Rows[0]["xml"]);
                                    doc.LoadXml(dtComprobante.Rows[0]["xml"].ToString());
                                }
                                else
                                {
                                    row["valido"] = false;
                                    row["error"] = Resources.resCorpusCFDIEs.varArchivoNoXml;
                                    break;
                                }
                                if (Path.GetExtension(row["nombre_pdf"].ToString()).ToLower() == ".pdf")
                                {
                                    //bPdf = File.ReadAllBytes(row["url_pdf"].ToString());
                                    //Si el otro archivo es pdf, obtiene los bytes
                                    bPdf = (byte[])dtComprobante.Rows[0]["pdf"];

                                }
                                else
                                {
                                    row["valido"] = false;
                                    row["error"] = Resources.resCorpusCFDIEs.varArchivoNoPdf;
                                    break;
                                }
                                //Se valida de nuevo que no exista en la base de datos
                                if (new clsOperacionComprobantes().fnComprobanteExiste(doc))
                                {
                                    row["valido"] = false;
                                    row["error"] = Resources.resCorpusCFDIEs.varComprobanteExiste;
                                    break;
                                }
                                clsResultadoValidacion resultado;
                                //Valida el documento 
                                if (pruebasValidacion)
                                {
                                    resultado = new clsOperacionComprobantes().fnValidarTest(doc);
                                }
                                else
                                {
                                    resultado = new clsOperacionComprobantes().fnValidarProduccion(doc);
                                }
                                //clsDatosComprobante dcComprobante = new clsDatosComprobante(doc, resultado, "", idUsuarioActual, bPdf,nIdSucursal);
                                //dcComprobante.fnGuardarComprobante();
                                
                                //Se guarda en la base de datos
                                new clsOperacionComprobantes().fnGuardarComprobante(doc, resultado, idUsuarioActual, bPdf, nIdSucursal, DateTime.Now);

                                row["valido"] = resultado.valido;
                                row["error"] = resultado.mensaje;
                            }
                            else
                            {
                                row["valido"] = false;
                                row["error"] = Resources.resCorpusCFDIEs.varArchivoPdfNoCoincide;
                            }
                        }
                        else
                        {
                            row["valido"] = false;
                            row["error"] = Resources.resCorpusCFDIEs.varArchivoNoXml;
                        }
                    }
                    catch (Exception ex)
                    {
                        row["valido"] = false;
                        row["error"] = Resources.resCorpusCFDIEs.varFallaValidacion;
                    }
                    

                }
                //Envia el acuse a los destinatarios correspondientes
                clsEnviaAcuseValidacion oAcuse = new clsEnviaAcuseValidacion();
                if (oAcuse.bCuentaActiva)
                    oAcuse.fnEnviarAcuse(dtComprobantesl, nIdSucursal);
                gvComprobantes.DataSource = dtComprobantesl;
                gvComprobantes.DataBind();
            }
            else
            {
                lMensaje.Text = Resources.resCorpusCFDIEs.varNoComprobantes;
            }
        }

        

        ///// <summary>
        ///// Extrae los documentos contenidos en el archivo ZIP
        ///// </summary>
        ///// <param name="rutaCompletaArchivo">Ruta Completa del archivo ZIP</param>
        ///// <param name="rutaDirectorio">Directorio hacia donde se va a descomprimir los archivos</param>
        //private String ExtraerArchivos(string rutaCompletaArchivo, string rutaDirectorio)
        //{
        //    //REFERENCIA: https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples
        //    ZipFile archivoZip = new ZipFile(rutaCompletaArchivo);
        //    String sArchivosDuplicados = string.Empty;
        //    try
        //    {

        //        foreach (ZipEntry zipEntry in archivoZip)
        //        {
        //            if (!zipEntry.IsFile)
        //            {
        //                continue;           // Ignore directories
        //            }
        //            String entryFileName = zipEntry.Name;
        //            // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
        //            // Optionally match entrynames against a selection list here to skip as desired.
        //            // The unpacked length is available in the zipEntry.Size property.

        //            byte[] buffer = new byte[4096];     // 4K is optimum
        //            Stream zipStream = archivoZip.GetInputStream(zipEntry);

        //            // Manipulate the output filename here as desired.
        //            String fullZipToPath = Path.Combine(rutaDirectorio, entryFileName);
        //            string directoryName = Path.GetDirectoryName(fullZipToPath);
        //            if (!File.Exists(fullZipToPath))
        //            {
        //                if (!Directory.Exists(directoryName))
        //                    Directory.CreateDirectory(directoryName);

        //                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
        //                // of the file, but does not waste memory.
        //                // The "using" will close the stream even if an exception occurs.
        //                using (FileStream streamWriter = File.Create(fullZipToPath))
        //                {
        //                    StreamUtils.Copy(zipStream, streamWriter, buffer);
        //                }
        //            }
        //            else
        //            {
        //                String[] aRutaArchivo = fullZipToPath.Split('\\');
        //                sArchivosDuplicados += aRutaArchivo[aRutaArchivo.Length - 1] + "|";
        //            }
        //        }

        //        ObtenerListaComprobantes(rutaDirectorio);
        //        if (!string.IsNullOrWhiteSpace(sArchivosDuplicados))
        //            sArchivosDuplicados = sArchivosDuplicados.Remove(sArchivosDuplicados.Length - 1, 1);
        //        return sArchivosDuplicados;
        //    }
        //    finally
        //    {
        //        if (archivoZip != null)
        //        {
        //            archivoZip.IsStreamOwner = true; // Makes close also shut the underlying stream
        //            archivoZip.Close(); // Ensure we release resources
        //        }
        //    }
        //}

        /// <summary>
        /// Extrae los documentos contenidos en el archivo ZIP
        /// </summary>
        /// <param name="rutaCompletaArchivo">Ruta Completa del archivo ZIP</param>
        /// <param name="rutaDirectorio">Directorio hacia donde se va a descomprimir los archivos</param>
        private String ExtraerArchivosZip(Stream strZip)
        {
            //REFERENCIA: https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples
            ZipFile archivoZip = new ZipFile(strZip);
            String sArchivosDuplicados = string.Empty;
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            try
            {
                string sMaxZip = clsComun.ObtenerParamentro("MaxZipFiles");
                DataTable tabla = new DataTable();
                        if (ViewState["odtComprobantes"] != null)
                            tabla = (DataTable)ViewState["odtComprobantes"];
                if (archivoZip.Count > (Convert.ToInt32(sMaxZip) * 2))
                {
                    sArchivosDuplicados = string.Format(Resources.resCorpusCFDIEs.varComprobantesNumero, sMaxZip);
                    return sArchivosDuplicados;

                }
                else
                {
                    if (tabla.Rows.Count + (archivoZip.Count / 2) > Convert.ToInt32(sMaxZip))
                    {
                        sArchivosDuplicados = string.Format(Resources.resCorpusCFDIEs.varComprobantesNumeroMax, sMaxZip);
                        return sArchivosDuplicados;
                    }
                }
                //Se recorren los archivos del ZIP
                foreach (ZipEntry zipEntry in archivoZip)
                {
                    //Si es un directorio, lo ignora
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    byte[] pdf = new byte[0];
                    byte[] xml = new byte[0];
                    //Verifica que el archivo sea xml
                    if (Path.GetExtension(zipEntry.Name).ToLower() == ".xml")
                    {
                        bool existe = false;

                        
                        //Verifica que no se encuentre en la lista
                        if (tabla != null)
                        {
                            foreach (DataRow row in tabla.Rows)
                            {
                                if (row["nombre_xml"].ToString() == zipEntry.Name.ToString())
                                {
                                    sArchivosDuplicados += zipEntry.Name + "|";
                                    existe = true;
                                    break;
                                }
                            }
                        }

                        if (!existe)
                        {
                            XmlDocument xXml = new XmlDocument();
                             Stream strXml =archivoZip.GetInputStream(zipEntry);
                            xXml.Load(strXml);
                            if (xXml.InnerXml.ToString().StartsWith("<?xml"))
                            {
                                string reemplazar = xXml.InnerXml.ToString().Split('>')[0] + ">";
                                xXml.LoadXml(xXml.InnerXml.ToString().Replace(reemplazar, ""));
                            }
                            //Verifica que el archivo no exista en la base de datos
                            if (new clsOperacionComprobantes().fnComprobanteExiste(xXml))
                            {
                                sArchivosDuplicados += zipEntry.Name + "|";
                                continue;
                            }
                            string sUrlPdf = string.Empty;
                            string sNombrePdf = string.Empty;
                            ZipEntry zipPdf = null;
                            //Busca el archivo pdf con el mismo nombre
                            foreach (ZipEntry zipArc in archivoZip)
                            {
                                if (zipArc.IsFile)
                                {
                                    if (Path.GetExtension(zipArc.Name).ToLower() == ".pdf")
                                    {
                                        if (Path.GetFileNameWithoutExtension(zipArc.Name) == Path.GetFileNameWithoutExtension(zipEntry.Name))
                                        {
                                            zipPdf = zipArc;
                                            break;
                                        }
                                    }
                                }
                            }
                            //Si existe, obtiene los bytes 
                            if (zipPdf != null)
                            {
                                sNombrePdf = zipPdf.Name;
                                Stream stPdf = archivoZip.GetInputStream(zipPdf);
                                pdf = new byte[zipPdf.Size];
                                stPdf.Read(pdf, 0, pdf.Length);
                            }

                           
                            //xml = Encoding.Default.GetBytes(xXml.OuterXml);
                            MemoryStream ms = new MemoryStream();
                            xXml.Save(ms);
                            xml = ms.ToArray();
                            new clsOperacionComprobantes().fnGuardarComprobanteTemp(
                                xXml,
                                usuarioActivo.id_usuario,
                                pdf,
                                zipEntry.Name,
                                sNombrePdf,
                                true,
                                string.Empty,
                                false);

                            //if (ViewState["odtComprobantes"] == null)
                            //{
                            //    //Si no existe la tabla, la crea y agrega los archivos
                            //    tabla = GenerarDataTableArchivos(zipEntry.Name,  sNombrePdf, xml, pdf);
                            //    ViewState["odtComprobantes"] = tabla;
                            //}
                            //else
                            //{
                            //    //Si ya existe, agrega los archivos
                            //    tabla = (DataTable)ViewState["odtComprobantes"];
                            //    DataRow drNewRow = tabla.NewRow();
                            //    drNewRow["nombre_xml"] = zipEntry.Name;
                            //    drNewRow["nombre_pdf"] = sNombrePdf;
                            //    drNewRow["validar"] = true;
                            //    drNewRow["error"] = string.Empty;
                            //    drNewRow["valido"] = false;
                            //    drNewRow["xml"] = xml;
                            //    drNewRow["pdf"] = pdf;
                            //    tabla.Rows.Add(drNewRow);
                            //    ViewState["odtComprobantes"] = tabla;

                            //}
                        }
                        else
                        {
                            sArchivosDuplicados += zipEntry.Name+ "|";
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                sArchivosDuplicados = Resources.resCorpusCFDIEs.varErrorZip;
            } 
            finally
            {
                if (archivoZip != null)
                {
                    archivoZip.IsStreamOwner = true; // Makes close also shut the underlying stream
                    archivoZip.Close(); // Ensure we release resources
                }
                strZip.Close();
            }
            if (!string.IsNullOrWhiteSpace(sArchivosDuplicados))
            {
                sArchivosDuplicados = Resources.resCorpusCFDIEs.varArchivosLista + ": " + sArchivosDuplicados.Replace("|", ", ");
            }
            return sArchivosDuplicados;
        }

        ///// <summary>
        ///// Obtiene la lista de los archivos xml contenidos en la ruta especificada
        ///// </summary>
        ///// <param name="rutaDirectorio">Directorio donde se encuentran los Comprobantes</param>
        //private void ObtenerListaComprobantes(string rutaDirectorio)
        //{
        //    if (Directory.Exists(rutaDirectorio))
        //    {
        //        try
        //        {
        //            DirectoryInfo dirInfo = new DirectoryInfo(rutaDirectorio);
        //            FileInfo[] archivos = dirInfo.GetFiles("*.xml");
        //            dtComprobantes = GenerarDataTableArchivos(archivos);
        //            gvComprobantes.AutoGenerateColumns = false;
        //            gvComprobantes.DataSource = dtComprobantes;
        //            ViewState["odtComprobantes"] = dtComprobantes;
        //            gvComprobantes.DataBind();
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //}
        ///// <summary>
        ///// Genera un DataTable a partir de la info de los archivos
        ///// </summary>
        ///// <param name="infoArchivos">Informacion de los archivos</param>
        ///// <returns>DataTable lleno con la informacion de los archivos</returns>
        //private DataTable GenerarDataTableArchivos(FileInfo[] infoArchivos)
        //{
        //    DataTable dtInfoArchivos = new DataTable("InfoArchivos");
        //    dtInfoArchivos.Columns.Add(new DataColumn("nombre", typeof(string)));
        //    dtInfoArchivos.Columns.Add(new DataColumn("validar", typeof(bool)));
        //    dtInfoArchivos.Columns.Add(new DataColumn("nombre_completo", typeof(string)));
        //    dtInfoArchivos.Columns.Add(new DataColumn("error", typeof(string)));
        //    dtInfoArchivos.Columns.Add(new DataColumn("valido", typeof(bool)));

        //    foreach (FileInfo info in infoArchivos)
        //    {
        //        DataRow drRegistro = dtInfoArchivos.NewRow();
        //        drRegistro["nombre"] = info.Name;
        //        drRegistro["validar"] = true;
        //        drRegistro["nombre_completo"] = info.FullName;
        //        drRegistro["error"] = string.Empty;
        //        drRegistro["valido"] = false;
        //        dtInfoArchivos.Rows.Add(drRegistro);
        //    }

        //    return dtInfoArchivos;
        //}
        ///// <summary>
        ///// Sobrecarga del metodo para el caso en que suban un xml y no el zip completo
        ///// </summary>
        ///// <returns></returns>
        //private DataTable GenerarDataTableArchivos(FileInfo info)
        //{
        //    DataTable dtInfoArchivos = new DataTable("InfoArchivos");
        //    dtInfoArchivos.Columns.Add(new DataColumn("nombre", typeof(string)));
        //    dtInfoArchivos.Columns.Add(new DataColumn("validar", typeof(bool)));
        //    dtInfoArchivos.Columns.Add(new DataColumn("nombre_completo", typeof(string)));
        //    dtInfoArchivos.Columns.Add(new DataColumn("error", typeof(string)));
        //    dtInfoArchivos.Columns.Add(new DataColumn("valido", typeof(bool)));

        //    DataRow drRegistro = dtInfoArchivos.NewRow();
        //    drRegistro["nombre"] = info.Name;
        //    drRegistro["validar"] = true;
        //    drRegistro["nombre_completo"] = info.FullName;
        //    drRegistro["error"] = string.Empty;
        //    drRegistro["valido"] = false;
        //    dtInfoArchivos.Rows.Add(drRegistro);

        //    return dtInfoArchivos;
        //}

        /// <summary>
        /// Crea la tabla de los documentos y agrega el primer elemento
        /// </summary>
        /// <returns></returns>
        private DataTable GenerarDataTableArchivos(string sNombreXml, string sNombrePdf, 
           byte[] xml, byte[] pdf)
        {
            DataTable dtInfoArchivos = new DataTable("InfoArchivos");
            dtInfoArchivos.Columns.Add(new DataColumn("nombre_xml", typeof(string)));
            dtInfoArchivos.Columns.Add(new DataColumn("nombre_pdf", typeof(string)));
            dtInfoArchivos.Columns.Add(new DataColumn("validar", typeof(bool)));
            dtInfoArchivos.Columns.Add(new DataColumn("error", typeof(string)));
            dtInfoArchivos.Columns.Add(new DataColumn("valido", typeof(bool)));
            dtInfoArchivos.Columns.Add(new DataColumn("xml", typeof(byte[])));
            dtInfoArchivos.Columns.Add(new DataColumn("pdf", typeof(byte[])));

            DataRow drRegistro = dtInfoArchivos.NewRow();
            drRegistro["nombre_xml"] = sNombreXml;
            drRegistro["nombre_pdf"] = sNombrePdf;
            drRegistro["validar"] = true;
            drRegistro["error"] = string.Empty;
            drRegistro["valido"] = false;
            drRegistro["xml"] = xml;
            drRegistro["pdf"] = pdf;
            dtInfoArchivos.Rows.Add(drRegistro);

            return dtInfoArchivos;
        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            ValidarComprobantes();
        }

        protected void btnFuPdf_Click(object sender, EventArgs e)
        {
            if (Path.GetExtension(fuComprobantes.FileName).ToLower() == ".xml")
            {
                fuPdf.Visible = true;
                lblfuPdf.Visible = true;
            }
        }
        protected override void InitializeCulture()
        {
            if (Session["Culture"] != null)
            {
                string lang = Session["Culture"].ToString();
                if ((lang != null) || (lang != ""))
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                }
            }
            else
            {
                string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            }
        }
        public void Page_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError().GetBaseException();

            if (!string.IsNullOrEmpty(objErr.Message))
            {
                Server.ClearError();
                Response.Redirect("~/webGlobalError.aspx");
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            new clsOperacionComprobantes().fnEliminarComprobantesTemp(usuarioActivo.id_usuario);
            fnObtenerComprobantes();

        }

        
    }
}