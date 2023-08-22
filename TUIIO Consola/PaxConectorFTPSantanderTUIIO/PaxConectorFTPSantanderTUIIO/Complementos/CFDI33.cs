using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

    class clsAtributos33
    {
        public Int32 concepto { get; set; }
        public String[] atributos { get; set; }
    }

    class clsAtributosPTE33
    {
        public Int32 PTE { get; set; }
        public Int32 concepto { get; set; }
        public String[] atributos { get; set; }
    }

    //Clase de Lectura de XML Formato Exclusivo de AeroMexico
    public class clsAtributosAeromexico
    {
        public Int32 concepto { get; set; }
        public List<String> atributos { get; set; }
    }

    public class ComprobanteFiscalDigital33
    {
        public static IList RecuperaListaArchivos(String directorioRaiz)
        {
            IList listaArchivos = Directory.GetFiles(directorioRaiz).ToList();
            return listaArchivos;
        }

        //Crea Elemento Raiz para Comprobante Fiscal Digital (Puede contener namespaces para los Complementos)
        public static void fnCrearElementoRootComplemento33(XmlDocument pxDoc, String[] pasAtributos, String tNamespace)
        {
            XmlAttribute xAttr;

            foreach (String a in pasAtributos)
            {
                String[] valores = a.Split('@');
                xAttr = pxDoc.CreateAttribute(valores[0]);
                xAttr.Value = valores[1];
                pxDoc.DocumentElement.Attributes.Append(xAttr);
            }

            String[] pspace = { "" };


            if (!(String.IsNullOrEmpty(tNamespace)))
            {
                pspace = tNamespace.Split('@');


                xAttr = pxDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                xAttr.Value = "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd";

                foreach (String item in pspace)
                {
                    String[] items = item.Split('|');
                    xAttr.Value = xAttr.Value + " " + items[1] + " " + items[2];
                    pxDoc.DocumentElement.SetAttribute("xmlns:" + items[0], items[1]);
                }
                pxDoc.DocumentElement.Attributes.Append(xAttr);
            }
            else
            {
                xAttr = pxDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                xAttr.Value = "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd";
                pxDoc.DocumentElement.Attributes.Append(xAttr);
                pxDoc.DocumentElement.Attributes.RemoveNamedItem("schemaLocation");
            }
        }

        //Crear Nodo para el Comprobante Fiscal Digital
        public static XmlElement fnCrearElemento(XmlDocument pxDoc, String psElemento, String[] pasAtributos)
        {
            XmlAttribute xAttr;
            XmlElement elemento = pxDoc.CreateElement("cfdi", psElemento, "http://www.sat.gob.mx/cfd/4");

            foreach (String a in pasAtributos)
            {
                String[] valores = a.Split('@');
                xAttr = pxDoc.CreateAttribute(valores[0]);
                xAttr.Value = valores[1];
                elemento.Attributes.Append(xAttr);
            } return elemento;
        }

        //Recupera Namespaces por Tipo de Complemento
        public static String fnRecuperaNamespaceComplemento(String nombre)
        {
            String Resultado = String.Empty;

            switch (nombre)
            {
                //Lista de Complementos Comprobante Fiscal Digital
                #region Complementos

                //Estado de cuenta de combustibles de monederos electrónicos como Factura Electrónica "ECC11"
                case "ecc11": Resultado = "ecc11" + "|" + "http://www.sat.gob.mx/EstadoDeCuentaCombustible"
                                              + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/EstadoDeCuentaCombustible/ecc11.xsd";
                    break;

                //Estado de cuenta de combustibles de monederos electrónicos como Factura Electrónica "ECC11"
                case "pagos10": Resultado = "pago10" + "|" + "http://www.sat.gob.mx/Pagos"
                                              + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos10.xsd";
                    break;
            //Estado de cuenta de combustibles de monederos electrónicos como Factura Electrónica "ECC11"
            case "pagos20":
                Resultado = "pago20" + "|" + "http://www.sat.gob.mx/Pagos"
                              + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd";
                break;

            case "cce11": Resultado = "cce11" + "|" + "http://www.sat.gob.mx/ComercioExterior11"
                                          + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/ComercioExterior11/ComercioExterior11.xsd";
                    break;

                //Datos requeridos para la emisión de Facturas Electrónicas por donativos.
                case "donatarias": Resultado = "donat" + "|" + "http://www.sat.gob.mx/donat"
                                                       + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/donat/donat11.xsd";
                    break;

                //Complemento para Facturas Electrónicas que amparen la compra - venta de Divisas
                case "divisas": Resultado = "divisas" + "|" + "http://www.sat.gob.mx/divisas"
                                                      + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/divisas/divisas.xsd";
                    break;

                //Complemento para incluir Otros Derechos e impuestos en la Factura Electrónica.
                case "implocal": Resultado = "implocal" + "|" + "http://www.sat.gob.mx/implocal"
                                                        + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/implocal/implocal.xsd";
                    break;

                //Complemento para incluir Leyendas Fiscales a la Factura Electrónica.
                case "leyendasFisc": Resultado = "leyendasFisc" + "|" + "http://www.sat.gob.mx/leyendasFiscales"
                                                                + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/leyendasFiscales/leyendasFisc.xsd";
                    break;

                //Complemento para Facturas Electrónicas de Personas Físicas integrantes de coordinados.
                case "pfic": Resultado = "pfic" + "|" + "http://www.sat.gob.mx/pfic"
                                                + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/pfic/pfic.xsd";
                    break;

                //Complemento para el manejo de datos de Turista Pasajero Extranjero en las Facturas Electrónicas.
                case "tpe": Resultado = "tpe" + "|" + "http://www.sat.gob.mx/TuristaPasajeroExtranjero"
                                              + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/TuristaPasajeroExtranjero/TuristaPasajeroExtranjero.xsd";
                    break;


                //Complemento para incluir en los Recibos de Nomina Version 1.2
                case "nomina12": Resultado = "nomina12" + "|" + "http://www.sat.gob.mx/nomina12"
                                                        + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina12.xsd";
                    break;

                case "ine": Resultado = "ine" + "|" + "http://www.sat.gob.mx/ine"
                                              + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/ine/INE11.xsd";
                    break;

                //Complemento para la expedición de comprobantes fiscales por la donación en la facilidad fiscal de Pago en Especie
                case "pagoenespecie": Resultado = "pagoenespecie" + "|" + "http://www.sat.gob.mx/pagoenespecie"
                                                                  + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/pagoenespecie/pagoenespecie.xsd";
                    break;
                //Complemento para integrar la información emitida por un prestador de servicios de monedero electrónico de vales de despensa en las Facturas Electrónicas
                case "valesdedespensa": Resultado = "valesdedespensa" + "|" + "http://www.sat.gob.mx/valesdedespensa"
                                                                      + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/valesdedespensa/valesdedespensa.xsd";
                    break;
                //Complemento para integrar la información de consumo de combustibles por monedero electrónico en las Facturas Electrónicas
                case "consumodecombustibles": Resultado = "consumodecombustibles" + "|" + "http://www.sat.gob.mx/consumodecombustibles"
                                                                                  + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/consumodecombustibles/consumodecombustibles.xsd";
                    break;
                //Complemento para el manejo de datos de Aerolíneas para pasajeros en las Facturas Electrónicas
                case "aerolineas": Resultado = "aerolineas" + "|" + "http://www.sat.gob.mx/aerolineas"
                                                            + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/aerolineas/aerolineas.xsd";
                    break;
                //Complemento para el manejo de la enajenación de bienes inmuebles en las Facturas Electrónicas
                case "notariospublicos": Resultado = "notariospublicos" + "|" + "http://www.sat.gob.mx/notariospublicos"
                                                                        + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/notariospublicos/notariospublicos.xsd";
                    break;
                //Complemento Vehículo Usado
                case "vehiculousado": Resultado = "vehiculousado" + "|" + "http://www.sat.gob.mx/vehiculousado"
                                                                  + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/vehiculousado/vehiculousado.xsd";
                    break;
                //Este complemento será utilizado por el prestador de servicios parciales de construcción, de conformidad con el “Decreto por el que se otorgan medidas de apoyo a la vivienda y otras medidas fiscales”, publicado en el Diario Oficial de la Federación el 26 de marzo de 2015 (incisos a) y b), de la fracción II, del artículo 2 del citado Decreto).
                case "servicioparcial": Resultado = "servicioparcial" + "|" + "http://www.sat.gob.mx/servicioparcialconstruccion"
                                                                      + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/servicioparcialconstruccion/servicioparcialconstruccion.xsd";
                    break;
                //Complemento para incorporar la información relativa a los estímulos por la renovación del parque vehicular del autotransporte y por el que se otorgan medidas para la sustitución de vehículos de autotransporte de pasaje y carga.
                case "decreto": Resultado = "decreto" + "|" + "http://www.sat.gob.mx/renovacionysustitucionvehiculos"
                                                      + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/renovacionysustitucionvehiculos/renovacionysustitucionvehiculos.xsd";
                    break;
                //Complemento para incorporar la información que integra el certificado de destrucción de vehículos destruidos por los centros de destrucción autorizados por el SAT.
                case "certdest": Resultado = "certificadodestruccion" + "|" + "http://www.sat.gob.mx/certificadodestruccion"
                                                                                    + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/certificadodestruccion/certificadodedestruccion.xsd";
                    break;
                //Complemento al Comprobante Fiscal Digital por Internet (CFDI) para el manejo de la enajenación de obras de artes plásticas y antigüedades.
                case "obrasarte": Resultado = "obrasarte" + "|" + "http://www.sat.gob.mx/arteantiguedades"
                                                          + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/arteantiguedades/obrasarteantiguedades.xsd";
                    break;

                #endregion
                //Lista de Complementos Concepto Comprobante Fiscal Digital
                #region Complementos Concepto

                //Complemento concepto para la expedición de comprobantes fiscales por parte de Instituciones Educativas Privadas, para los efectos del artículo primero y cuarto del decreto por el que se otorga un estímulo fiscal a las personas físicas en relación con los pagos por servicios educativos.
                case "iedu": Resultado = "iedu" + "|" + "http://www.sat.gob.mx/iedu"
                                                + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/iedu/iedu.xsd";
                    break;
                //Este complemento concepto permite incorporar a los fabricantes, ensambladores o distribuidores autorizados de automóviles nuevos, así como aquellos que importen automóviles para permanecer en forma definitiva en la franja fronteriza norte del país y en los Estados de Baja California, Baja California Sur y la región parcial del Estado de Sonora, a una Factura Electrónica la clave vehicular que corresponda a la versión enajenada.
                case "ventavehiculos": Resultado = "ventavehiculos" + "|" + "http://www.sat.gob.mx/ventavehiculos"
                                                                    + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/ventavehiculos/ventavehiculos11.xsd";
                    break;
                //Complemento concepto para Facturas Electrónicas por orden y cuenta de terceros.
                case "terceros": Resultado = "terceros" + "|" + "http://www.sat.gob.mx/terceros"
                                                        + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/terceros/terceros11.xsd";
                    break;

                #endregion
            }
            return Resultado;
        }

        //Crear Nodo Complemento para Comprobante Fiscal Digital
        public static XmlElement fnCrearElementoComplemento(XmlDocument pxDoc, string psElemento, string[] pasAtributos, string preFijo, string NamespaceURI)
        {
            XmlAttribute xAttr;
            XmlElement elemento = pxDoc.CreateElement(preFijo, psElemento, NamespaceURI);

            foreach (string a in pasAtributos)
            {
                string[] valores = a.Split('@');
                xAttr = pxDoc.CreateAttribute(valores[0]);
                xAttr.Value = valores[1];
                elemento.Attributes.Append(xAttr);
            } return elemento;
        }

        public static XmlElement fnCrearElemento(XmlDocument pxDoc, String psElemento, String preFijo, String NamespaceURI)
        {
            XmlElement elemento = pxDoc.CreateElement(preFijo, psElemento, NamespaceURI);
            return elemento;
        }

        public static XmlElement fnCrearElemento(XmlDocument pxDoc, String psElemento, String[] pasAtributos, String prefix, String NSpace)
        {
            XmlAttribute xAttr;
            XmlElement elemento = pxDoc.CreateElement(prefix, psElemento, NSpace);

            foreach (String a in pasAtributos)
            {
                String[] valores = a.Split('@');
                xAttr = pxDoc.CreateAttribute(valores[0]);
                xAttr.Value = valores[1];
                elemento.Attributes.Append(xAttr);
            } return elemento;
        }
    }