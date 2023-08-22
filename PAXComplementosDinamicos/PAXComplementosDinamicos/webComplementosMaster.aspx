<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="webComplementosMaster.aspx.cs" Inherits="webComplementosMaster" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <script type="text/javascript" language="javascript">

        var regex = /^(.+?)(\d+)$/i;
        var sXMLLayout = "";
        var cloneIndex = $(".clonedInput").length;
        var nNumeroComplemento = 0;

        function fnRevisarValidacion(cElementos) {

            var sMensaje = "";
        
            if (cElementos == null) {

                return;
            }

            for (var i = 0; i < cElementos.length; i++) {

                var sTipoElemento = cElementos[i].tagName.toLowerCase();
                var sNombreElemento = cElementos[i].id;

                if (sTipoElemento == "input") {

                    var oInput = document.getElementById(sNombreElemento);

                    if (oInput.checkValidity() == false) {
                    sMensaje = sMensaje + "|Elemento: " + oInput + ", Mensaje: " + oInput.validationMessage;
//                        alert(oInput.validationMessage);
                    }

                }
                else if (sTipoElemento == "select") {

                    
                }
                else {

                }
            }

            return sMensaje;
        }


        function fnColapsarContenedor() {
            var acc = document.getElementsByClassName("accordion");
            var i;

            for (i = 0; i < acc.length; i++) {
                acc[i].onclick = function () {
                    this.classList.toggle("active");
                    var panel = this.nextElementSibling;
                    if (panel.style.display === "block") {
                        panel.style.display = "none";
                    } else {
                        panel.style.display = "block";
                    }
                }
            }
        }

        function fnSelectedIndexChanged(sNombreObjeto, sValor) {
            var Id = sNombreObjeto.id;

            //                if (localStorage.getItem(sValor)) {
            //                    $('#' + Id + ' option:contains(' + localStorage.getItem(sValor) + ')').prop('selected', true);
            //                }
            var nIndice = document.getElementById(Id).selectedIndex;
            var x = document.getElementById(Id).children[nIndice];
            x.setAttribute("selected", "selected");


            //                $('#' + Id + ' option:contains(' + sValor + ')').prop('selected', true);
        }

        function showDiv(elem) {
            if (elem.value == 'No') {
                document.getElementById('tabla2').style.display = "none";
                document.getElementById('tabla2').disabled = true;
                document.getElementById('tabla2').required = false;
                document.getElementById('tabla1').style.display = "block";
                document.getElementById('tabla1').disabled = false;
            }
            else if (elem.value == 'Sí') {
                document.getElementById('tabla2').style.display = "block";
                document.getElementById('tabla2').disabled = false;
                document.getElementById('tabla1').style.display = "none";
                document.getElementById('tabla1').disabled = true;
                document.getElementById('tabla1').required = false;
            }
        }

        function showDiv2(elem) {
            if (elem.value == 'No') {
                document.getElementById('tabla4').style.display = "none";
                document.getElementById('tabla4').disabled = true;
                document.getElementById('tabla4').required = false;
                document.getElementById('tabla3').style.display = "block";
                document.getElementById('tabla3').disabled = false;
            }
            else if (elem.value == 'Sí') {
                document.getElementById('tabla4').style.display = "block";
                document.getElementById('tabla4').disabled = false;
                document.getElementById('tabla3').style.display = "none";
                document.getElementById('tabla3').disabled = true;
                document.getElementById('tabla3').required = false;
            }
        }

        function showDiv3(elem) {
            if (elem.value == '01') {
                document.getElementById('DecretoSustitVehicular').style.display = "none";
                document.getElementById('DecretoSustitVehicular').disabled = true;
                document.getElementById('DecretoSustitVehicular').required = false;
                document.getElementById('DecretoRenovVehicular').style.display = "block";
                document.getElementById('DecretoRenovVehicular').disabled = false;
            }
            else if (elem.value == '02') {
                document.getElementById('DecretoSustitVehicular').style.display = "block";
                document.getElementById('DecretoSustitVehicular').disabled = false;
                document.getElementById('DecretoRenovVehicular').style.display = "none";
                document.getElementById('DecretoRenovVehicular').disabled = true;
                document.getElementById('DecretoRenovVehicular').required = false;
            }
        }

        function showDiv4(elem) {
            if (elem.value == '01') {
                document.getElementById('TrasladosLocales').style.display = "none";
                document.getElementById('TrasladosLocales').disabled = true;
                document.getElementById('TrasladosLocales').required = false;
                document.getElementById('RetencionesLocales').style.display = "block";
                document.getElementById('RetencionesLocales').disabled = false;
            }
            else if (elem.value == '02') {
                document.getElementById('TrasladosLocales').style.display = "block";
                document.getElementById('TrasladosLocales').disabled = false;
                document.getElementById('RetencionesLocales').style.display = "none";
                document.getElementById('RetencionesLocales').disabled = true;
                document.getElementById('RetencionesLocales').required = false;
            }
        }

        function ImpRet(elem) {
            if (document.getElementById("importeRetencion").checked == true) {
                  document.getElementById('tasRet').disabled = true;
                  document.getElementById('tasRet').required = false;
                  document.getElementById('ImpRet').disabled = false;

            } else {
                  document.getElementById('tasRet').disabled = false;
                  document.getElementById('ImpRet').disabled = true;
                  document.getElementById('ImpRet').required = false;
            }
          }

          function ImpTras(elem) {
              if (document.getElementById("importeTraslado").checked == true) {
                  document.getElementById('tasTras').disabled = true;
                  document.getElementById('tasTras').required = false;
                  document.getElementById('ImpTras').disabled = false;

              } else {
                  document.getElementById('tasTras').disabled = false;
                  document.getElementById('ImpTras').disabled = true;
                  document.getElementById('ImpTras').required = false;
              }
          }

          function terRadio(elem) {
              if (document.getElementById("importeRetencion2").checked == true) {
                  document.getElementById('InformacionAduanera').style.display = "block";
                  document.getElementById('InformacionAduanera').disabled = false;
              } else {
                  document.getElementById('InformacionAduanera').style.display = "none";
                  document.getElementById('InformacionAduanera').disabled = true;
              }
              if (document.getElementById("importeRetencion3").checked == true) {
                  document.getElementById('CuentaPredial').style.display = "block";
                  document.getElementById('CuentaPredial').disabled = false;
              } else {
                  document.getElementById('CuentaPredial').style.display = "none";
                  document.getElementById('CuentaPredial').disabled = true;
              }
              if (document.getElementById("importeRetencion4").checked == true) {
                  document.getElementById('Parte').style.display = "block";
                  document.getElementById('Parte').disabled = false;
              } else {
                  document.getElementById('Parte').style.display = "none";
                  document.getElementById('Parte').disabled = true;
              }
          }

          function checkFiscalTercero(elem) {
              if (document.getElementById("importeRetencion").checked == true) {
                  document.getElementById('InfFiscalTercero').disabled = false;
              } else {
                  document.getElementById('InfFiscalTercero').disabled = true;
              }
          }

          function checkInfoGeneral(elem) {
              if (document.getElementById("IVATraslados").checked == true) {
                  document.getElementById('IvaImpTraslado').disabled = false;
                  document.getElementById('IvaTasTraslado').disabled = false;
              } else {
                  document.getElementById('IvaImpTraslado').disabled = true;
                  document.getElementById('IvaTasTraslado').disabled = true;
              }

              if (document.getElementById("IEPSTraslados").checked == true) {
                  document.getElementById('IEPSImpTraslado').disabled = false;
                  document.getElementById('IEPSTasTraslado').disabled = false;
              } else {
                  document.getElementById('IEPSImpTraslado').disabled = true;
                  document.getElementById('IEPSTasTraslado').disabled = true;
              }

              if (document.getElementById("IVARetenciones").checked == true) {
                  document.getElementById('IvaImpRetencion').disabled = false;
              } else {
                  document.getElementById('IvaImpRetencion').disabled = true;
              }

              if (document.getElementById("IEPSRetenciones").checked == true) {
                  document.getElementById('IEPSImpRetencion').disabled = false;
              } else {
                  document.getElementById('IEPSImpRetencion').disabled = true;
              }
          }

          function pruebaAduanera() {
              //document.getElementById('InfoAduaReciv').style.display = "block";
              // document.getElementById("pruebaInfo").innerHTML = "";*/
              $("#pruebaInfo").html("<%=Resources.resCorpusCFDIEs.btnAgregar%>");
          }
          

        function fnCerrar(div) {

            var IdDiv = div.id;
            var DivContent = div.innerHTML;
            //                var DivContent = $('#' + div.id).html();

            //                var bDiv = $(document.getElementById(div.id));
            //                var bDivClonado = bDiv.clone(true);
            //                var DivContent = bDivClonado.toString();

            //                var DivContent = ('.' + IdDiv).clone(true).html();

            alert(DivContent);

            $.ajax({
                type: "POST",
                data: JSON.stringify({ psComplemento: DivContent }),
                url: "webComplementos.aspx/fnEstablecerComplementoSesion",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    alert('Guardado correctamente')

                },
                error: function (requeset, status, error) {
                    alert('Error al guardar: ' + error);
                }
            });

        }

        function fnBuscarElementos(divComplemento) {

            var sDiv = document.getElementById(divComplemento.id)

            var cElementos = sDiv.getElementsByTagName("*");

            var sValidacion = fnRevisarValidacion(cElementos);
//            var form = $("#frmComplementos");

            if (sValidacion != null & sValidacion != undefined & sValidacion != "") {
                return;
            }
            else {
                alert('Valido');
            }

            var cElementos = document.body.getElementsByTagName("*");

            if (cElementos == null) {

                return;
            }

            var sXMLLayout = "";
            var sNombreNodo = "";

            for (var i = 0; i < cElementos.length; i++) {

                var sTipoElemento = cElementos[i].tagName.toLowerCase();
                var sNombreElemento = cElementos[i].id;

                var sNodo = "";

                if (sNombreElemento != null & sNombreElemento != "" & sNombreElemento != undefined) {

                    if (sTipoElemento == "div") {

                        var sComplemento = $("#" + sNombreElemento).attr("complemento");
                        var sNodo = $("#" + sNombreElemento).attr("nodo");

                        if (sComplemento != null & sComplemento != undefined & sComplemento != "") {

                            if (sXMLLayout != null & sXMLLayout != undefined & sXMLLayout != "") {
                                sXMLLayout = sXMLLayout + "/>\n<comp tipo=\"" + sComplemento + "\"";
                            }
                            else {
                                sXMLLayout = sXMLLayout + "<comp tipo=\"" + sComplemento + "\" />\n";
                            }


                        }
                        else if (sNodo != null & sNodo != undefined & sNodo != "") {

                            if (sNombreNodo != "") {

                                sXMLLayout = sXMLLayout + " />\n"
                                sXMLLayout = sXMLLayout + "<" + sNodo;
                            }
                            else {

                                sXMLLayout = sXMLLayout + "<" + sNodo;

                            }

                            sNombreNodo = sNodo;
                        }
                        else {

                        }
                    }
                    else if (sTipoElemento == "input") {

                        var sAtributo = $("#" + sNombreElemento).attr("atributo");

                        if (sAtributo != null & sAtributo != undefined & sAtributo != "") {

                            var sValor = cElementos[i].value;

                            if (sValor != null & sValor != undefined & sValor != "") {

                                sXMLLayout = sXMLLayout + " " + sAtributo + "=\"" + sValor + "\"";

                            }
                        }
                    }
                    else if (sTipoElemento == "textarea") {

                        var sAtributo = $("#" + sNombreElemento).attr("atributo");

                        if (sAtributo != null & sAtributo != undefined & sAtributo != "") {

                            var sValor = cElementos[i].value;

                            if (sValor != null & sValor != undefined & sValor != "") {

                                sXMLLayout = sXMLLayout + " " + sAtributo + "=\"" + sValor + "\"";

                            }
                        }
                    }
                    else if (sTipoElemento == "select") {

                        var sAtributo = $("#" + sNombreElemento).attr("atributo");

                        if (sAtributo != null & sAtributo != undefined & sAtributo != "") {

                            var sValor = cElementos[i].value;

                            if (sValor != null & sValor != undefined & sValor != "") {

                                sXMLLayout = sXMLLayout + " " + sAtributo + "=\"" + sValor + "\"";

                            }
                        }
                    }
                    else {

                    }
                }
            }

            if (sXMLLayout != null & sXMLLayout != undefined & sXMLLayout != "") {

                sXMLLayout = "<Datos>\n" + sXMLLayout + " />\n</Datos>";
            }

            if (sXMLLayout != null & sXMLLayout != undefined & sXMLLayout != "") {

                alert(sXMLLayout);
            }
            else {
                alert("Complemento no generado");
            }
        }




        function fnDivClone(sNombreObjeto, sNombrePanel, sNombrePanelPrincipal) {

            var sIdBoton = sNombreObjeto.id;
            var bBoton = $(document.getElementById(sIdBoton));
            //var bDivClonar = sNombrePanel.id;
            //var bDivPadre = bBoton.parent().parent().parent().parent();
            var bDivPadre = bBoton.parent().parent().parent().parent().parent().parent().parent().parent();

            var sIdPanelPadre = bDivPadre[0].id;
//            var sIdPanelPadre = sNombrePanelPrincipal.id;
            var sIdHijo = sNombrePanel.id;

            var sNombre = '.' + sIdHijo;
            var nCloneIndex = $(sNombre).length;

            bBoton.parents(sNombre).clone(true)
                        .appendTo('#' + sIdPanelPadre)
                        .attr("id", sIdHijo + "_Clone_" + nCloneIndex)
                        .find("*")
                        .each(function () {
                            var id = this.id || "";
                            if (this.id != null && this.id != undefined && this.id != '') {
                                this.id = this.id + '_' + (nCloneIndex);
                            }
                        }).on('display', 'button.botonEstilo', "none");
                        cloneIndex++;
        }

        function fnEliminar(sBoton) {

            var bBoton = $(document.getElementById(sBoton.id));
            //            var bDivClonar = sNombrePanel.id;
            var bDivPadre = bBoton.parent().parent().parent().parent().parent().parent().parent();

            var sNombre = "#" + bDivPadre[0].id;

            $(sNombre).remove();
        }

        var IdDiv = '';

        function fnAgregarComplemento(boton, div, combo) {
            IdDiv = div.id;
            var bBoton = $(document.getElementById(boton.id));
            var IdCombo = combo.id;
            var bCombo = $(document.getElementById(IdCombo));

            var sRutaSeccionado = bCombo[0].value;
            var sValorSeccionado = bCombo[0].options[bCombo[0].selectedIndex].text;



            $.ajax({
                type: "POST",
                data: JSON.stringify({ psRutaComplemento: sRutaSeccionado, psNombreComplemento: sValorSeccionado, pnNumeroComplementos: nNumeroComplemento }),
                url: "webComplementos.aspx/fnAgregarComplemento",
                //                data: "{ psRutaComplemento: '" + sRutaSeccionado + "',psNombreComplemento: '" + sValorSeccionado + "' ,pnNumeroComplementos: '1' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var bDiv = $(document.getElementById(div.id));
                    //                    alert(1);
                    //                    alert(data.d);
                    $("#" + IdDiv).append(data.d);
                    nNumeroComplemento = nNumeroComplemento + 1;
                    fnColapsarContenedor();

                },
                error: function (requeset, status, error) {
                    alert(error);
                }
            });

        }

        function fnAgregarNodo(boton, div, combo, nombre) {
            IdDiv = div.id;
            var bBoton = $(document.getElementById(boton.id));
            var IdCombo = combo.id;
            var bCombo = $(document.getElementById(IdCombo));

            var sRutaSeccionado = bCombo[0].value;
            var sValorSeccionado = nombre;



            $.ajax({
                type: "POST",
                data: JSON.stringify({ psRutaComplemento: sRutaSeccionado, psNombreComplemento: sValorSeccionado, pnNumeroComplementos: nNumeroComplemento }),
                url: "webComplementos.aspx/fnAgregarComplemento",
                //                data: "{ psRutaComplemento: '" + sRutaSeccionado + "',psNombreComplemento: '" + sValorSeccionado + "' ,pnNumeroComplementos: '1' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var bDiv = $(document.getElementById(div.id));
                    //                    alert(1);
                    //                    alert(data.d);
                    $("#" + IdDiv).append(data.d);
                    nNumeroComplemento = nNumeroComplemento + 1;
                    
                },
                error: function (requeset, status, error) {
                    alert(error);
                }
            });

        }

    </script>
    
    <select size="1" id="ddlComplementos" runat="server" name="ddlComplementos" 
        clientidmode="Static">
        <option value="D:\Forms Dinamicos\AcreditamientoIEPS10.xsd">aieps</option>
        <option value="D:\Forms Dinamicos\aerolineas.xsd">aerolineas</option>
        <option value="D:\Forms Dinamicos\certificadodedestruccion.xsd">destruccion</option>
        <option value="D:\Forms Dinamicos\cfdiregistrofiscal.xsd">CFDIRegistroFiscal</option>
        <option value="D:\Forms Dinamicos\ComercioExterior11.xsd">catComExt</option>
        <option value="D:\Forms Dinamicos\Divisas.xsd">Divisas</option>
        <option value="D:\Forms Dinamicos\leyendasFisc.xsd">LeyendasFiscales</option>
        <option value="D:\Forms Dinamicos\donat11.xsd">Donatarias</option>
        <option value="D:\Forms Dinamicos\ine11.xsd">ine</option>
        <option value="D:\Forms Dinamicos\terceros11.xsd">Terceros</option>
        <option value="D:\Forms Dinamicos\iedu.xsd">IEDU</option>
        <option value="D:\Forms Dinamicos\ecc11.xsd">ECC11</option>
        <option value="D:\Forms Dinamicos\implocal.xsd">ImpLocal</option>
        <option value="D:\Forms Dinamicos\pfic.xsd">PFIC</option>
        <option value="D:\Forms Dinamicos\TuristaPasajeroExtranjero.xsd">TuristaPasajeroExtranjero</option>
        <option value="D:\Forms Dinamicos\spei.xsd">SPEI</option>
        <option value="D:\Forms Dinamicos\detallista.xsd">Detallista</option>
        <option value="D:\Forms Dinamicos\notariospublicos.xsd">notariospublicos</option>
        <option value="D:\Forms Dinamicos\nomina11.xsd">Nomina</option>
        <option value="D:\Forms Dinamicos\nomina12.xsd">Nomina12</option>
        <option value="D:\Forms Dinamicos\Pagos10.xsd">Pagos10</option>
        <option value="D:\Forms Dinamicos\ventavehiculos11.xsd">ventavehiculos</option>
        <option value="D:\Forms Dinamicos\cfdv32.xsd">cfdi</option>
        <option value="D:\Forms Dinamicos\valesdedespensa.xsd">ValesDeDespensa</option>
        <option value="D:\Forms Dinamicos\obrasarteantiguedades.xsd">ObrasDeArteAntiguedades</option>
        <option value="D:\Forms Dinamicos\pagoenespecie.xsd">PagoEnEspecie</option>
        <option value="D:\Forms Dinamicos\servicioparcialconstruccion.xsd">ServicioParcialConstruccion</option>
        <option value="D:\Forms Dinamicos\vehiculousado.xsd">VehiculoUsado</option>
        <option value="D:\Forms Dinamicos\consumodecombustibles.xsd">consumodecombustibles</option>
        <option value="D:\Forms Dinamicos\renovacionysustitucionvehiculos.xsd">renysustvehiculos</option>
        <%--<option value="D:\Forms Dinamicos\vehiculousado.xsd">Imperio</option>--%>
    </select>

    
    <asp:Button ID="btnCargarComplemento" runat="server" CssClass="botonEstilo" Visible="False"
        Text="<%$ Resources:resCorpusCFDIEs, btnAgregar %>" 
        onclick="btnCargarComplemento_Click" />
        
    <div id="divContenedor">
             
        <asp:UpdatePanel ID="updComplementos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <button id="btnAgregar" onclick="fnAgregarComplemento(this, divComplemento, ddlComplementos);" class="botonEstilo"><span><%=Resources.resCorpusCFDIEs.btnAgregar%></span></button> 
                <div id="divComplemento" clientidmode="Static" runat="server">
                
                </div>
                <div id="divBotones" style="width: 700px">
                    <button id="btnGuardar" type="button" onclick="fnBuscarElementos(divComplemento);" class="botonEstilo"><span><%=Resources.resCorpusCFDIEs.btnGuardar%></span></button>
                    <button id="btnValidar" type="submit" onclick="fnRevisarValidacion(divComplemento);" class="botonEstilo"><span>Validar</span></button>
                    <button id="btnCerrar" type="submit" onclick="fnCerrar(divComplemento);" class="botonGrande" style="height:30px; width:150px"><span><%=Resources.resCorpusCFDIEs.btnCerraAddenda%></span></button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>     
    </div>

</asp:Content>


