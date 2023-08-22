<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webComplementos.aspx.cs" Inherits="webComplementos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title></title>

        <link href="~/Styles/webGlobalStyle.css" rel="stylesheet" type="text/css" />
        <link href="~/Styles/menu_style.css" rel="stylesheet" type="text/css" />
        <link href="~/Styles/Dynamic_Style.css" rel="stylesheet" type="text/css" />
        <link href="~/Styles/cssGlobalModales.css" rel="stylesheet" type="text/css" />

    </head>
    <body style="background-color: #FFFFFF">
        <style type="text/css">
            .BotonEstiloAgregar
            {
                background-image: url('../mnu_images/boton_entrar.png');
                background-repeat: no-repeat;
                background-position: center center;
                background-color: #FFFFFF;
                border-color: #FFFFFF;
                border-style: none;
                height: 32px;
                width: 120px;
                color: #FFFFFF;
                font-family: 'Century Gothic';
                font-size: 11px;
            }
            .BotonEstiloEliminar
            {
                background-image: url('../mnu_images/botonGris.png');
                background-repeat: no-repeat;
                background-position: center center;
                background-color: #FFFFFF;
                border-color: #FFFFFF;
                border-style: none;
                height: 32px;
                width: 120px;
                color: #FFFFFF;
                font-family: 'Century Gothic';
                font-size: 11px;
            }
            .style2
            {
                height: 21px;
            }
        </style>
        <script src='<%= ResolveUrl("~/Scripts/jquery-2.2.4.js") %>' type="text/javascript"></script>

        <script type="text/javascript" language="javascript">

            var regex = /^(.+?)(\d+)$/i;
            var sXMLLayout = "";
            var cloneIndex = $(".clonedInput").length;

            function CallMeAfterValidation() {
                // here is the place to run your code
                $("#frmComplementos").bind("invalid-form.validate", function () {

                });

                return true;
            }

            

            function mostrarMensaje(objeto) {
                alert(objeto.id)
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
                        alert('Error al guardar: '  + error);
                    }
                });
            
            }

            function fnBuscarElementos() {

                var form = $("#frmComplementos");
//                form.validate();
                if (!form[0].checkValidity()) {
                    return ;
                }
                else {
                    alert('Valido')
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




            function fnDivClone(sNombreObjeto, sNumeroComplemento, sNombrePanel, sNombrePanelPrincipal) {

                var sIdBoton = sNombreObjeto.id;
                var bBoton = $(document.getElementById(sIdBoton));
                //            var bDivClonar = sNombrePanel.id;
                var bDivPadre = bBoton.parent().parent().parent().parent();

                var sIdPanelPadre = bDivPadre[0].id;
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
                        }).on('click', 'button.style', "display:block");
                cloneIndex++;
            }

            function fnEliminar(sNombreObjeto) {
                var sIdBoton = sNombreObjeto.id;
                var bBoton = $(document.getElementById(sIdBoton));
                var bDiv = bBoton.parent().parent().parent();
                var sNombre = '.' + bDiv[0].id;
                var nCloneIndex = $(sNombre).length;

                bDiv.remove();

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
                    data: JSON.stringify({psRutaComplemento: sRutaSeccionado, psNombreComplemento: sValorSeccionado, pnNumeroComplementos: '1' }),
                    url: "webComplementos.aspx/fnAgregarComplemento",
    //                data: "{ psRutaComplemento: '" + sRutaSeccionado + "',psNombreComplemento: '" + sValorSeccionado + "' ,pnNumeroComplementos: '1' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var bDiv = $(document.getElementById(div.id));
                        //                    alert(1);
                        //                    alert(data.d);
                        $("#" + IdDiv).append(data.d);

                    },
                    error: function (requeset, status, error) {
                        alert(error);
                    }
                });

            }

        </script>

        <select size="1" id="ddlComplementos" name="ddlComplementos">
            <option value="D:\Forms Dinamicos\AcreditamientoIEPS10.xsd">aieps</option>
            <option value="D:\Forms Dinamicos\aerolineas.xsd">aerolineas</option>
            <option value="D:\Forms Dinamicos\certificadodedestruccion.xsd">destruccion</option>
            <option value="D:\Forms Dinamicos\cfdiregistrofiscal.xsd">CFDIRegistroFiscal</option>
            <option value="D:\Forms Dinamicos\ComercioExterior11.xsd">catComExt</option>
            <option value="D:\Forms Dinamicos\Divisas.xsd">Divisas</option>
            <option value="D:\Forms Dinamicos\leyendasFisc.xsd">LeyendasFiscales</option>
            <option value="D:\Forms Dinamicos\donat.xsd">Donatarias</option>
            <option value="D:\Forms Dinamicos\ine11.xsd">INE</option>
            <option value="D:\Forms Dinamicos\terceros11.xsd">Terceros</option>
            <option value="D:\Forms Dinamicos\iedu.xsd">IEDU</option>
            <option value="D:\Forms Dinamicos\ecc.xsd">ECC</option>
            <option value="D:\Forms Dinamicos\implocal.xsd">ImpLocal</option>
            <option value="D:\Forms Dinamicos\pfic.xsd">PFIC</option>
            <option value="D:\Forms Dinamicos\TuristaPasajeroExtranjero.xsd">TuristaPasajeroExtranjero</option>
            <option value="D:\Forms Dinamicos\spei.xsd">SPEI</option>
            <option value="D:\Forms Dinamicos\detallista.xsd">Detallista</option>
            <option value="D:\Forms Dinamicos\nomina11.xsd">Nomina</option>
            <option value="D:\Forms Dinamicos\nomina12.xsd">Nomina12</option>
            <option value="D:\Forms Dinamicos\Pagos10.xsd">Pagos10</option>
            <option value="D:\Forms Dinamicos\ventavehiculos.xsd">ventavehiculos</option>
            <option value="D:\Forms Dinamicos\cfdv32.xsd">cfdi</option>
        </select>
        <button id="btnAgregar" onclick="fnAgregarComplemento(this, divComplemento, ddlComplementos);" class="botonEstilo"><span><%=Resources.resCorpusCFDIEs.btnAgregar%></span></button> 
        
        <div id="divContenedor">
            <form id="frmComplementos">
            
                <div id="divComplemento" runat="server" style="width: 700px">
                </div>
                <div id="divBotones" style="width: 700px">
                    <button id="btnGuardar" type="submit" onclick="fnBuscarElementos();" class="botonEstilo"><span><%=Resources.resCorpusCFDIEs.btnGuardar%></span></button>
                    <button id="btnCerrar" type="submit" onclick="fnCerrar(divComplemento);" class="botonGrande" style="height:30px; width:150px"><span><%=Resources.resCorpusCFDIEs.btnCerraAddenda%></span></button>
                </div>    
                
                
                            
            </form>               
        </div>

       

    </body>
</html>
